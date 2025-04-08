using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.ScannÜber
{
    /// <summary>
    /// Überwacht einen Ordner und löst ein Event aus, wenn ein neuer Unterordner erstellt wird.
    /// </summary>
    public class FolderMonitor
    {
        private readonly FileSystemWatcher watcher;

        /// <summary>
        /// Event, das ausgelöst wird, wenn ein neuer Ordner erstellt wird.
        /// </summary>
        public event Action<string> FolderCreated;

        public FolderMonitor(string path)
        {
            watcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.DirectoryName,
                Filter = "*",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };

            watcher.Created += OnCreated;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath))
            {
                FolderCreated?.Invoke(e.FullPath);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }
    }
}
