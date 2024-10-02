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
        private string jsonDateiPfad = Properties.Settings.Default.DataJSON; // Pfad zur JSON-Datei auf dem Server
        private Benutzer benutzer;

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

        private ImplantatManager manager = new ImplantatManager();
        private List<ImplantatSystem> implantatsysteme = new List<ImplantatSystem>();

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
        private ImplantatSystemDetails aktuellBearbeitetesDetail = null; // Speichert das Detail, das bearbeitet wird
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
                        // Neue Detailinformationen sammeln
                        string detailName = textBoxName.Text;
                        int menge = int.Parse(textBoxMenge.Text);
                        int mindestbestand = int.Parse(textBoxMindestbestand.Text);

                        // Überprüfen, ob der Detailname leer ist
                        if (string.IsNullOrWhiteSpace(detailName))
                        {
                            MessageBox.Show("Bitte geben Sie einen gültigen Namen für das Detail ein.");
                            return;
                        }

                        if (aktuellBearbeitetesDetail == null)
                        {
                            // Neues Detail für das Implantatsystem erstellen (Hinzufügen)
                            var neuesDetail = new ImplantatSystemDetails
                            {
                                Beschreibung = detailName,
                                Menge = menge,
                                Mindestbestand = mindestbestand
                            };

                            // Detail zur Liste der Details des ausgewählten Systems hinzufügen
                            selectedSystem.Details.Add(neuesDetail);
                        }
                        else
                        {
                            // Bearbeitung des ausgewählten Details (Bearbeiten)
                            aktuellBearbeitetesDetail.Beschreibung = detailName;
                            aktuellBearbeitetesDetail.Menge = menge;
                            aktuellBearbeitetesDetail.Mindestbestand = mindestbestand;

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
                        textBoxName.Clear();
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
            // Überprüfen, ob eine gültige Zeile ausgewählt wurde
            if (e.RowIndex >= 0)
            {
                // Erhalte die aktuelle Zeile
                var selectedRow = dataGridView2.Rows[e.RowIndex];

                // Werte der aktuellen Zeile in die Textboxen übertragen
                textBoxName.Text = selectedRow.Cells["Name"].Value.ToString();
                textBoxMenge.Text = selectedRow.Cells["Menge"].Value.ToString();
                textBoxMindestbestand.Text = selectedRow.Cells["Mindestbestand"].Value.ToString();

                // Das ausgewählte Detail finden und für die Bearbeitung speichern
                string detailName = selectedRow.Cells["Name"].Value.ToString();
                var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == dataGridView1.CurrentRow.Cells["SystemName"].Value.ToString());
                if (selectedSystem != null)
                {
                    aktuellBearbeitetesDetail = selectedSystem.Details.FirstOrDefault(detail => detail.Beschreibung == detailName);
                }
            }
        }
        private ImplantatSystem aktuellBearbeitetesSystem = null; // Speichert das System, das bearbeitet wird
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Eingabe für den Systemnamen sammeln
                string systemName = textBoxSystemName.Text;

                // Überprüfen, ob der Systemname leer ist
                if (string.IsNullOrWhiteSpace(systemName))
                {
                    MessageBox.Show("Bitte geben Sie einen gültigen Systemnamen ein.");
                    return;
                }

                if (aktuellBearbeitetesSystem == null)
                {
                    // Neues System erstellen (Hinzufügen)
                    // SystemID automatisch generieren: Maximal bestehende SystemID + 1
                    int newSystemID = implantatsysteme.Any() ? implantatsysteme.Max(system => system.SystemID) + 1 : 1;

                    var neuesSystem = new ImplantatSystem
                    {
                        SystemID = newSystemID,
                        SystemName = systemName,
                        Details = new List<ImplantatSystemDetails>() // Leere Liste für Details
                    };

                    // Neues System zur Liste der Implantatsysteme hinzufügen
                    implantatsysteme.Add(neuesSystem);
                }
                else
                {
                    // System bearbeiten
                    aktuellBearbeitetesSystem.SystemName = systemName;

                    // Bearbeitung aufheben
                    aktuellBearbeitetesSystem = null;
                }

                // Änderungen speichern
                manager.SpeichereImplantatsysteme(implantatsysteme);

                // DataGridView für Implantatsysteme aktualisieren
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = implantatsysteme.Select(system => new
                {
                    system.SystemName // Die SystemID wird hier nicht mehr angezeigt
                }).ToList();

                // Eingabefelder leeren
                textBoxSystemName.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ein Fehler ist aufgetreten: " + ex.Message);
            }
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
                        // Den Namen des zu löschenden Details erhalten
                        string detailName = dataGridView2.CurrentRow.Cells["Name"].Value.ToString();

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
                                    // Detail aus der Liste entfernen
                                    selectedSystem.Details.Remove(detailToDelete);

                                    // Änderungen speichern
                                    manager.SpeichereImplantatsysteme(implantatsysteme);

                                    // DataGridView2 aktualisieren
                                    dataGridView2.DataSource = null;
                                    dataGridView2.DataSource = selectedSystem.Details.ToList();

                                    // Eingabefelder leeren
                                    textBoxName.Clear();
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
                Form2.instanze = new Form2();
                Form2.instanze.FormClosed += (s, args) => Form2.instanze = null; // Setzt die Instanz auf null, wenn Form2 geschlossen wird
                Form2.instanze.Show();
            }
            else
            {
                // Wenn Form2 bereits geöffnet ist, bringe es in den Vordergrund
                Form2.instanze.BringToFront();
            }
        }
    }
}
