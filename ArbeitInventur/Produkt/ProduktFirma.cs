using System.Collections.Generic;

namespace ArbeitInventur
{
    public class ProduktFirma
    {
        public int SystemID { get; set; }
        public string SystemName { get; set; }
        public List<ProduktFirmaProdukte> Details { get; set; } = new List<ProduktFirmaProdukte>();
    }
}