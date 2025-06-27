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
        public void SelectProductById(int productId)
        {
            var selectedSystemName = cbSystems.SelectedItem?.ToString();
            if (selectedSystemName == null) return;
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);
            if (selectedSystem == null) return;
            var filteredDetails = selectedSystem.Details
                .Where(d => d != null && !string.IsNullOrEmpty(d.Beschreibung))
                .ToList();
            int index = filteredDetails.FindIndex(d => d.Id == productId);
            if (index >= 0)
            {
                cbProducts.SelectedIndex = index;
            }
        }

        public void UpdateProductsComboBox()
        {
            cbProducts.Items.Clear();
            var selectedSystemName = cbSystems.SelectedItem?.ToString();
            if (selectedSystemName == null) return;
            var selectedSystem = _implantatsysteme.FirstOrDefault(s => s.SystemName == selectedSystemName);
            if (selectedSystem == null) return;
            var validDetails = selectedSystem.Details
                .Where(d => d != null && !string.IsNullOrEmpty(d.Beschreibung))
                .ToList();
            foreach (var detail in validDetails)
            {
                cbProducts.Items.Add($"{detail.Beschreibung} (ID: {detail.Id})");
            }
        }
    }
}