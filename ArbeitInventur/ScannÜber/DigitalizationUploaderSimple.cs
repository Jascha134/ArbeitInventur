using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.ScannÜber
{
    /// <summary>
    /// PC2: Überwacht den Ordner "C:\AG DentalData\Data\Constructions".
    /// Sobald ein neuer Auftrag (Ordner) erscheint, wird dieser in den Zielordner "X:\Jascha\Frässen\GeScannt" kopiert.
    /// Alle Vorgänge werden protokolliert.
    /// </summary>
    public class DigitalizationUploaderSimple
    {
        private readonly FolderMonitor folderMonitor;
        private readonly FolderTransferService folderTransferService;
        private readonly Logger logger;
        private readonly string serverDestinationPath;

        /// <summary>
        /// Initialisiert den Uploader.
        /// </summary>
        /// <param name="localWatchPath">
        /// Der Pfad, der überwacht wird. (Für PC2: "C:\AG DentalData\Data\Constructions")
        /// </param>
        /// <param name="serverDestinationPath">
        /// Der Zielpfad, wohin der Ordner kopiert werden soll (für PC2: "X:\Jascha\Frässen\GeScannt")
        /// </param>
        /// <param name="logger">Logger zur Protokollierung.</param>
        public DigitalizationUploaderSimple(string localWatchPath, string serverDestinationPath, Logger logger)
        {
            this.serverDestinationPath = serverDestinationPath;
            this.logger = logger;
            folderTransferService = new FolderTransferService();
            folderMonitor = new FolderMonitor(localWatchPath);
            folderMonitor.FolderCreated += OnFolderCreated;
            logger.Log($"[PC2] Überwachung gestartet: {localWatchPath}");
        }

        /// <summary>
        /// Wird ausgelöst, wenn im überwachten Ordner ein neuer Unterordner erstellt wird.
        /// </summary>
        /// <param name="folderPath">Der Pfad des neu erstellten Ordners.</param>
        private void OnFolderCreated(string folderPath)
        {
            logger.Log($"[PC2] Neuer digitalisierter Auftrag erkannt: {folderPath}");
            // Asynchrones Kopieren, damit der Überwachungs-Thread nicht blockiert wird.
            Task.Run(() =>
            {
                try
                {
                    // Zielpfad: Zielordner + Name des neu erstellten Ordners
                    string destPath = System.IO.Path.Combine(serverDestinationPath, System.IO.Path.GetFileName(folderPath));
                    folderTransferService.CopyFolder(folderPath, destPath);
                    logger.Log($"[PC2] Auftrag erfolgreich kopiert von '{folderPath}' nach '{destPath}'");
                }
                catch (Exception ex)
                {
                    logger.Log($"[PC2] Fehler beim Kopieren von '{folderPath}': {ex.Message}");
                }
            });
        }

        /// <summary>
        /// Beendet die Überwachung.
        /// </summary>
        public void Stop()
        {
            folderMonitor.Stop();
            logger.Log("[PC2] Überwachung gestoppt.");
        }
    }
}
