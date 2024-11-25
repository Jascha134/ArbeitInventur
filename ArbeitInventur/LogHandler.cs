using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public class LogHandler
    {
        private readonly string logFilePath;
        private readonly Benutzer Benutzer;

        public LogHandler(string path, Benutzer benutzer)
        {
            logFilePath = path;
            Benutzer = benutzer;
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        // Methode zum Laden aller Log-Einträge (optional, z.B. zur Anzeige in der UI)
        public List<LogEntry> LoadLogs()
        {
            try
            {
                if (File.Exists(logFilePath) && new FileInfo(logFilePath).Length > 0)
                {
                    string existingLogs = File.ReadAllText(logFilePath);
                    return JsonConvert.DeserializeObject<List<LogEntry>>(existingLogs) ?? new List<LogEntry>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Log-Einträge: " + ex.Message);
            }

            return new List<LogEntry>();
        }

        // Methode zum Protokollieren einer Handlung
        public void LogAction(string actionDescription)
        {
            LogAction(new LogEntry
            {
                Timestamp = DateTime.Now,
                Handlung = actionDescription,
                Benutzer = Benutzer
            });
        }

        // Überladene Methode zum Loggen mit zusätzlichen Informationen
        public void LogAction(LogEntry logEntry)
        {
            try
            {
                List<LogEntry> logEntries;

                if (new FileInfo(logFilePath).Length > 0)
                {
                    string existingLogs = File.ReadAllText(logFilePath);
                    logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(existingLogs) ?? new List<LogEntry>();
                }
                else
                {
                    logEntries = new List<LogEntry>();
                }

                // Füge den neuen Eintrag hinzu und speichere ihn
                logEntries.Add(logEntry);
                string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                File.WriteAllText(logFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Protokollieren der Aktion: " + ex.Message);
            }
        }

        // Klasse für die Log-Einträge
        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public Benutzer Benutzer { get; set; }
            public string Handlung { get; set; }
        }
    }
}
