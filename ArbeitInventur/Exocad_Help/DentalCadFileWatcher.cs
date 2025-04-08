using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks; // Für async/await benötigt

namespace ArbeitInventur.Exocad_Help
{
    public class DentalCadFileWatcher : IDisposable
    {
        // Delegate-Definition hinzufügen
        public delegate void FileHandledEventHandler(string message);
        public event FileHandledEventHandler FileHandled;

        private readonly string _watchFolderPath;
        private readonly string _targetFolderPath;
        private readonly FileSystemWatcher _watcher;
        private readonly LogHandler _logHandler;
        private readonly HashSet<string> _processedFiles;
        private bool _disposed;
        private readonly TimeSpan _maxWaitTime;
        private readonly int _maxRetries;

        public DentalCadFileWatcher() : this(TimeSpan.FromSeconds(5), 10) { }

        public DentalCadFileWatcher(TimeSpan maxWaitTime, int maxRetries)
        {
            _watchFolderPath = Properties.Settings.Default.ExocadConstructions;
            _targetFolderPath = Properties.Settings.Default.ExocaddentalCAD;
            _logHandler = new LogHandler(Path.Combine(Properties.Settings.Default.DataJSON, "ExocadListLog.json"));
            _processedFiles = new HashSet<string>();
            _watcher = InitializeFileWatcher();
            _maxWaitTime = maxWaitTime;
            _maxRetries = maxRetries;
            try
            {
                _logHandler.LoadLogEntries();
            }
            catch (Exception ex)
            {
                // Fehler beim Laden der Logs sollte den Konstruktor nicht abbrechen
                Console.WriteLine($"Warnung: Log-Initialisierung fehlgeschlagen: {ex.Message}");
            }
        }

        private FileSystemWatcher InitializeFileWatcher()
        {
            return new FileSystemWatcher(_watchFolderPath)
            {
                Filter = "*.dentalCAD",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                IncludeSubdirectories = true,
                EnableRaisingEvents = false
            };
        }

        public void StartWatching()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DentalCadFileWatcher));

            _watcher.Created += OnDentalCadFileEvent;
            _watcher.Changed += OnDentalCadFileEvent;
            _watcher.Renamed += OnDentalCadFileRenamed;
            _watcher.EnableRaisingEvents = true;
        }

        private async void OnDentalCadFileEvent(object sender, FileSystemEventArgs e)
        {
            await ProcessFileIfReadyAsync(e.FullPath, e.ChangeType.ToString().ToLower(), CancellationToken.None);
        }

        private async void OnDentalCadFileRenamed(object sender, RenamedEventArgs e)
        {
            await ProcessFileIfReadyAsync(e.FullPath, e.ChangeType.ToString(), CancellationToken.None);
        }

        private async Task ProcessFileIfReadyAsync(string filePath, string action, CancellationToken cancellationToken)
        {
            if (_processedFiles.Contains(filePath) || !await IsFileReadyAsync(filePath, cancellationToken))
                return;

            try
            {
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(_targetFolderPath, fileName);

                await WaitForFileAsync(filePath, cancellationToken);
                await Task.Run(() => File.Copy(filePath, destinationPath, true), cancellationToken);

                string message = $"Datei '{fileName}' wurde in den Zielordner kopiert ({action}).";
                ProcessSuccess(filePath, message, action);
            }
            catch (Exception ex)
            {
                ProcessError(filePath, $"Fehler beim Kopieren der Datei: {ex.Message}", ex);
            }
        }

        private void ProcessSuccess(string filePath, string message, string action)
        {
            _processedFiles.Add(filePath);
            _logHandler.AddToLog(message, action);
            FileHandled?.Invoke(message);
        }

        private void ProcessError(string filePath, string errorMessage, Exception ex)
        {
            _logHandler.AddToLog($"{errorMessage}\nStackTrace: {ex.StackTrace}", "Fehler");
            FileHandled?.Invoke(errorMessage);
        }

        private async Task<bool> IsFileReadyAsync(string filePath, CancellationToken cancellationToken)
        {
            int retries = 0;

            while (retries < _maxRetries)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        return stream.Length > 0;
                    }
                }
                catch (IOException)
                {
                    retries++;
                    await Task.Delay(100, cancellationToken);
                }
                cancellationToken.ThrowIfCancellationRequested();
            }
            return false;
        }

        private async Task WaitForFileAsync(string filePath, CancellationToken cancellationToken)
        {
            TimeSpan waitedTime = TimeSpan.Zero;

            while (waitedTime < _maxWaitTime)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        if (stream.Length > 0) return;
                    }
                }
                catch (IOException)
                {
                    await Task.Delay(100, cancellationToken);
                    waitedTime += TimeSpan.FromMilliseconds(100);
                }
                cancellationToken.ThrowIfCancellationRequested();
            }
            throw new IOException($"Timeout waiting for file access: {filePath}");
        }

        public void StopWatching()
        {
            if (_disposed || _watcher == null) return;

            _watcher.EnableRaisingEvents = false;
            _watcher.Created -= OnDentalCadFileEvent;
            _watcher.Changed -= OnDentalCadFileEvent;
            _watcher.Renamed -= OnDentalCadFileRenamed;
            _watcher.Dispose();
            _logHandler.AddToLog("Überwachung von .dentalCAD-Dateien gestoppt.", "Stopp");
        }

        public void DisplayLogEntriesInListBox(ListBox listBox)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DentalCadFileWatcher));
            if (listBox == null) return;

            listBox.Items.Clear();
            foreach (var entry in _logHandler.GetTodayLogEntries())
            {
                listBox.Items.Add($"{entry.Timestamp}: {entry.Action} - {entry.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                StopWatching();
            }

            _disposed = true;
        }

        ~DentalCadFileWatcher()
        {
            Dispose(false);
        }
    }
    public class LogHandler
    {
        private readonly string _logFilePath;
        private List<LogEntry> _logEntries;
        private readonly object _fileLock = new object(); // Für Thread-Sicherheit

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
            lock (_fileLock)
            {
                try
                {
                    if (File.Exists(_logFilePath) && new FileInfo(_logFilePath).Length > 0)
                    {
                        string json = File.ReadAllText(_logFilePath);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            _logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
                        }
                    }
                }
                catch (JsonReaderException ex)
                {
                    // Bei ungültigem JSON: Loggen und mit leerer Liste fortfahren
                    Console.WriteLine($"Fehler beim Lesen der Log-Datei: {ex.Message}. Starte mit leerer Liste.");
                    _logEntries = new List<LogEntry>();
                    // Optional: Backup der beschädigten Datei erstellen
                    string backupPath = _logFilePath + ".backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    File.Copy(_logFilePath, backupPath, true);
                    File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unerwarteter Fehler beim Laden der Log-Datei: {ex.Message}");
                    _logEntries = new List<LogEntry>();
                }
            }
        }

        public List<LogEntry> GetTodayLogEntries()
        {
            lock (_fileLock)
            {
                return _logEntries.Where(entry => entry.Timestamp.Date == DateTime.Now.Date).ToList();
            }
        }

        public void AddToLog(string message, string action)
        {
            lock (_fileLock)
            {
                try
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
                }
            }
        }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }
}