using System;
using System.IO;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class Uc_Settings : UserControl
    {
        public Uc_Settings()
        {
            InitializeComponent();
        }

        private void Uc_Settings_Load(object sender, EventArgs e)
        {
            txt_DataOrderJSON.Text = Properties.Settings.Default.DataJSON;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.Exists(fbd.SelectedPath))
                    {
                        txt_DataOrderJSON.Text = fbd.SelectedPath;
                        try
                        {
                            Properties.Settings.Default.DataJSON = txt_DataOrderJSON.Text;
                            Properties.Settings.Default.Save();
                            MessageBox.Show("Einstellungen erfolgreich gespeichert.", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Fehler beim Speichern der Einstellungen: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Der ausgewählte Ordner existiert nicht.", "Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void txt_DataOrderJSON_TextChanged(object sender, EventArgs e)
        {
            // Optional: Echtzeit-Validierung des Pfads
            if (!string.IsNullOrWhiteSpace(txt_DataOrderJSON.Text) && !Directory.Exists(txt_DataOrderJSON.Text))
            {
                txt_DataOrderJSON.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                txt_DataOrderJSON.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void lblDataOrderJSON_Click(object sender, EventArgs e)
        {
            // Optional: Kann für zusätzliche Funktionalität verwendet werden
        }
    }
}