using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class Form1 : Form
    {
        private string implantatsystem = Properties.Settings.Default.DataJSON + "\\implantatsysteme.json"; // Pfad zur JSON-Datei auf dem Server
        private string logg = Properties.Settings.Default.DataJSON + "\\log.json";

        private ProkuktManager manager = new ProkuktManager();
        private List<ProduktFirma> implantatsysteme = new List<ProduktFirma>();
        private Benutzer benutzer;
        private LogHandler logHandler;

        public Form1(Benutzer benutzer)
        {
            InitializeComponent();
            this.benutzer = benutzer;
            Benutzerauswachl.Instance.Hide();
            DGVSettings();
            originalControls = panel1.Controls.Cast<Control>().ToArray();
            this.MinimumSize = new Size(1208, 759);
        }
        private void DGVSettings()
        {
            dataGridView2.CellFormatting += dataGridView2_CellFormatting;
            dataGridView2.CellClick += dataGridView2_CellClick;
            DGVStyle.Dgv(dataGridView1);
            DGVStyle.Dgv(dataGridView2);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            lb_Benutzer.Text = "Benutzer : " + benutzer.Name;
            // Implantatsysteme laden
            implantatsysteme = manager.LadeImplantatsysteme();

            // Daten der Implantatsysteme in der ersten DataGridView anzeigen (ohne SystemID)
            dataGridView1.DataSource = implantatsysteme.Select(system => new
            {
                system.SystemName
            }).ToList();
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Die aktuell ausgewählte Zeile erhalten
                var selectedRow = dataGridView1.CurrentRow;

                // Den Systemnamen des ausgewählten Systems aus der Zeile erhalten
                string selectedSystemName = selectedRow.Cells["SystemName"].Value.ToString();

                // Das ausgewählte Implantatsystem anhand des Namens finden
                var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == selectedSystemName);

                if (selectedSystem != null)
                {
                    // Details des ausgewählten Systems in der zweiten DataGridView anzeigen
                    dataGridView2.DataSource = null;
                    dataGridView2.DataSource = selectedSystem.Details.ToList();
                    // Mindestbestand-Spalte ausblenden
                    MindestbestandSpalteAusblenden();
                }
            }
        }
        private ProduktFirmaProdukte aktuellBearbeitetesDetail = null; // Speichert das Detail, das bearbeitet wird
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                // Den Systemnamen des ausgewählten Systems erhalten
                string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();

                // Das ausgewählte Implantatsystem anhand des Namens finden
                var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == selectedSystemName);

                if (selectedSystem != null)
                {
                    try
                    {
                        logHandler = new LogHandler(logg, benutzer);
                        // Neue Detailinformationen sammeln
                        string kategorie = textBoxKategorie.Text;
                        string beschreibung = textBoxBeschreibung.Text;
                        int menge = int.Parse(textBoxMenge.Text);
                        int mindestbestand = int.Parse(textBoxMindestbestand.Text);

                        // Überprüfen, ob der Detailname leer ist
                        if (string.IsNullOrWhiteSpace(beschreibung))
                        {
                            MessageBox.Show("Bitte geben Sie einen gültigen Namen für das Detail ein.");
                            return;
                        }

                        if (aktuellBearbeitetesDetail == null)
                        {
                            // Neues Detail für das Implantatsystem erstellen (Hinzufügen)
                            var neuesDetail = new ProduktFirmaProdukte
                            {
                                Kategorie = kategorie,
                                Beschreibung = beschreibung,
                                Menge = menge,
                                Mindestbestand = mindestbestand
                            };

                            // Detail zur Liste der Details des ausgewählten Systems hinzufügen
                            selectedSystem.Details.Add(neuesDetail);
                            logHandler.LogAction($"Hinzugefügt : {neuesDetail.Kategorie} {neuesDetail.Beschreibung} || Menge: {neuesDetail.Menge} || Mindestbestand: {neuesDetail.Mindestbestand} ");
                        }
                        else
                        {
                            // Berechne die Veränderung der Menge
                            int differenzMenge = menge - aktuellBearbeitetesDetail.Menge;

                            // Bearbeitung des ausgewählten Details (Bearbeiten)
                            aktuellBearbeitetesDetail.Kategorie = kategorie;
                            aktuellBearbeitetesDetail.Beschreibung = beschreibung;
                            aktuellBearbeitetesDetail.Menge = menge;
                            aktuellBearbeitetesDetail.Mindestbestand = mindestbestand;

                            // Log-Nachricht anpassen je nach Veränderung der Menge (Einlagerung oder Auslagerung)
                            string logMessage;

                            if (differenzMenge > 0)
                            {
                                logMessage = $"Eingelagert: {aktuellBearbeitetesDetail.Beschreibung} | " +
                                             $"Eingelagerte Menge: {differenzMenge} | " +
                                             $"Neuer Bestand: {aktuellBearbeitetesDetail.Menge} | " ;
                            }
                            else if (differenzMenge < 0)
                            {
                                logMessage = $"Ausgelagert: {aktuellBearbeitetesDetail.Beschreibung} | " +
                                             $"Ausgelagerte Menge: {-differenzMenge} | " + // Differenz als positive Zahl anzeigen
                                             $"Verbleibender Bestand: {aktuellBearbeitetesDetail.Menge} | ";
                            }
                            else
                            {
                                logMessage = $"Bearbeitet: {aktuellBearbeitetesDetail.Beschreibung} | " +
                                             $"Kategorie: {aktuellBearbeitetesDetail.Kategorie} | " +
                                             $"Keine Mengenänderung | " +
                                             $"Bestand: {aktuellBearbeitetesDetail.Menge} | ";
                            }

                            logHandler.LogAction(logMessage);

                            // Nach der Bearbeitung aufheben
                            aktuellBearbeitetesDetail = null;
                        }

                        // Änderungen speichern
                        manager.SpeichereImplantatsysteme(implantatsysteme);

                        // DataGridView für Details aktualisieren
                        dataGridView2.DataSource = null;
                        dataGridView2.DataSource = selectedSystem.Details.ToList();

                        // Mindestbestand-Spalte ausblenden
                        MindestbestandSpalteAusblenden();

                        // Eingabefelder leeren
                        textBoxBeschreibung.Clear();
                        textBoxMenge.Clear();
                        textBoxMindestbestand.Clear();
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Bitte geben Sie gültige Werte für Menge und Mindestbestand ein.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie ein Implantatsystem aus, um Details hinzuzufügen oder zu bearbeiten.");
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            aktuellBearbeitetesDetail = null ;
            // Überprüfen, ob eine gültige Zeile ausgewählt wurde
            if (e.RowIndex >= 0)
            {
                // Erhalte die aktuelle Zeile
                var selectedRow = dataGridView2.Rows[e.RowIndex];

                // Werte der aktuellen Zeile in die Textboxen übertragen
                textBoxKategorie.Text = selectedRow.Cells["Kategorie"].Value.ToString();
                textBoxBeschreibung.Text = selectedRow.Cells["Beschreibung"].Value.ToString();
                textBoxMenge.Text = selectedRow.Cells["Menge"].Value.ToString();
                textBoxMindestbestand.Text = selectedRow.Cells["Mindestbestand"].Value.ToString();

                // Das ausgewählte Detail finden und für die Bearbeitung speichern
                string detailBeschreibung = selectedRow.Cells["Beschreibung"].Value.ToString();
                var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString());
                if (selectedSystem != null)
                {
                    aktuellBearbeitetesDetail = selectedSystem.Details.FirstOrDefault(detail => detail.Beschreibung == detailBeschreibung);
                }
            }
        }
        private ProduktFirma aktuellBearbeitetesSystem = null; // Speichert das System, das bearbeitet wird
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Initialisiere den LogHandler, falls er noch nicht gesetzt wurde
                if (logHandler == null)
                {
                    logHandler = new LogHandler(logg, benutzer);
                }

                // Eingabe für den Systemnamen sammeln und validieren
                string systemName = textBoxSystemName.Text.Trim();
                if (string.IsNullOrWhiteSpace(systemName))
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Systemnamen ein.", "Ungültiger Systemname", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Prüfen, ob wir ein neues System hinzufügen oder ein bestehendes bearbeiten
                if (aktuellBearbeitetesSystem == null)
                {
                    // Neues System erstellen (Hinzufügen)
                    int newSystemID = GeneriereNeueSystemID();

                    var neuesSystem = new ProduktFirma
                    {
                        SystemID = newSystemID,
                        SystemName = systemName,
                        Details = new List<ProduktFirmaProdukte>() // Leere Liste für Details
                    };

                    // Neues System zur Liste der Implantatsysteme hinzufügen
                    implantatsysteme.Add(neuesSystem);

                    // Log-Nachricht für das Hinzufügen
                    logHandler.LogAction($"{neuesSystem.SystemName} wurde zur Liste hinzugefügt (SystemID: {neuesSystem.SystemID})");
                }
                else
                {
                    // Vorherigen Systemnamen speichern, um die Änderung zu protokollieren
                    string alterSystemName = aktuellBearbeitetesSystem.SystemName;

                    // Systemname aktualisieren
                    aktuellBearbeitetesSystem.SystemName = systemName;

                    // Log-Nachricht für die Änderung
                    logHandler.LogAction($"Systemname geändert: {alterSystemName} wurde zu {systemName}");

                    // Bearbeitung aufheben
                    aktuellBearbeitetesSystem = null;
                }

                // Änderungen speichern und UI aktualisieren
                SpeichereUndAktualisiereUI();

                // Eingabefelder leeren
                LeereEingabefelder();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ungültiges Format eingegeben. Bitte überprüfen Sie die Eingabewerte.", "Formatfehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ein unerwarteter Fehler ist aufgetreten: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Methode zum Generieren einer neuen SystemID
        private int GeneriereNeueSystemID()
        {
            return implantatsysteme.Any() ? implantatsysteme.Max(system => system.SystemID) + 1 : 1;
        }
        // Methode zum Speichern und Aktualisieren der DataGridView
        private void SpeichereUndAktualisiereUI()
        {
            // Änderungen speichern
            manager.SpeichereImplantatsysteme(implantatsysteme);
            // DataGridView für Implantatsysteme aktualisieren
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = implantatsysteme.Select(system => new
            {
                system.SystemName // SystemID wird hier nicht angezeigt
            }).ToList();
        }
        // Methode zum Leeren der Eingabefelder
        private void LeereEingabefelder()
        {
            textBoxSystemName.Clear();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Überprüfen, ob eine gültige Zeile ausgewählt wurde
            if (e.RowIndex >= 0)
            {
                // Erhalte die aktuelle Zeile
                var selectedRow = dataGridView1.Rows[e.RowIndex];

                // Systemnamen in die Textbox übertragen
                string systemName = selectedRow.Cells["SystemName"].Value.ToString();
                textBoxSystemName.Text = systemName;

                // Das ausgewählte System finden und für die Bearbeitung speichern
                aktuellBearbeitetesSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == systemName);
            }
        }
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Überprüfen, ob die aktuelle Spalte die "Menge"-Spalte ist
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Menge" && e.RowIndex >= 0)
            {
                // Erhalte die Menge und den Mindestbestand der aktuellen Zeile
                var row = dataGridView2.Rows[e.RowIndex];
                int menge = Convert.ToInt32(row.Cells["Menge"].Value);
                int mindestbestand = Convert.ToInt32(row.Cells["Mindestbestand"].Value);

                // Wenn die Menge kleiner ist als der Mindestbestand, die Zelle rot färben
                if (menge < mindestbestand)
                {
                    e.CellStyle.BackColor = System.Drawing.Color.Red;
                    e.CellStyle.ForeColor = System.Drawing.Color.White;
                }
                else
                {
                    // Standardfarben beibehalten
                    e.CellStyle.BackColor = dataGridView2.DefaultCellStyle.BackColor;
                    e.CellStyle.ForeColor = dataGridView2.DefaultCellStyle.ForeColor;
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            {
                // Überprüfen, ob eine gültige Zeile in dataGridView2 ausgewählt wurde
                if (dataGridView2.CurrentRow != null && dataGridView1.CurrentRow != null)
                {
                    // Den Systemnamen des ausgewählten Systems erhalten
                    string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();

                    // Das ausgewählte Implantatsystem anhand des Namens finden
                    var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == selectedSystemName);

                    if (selectedSystem != null)
                    {
                        logHandler = new LogHandler(logg, benutzer);

                        // Den Namen des zu löschenden Details erhalten
                        string detailName = dataGridView2.CurrentRow.Cells["Beschreibung"].Value.ToString();

                        // Bestätigung zum Löschen anfordern
                        var result = MessageBox.Show($"Sind Sie sicher, dass Sie '{detailName}' löschen möchten?", "Löschen bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        // Überprüfen, ob der Benutzer "Ja" gewählt hat
                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                // Das ausgewählte Detail finden
                                var detailToDelete = selectedSystem.Details.FirstOrDefault(detail => detail.Beschreibung == detailName);

                                if (detailToDelete != null)
                                {
                                    logHandler.LogAction($"Löschen : {selectedSystemName} -> {detailName} wurde gelöscht");
                                    // Detail aus der Liste entfernen
                                    selectedSystem.Details.Remove(detailToDelete);

                                    // Änderungen speichern
                                    manager.SpeichereImplantatsysteme(implantatsysteme);

                                    // DataGridView2 aktualisieren
                                    dataGridView2.DataSource = null;
                                    dataGridView2.DataSource = selectedSystem.Details.ToList();

                                    // Eingabefelder leeren
                                    textBoxBeschreibung.Clear();
                                    textBoxMenge.Clear();
                                    textBoxMindestbestand.Clear();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie ein Detail zum Löschen aus.");
                }
            }
        }
        private void MindestbestandSpalteAusblenden()
        {
            // Überprüfen, ob die "Mindestbestand"-Spalte existiert und sie dann ausblenden
            if (dataGridView2.Columns["Mindestbestand"] != null)
            {
                dataGridView2.Columns["Mindestbestand"].Visible = false;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Überprüfen, ob eine gültige Zeile in dataGridView1 ausgewählt wurde
            if (dataGridView1.CurrentRow != null)
            {
                logHandler = new LogHandler(logg, benutzer);
                // Den Systemnamen des ausgewählten Systems erhalten
                string selectedSystemName = dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString();

                // Das ausgewählte Implantatsystem anhand des Namens finden
                var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == selectedSystemName);

                if (selectedSystem != null)
                {
                    // Bestätigung zum Löschen anfordern
                    string userInput = EingabeBestätigen("Geben Sie ' bestätigen ' ein, um fortzufahren.", "Löschen bestätigen");

                    // Überprüfen, ob der Benutzer 'bestätigen' eingegeben hat
                    if (userInput.ToLower() == "bestätigen")
                    {
                        try
                        {
                            // System aus der Liste der Implantatsysteme entfernen
                            implantatsysteme.Remove(selectedSystem);

                            // Änderungen speichern
                            manager.SpeichereImplantatsysteme(implantatsysteme);

                            // DataGridView für Implantatsysteme aktualisieren
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = implantatsysteme.Select(system => new
                            {
                                system.SystemName

                            }).ToList();
                            logHandler.LogAction($"Firmen / Hersteller - {selectedSystem.SystemName} wurde gelöscht ");
                            // DataGridView für Details leeren
                            dataGridView2.DataSource = null;

                            // Eingabefeld für den Systemnamen leeren
                            textBoxSystemName.Clear();

                            MessageBox.Show("Das System wurde erfolgreich gelöscht.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Löschung abgebrochen. Sie müssen 'bestätigen' eingeben, um das System zu löschen.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie ein Implantatsystem aus, um es zu löschen.");
            }
        }
        private string EingabeBestätigen(string nachricht, string titel)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = titel,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = nachricht, Width = 250 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 250 };
            Button bestatigenButton = new Button() { Text = "Bestätigen", Left = 150, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(bestatigenButton);
            prompt.AcceptButton = bestatigenButton;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Uc_Settings uc_Settings = new Uc_Settings();
            uc_Settings.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc_Settings);
        }
        private Control[] originalControls;
        private void button7_Click(object sender, EventArgs e)
        {
            // Das Panel leeren
            panel1.Controls.Clear();

            // Hier den ursprünglichen Inhalt wiederherstellen
            // Beispiel: Panel könnte vorher Label oder TextBox enthalten haben, dies hier wieder hinzufügen
            panel1.Controls.AddRange(originalControls);
        }
        private void btn_Chat_Click(object sender, EventArgs e)
        {
            UC_Chatcs uC_Chatcs = new UC_Chatcs(benutzer);
            panel1.Controls.Clear();
            panel1.Controls.Add(uC_Chatcs);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void btn_Minus_Click(object sender, EventArgs e)
        {
            if(textBoxMenge.Text != "")
            {
                int index = int.Parse(textBoxMenge.Text);
                if (index > 0)
                    index--;
                else
                    index = 0;
                textBoxMenge.Text = index.ToString();
            }
        }
        private void btn_Plus_Click(object sender, EventArgs e)
        {
            if (textBoxMenge.Text != "")
            {
                int index = int.Parse(textBoxMenge.Text);
                    index++;
                textBoxMenge.Text = index.ToString();
            }
            else
                textBoxMenge.Text = "0";
        }
        private void btn_MinusMindes_Click(object sender, EventArgs e)
        {
            if (textBoxMindestbestand.Text != "")
            {
                int index = int.Parse(textBoxMindestbestand.Text);
                if (index > 0)
                    index--;
                else
                    index = 0;
                textBoxMindestbestand.Text = index.ToString();
            }
        }
        private void btn_PlusMindest_Click(object sender, EventArgs e)
        {
            if (textBoxMindestbestand.Text != "")
            {
                int index = int.Parse(textBoxMindestbestand.Text);
                index++;
                textBoxMindestbestand.Text = index.ToString();
            }
            else
                textBoxMindestbestand.Text = "0";
        }
        private void btn_UC_Übersicht_Click(object sender, EventArgs e)
        {
            // Prüfen, ob Form2 bereits geöffnet ist
            if (Form2.instanze == null || Form2.instanze.IsDisposed)
            {

                Form2.instanze = new Form2(implantatsysteme);
                Form2.instanze.FormClosed += (s, args) => Form2.instanze = null; // Setzt die Instanz auf null, wenn Form2 geschlossen wird
                Form2.instanze.Show();
            }
            else
            {
                // Wenn Form2 bereits geöffnet ist, bringe es in den Vordergrund
                Form2.instanze.BringToFront();
            }
        }
        private void btn_New_Click(object sender, EventArgs e)
        {
            aktuellBearbeitetesDetail = null;
            textBoxBeschreibung.Clear();
            textBoxMenge.Clear();
            textBoxMindestbestand.Clear();
        }
        private void btn_SystemNameNew_Click(object sender, EventArgs e)
        {
            aktuellBearbeitetesSystem = null;
            textBoxSystemName.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            UC_History uC_Chatcs = new UC_History(benutzer);
            panel1.Controls.Clear();
            panel1.Controls.Add(uC_Chatcs);
        }
    }
}
