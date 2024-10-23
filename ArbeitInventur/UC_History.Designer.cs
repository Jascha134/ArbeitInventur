namespace ArbeitInventur
{
    partial class UC_History
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
            this.listBoxTimestamp = new System.Windows.Forms.ListBox();
            this.listBoxActions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxTimestamp
            // 
            this.listBoxTimestamp.FormattingEnabled = true;
            this.listBoxTimestamp.ItemHeight = 20;
            this.listBoxTimestamp.Location = new System.Drawing.Point(3, 16);
            this.listBoxTimestamp.Name = "listBoxTimestamp";
            this.listBoxTimestamp.Size = new System.Drawing.Size(259, 604);
            this.listBoxTimestamp.TabIndex = 0;
            // 
            // listBoxActions
            // 
            this.listBoxActions.FormattingEnabled = true;
            this.listBoxActions.ItemHeight = 20;
            this.listBoxActions.Location = new System.Drawing.Point(268, 16);
            this.listBoxActions.Name = "listBoxActions";
            this.listBoxActions.Size = new System.Drawing.Size(884, 604);
            this.listBoxActions.TabIndex = 1;
            // 
            // UC_History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBoxActions);
            this.Controls.Add(this.listBoxTimestamp);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UC_History";
            this.Size = new System.Drawing.Size(1155, 623);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTimestamp;
        private System.Windows.Forms.ListBox listBoxActions;
    }
}
