namespace ArbeitInventur.Formes
{
    partial class AddProductForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.RadioButton rbAddToExisting;
        private System.Windows.Forms.RadioButton rbCreateNew;
        private System.Windows.Forms.CheckBox chkNewSystem;
        private System.Windows.Forms.TextBox txtNewSystemName;
        private System.Windows.Forms.ComboBox cbExistingSystems;
        private System.Windows.Forms.ComboBox cbSystems;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.TextBox txtKategorie;
        private System.Windows.Forms.Label lblBeschreibung;
        private System.Windows.Forms.TextBox txtBeschreibung;
        private System.Windows.Forms.Label lblMenge;
        private System.Windows.Forms.TextBox txtMenge;
        private System.Windows.Forms.Label lblMindestbestand;
        private System.Windows.Forms.TextBox txtMindestbestand;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbExistingCategory;
        private System.Windows.Forms.RadioButton rbNewCategory;
        private System.Windows.Forms.ComboBox cbCategories;
        private System.Windows.Forms.CheckBox chkNewCategory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBarcode = new System.Windows.Forms.Label();
            this.rbAddToExisting = new System.Windows.Forms.RadioButton();
            this.rbCreateNew = new System.Windows.Forms.RadioButton();
            this.chkNewSystem = new System.Windows.Forms.CheckBox();
            this.txtNewSystemName = new System.Windows.Forms.TextBox();
            this.cbExistingSystems = new System.Windows.Forms.ComboBox();
            this.cbSystems = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.txtKategorie = new System.Windows.Forms.TextBox();
            this.lblBeschreibung = new System.Windows.Forms.Label();
            this.txtBeschreibung = new System.Windows.Forms.TextBox();
            this.lblMenge = new System.Windows.Forms.Label();
            this.txtMenge = new System.Windows.Forms.TextBox();
            this.lblMindestbestand = new System.Windows.Forms.Label();
            this.txtMindestbestand = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lb_ProductID = new System.Windows.Forms.Label();
            this.lb_Lot = new System.Windows.Forms.Label();
            this.lb_Productiondate = new System.Windows.Forms.Label();
            this.rbExistingCategory = new System.Windows.Forms.RadioButton();
            this.rbNewCategory = new System.Windows.Forms.RadioButton();
            this.cbCategories = new System.Windows.Forms.ComboBox();
            this.chkNewCategory = new System.Windows.Forms.CheckBox();
            this.lblSystem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(12, 9);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(110, 13);
            this.lblBarcode.TabIndex = 19;
            this.lblBarcode.Text = "Gescanntes Barcode:";
            // 
            // rbAddToExisting
            // 
            this.rbAddToExisting.AutoSize = true;
            this.rbAddToExisting.Checked = true;
            this.rbAddToExisting.Location = new System.Drawing.Point(12, 64);
            this.rbAddToExisting.Name = "rbAddToExisting";
            this.rbAddToExisting.Size = new System.Drawing.Size(185, 17);
            this.rbAddToExisting.TabIndex = 18;
            this.rbAddToExisting.TabStop = true;
            this.rbAddToExisting.Text = "Bestehendem Produkt hinzufügen";
            // 
            // rbCreateNew
            // 
            this.rbCreateNew.AutoSize = true;
            this.rbCreateNew.Location = new System.Drawing.Point(12, 87);
            this.rbCreateNew.Name = "rbCreateNew";
            this.rbCreateNew.Size = new System.Drawing.Size(137, 17);
            this.rbCreateNew.TabIndex = 17;
            this.rbCreateNew.Text = "Neues Produkt anlegen";
            // 
            // chkNewSystem
            // 
            this.chkNewSystem.AutoSize = true;
            this.chkNewSystem.Location = new System.Drawing.Point(12, 114);
            this.chkNewSystem.Name = "chkNewSystem";
            this.chkNewSystem.Size = new System.Drawing.Size(136, 17);
            this.chkNewSystem.TabIndex = 16;
            this.chkNewSystem.Text = "Neues System erstellen";
            this.chkNewSystem.Visible = false;
            // 
            // txtNewSystemName
            // 
            this.txtNewSystemName.Location = new System.Drawing.Point(12, 134);
            this.txtNewSystemName.Name = "txtNewSystemName";
            this.txtNewSystemName.Size = new System.Drawing.Size(350, 20);
            this.txtNewSystemName.TabIndex = 15;
            this.txtNewSystemName.Visible = false;
            // 
            // cbExistingSystems
            // 
            this.cbExistingSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExistingSystems.Location = new System.Drawing.Point(12, 134);
            this.cbExistingSystems.Name = "cbExistingSystems";
            this.cbExistingSystems.Size = new System.Drawing.Size(350, 21);
            this.cbExistingSystems.TabIndex = 14;
            this.cbExistingSystems.Visible = false;
            // 
            // cbSystems
            // 
            this.cbSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSystems.Location = new System.Drawing.Point(12, 130);
            this.cbSystems.Name = "cbSystems";
            this.cbSystems.Size = new System.Drawing.Size(350, 21);
            this.cbSystems.TabIndex = 12;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(12, 161);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(101, 13);
            this.lblProduct.TabIndex = 11;
            this.lblProduct.Text = "Produkt auswählen:";
            // 
            // cbProducts
            // 
            this.cbProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProducts.Location = new System.Drawing.Point(12, 180);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(350, 21);
            this.cbProducts.TabIndex = 10;
            // 
            // txtKategorie
            // 
            this.txtKategorie.Location = new System.Drawing.Point(12, 180);
            this.txtKategorie.Name = "txtKategorie";
            this.txtKategorie.Size = new System.Drawing.Size(350, 20);
            this.txtKategorie.TabIndex = 8;
            this.txtKategorie.Visible = false;
            // 
            // lblBeschreibung
            // 
            this.lblBeschreibung.AutoSize = true;
            this.lblBeschreibung.Location = new System.Drawing.Point(12, 214);
            this.lblBeschreibung.Name = "lblBeschreibung";
            this.lblBeschreibung.Size = new System.Drawing.Size(75, 13);
            this.lblBeschreibung.TabIndex = 7;
            this.lblBeschreibung.Text = "Beschreibung:";
            this.lblBeschreibung.Visible = false;
            // 
            // txtBeschreibung
            // 
            this.txtBeschreibung.Location = new System.Drawing.Point(12, 230);
            this.txtBeschreibung.Name = "txtBeschreibung";
            this.txtBeschreibung.Size = new System.Drawing.Size(350, 20);
            this.txtBeschreibung.TabIndex = 6;
            this.txtBeschreibung.Visible = false;
            // 
            // lblMenge
            // 
            this.lblMenge.AutoSize = true;
            this.lblMenge.Location = new System.Drawing.Point(12, 264);
            this.lblMenge.Name = "lblMenge";
            this.lblMenge.Size = new System.Drawing.Size(43, 13);
            this.lblMenge.TabIndex = 5;
            this.lblMenge.Text = "Menge:";
            this.lblMenge.Visible = false;
            // 
            // txtMenge
            // 
            this.txtMenge.Location = new System.Drawing.Point(12, 280);
            this.txtMenge.Name = "txtMenge";
            this.txtMenge.Size = new System.Drawing.Size(350, 20);
            this.txtMenge.TabIndex = 4;
            this.txtMenge.Visible = false;
            // 
            // lblMindestbestand
            // 
            this.lblMindestbestand.AutoSize = true;
            this.lblMindestbestand.Location = new System.Drawing.Point(12, 314);
            this.lblMindestbestand.Name = "lblMindestbestand";
            this.lblMindestbestand.Size = new System.Drawing.Size(85, 13);
            this.lblMindestbestand.TabIndex = 3;
            this.lblMindestbestand.Text = "Mindestbestand:";
            this.lblMindestbestand.Visible = false;
            // 
            // txtMindestbestand
            // 
            this.txtMindestbestand.Location = new System.Drawing.Point(12, 330);
            this.txtMindestbestand.Name = "txtMindestbestand";
            this.txtMindestbestand.Size = new System.Drawing.Size(350, 20);
            this.txtMindestbestand.TabIndex = 2;
            this.txtMindestbestand.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 374);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(287, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Abbrechen";
            // 
            // lb_ProductID
            // 
            this.lb_ProductID.AutoSize = true;
            this.lb_ProductID.Location = new System.Drawing.Point(12, 22);
            this.lb_ProductID.Name = "lb_ProductID";
            this.lb_ProductID.Size = new System.Drawing.Size(61, 13);
            this.lb_ProductID.TabIndex = 21;
            this.lb_ProductID.Text = "ProductID :";
            // 
            // lb_Lot
            // 
            this.lb_Lot.AutoSize = true;
            this.lb_Lot.Location = new System.Drawing.Point(12, 48);
            this.lb_Lot.Name = "lb_Lot";
            this.lb_Lot.Size = new System.Drawing.Size(28, 13);
            this.lb_Lot.TabIndex = 23;
            this.lb_Lot.Text = "Lot :";
            // 
            // lb_Productiondate
            // 
            this.lb_Productiondate.AutoSize = true;
            this.lb_Productiondate.Location = new System.Drawing.Point(12, 35);
            this.lb_Productiondate.Name = "lb_Productiondate";
            this.lb_Productiondate.Size = new System.Drawing.Size(79, 13);
            this.lb_Productiondate.TabIndex = 22;
            this.lb_Productiondate.Text = "Productiondate";
            // 
            // rbExistingCategory
            // 
            this.rbExistingCategory.AutoSize = true;
            this.rbExistingCategory.Location = new System.Drawing.Point(12, 164);
            this.rbExistingCategory.Name = "rbExistingCategory";
            this.rbExistingCategory.Size = new System.Drawing.Size(150, 17);
            this.rbExistingCategory.TabIndex = 24;
            this.rbExistingCategory.Text = "Bestehende Kategorie";
            this.rbExistingCategory.UseVisualStyleBackColor = true;
            // 
            // rbNewCategory
            // 
            this.rbNewCategory.AutoSize = true;
            this.rbNewCategory.Location = new System.Drawing.Point(12, 187);
            this.rbNewCategory.Name = "rbNewCategory";
            this.rbNewCategory.Size = new System.Drawing.Size(150, 17);
            this.rbNewCategory.TabIndex = 25;
            this.rbNewCategory.Text = "Neue Kategorie";
            this.rbNewCategory.UseVisualStyleBackColor = true;
            // 
            // cbCategories
            // 
            this.cbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategories.Location = new System.Drawing.Point(12, 179);
            this.cbCategories.Name = "cbCategories";
            this.cbCategories.Size = new System.Drawing.Size(350, 21);
            this.cbCategories.TabIndex = 25;
            this.cbCategories.Visible = false;
            // 
            // chkNewCategory
            // 
            this.chkNewCategory.AutoSize = true;
            this.chkNewCategory.Location = new System.Drawing.Point(12, 160);
            this.chkNewCategory.Name = "chkNewCategory";
            this.chkNewCategory.Size = new System.Drawing.Size(142, 17);
            this.chkNewCategory.TabIndex = 24;
            this.chkNewCategory.Text = "Neue Kategorie erstellen";
            this.chkNewCategory.Visible = false;
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(12, 114);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(98, 13);
            this.lblSystem.TabIndex = 13;
            this.lblSystem.Text = "System auswählen:";
            // 
            // AddProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 478);
            this.Controls.Add(this.lb_Lot);
            this.Controls.Add(this.lb_Productiondate);
            this.Controls.Add(this.lb_ProductID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtMindestbestand);
            this.Controls.Add(this.lblMindestbestand);
            this.Controls.Add(this.txtMenge);
            this.Controls.Add(this.lblMenge);
            this.Controls.Add(this.txtBeschreibung);
            this.Controls.Add(this.lblBeschreibung);
            this.Controls.Add(this.txtKategorie);
            this.Controls.Add(this.cbProducts);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.cbSystems);
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.cbExistingSystems);
            this.Controls.Add(this.txtNewSystemName);
            this.Controls.Add(this.chkNewSystem);
            this.Controls.Add(this.rbCreateNew);
            this.Controls.Add(this.rbAddToExisting);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.chkNewCategory);
            this.Controls.Add(this.cbCategories);
            this.Name = "AddProductForm";
            this.Text = "Produkt hinzufügen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label lb_ProductID;
        private System.Windows.Forms.Label lb_Lot;
        private System.Windows.Forms.Label lb_Productiondate;
        private System.Windows.Forms.Label lblSystem;
    }
}