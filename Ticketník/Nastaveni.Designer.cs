namespace Ticketník
{
    partial class Nastaveni
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clr_prescas = new Ticketník.CustomControls.Button();
            this.prescas = new System.Windows.Forms.Label();
            this.button1 = new Ticketník.CustomControls.Button();
            this.probiha = new System.Windows.Forms.Label();
            this.rdp = new System.Windows.Forms.Label();
            this.odpoved = new System.Windows.Forms.Label();
            this.ceka = new System.Windows.Forms.Label();
            this.vyreseno = new System.Windows.Forms.Label();
            this.clr_probiha = new Ticketník.CustomControls.Button();
            this.crl_rdp = new Ticketník.CustomControls.Button();
            this.clr_odpoved = new Ticketník.CustomControls.Button();
            this.clr_ceka = new Ticketník.CustomControls.Button();
            this.clr_vyreseno = new Ticketník.CustomControls.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textHigh = new System.Windows.Forms.Label();
            this.textOK = new System.Windows.Forms.Label();
            this.textMid = new System.Windows.Forms.Label();
            this.textLow = new System.Windows.Forms.Label();
            this.defaultCasy = new Ticketník.CustomControls.Button();
            this.zmenHigh = new Ticketník.CustomControls.Button();
            this.zmenOK = new Ticketník.CustomControls.Button();
            this.zmenMid = new Ticketník.CustomControls.Button();
            this.zmenLow = new Ticketník.CustomControls.Button();
            this.celkovyCasZobrazit = new Ticketník.CustomControls.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.jazykPopis = new System.Windows.Forms.Label();
            this.jazyk = new Ticketník.CustomControls.ComboBox();
            this.zmenitJazyk = new Ticketník.CustomControls.Button();
            this.skryteNastaveni = new Ticketník.CustomControls.Button();
            this.button2 = new Ticketník.CustomControls.Button();
            this.motiv = new System.Windows.Forms.Label();
            this.motivVyber = new Ticketník.CustomControls.ComboBox();
            this.onlineTerp = new Ticketník.CustomControls.CheckBox();
            this.checkBox1 = new Ticketník.CustomControls.CheckBox();
            this.numericUpDown1 = new Ticketník.CustomControls.NumericUpDown();
            this.autosave = new Ticketník.CustomControls.CheckBox();
            this.poStartu = new Ticketník.CustomControls.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ukládat každých";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(175, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "minut.";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.clr_prescas);
            this.groupBox1.Controls.Add(this.prescas);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.probiha);
            this.groupBox1.Controls.Add(this.rdp);
            this.groupBox1.Controls.Add(this.odpoved);
            this.groupBox1.Controls.Add(this.ceka);
            this.groupBox1.Controls.Add(this.vyreseno);
            this.groupBox1.Controls.Add(this.clr_probiha);
            this.groupBox1.Controls.Add(this.crl_rdp);
            this.groupBox1.Controls.Add(this.clr_odpoved);
            this.groupBox1.Controls.Add(this.clr_ceka);
            this.groupBox1.Controls.Add(this.clr_vyreseno);
            this.groupBox1.Location = new System.Drawing.Point(12, 157);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 221);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Barvy ticketů";
            // 
            // clr_prescas
            // 
            this.clr_prescas.BackColor = System.Drawing.SystemColors.Window;
            this.clr_prescas.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.clr_prescas.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.clr_prescas.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clr_prescas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clr_prescas.Location = new System.Drawing.Point(124, 164);
            this.clr_prescas.Name = "clr_prescas";
            this.clr_prescas.Size = new System.Drawing.Size(75, 23);
            this.clr_prescas.TabIndex = 12;
            this.clr_prescas.Tag = "prescas";
            this.clr_prescas.Text = "Změň";
            this.clr_prescas.UseVisualStyleBackColor = false;
            this.clr_prescas.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.clr_prescas.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.clr_prescas.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // prescas
            // 
            this.prescas.Location = new System.Drawing.Point(6, 164);
            this.prescas.Name = "prescas";
            this.prescas.Size = new System.Drawing.Size(112, 23);
            this.prescas.TabIndex = 11;
            this.prescas.Tag = "CustomColor";
            this.prescas.Text = "Přesčas";
            this.prescas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button1.Location = new System.Drawing.Point(6, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Default";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // probiha
            // 
            this.probiha.Location = new System.Drawing.Point(6, 135);
            this.probiha.Name = "probiha";
            this.probiha.Size = new System.Drawing.Size(112, 23);
            this.probiha.TabIndex = 9;
            this.probiha.Tag = "CustomColor";
            this.probiha.Text = "Probíhá";
            this.probiha.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdp
            // 
            this.rdp.Location = new System.Drawing.Point(6, 106);
            this.rdp.Name = "rdp";
            this.rdp.Size = new System.Drawing.Size(112, 23);
            this.rdp.TabIndex = 8;
            this.rdp.Tag = "CustomColor";
            this.rdp.Text = "RDP";
            this.rdp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // odpoved
            // 
            this.odpoved.Location = new System.Drawing.Point(6, 77);
            this.odpoved.Name = "odpoved";
            this.odpoved.Size = new System.Drawing.Size(112, 23);
            this.odpoved.TabIndex = 7;
            this.odpoved.Tag = "CustomColor";
            this.odpoved.Text = "Čeká se na odpověď";
            this.odpoved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ceka
            // 
            this.ceka.Location = new System.Drawing.Point(6, 48);
            this.ceka.Name = "ceka";
            this.ceka.Size = new System.Drawing.Size(112, 23);
            this.ceka.TabIndex = 6;
            this.ceka.Tag = "CustomColor";
            this.ceka.Text = "Čeká se";
            this.ceka.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vyreseno
            // 
            this.vyreseno.Location = new System.Drawing.Point(6, 19);
            this.vyreseno.Name = "vyreseno";
            this.vyreseno.Size = new System.Drawing.Size(112, 23);
            this.vyreseno.TabIndex = 5;
            this.vyreseno.Tag = "CustomColor";
            this.vyreseno.Text = "Vyřešeno";
            this.vyreseno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clr_probiha
            // 
            this.clr_probiha.BackColor = System.Drawing.SystemColors.Window;
            this.clr_probiha.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.clr_probiha.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.clr_probiha.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clr_probiha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clr_probiha.Location = new System.Drawing.Point(124, 135);
            this.clr_probiha.Name = "clr_probiha";
            this.clr_probiha.Size = new System.Drawing.Size(75, 23);
            this.clr_probiha.TabIndex = 4;
            this.clr_probiha.Tag = "probiha";
            this.clr_probiha.Text = "Změň";
            this.clr_probiha.UseVisualStyleBackColor = false;
            this.clr_probiha.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.clr_probiha.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.clr_probiha.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // crl_rdp
            // 
            this.crl_rdp.BackColor = System.Drawing.SystemColors.Window;
            this.crl_rdp.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.crl_rdp.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.crl_rdp.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.crl_rdp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.crl_rdp.Location = new System.Drawing.Point(124, 106);
            this.crl_rdp.Name = "crl_rdp";
            this.crl_rdp.Size = new System.Drawing.Size(75, 23);
            this.crl_rdp.TabIndex = 3;
            this.crl_rdp.Tag = "rdp";
            this.crl_rdp.Text = "Změň";
            this.crl_rdp.UseVisualStyleBackColor = false;
            this.crl_rdp.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.crl_rdp.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.crl_rdp.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // clr_odpoved
            // 
            this.clr_odpoved.BackColor = System.Drawing.SystemColors.Window;
            this.clr_odpoved.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.clr_odpoved.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.clr_odpoved.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clr_odpoved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clr_odpoved.Location = new System.Drawing.Point(124, 77);
            this.clr_odpoved.Name = "clr_odpoved";
            this.clr_odpoved.Size = new System.Drawing.Size(75, 23);
            this.clr_odpoved.TabIndex = 2;
            this.clr_odpoved.Tag = "odpoved";
            this.clr_odpoved.Text = "Změň";
            this.clr_odpoved.UseVisualStyleBackColor = false;
            this.clr_odpoved.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.clr_odpoved.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.clr_odpoved.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // clr_ceka
            // 
            this.clr_ceka.BackColor = System.Drawing.SystemColors.Window;
            this.clr_ceka.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.clr_ceka.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.clr_ceka.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clr_ceka.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clr_ceka.Location = new System.Drawing.Point(124, 48);
            this.clr_ceka.Name = "clr_ceka";
            this.clr_ceka.Size = new System.Drawing.Size(75, 23);
            this.clr_ceka.TabIndex = 1;
            this.clr_ceka.Tag = "ceka";
            this.clr_ceka.Text = "Změň";
            this.clr_ceka.UseVisualStyleBackColor = false;
            this.clr_ceka.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.clr_ceka.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.clr_ceka.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // clr_vyreseno
            // 
            this.clr_vyreseno.BackColor = System.Drawing.SystemColors.Window;
            this.clr_vyreseno.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.clr_vyreseno.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.clr_vyreseno.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.clr_vyreseno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clr_vyreseno.Location = new System.Drawing.Point(124, 19);
            this.clr_vyreseno.Name = "clr_vyreseno";
            this.clr_vyreseno.Size = new System.Drawing.Size(75, 23);
            this.clr_vyreseno.TabIndex = 0;
            this.clr_vyreseno.Tag = "vyreseno";
            this.clr_vyreseno.Text = "Změň";
            this.clr_vyreseno.UseVisualStyleBackColor = false;
            this.clr_vyreseno.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.clr_vyreseno.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.clr_vyreseno.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textHigh);
            this.groupBox2.Controls.Add(this.textOK);
            this.groupBox2.Controls.Add(this.textMid);
            this.groupBox2.Controls.Add(this.textLow);
            this.groupBox2.Controls.Add(this.defaultCasy);
            this.groupBox2.Controls.Add(this.zmenHigh);
            this.groupBox2.Controls.Add(this.zmenOK);
            this.groupBox2.Controls.Add(this.zmenMid);
            this.groupBox2.Controls.Add(this.zmenLow);
            this.groupBox2.Controls.Add(this.celkovyCasZobrazit);
            this.groupBox2.Location = new System.Drawing.Point(223, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 198);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zobrazení času za den";
            // 
            // textHigh
            // 
            this.textHigh.Location = new System.Drawing.Point(6, 135);
            this.textHigh.Name = "textHigh";
            this.textHigh.Size = new System.Drawing.Size(63, 23);
            this.textHigh.TabIndex = 9;
            this.textHigh.Tag = "CustomColor";
            this.textHigh.Text = "nad 8 hodin";
            this.textHigh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textOK
            // 
            this.textOK.Location = new System.Drawing.Point(6, 106);
            this.textOK.Name = "textOK";
            this.textOK.Size = new System.Drawing.Size(63, 23);
            this.textOK.TabIndex = 8;
            this.textOK.Tag = "CustomColor";
            this.textOK.Text = "8 hodin";
            this.textOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textMid
            // 
            this.textMid.Location = new System.Drawing.Point(6, 77);
            this.textMid.Name = "textMid";
            this.textMid.Size = new System.Drawing.Size(63, 23);
            this.textMid.TabIndex = 7;
            this.textMid.Tag = "CustomColor";
            this.textMid.Text = "4 - 8 hodin";
            this.textMid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textLow
            // 
            this.textLow.Location = new System.Drawing.Point(6, 48);
            this.textLow.Name = "textLow";
            this.textLow.Size = new System.Drawing.Size(63, 23);
            this.textLow.TabIndex = 6;
            this.textLow.Tag = "CustomColor";
            this.textLow.Text = "0 - 4 hodiny";
            this.textLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defaultCasy
            // 
            this.defaultCasy.BackColor = System.Drawing.SystemColors.Window;
            this.defaultCasy.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.defaultCasy.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.defaultCasy.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.defaultCasy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultCasy.Location = new System.Drawing.Point(6, 164);
            this.defaultCasy.Name = "defaultCasy";
            this.defaultCasy.Size = new System.Drawing.Size(143, 23);
            this.defaultCasy.TabIndex = 5;
            this.defaultCasy.Tag = "";
            this.defaultCasy.Text = "Default";
            this.defaultCasy.UseVisualStyleBackColor = false;
            this.defaultCasy.Click += new System.EventHandler(this.defaultCasy_Click);
            this.defaultCasy.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.defaultCasy.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // zmenHigh
            // 
            this.zmenHigh.BackColor = System.Drawing.SystemColors.Window;
            this.zmenHigh.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.zmenHigh.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.zmenHigh.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.zmenHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zmenHigh.Location = new System.Drawing.Point(74, 135);
            this.zmenHigh.Name = "zmenHigh";
            this.zmenHigh.Size = new System.Drawing.Size(75, 23);
            this.zmenHigh.TabIndex = 4;
            this.zmenHigh.Tag = "high";
            this.zmenHigh.Text = "Změň";
            this.zmenHigh.UseVisualStyleBackColor = false;
            this.zmenHigh.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.zmenHigh.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.zmenHigh.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // zmenOK
            // 
            this.zmenOK.BackColor = System.Drawing.SystemColors.Window;
            this.zmenOK.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.zmenOK.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.zmenOK.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.zmenOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zmenOK.Location = new System.Drawing.Point(74, 106);
            this.zmenOK.Name = "zmenOK";
            this.zmenOK.Size = new System.Drawing.Size(75, 23);
            this.zmenOK.TabIndex = 3;
            this.zmenOK.Tag = "ok";
            this.zmenOK.Text = "Změň";
            this.zmenOK.UseVisualStyleBackColor = false;
            this.zmenOK.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.zmenOK.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.zmenOK.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // zmenMid
            // 
            this.zmenMid.BackColor = System.Drawing.SystemColors.Window;
            this.zmenMid.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.zmenMid.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.zmenMid.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.zmenMid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zmenMid.Location = new System.Drawing.Point(74, 77);
            this.zmenMid.Name = "zmenMid";
            this.zmenMid.Size = new System.Drawing.Size(75, 23);
            this.zmenMid.TabIndex = 2;
            this.zmenMid.Tag = "mid";
            this.zmenMid.Text = "Změň";
            this.zmenMid.UseVisualStyleBackColor = false;
            this.zmenMid.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.zmenMid.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.zmenMid.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // zmenLow
            // 
            this.zmenLow.BackColor = System.Drawing.SystemColors.Window;
            this.zmenLow.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.zmenLow.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.zmenLow.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.zmenLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zmenLow.Location = new System.Drawing.Point(74, 48);
            this.zmenLow.Name = "zmenLow";
            this.zmenLow.Size = new System.Drawing.Size(75, 23);
            this.zmenLow.TabIndex = 1;
            this.zmenLow.Tag = "low";
            this.zmenLow.Text = "Změň";
            this.zmenLow.UseVisualStyleBackColor = false;
            this.zmenLow.Click += new System.EventHandler(this.clr_vyreseno_Click);
            this.zmenLow.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.zmenLow.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // celkovyCasZobrazit
            // 
            this.celkovyCasZobrazit.AutoSize = true;
            this.celkovyCasZobrazit.BoxColor = System.Drawing.SystemColors.Window;
            this.celkovyCasZobrazit.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.celkovyCasZobrazit.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.celkovyCasZobrazit.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.celkovyCasZobrazit.Location = new System.Drawing.Point(6, 19);
            this.celkovyCasZobrazit.Name = "celkovyCasZobrazit";
            this.celkovyCasZobrazit.Size = new System.Drawing.Size(140, 17);
            this.celkovyCasZobrazit.TabIndex = 0;
            this.celkovyCasZobrazit.Text = "Zobrazovat celkový čas";
            this.celkovyCasZobrazit.UseVisualStyleBackColor = true;
            this.celkovyCasZobrazit.CheckedChanged += new System.EventHandler(this.celkovyCasZobrazit_CheckedChanged);
            this.celkovyCasZobrazit.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.celkovyCasZobrazit.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.jazykPopis);
            this.groupBox3.Controls.Add(this.jazyk);
            this.groupBox3.Controls.Add(this.zmenitJazyk);
            this.groupBox3.Location = new System.Drawing.Point(220, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(158, 100);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Jazyk";
            // 
            // jazykPopis
            // 
            this.jazykPopis.Location = new System.Drawing.Point(6, 43);
            this.jazykPopis.Name = "jazykPopis";
            this.jazykPopis.Size = new System.Drawing.Size(146, 25);
            this.jazykPopis.TabIndex = 2;
            this.jazykPopis.Text = "Int. jazyk a verze";
            this.jazykPopis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jazyk
            // 
            this.jazyk.BorderColor = System.Drawing.Color.LightGray;
            this.jazyk.ButtonColor = System.Drawing.SystemColors.Window;
            this.jazyk.ButtonColorMouseOver = System.Drawing.SystemColors.GradientInactiveCaption;
            this.jazyk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jazyk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jazyk.FormattingEnabled = true;
            this.jazyk.Location = new System.Drawing.Point(6, 19);
            this.jazyk.Name = "jazyk";
            this.jazyk.Size = new System.Drawing.Size(146, 21);
            this.jazyk.TabIndex = 1;
            this.jazyk.SelectedIndexChanged += new System.EventHandler(this.jazyk_SelectedIndexChanged);
            this.jazyk.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.jazyk.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // zmenitJazyk
            // 
            this.zmenitJazyk.BackColor = System.Drawing.Color.Gainsboro;
            this.zmenitJazyk.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.zmenitJazyk.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.zmenitJazyk.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.zmenitJazyk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zmenitJazyk.Location = new System.Drawing.Point(6, 71);
            this.zmenitJazyk.Name = "zmenitJazyk";
            this.zmenitJazyk.Size = new System.Drawing.Size(146, 23);
            this.zmenitJazyk.TabIndex = 0;
            this.zmenitJazyk.Text = "Změňit jazyk";
            this.zmenitJazyk.UseVisualStyleBackColor = false;
            this.zmenitJazyk.Click += new System.EventHandler(this.zmenitJazyk_Click);
            this.zmenitJazyk.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.zmenitJazyk.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // skryteNastaveni
            // 
            this.skryteNastaveni.BackColor = System.Drawing.SystemColors.Window;
            this.skryteNastaveni.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.skryteNastaveni.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.skryteNastaveni.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.skryteNastaveni.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.skryteNastaveni.Location = new System.Drawing.Point(226, 107);
            this.skryteNastaveni.Name = "skryteNastaveni";
            this.skryteNastaveni.Size = new System.Drawing.Size(146, 23);
            this.skryteNastaveni.TabIndex = 9;
            this.skryteNastaveni.Text = "Skrytá nastavení";
            this.skryteNastaveni.UseVisualStyleBackColor = false;
            this.skryteNastaveni.Click += new System.EventHandler(this.SkryteNastaveni_Click);
            this.skryteNastaveni.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.skryteNastaveni.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Window;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(229, 341);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(143, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Výchozí nastavení";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.button2.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // motiv
            // 
            this.motiv.AutoSize = true;
            this.motiv.Location = new System.Drawing.Point(12, 133);
            this.motiv.Name = "motiv";
            this.motiv.Size = new System.Drawing.Size(33, 13);
            this.motiv.TabIndex = 13;
            this.motiv.Text = "Motiv";
            // 
            // motivVyber
            // 
            this.motivVyber.BorderColor = System.Drawing.Color.LightGray;
            this.motivVyber.ButtonColor = System.Drawing.SystemColors.Window;
            this.motivVyber.ButtonColorMouseOver = System.Drawing.SystemColors.GradientInactiveCaption;
            this.motivVyber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.motivVyber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.motivVyber.FormattingEnabled = true;
            this.motivVyber.Location = new System.Drawing.Point(64, 130);
            this.motivVyber.Name = "motivVyber";
            this.motivVyber.Size = new System.Drawing.Size(153, 21);
            this.motivVyber.TabIndex = 12;
            this.motivVyber.SelectedIndexChanged += new System.EventHandler(this.motivVyber_SelectedIndexChanged);
            this.motivVyber.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.motivVyber.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // onlineTerp
            // 
            this.onlineTerp.AutoSize = true;
            this.onlineTerp.BoxColor = System.Drawing.SystemColors.Window;
            this.onlineTerp.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.onlineTerp.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.onlineTerp.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.onlineTerp.Location = new System.Drawing.Point(12, 107);
            this.onlineTerp.Name = "onlineTerp";
            this.onlineTerp.Size = new System.Drawing.Size(186, 17);
            this.onlineTerp.TabIndex = 10;
            this.onlineTerp.Text = "Používat primárně terpy z MyTime";
            this.onlineTerp.UseVisualStyleBackColor = true;
            this.onlineTerp.CheckedChanged += new System.EventHandler(this.onlineTerp_CheckedChanged);
            this.onlineTerp.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.onlineTerp.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BoxColor = System.Drawing.SystemColors.Window;
            this.checkBox1.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.checkBox1.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.checkBox1.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.checkBox1.Location = new System.Drawing.Point(12, 84);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(171, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Zjednodušené nastavení času";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.checkBox1.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.checkBox1.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown1.Location = new System.Drawing.Point(121, 58);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // autosave
            // 
            this.autosave.AutoSize = true;
            this.autosave.BoxColor = System.Drawing.SystemColors.Window;
            this.autosave.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.autosave.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.autosave.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.autosave.Location = new System.Drawing.Point(12, 35);
            this.autosave.Name = "autosave";
            this.autosave.Size = new System.Drawing.Size(130, 17);
            this.autosave.TabIndex = 1;
            this.autosave.Text = "Automatické ukládání";
            this.autosave.UseVisualStyleBackColor = true;
            this.autosave.CheckedChanged += new System.EventHandler(this.autosave_CheckedChanged);
            this.autosave.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.autosave.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // poStartu
            // 
            this.poStartu.AutoSize = true;
            this.poStartu.BoxColor = System.Drawing.SystemColors.Window;
            this.poStartu.BoxColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(100)))));
            this.poStartu.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
            this.poStartu.CheckedColorMouseOver = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(110)))), ((int)(((byte)(191)))));
            this.poStartu.Location = new System.Drawing.Point(12, 12);
            this.poStartu.Name = "poStartu";
            this.poStartu.Size = new System.Drawing.Size(161, 17);
            this.poStartu.TabIndex = 0;
            this.poStartu.Text = "Spouštět Ticketník po startu";
            this.poStartu.UseVisualStyleBackColor = true;
            this.poStartu.CheckedChanged += new System.EventHandler(this.poStartu_CheckedChanged);
            this.poStartu.MouseEnter += new System.EventHandler(this.event_MouseEnter);
            this.poStartu.MouseLeave += new System.EventHandler(this.event_MouseLeave);
            // 
            // Nastaveni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 390);
            this.Controls.Add(this.motiv);
            this.Controls.Add(this.motivVyber);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.onlineTerp);
            this.Controls.Add(this.skryteNastaveni);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.autosave);
            this.Controls.Add(this.poStartu);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(406, 429);
            this.MinimumSize = new System.Drawing.Size(406, 429);
            this.Name = "Nastaveni";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Nastavení";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ticketník.CustomControls.CheckBox poStartu;
        private Ticketník.CustomControls.CheckBox autosave;
        private Ticketník.CustomControls.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Ticketník.CustomControls.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label probiha;
        private System.Windows.Forms.Label rdp;
        private System.Windows.Forms.Label odpoved;
        private System.Windows.Forms.Label ceka;
        private System.Windows.Forms.Label vyreseno;
        private Ticketník.CustomControls.Button clr_probiha;
        private Ticketník.CustomControls.Button crl_rdp;
        private Ticketník.CustomControls.Button clr_odpoved;
        private Ticketník.CustomControls.Button clr_ceka;
        private Ticketník.CustomControls.Button clr_vyreseno;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private Ticketník.CustomControls.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private Ticketník.CustomControls.CheckBox celkovyCasZobrazit;
        private Ticketník.CustomControls.Button defaultCasy;
        private Ticketník.CustomControls.Button zmenHigh;
        private Ticketník.CustomControls.Button zmenOK;
        private Ticketník.CustomControls.Button zmenMid;
        private Ticketník.CustomControls.Button zmenLow;
        private System.Windows.Forms.Label textHigh;
        private System.Windows.Forms.Label textOK;
        private System.Windows.Forms.Label textMid;
        private System.Windows.Forms.Label textLow;
        private System.Windows.Forms.GroupBox groupBox3;
        private Ticketník.CustomControls.ComboBox jazyk;
        private Ticketník.CustomControls.Button zmenitJazyk;
        private System.Windows.Forms.Label jazykPopis;
        private Ticketník.CustomControls.Button clr_prescas;
        private System.Windows.Forms.Label prescas;
        private Ticketník.CustomControls.Button skryteNastaveni;
        private Ticketník.CustomControls.CheckBox onlineTerp;
        private Ticketník.CustomControls.Button button2;
        private Ticketník.CustomControls.ComboBox motivVyber;
        private System.Windows.Forms.Label motiv;
    }
}