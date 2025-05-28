namespace ArbeitInventur
{
    partial class Main
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.txtBarcodeInput = new System.Windows.Forms.TextBox();
            this.lb_Benutzer = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btn_Chat = new System.Windows.Forms.Button();
            this.btn_UC_Übersicht = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_Exocad = new System.Windows.Forms.Button();
            this.btn_RefreshHistory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 216);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(318, 392);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(347, 5);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(804, 603);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView2_CellFormatting);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 86);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(200, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Hinzufügen / Bearbeiten";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.txtBarcodeInput);
            this.panelMain.Controls.Add(this.lb_Benutzer);
            this.panelMain.Controls.Add(this.dataGridView1);
            this.panelMain.Controls.Add(this.dataGridView2);
            this.panelMain.Controls.Add(this.button1);
            this.panelMain.Location = new System.Drawing.Point(18, 69);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1155, 623);
            this.panelMain.TabIndex = 16;
            // 
            // txtBarcodeInput
            // 
            this.txtBarcodeInput.Location = new System.Drawing.Point(6, 50);
            this.txtBarcodeInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBarcodeInput.Name = "txtBarcodeInput";
            this.txtBarcodeInput.Size = new System.Drawing.Size(200, 26);
            this.txtBarcodeInput.TabIndex = 29;
            this.txtBarcodeInput.Text = "Barcode scannen...";
            this.txtBarcodeInput.Enter += new System.EventHandler(this.txtBarcodeInput_Enter);
            this.txtBarcodeInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtBarcodeInput_KeyPress);
            this.txtBarcodeInput.Leave += new System.EventHandler(this.txtBarcodeInput_Leave);
            // 
            // lb_Benutzer
            // 
            this.lb_Benutzer.AutoSize = true;
            this.lb_Benutzer.Location = new System.Drawing.Point(6, 17);
            this.lb_Benutzer.Name = "lb_Benutzer";
            this.lb_Benutzer.Size = new System.Drawing.Size(74, 20);
            this.lb_Benutzer.TabIndex = 20;
            this.lb_Benutzer.Text = "Benutzer";
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.Location = new System.Drawing.Point(1038, 14);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 46);
            this.button6.TabIndex = 17;
            this.button6.Text = "Einstellungen";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(18, 14);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(135, 46);
            this.button7.TabIndex = 18;
            this.button7.Text = "Einlagern";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // btn_Chat
            // 
            this.btn_Chat.Location = new System.Drawing.Point(304, 14);
            this.btn_Chat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Chat.Name = "btn_Chat";
            this.btn_Chat.Size = new System.Drawing.Size(135, 46);
            this.btn_Chat.TabIndex = 19;
            this.btn_Chat.Text = "Pinwand";
            this.btn_Chat.UseVisualStyleBackColor = true;
            this.btn_Chat.Click += new System.EventHandler(this.btn_Chat_Click);
            // 
            // btn_UC_Übersicht
            // 
            this.btn_UC_Übersicht.Location = new System.Drawing.Point(161, 13);
            this.btn_UC_Übersicht.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_UC_Übersicht.Name = "btn_UC_Übersicht";
            this.btn_UC_Übersicht.Size = new System.Drawing.Size(135, 46);
            this.btn_UC_Übersicht.TabIndex = 21;
            this.btn_UC_Übersicht.Text = "Übersicht";
            this.btn_UC_Übersicht.UseVisualStyleBackColor = true;
            this.btn_UC_Übersicht.Click += new System.EventHandler(this.btn_UC_Übersicht_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(447, 14);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(135, 46);
            this.button4.TabIndex = 22;
            this.button4.Text = "History";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_Exocad
            // 
            this.btn_Exocad.Location = new System.Drawing.Point(590, 14);
            this.btn_Exocad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Exocad.Name = "btn_Exocad";
            this.btn_Exocad.Size = new System.Drawing.Size(135, 46);
            this.btn_Exocad.TabIndex = 23;
            this.btn_Exocad.Text = "Exocad";
            this.btn_Exocad.UseVisualStyleBackColor = true;
            this.btn_Exocad.Click += new System.EventHandler(this.btn_Exocad_Click);
            // 
            // btn_RefreshHistory
            // 
            this.btn_RefreshHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RefreshHistory.Location = new System.Drawing.Point(895, 13);
            this.btn_RefreshHistory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_RefreshHistory.Name = "btn_RefreshHistory";
            this.btn_RefreshHistory.Size = new System.Drawing.Size(135, 46);
            this.btn_RefreshHistory.TabIndex = 24;
            this.btn_RefreshHistory.Text = "Aktualisieren";
            this.btn_RefreshHistory.UseVisualStyleBackColor = true;
            this.btn_RefreshHistory.Click += new System.EventHandler(this.btn_RefreshHistory_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1192, 720);
            this.Controls.Add(this.btn_RefreshHistory);
            this.Controls.Add(this.btn_Exocad);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btn_UC_Übersicht);
            this.Controls.Add(this.btn_Chat);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ArbeitInventura";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btn_Chat;
        private System.Windows.Forms.Label lb_Benutzer;
        private System.Windows.Forms.Button btn_UC_Übersicht;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_Exocad;
        private System.Windows.Forms.TextBox txtBarcodeInput;
        private System.Windows.Forms.Button btn_RefreshHistory;
    }
}