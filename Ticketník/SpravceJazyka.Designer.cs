namespace Ticketník
{
    partial class SpravceJazyka
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
            this.listView1 = new Ticketník.CustomControls.ListView(this);
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.stahnout = new Ticketník.CustomControls.Button();
            this.odebrat = new Ticketník.CustomControls.Button();
            this.listView1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Controls.Add(this.pictureBox1);
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(346, 234);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 26;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Jazyk";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Zkratka";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Verze";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "";
            this.columnHeader5.Width = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Ticketník.Properties.Resources.animated_loading;
            this.pictureBox1.Location = new System.Drawing.Point(141, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // stahnout
            // 
            this.stahnout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stahnout.Location = new System.Drawing.Point(365, 12);
            this.stahnout.Name = "stahnout";
            this.stahnout.Size = new System.Drawing.Size(157, 23);
            this.stahnout.TabIndex = 1;
            this.stahnout.Text = "Stáhnout";
            this.stahnout.UseVisualStyleBackColor = true;
            this.stahnout.Click += new System.EventHandler(this.stahnout_Click);
            // 
            // odebrat
            // 
            this.odebrat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.odebrat.Location = new System.Drawing.Point(365, 41);
            this.odebrat.Name = "odebrat";
            this.odebrat.Size = new System.Drawing.Size(157, 23);
            this.odebrat.TabIndex = 2;
            this.odebrat.Text = "Odebrat";
            this.odebrat.UseVisualStyleBackColor = true;
            this.odebrat.Click += new System.EventHandler(this.odebrat_Click);
            // 
            // SpravceJazyka
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 260);
            this.Controls.Add(this.odebrat);
            this.Controls.Add(this.stahnout);
            this.Controls.Add(this.listView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(550, 299);
            this.MinimumSize = new System.Drawing.Size(550, 299);
            this.Name = "SpravceJazyka";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Správa jazyků";
            this.listView1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Ticketník.CustomControls.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private Ticketník.CustomControls.Button stahnout;
        private Ticketník.CustomControls.Button odebrat;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}