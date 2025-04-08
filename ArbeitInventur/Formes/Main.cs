using ArbeitInventur.Exocad_Help;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class Main : Form, IDisposable
    {
        private readonly string implantatsystemePfad;
        private readonly string logPfad;
        private readonly ProduktManager manager = new ProduktManager();
        private readonly List<ProduktFirma> implantatsysteme = new List<ProduktFirma>();
        private readonly Benutzer benutzer;
        private LogHandler logHandler;
        private DentalCadFileWatcher fileWatcher;
        private FolderWatcherAndUploader serverWatcher;
        private Control[] originalControls;
        private NotifyIcon notifyIcon;
        private bool disposed;

        public Main(Benutzer benutzer)
        {
            // Pfade vor der Initialisierung prüfen
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
            if (benutzer == null) throw new ArgumentNullException(nameof(benutzer));
            this.benutzer = benutzer;
            if (Login.Instance != null) Login.Instance.Hide();
            DGVSettings();
            originalControls = panelMain.Controls.Cast<Control>().ToArray();
            MinimumSize = new Size(1208, 759);

            implantatsystemePfad = Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
            logPfad = Path.Combine(Properties.Settings.Default.DataJSON, "log.json");

            logHandler = new LogHandler(logPfad, benutzer);
            fileWatcher = new DentalCadFileWatcher();
            fileWatcher.FileHandled += (message) => logHandler.LogAction(message);
            fileWatcher.StartWatching();

            // Server-Überwachung für PC1
            string serverFolder = Properties.Settings.Default.ServerTargetFolder;
            string localConstructionFolder = Properties.Settings.Default.ExocadConstructions;
            serverWatcher = new FolderWatcherAndUploader(serverFolder, localConstructionFolder);
            serverWatcher.FolderUploaded += OnServerFolderDetected;
            serverWatcher.StartWatching();

            notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Visible = true,
                Text = "ArbeitInventur"
            };

            LogMessage("Überwachung in Main gestartet.");

            CheckAndTransferPendingFolders(serverFolder, localConstructionFolder);
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
                notifyIcon.ShowBalloonTip(3000, "Neue Aufträge", $"{pendingFolders.Count} neue Aufträge vom Server übertragen!", ToolTipIcon.Info);
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

        private void DGVSettings()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVStyle.Dgv(dataGridView1);
            DGVStyle.Dgv(dataGridView2);
        }

        private void LogMessage(string message)
        {
            logHandler.LogAction($"{DateTime.Now}: {message}");
        }

        private void OnServerFolderDetected(string message)
        {
            Invoke(new Action(() =>
            {
                notifyIcon.ShowBalloonTip(3000, "Neuer Auftrag", "Neuer Auftrag zum Konstruieren vorliegt!", ToolTipIcon.Info);
                LogMessage(message);
            }));
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            lb_Benutzer.Text = $"Benutzer: {benutzer.Name}";
            await LadeImplantatsystemeAsync();
        }

        private async Task LadeImplantatsystemeAsync()
        {
            implantatsysteme.Clear();
            var loadedSystems = await manager.LadeImplantatsystemeAsync();
            implantatsysteme.AddRange(loadedSystems);
            dataGridView1.DataSource = implantatsysteme.Select(s => new { s.SystemName }).ToList();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();
            var selectedSystem = implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);

            if (selectedSystem != null)
            {
                dataGridView2.SuspendLayout();
                dataGridView2.DataSource = null;
                dataGridView2.DataSource = selectedSystem.Details.ToList();
                dataGridView2.ResumeLayout();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];
            textBoxSystemName.Text = row.Cells["SystemName"].Value.ToString();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            LogMessage("Button1 geklickt: Hinzufügen/Bearbeiten");
        }

        private void DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView2.Rows[e.RowIndex];
            textBoxKategorie.Text = row.Cells["Kategorie"].Value.ToString();
            textBoxBeschreibung.Text = row.Cells["Beschreibung"].Value.ToString();
            textBoxMenge.Text = row.Cells["Menge"].Value.ToString();
            textBoxMindestbestand.Text = row.Cells["Mindestbestand"].Value.ToString();
        }

        private void DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Menge" && e.RowIndex >= 0)
            {
                var row = dataGridView2.Rows[e.RowIndex];
                int menge = Convert.ToInt32(row.Cells["Menge"].Value);
                int mindestbestand = Convert.ToInt32(row.Cells["Mindestbestand"].Value);
                e.CellStyle.BackColor = menge < mindestbestand ? Color.Red : dataGridView2.DefaultCellStyle.BackColor;
                e.CellStyle.ForeColor = menge < mindestbestand ? Color.White : dataGridView2.DefaultCellStyle.ForeColor;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            LogMessage("Button2 geklickt: System Hinzufügen/Bearbeiten");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            LogMessage("Button3 geklickt: Löschen");
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            LogMessage("Button5 geklickt: System Löschen");
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            textBoxKategorie.Clear();
            textBoxBeschreibung.Clear();
            textBoxMenge.Clear();
            textBoxMindestbestand.Clear();
            LogMessage("Btn_New geklickt: Felder geleert");
        }

        private void btn_SystemNameNew_Click(object sender, EventArgs e)
        {
            textBoxSystemName.Clear();
            LogMessage("Btn_SystemNameNew geklickt: Systemname geleert");
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
            LogMessage("Button6 geklickt: Einstellungen geöffnet");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            panelMain.Controls.AddRange(originalControls);
            LogMessage("Button7 geklickt: Einlagern zurückgesetzt");
        }

        private void btn_Chat_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucChat = new UC_Chatcs(benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucChat);
            LogMessage("Btn_Chat geklickt: Pinwand geöffnet");
        }

        private async void btn_UC_Übersicht_Click(object sender, EventArgs e)
        {
            if (Übersicht.instanze == null || Übersicht.instanze.IsDisposed)
            {
                var loadedSystems = await manager.LadeImplantatsystemeAsync();
                Übersicht.instanze = new Übersicht(loadedSystems);
                Übersicht.instanze.FormClosed += (s, args) => Übersicht.instanze = null;
                Übersicht.instanze.Show();
            }
            else
            {
                Übersicht.instanze.BringToFront();
            }
            LogMessage("Btn_UC_Übersicht geklickt: Übersicht geöffnet");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucHistory = new UC_History(benutzer) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucHistory);
            LogMessage("Button4 geklickt: History geöffnet");
        }

        private void btn_Exocad_Click(object sender, EventArgs e)
        {
            panelMain.Controls.Clear();
            var ucExocad = new UC_Exocad(logHandler) { Dock = DockStyle.Fill };
            panelMain.Controls.Add(ucExocad);
            LogMessage("Btn_Exocad geklickt: Exocad geöffnet");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            Application.Exit();
            LogMessage("Formular geschlossen");
        }

        public new void Dispose()
        {
            if (disposed) return;
            fileWatcher?.StopWatching();
            fileWatcher?.Dispose();
            serverWatcher?.StopWatching();
            serverWatcher?.Dispose();
            notifyIcon?.Dispose();
            disposed = true;
            base.Dispose();
        }
    }
}