namespace ArbeitInventur.Formes
{
    partial class UC_AddToExisting
    {
        private System.ComponentModel.IContainer components = null;

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
            this.cbSystems = new System.Windows.Forms.ComboBox();
            this.cbProducts = new System.Windows.Forms.ComboBox();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblProduct = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbSystems
            // 
            this.cbSystems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSystems.Location = new System.Drawing.Point(100, 20);
            this.cbSystems.Name = "cbSystems";
            this.cbSystems.Size = new System.Drawing.Size(200, 21);
            this.cbSystems.TabIndex = 1;
            // 
            // cbProducts
            // 
            this.cbProducts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProducts.Location = new System.Drawing.Point(100, 60);
            this.cbProducts.Name = "cbProducts";
            this.cbProducts.Size = new System.Drawing.Size(200, 21);
            this.cbProducts.TabIndex = 3;
            // 
            // lblSystem
            // 
            this.lblSystem.Location = new System.Drawing.Point(20, 20);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(70, 21);
            this.lblSystem.TabIndex = 0;
            this.lblSystem.Text = "System:";
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(20, 60);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(70, 21);
            this.lblProduct.TabIndex = 2;
            this.lblProduct.Text = "Produkt:";
            // 
            // UC_AddToExisting
            // 
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.cbSystems);
            this.Controls.Add(this.lblProduct);
            this.Controls.Add(this.cbProducts);
            this.Name = "UC_AddToExisting";
            this.Size = new System.Drawing.Size(320, 100);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox cbSystems;
        private System.Windows.Forms.ComboBox cbProducts;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblProduct;
    }
}