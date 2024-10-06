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
        private LogHandler logger;
        private List<LogHandler.LogEntry> logEntries;
        private Benutzer benutzer;

        public UC_History(Benutzer benutzer)
        {
            InitializeComponent();

            // Initialisiere den Logger und lade die Logs
            logger = new LogHandler(Properties.Settings.Default.DataJSON + "\\log.json",benutzer);
            logEntries = logger.LoadLogs();

            // Event-Handler für das Ändern der Auswahl in listBoxTimestamp
            listBoxTimestamp.SelectedIndexChanged += listBoxTimestamp_SelectedIndexChanged_1;

            // ListBoxen befüllen
            LoadUniqueDates();
            this.benutzer = benutzer;
        }

        // Methode zum Laden der einzigartigen Datumsangaben in listBoxTimestamp
        private void LoadUniqueDates()
        {
            // Leere die ListBoxen, um sicherzustellen, dass sie keine alten Daten enthalten
            listBoxTimestamp.Items.Clear();
            listBoxActions.Items.Clear();

            // Gruppiere die Einträge nach Datum, um sicherzustellen, dass jedes Datum nur einmal angezeigt wird
            var uniqueDates = logEntries
                .Select(log => log.Timestamp.Date)  // Nur das Datum, ohne Uhrzeit
                .Distinct()                         // Nur einzigartige Datumswerte
                .OrderBy(date => date)              // Sortierung nach Datum
                .ToList();

            // Füge die einzigartigen Datumsangaben zur listBoxTimestamp hinzu
            foreach (var date in uniqueDates)
            {
                string dateWithDay = $"{date:dddd, dd.MM.yyyy}"; // Beispiel: "Montag, 20.03.2023"
                listBoxTimestamp.Items.Add(new ListBoxItem { Date = date, DisplayText = dateWithDay });
            }
        }

        // Hilfsklasse zur Speicherung der ListBox-Items mit Datum und Anzeige-Text
        private class ListBoxItem
        {
            public DateTime Date { get; set; }
            public string DisplayText { get; set; }

            // Wird verwendet, um den angezeigten Text in der ListBox darzustellen
            public override string ToString()
            {
                return DisplayText;
            }
        }
        // Event-Handler, wenn der Benutzer ein Datum in listBoxTimestamp auswählt
        private void listBoxTimestamp_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Überprüfen, ob ein gültiger Eintrag in der ListBox ausgewählt wurde
            if (listBoxTimestamp.SelectedItem is ListBoxItem selectedItem)
            {
                DateTime selectedDate = selectedItem.Date;

                // Filtere die Log-Einträge nach dem ausgewählten Datum
                var filteredEntries = logEntries.Where(log => log.Timestamp.Date == selectedDate).ToList();

                // Aktualisiere die zweite ListBox mit den gefilterten Einträgen
                listBoxActions.Items.Clear();
                foreach (var log in filteredEntries)
                {
                    listBoxActions.Items.Add($"[{log.Timestamp:HH:mm}]" + " - " + $"{log.Benutzer.Name + ": "}" + log.Handlung);
                }
            }
        }
    }
}
