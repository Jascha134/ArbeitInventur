using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class InventoryActionForm : Form
    {
        private readonly ProduktFirma selectedSystem;
        private readonly ProduktDetail product;
        private readonly ProduktManager manager;
        private readonly List<ProduktFirma> implantatsysteme;
        private readonly LogHandler logHandler;

        public InventoryActionForm(ProduktFirma system, ProduktDetail product, ProduktManager manager, List<ProduktFirma> implantatsysteme, LogHandler logHandler)
        {
            InitializeComponent();
            this.selectedSystem = system;
            this.product = product;
            this.manager = manager;
            this.implantatsysteme = implantatsysteme;
            this.logHandler = logHandler;

            lblInfo.Text = $"Produkt: {product.Beschreibung}\nAktuelle Menge: {product.Menge}\nMindestbestand: {product.Mindestbestand}\nProdukt-ID: {product.ProduktId}\nLot: {product.LotNummer}\nProduktionsdatum: {(product.Produktionsdatum?.ToString("dd.MM.yyyy") ?? "N/A")}";
            txtQuantity.Text = product.Menge.ToString();
        }

        private void btnEinlagern_Click(object sender, EventArgs e)
        {
            AdjustQuantity(1);
        }

        private void btnAuslagern_Click(object sender, EventArgs e)
        {
            AdjustQuantity(-1);
        }

        private async void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text, out int newQuantity) || newQuantity < 0)
            {
                MessageBox.Show("Bitte geben Sie eine gültige Menge ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int change = newQuantity - product.Menge;
            product.Menge = newQuantity;
            await manager.SpeichereImplantatsystemeAsync(implantatsysteme);
            logHandler.LogAction($"Produkt {product.Beschreibung}: Menge {(change > 0 ? "erhöht" : "reduziert")} um {Math.Abs(change)}. Neue Menge: {product.Menge}");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void AdjustQuantity(int change)
        {
            if (int.TryParse(txtQuantity.Text, out int currentQuantity))
            {
                int newQuantity = Math.Max(0, currentQuantity + change);
                txtQuantity.Text = newQuantity.ToString();
            }
        }
    }
}