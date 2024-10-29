namespace ArbeitInventur
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.labelError = new System.Windows.Forms.Label();
            this.btnRegistrieren = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.comboBoxBenutzername = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPasswort = new System.Windows.Forms.TextBox();
            this.labelBenutzername = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.Location = new System.Drawing.Point(13, 78);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(28, 16);
            this.labelError.TabIndex = 16;
            this.labelError.Text = "Info";
            // 
            // btnRegistrieren
            // 
            this.btnRegistrieren.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrieren.Location = new System.Drawing.Point(247, 140);
            this.btnRegistrieren.Name = "btnRegistrieren";
            this.btnRegistrieren.Size = new System.Drawing.Size(122, 28);
            this.btnRegistrieren.TabIndex = 15;
            this.btnRegistrieren.Text = "Registrieren";
            this.btnRegistrieren.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Location = new System.Drawing.Point(12, 97);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(357, 37);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // comboBoxBenutzername
            // 
            this.comboBoxBenutzername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxBenutzername.FormattingEnabled = true;
            this.comboBoxBenutzername.Location = new System.Drawing.Point(136, 6);
            this.comboBoxBenutzername.Name = "comboBoxBenutzername";
            this.comboBoxBenutzername.Size = new System.Drawing.Size(233, 28);
            this.comboBoxBenutzername.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password:";
            // 
            // textBoxPasswort
            // 
            this.textBoxPasswort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPasswort.Location = new System.Drawing.Point(136, 40);
            this.textBoxPasswort.Name = "textBoxPasswort";
            this.textBoxPasswort.Size = new System.Drawing.Size(233, 26);
            this.textBoxPasswort.TabIndex = 11;
            this.textBoxPasswort.UseSystemPasswordChar = true;
            // 
            // labelBenutzername
            // 
            this.labelBenutzername.AutoSize = true;
            this.labelBenutzername.BackColor = System.Drawing.Color.Transparent;
            this.labelBenutzername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBenutzername.Location = new System.Drawing.Point(12, 9);
            this.labelBenutzername.Name = "labelBenutzername";
            this.labelBenutzername.Size = new System.Drawing.Size(118, 20);
            this.labelBenutzername.TabIndex = 10;
            this.labelBenutzername.Text = "Benutzername:";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 177);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.btnRegistrieren);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.comboBoxBenutzername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPasswort);
            this.Controls.Add(this.labelBenutzername);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Button btnRegistrieren;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.ComboBox comboBoxBenutzername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPasswort;
        private System.Windows.Forms.Label labelBenutzername;
    }
}