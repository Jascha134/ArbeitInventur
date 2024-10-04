using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ArbeitInventur
{
    public class ImplantatManager
    {
        private string dateiPfad = Properties.Settings.Default.DataJSON + "\\implantatsysteme.json"; // Der Pfad zur JSON-Datei

        // Implantatsysteme aus der JSON-Datei laden
        public List<ImplantatSystem> LadeImplantatsysteme()
        {
            if (File.Exists(dateiPfad)) // Prüfen, ob die Datei existiert
            {
                string json = File.ReadAllText(dateiPfad); // JSON-Inhalt lesen
                return JsonConvert.DeserializeObject<List<ImplantatSystem>>(json); // In Liste von Implantatsystemen umwandeln
            }
            return new List<ImplantatSystem>(); // Leere Liste zurückgeben, wenn Datei nicht existiert
        }

        // Implantatsysteme in der JSON-Datei speichern
        public void SpeichereImplantatsysteme(List<ImplantatSystem> implantatsysteme)
        {
            string json = JsonConvert.SerializeObject(implantatsysteme,Newtonsoft.Json.Formatting.Indented); // Implantatsysteme in JSON-Format umwandeln
            File.WriteAllText(dateiPfad, json); // JSON-Inhalt in die Datei schreiben
        }
    }

}
