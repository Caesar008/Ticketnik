using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using fNbt;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ticketník
{
    public partial class TicketWindow : Form
    {
        //Form1 form;
        Ticket ticket;
        bool muze = false, dat = false;
        Form1 form;
        DateTime pauzaCelkem = new DateTime();
        bool novy, okClick, _search = false;
        internal string terpt = "";
        internal string task = "";
        string puvodniID = "";
        int lastSelected;

        public TicketWindow(Form1 form, bool novy, bool search = false, ListViewItem listViewItem = null, string terp = "", string task = "")
        {
            InitializeComponent();
            this.novy = novy;
            this.form = form;
            this.terpt = terp;
            this.task = task;
            form.LoadTerptaskFile();
            _search = search;

            groupBox1.Text = form.jazyk.Windows_Ticket_ZakladniInfo;
            groupBox2.Text = form.jazyk.Windows_Ticket_DobaPrace;
            groupBox6.Text = form.jazyk.Windows_Ticket_Casy;
            groupBox3.Text = form.jazyk.Windows_Ticket_Pauzy;
            groupBox4.Text = form.jazyk.Windows_Ticket_RozsireneInformace;
            groupBox5.Text = form.jazyk.Windows_Ticket_Poznamky;
            label1.Text = form.jazyk.Windows_Ticket_IDTicketu;
            label2.Text = form.jazyk.Windows_Ticket_Zakaznik;
            label3.Text = form.jazyk.Windows_Ticket_Pocitac;
            label4.Text = form.jazyk.Windows_Ticket_Kontakt;
            label14.Text = form.jazyk.Windows_Ticket_Popis;
            label9.Text = form.jazyk.Windows_Ticket_StavTicketu;
            label10.Text = form.jazyk.Windows_Ticket_CisteOdpracovano;
            label11.Text = form.jazyk.Windows_Ticket_Odpracovano;
            label12.Text = form.jazyk.Windows_Ticket_DobaPauzy;
            label13.Text = form.jazyk.Windows_Ticket_TerpKod;
            button1.Text = form.jazyk.Windows_Ticket_VlastniTerp;
            normalni.Text = form.jazyk.Windows_Ticket_Normalni;
            prescas.Text = form.jazyk.Windows_Ticket_Prescas;
            volno.Text = form.jazyk.Windows_Ticket_Volno;
            nahradni.Text = form.jazyk.Windows_Ticket_NahradniVolno;
            upravit.Text = form.jazyk.Windows_Ticket_Upravit;
            smazat.Text = form.jazyk.Windows_Ticket_Smazat;
            pridat.Text = form.jazyk.Windows_Ticket_Pridat;
            label8.Text = form.jazyk.Windows_Ticket_Do;
            label7.Text = form.jazyk.Windows_Ticket_Od;
            label6.Text = form.jazyk.Windows_Ticket_Konec;
            label5.Text = form.jazyk.Windows_Ticket_Zacatek;
            lbl_TicketWindow_onlineTerp.Text = form.jazyk.Windows_Ticket_TerpOnline;
            lbl_TicketWindow_onlineTask.Text = form.jazyk.Windows_Ticket_TaskOnline;
            lbl_TicketWindow_onlineType.Text = form.jazyk.Windows_Ticket_TypOnline;
            btn_TicketWindow_SearchTerp.Text = form.jazyk.Windows_Ticket_VyhledatTerp;
            btn_TicketWindow_UpdateSelected.Text = form.jazyk.Windows_Ticket_AktualizovatTerp;

            if (!Properties.Settings.Default.onlineTerp || !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask"))
            {
                newTerpTaskPanel.Dispose();
                newTerpTaskPanel = null;
            }

            stavTicketu.Items.AddRange(new string[] { form.jazyk.Windows_Ticket_Probiha, form.jazyk.Windows_Ticket_CekaSe, form.jazyk.Windows_Ticket_CekaSeNaOdpoved, form.jazyk.Windows_Ticket_RDP, form.jazyk.Windows_Ticket_Hotovo });

            if (!Properties.Settings.Default.shortTime)
            {
                groupBox2.Visible = false;
                groupBox6.Visible = true;
            }
            else
            {
                groupBox6.Visible = false;
                groupBox2.Visible = true;
            }

            if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
            {
                ok.Enabled = false;
                btn_TicketWindow_SearchTerp.Enabled = false;
                btn_TicketWindow_UpdateSelected.Enabled = false;
                if(form.terpFile != null)
                {
                    while (form.terpTaskFileLock)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                    form.terpTaskFileLock = true;
                    foreach(MyTimeTerp onlineTerpy in form.Terpy.Values)
                    {
                        if(onlineTerpy.LastUpdate == form.TerpFileUpdate && !search)
                            onlineTerpDropDown.Items.Add(onlineTerpy.Label);
                        else if(search)
                            onlineTerpDropDown.Items.Add(onlineTerpy.Label);
                    }
                    onlineTerpDropDown.DropDownWidth = ComboWidth(onlineTerpDropDown);
                    onlineTerpDropDown.Sorted = true;
                    form.terpTaskFileLock = false;
                }
            }

            cas.Items.AddRange(new string[] { "30 " + form.jazyk.Windows_Ticket_Minut, "1 " + form.jazyk.Windows_Ticket_Hodina, "1,5 " + form.jazyk.Windows_Ticket_Hodiny, "2 " + form.jazyk.Windows_Ticket_Hodiny, "2,5 " + form.jazyk.Windows_Ticket_Hodiny, "3 " + form.jazyk.Windows_Ticket_Hodiny, "3,5 " + form.jazyk.Windows_Ticket_Hodiny, "4 " + form.jazyk.Windows_Ticket_Hodiny, "4,5 " + form.jazyk.Windows_Ticket_Hodiny, "5 " + form.jazyk.Windows_Ticket_Hodin, "5,5 " + form.jazyk.Windows_Ticket_Hodin, "6 " + form.jazyk.Windows_Ticket_Hodin, "6,5 " + form.jazyk.Windows_Ticket_Hodin, "7 " + form.jazyk.Windows_Ticket_Hodin, "7,5 " + form.jazyk.Windows_Ticket_Hodin, "8 " + form.jazyk.Windows_Ticket_Hodin });

            foreach (string s in form.list.DejZakazniky().Keys)
            {
                zakaznik.Items.Add(s);
            }
            if (!search)
            {
                if (novy)
                {
                    string mesic = "";
                    switch (DateTime.Today.Month)
                    {
                        case 1:
                            mesic = "Leden";
                            break;
                        case 2:
                            mesic = "Únor";
                            break;
                        case 3:
                            mesic = "Březen";
                            break;
                        case 4:
                            mesic = "Duben";
                            break;
                        case 5:
                            mesic = "Květen";
                            break;
                        case 6:
                            mesic = "Červen";
                            break;
                        case 7:
                            mesic = "Červenec";
                            break;
                        case 8:
                            mesic = "Srpen";
                            break;
                        case 9:
                            mesic = "Září";
                            break;
                        case 10:
                            mesic = "Říjen";
                            break;
                        case 11:
                            mesic = "Listopad";
                            break;
                        case 12:
                            mesic = "Prosinec";
                            break;
                    }

                    ticket = new Ticket(-1, mesic, DateTime.Today, DateTime.Today, DateTime.Today, new List<DateTime>(), new List<DateTime>(), Ticket.Stav.Probiha, "", "", "", "", "", Ticket.TypTicketu.Normalni);

                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_Probiha;

                    if (terpt != "")
                        ticket.CustomTerp = terpt;
                    if (task != "")
                        ticket.CustomTask = task;

                    muze = true;

                    if (Properties.Settings.Default.lastSelected != "")
                    {
                        zakaznik.SelectedItem = Properties.Settings.Default.lastSelected;
                    }
                    normalni.Checked = true;

                    dat = true;
                    datum.Value = DateTime.Today;
                    dat = false;
                }
                else
                {
                    //tu je úprava ticketu

                    foreach (TabPage tp in form.tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey(form.vybranyMesic))
                        {
                            Ticket refer = null;
                            Dictionary<string, List<Ticket>> tempD = form.poDnech[((Tag)((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].Tag).Datum];
                            foreach (Ticket t in tempD[((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text])
                            {
                                long id = ((Tag)((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].Tag).IDlong;

                                if (t.IDtick == id)
                                {
                                    refer = t;
                                    break;
                                }
                            }
                            NbtCompound referC = null;
                            if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                            {
                                foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety"))
                                {
                                    if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                                    {
                                        referC = c;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety"))
                                {
                                    if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                                    {
                                        referC = c;
                                        break;
                                    }
                                }
                            }
                            muze = true;
                            ticket = refer;

                            idTicketu.Text = refer.ID;
                            zakaznik.SelectedItem = refer.Zakaznik;

                            switch (refer.StavT)
                            {
                                case Ticket.Stav.Probiha:
                                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_Probiha;
                                    break;
                                case Ticket.Stav.Ceka_se:
                                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_CekaSe;
                                    break;
                                case Ticket.Stav.Ceka_se_na_odpoved:
                                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_CekaSeNaOdpoved;
                                    break;
                                case Ticket.Stav.RDP:
                                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_RDP;
                                    break;
                                case Ticket.Stav.Vyreseno:
                                    stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_Hotovo;
                                    break;
                            }

                            switch (refer.TypPrace)
                            {
                                case (byte)Ticket.TypTicketu.Normalni:
                                    normalni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.ProblemTicket:
                                    normalni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.NahradniVolno:
                                    nahradni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.ProblemNahradniVolno:
                                    nahradni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.PraceOPrazdniny:
                                    volno.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.ProblemOPrazdniny:
                                    volno.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.Prescas:
                                    prescas.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.ProblemPrescas:
                                    prescas.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.Custom:
                                    normalni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.CustomNahradni:
                                    nahradni.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.CustomOPrazdniny:
                                    volno.Checked = true;
                                    break;
                                case (byte)Ticket.TypTicketu.CustomPrescas:
                                    prescas.Checked = true;
                                    break;
                            }

                            pocitac.Text = refer.PC;
                            kontakt.Text = refer.Kontakt;
                            richTextBox1.Text = refer.Poznamky;
                            zacatek.Text = refer.Od.ToString("H:mm");

                            if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
                            {
                                string tmpSelected = refer.CustomTerp;
                                foreach (string s in onlineTerpDropDown.Items)
                                {
                                    if (s.StartsWith(tmpSelected + " "))
                                    {
                                        tmpSelected = s;
                                        break;
                                    }
                                }
                                if(onlineTerpDropDown.Items.Contains(tmpSelected))
                                    onlineTerpDropDown.SelectedItem = tmpSelected;

                                tmpSelected = refer.CustomTask;
                                foreach (string s in onlineTaskComboBox.Items)
                                {
                                    if (s.StartsWith(tmpSelected + " "))
                                    {
                                        tmpSelected = s;
                                        break;
                                    }
                                }
                                if(onlineTaskComboBox.Items.Contains(tmpSelected))
                                    onlineTaskComboBox.SelectedItem = tmpSelected;

                                if (refer.TypPrace == (byte)Ticket.TypTicketu.Normalni || refer.TypPrace == (byte)Ticket.TypTicketu.Custom ||
                                    refer.TypPrace == (byte)Ticket.TypTicketu.Enkripce || refer.TypPrace == (byte)Ticket.TypTicketu.EnkripceProblem ||
                                    refer.TypPrace == (byte)Ticket.TypTicketu.Mobility || refer.TypPrace == (byte)Ticket.TypTicketu.MobilityProblem ||
                                    refer.TypPrace == (byte)Ticket.TypTicketu.ProblemTicket)
                                {
                                    foreach (string s in onlineTypeComboBox.Items)
                                    {
                                        if (s.ToLower().StartsWith("normal "))
                                        {
                                            onlineTypeComboBox.SelectedItem = s;
                                            break;
                                        }
                                    }
                                }
                                if (onlineTypeComboBox.Items.Contains(refer.OnlineTyp))
                                    onlineTypeComboBox.SelectedItem = refer.OnlineTyp;
                            }


                            if (refer.Do.ToString("H:mm") == "0:00")
                                konec.Text = "";
                            else
                                konec.Text = refer.Do.ToString("H:mm");
                            popis.Text = refer.Popis;

                            //Na základě odpracovanoCiste udělat označení

                            switch (odpracovano.Text)
                            {
                                case "0:30":
                                    cas.SelectedItem = "30 " + form.jazyk.Windows_Ticket_Minut;
                                    break;
                                case "1:00":
                                    cas.SelectedItem = "1 " + form.jazyk.Windows_Ticket_Hodina;
                                    break;
                                case "1:30":
                                    cas.SelectedItem = "1,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "2:00":
                                    cas.SelectedItem = "2 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "2:30":
                                    cas.SelectedItem = "2,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "3:00":
                                    cas.SelectedItem = "3 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "3:30":
                                    cas.SelectedItem = "3,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "4:00":
                                    cas.SelectedItem = "4 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "4:30":
                                    cas.SelectedItem = "4,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                    break;
                                case "5:00":
                                    cas.SelectedItem = "5 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "5:30":
                                    cas.SelectedItem = "5,5 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "6:00":
                                    cas.SelectedItem = "6 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "6:30":
                                    cas.SelectedItem = "6,5 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "7:00":
                                    cas.SelectedItem = "7 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "7:30":
                                    cas.SelectedItem = "7,5 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                case "8:00":
                                    cas.SelectedItem = "8 " + form.jazyk.Windows_Ticket_Hodin;
                                    break;
                                default:
                                    break;
                            }

                            for (int i = 0; i < refer.PauzyOd.Count; i++)
                            {
                                string pauzyDoString = "";
                                if (refer.PauzyDo.Count - 1 >= i)
                                {
                                    if (refer.PauzyDo[i].ToString("H:mm") == "0:00")
                                        pauzyDoString = "";
                                    else
                                        pauzyDoString = refer.PauzyDo[i].ToString("H:mm");
                                }

                                ListViewItem lvi = new ListViewItem(new string[] { "", refer.PauzyOd[i].ToString("H:mm"), "-", pauzyDoString });

                                listView1.Items.Add(lvi);
                            }
                            UpdatePauza();

                            break;
                        }
                    }
                    dat = true;
                    datum.Value = ticket.Datum;
                    dat = false;
                }

            }
            else //při hledání
            {
                foreach (TabPage tp in form.tabControl1.Controls)
                {
                    if (tp.Controls.ContainsKey(form.vybranyMesic))
                    {
                        Ticket refer = null;
                        Dictionary<string, List<Ticket>> tempD = form.poDnech[((Tag)listViewItem.Tag).Datum];
                        foreach (Ticket t in tempD[listViewItem.SubItems[3].Text])
                        {
                            long id = ((Tag)listViewItem.Tag).IDlong;

                            if (t.IDtick == id)
                            {
                                refer = t;
                                break;
                            }
                        }
                        NbtCompound referC = null;
                        if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                        {
                            foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(listViewItem.SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety"))
                            {
                                if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                                {
                                    referC = c;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(listViewItem.SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety"))
                            {
                                if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                                {
                                    referC = c;
                                    break;
                                }
                            }
                        }
                        muze = true;
                        ticket = refer;

                        idTicketu.Text = refer.ID;
                        zakaznik.SelectedItem = refer.Zakaznik;

                        switch (refer.StavT)
                        {
                            case Ticket.Stav.Probiha:
                                stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_Probiha;
                                break;
                            case Ticket.Stav.Ceka_se:
                                stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_CekaSe;
                                break;
                            case Ticket.Stav.Ceka_se_na_odpoved:
                                stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_CekaSeNaOdpoved;
                                break;
                            case Ticket.Stav.RDP:
                                stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_RDP;
                                break;
                            case Ticket.Stav.Vyreseno:
                                stavTicketu.SelectedItem = form.jazyk.Windows_Ticket_Hotovo;
                                break;
                        }

                        switch (refer.TypPrace)
                        {
                            case (byte)Ticket.TypTicketu.Normalni:
                                normalni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.ProblemTicket:
                                normalni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.NahradniVolno:
                                nahradni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.ProblemNahradniVolno:
                                nahradni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.PraceOPrazdniny:
                                volno.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.ProblemOPrazdniny:
                                volno.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.Prescas:
                                prescas.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.ProblemPrescas:
                                prescas.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.Custom:
                                normalni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.CustomNahradni:
                                nahradni.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.CustomOPrazdniny:
                                volno.Checked = true;
                                break;
                            case (byte)Ticket.TypTicketu.CustomPrescas:
                                prescas.Checked = true;
                                break;
                        }

                        pocitac.Text = refer.PC;
                        kontakt.Text = refer.Kontakt;
                        richTextBox1.Text = refer.Poznamky;
                        zacatek.Text = refer.Od.ToString("H:mm");
                        dat = true;
                        datum.Value = refer.Datum;
                        dat = false;

                        if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
                        {
                            string tmpSelected = refer.CustomTerp;
                            foreach (string s in onlineTerpDropDown.Items)
                            {
                                if (s.StartsWith(tmpSelected + " "))
                                {
                                    tmpSelected = s;
                                    break;
                                }
                            }
                            if (onlineTerpDropDown.Items.Contains(tmpSelected))
                                onlineTerpDropDown.SelectedItem = tmpSelected;

                            tmpSelected = refer.CustomTask;
                            foreach (string s in onlineTaskComboBox.Items)
                            {
                                if (s.StartsWith(tmpSelected + " "))
                                {
                                    tmpSelected = s;
                                    break;
                                }
                            }
                            if (onlineTaskComboBox.Items.Contains(tmpSelected))
                                onlineTaskComboBox.SelectedItem = tmpSelected;

                            if (refer.TypPrace == (byte)Ticket.TypTicketu.Normalni || refer.TypPrace == (byte)Ticket.TypTicketu.Custom ||
                                refer.TypPrace == (byte)Ticket.TypTicketu.Enkripce || refer.TypPrace == (byte)Ticket.TypTicketu.EnkripceProblem ||
                                refer.TypPrace == (byte)Ticket.TypTicketu.Mobility || refer.TypPrace == (byte)Ticket.TypTicketu.MobilityProblem ||
                                refer.TypPrace == (byte)Ticket.TypTicketu.ProblemTicket)
                            {
                                foreach (string s in onlineTypeComboBox.Items)
                                {
                                    if (s.ToLower().StartsWith("normal "))
                                    {
                                        onlineTypeComboBox.SelectedItem = s;
                                        break;
                                    }
                                }
                            }
                            if (onlineTypeComboBox.Items.Contains(refer.OnlineTyp))
                                onlineTypeComboBox.SelectedItem = refer.OnlineTyp;
                        }

                        if (refer.Do.ToString("H:mm") == "0:00")
                            konec.Text = "";
                        else
                            konec.Text = refer.Do.ToString("H:mm");
                        popis.Text = refer.Popis;

                        //Na základě odpracovanoCiste udělat označení
                        switch (odpracovano.Text)
                        {
                            case "0:30":
                                cas.SelectedItem = "30 " + form.jazyk.Windows_Ticket_Minut;
                                break;
                            case "1:00":
                                cas.SelectedItem = "1 " + form.jazyk.Windows_Ticket_Hodina;
                                break;
                            case "1:30":
                                cas.SelectedItem = "1,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "2:00":
                                cas.SelectedItem = "2 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "2:30":
                                cas.SelectedItem = "2,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "3:00":
                                cas.SelectedItem = "3 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "3:30":
                                cas.SelectedItem = "3,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "4:00":
                                cas.SelectedItem = "4 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "4:30":
                                cas.SelectedItem = "4,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "5:00":
                                cas.SelectedItem = "5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "5:30":
                                cas.SelectedItem = "5,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "6:00":
                                cas.SelectedItem = "6 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "6:30":
                                cas.SelectedItem = "6,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "7:00":
                                cas.SelectedItem = "7 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "7:30":
                                cas.SelectedItem = "7,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "8:00":
                                cas.SelectedItem = "8 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            default:
                                break;
                        }

                        for (int i = 0; i < refer.PauzyOd.Count; i++)
                        {
                            string pauzyDoString = "";
                            if (refer.PauzyDo.Count - 1 >= i)
                            {
                                if (refer.PauzyDo[i].ToString("H:mm") == "0:00")
                                    pauzyDoString = "";
                                else
                                    pauzyDoString = refer.PauzyDo[i].ToString("H:mm");
                            }

                            ListViewItem lvi = new ListViewItem(new string[] { "", refer.PauzyOd[i].ToString("H:mm"), "-", pauzyDoString });

                            listView1.Items.Add(lvi);
                        }

                        UpdatePauza();

                        break;
                    }
                }
            }
            puvodniID = ticket.ID;
            groupBox1.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox2.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox3.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox4.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox5.Paint += new PaintEventHandler(groupBox_Paint);
            groupBox6.Paint += new PaintEventHandler(groupBox_Paint);
            
            Motiv.SetMotiv(this);
            Motiv.SetControlColor(prescas);
        }

        private void stavTicketu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                stavTicketu.Select(0, 0);
                if ((string)stavTicketu.SelectedItem == form.jazyk.Windows_Ticket_Probiha)
                       ticket.StavT = Ticket.Stav.Probiha;
                else if ((string)stavTicketu.SelectedItem == form.jazyk.Windows_Ticket_CekaSe)
                    ticket.StavT = Ticket.Stav.Ceka_se;
                else if ((string)stavTicketu.SelectedItem == form.jazyk.Windows_Ticket_CekaSeNaOdpoved)
                       ticket.StavT = Ticket.Stav.Ceka_se_na_odpoved;
                else if ((string)stavTicketu.SelectedItem == form.jazyk.Windows_Ticket_RDP)
                       ticket.StavT = Ticket.Stav.RDP;
                else if ((string)stavTicketu.SelectedItem == form.jazyk.Windows_Ticket_Hotovo)
                {
                    ticket.StavT = Ticket.Stav.Vyreseno;
                    if (konec.Text == "")
                    {
                        konec.Text = DateTime.Now.ToString("H:mm");
                        UpdateDoba();
                        switch (odpracovano.Text)
                        {
                            case "0:30":
                                cas.SelectedItem = "30 " + form.jazyk.Windows_Ticket_Minut;
                                break;
                            case "1:00":
                                cas.SelectedItem = "1 " + form.jazyk.Windows_Ticket_Hodina;
                                break;
                            case "1:30":
                                cas.SelectedItem = "1,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "2:00":
                                cas.SelectedItem = "2 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "2:30":
                                cas.SelectedItem = "2,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "3:00":
                                cas.SelectedItem = "3 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "3:30":
                                cas.SelectedItem = "3,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "4:00":
                                cas.SelectedItem = "4 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "4:30":
                                cas.SelectedItem = "4,5 " + form.jazyk.Windows_Ticket_Hodiny;
                                break;
                            case "5:00":
                                cas.SelectedItem = "5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "5:30":
                                cas.SelectedItem = "5,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "6:00":
                                cas.SelectedItem = "6 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "6:30":
                                cas.SelectedItem = "6,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "7:00":
                                cas.SelectedItem = "7 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "7:30":
                                cas.SelectedItem = "7,5 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            case "8:00":
                                cas.SelectedItem = "8 " + form.jazyk.Windows_Ticket_Hodin;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        internal string DejTerp()
        {
            string typ = "";
            string terp = "";

            if (ticket.CustomTerp == "" && terpt == "" && ticket.CustomTask == "" && typ == "")
            {
                if (idTicketu.Text.StartsWith("INC") || idTicketu.Text.StartsWith("RIT") || idTicketu.Text.StartsWith("ITASK") || idTicketu.Text.StartsWith("RTASK") || idTicketu.Text.StartsWith("TASK"))
                {
                    typ = " | " + Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                }
                else if (idTicketu.Text.StartsWith("PRB") || idTicketu.Text.StartsWith("PTASK"))
                    typ = " | " + Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;
                else
                    typ = " | -";

                if (zakaznik.SelectedItem != null && ((string)zakaznik.SelectedItem).Length != 0)
                    switch (Zakaznici.DejVelikost((string)zakaznik.SelectedItem))
                    {
                        case 0:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                            break;
                        case 1:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                            break;
                        case 2:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                            break;
                        case 127:
                            ticket.CustomTerp = terp = Zakaznici.GetTerp((string)zakaznik.SelectedItem);
                            break;
                        default:
                            terp = "-";
                            break;
                    }
            }
            else if ((ticket.CustomTerp == "" && terpt == "") || ticket.CustomTerp == Zakaznici.GetTerp(Properties.Settings.Default.lastSelected))
            {
                if (task == "" && ticket.CustomTask != "")
                    typ = " | " + ticket.CustomTask;
                else if (task != "")
                {
                    typ = " | " + task;
                    ticket.CustomTask = task;
                }
                else
                    typ = " | -";

                if (zakaznik.SelectedItem != null && ((string)zakaznik.SelectedItem).Length != 0)
                    switch (Zakaznici.DejVelikost((string)zakaznik.SelectedItem))
                    {
                        case 0:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                            break;
                        case 1:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                            break;
                        case 2:
                            terp = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                            break;
                        case 127:
                            ticket.CustomTerp = terp = Zakaznici.GetTerp((string)zakaznik.SelectedItem);
                            break;
                        default:
                            terp = "-";
                            break;
                    }

                ticket.CustomTerp = terp;
            }
            else if (ticket.CustomTask == "" && typ == "" && task == "")
            {
                if (idTicketu.Text.StartsWith("INC") || idTicketu.Text.StartsWith("RIT") || idTicketu.Text.StartsWith("ITASK") || idTicketu.Text.StartsWith("RTASK") || idTicketu.Text.StartsWith("TASK"))
                {
                    typ = " | " + Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                    task = ticket.CustomTask = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                }
                else if (idTicketu.Text.StartsWith("PRB") || idTicketu.Text.StartsWith("PTASK"))
                {
                    typ = " | " + Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;
                    task = ticket.CustomTask = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;
                }
                else
                    typ = " | -";

                terp = ticket.CustomTerp;
            }
            else
            {
                if (terpt != "")
                {
                    ticket.CustomTerp = terpt;
                    ticket.CustomTask = task;
                }
                terp = ticket.CustomTerp;
                typ = " | " + ticket.CustomTask;
            }

            return terp + typ;
        }

        //přidat terpy podle zákazníků
        private void normalni_CheckedChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                terpKod.Text = DejTerp();

                if (ticket.CustomTerp != "")
                {
                    if (normalni.Checked)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.Custom;
                        ticket.Terp = Ticket.TerpKod.Custom;
                        ticket.TerpT = Ticket.TerpTyp.Custom;
                        popisTypu.Text = form.jazyk.Windows_Ticket_NormalniPopis;
                    }
                    else if (prescas.Checked)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomPrescas;
                        ticket.Terp = Ticket.TerpKod.Custom;
                        ticket.TerpT = Ticket.TerpTyp.CustomPrescas;
                        popisTypu.Text = form.jazyk.Windows_Ticket_Prescas;
                    }
                    else if (volno.Checked)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomOPrazdniny;
                        ticket.Terp = Ticket.TerpKod.CustomHoliday;
                        ticket.TerpT = Ticket.TerpTyp.CustomHoliday;
                        popisTypu.Text = form.jazyk.Windows_Ticket_Volno;
                    }
                    else if (nahradni.Checked)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomNahradni;
                        ticket.Terp = Ticket.TerpKod.Custom;
                        ticket.TerpT = Ticket.TerpTyp.CustomNahradni;
                        popisTypu.Text = form.jazyk.Windows_Ticket_NahradniVolnoPopis;
                    }
                }
                else
                {
                    if (idTicketu.Text.StartsWith("INC") || idTicketu.Text.StartsWith("RIT") || idTicketu.Text.StartsWith("ITASK") || idTicketu.Text.StartsWith("RTASK") || idTicketu.Text.StartsWith("TASK"))
                    {
                        if (normalni.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.Normalni;
                            popisTypu.Text = form.jazyk.Windows_Ticket_NormalniPopis;
                        }
                        else if (prescas.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.Prescas;
                            popisTypu.Text = form.jazyk.Windows_Ticket_Prescas;
                        }
                        else if (volno.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.PraceOPrazdniny;
                            popisTypu.Text = form.jazyk.Windows_Ticket_Volno;
                        }
                        else if (nahradni.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.NahradniVolno;
                            popisTypu.Text = form.jazyk.Windows_Ticket_NahradniVolnoPopis;
                        }
                    }
                    else if (idTicketu.Text.StartsWith("PRB") || idTicketu.Text.StartsWith("PTASK"))
                    {

                        if (normalni.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.ProblemTicket;
                            popisTypu.Text = "PR " + form.jazyk.Windows_Ticket_NormalniPopis;
                        }
                        else if (prescas.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.ProblemPrescas;
                            popisTypu.Text = "PR " + form.jazyk.Windows_Ticket_Prescas;
                        }
                        else if (volno.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.ProblemOPrazdniny;
                            popisTypu.Text = "PR " + form.jazyk.Windows_Ticket_Volno;
                        }
                        else if (nahradni.Checked)
                        {
                            ticket.TypPrace = (byte)Ticket.TypTicketu.ProblemNahradniVolno;
                            popisTypu.Text = "PR " + form.jazyk.Windows_Ticket_NahradniVolnoPopis;
                        }
                    }
                }
            }
        }

        private string DejNewTerp(string oldTerp)
        {
            string terp = oldTerp.Split('|')[0];
            foreach(string mtterp in form.Terpy.Keys)
            {
                if(mtterp.StartsWith(terp))
                {
                    return mtterp;
                }
            }
            return "";
        }

        private void zakaznik_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(muze)
            {
                terpKod.Text = DejTerp();
                ticket.Zakaznik = (string)zakaznik.SelectedItem;
                string tmpSelected = DejNewTerp(terpKod.Text);
                foreach (string s in onlineTerpDropDown.Items)
                {
                    if (s.StartsWith(tmpSelected + " "))
                    {
                        tmpSelected = s;
                        break;
                    }
                }
                if (onlineTerpDropDown.Items.Contains(tmpSelected))
                    onlineTerpDropDown.SelectedItem = tmpSelected;
                Properties.Settings.Default.lastSelected = (string)zakaznik.SelectedItem;
                Properties.Settings.Default.Save();
            }
        }

        private void idTicketu_TextChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                terpKod.Text = DejTerp();
                ticket.ID = idTicketu.Text.Replace(',',';');
                normalni_CheckedChanged(sender, e);
            }

        }

        private void ok_Click(object sender, EventArgs e)
        {
            bool canClose = true;
            if (novy)
            {
                ticket.PC = pocitac.Text;
                ticket.Kontakt = kontakt.Text;
                ticket.Poznamky = richTextBox1.Text;

                string mesic = "";
                switch (ticket.Mesic)
                {
                    case "Leden":
                        mesic = "leden";
                        break;
                    case "Únor":
                        mesic = "unor";
                        break;
                    case "Březen":
                        mesic = "brezen";
                        break;
                    case "Duben":
                        mesic = "duben";
                        break;
                    case "Květen":
                        mesic = "kveten";
                        break;
                    case "Červen":
                        mesic = "cerven";
                        break;
                    case "Červenec":
                        mesic = "cervenec";
                        break;
                    case "Srpen":
                        mesic = "srpen";
                        break;
                    case "Září":
                        mesic = "zari";
                        break;
                    case "Říjen":
                        mesic = "rijen";
                        break;
                    case "Listopad":
                        mesic = "listopad";
                        break;
                    case "Prosinec":
                        mesic = "prosinec";
                        break;
                }


                string[] casOd = zacatek.Text.Split(':');
                string[] casDo = konec.Text.Split(':');

                if (casDo.Length != 2)
                    casDo = new string[] { "0", "0" };

                DateTime casOdD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(casOd[0]), int.Parse(casOd[1]), 0);
                DateTime casDoD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(casDo[0]), int.Parse(casDo[1]), 0);
                ticket.Od = casOdD;
                ticket.Do = casDoD;
                if (konec.Text.Split(':').Length == 2 && ticket.Do.Hour == 0 && ticket.Do.Minute == 0)
                {
                    ticket.Od = ticket.Od.AddMinutes(1);
                    ticket.Do = ticket.Do.AddMinutes(1);
                }
                ticket.Popis = popis.Text.Replace(',', ';').Replace("\"", "\'");
                ticket.IDtick = form.file.RootTag.Get<NbtLong>("MaxID").Value++;

                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                {
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik) == null)
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Zakaznik));
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic) == null)
                    {
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound(mesic));
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtString("Mesic", ticket.Mesic));
                    }
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety") == null)
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtList("Tickety", NbtTagType.Compound));

                    if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.OnlineTyp;
                        ticket.TerpT = Ticket.TerpTyp.OnlineTerpTask;
                        ticket.Terp = Ticket.TerpKod.OnlineTerp;
                        foreach (MyTimeTask mtt in form.Terpy[(string)onlineTerpDropDown.SelectedItem].Tasks)
                        {
                            if (mtt.Label == (string)onlineTaskComboBox.SelectedItem)
                            {
                                ticket.CustomTask = mtt.Label;
                                ticket.CustomTerp = form.Terpy[(string)onlineTerpDropDown.SelectedItem].Number;
                                ticket.OnlineTyp = (string)onlineTypeComboBox.SelectedItem;
                                break;
                            }
                        }
                    }

                    form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                }
                else
                {
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic) == null)
                    {
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound(mesic));
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtString("Mesic", ticket.Mesic));
                    }
                    if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety") == null)
                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtList("Tickety", NbtTagType.Compound));

                    if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
                    {
                        ticket.TypPrace = (byte)Ticket.TypTicketu.OnlineTyp;
                        ticket.TerpT = Ticket.TerpTyp.OnlineTerpTask;
                        ticket.Terp = Ticket.TerpKod.OnlineTerp;
                        foreach (MyTimeTask mtt in form.Terpy[(string)onlineTerpDropDown.SelectedItem].Tasks)
                        {
                            if (mtt.Label == (string)onlineTaskComboBox.SelectedItem)
                            {
                                ticket.CustomTask = mtt.Label;
                                ticket.CustomTerp = form.Terpy[(string)onlineTerpDropDown.SelectedItem].Number;
                                ticket.OnlineTyp = (string)onlineTypeComboBox.SelectedItem;
                                break;
                            }
                        }
                    }

                    form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                }

                if (ticket.CustomTerp != "" && ticket.CustomTask == "")
                {
                    canClose = false;
                    form.LoadFile();
                }

                if (canClose)
                {
                    form.uložitToolStripMenuItem_Click(sender, e);
                    form.toolStripButton2.Enabled = false;
                    form.toolStripButton3.Enabled = false;
                    form.LoadFile();
                }
            }
            else
            {
                //úprava toho, co je
                foreach (TabPage tp in form.tabControl1.Controls)
                {
                    if (tp.Controls.ContainsKey(form.vybranyMesic))
                    {
                        Ticket refer = null;

                        if (((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems.Count == 0)
                            ((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).Items[lastSelected].Selected = true;

                        Dictionary<string, List<Ticket>> tempD = form.poDnech[((Tag)((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].Tag).Datum];
                        foreach (Ticket t in tempD[((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text])
                        {
                            long id = ((Tag)((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].Tag).IDlong;

                            if (t.IDtick == id)
                            {
                                refer = t;
                                break;
                            }
                        }

                        NbtList list;
                        if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                            list = form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety");
                        else
                            list = form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety");

                        foreach (NbtCompound c in list)
                        {
                            if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                            {
                                string zakaznik = ((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedItems[0].SubItems[3].Text;
                                int index;

                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                    index = form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").IndexOf(c);
                                else
                                    index = form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").IndexOf(c);

                                ticket.Popis = popis.Text.Replace(',', ';').Replace("\"", "\'");
                                string[] casOd = zacatek.Text.Split(':');
                                string[] casDo = konec.Text.Split(':');
                                ticket.PC = pocitac.Text;
                                ticket.Kontakt = kontakt.Text;
                                ticket.Poznamky = richTextBox1.Text;

                                if (casOd.Length == 2)
                                {
                                    ticket.Od = new DateTime(ticket.Od.Year, ticket.Od.Month, ticket.Od.Day, int.Parse(casOd[0]), int.Parse(casOd[1]), 0);
                                }
                                if (casDo.Length == 2)
                                {
                                    ticket.Do = new DateTime(ticket.Do.Year, ticket.Do.Month, ticket.Do.Day, int.Parse(casDo[0]), int.Parse(casDo[1]), 0);
                                }
                                else
                                    ticket.Do = new DateTime(ticket.Do.Year, ticket.Do.Month, ticket.Do.Day, 0, 0, 0);
                                
                                if (konec.Text.Split(':').Length == 2 && ticket.Do.Hour == 0 && ticket.Do.Minute == 0)
                                {
                                    ticket.Od = ticket.Od.AddMinutes(1);
                                    ticket.Do = ticket.Do.AddMinutes(1);
                                }

                                if (newTerpTaskPanel != null && Properties.Settings.Default.onlineTerp)
                                {
                                    ticket.TypPrace = (byte)Ticket.TypTicketu.OnlineTyp;
                                    ticket.TerpT = Ticket.TerpTyp.OnlineTerpTask;
                                    ticket.Terp = Ticket.TerpKod.OnlineTerp;
                                    foreach (MyTimeTask mtt in form.Terpy[(string)onlineTerpDropDown.SelectedItem].Tasks)
                                    {
                                        if (mtt.Label == (string)onlineTaskComboBox.SelectedItem)
                                        {
                                            ticket.CustomTask = mtt.Label;
                                            ticket.CustomTerp = form.Terpy[(string)onlineTerpDropDown.SelectedItem].Number;
                                            ticket.OnlineTyp = (string)onlineTypeComboBox.SelectedItem;
                                            break;
                                        }
                                    }
                                }

                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                    form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety")[index] = ticket.GetNbtObject();
                                else
                                    form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety")[index] = ticket.GetNbtObject();

                                string mesic = "";
                                switch (form.vybranyMesic)
                                {
                                    case "leden":
                                        mesic = "Leden";
                                        break;
                                    case "unor":
                                        mesic = "Únor";
                                        break;
                                    case "brezen":
                                        mesic = "Březen";
                                        break;
                                    case "duben":
                                        mesic = "Duben";
                                        break;
                                    case "kveten":
                                        mesic = "Květen";
                                        break;
                                    case "cerven":
                                        mesic = "Červen";
                                        break;
                                    case "cervenec":
                                        mesic = "Červenec";
                                        break;
                                    case "srpen":
                                        mesic = "Srpen";
                                        break;
                                    case "zari":
                                        mesic = "Září";
                                        break;
                                    case "rijen":
                                        mesic = "Říjen";
                                        break;
                                    case "listopad":
                                        mesic = "Listopad";
                                        break;
                                    case "prosinec":
                                        mesic = "Prosinec";
                                        break;
                                }

                                if (zakaznik != (string)this.zakaznik.SelectedItem)
                                {
                                    if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound((string)this.zakaznik.SelectedItem));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic) == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Add(new NbtCompound(form.vybranyMesic));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Add(new NbtString("Mesic", mesic));
                                        }
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety") == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Add(new NbtList("Tickety", NbtTagType.Compound));

                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").RemoveAt(index);
                                    }
                                    else
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound((string)this.zakaznik.SelectedItem));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic) == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Add(new NbtCompound(form.vybranyMesic));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Add(new NbtString("Mesic", mesic));
                                        }
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety") == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Add(new NbtList("Tickety", NbtTagType.Compound));

                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>((string)this.zakaznik.SelectedItem).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(form.vybranyMesic).Get<NbtList>("Tickety").RemoveAt(index);
                                    }
                                }
                                lastSelected = form.posledniVybrany = ((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedIndices[0];
                                break;
                            }
                        }
                        lastSelected = form.posledniVybrany = ((Ticketník.CustomControls.ListView)tp.Controls[form.vybranyMesic]).SelectedIndices[0];
                        break;
                    }
                }

                if (ticket.CustomTerp != "" && ticket.CustomTask == "")
                {
                    canClose = false;
                    form.LoadFile();
                }

                if (canClose)
                {
                    form.uložitToolStripMenuItem_Click(sender, e);
                    form.toolStripButton2.Enabled = false;
                    form.toolStripButton3.Enabled = false;
                    form.LoadFile();
                }
            }

            if (ticket.StavT == Ticket.Stav.RDP)
            {
                if (DialogResult.Yes == MessageBox.Show(form.jazyk.Message_VytvoritUpozorneni, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    form.upozozrneniMuze = false;
                    Upozorneni upozorneni = new Upozorneni(form);
                    upozorneni.StartPosition = FormStartPosition.Manual;
                    upozorneni.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                    upozorneni.noveUpozorneni_OnDemand(form, upozorneni, DateTime.Now, form.jazyk.Windows_Upozorneni_RDP, ticket.ID + " " + ticket.Zakaznik + " - " + ticket.Popis);
                    upozorneni.Close();
                    form.upozozrneniMuze = true;
                }
            }

            if (ticket.CustomTerp != "" && ticket.CustomTask == "")
            {
                MessageBox.Show(form.jazyk.Message_TaskCannotBeEmpty);
                canClose = false;
            }

            if (canClose)
            {
                okClick = true;
                this.Close();
            }
        }

        private void pridat_Click(object sender, EventArgs e)
        {
            if(pauzaOd.Text != "")
            {
                string[] pod = pauzaOd.Text.Split(':');
                string[] pdo = pauzaDo.Text.Split(':');
                if (pdo.Length != 2)
                    pdo = new string[] { "0", "0" };

                ticket.PauzyOd.Add(new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, int.Parse(pod[0]), int.Parse(pod[1]), 0));
                ticket.PauzyDo.Add(new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, int.Parse(pdo[0]), int.Parse(pdo[1]), 0));
                ListViewItem lvi = new ListViewItem(new string[] {"", pauzaOd.Text, "-" , pauzaDo.Text});
                listView1.Items.Add(lvi);
                pauzaOd.Text = string.Empty;
                pauzaDo.Text = string.Empty;


                UpdatePauza();
            }
        }

        private void UpdatePauza()
        {
            int phod = 0, pmin = 0, index = 0;
            foreach (DateTime dt in ticket.PauzyOd)
            {
                DateTime tmp = new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, phod, pmin, 0).AddHours(ticket.PauzyDo[index].Hour - ticket.PauzyOd[index].Hour);
                tmp = tmp.AddMinutes(ticket.PauzyDo[index].Minute - ticket.PauzyOd[index].Minute);

                phod = tmp.Hour;
                pmin = tmp.Minute;
                index++;
            }
            celkemPauza.Text = phod + ":" + pmin.ToString("00");
            pauzaCelkem = new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, phod, pmin, 0);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count != 0)
            {
                pauzaOd.Text = listView1.SelectedItems[0].SubItems[1].Text;
                pauzaDo.Text = listView1.SelectedItems[0].SubItems[3].Text;
                upravit.Enabled = true;
                smazat.Enabled = true;
            }
            else
            {
                upravit.Enabled = false;
                smazat.Enabled = false;
            }
        }

        private void upravit_Click(object sender, EventArgs e)
        {
            if (pauzaOd.Text != "")
            {
                string[] pod = pauzaOd.Text.Split(':');
                string[] pdo = pauzaDo.Text.Split(':');
                int index = listView1.SelectedIndices[0];
                if (pdo.Length != 2)
                    pdo = new string[] { "0", "0" };

                ticket.PauzyOd[index] = new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, int.Parse(pod[0]), int.Parse(pod[1]), 0);
                ticket.PauzyDo[index] = new DateTime(ticket.Datum.Year, ticket.Datum.Month, ticket.Datum.Day, int.Parse(pdo[0]), int.Parse(pdo[1]), 0);
                ListViewItem lvi = new ListViewItem(new string[] { "", pauzaOd.Text, "-", pauzaDo.Text });
                listView1.Items[index] = lvi;
                pauzaOd.Text = string.Empty;
                pauzaDo.Text = string.Empty;

                UpdatePauza();
            }
        }

        private void smazat_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                int index = listView1.SelectedIndices[0];
                ticket.PauzyOd.RemoveAt(index);
                ticket.PauzyDo.RemoveAt(index);
                listView1.Items.RemoveAt(index);
                pauzaOd.Text = string.Empty;
                pauzaDo.Text = string.Empty;

                UpdatePauza();
            }
        }

        

        private void UpdateDoba()
        {
            UpdatePauza();
            try
            {
                string[] casOd = zacatek.Text.Split(':');
                string[] casDo;
                if (stavTicketu.Text != form.jazyk.Status_Vyreseno)
                    casDo = konec.Text.Split(':');
                else
                {
                    if (konec.Text == "" || konec.Text == zacatek.Text)
                        casDo = new String[] { casOd[0], (int.Parse(casOd[1]) + 1).ToString() };
                    else
                        casDo = konec.Text.Split(':');
                }

                DateTime odpracovanoCiste = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(casDo[0]) - int.Parse(casOd[0]), 0, 0);
                int min = int.Parse(casDo[1]) - int.Parse(casOd[1]);
                odpracovanoCiste = odpracovanoCiste.AddMinutes(min);
                odpracovanoCiste = odpracovanoCiste.AddHours(-pauzaCelkem.Hour);
                odpracovanoCiste = odpracovanoCiste.AddMinutes(-pauzaCelkem.Minute);

                DateTime odpracovanoEdit = form.RoundUp(odpracovanoCiste, TimeSpan.FromMinutes(30));

                cistehocasu.Text = odpracovanoCiste.ToString("H:mm");
                odpracovano.Text = odpracovanoEdit.ToString("H:mm");

            }
            catch
            {
                cistehocasu.Text = "0:00";
                odpracovano.Text = "0:00";

            }
        }

        private void zacatek_TextChanged(object sender, EventArgs e)
        {
            UpdateDoba();
        }

        private void cas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.shortTime)
            {
                string[] casOd = zacatek.Text.Split(':');

                int hodiny = (cas.SelectedIndex + 1) / 2;
                int minuty = (cas.SelectedIndex % 2 == 0) ? 30 : 0;
                DateTime dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(casOd[0]), int.Parse(casOd[1]), 0);
                dt = dt.AddHours(hodiny);
                dt = dt.AddMinutes(minuty);
                konec.Text = dt.ToString("H:mm");
                UpdateDoba();
            }
        }

        private void TicketWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            else if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Shift)
                if(ok.Enabled)
                    ok_Click(null, null);
        }

        private void NovyTerp(string terp)
        {
            if (terp != null && terp != "")
            {
                bool nalezeno = false;
                if (Zakaznici.Terpy != null)
                {
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").Count != 0)
                    {
                        foreach (NbtString terpy in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
                        {
                            if (terpy.Value == terp.Split(new[] { ' ' }, 2)[0])
                            {
                                nalezeno = true;
                                break;
                            }
                        }
                    }

                    if (!nalezeno)
                    {
                        string[] s = new string[2] { null, null };
                        terp.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                        form.list.AddTerp(s[0], false);
                        if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") == null)
                            Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TerpPopis"));
                        if (s[1] == null)
                            s[1] = "";
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Add(new NbtString(s[0], s[1]));
                    }
                }
                else
                {
                    string[] s = new string[2] { null, null };
                    terp.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                    form.list.AddTerp(s[0], false);
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") == null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TerpPopis"));
                    if (s[1] == null)
                        s[1] = "";
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Add(new NbtString(s[0], s[1]));
                }
            }
        }

        private void NovyTask(string task)
        {
            if (task != null && task != "")
            {
                bool nalezeno = false;
                if (Zakaznici.Terpy != null)
                {
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").Count != 0)
                    {
                        foreach (NbtString terpy in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
                        {
                            if (terpy.Value == task.Split(new[] { ' ' }, 2)[0])
                            {
                                nalezeno = true;
                                break;
                            }
                        }
                    }

                    if (!nalezeno)
                    {
                        string[] s = new string[2] { null, null };
                        task.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                        form.list.AddTask(s[0], false);
                        if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") == null)
                            Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TaskPopis"));
                        if (s[1] == null)
                            s[1] = "";
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Add(new NbtString(s[0], s[1]));
                    }
                }
                else
                {
                    string[] s = new string[2] { null, null };
                    task.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                    form.list.AddTask(s[0], false);
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") == null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TaskPopis"));
                    if (s[1] == null)
                        s[1] = "";
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Add(new NbtString(s[0], s[1]));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TERP terp = new TERP(form, 3, ticket.CustomTerp, ticket.CustomTask);
            terp.Text = form.jazyk.Windows_Ticket_TicketTerp;
            terp.StartPosition = FormStartPosition.Manual;
            terp.Size = new System.Drawing.Size(300, 117);
            terp.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            if(terp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if ((terp.terpListTic.Text != "" && terp.taskListTic.Text != "") && terp.terpListTic.Text != null && terp.taskListTic.Text != null)
                {
                    task = ticket.CustomTask = terp.taskListTic.Text.Split(new[] { ' ' }, 2)[0];
                    terpt = ticket.CustomTerp = terp.terpListTic.Text.Split(new[] { ' ' }, 2)[0];

                    NovyTerp(terp.terpListTic.Text);
                    NovyTask(terp.taskListTic.Text);



                    if (normalni.Checked)
                        ticket.TypPrace = (byte)Ticket.TypTicketu.Custom;
                    else if (prescas.Checked)
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomPrescas;
                    else if (nahradni.Checked)
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomNahradni;
                    else if (volno.Checked)
                        ticket.TypPrace = (byte)Ticket.TypTicketu.CustomOPrazdniny;
                }
                else
                {

                    if (terp.taskListTic.Text == null || terp.taskListTic.Text == "")
                        ticket.CustomTask = "";
                    else
                    {
                        task = ticket.CustomTask = terp.taskListTic.Text.Split(new[] { ' ' }, 2)[0];

                        NovyTask(terp.taskListTic.Text);

                        if (normalni.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.Custom;
                        else if (prescas.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomPrescas;
                        else if (nahradni.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomNahradni;
                        else if (volno.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomOPrazdniny;

                    }

                    if (terp.terpListTic.Text == null || terp.terpListTic.Text == "")
                        ticket.CustomTerp = "";
                    else
                    {
                        terpt = ticket.CustomTerp = terp.terpListTic.Text.Split(new[] { ' ' }, 2)[0];

                        NovyTerp(terp.terpListTic.Text);
                        
                        if (normalni.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.Custom;
                        else if (prescas.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomPrescas;
                        else if (nahradni.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomNahradni;
                        else if (volno.Checked)
                            ticket.TypPrace = (byte)Ticket.TypTicketu.CustomOPrazdniny;

                    }
                }


                terp.Close();
                terpKod.Text = DejTerp();
            }
        }

        private void datum_ValueChanged(object sender, EventArgs e)
        {
            if (!dat)
            {
                dat = true;
                string mesic = "";
                switch (ticket.Mesic)
                {
                    case "Leden":
                        mesic = "leden";
                        break;
                    case "Únor":
                        mesic = "unor";
                        break;
                    case "Březen":
                        mesic = "brezen";
                        break;
                    case "Duben":
                        mesic = "duben";
                        break;
                    case "Květen":
                        mesic = "kveten";
                        break;
                    case "Červen":
                        mesic = "cerven";
                        break;
                    case "Červenec":
                        mesic = "cervenec";
                        break;
                    case "Srpen":
                        mesic = "srpen";
                        break;
                    case "Září":
                        mesic = "zari";
                        break;
                    case "Říjen":
                        mesic = "rijen";
                        break;
                    case "Listopad":
                        mesic = "listopad";
                        break;
                    case "Prosinec":
                        mesic = "prosinec";
                        break;
                }


                if (DialogResult.Yes == MessageBox.Show(form.jazyk.Windows_Ticket_ZmenaData, form.jazyk.Windows_Ticket_ZmenaDataOkno, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ticket.Datum = datum.Value;

                    switch(datum.Value.Month)
                    {
                        case 1:
                            if(ticket.Mesic != "Leden")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Leden";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("leden"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Add(new NbtString("Mesic", "Leden"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Leden";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("leden"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Add(new NbtString("Mesic", "Leden"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("leden").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (ticket.Mesic != "Únor")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Únor";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("unor"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Add(new NbtString("Mesic", "Únor"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Únor";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("unor"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Add(new NbtString("Mesic", "Únor"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("unor").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 3:
                            if (ticket.Mesic != "Březen")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Březen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("brezen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Add(new NbtString("Mesic", "Březen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Březen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("brezen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Add(new NbtString("Mesic", "Březen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("brezen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (ticket.Mesic != "Duben")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Duben";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("duben"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Add(new NbtString("Mesic", "Duben"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Duben";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("duben"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Add(new NbtString("Mesic", "Duben"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("duben").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 5:
                            if (ticket.Mesic != "Květen")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Květen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("kveten"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Add(new NbtString("Mesic", "Květen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Květen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("kveten"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Add(new NbtString("Mesic", "Květen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("kveten").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 6:
                            if (ticket.Mesic != "Červen")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Červen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("cerven"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Add(new NbtString("Mesic", "Červen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Červen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("cerven"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Add(new NbtString("Mesic", "Červen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cerven").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 7:
                            if (ticket.Mesic != "Červenec")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Červenec";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("cervenec"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Add(new NbtString("Mesic", "Červenec"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Červenec";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("cervenec"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Add(new NbtString("Mesic", "Červenec"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("cervenec").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 8:
                            if (ticket.Mesic != "Srpen")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Srpen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("srpen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Add(new NbtString("Mesic", "Srpen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Srpen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("srpen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Add(new NbtString("Mesic", "Srpen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("srpen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 9:
                            if (ticket.Mesic != "Září")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Září";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("zari"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Add(new NbtString("Mesic", "Září"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Září";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("zari"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Add(new NbtString("Mesic", "Září"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("zari").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 10:
                            if (ticket.Mesic != "Říjen")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Říjen"; 
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("rijen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Add(new NbtString("Mesic", "Říjen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Říjen";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("rijen"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Add(new NbtString("Mesic", "Říjen"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("rijen").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 11:
                            if (ticket.Mesic != "Listopad")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }
                                    //přidat do nového
                                    ticket.Mesic = "Listopad";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("listopad"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Add(new NbtString("Mesic", "Listopad"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }

                                    //přidat do nového
                                    ticket.Mesic = "Listopad";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("listopad"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Add(new NbtString("Mesic", "Listopad"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("listopad").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                        case 12:
                            if (ticket.Mesic != "Prosinec")
                            {
                                //najít a odstranit
                                int poc = 0;
                                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }

                                    //přidat do nového
                                    ticket.Mesic = "Prosinec";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("prosinec"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Add(new NbtString("Mesic", "Prosinec"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                                else
                                {
                                    if (ticket.IDtick != -1)
                                    {
                                        foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety"))
                                        {
                                            if (c.Get<NbtLong>("IDlong").Value == ticket.IDtick)
                                            {
                                                break;
                                            }
                                            poc++;
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(form.rokVyber.SelectedItem.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").RemoveAt(poc);
                                    }

                                    //přidat do nového
                                    ticket.Mesic = "Prosinec";
                                    if (ticket.IDtick != -1)
                                    {
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Datum.Year.ToString()));
                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));

                                        if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec") == null)
                                        {
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound("prosinec"));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Add(new NbtList("Tickety", NbtTagType.Compound));
                                            form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Add(new NbtString("Mesic", "Prosinec"));
                                        }
                                        form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Datum.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>("prosinec").Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                    datum.Value = ticket.Datum;
                dat = false;
            }
        }

        DateTime puvodni;

        private void datum_DropDown(object sender, EventArgs e)
        {
            dat = true;
            puvodni = ticket.Datum;
        }

        private void datum_CloseUp(object sender, EventArgs e)
        {
            dat = false;
            if(puvodni != datum.Value)
                datum_ValueChanged(sender, e);
        }

        private void datum_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void onlineTerpDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            onlineTaskComboBox.Items.Clear();
            onlineTaskComboBox.Text = "";
            onlineTypeComboBox.Items.Clear();
            onlineTypeComboBox.Text = "";
            ok.Enabled = false;
            foreach (MyTimeTask mtt in form.Terpy[(string)onlineTerpDropDown.SelectedItem].Tasks)
            {
                if(mtt.LastUpdate == form.TerpFileUpdate && !_search)
                    onlineTaskComboBox.Items.Add(mtt.Label);
                else if (_search)
                    onlineTaskComboBox.Items.Add(mtt.Label);
            }
            onlineTaskComboBox.DropDownWidth = ComboWidth(onlineTaskComboBox);
            onlineTaskComboBox.Sorted = true;
        }

        private void onlineTaskComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            onlineTypeComboBox.Items.Clear();
            onlineTypeComboBox.Text = "";
            ok.Enabled = false;
            foreach (MyTimeTask mtt in form.Terpy[(string)onlineTerpDropDown.SelectedItem].Tasks)
            {
                if(mtt.Label == (string)onlineTaskComboBox.SelectedItem)
                {
                    foreach(string mtty in mtt.TypeLabels)
                    {
                        onlineTypeComboBox.Items.Add(mtty);
                    }
                    break;
                }
            }
            onlineTypeComboBox.DropDownWidth = ComboWidth(onlineTypeComboBox);
            onlineTypeComboBox.Sorted = true;

            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                foreach (string s in onlineTypeComboBox.Items)
                {
                    if (s.ToLower().StartsWith("normal "))
                    {
                        onlineTypeComboBox.SelectedItem = s;
                        break;
                    }
                }
            }

        }

        private void onlineTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((string)onlineTypeComboBox.SelectedItem != string.Empty)
                ok.Enabled = true;
        }

        private void onlineTerpDropDown_TextChanged(object sender, EventArgs e)
        {
            if (onlineTerpDropDown.Text != "")
            {
                if (!onlineTerpDropDown.Items.Contains(onlineTerpDropDown.Text))
                {
                    btn_TicketWindow_SearchTerp.Enabled = true;
                    btn_TicketWindow_UpdateSelected.Enabled = false;
                }
                else
                {
                    btn_TicketWindow_SearchTerp.Enabled = false;
                    btn_TicketWindow_UpdateSelected.Enabled = true;
                }
            }
            else
            {
                btn_TicketWindow_SearchTerp.Enabled = false;
                btn_TicketWindow_UpdateSelected.Enabled = false;
            }
        }

        private void btn_TicketWindow_SearchTerp_Click(object sender, EventArgs e)
        {
            form.terpTaskFileLock = false;
            form.UpdateTerpTaskFile(onlineTerpDropDown.Text);
            string tmpSelected = onlineTerpDropDown.Text;
            onlineTerpDropDown.Items.Clear();
            onlineTerpDropDown.Text = "";
            foreach (MyTimeTerp onlineTerpy in form.Terpy.Values)
            {
                if(onlineTerpy.LastUpdate == form.TerpFileUpdate && !_search)
                    onlineTerpDropDown.Items.Add(onlineTerpy.Label);
                else if (_search)
                    onlineTerpDropDown.Items.Add(onlineTerpy.Label);
            }
            onlineTerpDropDown.DropDownWidth = ComboWidth(onlineTerpDropDown);
            onlineTerpDropDown.Sorted = true;

            foreach (string s in onlineTerpDropDown.Items)
            {
                if(s.StartsWith(tmpSelected + " "))
                {
                    tmpSelected = s;
                    break;
                }
            }

            onlineTerpDropDown.SelectedItem = tmpSelected;
        }

        private void btn_TicketWindow_UpdateSelected_Click(object sender, EventArgs e)
        {
            form.terpTaskFileLock = false;
            form.UpdateSelected(form.Terpy[onlineTerpDropDown.Text].Number);
            string tmpSelected = onlineTerpDropDown.Text; 
            onlineTerpDropDown.Items.Clear();
            onlineTerpDropDown.Text = "";
            foreach (MyTimeTerp onlineTerpy in form.Terpy.Values)
            {
                if (onlineTerpy.LastUpdate == form.TerpFileUpdate && !_search)
                    onlineTerpDropDown.Items.Add(onlineTerpy.Label);
                else if (_search)
                    onlineTerpDropDown.Items.Add(onlineTerpy.Label);
            }
            onlineTerpDropDown.DropDownWidth = ComboWidth(onlineTerpDropDown);
            onlineTerpDropDown.Sorted = true;
            onlineTerpDropDown.SelectedItem = tmpSelected;
        }

        private void search_btn_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex(@"\d+$");
            if (idTicketu.Text.Length != 0)
            {
                Match m = reg.Match(idTicketu.Text);

                System.Diagnostics.Process.Start("https://tieto.service-now.com/textsearch.do?sysparm_search=" + idTicketu.Text);
            }
        }

        private void TicketWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!okClick)
                ticket.ID = puvodniID;
        }

        private int ComboWidth(ComboBox cb)
        {
            if (cb != null)
            {
                ComboBox senderComboBox = (ComboBox)cb;
                int width = senderComboBox.DropDownWidth;
                Graphics g = senderComboBox.CreateGraphics();
                Font font = senderComboBox.Font;
                int vertScrollBarWidth =
                    (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

                int newWidth;
                foreach (string s in ((ComboBox)cb).Items)
                {
                    newWidth = (int)g.MeasureString(s, font).Width
                        + vertScrollBarWidth;
                    if (width < newWidth)
                    {
                        width = newWidth;
                    }
                }
                return width;
            }
            return 60;
        }

        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            Motiv.SetGroupBoxRamecek((GroupBox)sender, e);
        }

        private void comboBox_MouseEnter(object sender, EventArgs e)
        {
            Motiv.SetControlColorOver(sender);
        }

        private void event_MouseLeave(object sender, EventArgs e)
        {
            Motiv.SetControlColor(sender);
        }

        private void event_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).FlatAppearance.BorderColor = Color.DodgerBlue;
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
