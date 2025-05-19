using ArbeitInventur.Barcode;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArbeitInventur
{
    public class ProduktFirma
    {
        public int SystemID { get; set; }
        public string SystemName { get; set; }

        private List<ProduktDetail> _details = new List<ProduktDetail>();

        [JsonProperty("Details")]
        public List<ProduktDetail> Details
        {
            get => _details;
            set => _details = value ?? new List<ProduktDetail>(); // Stelle sicher, dass Details nie null ist
        }
    }
}