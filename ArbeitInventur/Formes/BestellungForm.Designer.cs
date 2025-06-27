namespace ArbeitInventur
{
    partial class BestellungForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewBestellung;
        private System.Windows.Forms.Button btnSenden;

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
            this.dataGridViewBestellung = new System.Windows.Forms.DataGridView();
            this.btnSenden = new System.Windows.Forms.Button();
            this.btn_Vorschau = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lb_Firma = new System.Windows.Forms.Label();
            this.btn_Firma = new System.Windows.Forms.Button();
            this.lb_Status = new System.Windows.Forms.Label();
            this.lb_Bestellen = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBestellung)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBestellung
            // 
            this.dataGridViewBestellung.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBestellung.Location = new System.Drawing.Point(12, 47);
            this.dataGridViewBestellung.Name = "dataGridViewBestellung";
            this.dataGridViewBestellung.Size = new System.Drawing.Size(600, 300);
            this.dataGridViewBestellung.TabIndex = 0;
            // 
            // btnSenden
            // 
            this.btnSenden.Location = new System.Drawing.Point(462, 353);
            this.btnSenden.Name = "btnSenden";
            this.btnSenden.Size = new System.Drawing.Size(150, 30);
            this.btnSenden.TabIndex = 0;
            this.btnSenden.Text = "Bestellung senden";
            this.btnSenden.Click += new System.EventHandler(this.btnSenden_Click);
            // 
            // btn_Vorschau
            // 
            this.btn_Vorschau.Location = new System.Drawing.Point(306, 353);
            this.btn_Vorschau.Name = "btn_Vorschau";
            this.btn_Vorschau.Size = new System.Drawing.Size(150, 30);
            this.btn_Vorschau.TabIndex = 1;
            this.btn_Vorschau.Text = "Vorschau";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(56, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // lb_Firma
            // 
            this.lb_Firma.AutoSize = true;
            this.lb_Firma.Location = new System.Drawing.Point(12, 9);
            this.lb_Firma.Name = "lb_Firma";
            this.lb_Firma.Size = new System.Drawing.Size(38, 13);
            this.lb_Firma.TabIndex = 3;
            this.lb_Firma.Text = "Firma :";
            // 
            // btn_Firma
            // 
            this.btn_Firma.Location = new System.Drawing.Point(390, 4);
            this.btn_Firma.Name = "btn_Firma";
            this.btn_Firma.Size = new System.Drawing.Size(75, 23);
            this.btn_Firma.TabIndex = 4;
            this.btn_Firma.Text = "Info";
            this.btn_Firma.UseVisualStyleBackColor = true;
            // 
            // lb_Status
            // 
            this.lb_Status.AutoSize = true;
            this.lb_Status.Location = new System.Drawing.Point(12, 362);
            this.lb_Status.Name = "lb_Status";
            this.lb_Status.Size = new System.Drawing.Size(37, 13);
            this.lb_Status.TabIndex = 5;
            this.lb_Status.Text = "Status";
            // 
            // lb_Bestellen
            // 
            this.lb_Bestellen.AutoSize = true;
            this.lb_Bestellen.Location = new System.Drawing.Point(13, 31);
            this.lb_Bestellen.Name = "lb_Bestellen";
            this.lb_Bestellen.Size = new System.Drawing.Size(69, 13);
            this.lb_Bestellen.TabIndex = 6;
            this.lb_Bestellen.Text = "Zu Bestellen ";
            // 
            // BestellungForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 395);
            this.Controls.Add(this.lb_Bestellen);
            this.Controls.Add(this.lb_Status);
            this.Controls.Add(this.btn_Firma);
            this.Controls.Add(this.lb_Firma);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btn_Vorschau);
            this.Controls.Add(this.btnSenden);
            this.Controls.Add(this.dataGridViewBestellung);
            this.Name = "BestellungForm";
            this.Text = "Bestellung erstellen";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBestellung)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btn_Vorschau;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lb_Firma;
        private System.Windows.Forms.Button btn_Firma;
        private System.Windows.Forms.Label lb_Status;
        private System.Windows.Forms.Label lb_Bestellen;
    }
}