using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Xml;
using System.Linq;

namespace Ticketník
{
    public partial class Nastaveni : Form
    {
        bool muze = false;
        Form1 form;
        Dictionary<string, string> jazyky = new Dictionary<string, string>();

        public Nastaveni(Form1 form)
        {
            InitializeComponent();
            //tady se musí udělat načtení dostupných jazyků. Zároveň se i s cestou jako value načtou do jazyky. 
            //Key bude to samé, co v combo boxu
            this.form = form;

            //přidání EN
            jazyky.Add("English (EN)", "EN");
            jazyk.Items.Add("English (EN)");

            skryteNastaveni.Visible = form.devtest;

            foreach(string s in Directory.GetFiles(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang"))
            {
                XmlDocument lang = new XmlDocument();
                lang.Load(s);
                jazyky.Add(lang.DocumentElement.Attributes.GetNamedItem("name").InnerText + " (" + lang.DocumentElement.Attributes.GetNamedItem("shortcut").InnerText +")", s);
                jazyk.Items.Add(lang.DocumentElement.Attributes.GetNamedItem("name").InnerText + " (" + lang.DocumentElement.Attributes.GetNamedItem("shortcut").InnerText + ")");
            }

            Setlang();

            label1.Enabled = label2.Enabled = numericUpDown1.Enabled = autosave.Checked = Properties.Settings.Default.autosave;
            checkBox1.Checked = Properties.Settings.Default.shortTime;
            numericUpDown1.Value = Properties.Settings.Default.minuty;
            if (Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run").GetValue("Ticketnik") != null && Application.ExecutablePath == Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run").GetValue("Ticketnik").ToString())
            {
                poStartu.Checked = true;
            }
            else
                poStartu.Checked = false;
            celkovyCasZobrazit.Checked = Properties.Settings.Default.pouzivatCasy;
            onlineTerp.Checked = Properties.Settings.Default.onlineTerp;

            motivVyber.SelectedIndex = Properties.Settings.Default.motiv;
            groupBox1.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox2.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox3.Paint += new PaintEventHandler(groupBox_Paint);
            
            muze = true;

            Motiv.SetMotiv(this);

            probiha.BackColor = Properties.Settings.Default.probiha;
            ceka.BackColor = Properties.Settings.Default.ceka;
            odpoved.BackColor = Properties.Settings.Default.odpoved;
            rdp.BackColor = Properties.Settings.Default.rdp;
            vyreseno.BackColor = Properties.Settings.Default.vyreseno;
            prescas.BackColor = Properties.Settings.Default.prescas;
            probiha.ForeColor = form.ContrastColor(Properties.Settings.Default.probiha);
            ceka.ForeColor = form.ContrastColor(Properties.Settings.Default.ceka);
            odpoved.ForeColor = form.ContrastColor(Properties.Settings.Default.odpoved);
            rdp.ForeColor = form.ContrastColor(Properties.Settings.Default.rdp);
            vyreseno.ForeColor = form.ContrastColor(Properties.Settings.Default.vyreseno);
            prescas.ForeColor = form.ContrastColor(Properties.Settings.Default.prescas);
            textLow.BackColor = Properties.Settings.Default.timeLow;
            textMid.BackColor = Properties.Settings.Default.timeMid;
            textOK.BackColor = Properties.Settings.Default.timeOK;
            textHigh.BackColor = Properties.Settings.Default.timeLong;
            textLow.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLow);
            textMid.ForeColor = form.ContrastColor(Properties.Settings.Default.timeMid);
            textHigh.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLong);
            textOK.ForeColor = form.ContrastColor(Properties.Settings.Default.timeOK);

            if (Properties.Settings.Default.Jazyk == string.Empty || Properties.Settings.Default.Jazyk == "EN")
                jazyk.SelectedItem = "English (EN)";
            else
                jazyk.SelectedItem = form.jazyk.Jmeno + " (" + form.jazyk.Zkratka + ")";
        }

