namespace ArbeitInventur.Formes
{
    partial class InventoryActionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.btnEinlagern = new System.Windows.Forms.Button();
            this.btnAuslagern = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(7, 6);
            this.lblInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(47, 13);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Produkt:";
            // 
            // lblQuantity
            // 
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(11, 137);
            this.lblQuantity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(72, 13);
            this.lblQuantity.TabIndex = 1;
            this.lblQuantity.Text = "Neue Menge:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(87, 134);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(55, 20);
            this.txtQuantity.TabIndex = 2;
            // 
            // btnEinlagern
            // 
            this.btnEinlagern.Location = new System.Drawing.Point(10, 185);
            this.btnEinlagern.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEinlagern.Name = "btnEinlagern";
            this.btnEinlagern.Size = new System.Drawing.Size(80, 20);
            this.btnEinlagern.TabIndex = 3;
            this.btnEinlagern.Text = "Einlagern (+1)";
            this.btnEinlagern.UseVisualStyleBackColor = true;
            this.btnEinlagern.Click += new System.EventHandler(this.btnEinlagern_Click);
            // 
            // btnAuslagern
            // 
            this.btnAuslagern.Location = new System.Drawing.Point(96, 185);
            this.btnAuslagern.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAuslagern.Name = "btnAuslagern";
            this.btnAuslagern.Size = new System.Drawing.Size(80, 20);
            this.btnAuslagern.TabIndex = 4;
            this.btnAuslagern.Text = "Auslagern (-1)";
            this.btnAuslagern.UseVisualStyleBackColor = true;
            this.btnAuslagern.Click += new System.EventHandler(this.btnAuslagern_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(10, 211);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 20);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Bestätigen";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(96, 211);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 20);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InventoryActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 240);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnAuslagern);
            this.Controls.Add(this.btnEinlagern);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventoryActionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inventur Aktion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnEinlagern;
        private System.Windows.Forms.Button btnAuslagern;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}