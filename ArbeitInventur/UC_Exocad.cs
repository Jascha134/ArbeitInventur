using ArbeitInventur.Exocad_Help;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class UC_Exocad : UserControl, IDisposable
    {

        private readonly HashSet<string> _loadedLogEntries = new HashSet<string>();
        private bool IsUiReady() => IsHandleCreated && !IsDisposed;

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
            // Lade Logeinträge des heutigen Tages
            LoadTodayLogs();
            InitializeWatchers();
        }
        private void AddToListBox(string message)
        {
            if (!IsUiReady()) return;

            BeginInvoke(new Action(() =>
            {
                if (_loadedLogEntries.Add(message)) // vermeide doppelte Anzeige
                {
                    listBox_Output.Items.Add(message);
                    listBox_Output.TopIndex = listBox_Output.Items.Count - 1;
                }
            }));
        }


        private void LoadTodayLogs()
        {
            try
            {
                var todayLogs = _logHandler.GetTodayLogEntries();

                foreach (var log in todayLogs)
                {
                    string formatted = $"{log.Timestamp:yyyy-MM-dd HH:mm:ss}: {log.Message}";
                    if (_loadedLogEntries.Add(formatted))
                    {
                        listBox_Output.Items.Add(formatted);
                    }
                }

                listBox_Output.TopIndex = listBox_Output.Items.Count - 1;
            }
            catch (Exception ex)
            {
                AddToListBox($"Fehler beim Laden der Logs: {ex.Message}");
            }
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
        }

        private void InitializeFolderUploader()
        {
            if (_folderUploader != null && chk_FolderUploader.Checked != true)
            {
                _folderUploader.FolderUploaded -= OnFolderUploaded;
                _folderUploader.StopWatching();
                _folderUploader.Dispose();
                _folderUploader = null;
                AddToListBox("Überwachung von Ordner-Upload gestoppt.");
            }
            else if (chk_FolderUploader.Checked == true)
            {
                string localFolder = txt_LocalScanFolder.Text.Trim();
                string serverFolder = txt_ServerTargetFolder.Text.Trim();

                if (string.IsNullOrWhiteSpace(localFolder) || string.IsNullOrWhiteSpace(serverFolder))
                {
                    LogMessage("Ungültige Pfade für FolderUploader.");
                    return;
                }
                _folderUploader = new FolderWatcherAndUploader(localFolder, serverFolder, _logHandler);
                _folderUploader.FolderUploaded += OnFolderUploaded;
                _folderUploader.StartWatching();
                AddToListBox("Überwachung von Ordner-Upload gestartet.");
            }
        }


        private void OnFileHandled(string message)
        {
            AddToListBox(message);
            _logHandler.LogAction(message); // wichtig: persistenter Log
        }

        private void OnFolderUploaded(string message)
        {
            AddToListBox(message);
            _logHandler.LogAction(message);
        }

        private void LogMessage(string message)
        {
            string fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
            _logHandler.LogAction(fullMessage); // Nur Datei
            AddToListBox(fullMessage);          // Nur UI
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