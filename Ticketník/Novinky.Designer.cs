namespace Ticketník
{
    partial class Novinky
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
            this.cz = new System.Windows.Forms.RichTextBox();
            this.en = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cz
            // 
            this.cz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cz.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cz.Location = new System.Drawing.Point(12, 12);
            this.cz.Name = "cz";
            this.cz.ReadOnly = true;
            this.cz.Size = new System.Drawing.Size(380, 426);
            this.cz.TabIndex = 0;
            this.cz.Text = "";
            // 
            // en
            // 
            this.en.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.en.BackColor = System.Drawing.SystemColors.Control;
            this.en.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.en.Location = new System.Drawing.Point(408, 12);
            this.en.Name = "en";
            this.en.ReadOnly = true;
            this.en.Size = new System.Drawing.Size(380, 426);
            this.en.TabIndex = 1;
            this.en.Text = "";
            // 
            // Novinky
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.en);
            this.Controls.Add(this.cz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Novinky";
            this.Text = "Novinky / News";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Novinky_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox cz;
        private System.Windows.Forms.RichTextBox en;
    }
}