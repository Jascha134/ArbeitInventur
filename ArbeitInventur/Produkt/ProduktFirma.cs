using System.Collections.Generic;

namespace ArbeitInventur
{
    public class ProduktFirma
    {
        public string SystemName { get; set; }
        public int IdStartNumber { get; set; }
        public string LieferantEmail { get; set; }
        public string KontaktPerson { get; set; }
        public string Lieferadresse { get; set; }
        public List<ProduktDetail> Details { get; set; }
    }
}