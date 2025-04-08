using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArbeitInventur.UserInterface
{
    public class BenutzerVerwaltung
    {
        private List<Benutzer> benutzerListe = new List<Benutzer>();
        private readonly string jsonDateiName = "User.json";
        private readonly string jsonPfad;

        public BenutzerVerwaltung()
        {
            jsonPfad = Path.Combine(Properties.Settings.Default.DataJSON ?? @"C:\DefaultData", jsonDateiName);
        }

        // Benutzer hinzufügen
        public void BenutzerHinzufuegen(Benutzer benutzer)
        {
            if (benutzer == null) throw new ArgumentNullException(nameof(benutzer));
            benutzerListe.Add(benutzer);
            DatenSpeichern();
        }

        // Alle Benutzer speichern
        public void DatenSpeichern()
        {
            try
            {
                // Verzeichnis erstellen, falls es nicht existiert
                string directory = Path.GetDirectoryName(jsonPfad);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonConvert.SerializeObject(benutzerListe, Formatting.Indented);
                File.WriteAllText(jsonPfad, json);
            }
            catch (Exception ex)
            {
                // Fehler protokollieren, aber nicht abbrechen
                Console.WriteLine($"Fehler beim Speichern der Benutzerdaten: {ex.Message}");
            }
        }

        // Benutzer beim Programmstart laden oder erstellen
        public void DatenLaden()
        {
            try
            {
                if (File.Exists(jsonPfad))
                {
                    string json = File.ReadAllText(jsonPfad);
                    var loadedList = JsonConvert.DeserializeObject<List<Benutzer>>(json);
                    benutzerListe = loadedList ?? new List<Benutzer>();
                }
                else
                {
                    // Wenn die Datei nicht existiert, Standard-Benutzer erstellen
                    benutzerListe = new List<Benutzer>();
                    ErstelleStandardBenutzer();
                    DatenSpeichern();
                }
            }
            catch (Exception ex)
            {
                // Bei Fehlern (z. B. ungültiges JSON) Standard-Benutzer erstellen
                Console.WriteLine($"Fehler beim Laden der Benutzerdaten: {ex.Message}");
                benutzerListe = new List<Benutzer>();
                ErstelleStandardBenutzer();
                DatenSpeichern();
            }
        }

        // Standard-Benutzer erstellen
        private void ErstelleStandardBenutzer()
        {
            Benutzer admin = new Benutzer
            {
                Name = "Admin",
                Password = PasswortHashing.ErzeugePasswortHash("Scan3000"),
                Abteilungsnummer = 1,
                Abteilung = "Administration",
                rechte = new Rechte
                {
                    Einlagern = true,
                    auslagern = true
                }
            };
            benutzerListe.Add(admin);
        }

        // Methode, um die Benutzerliste zu erhalten
        public List<Benutzer> GetBenutzerListe()
        {
            return new List<Benutzer>(benutzerListe); // Rückgabe einer Kopie für Thread-Sicherheit
        }
    }
}