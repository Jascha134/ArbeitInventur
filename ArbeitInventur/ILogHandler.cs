using System.Collections.Generic;

namespace ArbeitInventur
{
    // Beispielhafte Schnittstelle – passe diese ggf. an Deine Vorgaben an.
    public interface ILogHandler
    {
        void AddToLog(string message, string action);
        void LogAction(string message);
        List<LogHandler.LogEntry> LoadLogs();
   }

}
