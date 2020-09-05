namespace Ticketník
{
    partial class Upozorneni
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Upozorneni));
            this.listView1 = new System.Windows.Forms.ListView();
            this.slTyp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.slDatum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.slCas = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.slPopis = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.noveUpozorneni = new System.Windows.Forms.Button();
            this.upravitUpozorneni = new System.Windows.Forms.Button();
            this.smazatUpozorneni = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.slTyp,
            this.slDatum,
            this.slCas,
            this.slPopis});
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(376, 367);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.upravitUpozorneni_Click);
            // 
            // slTyp
            // 
            this.slTyp.DisplayIndex = 2;
            this.slTyp.Text = "Typ";
            this.slTyp.Width = 40;
            // 
            // slDatum
            // 
            this.slDatum.DisplayIndex = 0;
            this.slDatum.Text = "Datum";
            this.slDatum.Width = 90;
            // 
            // slCas
            // 
            this.slCas.DisplayIndex = 1;
            this.slCas.Text = "Čas";
            // 
            // slPopis
            // 
            this.slPopis.Text = "Popis";
            this.slPopis.Width = 140;
            // 
            // noveUpozorneni
            // 
            this.noveUpozorneni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.noveUpozorneni.Location = new System.Drawing.Point(394, 12);
            this.noveUpozorneni.Name = "noveUpozorneni";
            this.noveUpozorneni.Size = new System.Drawing.Size(150, 23);
            this.noveUpozorneni.TabIndex = 1;
            this.noveUpozorneni.Text = "Nové upozornění";
            this.noveUpozorneni.UseVisualStyleBackColor = true;
            this.noveUpozorneni.Click += new System.EventHandler(this.noveUpozorneni_Click);
            // 
            // upravitUpozorneni
            // 
            this.upravitUpozorneni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upravitUpozorneni.Location = new System.Drawing.Point(394, 41);
            this.upravitUpozorneni.Name = "upravitUpozorneni";
            this.upravitUpozorneni.Size = new System.Drawing.Size(150, 23);
            this.upravitUpozorneni.TabIndex = 2;
            this.upravitUpozorneni.Text = "Upravit upozornění";
            this.upravitUpozorneni.UseVisualStyleBackColor = true;
            this.upravitUpozorneni.Click += new System.EventHandler(this.upravitUpozorneni_Click);
            // 
            // smazatUpozorneni
            // 
            this.smazatUpozorneni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.smazatUpozorneni.Location = new System.Drawing.Point(394, 70);
            this.smazatUpozorneni.Name = "smazatUpozorneni";
            this.smazatUpozorneni.Size = new System.Drawing.Size(150, 23);
            this.smazatUpozorneni.TabIndex = 3;
            this.smazatUpozorneni.Text = "Smazat upozornění";
            this.smazatUpozorneni.UseVisualStyleBackColor = true;
            this.smazatUpozorneni.Click += new System.EventHandler(this.smazatUpozorneni_Click);
            // 
            // Upozorneni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 391);
            this.Controls.Add(this.smazatUpozorneni);
            this.Controls.Add(this.upravitUpozorneni);
            this.Controls.Add(this.noveUpozorneni);
            this.Controls.Add(this.listView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(572, 430);
            this.Name = "Upozorneni";
            this.ShowInTaskbar = false;
            this.Text = "Upozornění";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Upozorneni_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader slDatum;
        private System.Windows.Forms.ColumnHeader slCas;
        private System.Windows.Forms.ColumnHeader slTyp;
        private System.Windows.Forms.ColumnHeader slPopis;
        private System.Windows.Forms.Button noveUpozorneni;
        private System.Windows.Forms.Button upravitUpozorneni;
        private System.Windows.Forms.Button smazatUpozorneni;
    }
}