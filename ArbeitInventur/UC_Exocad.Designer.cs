namespace ArbeitInventur
{
    partial class UC_Exocad
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox_Output = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Exocad_Constructions = new System.Windows.Forms.TextBox();
            this.txt_dentalCAD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Exocad_Ordner = new System.Windows.Forms.Button();
            this.btn_ExoCad_ZielOrdner = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Output
            // 
            this.listBox_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Output.FormattingEnabled = true;
            this.listBox_Output.ItemHeight = 20;
            this.listBox_Output.Location = new System.Drawing.Point(4, 5);
            this.listBox_Output.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBox_Output.Name = "listBox_Output";
            this.listBox_Output.Size = new System.Drawing.Size(770, 604);
            this.listBox_Output.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(782, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Exocad Ordner:";
            // 
            // txt_Exocad_Constructions
            // 
            this.txt_Exocad_Constructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Exocad_Constructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Exocad_Constructions.Location = new System.Drawing.Point(786, 30);
            this.txt_Exocad_Constructions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_Exocad_Constructions.Name = "txt_Exocad_Constructions";
            this.txt_Exocad_Constructions.Size = new System.Drawing.Size(297, 26);
            this.txt_Exocad_Constructions.TabIndex = 2;
            // 
            // txt_dentalCAD
            // 
            this.txt_dentalCAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_dentalCAD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_dentalCAD.Location = new System.Drawing.Point(786, 86);
            this.txt_dentalCAD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_dentalCAD.Name = "txt_dentalCAD";
            this.txt_dentalCAD.Size = new System.Drawing.Size(297, 26);
            this.txt_dentalCAD.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(782, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ziel Ordner:";
            // 
            // btn_Exocad_Ordner
            // 
            this.btn_Exocad_Ordner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Exocad_Ordner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Exocad_Ordner.Location = new System.Drawing.Point(1091, 30);
            this.btn_Exocad_Ordner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Exocad_Ordner.Name = "btn_Exocad_Ordner";
            this.btn_Exocad_Ordner.Size = new System.Drawing.Size(55, 26);
            this.btn_Exocad_Ordner.TabIndex = 6;
            this.btn_Exocad_Ordner.Text = "...";
            this.btn_Exocad_Ordner.UseVisualStyleBackColor = true;
            this.btn_Exocad_Ordner.Click += new System.EventHandler(this.btn_Exocad_Ordner_Click);
            // 
            // btn_ExoCad_ZielOrdner
            // 
            this.btn_ExoCad_ZielOrdner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ExoCad_ZielOrdner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ExoCad_ZielOrdner.Location = new System.Drawing.Point(1091, 87);
            this.btn_ExoCad_ZielOrdner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_ExoCad_ZielOrdner.Name = "btn_ExoCad_ZielOrdner";
            this.btn_ExoCad_ZielOrdner.Size = new System.Drawing.Size(55, 25);
            this.btn_ExoCad_ZielOrdner.TabIndex = 7;
            this.btn_ExoCad_ZielOrdner.Text = "...";
            this.btn_ExoCad_ZielOrdner.UseVisualStyleBackColor = true;
            this.btn_ExoCad_ZielOrdner.Click += new System.EventHandler(this.btn_ExoCad_ZielOrdner_Click);
            // 
            // UC_Exocad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btn_ExoCad_ZielOrdner);
            this.Controls.Add(this.btn_Exocad_Ordner);
            this.Controls.Add(this.txt_dentalCAD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_Exocad_Constructions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Output);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UC_Exocad";
            this.Size = new System.Drawing.Size(1155, 623);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Output;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Exocad_Constructions;
        private System.Windows.Forms.TextBox txt_dentalCAD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Exocad_Ordner;
        private System.Windows.Forms.Button btn_ExoCad_ZielOrdner;
    }
}
