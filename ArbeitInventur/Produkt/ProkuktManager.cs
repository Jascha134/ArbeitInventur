using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    public class ProduktManager
    {
        private readonly string dateiPfad;

        public ProduktManager(string pfad = null)
        {
            dateiPfad = pfad ?? Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
        }

        // Asynchrones Laden mit Task.Run für .NET Framework
        public Task<List<T>> LadeDatenAsync<T>()
        {
            return Task.Run(() =>
            {
                if (!File.Exists(dateiPfad)) return new List<T>();

                try
                {
                    string json = File.ReadAllText(dateiPfad);
                    return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Laden der Daten: {ex.Message}");
                    return new List<T>();
                }
            });
        }

        // Asynchrones Speichern mit Task.Run für .NET Framework
        public Task SpeichereDatenAsync<T>(List<T> daten)
        {
            return Task.Run(() =>
            {
                try
                {
                    string json = JsonConvert.SerializeObject(daten, Formatting.Indented);
                    File.WriteAllText(dateiPfad, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler beim Speichern der Daten: {ex.Message}");
                }
            });
        }

        public Task<List<ProduktFirma>> LadeImplantatsystemeAsync() => LadeDatenAsync<ProduktFirma>();
        public Task SpeichereImplantatsystemeAsync(List<ProduktFirma> implantatsysteme) => SpeichereDatenAsync(implantatsysteme);
    }
}