using ArbeitInventur.Barcode;
using ArbeitInventur.Exocad_Help;
using ArbeitInventur.Formes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class Main : Form, IDisposable
    {
        private readonly string _implantatsystemePfad;
        private readonly string _logPfad;
        private readonly ProduktManager _produktManager;
        private readonly List<ProduktFirma> _implantatsysteme;
        private readonly Benutzer _benutzer;
        private readonly LogHandler _logHandler;
        private readonly DentalCadFileWatcher _fileWatcher;
        private readonly FolderWatcherAndUploader _serverWatcher;
        private readonly NotifyIcon _notifyIcon;
        private readonly Dictionary<string, (ProduktFirma System, ProduktDetail Product)> _barcodeIndex;
        private Control[] _originalControls;
        private bool _disposed;

        public Main(Benutzer benutzer)
        {
            if (benutzer == null) throw new ArgumentNullException(nameof(benutzer));
            _benutzer = benutzer;

            // Pfade validieren und ggf. Setup-Fenster anzeigen
            if (!ArePathsValid())
            {
                using (var pathSetupForm = new PathSetupForm())
                {
                    if (pathSetupForm.ShowDialog() != DialogResult.OK)
                    {
                        throw new Exception("Pfad-Konfiguration abgebrochen. Programm wird beendet.");
                    }
                }
            }

            InitializeComponent();
            _produktManager = new ProduktManager();
            _implantatsysteme = new List<ProduktFirma>();
            _barcodeIndex = new Dictionary<string, (ProduktFirma, ProduktDetail)>();

            _implantatsystemePfad = Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
            _logPfad = Path.Combine(Properties.Settings.Default.DataJSON, "log.json");
            _logHandler = new LogHandler(_logPfad, _benutzer);

            _fileWatcher = new DentalCadFileWatcher(_logHandler, TimeSpan.FromSeconds(30), 10);
            _serverWatcher = new FolderWatcherAndUploader(
                Properties.Settings.Default.ServerTargetFolder,
                Properties.Settings.Default.ExocadConstructions,
                _logHandler);
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true,
                Text = "ArbeitInventur"
            };

            InitializeFileWatchers();
            InitializeUI();
            UpdateBarcodeIndex();

            CheckAndTransferPendingFolders(
                Properties.Settings.Default.ServerTargetFolder,
                Properties.Settings.Default.ExocadConstructions);

            LogMessage("Überwachung in Main gestartet.");
        }

        #region Initialisierung

        private void InitializeFileWatchers()
        {
            _fileWatcher.FileHandled += (message) => _logHandler.LogAction(message);
            _fileWatcher.StartWatching();

            _serverWatcher.FolderUploaded += OnServerFolderDetected;
            _serverWatcher.StartWatching();
        }

        private void InitializeUI()
        {
            _originalControls = panelMain.Controls.Cast<Control>().ToArray();
            MinimumSize = new Size(1208, 759);
            ConfigureDataGridViews();
            if (Login.Instance != null) Login.Instance.Hide();
            lb_Benutzer.Text = $"Benutzer: {_benutzer.Name}";
        }

        private void ConfigureDataGridViews()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVStyle.Dgv(dataGridView1);
            DGVStyle.Dgv(dataGridView2);
        }

        private bool ArePathsValid()
        {
            return !string.IsNullOrWhiteSpace(Properties.Settings.Default.DataJSON) &&
                   !string.IsNullOrWhiteSpace(Properties.Settings.Default.ExocadConstructions) &&
                   !string.IsNullOrWhiteSpace(Properties.Settings.Default.ExocaddentalCAD) &&
                   !string.IsNullOrWhiteSpace(Properties.Settings.Default.LocalScanFolder) &&
                   !string.IsNullOrWhiteSpace(Properties.Settings.Default.ServerTargetFolder) &&
                   Directory.Exists(Properties.Settings.Default.DataJSON) &&
                   Directory.Exists(Properties.Settings.Default.ExocadConstructions) &&
                   Directory.Exists(Properties.Settings.Default.ExocaddentalCAD) &&
                   Directory.Exists(Properties.Settings.Default.LocalScanFolder) &&
                   Directory.Exists(Properties.Settings.Default.ServerTargetFolder);
        }

        #endregion

        #region QR-Code-Verarbeitung

        private bool IsValidBarcode(string barcode)
        {
            if (string.IsNullOrEmpty(barcode) || barcode == "Barcode scannen...")
                return false;

            // Unterstützung für verschiedene QR-Code-Formate
            // 1. GS1-Format (z. B. MEDENTIKA: 01<14 Ziffern>11<6 Ziffern>10<Lot>)
            var gs1Match = Regex.Match(barcode, @"^01(\d{14})11(\d{6})10([\w\s]{1,20})$");
            if (gs1Match.Success)
            {
                string produktId = gs1Match.Groups[1].Value;
                string datum = gs1Match.Groups[2].Value;
                string lot = gs1Match.Groups[3].Value;

                if (!Regex.IsMatch(produktId, @"^\d{14}$") ||
                    !DateTime.TryParseExact(datum, "ddMMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _) ||
                    string.IsNullOrEmpty(lot))
                {
                    _logHandler.LogAction($"Ungültiges GS1-Format: Produkt-ID={produktId}, Datum={datum}, Lot={lot}");
                    return false;
                }
                return true;
            }

            // 2. MaschieneWerkzeug (z. B. +E0007606629/$$8017/SZ)
            if (barcode.StartsWith("+E"))
            {
                var parts = barcode.Split('/');
                return parts.Length >= 1 && !string.IsNullOrEmpty(parts[0]);
            }

            // 3. Sinterperlen (z. B. +611787559/$$8017BLF824/SE000N)
            if (barcode.StartsWith("+6"))
            {
                var parts = barcode.Split('/');
                return parts.Length >= 1 && !string.IsNullOrEmpty(parts[0]);
            }

            // 4. Dentsply Sirona (z. B. +D00181030161/$$70000051362/16D20241101/14D20341101E)
            if (barcode.StartsWith("+D"))
            {
                var parts = barcode.Split('/');
                return parts.Length >= 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]);
            }

            _logHandler.LogAction($"Unbekanntes QR-Code-Format: {barcode}");
            return false;
        }

        private (string Barcode, string ProduktId, DateTime? Produktionsdatum, string LotNummer) ParseQRCode(string barcode)
        {
            try
            {
                // 1. GS1-Format
                var gs1Match = Regex.Match(barcode, @"^01(\d{14})11(\d{6})10([\w\s]{1,20})$");
                if (gs1Match.Success)
                {
                    string produktId = gs1Match.Groups[1].Value;
                    string datumStr = gs1Match.Groups[2].Value;
                    string lot = gs1Match.Groups[3].Value;

                    DateTime? produktionsdatum = null;
                    if (DateTime.TryParseExact(datumStr, "ddMMyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var datum))
                    {
                        produktionsdatum = datum;
                    }
                    return (barcode, produktId, produktionsdatum, lot);
                }

                // 2. MaschieneWerkzeug (z. B. +E0007606629/$$8017/SZ)
                if (barcode.StartsWith("+E"))
                {
                    var parts = barcode.Split('/');
                    string barcodeId = parts[0];
                    string lotNummer = parts.Length > 2 ? parts[2] : "";
                    return (barcodeId, barcodeId, null, lotNummer);
                }

                // 3. Sinterperlen (z. B. +611787559/$$8017BLF824/SE000N)
                if (barcode.StartsWith("+6"))
                {
                    var parts = barcode.Split('/');
                    string barcodeId = parts[0];
                    string lotNummer = parts.Length > 2 ? parts[2] : "";
                    return (barcodeId, barcodeId, null, lotNummer);
                }

                // 4. Dentsply Sirona (z. B. +D00181030161/$$70000051362/16D20241101/14D20341101E)
                if (barcode.StartsWith("+D"))
                {
                    var parts = barcode.Split('/');
                    string barcodeId = parts[0];
                    string produktId = parts.Length > 1 ? parts[1].Replace("$$", "") : barcodeId;
                    string lotNummer = parts.Length > 2 ? parts[2] : "";
                    DateTime? produktionsdatum = null;
                    if (lotNummer.Length >= 10 && lotNummer.StartsWith("16D"))
                    {
                        string dateStr = lotNummer.Substring(3, 8); // z. B. 20241101
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out var date))
                        {
                            produktionsdatum = date;
                        }
                    }
                    return (barcodeId, produktId, produktionsdatum, lotNummer);
                }

                // Fallback für unbekannte Formate
                _logHandler.LogAction($"Unbekanntes QR-Code-Format: {barcode}");
                return (barcode, barcode, null, "");
            }
            catch (Exception ex)
            {
                _logHandler.LogAction($"Fehler beim Parsen des QR-Codes {barcode}: {ex.Message}");
                return (barcode, barcode, null, "");
            }
        }

        private void ProcessBarcode(string barcode)
        {
            try
            {
                _logHandler.LogAction($"QR-Code-Scan: {barcode}");
                var (barcodeId, produktId, produktionsdatum, lotNummer) = ParseQRCode(barcode);

                if (string.IsNullOrEmpty(produktId))
                {
                    MessageBox.Show("Konnte QR-Code-Daten nicht parsen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prüfen, ob das Produkt bereits registriert ist
                var existingProduct = _implantatsysteme
                    .SelectMany(s => s.Details)
                    .FirstOrDefault(d => d.Barcode == barcodeId && d.LotNummer == lotNummer);

                ProduktFirma selectedSystem = null;
                ProduktDetail matchingProduct = null;

                if (existingProduct != null)
                {
                    selectedSystem = _implantatsysteme.FirstOrDefault(s => s.Details.Contains(existingProduct));
                    matchingProduct = existingProduct;
                }

                if (matchingProduct != null)
                {
                    // Produkt existiert, öffne InventoryActionForm
                    UpdateDataGridViews(selectedSystem, matchingProduct);
                    ShowInventoryActionForm(selectedSystem, matchingProduct);
                }
                else
                {
                    // Produkt existiert nicht, öffne AddProductForm
                    using (var addForm = new AddProductForm(barcodeId, produktId, produktionsdatum, lotNummer, _implantatsysteme, _produktManager, _logHandler))
                    {
                        if (addForm.ShowDialog() == DialogResult.OK)
                        {
                            UpdateBarcodeIndex();
                            UpdateDataGridViews(addForm.SelectedSystem, addForm.AddedProduct);
                            MessageBox.Show("Produkt erfolgreich hinzugefügt.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _logHandler.LogAction($"Produkt hinzugefügt: {addForm.AddedProduct.Beschreibung}, Barcode: {barcodeId}, Lot: {lotNummer}");
                        }
                        else
                        {
                            _logHandler.LogAction($"Hinzufügen von QR-Code {barcodeId} abgebrochen.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logHandler.LogAction($"Fehler beim Verarbeiten von QR-Code {barcode}: {ex.Message}");
                MessageBox.Show("Ein Fehler ist aufgetreten. Bitte versuchen Sie es erneut.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBarcodeIndex()
        {
            _barcodeIndex.Clear();
            foreach (var system in _implantatsysteme)
            {
                if (system.Details == null) continue;
                foreach (var product in system.Details.Where(d => !string.IsNullOrEmpty(d.Barcode)))
                {
                    _barcodeIndex[product.Barcode] = (system, product);
                }
            }
        }

        #endregion

        #region UI-Event-Handler

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LadeImplantatsystemeAsync();
        }

        private async Task LadeImplantatsystemeAsync()
        {
            // Manipuliere die bestehende Liste, anstatt sie neu zuzuweisen
            _implantatsysteme.Clear();
            var loadedSystems = await _produktManager.LadeImplantatsystemeAsync();
            _implantatsysteme.AddRange(loadedSystems);
            UpdateBarcodeIndex();
            dataGridView1.DataSource = _implantatsysteme.Select(s => new { s.SystemName }).ToList();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);

            if (selectedSystem != null)
            {
                dataGridView2.SuspendLayout();
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = selectedSystem.Details?.ToList() ?? new List<ProduktDetail>();
                dataGridView2.ResumeLayout();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            textBoxSystemName.Text = dataGridView1.Rows[e.RowIndex].Cells["SystemName"].Value.ToString();
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView2.Rows[e.RowIndex];
            textBoxKategorie.Text = row.Cells["Kategorie"].Value?.ToString() ?? "";
            textBoxBeschreibung.Text = row.Cells["Beschreibung"].Value?.ToString() ?? "";
            textBoxMenge.Text = row.Cells["Menge"].Value?.ToString() ?? "0";
            textBoxMindestbestand.Text = row.Cells["Mindestbestand"].Value?.ToString() ?? "0";
        }

        private void DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Menge" && e.RowIndex >= 0)
            {
                var row = dataGridView2.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["Menge"].Value?.ToString(), out int menge) &&
                    int.TryParse(row.Cells["Mindestbestand"].Value?.ToString(), out int mindestbestand))
                {
                    e.CellStyle.BackColor = menge < mindestbestand ? Color.Red : dataGridView2.DefaultCellStyle.BackColor;
                    e.CellStyle.ForeColor = menge < mindestbestand ? Color.White : dataGridView2.DefaultCellStyle.ForeColor;
                }
            }
        }

        private void txtBarcodeInput_Enter(object sender, EventArgs e)
        {
            txtBarcodeInput.Text = "";
        }

        private void txtBarcodeInput_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBarcodeInput.Text))
                txtBarcodeInput.Text = "Barcode scannen...";
        }

        private void TxtBarcodeInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string barcode = txtBarcodeInput.Text.Trim();
                if (IsValidBarcode(barcode))
                {
                    ProcessBarcode(barcode);
                    txtBarcodeInput.Text = "";
                }
                else
                {
                    MessageBox.Show("Ungültiger QR-Code. Bitte erneut scannen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _logHandler.LogAction($"Ungültiger QR-Code gescannt: {barcode}");
                }
                e.Handled = true;
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            textBoxKategorie.Clear();
            textBoxBeschreibung.Clear();
            textBoxMenge.Clear();
            textBoxMindestbestand.Clear();
            LogMessage("Felder geleert.");
        }

        private void btn_SystemNameNew_Click(object sender, EventArgs e)
        {
            textBoxSystemName.Clear();
            LogMessage("Systemname geleert.");
        }

        private void btn_Minus_Click(object sender, EventArgs e) => AdjustNumericTextBox(textBoxMenge, -1);
        private void btn_Plus_Click(object sender, EventArgs e) => AdjustNumericTextBox(textBoxMenge, 1);
        private void btn_MinusMindes_Click(object sender, EventArgs e) => AdjustNumericTextBox(textBoxMindestbestand, -1);
        private void btn_PlusMindest_Click(object sender, EventArgs e) => AdjustNumericTextBox(textBoxMindestbestand, 1);

        private void AdjustNumericTextBox(TextBox textBox, int change)
        {
            if (string.IsNullOrEmpty(textBox.Text)) textBox.Text = "0";
            if (int.TryParse(textBox.Text, out int value))
            {
                value = Math.Max(0, value + change);
                textBox.Text = value.ToString();
            }
            LogMessage($"TextBox {textBox.Name} angepasst: {textBox.Text}");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucSettings = new Uc_Settings { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucSettings);
            LogMessage("Einstellungen geöffnet.");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            panelMain.Controls.AddRange(_originalControls);
            LogMessage("Einlagern zurückgesetzt.");
        }

        private void btn_Chat_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucChat = new UC_Chatcs(_benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucChat);
            LogMessage("Pinwand geöffnet.");
        }

        private async void btn_UC_Übersicht_Click(object sender, EventArgs e)
        {
            if (Übersicht.instanze == null || Übersicht.instanze.IsDisposed)
            {
                var loadedSystems = await _produktManager.LadeImplantatsystemeAsync();
                Übersicht.instanze = new Übersicht(loadedSystems);
                Übersicht.instanze.FormClosed += (s, args) => Übersicht.instanze = null;
                Übersicht.instanze.Show();
            }
            else
            {
                Übersicht.instanze.BringToFront();
            }
            LogMessage("Übersicht geöffnet.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucHistory = new UC_History(_benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucHistory);
            LogMessage("History geöffnet.");
        }

        private void btn_Exocad_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucExocad = new UC_Exocad(_logHandler) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucExocad);
            LogMessage("Exocad geöffnet.");
        }

        // Button1: Produkt hinzufügen oder bearbeiten
        private async void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Bitte wählen Sie ein System aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);

            if (selectedSystem == null)
            {
                MessageBox.Show("Das ausgewählte System konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProduktDetail product = null;
            if (dataGridView2.CurrentRow != null)
            {
                string beschreibung = dataGridView2.CurrentRow.Cells["Beschreibung"].Value.ToString();
                product = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == beschreibung);
            }

            // Öffne AddProductForm ohne Barcode-Daten, da dies manuelles Hinzufügen/Bearbeiten ist
            using (var addForm = new AddProductForm(null, null, null, null, _implantatsysteme, _produktManager, _logHandler))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateBarcodeIndex();
                    UpdateDataGridViews(addForm.SelectedSystem, addForm.AddedProduct);
                    await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
                    LogMessage($"Produkt hinzugefügt/bearbeitet: {addForm.AddedProduct.Beschreibung}");
                }
            }
        }

        // Button2: Neues System hinzufügen
        private async void button2_Click(object sender, EventArgs e)
        {
            string newSystemName = textBoxSystemName.Text.Trim();
            if (string.IsNullOrEmpty(newSystemName))
            {
                MessageBox.Show("Bitte geben Sie einen Systemnamen ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_implantatsysteme.Any(s => s.SystemName == newSystemName))
            {
                MessageBox.Show("Ein System mit diesem Namen existiert bereits.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newSystem = new ProduktFirma { SystemName = newSystemName, Details = new List<ProduktDetail>() };
            _implantatsysteme.Add(newSystem);
            await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
            dataGridView1.DataSource = _implantatsysteme.Select(s => new { s.SystemName }).ToList();
            LogMessage($"Neues System hinzugefügt: {newSystemName}");
        }

        // Button3: Produkt löschen
        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView2.CurrentRow == null)
            {
                MessageBox.Show("Bitte wählen Sie ein System und ein Produkt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);

            if (selectedSystem == null)
            {
                MessageBox.Show("Das ausgewählte System konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string beschreibung = dataGridView2.CurrentRow.Cells["Beschreibung"].Value.ToString();
            var product = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == beschreibung);

            if (product == null)
            {
                MessageBox.Show("Das ausgewählte Produkt konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Möchten Sie das Produkt '{beschreibung}' wirklich löschen?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                selectedSystem.Details.Remove(product);
                await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
                UpdateBarcodeIndex();
                dataGridView2.DataSource = selectedSystem.Details?.ToList() ?? new List<ProduktDetail>();
                LogMessage($"Produkt gelöscht: {beschreibung}");
            }
        }

        // ButtonSystemDelete (button5): System löschen
        private async void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Bitte wählen Sie ein System aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);

            if (selectedSystem == null)
            {
                MessageBox.Show("Das ausgewählte System konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Möchten Sie das System '{selectedSystemName}' und alle zugehörigen Produkte wirklich löschen?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _implantatsysteme.Remove(selectedSystem);
                await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
                UpdateBarcodeIndex();
                dataGridView1.DataSource = _implantatsysteme.Select(s => new { s.SystemName }).ToList();
                dataGridView2.DataSource = null;
                LogMessage($"System gelöscht: {selectedSystemName}");
            }
        }

        #endregion

        #region Server- und Ordnerverwaltung

        private void CheckAndTransferPendingFolders(string serverFolder, string localFolder)
        {
            if (!Directory.Exists(serverFolder) || !Directory.Exists(localFolder))
            {
                LogMessage($"Server- oder lokaler Ordner nicht vorhanden: {serverFolder}, {localFolder}");
                return;
            }

            var serverDirs = Directory.GetDirectories(serverFolder);
            var localDirs = Directory.GetDirectories(localFolder).Select(Path.GetFileName).ToHashSet();
            var pendingFolders = serverDirs.Where(d => !localDirs.Contains(Path.GetFileName(d))).ToList();

            if (pendingFolders.Any())
            {
                foreach (var folder in pendingFolders)
                {
                    string targetPath = Path.Combine(localFolder, Path.GetFileName(folder));
                    CopyDirectory(folder, targetPath);
                    LogMessage($"Pending Ordner übertragen: {folder} -> {targetPath}");
                }
                _notifyIcon.ShowBalloonTip(3000, "Neue Aufträge", $"{pendingFolders.Count} neue Aufträge vom Server übertragen!", ToolTipIcon.Info);
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return;

            Directory.CreateDirectory(destDir);
            foreach (var file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }
            foreach (var subDir in dir.GetDirectories())
            {
                string targetSubDirPath = Path.Combine(destDir, subDir.Name);
                CopyDirectory(subDir.FullName, targetSubDirPath);
            }
        }

        private void OnServerFolderDetected(string message)
        {
            Invoke(new Action(() =>
            {
                _notifyIcon.ShowBalloonTip(3000, "Neuer Auftrag", "Neuer Auftrag zum Konstruieren vorliegt!", ToolTipIcon.Info);
                LogMessage(message);
            }));
        }

        #endregion

        #region Hilfsmethoden

        private void LogMessage(string message)
        {
            _logHandler.LogAction($"{DateTime.Now}: {message}");
        }

        private void UpdateDataGridViews(ProduktFirma selectedSystem, ProduktDetail matchingProduct)
        {
            dataGridView1.ClearSelection();
            int rowIndex = dataGridView1.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["SystemName"].Value.ToString() == selectedSystem.SystemName)?.Index ?? -1;
            if (rowIndex >= 0)
            {
                dataGridView1.Rows[rowIndex].Selected = true;
                dataGridView2.DataSource = selectedSystem.Details?.ToList() ?? new List<ProduktDetail>();

                int detailRowIndex = dataGridView2.Rows.Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["Beschreibung"].Value.ToString() == matchingProduct.Beschreibung)?.Index ?? -1;
                if (detailRowIndex >= 0)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[detailRowIndex].Selected = true;
                }
            }
        }

        private void ShowInventoryActionForm(ProduktFirma system, ProduktDetail product)
        {
            using (var actionForm = new InventoryActionForm(system, product, _produktManager, _implantatsysteme, _logHandler))
            {
                if (actionForm.ShowDialog() == DialogResult.OK)
                {
                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = system.Details?.ToList() ?? new List<ProduktDetail>();
                }
            }
        }

        #endregion

        #region Dispose

        public new void Dispose()
        {
            if (_disposed) return;
            _fileWatcher?.StopWatching();
            _fileWatcher?.Dispose();
            _serverWatcher?.StopWatching();
            _serverWatcher?.Dispose();
            _notifyIcon?.Dispose();
            _disposed = true;
            base.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            Application.Exit();
            LogMessage("Formular geschlossen.");
        }

        #endregion
    }
}