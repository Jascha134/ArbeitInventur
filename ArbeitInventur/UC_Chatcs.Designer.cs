namespace ArbeitInventur
{
    partial class UC_Chatcs
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
            this.ListBoxChat = new System.Windows.Forms.ListBox();
            this.txt_Chat = new System.Windows.Forms.TextBox();
            this.lb_Benutzer = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ListBoxChat
            // 
            this.ListBoxChat.FormattingEnabled = true;
            this.ListBoxChat.ItemHeight = 20;
            this.ListBoxChat.Location = new System.Drawing.Point(3, 3);
            this.ListBoxChat.Name = "ListBoxChat";
            this.ListBoxChat.Size = new System.Drawing.Size(1149, 584);
            this.ListBoxChat.TabIndex = 0;
            // 
            // txt_Chat
            // 
            this.txt_Chat.Location = new System.Drawing.Point(83, 593);
            this.txt_Chat.Name = "txt_Chat";
            this.txt_Chat.Size = new System.Drawing.Size(982, 26);
            this.txt_Chat.TabIndex = 1;
            this.txt_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Chat_KeyDown);
            // 
            // lb_Benutzer
            // 
            this.lb_Benutzer.AutoSize = true;
            this.lb_Benutzer.Location = new System.Drawing.Point(3, 596);
            this.lb_Benutzer.Name = "lb_Benutzer";
            this.lb_Benutzer.Size = new System.Drawing.Size(74, 20);
            this.lb_Benutzer.TabIndex = 2;
            this.lb_Benutzer.Text = "Benutzer";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1071, 593);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "Senden";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UC_Chatcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lb_Benutzer);
            this.Controls.Add(this.txt_Chat);
            this.Controls.Add(this.ListBoxChat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UC_Chatcs";
            this.Size = new System.Drawing.Size(1155, 623);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ListBoxChat;
        private System.Windows.Forms.TextBox txt_Chat;
        private System.Windows.Forms.Label lb_Benutzer;
        private System.Windows.Forms.Button button1;
    }
}
