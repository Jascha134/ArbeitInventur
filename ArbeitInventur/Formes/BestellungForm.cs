using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace ArbeitInventur
{
    public partial class BestellungForm : Form
    {
        private ProduktFirma selectedSystem;
        private List<ProduktDetail> bestellteProdukte;

        public BestellungForm(ProduktFirma system)
        {
            InitializeComponent();
            selectedSystem = system;
            bestellteProdukte = new List<ProduktDetail>();

            // Füge Produkte hinzu, die unter dem Mindestbestand sind
            var unterMindestbestand = selectedSystem.Details
                .Where(d => d.Menge < d.Mindestbestand)
                .ToList();
            bestellteProdukte.AddRange(unterMindestbestand);

            // Zeige sie in einem DataGridView an
            dataGridViewBestellung.DataSource = bestellteProdukte;
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dataGridViewBestellung.AutoGenerateColumns = true;
            if (dataGridViewBestellung.Columns.Contains("Id"))
                dataGridViewBestellung.Columns["Id"].Visible = false;
            if (dataGridViewBestellung.Columns.Contains("Barcode"))
                dataGridViewBestellung.Columns["Barcode"].Visible = false;
            if (dataGridViewBestellung.Columns.Contains("ProduktId"))
                dataGridViewBestellung.Columns["ProduktId"].Visible = false;
            if (dataGridViewBestellung.Columns.Contains("Produktionsdatum"))
                dataGridViewBestellung.Columns["Produktionsdatum"].Visible = false;
            if (dataGridViewBestellung.Columns.Contains("LotNummer"))
                dataGridViewBestellung.Columns["LotNummer"].Visible = false;
        }

        public void AddProductToOrder(ProduktDetail product)
        {
            if (!bestellteProdukte.Contains(product))
            {
                bestellteProdukte.Add(product);
                dataGridViewBestellung.DataSource = null;
                dataGridViewBestellung.DataSource = bestellteProdukte;
                ConfigureDataGridView();
            }
        }

        private void btnSenden_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSystem.LieferantEmail))
            {
                MessageBox.Show("Keine E-Mail-Adresse für den Lieferanten angegeben.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!bestellteProdukte.Any())
            {
                MessageBox.Show("Keine Produkte in der Bestellung.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Bestellung für: {selectedSystem.SystemName}");
            sb.AppendLine($"Kontaktperson: {selectedSystem.KontaktPerson ?? "N/A"}");
            sb.AppendLine($"Lieferadresse: {selectedSystem.Lieferadresse ?? "N/A"}");
            sb.AppendLine("Produkte:");
            sb.AppendLine("-------------------------");
            foreach (var produkt in bestellteProdukte)
            {
                int bestellMenge = produkt.Mindestbestand - produkt.Menge > 0 ? produkt.Mindestbestand - produkt.Menge : 1;
                sb.AppendLine($"{produkt.Beschreibung} | Menge: {bestellMenge}");
            }
            sb.AppendLine("-------------------------");

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("deine-email@example.com"); // Anpassen
                    mail.To.Add(selectedSystem.LieferantEmail);
                    mail.Subject = $"Neue Bestellung von {selectedSystem.SystemName}";
                    mail.Body = sb.ToString();

                    using (SmtpClient smtp = new SmtpClient("smtp.example.com", 587)) // Anpassen
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential("deine-email@example.com", "deinPasswort"); // Anpassen
                        smtp.Send(mail);
                    }
                }
                MessageBox.Show("Bestellung erfolgreich versendet!", "Erfolg", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Senden der Bestellung: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}