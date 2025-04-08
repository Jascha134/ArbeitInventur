using System;
using System.IO;
using System.Threading.Tasks;

namespace ArbeitInventur.Exocad_Help
{
    public class FolderWatcherAndUploader : IDisposable
    {
        private readonly string localFolderPath;  // Pfad zum überwachten lokalen Ordner auf PC2
        private readonly string serverFolderPath; // Zielpfad auf dem Server
        private readonly FileSystemWatcher watcher;
        private bool disposed = false;

        public event Action<string> FolderUploaded; // Event, das ausgelöst wird, wenn der Ordner hochgeladen wurde

        public FolderWatcherAndUploader(string localPath, string serverPath)
        {
            localFolderPath = localPath ?? throw new ArgumentNullException(nameof(localPath));
            serverFolderPath = serverPath ?? throw new ArgumentNullException(nameof(serverPath));

            // FileSystemWatcher initialisieren
            watcher = new FileSystemWatcher
            {
                Path = localFolderPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite,
                Filter = "*.*", // Alle Dateien überwachen
                EnableRaisingEvents = false
            };

            watcher.Created += OnFileCreated;
            watcher.Changed += OnFileChanged;
        }

        public void StartWatching()
        {
            if (disposed) throw new ObjectDisposedException(nameof(FolderWatcherAndUploader));
            if (!Directory.Exists(localFolderPath))
            {
                Directory.CreateDirectory(localFolderPath);
            }
            watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Überwachung von {localFolderPath} gestartet.");
        }

        public void StopWatching()
        {
            if (!disposed)
            {
                watcher.EnableRaisingEvents = false;
            }
        }

        private async void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            await HandleFileEventAsync(e.FullPath);
        }

        private async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            await HandleFileEventAsync(e.FullPath);
        }

        private async Task HandleFileEventAsync(string filePath)
        {
            // Prüfen, ob die .dentalProject-Datei existiert (Signal für Scan-Abschluss)
            if (Path.GetExtension(filePath).ToLower() == ".dentalproject" && IsFileReady(filePath))
            {
                try
                {
                    // Warten, bis alle Dateien geschrieben wurden (z. B. kurze Verzögerung)
                    await Task.Delay(1000); // Anpassbar je nach Scan-Prozess

                    // Ordner auf Server kopieren
                    await CopyFolderToServerAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Hochladen des Ordners: {ex.Message}");
                }
            }
        }

        private bool IsFileReady(string filePath)
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
                return false;
            }
        }

        private async Task CopyFolderToServerAsync()
        {
            try
            {
                string folderName = Path.GetFileName(localFolderPath);
                string targetPath = Path.Combine(serverFolderPath, folderName);

                if (!Directory.Exists(serverFolderPath))
                {
                    Directory.CreateDirectory(serverFolderPath);
                }

                // Kopieren des gesamten Ordners auf den Server
                await Task.Run(() =>
                {
                    CopyDirectory(localFolderPath, targetPath);
                });

                Console.WriteLine($"Ordner {folderName} erfolgreich auf {serverFolderPath} hochgeladen.");
                FolderUploaded?.Invoke($"Ordner {folderName} hochgeladen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Kopieren des Ordners: {ex.Message}");
                throw;
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) throw new DirectoryNotFoundException($"Quellordner nicht gefunden: {sourceDir}");

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