namespace Ticketník
{
    partial class SkrytaNastaveni
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
            this.cesta_k_souboru = new Ticketník.CustomControls.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.update_cesta = new Ticketník.CustomControls.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.zalozni_update = new Ticketník.CustomControls.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new Ticketník.CustomControls.Button();
            this.novyExport = new Ticketník.CustomControls.CheckBox();
            this.zalozniUpdateBox = new Ticketník.CustomControls.CheckBox();
            this.SuspendLayout();
            // 
            // cesta_k_souboru
            // 
            this.cesta_k_souboru.Location = new System.Drawing.Point(128, 12);
            this.cesta_k_souboru.Name = "cesta_k_souboru";
            this.cesta_k_souboru.Size = new System.Drawing.Size(466, 20);
            this.cesta_k_souboru.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cesta k .tic souboru";
            // 
            // update_cesta
            // 
            this.update_cesta.Location = new System.Drawing.Point(128, 38);
            this.update_cesta.Name = "update_cesta";
            this.update_cesta.Size = new System.Drawing.Size(466, 20);
            this.update_cesta.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Update cesta (UNC)";
            // 
            // zalozni_update
            // 
            this.zalozni_update.Location = new System.Drawing.Point(128, 64);
            this.zalozni_update.Name = "zalozni_update";
            this.zalozni_update.Size = new System.Drawing.Size(466, 20);
            this.zalozni_update.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Záložní update (URL)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(519, 125);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Nastav";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // novyExport
            // 
            this.novyExport.AutoSize = true;
            this.novyExport.Location = new System.Drawing.Point(12, 113);
            this.novyExport.Name = "novyExport";
            this.novyExport.Size = new System.Drawing.Size(115, 17);
            this.novyExport.TabIndex = 7;
            this.novyExport.Text = "Použít nový export";
            this.novyExport.UseVisualStyleBackColor = true;
            // 
            // zalozniUpdateBox
            // 
            this.zalozniUpdateBox.AutoSize = true;
            this.zalozniUpdateBox.Location = new System.Drawing.Point(12, 90);
            this.zalozniUpdateBox.Name = "zalozniUpdateBox";
            this.zalozniUpdateBox.Size = new System.Drawing.Size(185, 17);
            this.zalozniUpdateBox.TabIndex = 8;
            this.zalozniUpdateBox.Text = "Primárně používát záložní update";
            this.zalozniUpdateBox.UseVisualStyleBackColor = true;
            // 
            // SkrytaNastaveni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 160);
            this.Controls.Add(this.zalozniUpdateBox);
            this.Controls.Add(this.novyExport);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.zalozni_update);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.update_cesta);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cesta_k_souboru);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SkrytaNastaveni";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Skrytá Nastavení";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ticketník.CustomControls.TextBox cesta_k_souboru;
        private System.Windows.Forms.Label label1;
        private Ticketník.CustomControls.TextBox update_cesta;
        private System.Windows.Forms.Label label2;
        private Ticketník.CustomControls.TextBox zalozni_update;
        private System.Windows.Forms.Label label3;
        private Ticketník.CustomControls.Button button1;
        private Ticketník.CustomControls.CheckBox novyExport;
        private Ticketník.CustomControls.CheckBox zalozniUpdateBox;
    }
}