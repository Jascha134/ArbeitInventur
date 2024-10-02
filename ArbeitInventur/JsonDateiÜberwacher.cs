using System;
using System.IO;

namespace ArbeitInventur
{
    public class JsonDateiÜberwacher
    {
        private FileSystemWatcher fileWatcher;

        // Event, das ausgelöst wird, wenn eine Änderung erkannt wird
        public event Action DateiGeändert;

        public JsonDateiÜberwacher(string dateiPfad)
        {
            // Überprüfen, ob der übergebene Pfad gültig ist
            if (string.IsNullOrWhiteSpace(dateiPfad))
            {
                // Wenn der Pfad leer ist oder nur aus Leerzeichen besteht, keine Ausnahme werfen
                // Die Überwachung wird in diesem Fall einfach nicht gestartet
                return;
            }

            if (!File.Exists(dateiPfad))
            {
                throw new FileNotFoundException("Die angegebene Datei wurde nicht gefunden.", dateiPfad);
            }

            // FileSystemWatcher konfigurieren
            fileWatcher = new FileSystemWatcher
            {
                Path = Path.GetDirectoryName(dateiPfad),
                Filter = Path.GetFileName(dateiPfad),
                NotifyFilter = NotifyFilters.LastWrite, // Überwache nur Änderungen im Inhalt der Datei
                EnableRaisingEvents = true // Überwachung aktivieren
            };

            // Event-Handler für Änderungen registrieren
            fileWatcher.Changed += OnDateiGeändert;
        }

        private void OnDateiGeändert(object sender, FileSystemEventArgs e)
        {
            // Kurze Verzögerung, um sicherzustellen, dass die Datei vollständig geschrieben wurde
            System.Threading.Thread.Sleep(500);

            // Überprüfen, ob ein Abonnent vorhanden ist und das Event auslösen
            DateiGeändert?.Invoke();
        }

        // Methode zum Stoppen der Überwachung
        public void StopÜberwachung()
        {
            if (fileWatcher != null)
            {
                fileWatcher.EnableRaisingEvents = false;
            }
        }

        // Methode zum Starten der Überwachung
        public void StartÜberwachung()
        {
            if (fileWatcher != null)
            {
                fileWatcher.EnableRaisingEvents = true;
            }
        }
    }


}
