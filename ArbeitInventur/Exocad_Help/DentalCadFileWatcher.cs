using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArbeitInventur.Exocad_Help
{
    public class DentalCadFileWatcher : IDisposable
    {
        public delegate void FileHandledEventHandler(string message);
        public event FileHandledEventHandler FileHandled;

        private readonly string _watchFolderPath;
        private readonly string _targetFolderPath;
        private readonly FileSystemWatcher _watcher;
        private readonly LogHandler _logHandler;
        private readonly HashSet<string> _processedFiles;
        private readonly object _processedFilesLock = new object();
        private bool _disposed;
        private readonly TimeSpan _maxWaitTime;
        private readonly int _maxRetries;

        public DentalCadFileWatcher(LogHandler logHandler, TimeSpan maxWaitTime, int maxRetries)
        {
            _watchFolderPath = Properties.Settings.Default.ExocadConstructions ?? throw new ArgumentException("ExocadConstructions-Pfad ist nicht konfiguriert.");
            _targetFolderPath = Properties.Settings.Default.ExocaddentalCAD ?? throw new ArgumentException("ExocaddentalCAD-Pfad ist nicht konfiguriert.");
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));
            _processedFiles = new HashSet<string>();
            _watcher = InitializeFileWatcher();
            _maxWaitTime = maxWaitTime;
            _maxRetries = maxRetries;
        }

        private FileSystemWatcher InitializeFileWatcher()
        {
            if (!Directory.Exists(_watchFolderPath))
            {
                throw new DirectoryNotFoundException($"Überwachungsordner {_watchFolderPath} existiert nicht.");
            }
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
            if (!Directory.Exists(_targetFolderPath))
            {
                throw new DirectoryNotFoundException($"Zielordner {_targetFolderPath} existiert nicht.");
            }

            _watcher.Created += async (s, e) => await OnDentalCadFileEvent(s, e);
            _watcher.Changed += async (s, e) => await OnDentalCadFileEvent(s, e);
            _watcher.Renamed += async (s, e) => await OnDentalCadFileRenamed(s, e);
            _watcher.EnableRaisingEvents = true;
            _logHandler.AddToLog("DentalCadFileWatcher gestartet.", "Start");
        }

        private async Task OnDentalCadFileEvent(object sender, FileSystemEventArgs e)
        {
            try
            {
                await ProcessFileIfReadyAsync(e.FullPath, e.ChangeType.ToString().ToLower(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logHandler.AddToLog($"Fehler in OnDentalCadFileEvent: {ex.Message}", "Fehler");
            }
        }

        private async Task OnDentalCadFileRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                await ProcessFileIfReadyAsync(e.FullPath, e.ChangeType.ToString(), CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logHandler.AddToLog($"Fehler in OnDentalCadFileRenamed: {ex.Message}", "Fehler");
            }
        }

        private async Task ProcessFileIfReadyAsync(string filePath, string action, CancellationToken cancellationToken)
        {
            bool alreadyProcessed;
            lock (_processedFilesLock)
            {
                alreadyProcessed = _processedFiles.Contains(filePath);
            }

            if (alreadyProcessed || !await IsFileReadyAsync(filePath, cancellationToken))
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
            lock (_processedFilesLock)
            {
                _processedFiles.Add(filePath);
            }
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
            _watcher.Created -= async (s, e) => await OnDentalCadFileEvent(s, e);
            _watcher.Changed -= async (s, e) => await OnDentalCadFileEvent(s, e);
            _watcher.Renamed -= async (s, e) => await OnDentalCadFileRenamed(s, e);
            _watcher.Dispose();
            _logHandler.AddToLog("Überwachung von .dentalCAD-Dateien gestoppt.", "Stopp");
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
}