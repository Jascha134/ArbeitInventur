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
        private ContextMenuStrip _contextMenuStrip;

        public Main(Benutzer benutzer)
        {
            if (benutzer == null) throw new ArgumentNullException(nameof(benutzer));
            _benutzer = benutzer;

            // Pfade validieren und ggf. Setup-Fenster anzeigen
            if (!ArePathsValid())
            {
            }

            // Validierung des DataJSON-Pfads
            if (!Directory.Exists(Properties.Settings.Default.DataJSON))
            {
                Directory.CreateDirectory(Properties.Settings.Default.DataJSON);
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
            InitializeContextMenu();
            UpdateBarcodeIndex();

            CheckAndTransferPendingFolders(
                Properties.Settings.Default.ServerTargetFolder,
                Properties.Settings.Default.ExocadConstructions);
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
            btn_RefreshHistory.Click += btn_RefreshHistory_Click;
        }

        private void InitializeContextMenu()
        {
            _contextMenuStrip = new ContextMenuStrip();
            _contextMenuStrip.Items.Add("Bearbeiten", null, ContextMenuEdit_Click);
            _contextMenuStrip.Items.Add("Löschen", null, ContextMenuDelete_Click);
            dataGridView2.ContextMenuStrip = _contextMenuStrip;
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
                    using (var addForm = new AddProductForm(barcodeId, produktId, produktionsdatum, lotNummer, _implantatsysteme, _produktManager, _logHandler, "Neues Produkt erstellen",true))
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
            if (dataGridView2.Columns.Contains("Barcode"))
            {
                dataGridView2.Columns["Barcode"].Visible = false;
                dataGridView2.Columns["ProduktId"].Visible = false;
                dataGridView2.Columns["Produktionsdatum"].Visible = false;
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
                ProcessBarcode(barcode);
                txtBarcodeInput.Text = "";
                e.Handled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucSettings = new Uc_Settings { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucSettings);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            panelMain.Controls.AddRange(_originalControls);
        }

        private void btn_Chat_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucChat = new UC_Chatcs(_benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucChat);
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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucHistory = new UC_History(_benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucHistory);
        }

        private void btn_Exocad_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucExocad = new UC_Exocad(_logHandler) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucExocad);
        }

        private async void button1_Click_1(object sender, EventArgs e)
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

            using (var addForm = new AddProductForm(null, null, null, null, _implantatsysteme, _produktManager, _logHandler, "Neues Produkt erstellen",true))
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

        private void btn_RefreshHistory_Click(object sender, EventArgs e)
        {
            var ucHistory = panelMain.Controls.OfType<UC_History>().FirstOrDefault();
            if (ucHistory != null)
            {
                ucHistory.RefreshHistory();
            }
        }

        private async void ContextMenuEdit_Click(object sender, EventArgs e)
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

            // Produkt direkt über DataBoundItem auswählen, um Eindeutigkeit zu gewährleisten
            var productToEdit = (ProduktDetail)dataGridView2.CurrentRow.DataBoundItem;
            if (productToEdit == null)
            {
                MessageBox.Show("Das ausgewählte Produkt konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var addForm = new AddProductForm(productToEdit.Barcode, productToEdit.ProduktId, productToEdit.Produktionsdatum, productToEdit.LotNummer, _implantatsysteme, _produktManager, _logHandler, "Produkt Bearbeiten", false))
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateBarcodeIndex();
                    UpdateDataGridViews(addForm.SelectedSystem, addForm.AddedProduct);
                    await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
                    LogMessage($"Produkt bearbeitet: {addForm.AddedProduct.Beschreibung}");
                }
            }
        }

        private async void ContextMenuDelete_Click(object sender, EventArgs e)
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
            var selectedProduct = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == beschreibung);

            if (selectedProduct == null)
            {
                MessageBox.Show("Das ausgewählte Produkt konnte nicht gefunden werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Möchten Sie das Produkt '{selectedProduct.Beschreibung}' wirklich löschen?", "Bestätigung", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                selectedSystem.Details.Remove(selectedProduct);
                UpdateBarcodeIndex();
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = selectedSystem.Details?.ToList() ?? new List<ProduktDetail>();
                await _produktManager.SpeichereImplantatsystemeAsync(_implantatsysteme);
                LogMessage($"Produkt gelöscht: {selectedProduct.Beschreibung}");
                MessageBox.Show("Produkt erfolgreich gelöscht.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            _contextMenuStrip?.Dispose();
            _disposed = true;
            base.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            Application.Exit();
        }

        #endregion
    }
}