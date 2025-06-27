using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArbeitInventur
{
    public class ProduktManager
    {
        private readonly string _implantatsystemePfad;
        private readonly Dictionary<string, int> _firmaStartNumbers = new Dictionary<string, int>();
        private const int START_NUMBER_INCREMENT = 10;

        public ProduktManager()
        {
            _implantatsystemePfad = Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
            LoadFirmaStartNumbers();
        }

        private void LoadFirmaStartNumbers()
        {
            if (File.Exists(_implantatsystemePfad))
            {
                var json = File.ReadAllText(_implantatsystemePfad);
                var systems = JsonConvert.DeserializeObject<List<ProduktFirma>>(json);
                if (systems != null)
                {
                    foreach (var system in systems)
                    {
                        if (!_firmaStartNumbers.ContainsKey(system.SystemName))
                        {
                            _firmaStartNumbers[system.SystemName] = system.IdStartNumber;
                        }
                    }
                }
            }
        }

        public async Task<List<ProduktFirma>> LadeImplantatsystemeAsync()
        {
            if (!File.Exists(_implantatsystemePfad))
                return new List<ProduktFirma>();

            var json = await FileAsyncHelper.ReadAllTextAsync(_implantatsystemePfad);
            var systems = JsonConvert.DeserializeObject<List<ProduktFirma>>(json) ?? new List<ProduktFirma>();

            // Migration für bestehende Produkte ohne ID
            foreach (var system in systems)
            {
                if (!_firmaStartNumbers.ContainsKey(system.SystemName))
                {
                    int nextStartNumber = _firmaStartNumbers.Values.DefaultIfEmpty(0).Max() + START_NUMBER_INCREMENT;
                    _firmaStartNumbers[system.SystemName] = nextStartNumber;
                    system.IdStartNumber = nextStartNumber;
                }

                if (system.Details != null)
                {
                    int nextId = system.IdStartNumber;
                    foreach (var product in system.Details.Where(p => p.Id == 0))
                    {
                        product.Id = nextId++;
                    }
                }
            }

            return systems;
        }

        public async Task SpeichereImplantatsystemeAsync(List<ProduktFirma> implantatsysteme)
        {
            var json = JsonConvert.SerializeObject(implantatsysteme, Formatting.Indented);
            await FileAsyncHelper.WriteAllTextAsync(_implantatsystemePfad, json);
        }

        public int GenerateProductId(ProduktFirma firma)
        {
            if (!_firmaStartNumbers.ContainsKey(firma.SystemName))
            {
                int nextStartNumber = _firmaStartNumbers.Values.DefaultIfEmpty(0).Max() + START_NUMBER_INCREMENT;
                _firmaStartNumbers[firma.SystemName] = nextStartNumber;
                firma.IdStartNumber = nextStartNumber;
            }

            int maxId = firma.Details?.Select(d => d.Id).DefaultIfEmpty(firma.IdStartNumber - 1).Max() ?? (firma.IdStartNumber - 1);
            return maxId + 1;
        }
    }
}