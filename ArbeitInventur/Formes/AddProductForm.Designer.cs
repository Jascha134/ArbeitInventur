namespace ArbeitInventur.Formes
{
    partial class AddProductForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Label lb_ProductID;
        private System.Windows.Forms.Label lb_Productiondate;
        private System.Windows.Forms.Label lb_Lot;
        private System.Windows.Forms.RadioButton rbAddToExisting;
        private System.Windows.Forms.RadioButton rbCreateNew;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel _controlPanel;

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
            this.lb_ProductID = new System.Windows.Forms.Label();
            this.lb_Productiondate = new System.Windows.Forms.Label();
            this.lb_Lot = new System.Windows.Forms.Label();
            this.rbAddToExisting = new System.Windows.Forms.RadioButton();
            this.rbCreateNew = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this._controlPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Location = new System.Drawing.Point(12, 9);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(73, 13);
            this.lblBarcode.TabIndex = 19;
            this.lblBarcode.Text = "Barcode: N/A";
            // 
            // lb_ProductID
            // 
            this.lb_ProductID.AutoSize = true;
            this.lb_ProductID.Location = new System.Drawing.Point(12, 28);
            this.lb_ProductID.Name = "lb_ProductID";
            this.lb_ProductID.Size = new System.Drawing.Size(84, 13);
            this.lb_ProductID.TabIndex = 21;
            this.lb_ProductID.Text = "Produkt ID: N/A";
            // 
            // lb_Productiondate
            // 
            this.lb_Productiondate.AutoSize = true;
            this.lb_Productiondate.Location = new System.Drawing.Point(12, 47);
            this.lb_Productiondate.Name = "lb_Productiondate";
            this.lb_Productiondate.Size = new System.Drawing.Size(118, 13);
            this.lb_Productiondate.TabIndex = 22;
            this.lb_Productiondate.Text = "Produktionsdatum: N/A";
            // 
            // lb_Lot
            // 
            this.lb_Lot.AutoSize = true;
            this.lb_Lot.Location = new System.Drawing.Point(12, 66);
            this.lb_Lot.Name = "lb_Lot";
            this.lb_Lot.Size = new System.Drawing.Size(85, 13);
            this.lb_Lot.TabIndex = 23;
            this.lb_Lot.Text = "Lotnummer: N/A";
            // 
            // rbAddToExisting
            // 
            this.rbAddToExisting.AutoSize = true;
            this.rbAddToExisting.Checked = true;
            this.rbAddToExisting.Location = new System.Drawing.Point(12, 90);
            this.rbAddToExisting.Name = "rbAddToExisting";
            this.rbAddToExisting.Size = new System.Drawing.Size(197, 17);
            this.rbAddToExisting.TabIndex = 18;
            this.rbAddToExisting.TabStop = true;
            this.rbAddToExisting.Text = "Zu bestehendem System hinzufügen";
            this.rbAddToExisting.CheckedChanged += new System.EventHandler(this.RbAddToExisting_CheckedChanged);
            // 
            // rbCreateNew
            // 
            this.rbCreateNew.AutoSize = true;
            this.rbCreateNew.Location = new System.Drawing.Point(12, 113);
            this.rbCreateNew.Name = "rbCreateNew";
            this.rbCreateNew.Size = new System.Drawing.Size(138, 17);
            this.rbCreateNew.TabIndex = 17;
            this.rbCreateNew.Text = "Neues Produkt erstellen";
            this.rbCreateNew.CheckedChanged += new System.EventHandler(this.RbCreateNew_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 360);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(287, 360);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _controlPanel
            // 
            this._controlPanel.Location = new System.Drawing.Point(12, 140);
            this._controlPanel.Name = "_controlPanel";
            this._controlPanel.Size = new System.Drawing.Size(350, 200);
            this._controlPanel.TabIndex = 24;
            // 
            // AddProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 400);
            this.Controls.Add(this._controlPanel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rbCreateNew);
            this.Controls.Add(this.rbAddToExisting);
            this.Controls.Add(this.lb_Lot);
            this.Controls.Add(this.lb_Productiondate);
            this.Controls.Add(this.lb_ProductID);
            this.Controls.Add(this.lblBarcode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AddProductForm";
            this.Text = "Produkt hinzufügen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}