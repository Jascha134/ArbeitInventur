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
        private List<LogHandler.LogEntry> _logEntries;
        private readonly Benutzer _benutzer;

        public UC_History(Benutzer benutzer)
        {
            InitializeComponent();

            _benutzer = benutzer;
            _logger = new LogHandler(Properties.Settings.Default.DataJSON + "\\log.json", benutzer);
            _logEntries = new List<LogHandler.LogEntry>();

            listBoxTimestamp.SelectedIndexChanged += ListBoxTimestamp_SelectedIndexChanged;

            // Log-Einträge werden nun nur bei Bedarf geladen (Lazy Loading)
            LoadUniqueDates();
        }

        // Methode zum Lazy Laden der Log-Einträge
        private List<LogHandler.LogEntry> LoadLogEntriesIfNeeded()
        {
            if (_logEntries == null || !_logEntries.Any())
            {
                _logEntries = _logger.LoadLogs();
            }
            return _logEntries;
        }

        // Methode zum Laden der einzigartigen Datumsangaben in listBoxTimestamp
        private void LoadUniqueDates()
        {
            ClearListBoxes();

            var uniqueDates = LoadLogEntriesIfNeeded()
                .Select(log => log.Timestamp.Date)
                .Distinct()
                .OrderBy(date => date)
                .ToList();

            foreach (var date in uniqueDates)
            {
                string dateWithDay = $"{date:dddd, dd.MM.yyyy}";
                listBoxTimestamp.Items.Add(new ListBoxItem { Date = date, DisplayText = dateWithDay });
            }
        }

        // Methode zum Leeren der ListBoxen (DRY Prinzip)
        private void ClearListBoxes()
        {
            listBoxTimestamp.Items.Clear();
            listBoxActions.Items.Clear();
        }

        // Hilfsklasse zur Speicherung der ListBox-Items
        private class ListBoxItem
        {
            public DateTime Date { get; set; }
            public string DisplayText { get; set; }

            public override string ToString() => DisplayText;
        }

        // Event-Handler für Datumsauswahl
        private void ListBoxTimestamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTimestamp.SelectedItem is ListBoxItem selectedItem)
            {
                UpdateActionsForDate(selectedItem.Date);
            }
        }

        // Aktualisiert die Actions-ListBox für das ausgewählte Datum
        private void UpdateActionsForDate(DateTime selectedDate)
        {
            var filteredEntries = LoadLogEntriesIfNeeded()
                .Where(log => log.Timestamp.Date == selectedDate)
                .ToList();

            listBoxActions.Items.Clear();
            foreach (var log in filteredEntries)
            {
                listBoxActions.Items.Add($"[{log.Timestamp:HH:mm}] - {log.Benutzer.Name}: {log.Handlung}");
            }
        }
    }
}
