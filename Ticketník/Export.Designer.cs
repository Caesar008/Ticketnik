﻿namespace Ticketník
{
    partial class Export
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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkBox1 = new Ticketník.CustomControls.CheckBox();
            this.button1 = new Ticketník.CustomControls.Button();
            this.dateTimePicker2 = new Ticketník.CustomControls.DateTimePicker();
            this.dateTimePicker1 = new Ticketník.CustomControls.DateTimePicker();
            this.radioButton3 = new Ticketník.CustomControls.RadioButton();
            this.radioButton2 = new Ticketník.CustomControls.RadioButton();
            this.radioButton1 = new Ticketník.CustomControls.RadioButton();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // checkBox1
            // 
            this.checkBox1.BoxColor = System.Drawing.SystemColors.Window;
            this.checkBox1.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox1.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.checkBox1.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.checkBox1.Location = new System.Drawing.Point(168, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 48);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Automatically upload to MyTime";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Exportovat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.BorderColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.dateTimePicker2.ButtonColorDisabled = System.Drawing.SystemColors.Control;
            this.dateTimePicker2.ButtonColorMouseOver = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dateTimePicker2.CustomFormat = null;
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker2.ForeColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.dateTimePicker2.Location = new System.Drawing.Point(12, 107);
            this.dateTimePicker2.MaxDate = new System.DateTime(8999, 12, 31, 0, 0, 0, 0);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 4;
            this.dateTimePicker2.Value = new System.DateTime(2023, 6, 19, 0, 0, 0, 0);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.BorderColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.dateTimePicker1.ButtonColorDisabled = System.Drawing.SystemColors.Control;
            this.dateTimePicker1.ButtonColorMouseOver = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dateTimePicker1.CustomFormat = null;
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.ForeColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 81);
            this.dateTimePicker1.MaxDate = new System.DateTime(8999, 12, 31, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.Value = new System.DateTime(2023, 6, 19, 0, 0, 0, 0);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.BoxColor = System.Drawing.SystemColors.Window;
            this.radioButton3.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.radioButton3.CheckedColor = System.Drawing.Color.LimeGreen;
            this.radioButton3.CheckedColorMouseOver = System.Drawing.Color.LimeGreen;
            this.radioButton3.Location = new System.Drawing.Point(12, 58);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(101, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Vybrané období";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BoxColor = System.Drawing.SystemColors.Window;
            this.radioButton2.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.radioButton2.CheckedColor = System.Drawing.Color.LimeGreen;
            this.radioButton2.CheckedColorMouseOver = System.Drawing.Color.LimeGreen;
            this.radioButton2.Location = new System.Drawing.Point(12, 35);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(84, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Minulý týden";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BoxColor = System.Drawing.SystemColors.Window;
            this.radioButton1.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.radioButton1.CheckedColor = System.Drawing.Color.LimeGreen;
            this.radioButton1.CheckedColorMouseOver = System.Drawing.Color.LimeGreen;
            this.radioButton1.Location = new System.Drawing.Point(12, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(82, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Tento týden";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // Export
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 141);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(320, 180);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 180);
            this.Name = "Export";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Exportovat do MyTime";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Ticketník.CustomControls.DateTimePicker dateTimePicker1;
        private Ticketník.CustomControls.DateTimePicker dateTimePicker2;
        private Ticketník.CustomControls.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private CustomControls.CheckBox checkBox1;
        private CustomControls.RadioButton radioButton1;
        private CustomControls.RadioButton radioButton2;
        private CustomControls.RadioButton radioButton3;
    }
}