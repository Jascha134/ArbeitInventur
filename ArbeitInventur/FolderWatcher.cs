using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms; // Für ListBox

namespace ArbeitInventur
{
    public class FolderWatcher
    {
        protected readonly string _sourcePath;
        protected readonly string _destinationPath;
        protected readonly FileSystemWatcher _watcher;
        protected LogHandler _logHandler; // Gemeinsamer Logger

        public delegate void OperationHandledEventHandler(string message);
        public event OperationHandledEventHandler OperationHandled;

        // Parameterloser Konstruktor: Liest Pfade und Log-Datei aus den Einstellungen
        public FolderWatcher()
            : this(Properties.Settings.Default.sourcePath,
                   Properties.Settings.Default.destinationPath,
                   Path.Combine(Properties.Settings.Default.DataJSON, "FolderWatcherLog.json"))
        {
        }

        // Konstruktor mit LogFile-Parameter
        public FolderWatcher(string sourcePath, string destinationPath, string logFilePath)
        {
            _sourcePath = sourcePath;
            _destinationPath = destinationPath;

            if (!string.IsNullOrEmpty(logFilePath))
            {
                _logHandler = new LogHandler(Path.Combine(Properties.Settings.Default.DataJSON, "FolderWatcherLog.json"));

                _logHandler.LoadLogEntries();
            }

            if (!string.IsNullOrEmpty(_sourcePath) && !string.IsNullOrEmpty(_destinationPath))
            {
                _watcher = new FileSystemWatcher(_sourcePath)
                {
                    NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName,
                    Filter = "*.*",
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = false // Aktivierung in StartWatching()
                };

                _watcher.Created += OnNewItemCreated;
            }
        }

        // Reagiert auf neue Elemente (Ordner)
        protected virtual void OnNewItemCreated(object sender, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath))
            {
                Thread.Sleep(5000); // Verzögerung, um Schreibvorgänge abzuwarten
                if (IsFolderFullyWritten(e.FullPath))
                {
                    string destFolder = Path.Combine(_destinationPath, new DirectoryInfo(e.FullPath).Name);
                    CopyFolder(e.FullPath, destFolder);
                    // Logge den Vorgang
                    LogMessage($"Ordner '{new DirectoryInfo(e.FullPath).Name}' wurde in den Zielordner kopiert.", "Ordner kopiert");
                }
            }
        }

        protected bool IsFolderFullyWritten(string folderPath)
        {
            int retries = 3;
            while (retries > 0)
            {
                long initialSize = GetDirectorySize(folderPath);
                Thread.Sleep(2000);
                long newSize = GetDirectorySize(folderPath);

                if (initialSize == newSize)
                    return true;
                retries--;
            }
            return false;
        }

        protected long GetDirectorySize(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories)
                            .Sum(file => new FileInfo(file).Length);
        }

        protected void CopyFolder(string source, string destination)
        {
            Directory.CreateDirectory(destination);
            foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                string destFile = file.Replace(source, destination);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                File.Copy(file, destFile, true);
            }
        }

        // Schreibt eine Log-Nachricht und feuert ein Event
        protected void LogMessage(string message, string action)
        {
            if (_logHandler != null)
            {
                _logHandler.AddToLog(message, action);
                OperationHandled?.Invoke(message);
            }
        }

        // Startet die Überwachung
        public virtual void StartWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = true;
            }
        }

        public void StopWatching()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
        }

        // Zeigt die Log-Einträge in der ListBox an
        public virtual void DisplayLogEntriesInListBox(ListBox listBox)
        {
            listBox.Items.Clear();
            if (_logHandler != null)
            {
                foreach (var entry in _logHandler.GetTodayLogEntries())
                {
                    listBox.Items.Add($"{entry.Timestamp}: {entry.Action} - {entry.Message}");
                }
            }
        }
    }
}
