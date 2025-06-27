namespace ArbeitInventur
{
    partial class Übersicht
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Übersicht));
            this.dataGridViewOverview = new System.Windows.Forms.DataGridView();
            this.textBoxSuche = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_MinOrders = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOverview)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewOverview
            // 
            this.dataGridViewOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewOverview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOverview.Location = new System.Drawing.Point(18, 102);
            this.dataGridViewOverview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridViewOverview.Name = "dataGridViewOverview";
            this.dataGridViewOverview.Size = new System.Drawing.Size(1164, 607);
            this.dataGridViewOverview.TabIndex = 0;
            // 
            // textBoxSuche
            // 
            this.textBoxSuche.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSuche.Location = new System.Drawing.Point(129, 52);
            this.textBoxSuche.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSuche.Name = "textBoxSuche";
            this.textBoxSuche.Size = new System.Drawing.Size(516, 26);
            this.textBoxSuche.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Suchen :";
            // 
            // checkBox_MinOrders
            // 
            this.checkBox_MinOrders.AutoSize = true;
            this.checkBox_MinOrders.Location = new System.Drawing.Point(21, 12);
            this.checkBox_MinOrders.Name = "checkBox_MinOrders";
            this.checkBox_MinOrders.Size = new System.Drawing.Size(244, 24);
            this.checkBox_MinOrders.TabIndex = 3;
            this.checkBox_MinOrders.Text = "Mindestbestand unterschritten";
            this.checkBox_MinOrders.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1052, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Übersicht
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 727);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox_MinOrders);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSuche);
            this.Controls.Add(this.dataGridViewOverview);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Übersicht";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bestellübersicht";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOverview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewOverview;
        private System.Windows.Forms.TextBox textBoxSuche;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_MinOrders;
        private System.Windows.Forms.Button button1;
    }
}