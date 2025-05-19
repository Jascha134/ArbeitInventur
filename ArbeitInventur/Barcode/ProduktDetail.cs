using System;

namespace ArbeitInventur.Barcode
{
    public class ProduktDetail
    {
        public string Kategorie { get; set; }
        public string Beschreibung { get; set; }
        public int Menge { get; set; }
        public int Mindestbestand { get; set; }
        public string Barcode { get; set; }
        public string ProduktId { get; set; }
        public DateTime? Produktionsdatum { get; set; }
        public string LotNummer { get; set; }
    }
}