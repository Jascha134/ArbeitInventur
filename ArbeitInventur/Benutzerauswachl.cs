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
    public partial class Benutzerauswachl : Form
    {
        public static Benutzerauswachl Instance { get; set; }
        public Benutzerauswachl()
        {
            Instance = this;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Benutzer benutzer = new Benutzer();
            if(!string.IsNullOrEmpty(comboBox1.Text) )
            {
                benutzer.Name = comboBox1.Text;

                if(checkBox1.Checked)
                {
                    Properties.Settings.Default.BenutzerMerken = true;
                    Properties.Settings.Default.BenutzerGemerkt = comboBox1.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.BenutzerMerken = false;
                    Properties.Settings.Default.BenutzerGemerkt = "";
                    Properties.Settings.Default.Save();
                }

                Form1 form1 = new Form1(benutzer);
                form1.ShowDialog();
            }
        }

        private void Benutzerauswachl_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Properties.Settings.Default.BenutzerMerken;
            if(checkBox1.Checked )
            {
                comboBox1.Text = Properties.Settings.Default.BenutzerGemerkt;
            }
        }
    }
}
