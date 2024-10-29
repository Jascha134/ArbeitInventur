using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    public class Benutzer
    {
        public string Name { get; set; }
        public int Abteilungsnummer { get; set; }
        public string Password { get; set; }   
        public string Abteilung { get; set; }

        public Rechte rechte { get; set; }
    }
    public class Rechte
    {
        public bool Einlagern { get; set; }
        public bool auslagern { get; set; }
    }
}
