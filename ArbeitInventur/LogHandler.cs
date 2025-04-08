using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArbeitInventur
{
    public class LogHandler
    {
        private readonly string logFilePath;
        private readonly Benutzer benutzer;
        private readonly object fileLock = new object();

        public LogHandler(string path, Benutzer benutzer)
        {
            this.logFilePath = path;
            this.benutzer = benutzer;
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        // Synchrones Laden, da keine echte asynchrone Dateioperation verfügbar
        public List<LogEntry> LoadLogs()
        {
            lock (fileLock)
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
                    Console.WriteLine($"Fehler beim Laden der Log-Einträge: {ex.Message}");
                }
                return new List<LogEntry>();
            }
        }

        public void LogAction(string actionDescription)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Handlung = actionDescription,
                Benutzer = benutzer
            };
            LogAction(logEntry);
        }

        private void LogAction(LogEntry logEntry)
        {
            lock (fileLock)
            {
                try
                {
                    List<LogEntry> logEntries = new FileInfo(logFilePath).Length > 0
                        ? JsonConvert.DeserializeObject<List<LogEntry>>(File.ReadAllText(logFilePath)) ?? new List<LogEntry>()
                        : new List<LogEntry>();

                    logEntries.Add(logEntry);
                    string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
                    File.WriteAllText(logFilePath, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Protokollieren der Aktion: {ex.Message}");
                }
            }
        }

        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public Benutzer Benutzer { get; set; }
            public string Handlung { get; set; }
        }
    }
}