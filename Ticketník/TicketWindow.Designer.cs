namespace Ticketník
{
    partial class TicketWindow
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
            this.idTicketu = new System.Windows.Forms.TextBox();
            this.zakaznik = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pocitac = new System.Windows.Forms.TextBox();
            this.kontakt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.search_btn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.popis = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cas = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.upravit = new System.Windows.Forms.Button();
            this.smazat = new System.Windows.Forms.Button();
            this.pridat = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.pauzaDo = new System.Windows.Forms.TextBox();
            this.pauzaOd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.konec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.zacatek = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.mdm = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.nahradni = new System.Windows.Forms.RadioButton();
            this.volno = new System.Windows.Forms.RadioButton();
            this.popisTypu = new System.Windows.Forms.Label();
            this.terpKod = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.celkemPauza = new System.Windows.Forms.Label();
            this.odpracovano = new System.Windows.Forms.Label();
            this.cistehocasu = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.normalni = new System.Windows.Forms.RadioButton();
            this.prescas = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.stavTicketu = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.ok = new System.Windows.Forms.Button();
            this.datum = new System.Windows.Forms.DateTimePicker();
            this.newTerpTaskPanel = new System.Windows.Forms.Panel();
            this.onlineTerpDropDown = new System.Windows.Forms.ComboBox();
            this.lbl_TicketWindow_onlineTerp = new System.Windows.Forms.Label();
            this.onlineTaskComboBox = new System.Windows.Forms.ComboBox();
            this.onlineTypeComboBox = new System.Windows.Forms.ComboBox();
            this.lbl_TicketWindow_onlineTask = new System.Windows.Forms.Label();
            this.lbl_TicketWindow_onlineType = new System.Windows.Forms.Label();
            this.btn_TicketWindow_UpdateSelected = new System.Windows.Forms.Button();
            this.btn_TicketWindow_SearchTerp = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.newTerpTaskPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID ticketu";
            // 
            // idTicketu
            // 
            this.idTicketu.Location = new System.Drawing.Point(65, 19);
            this.idTicketu.Name = "idTicketu";
            this.idTicketu.Size = new System.Drawing.Size(122, 20);
            this.idTicketu.TabIndex = 1;
            this.idTicketu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.idTicketu.TextChanged += new System.EventHandler(this.idTicketu_TextChanged);
            // 
            // zakaznik
            // 
            this.zakaznik.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zakaznik.FormattingEnabled = true;
            this.zakaznik.Location = new System.Drawing.Point(65, 45);
            this.zakaznik.Name = "zakaznik";
            this.zakaznik.Size = new System.Drawing.Size(156, 21);
            this.zakaznik.TabIndex = 2;
            this.zakaznik.SelectedIndexChanged += new System.EventHandler(this.zakaznik_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Zákazník";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Počítač";
            // 
            // pocitac
            // 
            this.pocitac.Location = new System.Drawing.Point(65, 72);
            this.pocitac.Name = "pocitac";
            this.pocitac.Size = new System.Drawing.Size(156, 20);
            this.pocitac.TabIndex = 5;
            this.pocitac.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // kontakt
            // 
            this.kontakt.Location = new System.Drawing.Point(65, 98);
            this.kontakt.Name = "kontakt";
            this.kontakt.Size = new System.Drawing.Size(156, 20);
            this.kontakt.TabIndex = 6;
            this.kontakt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Kontakt";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Začátek";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.search_btn);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.popis);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.idTicketu);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.zakaznik);
            this.groupBox1.Controls.Add(this.kontakt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pocitac);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 154);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Základní informace";
            // 
            // search_btn
            // 
            this.search_btn.Image = global::Ticketník.Properties.Resources.search;
            this.search_btn.Location = new System.Drawing.Point(195, 15);
            this.search_btn.Name = "search_btn";
            this.search_btn.Size = new System.Drawing.Size(26, 26);
            this.search_btn.TabIndex = 10;
            this.search_btn.UseVisualStyleBackColor = true;
            this.search_btn.Click += new System.EventHandler(this.search_btn_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Location = new System.Drawing.Point(6, 127);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 9;
            this.label14.Text = "Popis";
            // 
            // popis
            // 
            this.popis.Location = new System.Drawing.Point(65, 124);
            this.popis.Name = "popis";
            this.popis.Size = new System.Drawing.Size(156, 20);
            this.popis.TabIndex = 8;
            this.popis.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cas);
            this.groupBox2.Location = new System.Drawing.Point(12, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 194);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Doba práce na ticketu";
            // 
            // cas
            // 
            this.cas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cas.FormattingEnabled = true;
            this.cas.Location = new System.Drawing.Point(6, 19);
            this.cas.Name = "cas";
            this.cas.Size = new System.Drawing.Size(220, 21);
            this.cas.TabIndex = 0;
            this.cas.SelectedIndexChanged += new System.EventHandler(this.cas_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.upravit);
            this.groupBox3.Controls.Add(this.smazat);
            this.groupBox3.Controls.Add(this.pridat);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.pauzaDo);
            this.groupBox3.Controls.Add(this.pauzaOd);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.listView1);
            this.groupBox3.Location = new System.Drawing.Point(6, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 143);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pauzy";
            // 
            // upravit
            // 
            this.upravit.Enabled = false;
            this.upravit.Location = new System.Drawing.Point(131, 53);
            this.upravit.Name = "upravit";
            this.upravit.Size = new System.Drawing.Size(82, 23);
            this.upravit.TabIndex = 7;
            this.upravit.Text = "Upravit";
            this.upravit.UseVisualStyleBackColor = true;
            this.upravit.Click += new System.EventHandler(this.upravit_Click);
            // 
            // smazat
            // 
            this.smazat.Enabled = false;
            this.smazat.Location = new System.Drawing.Point(131, 82);
            this.smazat.Name = "smazat";
            this.smazat.Size = new System.Drawing.Size(82, 23);
            this.smazat.TabIndex = 6;
            this.smazat.Text = "Smazat";
            this.smazat.UseVisualStyleBackColor = true;
            this.smazat.Click += new System.EventHandler(this.smazat_Click);
            // 
            // pridat
            // 
            this.pridat.Location = new System.Drawing.Point(132, 24);
            this.pridat.Name = "pridat";
            this.pridat.Size = new System.Drawing.Size(82, 23);
            this.pridat.TabIndex = 5;
            this.pridat.Text = "Přidat";
            this.pridat.UseVisualStyleBackColor = true;
            this.pridat.Click += new System.EventHandler(this.pridat_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(108, 120);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Do";
            // 
            // pauzaDo
            // 
            this.pauzaDo.Location = new System.Drawing.Point(135, 117);
            this.pauzaDo.MaxLength = 5;
            this.pauzaDo.Name = "pauzaDo";
            this.pauzaDo.Size = new System.Drawing.Size(43, 20);
            this.pauzaDo.TabIndex = 3;
            this.pauzaDo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pauzaOd
            // 
            this.pauzaOd.Location = new System.Drawing.Point(59, 117);
            this.pauzaOd.MaxLength = 5;
            this.pauzaOd.Name = "pauzaOd";
            this.pauzaOd.Size = new System.Drawing.Size(43, 20);
            this.pauzaOd.TabIndex = 2;
            this.pauzaOd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Od";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(6, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(120, 92);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "od";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "pomlcka";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 16;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "do";
            this.columnHeader3.Width = 50;
            // 
            // konec
            // 
            this.konec.Location = new System.Drawing.Point(178, 19);
            this.konec.MaxLength = 5;
            this.konec.Name = "konec";
            this.konec.Size = new System.Drawing.Size(43, 20);
            this.konec.TabIndex = 10;
            this.konec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.konec.TextChanged += new System.EventHandler(this.zacatek_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(114, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Konec";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zacatek
            // 
            this.zacatek.Location = new System.Drawing.Point(65, 19);
            this.zacatek.MaxLength = 5;
            this.zacatek.Name = "zacatek";
            this.zacatek.Size = new System.Drawing.Size(43, 20);
            this.zacatek.TabIndex = 0;
            this.zacatek.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.zacatek.TextChanged += new System.EventHandler(this.zacatek_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox3);
            this.groupBox6.Controls.Add(this.konec);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.zacatek);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Location = new System.Drawing.Point(12, 172);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(232, 194);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Časy";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.newTerpTaskPanel);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.mdm);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Controls.Add(this.nahradni);
            this.groupBox4.Controls.Add(this.volno);
            this.groupBox4.Controls.Add(this.popisTypu);
            this.groupBox4.Controls.Add(this.terpKod);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.celkemPauza);
            this.groupBox4.Controls.Add(this.odpracovano);
            this.groupBox4.Controls.Add(this.cistehocasu);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.normalni);
            this.groupBox4.Controls.Add(this.prescas);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.stavTicketu);
            this.groupBox4.Location = new System.Drawing.Point(250, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(284, 154);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rozšířené informace";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(154, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Vlastní terp a task";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mdm
            // 
            this.mdm.AutoSize = true;
            this.mdm.Location = new System.Drawing.Point(151, 66);
            this.mdm.Name = "mdm";
            this.mdm.Size = new System.Drawing.Size(52, 17);
            this.mdm.TabIndex = 16;
            this.mdm.Text = "MDM";
            this.mdm.UseVisualStyleBackColor = true;
            this.mdm.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(209, 67);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 17);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Enkrypce";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // nahradni
            // 
            this.nahradni.AutoSize = true;
            this.nahradni.Location = new System.Drawing.Point(15, 66);
            this.nahradni.Name = "nahradni";
            this.nahradni.Size = new System.Drawing.Size(99, 17);
            this.nahradni.TabIndex = 14;
            this.nahradni.TabStop = true;
            this.nahradni.Text = "Náhradní volno";
            this.nahradni.UseVisualStyleBackColor = true;
            this.nahradni.CheckedChanged += new System.EventHandler(this.normalni_CheckedChanged);
            // 
            // volno
            // 
            this.volno.AutoSize = true;
            this.volno.Location = new System.Drawing.Point(186, 46);
            this.volno.Name = "volno";
            this.volno.Size = new System.Drawing.Size(59, 17);
            this.volno.TabIndex = 13;
            this.volno.TabStop = true;
            this.volno.Text = "Svátek";
            this.volno.UseVisualStyleBackColor = true;
            this.volno.CheckedChanged += new System.EventHandler(this.normalni_CheckedChanged);
            // 
            // popisTypu
            // 
            this.popisTypu.Location = new System.Drawing.Point(175, 114);
            this.popisTypu.Name = "popisTypu";
            this.popisTypu.Size = new System.Drawing.Size(103, 13);
            this.popisTypu.TabIndex = 12;
            this.popisTypu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // terpKod
            // 
            this.terpKod.Location = new System.Drawing.Point(175, 100);
            this.terpKod.Name = "terpKod";
            this.terpKod.Size = new System.Drawing.Size(103, 13);
            this.terpKod.TabIndex = 11;
            this.terpKod.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(196, 86);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "TERP kód";
            // 
            // celkemPauza
            // 
            this.celkemPauza.AutoSize = true;
            this.celkemPauza.Location = new System.Drawing.Point(132, 114);
            this.celkemPauza.Name = "celkemPauza";
            this.celkemPauza.Size = new System.Drawing.Size(28, 13);
            this.celkemPauza.TabIndex = 9;
            this.celkemPauza.Text = "0:00";
            // 
            // odpracovano
            // 
            this.odpracovano.AutoSize = true;
            this.odpracovano.Location = new System.Drawing.Point(132, 100);
            this.odpracovano.Name = "odpracovano";
            this.odpracovano.Size = new System.Drawing.Size(28, 13);
            this.odpracovano.TabIndex = 8;
            this.odpracovano.Text = "0:00";
            // 
            // cistehocasu
            // 
            this.cistehocasu.AutoSize = true;
            this.cistehocasu.Location = new System.Drawing.Point(132, 86);
            this.cistehocasu.Name = "cistehocasu";
            this.cistehocasu.Size = new System.Drawing.Size(28, 13);
            this.cistehocasu.TabIndex = 7;
            this.cistehocasu.Text = "0:00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Celková doba pauzy";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Odpracováno";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Čistě odpracováno";
            // 
            // normalni
            // 
            this.normalni.AutoSize = true;
            this.normalni.Location = new System.Drawing.Point(15, 46);
            this.normalni.Name = "normalni";
            this.normalni.Size = new System.Drawing.Size(95, 17);
            this.normalni.TabIndex = 3;
            this.normalni.TabStop = true;
            this.normalni.Text = "Normální doba";
            this.normalni.UseVisualStyleBackColor = true;
            this.normalni.CheckedChanged += new System.EventHandler(this.normalni_CheckedChanged);
            // 
            // prescas
            // 
            this.prescas.AutoSize = true;
            this.prescas.Location = new System.Drawing.Point(116, 46);
            this.prescas.Name = "prescas";
            this.prescas.Size = new System.Drawing.Size(64, 17);
            this.prescas.TabIndex = 2;
            this.prescas.TabStop = true;
            this.prescas.Text = "Přesčas";
            this.prescas.UseVisualStyleBackColor = true;
            this.prescas.CheckedChanged += new System.EventHandler(this.normalni_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Stav ticketu";
            // 
            // stavTicketu
            // 
            this.stavTicketu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stavTicketu.FormattingEnabled = true;
            this.stavTicketu.Location = new System.Drawing.Point(114, 19);
            this.stavTicketu.Name = "stavTicketu";
            this.stavTicketu.Size = new System.Drawing.Size(164, 21);
            this.stavTicketu.TabIndex = 0;
            this.stavTicketu.SelectedIndexChanged += new System.EventHandler(this.stavTicketu_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Location = new System.Drawing.Point(250, 172);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(284, 165);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Poznámky";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(272, 140);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(459, 343);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 13;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // datum
            // 
            this.datum.Location = new System.Drawing.Point(250, 343);
            this.datum.Name = "datum";
            this.datum.Size = new System.Drawing.Size(160, 20);
            this.datum.TabIndex = 14;
            this.datum.CloseUp += new System.EventHandler(this.datum_CloseUp);
            this.datum.ValueChanged += new System.EventHandler(this.datum_ValueChanged);
            this.datum.DropDown += new System.EventHandler(this.datum_DropDown);
            this.datum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.datum_KeyDown);
            // 
            // newTerpTaskPanel
            // 
            this.newTerpTaskPanel.Controls.Add(this.btn_TicketWindow_SearchTerp);
            this.newTerpTaskPanel.Controls.Add(this.btn_TicketWindow_UpdateSelected);
            this.newTerpTaskPanel.Controls.Add(this.lbl_TicketWindow_onlineType);
            this.newTerpTaskPanel.Controls.Add(this.lbl_TicketWindow_onlineTask);
            this.newTerpTaskPanel.Controls.Add(this.onlineTypeComboBox);
            this.newTerpTaskPanel.Controls.Add(this.onlineTaskComboBox);
            this.newTerpTaskPanel.Controls.Add(this.lbl_TicketWindow_onlineTerp);
            this.newTerpTaskPanel.Controls.Add(this.onlineTerpDropDown);
            this.newTerpTaskPanel.Location = new System.Drawing.Point(1, 46);
            this.newTerpTaskPanel.Name = "newTerpTaskPanel";
            this.newTerpTaskPanel.Size = new System.Drawing.Size(277, 106);
            this.newTerpTaskPanel.TabIndex = 18;
            // 
            // onlineTerpDropDown
            // 
            this.onlineTerpDropDown.FormattingEnabled = true;
            this.onlineTerpDropDown.Location = new System.Drawing.Point(99, 3);
            this.onlineTerpDropDown.Name = "onlineTerpDropDown";
            this.onlineTerpDropDown.Size = new System.Drawing.Size(121, 21);
            this.onlineTerpDropDown.TabIndex = 0;
            // 
            // lbl_TicketWindow_onlineTerp
            // 
            this.lbl_TicketWindow_onlineTerp.AutoSize = true;
            this.lbl_TicketWindow_onlineTerp.Location = new System.Drawing.Point(5, 6);
            this.lbl_TicketWindow_onlineTerp.Name = "lbl_TicketWindow_onlineTerp";
            this.lbl_TicketWindow_onlineTerp.Size = new System.Drawing.Size(29, 13);
            this.lbl_TicketWindow_onlineTerp.TabIndex = 1;
            this.lbl_TicketWindow_onlineTerp.Text = "Terp";
            // 
            // onlineTaskComboBox
            // 
            this.onlineTaskComboBox.FormattingEnabled = true;
            this.onlineTaskComboBox.Location = new System.Drawing.Point(99, 30);
            this.onlineTaskComboBox.Name = "onlineTaskComboBox";
            this.onlineTaskComboBox.Size = new System.Drawing.Size(121, 21);
            this.onlineTaskComboBox.TabIndex = 2;
            // 
            // onlineTypeComboBox
            // 
            this.onlineTypeComboBox.FormattingEnabled = true;
            this.onlineTypeComboBox.Location = new System.Drawing.Point(99, 57);
            this.onlineTypeComboBox.Name = "onlineTypeComboBox";
            this.onlineTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.onlineTypeComboBox.TabIndex = 3;
            // 
            // lbl_TicketWindow_onlineTask
            // 
            this.lbl_TicketWindow_onlineTask.AutoSize = true;
            this.lbl_TicketWindow_onlineTask.Location = new System.Drawing.Point(5, 33);
            this.lbl_TicketWindow_onlineTask.Name = "lbl_TicketWindow_onlineTask";
            this.lbl_TicketWindow_onlineTask.Size = new System.Drawing.Size(31, 13);
            this.lbl_TicketWindow_onlineTask.TabIndex = 4;
            this.lbl_TicketWindow_onlineTask.Text = "Task";
            // 
            // lbl_TicketWindow_onlineType
            // 
            this.lbl_TicketWindow_onlineType.AutoSize = true;
            this.lbl_TicketWindow_onlineType.Location = new System.Drawing.Point(5, 60);
            this.lbl_TicketWindow_onlineType.Name = "lbl_TicketWindow_onlineType";
            this.lbl_TicketWindow_onlineType.Size = new System.Drawing.Size(25, 13);
            this.lbl_TicketWindow_onlineType.TabIndex = 5;
            this.lbl_TicketWindow_onlineType.Text = "Typ";
            // 
            // btn_TicketWindow_UpdateSelected
            // 
            this.btn_TicketWindow_UpdateSelected.Location = new System.Drawing.Point(2, 83);
            this.btn_TicketWindow_UpdateSelected.Name = "btn_TicketWindow_UpdateSelected";
            this.btn_TicketWindow_UpdateSelected.Size = new System.Drawing.Size(130, 23);
            this.btn_TicketWindow_UpdateSelected.TabIndex = 6;
            this.btn_TicketWindow_UpdateSelected.Text = "Aktualizovat vybrané";
            this.btn_TicketWindow_UpdateSelected.UseVisualStyleBackColor = true;
            // 
            // btn_TicketWindow_SearchTerp
            // 
            this.btn_TicketWindow_SearchTerp.Location = new System.Drawing.Point(137, 83);
            this.btn_TicketWindow_SearchTerp.Name = "btn_TicketWindow_SearchTerp";
            this.btn_TicketWindow_SearchTerp.Size = new System.Drawing.Size(138, 23);
            this.btn_TicketWindow_SearchTerp.TabIndex = 7;
            this.btn_TicketWindow_SearchTerp.Text = "Vyhledat Terp";
            this.btn_TicketWindow_SearchTerp.UseVisualStyleBackColor = true;
            // 
            // TicketWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 378);
            this.Controls.Add(this.datum);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(562, 417);
            this.MinimumSize = new System.Drawing.Size(562, 417);
            this.Name = "TicketWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TicketWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TicketWindow_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TicketWindow_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.newTerpTaskPanel.ResumeLayout(false);
            this.newTerpTaskPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label popisTypu;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label celkemPauza;
        private System.Windows.Forms.Label odpracovano;
        private System.Windows.Forms.Label cistehocasu;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox5;
        internal System.Windows.Forms.TextBox zacatek;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label14;
        internal System.Windows.Forms.TextBox idTicketu;
        internal System.Windows.Forms.ComboBox zakaznik;
        internal System.Windows.Forms.TextBox pocitac;
        internal System.Windows.Forms.TextBox kontakt;
        internal System.Windows.Forms.TextBox konec;
        internal System.Windows.Forms.Button upravit;
        internal System.Windows.Forms.Button smazat;
        internal System.Windows.Forms.Button pridat;
        internal System.Windows.Forms.TextBox pauzaDo;
        internal System.Windows.Forms.TextBox pauzaOd;
        internal System.Windows.Forms.RadioButton normalni;
        internal System.Windows.Forms.RadioButton prescas;
        internal System.Windows.Forms.ComboBox stavTicketu;
        internal System.Windows.Forms.RichTextBox richTextBox1;
        internal System.Windows.Forms.RadioButton volno;
        internal System.Windows.Forms.RadioButton nahradni;
        internal System.Windows.Forms.TextBox popis;
        internal System.Windows.Forms.ListView listView1;
        internal System.Windows.Forms.Button ok;
        internal System.Windows.Forms.CheckBox checkBox1;
        internal System.Windows.Forms.ComboBox cas;
        private System.Windows.Forms.CheckBox mdm;
        internal System.Windows.Forms.Label terpKod;
        private System.Windows.Forms.Button search_btn;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.DateTimePicker datum;
        private System.Windows.Forms.Panel newTerpTaskPanel;
        private System.Windows.Forms.Button btn_TicketWindow_SearchTerp;
        private System.Windows.Forms.Button btn_TicketWindow_UpdateSelected;
        private System.Windows.Forms.Label lbl_TicketWindow_onlineType;
        private System.Windows.Forms.Label lbl_TicketWindow_onlineTask;
        private System.Windows.Forms.ComboBox onlineTypeComboBox;
        private System.Windows.Forms.ComboBox onlineTaskComboBox;
        private System.Windows.Forms.Label lbl_TicketWindow_onlineTerp;
        private System.Windows.Forms.ComboBox onlineTerpDropDown;
    }
}