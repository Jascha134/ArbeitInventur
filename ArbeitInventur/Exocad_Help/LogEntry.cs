using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur.Exocad_Help
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Handlung { get; set; }
        public string Message { get; set; }
        public Benutzer Benutzer { get; set; }
    }
}
