using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArbeitInventur.Exocad_Help
{
    public class FolderWatcherAndUploader : IDisposable
    {
        private readonly string localFolderPath;
        private readonly string serverFolderPath;
        private readonly FileSystemWatcher watcher;
        private readonly LogHandler _logHandler;
        private bool disposed = false;

        public event Action<string> FolderUploaded;

        public FolderWatcherAndUploader(string localPath, string serverPath, LogHandler logHandler)
        {
            localFolderPath = localPath ?? throw new ArgumentNullException(nameof(localPath));
            serverFolderPath = serverPath ?? throw new ArgumentNullException(nameof(serverPath));
            _logHandler = logHandler ?? throw new ArgumentNullException(nameof(logHandler));

            watcher = new FileSystemWatcher
            {
                Path = localFolderPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite,
                Filter = "*.*",
                EnableRaisingEvents = false
            };

            watcher.Created += async (s, e) => await OnFileCreated(s, e);
            watcher.Changed += async (s, e) => await OnFileChanged(s, e);
        }

        public void StartWatching()
        {
            if (disposed) throw new ObjectDisposedException(nameof(FolderWatcherAndUploader));
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }
            if (!Directory.Exists(serverFolderPath))
            {
                throw new ArgumentException($"Serverpfad {serverFolderPath} existiert nicht oder ist nicht zugänglich.");
            }
            watcher.EnableRaisingEvents = true;
            _logHandler.AddToLog($"Überwachung von {localFolderPath} gestartet.", "Start");
        }

        public void StopWatching()
        {
            if (!disposed)
            {
                watcher.EnableRaisingEvents = false;
            }
            _logHandler.AddToLog($"Überwachung von {localFolderPath} gestoppt.", "Stop");
        }

        private async Task OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                await HandleFileEventAsync(e.FullPath, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logHandler.AddToLog($"Fehler in OnFileCreated: {ex.Message}", "Fehler");
            }
        }

        private async Task OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                await HandleFileEventAsync(e.FullPath, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logHandler.AddToLog($"Fehler in OnFileChanged: {ex.Message}", "Fehler");
            }
        }

        private async Task HandleFileEventAsync(string filePath, CancellationToken cancellationToken)
        {
            if (Path.GetExtension(filePath).ToLower() == ".dentalproject" && await IsFileReadyAsync(filePath, cancellationToken))
            {
                try
                {
                    await Task.Delay(1000, cancellationToken);
                    await CopyFolderToServerAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logHandler.AddToLog($"Fehler beim Hochladen des Ordners: {ex.Message}", "Fehler");
                }
            }
        }

        private async Task<bool> IsFileReadyAsync(string filePath, CancellationToken cancellationToken)
        {
            int retries = 0;
            const int maxRetries = 10;
            while (retries < maxRetries)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        return true;
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

        private async Task CopyFolderToServerAsync(CancellationToken cancellationToken)
        {
            try
            {
                string folderName = Path.GetFileName(localFolderPath);
                string targetPath = Path.Combine(serverFolderPath, folderName);
                Directory.CreateDirectory(targetPath);

                var dir = new DirectoryInfo(localFolderPath);
                foreach (var file in dir.GetFiles("*.dentalproject"))
                {
                    try
                    {
                        string targetFilePath = Path.Combine(targetPath, file.Name);
                        if (!File.Exists(targetFilePath) || file.LastWriteTime > File.GetLastWriteTime(targetFilePath))
                        {
                            await Task.Run(() => file.CopyTo(targetFilePath, true), cancellationToken);
                            _logHandler.AddToLog($"Datei {file.Name} kopiert.", "Kopieren");
                        }
                    }
                    catch (IOException ex)
                    {
                        _logHandler.AddToLog($"Fehler beim Kopieren der Datei {file.Name}: {ex.Message}", "Fehler");
                    }
                }

                FolderUploaded?.Invoke($"Ordner {folderName} hochgeladen.");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logHandler.AddToLog($"Zugriffsfehler beim Hochladen des Ordners: {ex.Message}", "Fehler");
                throw;
            }
            catch (Exception ex)
            {
                _logHandler.AddToLog($"Unbekannter Fehler beim Hochladen des Ordners: {ex.Message}", "Fehler");
                throw;
            }
        }

        private async Task CopyDirectoryAsync(string sourceDir, string destDir, CancellationToken cancellationToken)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) throw new DirectoryNotFoundException($"Quellordner nicht gefunden: {sourceDir}");
            Directory.CreateDirectory(destDir);

            foreach (var file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                await Task.Run(() => file.CopyTo(targetFilePath, true), cancellationToken);
                _logHandler.AddToLog($"Datei {file.Name} kopiert.", "Kopieren");
            }

            foreach (var subDir in dir.GetDirectories())
            {
                string targetSubDirPath = Path.Combine(destDir, subDir.Name);
                await CopyDirectoryAsync(subDir.FullName, targetSubDirPath, cancellationToken);
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                StopWatching();
                watcher.Dispose();
                disposed = true;
            }
        }
    }
}