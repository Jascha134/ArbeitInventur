using ArbeitInventur.UserInterface;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class Registrierung : Form
    {
        private BenutzerVerwaltung benutzerVerwaltung;

        public Registrierung()
        {
            InitializeComponent();


            labelError.Text = "";

            // Initialisiere die BenutzerVerwaltung im Konstruktor
            benutzerVerwaltung = new BenutzerVerwaltung();

            // Optionale Datenladung (falls du bereits Daten aus JSON hast)
            benutzerVerwaltung.DatenLaden();
            FuelleComboBoxAbteilung();
        }

        private void btnRegistrieren_Click(object sender, EventArgs e)
        {

            // Daten aus den TextBoxen und ComboBox holen
            string benutzerName = textBoxBenutzer.Text.Trim();
            string personalID = textBoxPersonalnummer.Text.Trim();
            string password = textBoxPassword.Text;
            string passwordWiederholen = textBoxPasswordwiederholen.Text;
            string abteilung = comboBoxAbteilung.SelectedItem?.ToString();

            // Überprüfen, ob alle Felder ausgefüllt sind
            if (string.IsNullOrWhiteSpace(benutzerName) || string.IsNullOrWhiteSpace(personalID) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordWiederholen))
            {
                labelError.Text = "Alle Felder müssen ausgefüllt werden.";
                return;
            }

            // Überprüfen, ob eine Abteilung ausgewählt wurde
            if (abteilung == null)
            {
                labelError.Text = "Bitte eine Abteilung auswählen.";
                return;
            }

            // Überprüfen, ob die Passwörter übereinstimmen
            if (password != passwordWiederholen)
            {
                labelError.Text = "Die Passwörter stimmen nicht überein.";
                textBoxPassword.Clear();
                textBoxPasswordwiederholen.Clear();
                return;
            }

            // Überprüfen, ob das Passwort mindestens 6 Zeichen lang ist
            if (password.Length < 6)
            {
                labelError.Text = "Das Passwort muss mindestens 6 Zeichen lang sein.";
                return;
            }

            // Überprüfung auf doppelten Benutzernamen
            if (BenutzerExistiert(benutzerName))
            {
                labelError.Text = "Der Benutzername ist bereits vergeben.";
                return;
            }
            // Überprüfung auf doppelte Personalnummer
            if (PersonalnummerExistiert(personalID))
            {
                labelError.Text = "Die Personalnummer ist bereits vergeben.";
                return;
            }

            // Rechte für den neuen Benutzer definieren
            Rechte rechte = new Rechte
            {
                Einlagern = true,
                auslagern = true,
            };

            // Passwort hashen
            string passwortHash = PasswortHashing.ErzeugePasswortHash(password);

            // Neuen Benutzer erstellen und das Passwort verschlüsseln
            Benutzer neuerBenutzer = new Benutzer
            {
                Name = benutzerName,
                Password = passwortHash,
                Abteilungsnummer = int.Parse(personalID),
                Abteilung = abteilung,
                rechte = rechte
            };

            // Neuen Benutzer zur Benutzerverwaltung hinzufügen und Daten speichern
            benutzerVerwaltung.BenutzerHinzufuegen(neuerBenutzer);

            // Erfolgreiche Registrierung anzeigen
            labelError.Text = "Registrierung erfolgreich!";
            ClearFields();
            this.Close();
            Application.Restart();
        }
        private void FuelleComboBoxAbteilung()
        {
            // Liste der Abteilungen
            List<string> abteilungen = new List<string>
            {
            "Verwaltung",
            "Zahntechniker"
            };

            // ComboBox leeren, falls sie bereits gefüllt ist
            comboBoxAbteilung.Items.Clear();

            // Abteilungen zur ComboBox hinzufügen
            foreach (var abteilung in abteilungen)
            {
                comboBoxAbteilung.Items.Add(abteilung);
            }

            // Optional: Wähle eine Standardabteilung aus (z.B. die erste in der Liste)
            if (comboBoxAbteilung.Items.Count > 0)
            {
                comboBoxAbteilung.SelectedIndex = 0; // Erste Abteilung auswählen
            }
        }
        private bool BenutzerExistiert(string benutzerName)
        {
            List<Benutzer> benutzerListe = benutzerVerwaltung.GetBenutzerListe();

            foreach (var benutzer in benutzerListe)
            {
                if (benutzer.Name.Equals(benutzerName, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Benutzername existiert bereits
                }
            }

            return false; // Benutzername ist frei
        }
        // Methode zur Überprüfung, ob der Personalnummer bereits existiert
        private bool PersonalnummerExistiert(string personalnummer)
        {
            List<Benutzer> benutzerListe = benutzerVerwaltung.GetBenutzerListe();

            foreach (var benutzer in benutzerListe)
            {
                if (benutzer.Abteilungsnummer.ToString().Equals(personalnummer, StringComparison.OrdinalIgnoreCase))
                {
                    return true; // Benutzername existiert bereits
                }
            }

            return false; // Benutzername ist frei
        }

        // Methode zum Zurücksetzen der Felder nach erfolgreicher Registrierung
        private void ClearFields()
        {
            textBoxBenutzer.Clear();
            textBoxPersonalnummer.Clear();
            textBoxPassword.Clear();
            textBoxPasswordwiederholen.Clear();
            comboBoxAbteilung.SelectedIndex = -1;
        }
    }
}
