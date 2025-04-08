using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ArbeitInventur
{
    public partial class Übersicht : Form
    {
        private readonly ProduktManager manager;
        private List<ProduktFirma> implantatsysteme;
        private readonly JsonDateiÜberwacher jsonÜberwacher;
        private readonly string jsonDateiPfad = Path.Combine(Properties.Settings.Default.DataJSON, "implantatsysteme.json");
        private readonly System.Timers.Timer debounceTimer;
        private SortOrder currentSortOrder = SortOrder.None;
        private string currentSortColumn = string.Empty;
        public static Übersicht instanze;

        public Übersicht(List<ProduktFirma> vorabGeladeneDaten)
        {
            InitializeComponent();

            implantatsysteme = vorabGeladeneDaten ?? new List<ProduktFirma>();
            instanze = this;
            DGVStyle.Dgv(dataGridViewOverview);
            manager = new ProduktManager();
            jsonÜberwacher = new JsonDateiÜberwacher(jsonDateiPfad);
            jsonÜberwacher.DateiGeändert += JsonDateiWurdeGeändert;

            debounceTimer = new System.Timers.Timer(500) { AutoReset = false };
            debounceTimer.Elapsed += DebounceTimerElapsed;

            textBoxSuche.TextChanged += TextBoxSuche_TextChanged;
            dataGridViewOverview.ColumnHeaderMouseClick += DataGridViewOverview_ColumnHeaderMouseClick; // Sortier-Event hinzufügen
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            jsonÜberwacher.StartÜberwachung();
            ÜbersichtLaden();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            jsonÜberwacher.StopÜberwachung();
            debounceTimer.Stop();
            debounceTimer.Dispose();
        }

        private void JsonDateiWurdeGeändert()
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private async void DebounceTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsHandleCreated) return;

            await InvokeAsync(async () => await DatenLadenUndAnzeigenAsync());
        }

        private async Task DatenLadenUndAnzeigenAsync()
        {
            try
            {
                implantatsysteme = await manager.LadeImplantatsystemeAsync();
                ÜbersichtLaden();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ÜbersichtLaden(string filter = "")
        {
            if (implantatsysteme == null || dataGridViewOverview == null) return;

            var systemsWithDetails = implantatsysteme.SelectMany(system => system.Details.Select(detail => new
            {
                SystemName = system.SystemName,
                Kategorie = detail.Kategorie,
                DetailName = detail.Beschreibung,
                Menge = detail.Menge,
                Mindestbestand = detail.Mindestbestand
            }));

            if (!string.IsNullOrWhiteSpace(filter))
            {
                string lowerFilter = filter.ToLower();
                systemsWithDetails = systemsWithDetails.Where(item =>
                    item.SystemName.ToLower().Contains(lowerFilter) ||
                    item.DetailName.ToLower().Contains(lowerFilter) ||
                    item.Kategorie.ToLower().Contains(lowerFilter));
            }

            if (checkBox_MinOrders?.Checked == true)
            {
                systemsWithDetails = systemsWithDetails.Where(item => item.Menge < item.Mindestbestand);
            }

            var data = ApplySorting(systemsWithDetails).ToList();

            if (IsHandleCreated)
            {
                dataGridViewOverview.Invoke((MethodInvoker)(() =>
                {
                    dataGridViewOverview.DataSource = null;
                    dataGridViewOverview.DataSource = data;

                    dataGridViewOverview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewOverview.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    if (dataGridViewOverview.Columns["SystemName"] != null) dataGridViewOverview.Columns["SystemName"].HeaderText = "System Name";
                    if (dataGridViewOverview.Columns["Kategorie"] != null) dataGridViewOverview.Columns["Kategorie"].HeaderText = "Kategorie";
                    if (dataGridViewOverview.Columns["DetailName"] != null) dataGridViewOverview.Columns["DetailName"].HeaderText = "Detail Name";
                    if (dataGridViewOverview.Columns["Menge"] != null) dataGridViewOverview.Columns["Menge"].HeaderText = "Menge";
                    if (dataGridViewOverview.Columns["Mindestbestand"] != null)
                    {
                        dataGridViewOverview.Columns["Mindestbestand"].HeaderText = "Mindestbestand";
                        dataGridViewOverview.Columns["Mindestbestand"].ReadOnly = true;
                    }

                    // Sortierreihenfolge visuell anzeigen
                    UpdateSortGlyph();
                }));
            }
        }

        private IEnumerable<dynamic> ApplySorting(IEnumerable<dynamic> data)
        {
            if (string.IsNullOrEmpty(currentSortColumn) || currentSortOrder == SortOrder.None) return data;

            switch (currentSortColumn)
            {
                case "SystemName":
                    return currentSortOrder == SortOrder.Ascending
                        ? data.OrderBy(item => item.SystemName)
                        : data.OrderByDescending(item => item.SystemName);
                case "Kategorie":
                    return currentSortOrder == SortOrder.Ascending
                        ? data.OrderBy(item => item.Kategorie)
                        : data.OrderByDescending(item => item.Kategorie);
                case "DetailName":
                    return currentSortOrder == SortOrder.Ascending
                        ? data.OrderBy(item => item.DetailName)
                        : data.OrderByDescending(item => item.DetailName);
                case "Menge":
                    return currentSortOrder == SortOrder.Ascending
                        ? data.OrderBy(item => item.Menge)
                        : data.OrderByDescending(item => item.Menge);
                case "Mindestbestand":
                    return currentSortOrder == SortOrder.Ascending
                        ? data.OrderBy(item => item.Mindestbestand)
                        : data.OrderByDescending(item => item.Mindestbestand);
                default:
                    return data;
            }
        }

        private void UpdateSortGlyph()
        {
            foreach (DataGridViewColumn column in dataGridViewOverview.Columns)
            {
                column.HeaderCell.SortGlyphDirection = column.Name == currentSortColumn
                    ? currentSortOrder
                    : SortOrder.None;
            }
        }

        private void DataGridViewOverview_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string clickedColumn = dataGridViewOverview.Columns[e.ColumnIndex].Name;

            if (currentSortColumn == clickedColumn)
            {
                // Umschalten zwischen Aufwärts- und Abwärts-Sortierung
                currentSortOrder = currentSortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                // Neue Spalte sortieren, standardmäßig aufwärts
                currentSortColumn = clickedColumn;
                currentSortOrder = SortOrder.Ascending;
            }

            ÜbersichtLaden(textBoxSuche.Text);
        }

        private void TextBoxSuche_TextChanged(object sender, EventArgs e)
        {
            ÜbersichtLaden(textBoxSuche.Text);
        }

        private void dataGridViewOverview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
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
            // Bestehende Logik kann hier beibehalten oder angepasst werden
            if (e.RowIndex < 0) return;

            var selectedRow = dataGridViewOverview.Rows[e.RowIndex];
            string systemName = selectedRow.Cells["SystemName"].Value?.ToString();

            if (string.IsNullOrEmpty(systemName)) return;

            var selectedSystem = implantatsysteme.FirstOrDefault(system => system.SystemName == systemName);
            if (selectedSystem == null) return;

            ÜbersichtLaden(textBoxSuche.Text); // Einfach neu laden als Platzhalter
        }

        private void checkBox_MinOrders_CheckedChanged(object sender, EventArgs e)
        {
            ÜbersichtLaden(textBoxSuche.Text);
        }

        private Task InvokeAsync(Func<Task> action)
        {
            if (!IsHandleCreated) return Task.CompletedTask;

            if (InvokeRequired)
            {
                var tcs = new TaskCompletionSource<bool>();
                Invoke(new Action(async () =>
                {
                    try
                    {
                        await action();
                        tcs.SetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }));
                return tcs.Task;
            }
            return action();
        }
    }
}