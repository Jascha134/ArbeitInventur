using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.ScannÜber
{
    /// <summary>
    /// Einfache Logger-Klasse, die Nachrichten in eine Log-Datei schreibt.
    /// </summary>
    public class Logger
    {
        private readonly string logFilePath;

        public Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(logFilePath, entry + Environment.NewLine);
        }
    }
}
