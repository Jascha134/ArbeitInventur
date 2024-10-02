using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    using System.Collections.Generic;

    public class ImplantatSystem
    {
        // Eindeutige ID für jedes Implantatsystem
        public int SystemID { get; set; }

        // Der Name des Implantatsystems, z.B. "Straumann"
        public string SystemName { get; set; }

        // Eine Liste von Details für dieses System
        public List<ImplantatSystemDetails> Details { get; set; }
    }

}
