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

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txt_DataOrderJSON.Text = fbd.SelectedPath;
                }
                Properties.Settings.Default.DataJSON = txt_DataOrderJSON.Text;
                Properties.Settings.Default.Save();
            }
        }
    }
}
