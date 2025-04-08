using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public class LogHandler
    {
        private readonly string _logFilePath;
        private List<LogEntry> _logEntries;

        // Konstruktor benötigt nur den Pfad zur Logdatei
        public LogHandler(string logFilePath)
        {
            _logFilePath = logFilePath;
            _logEntries = new List<LogEntry>();

            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }

        // Lädt vorhandene Logeinträge aus der Datei
        public List<LogEntry> LoadLogEntries()
        {
            if (File.Exists(_logFilePath))
            {
                string json = File.ReadAllText(_logFilePath);
                return JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            return new List<LogEntry>();
        }


        // Gibt alle Logeinträge des aktuellen Tages zurück
        public List<LogEntry> GetTodayLogEntries()
        {
            return _logEntries.Where(entry => entry.Timestamp.Date == DateTime.Now.Date).ToList();
        }
        public List<LogEntry> GetAllLogEntries()
        {
            return _logEntries; // _logEntries wurde ja beim Laden der Logdatei befüllt
        }

        // Fügt einen neuen Logeintrag hinzu und speichert die Datei
        public void AddToLog(string message, string action)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Action = action,
                Message = message
            };

            _logEntries.Add(logEntry);
            File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
        }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }

        // Alias für die alte Bezeichnung "Handlung"
        public string Handlung
        {
            get { return Action; }
            set { Action = value; }
        }
    }

}
