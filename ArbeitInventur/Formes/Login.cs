using ArbeitInventur.Exocad_Help;
using ArbeitInventur.Formes;
using ArbeitInventur.UserInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class Login : Form
    {
        public static Login Instance { get; set; }
        private Timer errorTimer;
        private BenutzerVerwaltung benutzerVerwaltung;
        public Login()
        {
            Instance = this;
            InitializeComponent();
            labelError.Text = "";

            errorTimer = new Timer();
            errorTimer.Interval = 1500; // 2 Sekunden (2000 Millisekunden)
            errorTimer.Tick += ErrorTimer_Tick;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Login_KeyDown);

            benutzerVerwaltung = new BenutzerVerwaltung();
            benutzerVerwaltung.DatenLaden();

            FuelleComboBoxMitBenutzernamen();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Eingegebenen Benutzernamen und Passwort aus den Steuerelementen holen
            string benutzerName = comboBoxBenutzername.SelectedItem?.ToString();
            string passwort = textBoxPasswort.Text;

            // Überprüfe, ob der Benutzername und das Passwort gültig sind
            // Suche den Benutzer in der Benutzerliste
            Benutzer benutzer = benutzerVerwaltung.GetBenutzerListe().FirstOrDefault(b => b.Name == benutzerName);

            if (benutzer != null)
            {
                if (PasswortHashing.UeberpruefePasswort(passwort, benutzer.Password))
                {
                    // Wenn der Login erfolgreich ist, speichern wir den Benutzernamen automatisch
                    SpeichereBenutzernamen(benutzerName);
                    labelError.ForeColor = Color.Green;

                    // Zeige eine Erfolgsmeldung an oder navigiere zur Hauptanwendung
                    labelError.Text = "Login erfolgreich!";
                    // Weiteres Verhalten nach dem erfolgreichen Login (z.B. das Hauptfenster anzeigen)

                    Main mainForm = new Main(benutzer);
                    mainForm.ShowDialog();
                }
                else
                {
                    labelError.ForeColor = Color.Red;
                    // Wenn der Login fehlschlägt, zeige eine Fehlermeldung an
                    labelError.Text = "Benutzername oder Passwort ist falsch.";
                }
            }
            else
            {
                labelError.ForeColor = Color.Red;
                // Wenn der Login fehlschlägt, zeige eine Fehlermeldung an
                labelError.Text = "Benutzername nicht gefunden";
            }
            errorTimer.Start();
        }
        private void ErrorTimer_Tick(object sender, EventArgs e)
        {
            // Text des Labels auf leer setzen
            labelError.Text = "";

            // Timer stoppen, damit er nicht erneut feuert
            errorTimer.Stop();
        }
        private void SpeichereBenutzernamen(string benutzerName)
        {
            Properties.Settings.Default.BenutzerGemerkt = benutzerName;
            Properties.Settings.Default.Save();  // Speichern der Einstellungen
        }
        private void FuelleComboBoxMitBenutzernamen()
        {
            comboBoxBenutzername.Items.Clear();

            // Benutzerliste aus der BenutzerVerwaltung holen
            List<Benutzer> benutzerListe = benutzerVerwaltung.GetBenutzerListe();

            // Jeden Benutzernamen zur ComboBox hinzufügen
            foreach (var benutzer in benutzerListe)
            {
                comboBoxBenutzername.Items.Add(benutzer.Name);
            }

            // Prüfen, ob ein Benutzername gespeichert ist und diesen auswählen
            string gespeicherterBenutzername = Properties.Settings.Default.BenutzerGemerkt;

            if (!string.IsNullOrWhiteSpace(gespeicherterBenutzername) && comboBoxBenutzername.Items.Contains(gespeicherterBenutzername))
            {
                comboBoxBenutzername.SelectedItem = gespeicherterBenutzername;
            }
            else if (comboBoxBenutzername.Items.Count > 0)
            {
                comboBoxBenutzername.SelectedIndex = 0;
            }
        }
        private bool ÜberprüfeLoginDaten(string benutzerName, string passwort)
        {
            // Benutzerliste aus der BenutzerVerwaltung holen
            List<Benutzer> benutzerListe = benutzerVerwaltung.GetBenutzerListe();

            // Benutzer mit dem eingegebenen Namen suchen
            foreach (var benutzer in benutzerListe)
            {
                // Prüfen, ob der Benutzername übereinstimmt
                if (benutzer.Name == benutzerName)
                {
                    // Passwort verschlüsseln, bevor wir es mit dem gespeicherten vergleichen
                    string verschluesseltesPasswort = PasswortHashing.ErzeugePasswortHash(passwort);

                    // Prüfen, ob das verschlüsselte Passwort übereinstimmt
                    if (benutzer.Password == verschluesseltesPasswort)
                    {
                        return true; // Login erfolgreich
                    }
                }
            }
            return false; // Benutzername oder Passwort war falsch
        }

        private void btnRegistrieren_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registrierung registrierungsForm = new Registrierung();
            registrierungsForm.ShowDialog();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            // Überprüfen, ob die Enter-Taste gedrückt wurde
            if (e.KeyCode == Keys.Enter)
            {
                // Das Click-Ereignis für den Button auslösen
                btnLogin.PerformClick();

                // Optional: Event als verarbeitet markieren
                e.Handled = true;
            }
        }
    }
}
