using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArbeitInventur.Formes
{
    public partial class UC_AddToExisting : UserControl
    {
        private readonly List<ProduktFirma> _implantatsysteme;
        public ComboBox CbSystems => cbSystems;
        public ComboBox CbProducts => cbProducts;

        public UC_AddToExisting(List<ProduktFirma> implantatsysteme)
        {
            InitializeComponent();
            _implantatsysteme = implantatsysteme ?? new List<ProduktFirma>();

            if (_implantatsysteme.Any())
            {
                cbSystems.Items.AddRange(_implantatsysteme.Select(s => s.SystemName).ToArray());
                if (cbSystems.Items.Count > 0) cbSystems.SelectedIndex = 0;
            }

            cbSystems.SelectedIndexChanged += (s, e) => UpdateProductsComboBox();
            UpdateProductsComboBox();
        }

        public void UpdateProductsComboBox()
        {
            cbProducts.Items.Clear();

            if (_implantatsysteme == null || !_implantatsysteme.Any() || cbSystems.SelectedIndex < 0)
                return;

            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == cbSystems.SelectedItem?.ToString());
            if (selectedSystem?.Details == null)
                return;

            var validDescriptions = selectedSystem.Details
                .Where(d => d != null && !string.IsNullOrEmpty(d.Beschreibung))
                .Select(d => d.Beschreibung)
                .ToArray();

            cbProducts.Items.AddRange(validDescriptions);
            if (cbProducts.Items.Count > 0)
                cbProducts.SelectedIndex = 0;
        }
    }
}