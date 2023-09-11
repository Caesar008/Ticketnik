namespace Ticketník
{
    partial class TERP
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
            this.bterp = new Ticketník.CustomControls.Button();
            this.btask = new Ticketník.CustomControls.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new Ticketník.CustomControls.TextBox();
            this.textBox2 = new Ticketník.CustomControls.TextBox();
            this.upravitPan = new System.Windows.Forms.Panel();
            this.novyTaskB = new Ticketník.CustomControls.Button();
            this.novyTask = new Ticketník.CustomControls.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.staryTask = new Ticketník.CustomControls.ComboBox();
            this.novyTerpB = new Ticketník.CustomControls.Button();
            this.novyTerp = new Ticketník.CustomControls.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.staryTerp = new Ticketník.CustomControls.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.smazatTask = new Ticketník.CustomControls.Button();
            this.smazatTerp = new Ticketník.CustomControls.Button();
            this.taskList = new Ticketník.CustomControls.ComboBox();
            this.terpList = new Ticketník.CustomControls.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new Ticketník.CustomControls.Button();
            this.taskListTic = new Ticketník.CustomControls.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.terpListTic = new Ticketník.CustomControls.ComboBox();
            this.upravitPan.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bterp
            // 
            this.bterp.Location = new System.Drawing.Point(197, 12);
            this.bterp.Name = "bterp";
            this.bterp.Size = new System.Drawing.Size(75, 23);
            this.bterp.TabIndex = 0;
            this.bterp.Text = "Přidat Terp";
            this.bterp.UseVisualStyleBackColor = true;
            this.bterp.Click += new System.EventHandler(this.bterp_Click);
            // 
            // btask
            // 
            this.btask.Location = new System.Drawing.Point(197, 41);
            this.btask.Name = "btask";
            this.btask.Size = new System.Drawing.Size(75, 23);
            this.btask.TabIndex = 1;
            this.btask.Text = "Přidat task";
            this.btask.UseVisualStyleBackColor = true;
            this.btask.Click += new System.EventHandler(this.btask_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Terp kód";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Task";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(68, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(123, 20);
            this.textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(68, 43);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(123, 20);
            this.textBox2.TabIndex = 5;
            // 
            // upravitPan
            // 
            this.upravitPan.Controls.Add(this.novyTaskB);
            this.upravitPan.Controls.Add(this.novyTask);
            this.upravitPan.Controls.Add(this.label4);
            this.upravitPan.Controls.Add(this.staryTask);
            this.upravitPan.Controls.Add(this.novyTerpB);
            this.upravitPan.Controls.Add(this.novyTerp);
            this.upravitPan.Controls.Add(this.label3);
            this.upravitPan.Controls.Add(this.staryTerp);
            this.upravitPan.Location = new System.Drawing.Point(0, 0);
            this.upravitPan.Name = "upravitPan";
            this.upravitPan.Size = new System.Drawing.Size(354, 78);
            this.upravitPan.TabIndex = 6;
            // 
            // novyTaskB
            // 
            this.novyTaskB.Enabled = false;
            this.novyTaskB.Location = new System.Drawing.Point(268, 41);
            this.novyTaskB.Name = "novyTaskB";
            this.novyTaskB.Size = new System.Drawing.Size(75, 23);
            this.novyTaskB.TabIndex = 7;
            this.novyTaskB.Text = "Ulož Task";
            this.novyTaskB.UseVisualStyleBackColor = true;
            this.novyTaskB.Click += new System.EventHandler(this.novyTaskB_Click);
            // 
            // novyTask
            // 
            this.novyTask.Location = new System.Drawing.Point(173, 43);
            this.novyTask.Name = "novyTask";
            this.novyTask.Size = new System.Drawing.Size(89, 20);
            this.novyTask.TabIndex = 6;
            this.novyTask.TextChanged += new System.EventHandler(this.novyTask_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Nový";
            // 
            // staryTask
            // 
            this.staryTask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.staryTask.FormattingEnabled = true;
            this.staryTask.Location = new System.Drawing.Point(15, 43);
            this.staryTask.Name = "staryTask";
            this.staryTask.Size = new System.Drawing.Size(114, 21);
            this.staryTask.TabIndex = 4;
            this.staryTask.SelectedIndexChanged += new System.EventHandler(this.staryTask_SelectedIndexChanged);
            // 
            // novyTerpB
            // 
            this.novyTerpB.Enabled = false;
            this.novyTerpB.Location = new System.Drawing.Point(268, 10);
            this.novyTerpB.Name = "novyTerpB";
            this.novyTerpB.Size = new System.Drawing.Size(75, 23);
            this.novyTerpB.TabIndex = 3;
            this.novyTerpB.Text = "Ulož Terp";
            this.novyTerpB.UseVisualStyleBackColor = true;
            this.novyTerpB.Click += new System.EventHandler(this.novyTerpB_Click);
            // 
            // novyTerp
            // 
            this.novyTerp.Location = new System.Drawing.Point(173, 12);
            this.novyTerp.Name = "novyTerp";
            this.novyTerp.Size = new System.Drawing.Size(89, 20);
            this.novyTerp.TabIndex = 2;
            this.novyTerp.TextChanged += new System.EventHandler(this.novyTerp_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nový";
            // 
            // staryTerp
            // 
            this.staryTerp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.staryTerp.FormattingEnabled = true;
            this.staryTerp.Location = new System.Drawing.Point(15, 12);
            this.staryTerp.Name = "staryTerp";
            this.staryTerp.Size = new System.Drawing.Size(114, 21);
            this.staryTerp.TabIndex = 0;
            this.staryTerp.SelectedIndexChanged += new System.EventHandler(this.staryTerp_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.smazatTask);
            this.panel1.Controls.Add(this.smazatTerp);
            this.panel1.Controls.Add(this.taskList);
            this.panel1.Controls.Add(this.terpList);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(283, 78);
            this.panel1.TabIndex = 7;
            // 
            // smazatTask
            // 
            this.smazatTask.Enabled = false;
            this.smazatTask.Location = new System.Drawing.Point(173, 41);
            this.smazatTask.Name = "smazatTask";
            this.smazatTask.Size = new System.Drawing.Size(99, 23);
            this.smazatTask.TabIndex = 3;
            this.smazatTask.Text = "Smazat Task";
            this.smazatTask.UseVisualStyleBackColor = true;
            this.smazatTask.Click += new System.EventHandler(this.smazatTask_Click);
            // 
            // smazatTerp
            // 
            this.smazatTerp.Enabled = false;
            this.smazatTerp.Location = new System.Drawing.Point(173, 12);
            this.smazatTerp.Name = "smazatTerp";
            this.smazatTerp.Size = new System.Drawing.Size(99, 23);
            this.smazatTerp.TabIndex = 2;
            this.smazatTerp.Text = "Smazat Terp";
            this.smazatTerp.UseVisualStyleBackColor = true;
            this.smazatTerp.Click += new System.EventHandler(this.smazatTerp_Click);
            // 
            // taskList
            // 
            this.taskList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taskList.FormattingEnabled = true;
            this.taskList.Location = new System.Drawing.Point(15, 43);
            this.taskList.Name = "taskList";
            this.taskList.Size = new System.Drawing.Size(152, 21);
            this.taskList.TabIndex = 1;
            this.taskList.SelectedIndexChanged += new System.EventHandler(this.taskList_SelectedIndexChanged);
            // 
            // terpList
            // 
            this.terpList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.terpList.FormattingEnabled = true;
            this.terpList.Location = new System.Drawing.Point(15, 12);
            this.terpList.Name = "terpList";
            this.terpList.Size = new System.Drawing.Size(152, 21);
            this.terpList.TabIndex = 0;
            this.terpList.SelectedIndexChanged += new System.EventHandler(this.terpList_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.taskListTic);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.terpListTic);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 78);
            this.panel2.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(197, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 59);
            this.button1.TabIndex = 4;
            this.button1.Text = "Nastav";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // taskListTic
            // 
            this.taskListTic.FormattingEnabled = true;
            this.taskListTic.Location = new System.Drawing.Point(49, 43);
            this.taskListTic.Name = "taskListTic";
            this.taskListTic.Size = new System.Drawing.Size(142, 21);
            this.taskListTic.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Task";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Terp";
            // 
            // terpListTic
            // 
            this.terpListTic.DropDownWidth = 142;
            this.terpListTic.FormattingEnabled = true;
            this.terpListTic.Location = new System.Drawing.Point(49, 9);
            this.terpListTic.Name = "terpListTic";
            this.terpListTic.Size = new System.Drawing.Size(142, 21);
            this.terpListTic.TabIndex = 0;
            // 
            // TERP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 78);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.upravitPan);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btask);
            this.Controls.Add(this.bterp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TERP";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TERP";
            this.upravitPan.ResumeLayout(false);
            this.upravitPan.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ticketník.CustomControls.Button bterp;
        private Ticketník.CustomControls.Button btask;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Ticketník.CustomControls.TextBox textBox1;
        private Ticketník.CustomControls.TextBox textBox2;
        private System.Windows.Forms.Panel upravitPan;
        private Ticketník.CustomControls.Button novyTerpB;
        private Ticketník.CustomControls.TextBox novyTerp;
        private System.Windows.Forms.Label label3;
        private Ticketník.CustomControls.ComboBox staryTerp;
        private Ticketník.CustomControls.Button novyTaskB;
        private Ticketník.CustomControls.TextBox novyTask;
        private System.Windows.Forms.Label label4;
        private Ticketník.CustomControls.ComboBox staryTask;
        private System.Windows.Forms.Panel panel1;
        private Ticketník.CustomControls.Button smazatTask;
        private Ticketník.CustomControls.Button smazatTerp;
        private Ticketník.CustomControls.ComboBox taskList;
        private Ticketník.CustomControls.ComboBox terpList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Ticketník.CustomControls.Button button1;
        internal Ticketník.CustomControls.ComboBox taskListTic;
        internal Ticketník.CustomControls.ComboBox terpListTic;
    }
}