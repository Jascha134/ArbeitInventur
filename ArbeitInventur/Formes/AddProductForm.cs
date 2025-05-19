using ArbeitInventur.Barcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class AddProductForm : Form
    {
        private readonly List<ProduktFirma> implantatsysteme;
        private readonly ProduktManager manager;
        private readonly LogHandler logHandler;
        private readonly string barcode;
        private readonly string produktId;
        private readonly DateTime? produktionsdatum;
        private readonly string lotNummer;

        public ProduktDetail AddedProduct { get; private set; }
        public ProduktFirma SelectedSystem { get; private set; }

        public AddProductForm(string barcode, string produktId, DateTime? produktionsdatum, string lotNummer, List<ProduktFirma> implantatsysteme, ProduktManager manager, LogHandler logHandler)
        {
            InitializeComponent();
            this.barcode = barcode;
            this.produktId = produktId;
            this.produktionsdatum = produktionsdatum;
            this.lotNummer = lotNummer;
            this.implantatsysteme = implantatsysteme ?? new List<ProduktFirma>();
            this.manager = manager;
            this.logHandler = logHandler;

            lblBarcode.Text = $"Barcode: {barcode}";
            lb_ProductID.Text = $"Produkt ID: {produktId}";
            lb_Productiondate.Text = $"Produktionsdatum: {produktionsdatum?.ToString("dd.MM.yyyy") ?? "N/A"}";
            lb_Lot.Text = $"Lotnummer: {lotNummer}";

            // Autovervollständigung für Kategorien
            var categories = implantatsysteme.SelectMany(s => s.Details.Select(d => d.Kategorie)).Distinct().OrderBy(c => c).ToArray();
            txtKategorie.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtKategorie.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtKategorie.AutoCompleteCustomSource.AddRange(categories);

            // Kategorien in ComboBox laden
            cbCategories.Items.AddRange(categories);
            if (categories.Length == 0)
            {
                chkNewCategory.Checked = true;
                chkNewCategory.Enabled = false;
            }
            else if (cbCategories.Items.Count > 0)
            {
                cbCategories.SelectedIndex = 0;
            }

            // Standardwerte
            txtMenge.Text = "1";
            txtMindestbestand.Text = "0";

            if (implantatsysteme.Any())
            {
                cbSystems.Items.AddRange(implantatsysteme.Select(s => s.SystemName).ToArray());
                cbExistingSystems.Items.AddRange(implantatsysteme.Select(s => s.SystemName).ToArray());
                if (cbSystems.Items.Count > 0) cbSystems.SelectedIndex = 0;
                if (cbExistingSystems.Items.Count > 0) cbExistingSystems.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Keine Systeme vorhanden. Bitte legen Sie zuerst ein System an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rbAddToExisting.Enabled = false;
                rbCreateNew.Checked = true;
            }

            // Ereignishandler
            rbAddToExisting.CheckedChanged += rbAddToExisting_CheckedChanged;
            rbCreateNew.CheckedChanged += rbCreateNew_CheckedChanged;
            chkNewSystem.CheckedChanged += chkNewSystem_CheckedChanged;
            chkNewCategory.CheckedChanged += chkNewCategory_CheckedChanged;
            cbSystems.SelectedIndexChanged += (s, e) => UpdateProductsComboBox();
            btnOk.Click += (s, e) => BtnOk_Click();
            btnCancel.Click += (s, e) => Close();

            UpdateControlVisibility();
        }

        private void UpdateControlVisibility()
        {
            bool addToExisting = rbAddToExisting.Checked;
            bool creatingNew = rbCreateNew.Checked;

            lblSystem.Visible = addToExisting;
            cbSystems.Visible = addToExisting;
            lblProduct.Visible = addToExisting;
            cbProducts.Visible = addToExisting;
            chkNewCategory.Visible = creatingNew;
            lblBeschreibung.Visible = creatingNew;
            txtBeschreibung.Visible = creatingNew;
            lblMenge.Visible = creatingNew;
            txtMenge.Visible = creatingNew;
            lblMindestbestand.Visible = creatingNew;
            txtMindestbestand.Visible = creatingNew;

            chkNewSystem.Visible = creatingNew;
            if (creatingNew)
            {
                ToggleSystemSelection();
                ToggleCategorySelection();
            }
            else
            {
                txtNewSystemName.Visible = false;
                cbExistingSystems.Visible = false;
                cbCategories.Visible = false;
                txtKategorie.Visible = false;
            }
        }

        private void ToggleSystemSelection()
        {
            txtNewSystemName.Visible = chkNewSystem.Checked;
            cbExistingSystems.Visible = !chkNewSystem.Checked;
        }

        private void ToggleCategorySelection()
        {
            txtKategorie.Visible = chkNewCategory.Checked;
            cbCategories.Visible = !chkNewCategory.Checked;
        }

        private void rbAddToExisting_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlVisibility();
        }

        private void rbCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlVisibility();
        }

        private void chkNewSystem_CheckedChanged(object sender, EventArgs e)
        {
            ToggleSystemSelection();
        }

        private void chkNewCategory_CheckedChanged(object sender, EventArgs e)
        {
            ToggleCategorySelection();
        }

        private void UpdateProductsComboBox()
        {
            if (cbProducts == null)
            {
                MessageBox.Show("Die ComboBox 'cbProducts' wurde nicht initialisiert.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cbProducts.Items.Clear();

            if (implantatsysteme == null || !implantatsysteme.Any())
            {
                return;
            }

            if (cbSystems == null || cbSystems.SelectedIndex < 0 || cbSystems.SelectedItem == null)
            {
                return;
            }

            var selectedSystem = implantatsysteme.FirstOrDefault(s => s.SystemName == cbSystems.SelectedItem.ToString());
            if (selectedSystem == null)
            {
                return;
            }

            if (selectedSystem.Details == null)
            {
                selectedSystem.Details = new List<ProduktDetail>();
            }

            var validDescriptions = selectedSystem.Details
                .Where(d => d != null && !string.IsNullOrEmpty(d.Beschreibung))
                .Select(d => d.Beschreibung)
                .ToArray();

            cbProducts.Items.AddRange(validDescriptions);
            if (cbProducts.Items.Count > 0)
            {
                cbProducts.SelectedIndex = 0;
            }
        }

        private async void BtnOk_Click()
        {
            if (rbAddToExisting.Checked)
            {
                if (cbSystems.SelectedIndex < 0 || cbProducts.SelectedIndex < 0)
                {
                    MessageBox.Show("Bitte wählen Sie ein System und ein Produkt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedSystem = implantatsysteme.FirstOrDefault(s => s.SystemName == cbSystems.SelectedItem.ToString());
                if (selectedSystem != null)
                {
                    var selectedProduct = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == cbProducts.SelectedItem.ToString());
                    if (selectedProduct != null)
                    {
                        selectedProduct.Barcode = barcode;
                        selectedProduct.ProduktId = produktId;
                        selectedProduct.Produktionsdatum = produktionsdatum;
                        selectedProduct.LotNummer = lotNummer;
                        AddedProduct = selectedProduct;
                        SelectedSystem = selectedSystem;
                        logHandler.LogAction($"QR-Code {barcode} wurde dem Produkt {selectedProduct.Beschreibung} im System {selectedSystem.SystemName} zugewiesen.");
                    }
                }
            }
            else
            {
                if (chkNewSystem.Checked)
                {
                    string newSystemName = txtNewSystemName.Text.Trim();
                    if (string.IsNullOrEmpty(newSystemName))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen für das neue System ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = new ProduktFirma { SystemName = newSystemName };
                    implantatsysteme.Add(SelectedSystem);
                }
                else
                {
                    if (cbExistingSystems.SelectedIndex < 0)
                    {
                        MessageBox.Show("Bitte wählen Sie ein bestehendes System aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = implantatsysteme[cbExistingSystems.SelectedIndex];
                }

                string category;
                if (chkNewCategory.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtKategorie.Text))
                    {
                        MessageBox.Show("Bitte geben Sie eine neue Kategorie ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = txtKategorie.Text;
                }
                else
                {
                    if (cbCategories.SelectedItem == null)
                    {
                        MessageBox.Show("Bitte wählen Sie eine bestehende Kategorie aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = cbCategories.SelectedItem.ToString();
                }

                if (string.IsNullOrWhiteSpace(txtBeschreibung.Text) ||
                    !int.TryParse(txtMenge.Text, out int menge) || !int.TryParse(txtMindestbestand.Text, out int mindestbestand))
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder korrekt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newProduct = new ProduktDetail
                {
                    Kategorie = category,
                    Beschreibung = txtBeschreibung.Text,
                    Menge = menge,
                    Mindestbestand = mindestbestand,
                    Barcode = barcode,
                    ProduktId = produktId,
                    Produktionsdatum = produktionsdatum,
                    LotNummer = lotNummer
                };

                if (SelectedSystem.Details == null)
                {
                    SelectedSystem.Details = new List<ProduktDetail>();
                }
                SelectedSystem.Details.Add(newProduct);
                AddedProduct = newProduct;
                logHandler.LogAction($"Neues Produkt {newProduct.Beschreibung} mit QR-Code {barcode} im System {SelectedSystem.SystemName} erstellt.");
            }

            await manager.SpeichereImplantatsystemeAsync(implantatsysteme);
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (rbAddToExisting.Checked)
            {
                if (cbSystems.SelectedIndex < 0 || cbProducts.SelectedIndex < 0)
                {
                    MessageBox.Show("Bitte wählen Sie ein System und ein Produkt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedSystem = implantatsysteme.FirstOrDefault(s => s.SystemName == cbSystems.SelectedItem.ToString());
                if (selectedSystem != null)
                {
                    var selectedProduct = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == cbProducts.SelectedItem.ToString());
                    if (selectedProduct != null)
                    {
                        // Prüfen, ob ein Produkt mit gleichem Barcode oder Produkt-ID, aber anderer Lot-Nummer existiert
                        var existingProduct = selectedSystem.Details.FirstOrDefault(d =>
                            (d.Barcode == barcode || d.ProduktId == produktId) && d.LotNummer != lotNummer);
                        if (existingProduct != null)
                        {
                            // Neues ProduktDetail für andere Lot-Nummer erstellen
                            var newProduct = new ProduktDetail
                            {
                                Kategorie = selectedProduct.Kategorie,
                                Beschreibung = selectedProduct.Beschreibung,
                                Menge = 1,
                                Mindestbestand = selectedProduct.Mindestbestand,
                                Barcode = barcode,
                                ProduktId = produktId,
                                Produktionsdatum = produktionsdatum,
                                LotNummer = lotNummer
                            };
                            selectedSystem.Details.Add(newProduct);
                            AddedProduct = newProduct;
                            logHandler.LogAction($"Neues Produkt {newProduct.Beschreibung} mit QR-Code {barcode} und Lot-Nummer {lotNummer} im System {selectedSystem.SystemName} erstellt.");
                        }
                        else
                        {
                            // Bestehendes Produkt aktualisieren
                            selectedProduct.Barcode = barcode;
                            selectedProduct.ProduktId = produktId;
                            selectedProduct.Produktionsdatum = produktionsdatum;
                            selectedProduct.LotNummer = lotNummer;
                            AddedProduct = selectedProduct;
                            logHandler.LogAction($"QR-Code {barcode} wurde dem Produkt {selectedProduct.Beschreibung} im System {selectedSystem.SystemName} zugewiesen.");
                        }
                    }
                }
            }
            else
            {
                // Bestehende Logik für neues Produkt bleibt weitgehend gleich
                if (chkNewSystem.Checked)
                {
                    string newSystemName = txtNewSystemName.Text.Trim();
                    if (string.IsNullOrEmpty(newSystemName))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen für das neue System ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = new ProduktFirma { SystemName = newSystemName };
                    implantatsysteme.Add(SelectedSystem);
                }
                else
                {
                    if (cbExistingSystems.SelectedIndex < 0)
                    {
                        MessageBox.Show("Bitte wählen Sie ein bestehendes System aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = implantatsysteme[cbExistingSystems.SelectedIndex];
                }

                string category;
                if (chkNewCategory.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtKategorie.Text))
                    {
                        MessageBox.Show("Bitte geben Sie eine neue Kategorie ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = txtKategorie.Text;
                }
                else
                {
                    if (cbCategories.SelectedItem == null)
                    {
                        MessageBox.Show("Bitte wählen Sie eine bestehende Kategorie aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = cbCategories.SelectedItem.ToString();
                }

                if (string.IsNullOrWhiteSpace(txtBeschreibung.Text) ||
                    !int.TryParse(txtMenge.Text, out int menge) || !int.TryParse(txtMindestbestand.Text, out int mindestbestand))
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder korrekt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newProduct = new ProduktDetail
                {
                    Kategorie = category,
                    Beschreibung = txtBeschreibung.Text,
                    Menge = menge,
                    Mindestbestand = mindestbestand,
                    Barcode = barcode,
                    ProduktId = produktId,
                    Produktionsdatum = produktionsdatum,
                    LotNummer = lotNummer
                };

                if (SelectedSystem.Details == null)
                {
                    SelectedSystem.Details = new List<ProduktDetail>();
                }
                SelectedSystem.Details.Add(newProduct);
                AddedProduct = newProduct;
                logHandler.LogAction($"Neues Produkt {newProduct.Beschreibung} mit QR-Code {barcode} im System {SelectedSystem.SystemName} erstellt.");
            }

            await manager.SpeichereImplantatsystemeAsync(implantatsysteme);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}