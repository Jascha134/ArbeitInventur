using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class AddProductForm : Form
    {
        private readonly List<ProduktFirma> _implantatsysteme;
        private readonly ProduktManager _manager;
        private readonly LogHandler _logHandler;
        private readonly string _barcode;
        private readonly string _produktId;
        private readonly DateTime? _produktionsdatum;
        private readonly string _lotNummer;
        private readonly int? _productId;
        private readonly string _systemName;
        private UC_AddToExisting _ucAddToExisting;
        private UC_CreateNew _ucCreateNew;

        public ProduktDetail AddedProduct { get; private set; }
        public ProduktFirma SelectedSystem { get; private set; }

        public AddProductForm(string barcode, string produktId, DateTime? produktionsdatum, string lotNummer, List<ProduktFirma> implantatsysteme, ProduktManager manager, LogHandler logHandler, string rbCreateNewName, bool rbAddToExistingEnable, int? productId = null, string systemName = null)
        {
            InitializeComponent();
            _barcode = barcode;
            _produktId = produktId;
            _produktionsdatum = produktionsdatum;
            _lotNummer = lotNummer;
            _productId = productId;
            _systemName = systemName;
            _implantatsysteme = implantatsysteme ?? new List<ProduktFirma>();
            _manager = manager;
            _logHandler = logHandler;

            rbAddToExisting.Visible = rbAddToExistingEnable;
            if (!rbAddToExistingEnable)
            {
                rbAddToExisting.Checked = false;
                rbCreateNew.Checked = true;
            }

            rbCreateNew.Text = rbCreateNewName;
            lblBarcode.Text = $"Barcode: {barcode ?? "N/A"}";
            lb_ProductID.Text = $"Produkt ID: {produktId ?? "N/A"}";
            lb_Productiondate.Text = $"Produktionsdatum: {produktionsdatum?.ToString("dd.MM.yyyy") ?? "N/A"}";
            lb_Lot.Text = $"Lotnummer: {lotNummer ?? "N/A"}";

            _ucAddToExisting = new UC_AddToExisting(_implantatsysteme) { Dock = DockStyle.Fill };
            _ucCreateNew = new UC_CreateNew(_implantatsysteme) { Dock = DockStyle.Fill };

            if (_productId.HasValue && _systemName != null)
            {
                var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == _systemName);
                if (selectedSystem != null)
                {
                    var existingProduct = selectedSystem.Details.FirstOrDefault(d => d.Id == _productId.Value);
                    if (existingProduct != null)
                    {
                        var system = _implantatsysteme.FirstOrDefault(s => s.Details.Contains(existingProduct));
                        if (system != null)
                        {
                            _ucAddToExisting.CbSystems.SelectedIndex = _ucAddToExisting.CbSystems.Items.IndexOf(system.SystemName);
                            _ucAddToExisting.UpdateProductsComboBox();
                            _ucAddToExisting.CbProducts.SelectedIndex = _ucAddToExisting.CbProducts.Items.IndexOf(existingProduct.Beschreibung);
                            _ucCreateNew.TxtBeschreibung.Text = existingProduct.Beschreibung;
                            _ucCreateNew.TxtMenge.Text = existingProduct.Menge.ToString();
                            _ucCreateNew.TxtMindestbestand.Text = existingProduct.Mindestbestand.ToString();
                            if (_ucCreateNew.CbCategories.Items.Contains(existingProduct.Kategorie))
                            {
                                _ucCreateNew.CbCategories.SelectedIndex = _ucCreateNew.CbCategories.Items.IndexOf(existingProduct.Kategorie);
                            }
                            else
                            {
                                _ucCreateNew.ChkNewCategory.Checked = true;
                                _ucCreateNew.TxtKategorie.Text = existingProduct.Kategorie;
                            }
                            if (_ucCreateNew.CbExistingSystems.Items.Contains(system.SystemName))
                            {
                                _ucCreateNew.CbExistingSystems.SelectedIndex = _ucCreateNew.CbExistingSystems.Items.IndexOf(system.SystemName);
                            }
                        }
                    }
                }
            }

            if (!_implantatsysteme.Any())
            {
                MessageBox.Show("Keine Systeme vorhanden. Bitte legen Sie zuerst ein System an.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rbAddToExisting.Enabled = false;
                rbCreateNew.Checked = true;
            }

            UpdateUserControl();
        }

        private void RbAddToExisting_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUserControl();
        }

        private void RbCreateNew_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUserControl();
        }

        private void UpdateUserControl()
        {
            _controlPanel.Controls.Clear();
            if (rbAddToExisting.Checked)
            {
                _controlPanel.Size = new System.Drawing.Size(350, 100);
                _controlPanel.Controls.Add(_ucAddToExisting);
            }
            else
            {
                _controlPanel.Size = new System.Drawing.Size(350, 220);
                _controlPanel.Controls.Add(_ucCreateNew);
            }
        }

        private async void BtnOk_Click(object sender, EventArgs e)
        {
            if (rbAddToExisting.Checked)
            {
                if (_ucAddToExisting.CbSystems.SelectedIndex < 0 || _ucAddToExisting.CbProducts.SelectedIndex < 0)
                {
                    MessageBox.Show("Bitte wählen Sie ein System und ein Produkt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == _ucAddToExisting.CbSystems.SelectedItem.ToString());
                if (selectedSystem != null)
                {
                    var selectedProduct = selectedSystem.Details.FirstOrDefault(d => d.Beschreibung == _ucAddToExisting.CbProducts.SelectedItem.ToString());
                    if (selectedProduct != null)
                    {
                        var existingProduct = selectedSystem.Details.FirstOrDefault(d => d.Id == _productId);
                        if (existingProduct != null)
                        {
                            existingProduct.Barcode = _barcode;
                            existingProduct.ProduktId = _produktId;
                            existingProduct.Produktionsdatum = _produktionsdatum;
                            existingProduct.LotNummer = _lotNummer;
                            AddedProduct = existingProduct;
                            _logHandler.LogAction($"Produkt {existingProduct.Beschreibung} (ID: {existingProduct.Id}) im System {selectedSystem.SystemName} aktualisiert.");
                        }
                        else
                        {
                            var newProduct = new ProduktDetail
                            {
                                Id = _manager.GenerateProductId(selectedSystem),
                                Kategorie = selectedProduct.Kategorie,
                                Beschreibung = selectedProduct.Beschreibung,
                                Menge = 1,
                                Mindestbestand = selectedProduct.Mindestbestand,
                                Barcode = _barcode,
                                ProduktId = _produktId,
                                Produktionsdatum = _produktionsdatum,
                                LotNummer = _lotNummer
                            };
                            selectedSystem.Details.Add(newProduct);
                            AddedProduct = newProduct;
                            _logHandler.LogAction($"Neues Produkt {newProduct.Beschreibung} (ID: {newProduct.Id}) mit QR-Code {_barcode} im System {selectedSystem.SystemName} erstellt.");
                        }
                        SelectedSystem = selectedSystem;
                    }
                }
            }
            else
            {
                if (_ucCreateNew.ChkNewSystem.Checked)
                {
                    string newSystemName = _ucCreateNew.TxtNewSystemName.Text.Trim();
                    if (string.IsNullOrEmpty(newSystemName))
                    {
                        MessageBox.Show("Bitte geben Sie einen Namen für das neue System ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = new ProduktFirma { SystemName = newSystemName };
                    _implantatsysteme.Add(SelectedSystem);
                }
                else
                {
                    if (_ucCreateNew.CbExistingSystems.SelectedIndex < 0)
                    {
                        MessageBox.Show("Bitte wählen Sie ein bestehendes System aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    SelectedSystem = _implantatsysteme[_ucCreateNew.CbExistingSystems.SelectedIndex];
                }

                string category;
                if (_ucCreateNew.ChkNewCategory.Checked)
                {
                    if (string.IsNullOrWhiteSpace(_ucCreateNew.TxtKategorie.Text))
                    {
                        MessageBox.Show("Bitte geben Sie eine neue Kategorie ein.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = _ucCreateNew.TxtKategorie.Text;
                }
                else
                {
                    if (_ucCreateNew.CbCategories.SelectedItem == null)
                    {
                        MessageBox.Show("Bitte wählen Sie eine Kategorie aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    category = _ucCreateNew.CbCategories.SelectedItem.ToString();
                }

                if (string.IsNullOrWhiteSpace(_ucCreateNew.TxtBeschreibung.Text) ||
                    !int.TryParse(_ucCreateNew.TxtMenge.Text, out int menge) || !int.TryParse(_ucCreateNew.TxtMindestbestand.Text, out int mindestbestand))
                {
                    MessageBox.Show("Bitte füllen Sie alle Felder korrekt aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_productId.HasValue)
                {
                    var existingProduct = SelectedSystem.Details?.FirstOrDefault(d => d.Id == _productId.Value);
                    if (existingProduct != null)
                    {
                        existingProduct.Kategorie = category;
                        existingProduct.Beschreibung = _ucCreateNew.TxtBeschreibung.Text;
                        existingProduct.Menge = menge;
                        existingProduct.Mindestbestand = mindestbestand;
                        existingProduct.Barcode = _barcode;
                        existingProduct.ProduktId = _produktId;
                        existingProduct.Produktionsdatum = _produktionsdatum;
                        existingProduct.LotNummer = _lotNummer;
                        AddedProduct = existingProduct;
                        _logHandler.LogAction($"Produkt {existingProduct.Beschreibung} (ID: {existingProduct.Id}) im System {SelectedSystem.SystemName} aktualisiert.");
                    }
                }
                else
                {
                    var newProduct = new ProduktDetail
                    {
                        Id = _manager.GenerateProductId(SelectedSystem),
                        Kategorie = category,
                        Beschreibung = _ucCreateNew.TxtBeschreibung.Text,
                        Menge = menge,
                        Mindestbestand = mindestbestand,
                        Barcode = _barcode,
                        ProduktId = _produktId,
                        Produktionsdatum = _produktionsdatum,
                        LotNummer = _lotNummer
                    };
                    if (SelectedSystem.Details == null)
                    {
                        SelectedSystem.Details = new List<ProduktDetail>();
                    }
                    SelectedSystem.Details.Add(newProduct);
                    AddedProduct = newProduct;
                    _logHandler.LogAction($"Neues Produkt {newProduct.Beschreibung} (ID: {newProduct.Id}) mit QR-Code {_barcode} im System {SelectedSystem.SystemName} erstellt.");
                }
            }

            await _manager.SpeichereImplantatsystemeAsync(_implantatsysteme);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}