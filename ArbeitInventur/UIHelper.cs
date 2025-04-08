using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public static class UIHelper
    {
        private static ToolTip toolTip = new ToolTip();

        /// <summary>
        /// Zeigt einen ToolTip mit dem vollständigen Text an, falls der Inhalt einer TextBox abgeschnitten ist.
        /// </summary>
        /// <param name="textBox">Die TextBox, für die der Tooltip angezeigt werden soll.</param>
        public static void ShowToolTipIfTextTruncated(TextBox textBox)
        {
            if (textBox == null || string.IsNullOrWhiteSpace(textBox.Text))
                return;

            using (Graphics g = textBox.CreateGraphics())
            {
                int textWidth = (int)g.MeasureString(textBox.Text, textBox.Font).Width;

                if (textWidth > textBox.Width - 5) // 5px Puffer für Scrollbar
                {
                    toolTip.Show(textBox.Text, textBox, textBox.Width, textBox.Height, 3000);
                }
            }
        }

        /// <summary>
        /// Versteckt den aktuell angezeigten ToolTip.
        /// </summary>
        /// <param name="control">Das Control, zu dem der ToolTip gehört.</param>
        public static void HideToolTip(Control control)
        {
            toolTip.Hide(control);
        }
    }
}
