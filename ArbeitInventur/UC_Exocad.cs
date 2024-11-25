using ArbeitInventur.Exocad_Help;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class UC_Exocad : UserControl, IDisposable
    {
        private DentalCadFileWatcher _fileWatcher;

        public UC_Exocad()
        {
            InitializeComponent();
            txt_Exocad_Constructions.Text = Properties.Settings.Default.ExocadConstructions;
            txt_dentalCAD.Text = Properties.Settings.Default.ExocaddentalCAD;

            listBox_Output.HorizontalScrollbar = true;

            try
            {
                _fileWatcher = new DentalCadFileWatcher();

                // Registriere Eventhandler
                _fileWatcher.FileHandled += OnFileHandled;

                // Starten des FileWatchers
                _fileWatcher.StartWatching();

                // Log-Einträge beim Start laden und in die ListBox anzeigen
                _fileWatcher.DisplayLogEntriesInListBox(listBox_Output);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Starten der Überwachung: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OnFileHandled(string message)
        {
            // GUI-Komponente aktualisieren
            Invoke(new Action(() =>
            {
                listBox_Output.Items.Add(message);
            }));
        }
        private void btn_Exocad_Ordner_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_Exocad_Constructions.Text = selectedPath;
                Properties.Settings.Default.ExocadConstructions = selectedPath;
                Properties.Settings.Default.Save();
            }
        }
        private void btn_ExoCad_ZielOrdner_Click(object sender, EventArgs e)
        {
            string selectedPath = GetSelectedFolderPath();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                txt_dentalCAD.Text = selectedPath;
                Properties.Settings.Default.ExocaddentalCAD = selectedPath;
                Properties.Settings.Default.Save();
            }
        }
        private string GetSelectedFolderPath()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    return fbd.SelectedPath;
                }
            }
            return null;
        }
    }
}