        private void poStartu_CheckedChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (poStartu.Checked)
                {
                    registryKey.SetValue("Ticketnik", Application.ExecutablePath);
                }
                else
                {
                    registryKey.DeleteValue("Ticketnik");
                }
            }
        }

        private void autosave_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = label2.Enabled = numericUpDown1.Enabled = Properties.Settings.Default.autosave = autosave.Checked;
            Properties.Settings.Default.Save();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.minuty = (short)numericUpDown1.Value;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.shortTime = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void clr_vyreseno_Click(object sender, EventArgs e)
        {
            switch ((string)((Button)sender).Tag)
            {
                case "vyreseno":
                    colorDialog1.Color = Properties.Settings.Default.vyreseno;
                    break;
                case "ceka":
                    colorDialog1.Color = Properties.Settings.Default.ceka;
                    break;
                case "odpoved":
                    colorDialog1.Color = Properties.Settings.Default.odpoved;
                    break;
                case "rdp":
                    colorDialog1.Color = Properties.Settings.Default.rdp;
                    break;
                case "probiha":
                    colorDialog1.Color = Properties.Settings.Default.probiha;
                    break;
                case "prescas":
                    colorDialog1.Color = Properties.Settings.Default.prescas;
                    break;
                case "low":
                    colorDialog1.Color = Properties.Settings.Default.timeLow;
                    break;
                case "mid":
                    colorDialog1.Color = Properties.Settings.Default.timeMid;
                    break;
                case "ok":
                    colorDialog1.Color = Properties.Settings.Default.timeOK;
                    break;
                case "high":
                    colorDialog1.Color = Properties.Settings.Default.timeLong;
                    break;
            }
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch ((string)((Button)sender).Tag)
                {
                    case "vyreseno":
                        Properties.Settings.Default.vyreseno = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        vyreseno.BackColor = Properties.Settings.Default.vyreseno;
                        vyreseno.ForeColor = form.ContrastColor(Properties.Settings.Default.vyreseno);
                        break;
                    case "ceka":
                        Properties.Settings.Default.ceka = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        ceka.BackColor = Properties.Settings.Default.ceka;
                        ceka.ForeColor = form.ContrastColor(Properties.Settings.Default.ceka);
                        break;
                    case "odpoved":
                        Properties.Settings.Default.odpoved = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        odpoved.BackColor = Properties.Settings.Default.odpoved;
                        odpoved.ForeColor = form.ContrastColor(Properties.Settings.Default.odpoved);
                        break;
                    case "rdp":
                        Properties.Settings.Default.rdp = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        rdp.BackColor = Properties.Settings.Default.rdp;
                        rdp.ForeColor = form.ContrastColor(Properties.Settings.Default.rdp);
                        break;
                    case "probiha":
                        Properties.Settings.Default.probiha = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        probiha.BackColor = Properties.Settings.Default.probiha;
                        probiha.ForeColor = form.ContrastColor(Properties.Settings.Default.probiha);
                        break;
                    case "prescas":
                        Properties.Settings.Default.prescas = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        prescas.BackColor = Properties.Settings.Default.prescas;
                        prescas.ForeColor = form.ContrastColor(Properties.Settings.Default.prescas);
                        break;
                    case "low":
                        Properties.Settings.Default.timeLow = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        textLow.BackColor = Properties.Settings.Default.timeLow;
                        textLow.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLow);
                        break;
                    case "mid":
                        Properties.Settings.Default.timeMid = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        textMid.BackColor = Properties.Settings.Default.timeMid;
                        textMid.ForeColor = form.ContrastColor(Properties.Settings.Default.timeMid);
                        break;
                    case "ok":
                        Properties.Settings.Default.timeOK = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        textOK.BackColor = Properties.Settings.Default.timeOK;
                        textOK.ForeColor = form.ContrastColor(Properties.Settings.Default.timeOK);
                        break;
                    case "high":
                        Properties.Settings.Default.timeLong = colorDialog1.Color;
                        Properties.Settings.Default.Save();
                        textHigh.BackColor = Properties.Settings.Default.timeLong;
                        textHigh.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLong);
                        break;
                }
                form.LoadFile();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.vyreseno = Color.FromArgb(255, 0, 200, 0);
            Properties.Settings.Default.ceka = Properties.Settings.Default.odpoved = Properties.Settings.Default.rdp = Color.Yellow;
            Properties.Settings.Default.probiha = Color.FromArgb(255, 255, 255, 160);
            Properties.Settings.Default.prescas = Color.Fuchsia;
            Properties.Settings.Default.Save();
            probiha.BackColor = Properties.Settings.Default.probiha;
            ceka.BackColor = Properties.Settings.Default.ceka;
            odpoved.BackColor = Properties.Settings.Default.odpoved;
            rdp.BackColor = Properties.Settings.Default.rdp;
            vyreseno.BackColor = Properties.Settings.Default.vyreseno;
            prescas.BackColor = Properties.Settings.Default.prescas;
            probiha.ForeColor = form.ContrastColor(Properties.Settings.Default.probiha);
            ceka.ForeColor = form.ContrastColor(Properties.Settings.Default.ceka);
            odpoved.ForeColor = form.ContrastColor(Properties.Settings.Default.odpoved);
            rdp.ForeColor = form.ContrastColor(Properties.Settings.Default.rdp);
            vyreseno.ForeColor = form.ContrastColor(Properties.Settings.Default.vyreseno);
            prescas.ForeColor = form.ContrastColor(Properties.Settings.Default.prescas);
            form.LoadFile();
        }

        private void celkovyCasZobrazit_CheckedChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                Properties.Settings.Default.pouzivatCasy = celkovyCasZobrazit.Checked;
                Properties.Settings.Default.Save();
                form.LoadFile();
            }
        }

        private void defaultCasy_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.timeLow = Color.Red;
            Properties.Settings.Default.timeMid = Color.Orange;
            Properties.Settings.Default.timeOK = Color.FromArgb(255, 0, 200, 0);
            Properties.Settings.Default.timeLong = Color.Fuchsia;
            textLow.BackColor = Properties.Settings.Default.timeLow;
            textMid.BackColor = Properties.Settings.Default.timeMid;
            textOK.BackColor = Properties.Settings.Default.timeOK;
            textHigh.BackColor = Properties.Settings.Default.timeLong;
            textLow.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLow);
            textMid.ForeColor = form.ContrastColor(Properties.Settings.Default.timeMid);
            textHigh.ForeColor = form.ContrastColor(Properties.Settings.Default.timeLong);
            textOK.ForeColor = form.ContrastColor(Properties.Settings.Default.timeOK);
            muze = false;
            Properties.Settings.Default.pouzivatCasy = true;
            Properties.Settings.Default.Save();
            celkovyCasZobrazit.Checked = Properties.Settings.Default.pouzivatCasy;
            muze = true;
            form.LoadFile();
        }

        private void zmenitJazyk_Click(object sender, EventArgs e)
        {
            if ((string)jazyk.SelectedItem != "English (EN)")
                Properties.Settings.Default.JazykCesta = jazyky[(string)jazyk.SelectedItem];
            else
                Properties.Settings.Default.JazykCesta = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml";
            int zacZav = ((string)jazyk.SelectedItem).LastIndexOf("(") + 1;
            int konZav = ((string)jazyk.SelectedItem).LastIndexOf(")");
            Properties.Settings.Default.Jazyk = ((string)jazyk.SelectedItem).Remove(konZav).Remove(0, zacZav);
            Properties.Settings.Default.Save();
            form.jazyk = new Jazyk();
            form.jazyk.Reload(form);
            form.LoadFile();

            Setlang();
        }

        void Setlang()
        {
            this.SuspendLayout();
            form.SuspendLayout();
            int selectedMotiv = motivVyber.SelectedIndex;
            poStartu.Text = form.jazyk.Windows_Nastaveni_PoStratu;
            autosave.Text = form.jazyk.Windows_Nastaveni_Autosave;
            label1.Text = form.jazyk.Windows_Nastaveni_UkladatKazdych;
            label2.Text = form.jazyk.Windows_Nastaveni_Minut;
            checkBox1.Text = form.jazyk.Windows_Nastaveni_ZjednodusenyCas;
            this.Text = form.jazyk.Windows_Nastaveni_Nastaveni;
            groupBox1.Text = form.jazyk.Windows_Nastaveni_BarvyTicketu;
            clr_ceka.Text = clr_odpoved.Text = clr_probiha.Text = clr_vyreseno.Text = crl_rdp.Text = clr_prescas.Text = zmenHigh.Text = zmenLow.Text = zmenMid.Text = zmenOK.Text = form.jazyk.Windows_Nastaveni_Zmen;
            vyreseno.Text = form.jazyk.Windows_Nastaveni_Vyreseno;
            ceka.Text = form.jazyk.Windows_Nastaveni_CekaSe;
            odpoved.Text = form.jazyk.Windows_Nastaveni_CekaSeNaOdpoved;
            rdp.Text = form.jazyk.Windows_Nastaveni_RDP;
            probiha.Text = form.jazyk.Windows_Nastaveni_Probiha;
            button1.Text = defaultCasy.Text = form.jazyk.Windows_Nastaveni_Default;
            groupBox2.Text = form.jazyk.Windows_Nastaveni_CasZaDen;
            celkovyCasZobrazit.Text = form.jazyk.Windows_Nastaveni_CelkovyCas;
            textLow.Text = "0 - 4 " + form.jazyk.Windows_Nastaveni_Hodiny;
            textMid.Text = "4 - 8 " + form.jazyk.Windows_Nastaveni_Hodin;
            textHigh.Text = form.jazyk.Windows_Nastaveni_Nad + " 8 " + form.jazyk.Windows_Nastaveni_Hodin;
            textOK.Text = "8 " + form.jazyk.Windows_Nastaveni_Hodin;
            groupBox3.Text = form.jazyk.Windows_Nastaveni_Jazyk;
            zmenitJazyk.Text = form.jazyk.Windows_Nastaveni_ZmenJazyk;
            prescas.Text = form.jazyk.Windows_Ticket_Prescas;
            onlineTerp.Text = form.jazyk.Windows_Nastaveni_OnlineTerp;
            button2.Text = form.jazyk.Windows_Nastaveni_Reset_Vychozi;
            motiv.Text = form.jazyk.Windows_Nastaveni_Theme;
            motivVyber.DataSource = new Dictionary<int, string>(){
                {0, form.jazyk.Windows_Nastaveni_Svetly },
                {1, form.jazyk.Windows_Nastaveni_Tmavy},
                {2, form.jazyk.Windows_Nastaveni_PodleSystemu}}.ToList();
            motivVyber.SelectedIndex = selectedMotiv;
            motivVyber.ValueMember = "Key";
            motivVyber.DisplayMember = "Value";
            this.ResumeLayout();
            form.ResumeLayout();
            Motiv.SetMotiv(this);
        }

        private void jazyk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(muze)
            {
                if((string)jazyk.SelectedItem == "English (EN)")
                {
                    XmlDocument lang = new XmlDocument();
                    lang.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml");

                    jazykPopis.Text = "English (EN) " + lang.DocumentElement.Attributes.GetNamedItem("version").InnerText + "." + lang.DocumentElement.Attributes.GetNamedItem("revision").InnerText;
                    zmenitJazyk.Text = lang.DocumentElement.SelectSingleNode("Windows/Settings/ChangeLanguage").Attributes.GetNamedItem("en").InnerText;
                        //Windows/Settings/Change
                }
                else
                {
                    XmlDocument lang = new XmlDocument();
                    lang.Load(jazyky[(string)jazyk.SelectedItem]);

                    jazykPopis.Text = lang.DocumentElement.Attributes.GetNamedItem("international").InnerText + " (" + lang.DocumentElement.Attributes.GetNamedItem("shortcut").InnerText + ") " + lang.DocumentElement.Attributes.GetNamedItem("version").InnerText + "." + lang.DocumentElement.Attributes.GetNamedItem("revision").InnerText;
                    zmenitJazyk.Text = lang.DocumentElement.SelectSingleNode("Windows/Settings/ChangeLanguage").InnerText;
                }
            }
        }

        private void SkryteNastaveni_Click(object sender, EventArgs e)
        {
            SkrytaNastaveni sn = new SkrytaNastaveni(form);
            sn.StartPosition = FormStartPosition.Manual;
            sn.Location = this.Location;
            sn.ShowDialog();
        }

        private void onlineTerp_CheckedChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                if (!onlineTerp.Checked)
                    MessageBox.Show(form.jazyk.Message_DisableNotRecommended);
                Properties.Settings.Default.onlineTerp = onlineTerp.Checked;
                form.terpToolStripMenuItem.Visible = form.přidatTERPKódToolStripMenuItem.Visible = form.upravitTERPKódToolStripMenuItem.Visible = form.smazatTERPKódToolStripMenuItem.Visible = !onlineTerp.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.filePath = "";
            Properties.Settings.Default.lastSelected = "";
            Properties.Settings.Default.velikost = new System.Drawing.Size(850, 460);
            Properties.Settings.Default.maximized = false;
            Properties.Settings.Default.umisteni = new System.Drawing.Point(0, 0);
            Properties.Settings.Default.minuty = 5;
            Properties.Settings.Default.autosave = false;
            Properties.Settings.Default.colPC = 68;
            Properties.Settings.Default.colID = 82;
            Properties.Settings.Default.colZak = 84;
            Properties.Settings.Default.colPop = 92;
            Properties.Settings.Default.colKon = 89;
            Properties.Settings.Default.colOd = 49;
            Properties.Settings.Default.colDo = 49;
            Properties.Settings.Default.colPau = 44;
            Properties.Settings.Default.colStav = 60;
            Properties.Settings.Default.colPoz = 149;
            Properties.Settings.Default.shortTime = true;
            Properties.Settings.Default.probiha = System.Drawing.Color.FromArgb(255, 255, 160);
            Properties.Settings.Default.ceka = System.Drawing.Color.Yellow;
            Properties.Settings.Default.odpoved = System.Drawing.Color.Yellow;
            Properties.Settings.Default.rdp = System.Drawing.Color.Yellow;
            Properties.Settings.Default.vyreseno = System.Drawing.Color.FromArgb(0, 200, 0);
            Properties.Settings.Default.prescas = System.Drawing.Color.Fuchsia;
            Properties.Settings.Default.updateCesta = @"\\10.14.18.19\Shareforyou\tools\Ticketnik\Update";
            Properties.Settings.Default.ZalozniUpdate = "https://github.com/Caesar008/Ticketnik/raw/master/Ticketn%C3%ADk/bin/Release";
            Properties.Settings.Default.pouzivatZalozniUpdate = true;
            Properties.Settings.Default.NovyExport = true;
            Properties.Settings.Default.timeLow = System.Drawing.Color.Red;
            Properties.Settings.Default.timeMid = System.Drawing.Color.Orange;
            Properties.Settings.Default.timeLong = System.Drawing.Color.Fuchsia;
            Properties.Settings.Default.timeOK = System.Drawing.Color.FromArgb(0, 200, 0);
            Properties.Settings.Default.pouzivatCasy = true;
            Properties.Settings.Default.Jazyk = "";
            Properties.Settings.Default.JazykCesta = "";
            Properties.Settings.Default.colIDPoradi = 2;
            Properties.Settings.Default.colPCPoradi = 1;
            Properties.Settings.Default.colZakPoradi = 3;
            Properties.Settings.Default.colPopPoradi = 4;
            Properties.Settings.Default.colKonPoradi = 5;
            Properties.Settings.Default.colTerpPoradi = 6;
            Properties.Settings.Default.colTaskPoradi = 7;
            Properties.Settings.Default.colCasPoradi = 8;
            Properties.Settings.Default.colStavPoradi = 9;
            Properties.Settings.Default.colPozPoradi = 10;
            Properties.Settings.Default.onlineTerp = true;
            Properties.Settings.Default.lastUpdateNotif = 0;
            Properties.Settings.Default.motiv = 2;

            Properties.Settings.Default.Save();
            try
            {
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                registryKey.DeleteValue("Ticketnik");
            }
            catch { }
            Motiv.SetMotiv(this);
        }

        private void motivVyber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                Properties.Settings.Default.motiv = motivVyber.SelectedIndex;
                Motiv.SetMotiv(this);
                Motiv.SetMotiv(form);
            }
        }

        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            Motiv.SetGroupBoxRamecek((GroupBox)sender, e);
        }

        private void event_MouseEnter(object sender, EventArgs e)
        {
            Motiv.SetControlColorOver(sender);
        }

        private void event_MouseLeave(object sender, EventArgs e)
        {
            Motiv.SetControlColor(sender);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
