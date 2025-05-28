using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if (string.IsNullOrEmpty(logFilePath))
                throw new ArgumentNullException(nameof(logFilePath));

            _logFilePath = logFilePath;
            _benutzer = benutzer;
            _logEntries = new List<LogEntry>();

            // Erstelle das Verzeichnis, falls es nicht existiert
            string directory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Erstelle die Datei, falls sie nicht existiert
            if (!File.Exists(_logFilePath))
            {
                File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
            }

            // Lade vorhandene Einträge
            _logEntries = LoadLogEntries();
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
                            // Deserialisiere JSON manuell, um alte und neue Formate zu unterstützen
                            var jArray = JArray.Parse(json);
                            var entries = new List<LogEntry>();

                            foreach (var jObject in jArray)
                            {
                                var entry = new LogEntry
                                {
                                    Timestamp = jObject["Timestamp"]?.ToObject<DateTime>() ?? DateTime.MinValue,
                                    BenutzerName = jObject["BenutzerName"]?.ToString(),
                                    Action = jObject["Action"]?.ToString(),
                                    Message = jObject["Message"]?.ToString()
                                };

                                // Unterstütze altes Format mit Benutzer-Objekt
                                if (string.IsNullOrEmpty(entry.BenutzerName) && jObject["Benutzer"] != null)
                                {
                                    entry.BenutzerName = jObject["Benutzer"]?["Name"]?.ToString();
                                }

                                // Überspring ungültige Einträge
                                if (entry.Timestamp != DateTime.MinValue && !string.IsNullOrEmpty(entry.Action))
                                {
                                    entries.Add(entry);
                                }
                            }

                            return entries;
                        }
                    }
                    return new List<LogEntry>();
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"Fehler beim Lesen der Log-Datei: {ex.Message}. Starte mit leerer Liste, Datei bleibt unverändert.");
                    string backupPath = _logFilePath + ".backup_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (File.Exists(_logFilePath))
                    {
                        File.Copy(_logFilePath, backupPath, true);
                    }
                    return new List<LogEntry>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unerwarteter Fehler beim Laden der Log-Datei: {ex.Message}");
                    return new List<LogEntry>();
                }
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
                    if (_benutzer == null)
                    {
                        Console.WriteLine("Warnung: Benutzer ist null. Logeintrag wird nicht erstellt.");
                        return;
                    }

                    var logEntry = new LogEntry
                    {
                        Timestamp = DateTime.Now,
                        BenutzerName = _benutzer.Name,
                        Action = action,
                        Message = message
                    };
                    _logEntries.Add(logEntry);
                    File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(_logEntries, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
                    string errorLogPath = Path.Combine(Path.GetDirectoryName(_logFilePath), "error.log");
                    File.AppendAllText(errorLogPath, $"{DateTime.Now}: Fehler beim Schreiben in Log-Datei: {ex.Message}\n");
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
        public string BenutzerName { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
    }
}