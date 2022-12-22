namespace Ticketník
{
    partial class SeznamZak
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
            this.button1 = new Ticketník.CustomControls.Button();
            this.zakaznik = new Ticketník.CustomControls.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(204, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // zakaznik
            // 
            this.zakaznik.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zakaznik.FormattingEnabled = true;
            this.zakaznik.Location = new System.Drawing.Point(12, 12);
            this.zakaznik.Name = "zakaznik";
            this.zakaznik.Size = new System.Drawing.Size(267, 21);
            this.zakaznik.TabIndex = 1;
            // 
            // SeznamZak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 75);
            this.Controls.Add(this.zakaznik);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(307, 114);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(307, 114);
            this.Name = "SeznamZak";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Zákazník";
            this.ResumeLayout(false);

        }

        #endregion

        private Ticketník.CustomControls.Button button1;
        private Ticketník.CustomControls.ComboBox zakaznik;
    }
}