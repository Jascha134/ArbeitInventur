using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur
{
    internal static class DGVStyle
    {
        public static void Dgv(DataGridView dgv)
        {
            // Row Header ausblenden
            dgv.RowHeadersVisible = false;

            // Zeilen gleichmäßig groß machen
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Zellen schreibgeschützt machen
            dgv.ReadOnly = true;

            // Spaltenbreite automatisch anpassen, um Platz gleichmäßig zu nutzen
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Schriftgröße anpassen
            dgv.DefaultCellStyle.Font = new Font("Arial", 12F);
        }
    }
}
