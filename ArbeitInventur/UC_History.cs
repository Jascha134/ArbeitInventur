using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class UC_History : UserControl
    {
        private readonly LogHandler _logger;
        private List<LogEntry> _logEntries;
        private readonly Benutzer _benutzer;

        public UC_History(Benutzer benutzer)
        {
            InitializeComponent();

            _benutzer = benutzer ?? throw new ArgumentNullException(nameof(benutzer));
            _logger = new LogHandler(Properties.Settings.Default.DataJSON + "\\log.json", benutzer);
            _logEntries = new List<LogEntry>();

            listBoxTimestamp.SelectedIndexChanged += ListBoxTimestamp_SelectedIndexChanged;

            LoadUniqueDates();
        }

        private List<LogEntry> LoadLogEntriesIfNeeded()
        {
            _logEntries = _logger.LoadLogEntries();
            return _logEntries;
        }

        private void LoadUniqueDates()
        {
            ClearListBoxes();

            var uniqueDates = LoadLogEntriesIfNeeded()
                .Where(log => log.BenutzerName == _benutzer.Name)
                .Select(log => log.Timestamp.Date)
                .Distinct()
                .OrderBy(date => date)
                .ToList();

            foreach (var date in uniqueDates)
            {
                string dateWithDay = $"{date:dddd, dd.MM.yyyy}";
                listBoxTimestamp.Items.Add(new ListBoxItem { Date = date, DisplayText = dateWithDay });
            }

            // Wähle den aktuellen Tag automatisch aus
            var today = DateTime.Today;
            var todayItem = listBoxTimestamp.Items
                .Cast<ListBoxItem>()
                .FirstOrDefault(item => item.Date == today);

            if (todayItem != null)
            {
                listBoxTimestamp.SelectedItem = todayItem;
            }
            else
            {
                listBoxActions.Items.Clear();
                listBoxActions.Items.Add("Keine Einträge für den aktuellen Tag vorhanden.");
            }
        }

        private void ClearListBoxes()
        {
            listBoxTimestamp.Items.Clear();
            listBoxActions.Items.Clear();
        }

        private class ListBoxItem
        {
            public DateTime Date { get; set; }
            public string DisplayText { get; set; }

            public override string ToString() => DisplayText;
        }

        private void ListBoxTimestamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTimestamp.SelectedItem is ListBoxItem selectedItem)
            {
                UpdateActionsForDate(selectedItem.Date);
            }
        }

        private void UpdateActionsForDate(DateTime selectedDate)
        {
            var filteredEntries = LoadLogEntriesIfNeeded()
                .Where(log => log.Timestamp.Date == selectedDate && log.BenutzerName == _benutzer.Name)
                .ToList();

            listBoxActions.Items.Clear();
            if (filteredEntries.Any())
            {
                foreach (var log in filteredEntries)
                {
                    string displayText = $"[{log.Timestamp:HH:mm}] - {log.BenutzerName ?? "Unbekannt"}: {log.Action} - {log.Message}";
                    listBoxActions.Items.Add(displayText);
                }
            }
            else
            {
                listBoxActions.Items.Add("Keine Aktionen für dieses Datum vorhanden.");
            }
        }

        public void RefreshHistory()
        {
            LoadUniqueDates();
        }
    }
}