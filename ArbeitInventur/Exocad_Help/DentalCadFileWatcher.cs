using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace ArbeitInventur.Exocad_Help
{
    public class DentalCadFileWatcher 
    {
        public delegate void FileHandledEventHandler(string message);
        public event FileHandledEventHandler FileHandled;

        private readonly string _watchFolderPath;
        private readonly string _targetFolderPath;
        private FileSystemWatcher _watcher;
        private readonly LogHandler _logHandler;
        private HashSet<string> _processedFiles;

        public DentalCadFileWatcher()
        {
            _watchFolderPath = Properties.Settings.Default.ExocadConstructions;
            _targetFolderPath = Properties.Settings.Default.ExocaddentalCAD;
            string logFilePath = Path.Combine(Properties.Settings.Default.DataJSON, "ExocadListLog.json");
            _processedFiles = new HashSet<string>();

            _logHandler = new LogHandler(logFilePath);
            _logHandler.LoadLogEntries();
        }
        public void StartWatching()
        {
            _watcher = new FileSystemWatcher(_watchFolderPath)
            {
                Filter = "*.dentalCAD",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                IncludeSubdirectories = true
            };

            _watcher.Created += OnDentalCadFileEvent;
            _watcher.Changed += OnDentalCadFileEvent;
            _watcher.Renamed += OnDentalCadFileRenamed;
            _watcher.EnableRaisingEvents = true;

        }
        private void OnDentalCadFileEvent(object sender, FileSystemEventArgs e)
        {
            if (!_processedFiles.Contains(e.FullPath) && IsFileReady(e.FullPath))
            {
                HandleDentalCadFile(e.FullPath, e.ChangeType.ToString().ToLower());
            }
        }
        private void OnDentalCadFileRenamed(object sender, RenamedEventArgs e)
        {
            HandleDentalCadFile(e.FullPath, e.ChangeType.ToString());
        }
        private void HandleDentalCadFile(string filePath, string action)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(_targetFolderPath, fileName);

                WaitForFile(filePath);

                File.Copy(filePath, destinationPath, true);
                string message = $"Datei '{fileName}' wurde in den Zielordner kopiert ({action}).";
                LogMessage(message, action);

                // Event auslösen, damit die GUI-Komponente aktualisiert wird
                FileHandled?.Invoke(message);

                _processedFiles.Add(filePath);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Fehler beim Kopieren der Datei: {ex.Message}";
                LogMessage(errorMessage, "Fehler");

                // Event auslösen, damit die GUI-Komponente aktualisiert wird
                FileHandled?.Invoke(errorMessage);
            }
        }
        private void WaitForFile(string filePath)
        {
            while (true)
            {
                try
                {
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        if (stream.Length > 0)
                        {
                            break;
                        }
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }
        }
        private bool IsFileReady(string filePath)
        {
            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return stream.Length > 0;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }
        public void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                LogMessage("Überwachung von .dentalCAD-Dateien gestoppt.", "Stopp");
            }
        }
        private void LogMessage(string message, string action)
        {
            _logHandler.AddToLog(message, action);
        }
        public void DisplayLogEntriesInListBox(ListBox listBox)
        {
            listBox.Items.Clear();
            foreach (var entry in _logHandler.GetTodayLogEntries())
            {
                listBox.Items.Add($"{entry.Timestamp}: {entry.Action} - {entry.Message}");
            }
        }
    }
    public class LogHandler
    {
        private readonly string _logFilePath;
        private List<LogEntry> _logEntries;

        public LogHandler(string logFilePath)
        {
            _logFilePath = logFilePath;
            _logEntries = new List<LogEntry>();

            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }

        public void LoadLogEntries()
        {
            if (File.Exists(_logFilePath))
            {
                string json = File.ReadAllText(_logFilePath);
                _logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
        }

        public List<LogEntry> GetTodayLogEntries()
        {
            return _logEntries.Where(entry => entry.Timestamp.Date == DateTime.Now.Date).ToList();
        }

        public void AddToLog(string message, string action)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Action = action,
                Message = message
            };

            _logEntries.Add(logEntry);
            File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
        }
    }
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }
}



