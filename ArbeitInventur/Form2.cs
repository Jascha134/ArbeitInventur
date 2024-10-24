namespace ArbeitInventur
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Timers;
    using System.Windows.Forms;

    public partial class Form2 : Form
    {
        // Fields
        private ImplantatManager manager;
        private List<ImplantatSystem> implantatsysteme;
        private JsonDateiÜberwacher jsonÜberwacher;
        private string jsonDateiPfad = Properties.Settings.Default.DataJSON + "\\implantatsysteme.json"; // Pfad zur JSON-Datei auf dem Server
        private System.Timers.Timer debounceTimer;
        public static Form2 instanze;
        private static List<dynamic> systemsWithDetailsCache = null;
        private static bool cacheInitialisiert = false;

        // Constructor
        public Form2()
        {
            InitializeComponent();

            instanze = this;
            DGVStyle.Dgv(dataGridViewOverview);

            InitializeManager();
            InitializeJsonWatcher();
            InitializeDebounceTimer();

            textBoxSuche.TextChanged += textBoxSuche_TextChanged; // Event-Handler für die Suche registrieren
            checkBox_MinOrders.CheckedChanged += checkBox_MinOrders_CheckedChanged; // Event-Handler für Checkbox registrieren
            dataGridViewOverview.ColumnHeaderMouseClick += dataGridViewOverview_ColumnHeaderMouseClick; // Event-Handler für Spaltenheader-Klick registrieren
            dataGridViewOverview.CellFormatting += dataGridViewOverview_CellFormatting; // Event-Handler für Zellformatierung registrieren
            dataGridViewOverview.CellClick += dataGridViewOverview_CellClick; // Event-Handler für Zellklick registrieren
        }

        // Initialization Methods
        private void InitializeManager()
        {
            // Instanz von ImplantatManager erstellen
            manager = new ImplantatManager();
        }

        private void InitializeJsonWatcher()
        {
            // Instanz von JsonDateiÜberwacher erstellen
            jsonÜberwacher = new JsonDateiÜberwacher(jsonDateiPfad);
            jsonÜberwacher.DateiGeändert += JsonDateiWurdeGeändert;
        }

        private void InitializeDebounceTimer()
        {
            // Timer initialisieren
            debounceTimer = new System.Timers.Timer(500); // 500 ms Verzögerung
            debounceTimer.AutoReset = false;
            debounceTimer.Elapsed += DebounceTimerElapsed;
        }

        // Form Event Handlers
        private void Form2_Load(object sender, EventArgs e)
        {
            // Überwachung starten, wenn das Formular geladen wird
            jsonÜberwacher.StartÜberwachung();

            // Initiale Daten nur einmal laden, wenn das Fenster vollständig erstellt ist
            if (!cacheInitialisiert)
            {
                DatenLadenUndAnzeigen();
                cacheInitialisiert = true;
            }
            else
            {
                // Wenn Daten bereits geladen wurden, nur anzeigen
                ÜbersichtLaden();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Überwachung stoppen, wenn das Formular geschlossen wird
            jsonÜberwacher.StopÜberwachung();
        }

        // JSON Change Handling
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
                    Invoke(new MethodInvoker(AktualisierteDatenLaden));
                }
                else
                {
                    AktualisierteDatenLaden();
                }
            }
        }

        // Data Loading and Display
        private void DatenLadenUndAnzeigen()
        {
            // Implantatsysteme laden
            implantatsysteme = manager.LadeImplantatsysteme();

            // Übersicht der geladenen Daten anzeigen
            ÜbersichtLaden(forceReload: true);
        }

        private void AktualisierteDatenLaden()
        {
            // Nur aktualisierte Daten laden, um den Cache zu aktualisieren
            implantatsysteme = manager.LadeImplantatsysteme();
            systemsWithDetailsCache = LoadSystemsWithDetails();

            // Übersicht aktualisieren
            ÜbersichtLaden();
        }

        private void ÜbersichtLaden(string filter = "", bool forceReload = false)
        {
            // Die Daten nur neu laden, wenn forceReload true ist oder keine Daten im Cache vorhanden sind
            if (systemsWithDetailsCache == null || forceReload)
            {
                systemsWithDetailsCache = LoadSystemsWithDetails();
            }

            var systemsWithDetails = systemsWithDetailsCache;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                systemsWithDetails = ApplyFilter(systemsWithDetails, filter);
            }

            if (checkBox_MinOrders.Checked)
            {
                // Filter anwenden: Nur Elemente anzeigen, deren Menge unter dem Mindestbestand liegt
                systemsWithDetails = systemsWithDetails.Where(item => item.Menge < item.Mindestbestand).ToList();
            }

            DisplayDataInGrid(systemsWithDetails);
        }

        private List<dynamic> LoadSystemsWithDetails()
        {
            // Liste erstellen, die alle Systeme mit Details kombiniert
            return implantatsysteme.SelectMany(system => system.Details.Select(detail => new
            {
                SystemName = system.SystemName,
                Kategorie = detail.Kategorie,
                DetailName = detail.Beschreibung,
                Menge = detail.Menge,
                Mindestbestand = detail.Mindestbestand
            })).Cast<dynamic>().ToList();
        }

        private List<dynamic> ApplyFilter(List<dynamic> systemsWithDetails, string filter)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                string lowerFilter = filter.ToLower(); // Filter in Kleinbuchstaben umwandeln
                systemsWithDetails = systemsWithDetails
                    .Where(item => item.SystemName.ToLower().Contains(lowerFilter) ||
                                   item.DetailName.ToLower().Contains(lowerFilter) ||
                                   item.Kategorie.ToLower().Contains(lowerFilter))
                    .ToList();
            }
            return systemsWithDetails;
        }

        private void DisplayDataInGrid(List<dynamic> systemsWithDetails)
        {
            if (IsHandleCreated)
            {
                dataGridViewOverview.Invoke((MethodInvoker)(() =>
                {
                    dataGridViewOverview.DataSource = null; // Sicherstellen, dass die Datenquelle neu gesetzt wird
                    dataGridViewOverview.DataSource = systemsWithDetails;

                    // Spalten automatisch anpassen, um eine bessere Lesbarkeit zu erreichen
                    dataGridViewOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewOverview.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    SetColumnHeaders();
                    HighlightLowStockRows();
                }));
            }
        }

        // UI Update Methods
        private void SetColumnHeaders()
        {
            // Spaltenüberschriften anpassen
            dataGridViewOverview.Columns["SystemName"].HeaderText = "System Name";
            dataGridViewOverview.Columns["Kategorie"].HeaderText = "Kategorie";
            dataGridViewOverview.Columns["DetailName"].HeaderText = "Detail Name";
            dataGridViewOverview.Columns["Menge"].HeaderText = "Menge";
            dataGridViewOverview.Columns["Mindestbestand"].HeaderText = "Mindestbestand";

            // Optional: Mindestbestand-Spalte als readonly setzen
            dataGridViewOverview.Columns["Mindestbestand"].ReadOnly = true;
        }

        private void HighlightLowStockRows()
        {
            foreach (DataGridViewRow row in dataGridViewOverview.Rows)
            {
                if (row.Cells["Menge"].Value != null && row.Cells["Mindestbestand"].Value != null)
                {
                    int menge = Convert.ToInt32(row.Cells["Menge"].Value);
                    int mindestbestand = Convert.ToInt32(row.Cells["Mindestbestand"].Value);

                    if (menge < mindestbestand)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral; // Zeile rot markieren, wenn Bestand unter Mindestbestand
                    }
                }
            }
        }

        // Event Handlers
        private void textBoxSuche_TextChanged(object sender, EventArgs e)
        {
            // Filter basierend auf dem eingegebenen Text anwenden
            ÜbersichtLaden(textBoxSuche.Text);
        }

        private void checkBox_MinOrders_CheckedChanged(object sender, EventArgs e)
        {
            // Filter anwenden, wenn die Checkbox geändert wird
            ÜbersichtLaden(textBoxSuche.Text);
        }

        private void dataGridViewOverview_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Sortieren nach der angeklickten Spalte
            string columnName = dataGridViewOverview.Columns[e.ColumnIndex].Name;
            bool isSortedAscending = dataGridViewOverview.SortOrder == SortOrder.Ascending;

            // Sortierung anwenden
            var sortedList = isSortedAscending
                ? systemsWithDetailsCache.OrderByDescending(item => GetPropertyValue(item, columnName)).ToList()
                : systemsWithDetailsCache.OrderBy(item => GetPropertyValue(item, columnName)).ToList();

            // Falls das Kontrollkästchen "Min Orders" aktiviert ist, filtern
            if (checkBox_MinOrders.Checked)
            {
                sortedList = sortedList.Where(item => item.Menge < item.Mindestbestand).ToList();
            }
            dataGridViewOverview.DataSource = null;
            // Datenquelle aktualisieren
            dataGridViewOverview.DataSource = sortedList;
        }

        private object GetPropertyValue(dynamic item, string propertyName)
        {
            return item.GetType().GetProperty(propertyName)?.GetValue(item, null);
        }

        private void dataGridViewOverview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Formatieren der Zellen in Abhängigkeit von bestimmten Kriterien
            if (dataGridViewOverview.Columns[e.ColumnIndex].Name == "Menge" && e.RowIndex >= 0)
            {
                var row = dataGridViewOverview.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["Menge"].Value?.ToString(), out int menge) &&
                    int.TryParse(row.Cells["Mindestbestand"].Value?.ToString(), out int mindestbestand))
                {
                    if (menge < mindestbestand)
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
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
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridViewOverview.Rows[e.RowIndex];
                // Beispiel: Aktion basierend auf dem angeklickten Element durchführen
                string systemName = selectedRow.Cells["SystemName"].Value.ToString();
            }
        }
    }
}

