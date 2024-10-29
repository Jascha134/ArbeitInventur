using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArbeitInventur
{
    public class ProduktManager
    {
        private readonly string dateiPfad;

        // Konstruktor mit Standard- oder benutzerdefiniertem Pfad
        public ProduktManager(string pfad = null)
        {
            dateiPfad = pfad ?? Properties.Settings.Default.DataJSON + "\\implantatsysteme.json"; // Standardpfad
        }

        // Generische Methode zum Laden von Daten aus einer JSON-Datei
        public List<T> LadeDaten<T>()
        {
            if (File.Exists(dateiPfad))
            {
                try
                {
                    string json = File.ReadAllText(dateiPfad);
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>(); // Leere Liste als Fallback
                }
                catch (Exception ex)
                {
                    // Fehlerprotokollierung (Falls gewünscht)
                    Console.WriteLine($"Fehler beim Laden der Daten: {ex.Message}");
                    return new List<T>(); // Leere Liste bei Fehlern
                }
            }
            return new List<T>(); // Leere Liste, falls Datei fehlt
        }

        // Generische Methode zum Speichern von Daten in einer JSON-Datei
        public void SpeichereDaten<T>(List<T> daten)
        {
            try
            {
                string json = JsonConvert.SerializeObject(daten, Formatting.Indented);
                File.WriteAllText(dateiPfad, json);
            }
            catch (Exception ex)
            {
                // Fehlerprotokollierung (Falls gewünscht)
                Console.WriteLine($"Fehler beim Speichern der Daten: {ex.Message}");
            }
        }

        // Beispielmethoden für spezifische Typen
        public List<ProduktFirma> LadeImplantatsysteme() => LadeDaten<ProduktFirma>();

        public void SpeichereImplantatsysteme(List<ProduktFirma> implantatsysteme) => SpeichereDaten(implantatsysteme);
    }
}
