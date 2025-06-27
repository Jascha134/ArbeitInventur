using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class ErrorStart : Form
    {
        public ErrorStart()
        {
            InitializeComponent();
            txt_Hauptverzeichnis.Text = Properties.Settings.Default.DataJSON; // Voreinstellung des Textfeldes mit dem gespeicherten Pfad
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DataJSON = txt_Hauptverzeichnis.Text;
            Properties.Settings.Default.Save(); // Speichern der Einstellungen
            Application.Restart(); // Anwendung neu starten, um die Änderungen zu übernehmen
        }
    }
}
