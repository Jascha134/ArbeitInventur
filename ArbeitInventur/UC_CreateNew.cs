using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class UC_CreateNew : UserControl
    {
        private readonly List<ProduktFirma> _implantatsysteme;
        public CheckBox ChkNewSystem => chkNewSystem;
        public TextBox TxtNewSystemName => txtNewSystemName;
        public ComboBox CbExistingSystems => cbExistingSystems;
        public CheckBox ChkNewCategory => chkNewCategory;
        public TextBox TxtKategorie => txtKategorie;
        public ComboBox CbCategories => cbCategories;
        public TextBox TxtBeschreibung => txtBeschreibung;
        public TextBox TxtMenge => txtMenge;
        public TextBox TxtMindestbestand => txtMindestbestand;

        public UC_CreateNew(List<ProduktFirma> implantatsysteme)
        {
            InitializeComponent();
            _implantatsysteme = implantatsysteme ?? new List<ProduktFirma>();

            // Kategorien laden
            var categories = _implantatsysteme.SelectMany(s => s.Details.Select(d => d.Kategorie)).Distinct().OrderBy(c => c).ToArray();
            txtKategorie.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtKategorie.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtKategorie.AutoCompleteCustomSource.AddRange(categories);
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

            // Systeme laden
            if (_implantatsysteme.Any())
            {
                cbExistingSystems.Items.AddRange(_implantatsysteme.Select(s => s.SystemName).ToArray());
                if (cbExistingSystems.Items.Count > 0) cbExistingSystems.SelectedIndex = 0;
            }
            else
            {
                chkNewSystem.Checked = true;
                chkNewSystem.Enabled = false;
            }

            // Standardwerte
            txtMenge.Text = "1";
            txtMindestbestand.Text = "0";

            chkNewSystem.CheckedChanged += (s, e) => ToggleSystemSelection();
            chkNewCategory.CheckedChanged += (s, e) => ToggleCategorySelection();
            ToggleSystemSelection();
            ToggleCategorySelection();
        }

        private void ToggleSystemSelection()
        {
            txtNewSystemName.Visible = chkNewSystem.Checked;
            cbExistingSystems.Visible = !chkNewSystem.Checked;
            lblNewSystem.Visible = true;
        }

        private void ToggleCategorySelection()
        {
            txtKategorie.Visible = chkNewCategory.Checked;
            cbCategories.Visible = !chkNewCategory.Checked;
            lblKategorie.Visible = true;
        }
    }
}