namespace ArbeitInventur
{
    partial class UC_Exocad
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
            this.listBox_Output = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Exocad_Constructions = new System.Windows.Forms.TextBox();
            this.txt_dentalCAD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Exocad_Ordner = new System.Windows.Forms.Button();
            this.btn_ExoCad_ZielOrdner = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_LocalScanFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_ServerTargetFolder = new System.Windows.Forms.TextBox();
            this.btn_LocalScanFolder = new System.Windows.Forms.Button();
            this.btn_ServerTargetFolder = new System.Windows.Forms.Button();
            this.chk_DentalCadWatcher = new System.Windows.Forms.CheckBox();
            this.chk_FolderUploader = new System.Windows.Forms.CheckBox();
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
            this.label1.Location = new System.Drawing.Point(777, 32);
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
            this.txt_Exocad_Constructions.Location = new System.Drawing.Point(781, 57);
            this.txt_Exocad_Constructions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_Exocad_Constructions.Name = "txt_Exocad_Constructions";
            this.txt_Exocad_Constructions.Size = new System.Drawing.Size(297, 26);
            this.txt_Exocad_Constructions.TabIndex = 2;
            // 
            // txt_dentalCAD
            // 
            this.txt_dentalCAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_dentalCAD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_dentalCAD.Location = new System.Drawing.Point(781, 113);
            this.txt_dentalCAD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_dentalCAD.Name = "txt_dentalCAD";
            this.txt_dentalCAD.Size = new System.Drawing.Size(297, 26);
            this.txt_dentalCAD.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(777, 88);
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
            this.btn_Exocad_Ordner.Location = new System.Drawing.Point(1086, 57);
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
            this.btn_ExoCad_ZielOrdner.Location = new System.Drawing.Point(1086, 114);
            this.btn_ExoCad_ZielOrdner.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_ExoCad_ZielOrdner.Name = "btn_ExoCad_ZielOrdner";
            this.btn_ExoCad_ZielOrdner.Size = new System.Drawing.Size(55, 25);
            this.btn_ExoCad_ZielOrdner.TabIndex = 7;
            this.btn_ExoCad_ZielOrdner.Text = "...";
            this.btn_ExoCad_ZielOrdner.UseVisualStyleBackColor = true;
            this.btn_ExoCad_ZielOrdner.Click += new System.EventHandler(this.btn_ExoCad_ZielOrdner_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(777, 174);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Scan-Ordner:";
            // 
            // txt_LocalScanFolder
            // 
            this.txt_LocalScanFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_LocalScanFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_LocalScanFolder.Location = new System.Drawing.Point(781, 199);
            this.txt_LocalScanFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_LocalScanFolder.Name = "txt_LocalScanFolder";
            this.txt_LocalScanFolder.Size = new System.Drawing.Size(297, 26);
            this.txt_LocalScanFolder.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(777, 230);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Server-Ziel:";
            // 
            // txt_ServerTargetFolder
            // 
            this.txt_ServerTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_ServerTargetFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ServerTargetFolder.Location = new System.Drawing.Point(781, 255);
            this.txt_ServerTargetFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_ServerTargetFolder.Name = "txt_ServerTargetFolder";
            this.txt_ServerTargetFolder.Size = new System.Drawing.Size(297, 26);
            this.txt_ServerTargetFolder.TabIndex = 11;
            // 
            // btn_LocalScanFolder
            // 
            this.btn_LocalScanFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_LocalScanFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_LocalScanFolder.Location = new System.Drawing.Point(1086, 199);
            this.btn_LocalScanFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_LocalScanFolder.Name = "btn_LocalScanFolder";
            this.btn_LocalScanFolder.Size = new System.Drawing.Size(55, 26);
            this.btn_LocalScanFolder.TabIndex = 12;
            this.btn_LocalScanFolder.Text = "...";
            this.btn_LocalScanFolder.UseVisualStyleBackColor = true;
            this.btn_LocalScanFolder.Click += new System.EventHandler(this.btn_LocalScanFolder_Click);
            // 
            // btn_ServerTargetFolder
            // 
            this.btn_ServerTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_ServerTargetFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ServerTargetFolder.Location = new System.Drawing.Point(1086, 255);
            this.btn_ServerTargetFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_ServerTargetFolder.Name = "btn_ServerTargetFolder";
            this.btn_ServerTargetFolder.Size = new System.Drawing.Size(55, 25);
            this.btn_ServerTargetFolder.TabIndex = 13;
            this.btn_ServerTargetFolder.Text = "...";
            this.btn_ServerTargetFolder.UseVisualStyleBackColor = true;
            this.btn_ServerTargetFolder.Click += new System.EventHandler(this.btn_ServerTargetFolder_Click);
            // 
            // chk_DentalCadWatcher
            // 
            this.chk_DentalCadWatcher.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_DentalCadWatcher.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_DentalCadWatcher.Location = new System.Drawing.Point(781, 5);
            this.chk_DentalCadWatcher.Name = "chk_DentalCadWatcher";
            this.chk_DentalCadWatcher.Size = new System.Drawing.Size(200, 24);
            this.chk_DentalCadWatcher.TabIndex = 14;
            this.chk_DentalCadWatcher.Text = "DentalCad Überwachung";
            this.chk_DentalCadWatcher.UseVisualStyleBackColor = true;
            this.chk_DentalCadWatcher.CheckedChanged += new System.EventHandler(this.chk_DentalCadWatcher_CheckedChanged);
            // 
            // chk_FolderUploader
            // 
            this.chk_FolderUploader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_FolderUploader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_FolderUploader.Location = new System.Drawing.Point(781, 147);
            this.chk_FolderUploader.Name = "chk_FolderUploader";
            this.chk_FolderUploader.Size = new System.Drawing.Size(160, 24);
            this.chk_FolderUploader.TabIndex = 15;
            this.chk_FolderUploader.Text = "Ordner-Upload";
            this.chk_FolderUploader.UseVisualStyleBackColor = true;
            this.chk_FolderUploader.CheckedChanged += new System.EventHandler(this.chk_FolderUploader_CheckedChanged);
            // 
            // UC_Exocad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.chk_FolderUploader);
            this.Controls.Add(this.chk_DentalCadWatcher);
            this.Controls.Add(this.btn_ServerTargetFolder);
            this.Controls.Add(this.btn_LocalScanFolder);
            this.Controls.Add(this.txt_ServerTargetFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_LocalScanFolder);
            this.Controls.Add(this.label3);
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

        #region Komponenten
        private System.Windows.Forms.ListBox listBox_Output;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Exocad_Constructions;
        private System.Windows.Forms.TextBox txt_dentalCAD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Exocad_Ordner;
        private System.Windows.Forms.Button btn_ExoCad_ZielOrdner;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_LocalScanFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_ServerTargetFolder;
        private System.Windows.Forms.Button btn_LocalScanFolder;
        private System.Windows.Forms.Button btn_ServerTargetFolder;
        private System.Windows.Forms.CheckBox chk_DentalCadWatcher;
        private System.Windows.Forms.CheckBox chk_FolderUploader;
        #endregion
    }
}