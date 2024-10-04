
namespace ArbeitInventur
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using System.Windows.Forms;

    public partial class Form2 : Form
    {
        private ImplantatManager manager;
        private List<ImplantatSystem> implantatsysteme;
        private JsonDateiÜberwacher jsonÜberwacher;
        private string jsonDateiPfad = Properties.Settings.Default.DataJSON+ "\\implantatsysteme.json"; // Pfad zur JSON-Datei auf dem Server
        private System.Timers.Timer debounceTimer;
        public static Form2 instanze;

        public Form2()
        {
            InitializeComponent();

            instanze = this;
            DGVStyle.Dgv(dataGridViewOverview);

            // Instanz von ImplantatManager erstellen
            manager = new ImplantatManager();

            // Instanz von JsonDateiÜberwacher erstellen
            jsonÜberwacher = new JsonDateiÜberwacher(jsonDateiPfad);
            jsonÜberwacher.DateiGeändert += JsonDateiWurdeGeändert;

            // Timer initialisieren
            debounceTimer = new System.Timers.Timer(500); // 500 ms Verzögerung
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimerElapsed;

            textBoxSuche.TextChanged += textBoxSuche_TextChanged; // Event-Handler für die Suche registrieren
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Überwachung starten, wenn das Formular geladen wird
            jsonÜberwacher.StartÜberwachung();

            // Initiale Daten laden, wenn das Fenster vollständig erstellt ist
            DatenLadenUndAnzeigen();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Überwachung stoppen, wenn das Formular geschlossen wird
            jsonÜberwacher.StopÜberwachung();
        }

        private void JsonDateiWurdeGeändert()
        {
            // Wenn der FileSystemWatcher eine Änderung erkennt, den Timer neu starten
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void DebounceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Die Methode wird im richtigen Thread aufgerufen
            if (IsHandleCreated)
            {
                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(() =>
                    {
                        DatenLadenUndAnzeigen();
                    }));
                }
                else
                {
                    DatenLadenUndAnzeigen();
                }
            }
        }

        private void DatenLadenUndAnzeigen()
        {
            // Implantatsysteme laden
            implantatsysteme = manager.LadeImplantatsysteme();

            // Übersicht der geladenen Daten anzeigen
            ÜbersichtLaden();
        }

        private void ÜbersichtLaden(string filter = "")
        {
            // Liste erstellen, die alle Systeme mit Details kombiniert
            var systemsWithDetails = implantatsysteme.SelectMany(system => system.Details.Select(detail => new
            {
                SystemName = system.SystemName,
                Kategorie = detail.Kategorie,
                DetailName = detail.Beschreibung,
                Menge = detail.Menge,
                Mindestbestand = detail.Mindestbestand
            })).ToList();

            // Wenn ein Filter vorhanden ist, die Liste entsprechend filtern
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string lowerFilter = filter.ToLower(); // Filter in Kleinbuchstaben umwandeln
                systemsWithDetails = systemsWithDetails
                    .Where(item => item.SystemName.ToLower().Contains(lowerFilter) ||
                                   item.DetailName.ToLower().Contains(lowerFilter) ||
                                   item.Kategorie.ToLower().Contains(lowerFilter))
                    .ToList();
            }

            // Daten in der DataGridView anzeigen
            if (IsHandleCreated)
            {
                dataGridViewOverview.Invoke((MethodInvoker)(() =>
                {
                    dataGridViewOverview.DataSource = null; // Sicherstellen, dass die Datenquelle neu gesetzt wird
                    dataGridViewOverview.DataSource = systemsWithDetails;

                    // Spalten automatisch anpassen, um eine bessere Lesbarkeit zu erreichen
                    dataGridViewOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewOverview.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    // Spaltenüberschriften anpassen
                    dataGridViewOverview.Columns["SystemName"].HeaderText = "System Name";
                    dataGridViewOverview.Columns["Kategorie"].HeaderText = "Kategorie";
                    dataGridViewOverview.Columns["DetailName"].HeaderText = "Detail Name";
                    dataGridViewOverview.Columns["Menge"].HeaderText = "Menge";
                    dataGridViewOverview.Columns["Mindestbestand"].HeaderText = "Mindestbestand";

                    // Optional: Mindestbestand-Spalte als readonly setzen
                    dataGridViewOverview.Columns["Mindestbestand"].ReadOnly = true;
                }));
            }
        }


        private void textBoxSuche_TextChanged(object sender, EventArgs e)
        {
            // Filter basierend auf dem eingegebenen Text anwenden
            ÜbersichtLaden(textBoxSuche.Text);
        }

        private void dataGridViewOverview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Überprüfen, ob die aktuelle Spalte die "Menge"-Spalte ist
            if (dataGridViewOverview.Columns[e.ColumnIndex].Name == "Menge" && e.RowIndex >= 0)
            {
                var row = dataGridViewOverview.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["Menge"].Value?.ToString(), out int menge) &&
                    int.TryParse(row.Cells["Mindestbestand"].Value?.ToString(), out int mindestbestand))
                {
                    // Wenn die Menge kleiner ist als der Mindestbestand, die Zelle rot färben
                    if (menge < mindestbestand)
                    {
                        e.CellStyle.BackColor = System.Drawing.Color.Red;
                        e.CellStyle.ForeColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        e.CellStyle.BackColor = dataGridViewOverview.DefaultCellStyle.BackColor;
                        e.CellStyle.ForeColor = dataGridViewOverview.DefaultCellStyle.ForeColor;
                    }
                }
            }
        }

        private void dataGridViewOverview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Überprüfen, ob eine gültige Zeile angeklickt wurde
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var selectedRow = dataGridViewOverview.Rows[e.RowIndex];

                // Verifizieren, dass die angeklickte Zeile einen "SystemName" enthält, der keine Detailzeile ist
                if (selectedRow.Cells[0].Value != null && !selectedRow.Cells[0].Value.ToString().StartsWith("    "))
                {
                    string systemName = selectedRow.Cells[0].Value.ToString();

                    // Das ausgewählte Implantatsystem finden
                    var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == systemName);

                    if (selectedSystem != null)
                    {
                        if (selectedRow.Cells.Count > 1 && selectedRow.Cells[1].Value != null && selectedRow.Cells[1].Value.ToString() == "Expand")
                        {
                            int insertIndex = e.RowIndex + 1;
                            foreach (var detail in selectedSystem.Details)
                            {
                                dataGridViewOverview.Rows.Insert(insertIndex, "    " + detail.Beschreibung, $"Menge: {detail.Menge}, Mindestbestand: {detail.Mindestbestand}");
                                insertIndex++;
                            }
                            selectedRow.Cells[1].Value = "Collapse";
                        }
                        else if (selectedRow.Cells.Count > 1 && selectedRow.Cells[1].Value != null && selectedRow.Cells[1].Value.ToString() == "Collapse")
                        {
                            int removeIndex = e.RowIndex + 1;
                            while (removeIndex < dataGridViewOverview.Rows.Count && dataGridViewOverview.Rows[removeIndex].Cells[0].Value.ToString().StartsWith("    "))
                            {
                                dataGridViewOverview.Rows.RemoveAt(removeIndex);
                            }
                            selectedRow.Cells[1].Value = "Expand";
                        }
                    }
                }
            }
        }

        private void checkBox_MinOrders_CheckedChanged(object sender, EventArgs e)
        {
            ÜbersichtLaden();
        }
        private void ÜbersichtLaden()
        {
            // Überprüfen, ob die Liste der Implantatsysteme initialisiert wurde
            if (implantatsysteme == null)
            {
                MessageBox.Show("Die Liste der Implantatsysteme wurde nicht geladen.");
                return;
            }

            // Überprüfen, ob die DataGridView initialisiert wurde
            if (dataGridViewOverview == null)
            {
                MessageBox.Show("dataGridViewOverview ist nicht korrekt initialisiert.");
                return;
            }

            List<object> systemsWithDetails;

            if (checkBox_MinOrders != null && checkBox_MinOrders.Checked)
            {
                // Filter anwenden: Nur Materialien unter dem Mindestbestand anzeigen
                systemsWithDetails = implantatsysteme.SelectMany(system => system.Details
                                        .Where(detail => detail.Menge < detail.Mindestbestand)
                                        .Select(detail => (object)new
                                        {
                                            SystemName = system.SystemName,
                                            Kategorie = detail.Kategorie,
                                            DetailName = detail.Beschreibung,
                                            Menge = detail.Menge,
                                            Mindestbestand = detail.Mindestbestand
                                        })).ToList();
            }
            else
            {
                // Alle Materialien anzeigen
                systemsWithDetails = implantatsysteme.SelectMany(system => system.Details
                                        .Select(detail => (object)new
                                        {
                                            SystemName = system.SystemName,
                                            Kategorie = detail.Kategorie,
                                            DetailName = detail.Beschreibung,
                                            Menge = detail.Menge,
                                            Mindestbestand = detail.Mindestbestand
                                        })).ToList();
            }

            // Daten in der DataGridView anzeigen
            if (IsHandleCreated)
            {
                dataGridViewOverview.Invoke((MethodInvoker)(() =>
                {
                    dataGridViewOverview.DataSource = null; // Sicherstellen, dass die Datenquelle neu gesetzt wird
                    dataGridViewOverview.DataSource = systemsWithDetails;

                    // Spalten automatisch anpassen, um eine bessere Lesbarkeit zu erreichen
                    dataGridViewOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewOverview.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    // Spaltenüberschriften anpassen
                    if (dataGridViewOverview.Columns.Contains("SystemName"))
                        dataGridViewOverview.Columns["SystemName"].HeaderText = "System Name";
                    if (dataGridViewOverview.Columns.Contains("Kategorie"))
                        dataGridViewOverview.Columns["Kategorie"].HeaderText = "Kategorie";
                    if (dataGridViewOverview.Columns.Contains("DetailName"))
                        dataGridViewOverview.Columns["DetailName"].HeaderText = "Detail Name";

                    if (dataGridViewOverview.Columns.Contains("Menge"))
                        dataGridViewOverview.Columns["Menge"].HeaderText = "Menge";

                    if (dataGridViewOverview.Columns.Contains("Mindestbestand"))
                    {
                        dataGridViewOverview.Columns["Mindestbestand"].HeaderText = "Mindestbestand";
                        dataGridViewOverview.Columns["Mindestbestand"].ReadOnly = true; // Optional: Spalte als read-only setzen
                    }
                }));
            }
        }
    }
}
