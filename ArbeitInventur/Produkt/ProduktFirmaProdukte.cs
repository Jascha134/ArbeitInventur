using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    public class ProduktFirmaProdukte
    {
        public string Kategorie { get; set; } // Neue Eigenschaft zur Kategorisierung, steht jetzt vor Name
        public string Beschreibung { get; set; }
        public int Menge { get; set; }
        public int Mindestbestand { get; set; }
    }
}
