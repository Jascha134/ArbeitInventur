using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ArbeitInventur.UserInterface
{
    public class BenutzerVerwaltung
    {
        private List<Benutzer> benutzerListe = new List<Benutzer>();

        // JSON-Dateiname festlegen
        private string jsonDateiName = "User.json";

        // Benutzer hinzufügen
        public void BenutzerHinzufuegen(Benutzer benutzer)
        {
            benutzerListe.Add(benutzer);
            DatenSpeichern();
        }

        // Alle Benutzer speichern
        public void DatenSpeichern()
        {
            string pfad = Properties.Settings.Default.DataJSON + "\\" + jsonDateiName;
            string json = JsonConvert.SerializeObject(benutzerListe, Formatting.Indented);
            File.WriteAllText(pfad, json);
        }

        // Benutzer beim Programmstart laden oder erstellen
        public void DatenLaden()
        {
            string pfad = Properties.Settings.Default.DataJSON + "\\" + jsonDateiName; // Dateiname muss explizit sein

            // Prüfen, ob die JSON-Datei existiert
            if (File.Exists(pfad))
            {
                // Datei lesen und Benutzerliste aus der JSON-Datei laden
                string json = File.ReadAllText(pfad);
                benutzerListe = JsonConvert.DeserializeObject<List<Benutzer>>(json);
            }
            else
            {
                // Wenn die Datei nicht existiert, eine leere Benutzerliste erstellen und speichern
                benutzerListe = new List<Benutzer>();

                // Optional: Standard-Benutzer hinzufügen (z.B. Admin-Konto)
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
                BenutzerHinzufuegen(admin);

                // Daten speichern
                DatenSpeichern();
            }
        }

        // Methode, um die Benutzerliste zu erhalten
        public List<Benutzer> GetBenutzerListe()
        {
            return benutzerListe;
        }
    }
}
