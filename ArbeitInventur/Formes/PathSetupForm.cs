using System;
using System.Windows.Forms;
using System.IO;

namespace ArbeitInventur
{
    public partial class PathSetupForm : Form
    {
        public PathSetupForm()
        {
            InitializeComponent();
            InitializePaths();
        }

        private void InitializeComponent()
        {
            this.Text = "Pfad-Konfiguration";
            this.Width = 500;
            this.Height = 350;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Labels und TextBoxen
            Label lblDataJSON = new Label { Text = "DataJSON Pfad:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            TextBox txtDataJSON = new TextBox { Name = "txtDataJSON", Location = new System.Drawing.Point(150, 20), Width = 300 };
            Button btnDataJSON = new Button { Text = "...", Location = new System.Drawing.Point(460, 20), Width = 30, Height = 25 };
            btnDataJSON.Click += (s, e) => SelectFolder(txtDataJSON);

            Label lblExocadConstructions = new Label { Text = "Exocad Constructions:", Location = new System.Drawing.Point(20, 60), AutoSize = true };
            TextBox txtExocadConstructions = new TextBox { Name = "txtExocadConstructions", Location = new System.Drawing.Point(150, 60), Width = 300 };
            Button btnExocadConstructions = new Button { Text = "...", Location = new System.Drawing.Point(460, 60), Width = 30, Height = 25 };
            btnExocadConstructions.Click += (s, e) => SelectFolder(txtExocadConstructions);

            Label lblExocaddentalCAD = new Label { Text = "Exocad DentalCAD:", Location = new System.Drawing.Point(20, 100), AutoSize = true };
            TextBox txtExocaddentalCAD = new TextBox { Name = "txtExocaddentalCAD", Location = new System.Drawing.Point(150, 100), Width = 300 };
            Button btnExocaddentalCAD = new Button { Text = "...", Location = new System.Drawing.Point(460, 100), Width = 30, Height = 25 };
            btnExocaddentalCAD.Click += (s, e) => SelectFolder(txtExocaddentalCAD);

            Label lblLocalScanFolder = new Label { Text = "Lokaler Scan-Ordner:", Location = new System.Drawing.Point(20, 140), AutoSize = true };
            TextBox txtLocalScanFolder = new TextBox { Name = "txtLocalScanFolder", Location = new System.Drawing.Point(150, 140), Width = 300 };
            Button btnLocalScanFolder = new Button { Text = "...", Location = new System.Drawing.Point(460, 140), Width = 30, Height = 25 };
            btnLocalScanFolder.Click += (s, e) => SelectFolder(txtLocalScanFolder);

            Label lblServerTargetFolder = new Label { Text = "Server-Zielordner:", Location = new System.Drawing.Point(20, 180), AutoSize = true };
            TextBox txtServerTargetFolder = new TextBox { Name = "txtServerTargetFolder", Location = new System.Drawing.Point(150, 180), Width = 300 };
            Button btnServerTargetFolder = new Button { Text = "...", Location = new System.Drawing.Point(460, 180), Width = 30, Height = 25 };
            btnServerTargetFolder.Click += (s, e) => SelectFolder(txtServerTargetFolder);

            // OK-Button
            Button btnOK = new Button { Text = "OK", Location = new System.Drawing.Point(150, 220), Width = 100, Height = 30 };
            btnOK.Click += BtnOK_Click;

            // Cancel-Button
            Button btnCancel = new Button { Text = "Abbrechen", Location = new System.Drawing.Point(260, 220), Width = 100, Height = 30 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            this.Controls.AddRange(new Control[] { lblDataJSON, txtDataJSON, btnDataJSON, lblExocadConstructions, txtExocadConstructions, btnExocadConstructions,
                lblExocaddentalCAD, txtExocaddentalCAD, btnExocaddentalCAD, lblLocalScanFolder, txtLocalScanFolder, btnLocalScanFolder,
                lblServerTargetFolder, txtServerTargetFolder, btnServerTargetFolder, btnOK, btnCancel });
        }

        private void InitializePaths()
        {
            TextBox txtDataJSON = Controls["txtDataJSON"] as TextBox;
            TextBox txtExocadConstructions = Controls["txtExocadConstructions"] as TextBox;
            TextBox txtExocaddentalCAD = Controls["txtExocaddentalCAD"] as TextBox;
            TextBox txtLocalScanFolder = Controls["txtLocalScanFolder"] as TextBox;
            TextBox txtServerTargetFolder = Controls["txtServerTargetFolder"] as TextBox;

            txtDataJSON.Text = Properties.Settings.Default.DataJSON ?? @"C:\DefaultData";
            txtExocadConstructions.Text = Properties.Settings.Default.ExocadConstructions ?? @"C:\Exocad\Constructions";
            txtExocaddentalCAD.Text = Properties.Settings.Default.ExocaddentalCAD ?? @"C:\Exocad\DentalCAD";
            txtLocalScanFolder.Text = Properties.Settings.Default.LocalScanFolder ?? @"C:\DefaultScanFolder";
            txtServerTargetFolder.Text = Properties.Settings.Default.ServerTargetFolder ?? @"\\Server\DefaultTarget";
        }

        private void SelectFolder(TextBox textBox)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            TextBox txtDataJSON = Controls["txtDataJSON"] as TextBox;
            TextBox txtExocadConstructions = Controls["txtExocadConstructions"] as TextBox;
            TextBox txtExocaddentalCAD = Controls["txtExocaddentalCAD"] as TextBox;
            TextBox txtLocalScanFolder = Controls["txtLocalScanFolder"] as TextBox;
            TextBox txtServerTargetFolder = Controls["txtServerTargetFolder"] as TextBox;

            if (string.IsNullOrWhiteSpace(txtDataJSON.Text) || string.IsNullOrWhiteSpace(txtExocadConstructions.Text) ||
                string.IsNullOrWhiteSpace(txtExocaddentalCAD.Text) || string.IsNullOrWhiteSpace(txtLocalScanFolder.Text) ||
                string.IsNullOrWhiteSpace(txtServerTargetFolder.Text))
            {
                MessageBox.Show("Alle Pfade müssen ausgefüllt sein!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Properties.Settings.Default.DataJSON = txtDataJSON.Text;
            Properties.Settings.Default.ExocadConstructions = txtExocadConstructions.Text;
            Properties.Settings.Default.ExocaddentalCAD = txtExocaddentalCAD.Text;
            Properties.Settings.Default.LocalScanFolder = txtLocalScanFolder.Text;
            Properties.Settings.Default.ServerTargetFolder = txtServerTargetFolder.Text;
            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
