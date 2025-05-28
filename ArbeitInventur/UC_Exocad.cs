using ArbeitInventur.Exocad_Help;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ArbeitInventur
{
    public partial class UC_Exocad : UserControl, IDisposable
    {
        private DentalCadFileWatcher _fileWatcher;
        private FolderWatcherAndUploader _folderUploader;
        private LogHandler _logHandler;
        private bool disposed = false;
        private bool _dentalCadWatcherActive = false;
        private bool _folderUploaderActive = false;

        public UC_Exocad(LogHandler logHandler)
        {
            InitializeComponent();
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));

            txt_Exocad_Constructions.Text = Properties.Settings.Default.ExocadConstructions ?? @"C:\Exocad\Constructions";
            txt_dentalCAD.Text = Properties.Settings.Default.ExocaddentalCAD ?? @"C:\Exocad\DentalCAD";
            txt_LocalScanFolder.Text = Properties.Settings.Default.LocalScanFolder ?? @"C:\DefaultScanFolder";
            txt_ServerTargetFolder.Text = Properties.Settings.Default.ServerTargetFolder ?? @"\\Server\DefaultTarget";
            listBox_Output.HorizontalScrollbar = true;

            chk_DentalCadWatcher.Checked = Properties.Settings.Default.EnableDentalCadWatcher;
            chk_FolderUploader.Checked = Properties.Settings.Default.EnableFolderUploader;

            this.Load += UC_Exocad_Load;
        }

        private void UC_Exocad_Load(object sender, EventArgs e)
        {
            InitializeWatchers();
        }

        private void InitializeWatchers()
        {
            try
            {
                InitializeDentalCadWatcher();
                InitializeFolderUploader();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Starten der Überwachung: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMessage($"Fehler beim Starten der Überwachung: {ex.Message}");
            }
        }

        private void InitializeDentalCadWatcher()
        {
            if (_fileWatcher != null)
            {
                _fileWatcher.FileHandled -= OnFileHandled;
                _fileWatcher.StopWatching();
                _fileWatcher.Dispose();
                _fileWatcher = null;
            }

            _fileWatcher = new DentalCadFileWatcher(_logHandler, TimeSpan.FromSeconds(30), 10);
            _fileWatcher.FileHandled += OnFileHandled;

            if (chk_DentalCadWatcher.Checked)
            {
                _fileWatcher.StartWatching();
                _dentalCadWatcherActive = true;
                LogMessage("DentalCadFileWatcher gestartet.");
            }
            else
            {
                _fileWatcher.StopWatching();
                _dentalCadWatcherActive = false;
                LogMessage("DentalCadFileWatcher gestoppt.");
            }
        }

        private void InitializeFolderUploader()
        {
            if (_folderUploader != null)
            {
                _folderUploader.FolderUploaded -= OnFolderUploaded;
                _folderUploader.StopWatching();
                _folderUploader.Dispose();
                _folderUploader = null;
            }

            string localFolder = txt_LocalScanFolder.Text.Trim();
            string serverFolder = txt_ServerTargetFolder.Text.Trim();

            if (string.IsNullOrWhiteSpace(localFolder) || string.IsNullOrWhiteSpace(serverFolder))
            {
                LogMessage("Ungültige Pfade: Lokaler Scan-Ordner oder Server-Zielordner ist leer.");
                return;
            }

            _folderUploader = new FolderWatcherAndUploader(localFolder, serverFolder, _logHandler);
            _folderUploader.FolderUploaded += OnFolderUploaded;

            if (chk_FolderUploader.Checked)
            {
                _folderUploader.StartWatching();
                _folderUploaderActive = true;
                LogMessage("FolderWatcherAndUploader gestartet.");
            }
            else
            {
                _folderUploader.StopWatching();
                _folderUploaderActive = false;
                LogMessage("FolderWatcherAndUploader gestoppt.");
            }
        }

        private void OnFileHandled(string message)
        {
            listBox_Output.Items.Add($"FileHandled ausgelöst: {message}");

            if (IsHandleCreated && !IsDisposed)
            {
                BeginInvoke(new Action(() =>
                {
                    listBox_Output.Items.Add(message);
                    listBox_Output.TopIndex = listBox_Output.Items.Count - 1;
                }));
            }
            else
            {
                listBox_Output.Items.Add("UI nicht bereit für Update: " + message);
            }
            _logHandler.LogAction(message);
        }

        private void OnFolderUploaded(string message)
        {
            if (IsHandleCreated && !IsDisposed)
            {
                BeginInvoke(new Action(() =>
                {
                    listBox_Output.Items.Add(message);
                    listBox_Output.TopIndex = listBox_Output.Items.Count - 1;
                }));
            }
            _logHandler.LogAction(message);
        }

        private void LogMessage(string message)
        {
            OnFileHandled($"{DateTime.Now}: {message}");
        }

        private void btn_Exocad_Ordner_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_Exocad_Constructions.Text = selectedPath;
                Properties.Settings.Default.ExocadConstructions = selectedPath;
                Properties.Settings.Default.Save();
                LogMessage($"Exocad-Ordner geändert: {selectedPath}");
            }
        }

        private void btn_ExoCad_ZielOrdner_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_dentalCAD.Text = selectedPath;
                Properties.Settings.Default.ExocaddentalCAD = selectedPath;
                Properties.Settings.Default.Save();
                LogMessage($"Zielordner geändert: {selectedPath}");
            }
        }

        private void btn_LocalScanFolder_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_LocalScanFolder.Text = selectedPath;
                Properties.Settings.Default.LocalScanFolder = selectedPath;
                Properties.Settings.Default.Save();
                RestartFolderWatcher();
                LogMessage($"Lokaler Scan-Ordner geändert: {selectedPath}");
            }
        }

        private void btn_ServerTargetFolder_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_ServerTargetFolder.Text = selectedPath;
                Properties.Settings.Default.ServerTargetFolder = selectedPath;
                Properties.Settings.Default.Save();
                RestartFolderWatcher();
                LogMessage($"Server-Zielordner geändert: {selectedPath}");
            }
        }

        private void chk_DentalCadWatcher_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableDentalCadWatcher = chk_DentalCadWatcher.Checked;
            Properties.Settings.Default.Save();
        }

        private void chk_FolderUploader_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EnableFolderUploader = chk_FolderUploader.Checked;
            Properties.Settings.Default.Save();
            InitializeFolderUploader();
        }

        private string GetSelectedFolderPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                return fbd.ShowDialog() == DialogResult.OK ? fbd.SelectedPath : null;
            }
        }

        private void RestartFolderWatcher()
        {
            InitializeFolderUploader();
        }

        public new void Dispose()
        {
            if (!disposed)
            {
                _fileWatcher?.StopWatching();
                _fileWatcher?.Dispose();
                _folderUploader?.StopWatching();
                _folderUploader?.Dispose();
                disposed = true;
                base.Dispose();
            }
        }
    }
}