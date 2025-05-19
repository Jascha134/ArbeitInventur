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
        private List<ProduktFirma> cachedSystems;

        public ProduktManager(string pfad = null)
        {
            dateiPfad = pfad ?? Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
            cachedSystems = null;
        }

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

        public async Task<List<ProduktFirma>> LadeImplantatsystemeAsync()
        {
            if (cachedSystems != null)
            {
                return cachedSystems;
            }

            cachedSystems = await LadeDatenAsync<ProduktFirma>();
            return cachedSystems;
        }

        public async Task SpeichereImplantatsystemeAsync(List<ProduktFirma> implantatsysteme)
        {
            cachedSystems = implantatsysteme;
            await SpeichereDatenAsync(implantatsysteme);
        }
    }
}