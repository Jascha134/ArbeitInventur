using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.ScannÜber
{
    /// <summary>
    /// PC1: Überwacht den Ordner "X:\Jascha\Frässen\GeScannt".
    /// Sobald ein neuer Auftrag (Ordner) erscheint, wird dieser in den lokalen Ordner "C:\AG DentalData\Data\Constructions" kopiert.
    /// Der Vorgang wird protokolliert.
    /// </summary>
    public class DigitalOrderDownloaderSimple
    {
        private readonly FolderMonitor folderMonitor;
        private readonly FolderTransferService folderTransferService;
        private readonly Logger logger;
        private readonly string localConstructionsPath;

        /// <summary>
        /// Initialisiert den Downloader.
        /// </summary>
        /// <param name="serverWatchPath">
        /// Der Pfad, der überwacht wird. (Für PC1: "X:\Jascha\Frässen\GeScannt")
        /// </param>
        /// <param name="localConstructionsPath">
        /// Der Zielpfad, wohin der Ordner kopiert werden soll (für PC1: "C:\AG DentalData\Data\Constructions")
        /// </param>
        /// <param name="logger">Logger zur Protokollierung.</param>
        public DigitalOrderDownloaderSimple(string serverWatchPath, string localConstructionsPath, Logger logger)
        {
            this.localConstructionsPath = localConstructionsPath;
            this.logger = logger;
            folderTransferService = new FolderTransferService();
            folderMonitor = new FolderMonitor(serverWatchPath);
            folderMonitor.FolderCreated += OnFolderCreated;
            logger.Log($"[PC1] Überwachung gestartet: {serverWatchPath}");
        }

        /// <summary>
        /// Wird ausgelöst, wenn im überwachten Ordner ein neuer Unterordner erstellt wird.
        /// </summary>
        /// <param name="folderPath">Der Pfad des neu erstellten Ordners.</param>
        private void OnFolderCreated(string folderPath)
        {
            logger.Log($"[PC1] Neuer Auftrag registriert: {folderPath}");
            // Asynchrones Kopieren
            Task.Run(() =>
            {
                try
                {
                    // Zielpfad: lokaler Constructions-Pfad + Name des neu erstellten Ordners
                    string destPath = System.IO.Path.Combine(localConstructionsPath, System.IO.Path.GetFileName(folderPath));
                    folderTransferService.CopyFolder(folderPath, destPath);
                    logger.Log($"[PC1] Auftrag erfolgreich kopiert von '{folderPath}' nach '{destPath}'");
                }
                catch (Exception ex)
                {
                    logger.Log($"[PC1] Fehler beim Kopieren von '{folderPath}': {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Beendet die Überwachung.
        /// </summary>
        public void Stop()
        {
            folderMonitor.Stop();
            logger.Log("[PC1] Überwachung gestoppt.");
        }
    }
}
