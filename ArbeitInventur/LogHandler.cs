using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArbeitInventur
{
    public class LogHandler
    {
        private readonly string _logFilePath;
        private readonly Benutzer _benutzer;
        private List<LogEntry> _logEntries;
        private readonly object _fileLock = new object();

        public LogHandler(string logFilePath, Benutzer benutzer = null)
        {
            _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
            _benutzer = benutzer;
            _logEntries = new List<LogEntry>();
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }

        public List<LogEntry> LoadLogEntries()
        {
            lock (_fileLock)
            {
                try
                {
                    if (File.Exists(_logFilePath) && new FileInfo(_logFilePath).Length > 0)
                    {
                        string json = File.ReadAllText(_logFilePath);
                        if (!string.IsNullOrWhiteSpace(json))
                        {
                            _logEntries = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
                        }
                    }
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Fehler beim Lesen der Log-Datei: {ex.Message}. Starte mit leerer Liste.");
                    _logEntries = new List<LogEntry>();
                    string backupPath = _logFilePath + ".backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    File.Copy(_logFilePath, backupPath, true);
                    File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unerwarteter Fehler beim Laden der Log-Datei: {ex.Message}");
                    _logEntries = new List<LogEntry>();
                }
                return _logEntries.ToList();
            }
        }

        public List<LogEntry> GetTodayLogEntries()
        {
            lock (_fileLock)
            {
                return _logEntries.Where(entry => entry.Timestamp.Date == DateTime.Now.Date).ToList();
            }
        }

        public List<LogEntry> GetAllLogEntries()
        {
            lock (_fileLock)
            {
                return _logEntries.ToList();
            }
        }

        public void AddToLog(string message, string action)
        {
            lock (_fileLock)
            {
                try
                {
                    var logEntry = new LogEntry
                    {
                        Timestamp = DateTime.Now,
                        Benutzer = _benutzer,
                        Action = action,
                        Message = message
                    };
                    _logEntries.Add(logEntry);
                    File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
                }
            }
        }

        public void LogAction(string message)
        {
            AddToLog(message, "Action");
        }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public Benutzer Benutzer { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }
}