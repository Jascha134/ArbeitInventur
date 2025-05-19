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
            this.textBoxBeschreibung = new System.Windows.Forms.TextBox();
            this.textBoxMenge = new System.Windows.Forms.TextBox();
            this.textBoxMindestbestand = new System.Windows.Forms.TextBox();
            this.textBoxSystemName = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSystemDelete = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.txtBarcodeInput = new System.Windows.Forms.TextBox();
            this.btn_SystemNameNew = new System.Windows.Forms.Button();
            this.btn_New = new System.Windows.Forms.Button();
            this.textBoxKategorie = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_PlusMindest = new System.Windows.Forms.Button();
            this.btn_MinusMindes = new System.Windows.Forms.Button();
            this.btn_Plus = new System.Windows.Forms.Button();
            this.btn_Minus = new System.Windows.Forms.Button();
            this.lb_Benutzer = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.btn_Chat = new System.Windows.Forms.Button();
            this.btn_UC_Übersicht = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_Exocad = new System.Windows.Forms.Button();
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
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(347, 216);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(804, 392);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView2_CellClick);
            this.dataGridView2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataGridView2_CellFormatting);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 160);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(358, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Hinzufügen / Bearbeiten";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxBeschreibung
            // 
            this.textBoxBeschreibung.Location = new System.Drawing.Point(463, 50);
            this.textBoxBeschreibung.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxBeschreibung.Name = "textBoxBeschreibung";
            this.textBoxBeschreibung.Size = new System.Drawing.Size(228, 26);
            this.textBoxBeschreibung.TabIndex = 4;
            // 
            // textBoxMenge
            // 
            this.textBoxMenge.Location = new System.Drawing.Point(463, 86);
            this.textBoxMenge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxMenge.Name = "textBoxMenge";
            this.textBoxMenge.Size = new System.Drawing.Size(228, 26);
            this.textBoxMenge.TabIndex = 5;
            // 
            // textBoxMindestbestand
            // 
            this.textBoxMindestbestand.Location = new System.Drawing.Point(463, 122);
            this.textBoxMindestbestand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxMindestbestand.Name = "textBoxMindestbestand";
            this.textBoxMindestbestand.Size = new System.Drawing.Size(228, 26);
            this.textBoxMindestbestand.TabIndex = 6;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.Location = new System.Drawing.Point(73, 120);
            this.textBoxSystemName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.Size = new System.Drawing.Size(187, 26);
            this.textBoxSystemName.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 160);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(199, 46);
            this.button2.TabIndex = 7;
            this.button2.Text = "Hinzufügen / Bearbeiten";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(699, 160);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(189, 46);
            this.button3.TabIndex = 9;
            this.button3.Text = "Löschen";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Firma :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Beschreibung :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Mänge :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(329, 125);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Mindestbestand :";
            // 
            // buttonSystemDelete
            // 
            this.buttonSystemDelete.Location = new System.Drawing.Point(214, 160);
            this.buttonSystemDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSystemDelete.Name = "buttonSystemDelete";
            this.buttonSystemDelete.Size = new System.Drawing.Size(111, 46);
            this.buttonSystemDelete.TabIndex = 15;
            this.buttonSystemDelete.Text = "Löschen";
            this.buttonSystemDelete.UseVisualStyleBackColor = true;
            this.buttonSystemDelete.Click += new System.EventHandler(this.button5_Click);
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.txtBarcodeInput);
            this.panelMain.Controls.Add(this.btn_SystemNameNew);
            this.panelMain.Controls.Add(this.btn_New);
            this.panelMain.Controls.Add(this.textBoxKategorie);
            this.panelMain.Controls.Add(this.label5);
            this.panelMain.Controls.Add(this.btn_PlusMindest);
            this.panelMain.Controls.Add(this.btn_MinusMindes);
            this.panelMain.Controls.Add(this.btn_Plus);
            this.panelMain.Controls.Add(this.btn_Minus);
            this.panelMain.Controls.Add(this.lb_Benutzer);
            this.panelMain.Controls.Add(this.dataGridView1);
            this.panelMain.Controls.Add(this.textBoxMenge);
            this.panelMain.Controls.Add(this.buttonSystemDelete);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.textBoxMindestbestand);
            this.panelMain.Controls.Add(this.dataGridView2);
            this.panelMain.Controls.Add(this.textBoxBeschreibung);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.button2);
            this.panelMain.Controls.Add(this.label4);
            this.panelMain.Controls.Add(this.label3);
            this.panelMain.Controls.Add(this.textBoxSystemName);
            this.panelMain.Controls.Add(this.button3);
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
            // btn_SystemNameNew
            // 
            this.btn_SystemNameNew.Location = new System.Drawing.Point(268, 120);
            this.btn_SystemNameNew.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_SystemNameNew.Name = "btn_SystemNameNew";
            this.btn_SystemNameNew.Size = new System.Drawing.Size(56, 27);
            this.btn_SystemNameNew.TabIndex = 28;
            this.btn_SystemNameNew.Text = "Neu";
            this.btn_SystemNameNew.UseVisualStyleBackColor = true;
            this.btn_SystemNameNew.Click += new System.EventHandler(this.btn_SystemNameNew_Click);
            // 
            // btn_New
            // 
            this.btn_New.Location = new System.Drawing.Point(699, 14);
            this.btn_New.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(56, 46);
            this.btn_New.TabIndex = 27;
            this.btn_New.Text = "Neu";
            this.btn_New.UseVisualStyleBackColor = true;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // textBoxKategorie
            // 
            this.textBoxKategorie.Location = new System.Drawing.Point(463, 14);
            this.textBoxKategorie.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxKategorie.Name = "textBoxKategorie";
            this.textBoxKategorie.Size = new System.Drawing.Size(228, 26);
            this.textBoxKategorie.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 17);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 20);
            this.label5.TabIndex = 26;
            this.label5.Text = "Kategorie :";
            // 
            // btn_PlusMindest
            // 
            this.btn_PlusMindest.FlatAppearance.BorderSize = 0;
            this.btn_PlusMindest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PlusMindest.Location = new System.Drawing.Point(730, 123);
            this.btn_PlusMindest.Name = "btn_PlusMindest";
            this.btn_PlusMindest.Size = new System.Drawing.Size(25, 25);
            this.btn_PlusMindest.TabIndex = 24;
            this.btn_PlusMindest.Text = "+";
            this.btn_PlusMindest.UseVisualStyleBackColor = true;
            this.btn_PlusMindest.Click += new System.EventHandler(this.btn_PlusMindest_Click);
            // 
            // btn_MinusMindes
            // 
            this.btn_MinusMindes.FlatAppearance.BorderSize = 0;
            this.btn_MinusMindes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_MinusMindes.Location = new System.Drawing.Point(699, 122);
            this.btn_MinusMindes.Name = "btn_MinusMindes";
            this.btn_MinusMindes.Size = new System.Drawing.Size(25, 25);
            this.btn_MinusMindes.TabIndex = 23;
            this.btn_MinusMindes.Text = "-";
            this.btn_MinusMindes.UseVisualStyleBackColor = true;
            this.btn_MinusMindes.Click += new System.EventHandler(this.btn_MinusMindes_Click);
            // 
            // btn_Plus
            // 
            this.btn_Plus.FlatAppearance.BorderSize = 0;
            this.btn_Plus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Plus.Location = new System.Drawing.Point(730, 87);
            this.btn_Plus.Name = "btn_Plus";
            this.btn_Plus.Size = new System.Drawing.Size(25, 25);
            this.btn_Plus.TabIndex = 22;
            this.btn_Plus.Text = "+";
            this.btn_Plus.UseVisualStyleBackColor = true;
            this.btn_Plus.Click += new System.EventHandler(this.btn_Plus_Click);
            // 
            // btn_Minus
            // 
            this.btn_Minus.FlatAppearance.BorderSize = 0;
            this.btn_Minus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Minus.Location = new System.Drawing.Point(699, 86);
            this.btn_Minus.Name = "btn_Minus";
            this.btn_Minus.Size = new System.Drawing.Size(25, 25);
            this.btn_Minus.TabIndex = 21;
            this.btn_Minus.Text = "-";
            this.btn_Minus.UseVisualStyleBackColor = true;
            this.btn_Minus.Click += new System.EventHandler(this.btn_Minus_Click);
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
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1192, 720);
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
        private System.Windows.Forms.TextBox textBoxBeschreibung;
        private System.Windows.Forms.TextBox textBoxMenge;
        private System.Windows.Forms.TextBox textBoxMindestbestand;
        private System.Windows.Forms.TextBox textBoxSystemName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSystemDelete;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btn_Chat;
        private System.Windows.Forms.Label lb_Benutzer;
        private System.Windows.Forms.Button btn_Minus;
        private System.Windows.Forms.Button btn_Plus;
        private System.Windows.Forms.Button btn_PlusMindest;
        private System.Windows.Forms.Button btn_MinusMindes;
        private System.Windows.Forms.Button btn_UC_Übersicht;
        private System.Windows.Forms.TextBox textBoxKategorie;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_New;
        private System.Windows.Forms.Button btn_SystemNameNew;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_Exocad;
        private System.Windows.Forms.TextBox txtBarcodeInput;
    }
}