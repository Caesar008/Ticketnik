using System;
using System.Drawing;
using System.Windows.Forms;
using fNbt;
using System.Text.RegularExpressions;

namespace Ticketník
{
    public partial class Search : Form
    {
        Form1 form;
        int vybranyIndex = 0;
        string fmPuvodni = "";
        int fiPuvodni = 0;
        bool pouzitRegex = false;

        public Search(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            fmPuvodni = form.vybranyMesic;
            fiPuvodni = form.posledniVybrany;
            groupBox1.Text = form.jazyk.Windows_Search_KriteriaHledani;
            label1.Text = form.jazyk.Windows_Search_PC;
            label2.Text = form.jazyk.Windows_Search_IDTicketu;
            label3.Text = form.jazyk.Windows_Search_Zakaznik;
            label4.Text = form.jazyk.Windows_Search_Popis;
            label5.Text = form.jazyk.Windows_Search_Kontakt;
            label7.Text = form.jazyk.Windows_Search_Poznamka;
            checkBox1.Text = form.jazyk.Windows_Search_Regex;
            button1.Text = form.jazyk.Windows_Search_Hledej;
            richTextBox1.Text = form.jazyk.Windows_Search_HledaniNapoveda;
            this.Text = form.jazyk.Windows_Search_Hledej;
            listView1.Columns[1].Text = form.jazyk.Windows_Search_PC;
            listView1.Columns[2].Text = form.jazyk.Windows_Search_IDTicketu;
            listView1.Columns[3].Text = form.jazyk.Windows_Search_Zakaznik;
            listView1.Columns[4].Text = form.jazyk.Windows_Search_Popis;
            listView1.Columns[5].Text = form.jazyk.Windows_Search_Kontakt;
            listView1.Columns[6].Text = form.jazyk.Windows_Search_Datum;
            listView1.Columns[7].Text = form.jazyk.Windows_Search_Pauzy;
            listView1.Columns[8].Text = form.jazyk.Windows_Search_Status;
            listView1.Columns[9].Text = form.jazyk.Windows_Search_Poznamka;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                if (pc.Text == "*")
                    pc.Text = "";
                if (zakaznik.Text == "*")
                    zakaznik.Text = "";
                if (popis.Text == "*")
                    popis.Text = "";
                if (poznamka.Text == "*")
                    poznamka.Text = "";
                if (id.Text == "*")
                    id.Text = "";
                if (kontakt.Text == "*")
                    kontakt.Text = "";
            }

            pouzitRegex = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();

            if (!pouzitRegex)
            {
                //hledání
                if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                {
                    foreach (NbtTag c in form.file.RootTag.Get<NbtCompound>("Zakaznici"))
                    {
                        if (c.TagType != NbtTagType.Compound)
                            continue;
                        if (c.Name.ToLower().Contains(zakaznik.Text.ToLower()))
                        {
                            foreach (NbtCompound cc in (NbtCompound)c)
                            {
                                int mesic = 0;
                                switch (cc.Name)
                                {
                                    case "leden":
                                        mesic = 1;
                                        break;
                                    case "unor":
                                        mesic = 2;
                                        break;
                                    case "brezen":
                                        mesic = 3;
                                        break;
                                    case "duben":
                                        mesic = 4;
                                        break;
                                    case "kveten":
                                        mesic = 5;
                                        break;
                                    case "cerven":
                                        mesic = 6;
                                        break;
                                    case "cervenec":
                                        mesic = 7;
                                        break;
                                    case "srpen":
                                        mesic = 8;
                                        break;
                                    case "zari":
                                        mesic = 9;
                                        break;
                                    case "rijen":
                                        mesic = 10;
                                        break;
                                    case "listopad":
                                        mesic = 11;
                                        break;
                                    case "prosinec":
                                        mesic = 12;
                                        break;

                                }
                                foreach (NbtCompound ti in cc.Get<NbtList>("Tickety"))
                                {
                                    if (ti.Get<NbtString>("ID").Value.ToLower().Contains(id.Text.ToLower()) &&
                                        ti.Get<NbtString>("Kontakt").Value.ToLower().Contains(kontakt.Text.ToLower()) &&
                                        ti.Get<NbtString>("PC").Value.ToLower().Contains(pc.Text.ToLower()) &&
                                        ti.Get<NbtString>("Popis").Value.ToLower().Contains(popis.Text.ToLower()) &&
                                        ti.Get<NbtString>("Poznamky").Value.ToLower().Contains(poznamka.Text.ToLower())
                                        )
                                    {
                                        string stst = "";
                                        Color barvaP;

                                        switch (ti.Get<NbtByte>("Stav").Value)
                                        {
                                            case (byte)Ticket.Stav.Probiha:
                                                stst = form.jazyk.Windows_Search_Probiha;
                                                barvaP = Properties.Settings.Default.probiha;
                                                break;
                                            case (byte)Ticket.Stav.Ceka_se:
                                                stst = form.jazyk.Windows_Search_CekaSe;
                                                barvaP = Properties.Settings.Default.ceka;
                                                break;
                                            case (byte)Ticket.Stav.RDP:
                                                stst = form.jazyk.Windows_Search_RDP;
                                                barvaP = Properties.Settings.Default.rdp;
                                                break;
                                            case (byte)Ticket.Stav.Ceka_se_na_odpoved:
                                                stst = form.jazyk.Windows_Search_CekaSeNa;
                                                barvaP = Properties.Settings.Default.odpoved;
                                                break;
                                            case (byte)Ticket.Stav.Vyreseno:
                                                stst = form.jazyk.Windows_Search_Hotovo;
                                                if (ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.Prescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.CustomPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripcePrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityPrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                    barvaP = Properties.Settings.Default.prescas;
                                                else
                                                    barvaP = Properties.Settings.Default.vyreseno;
                                                break;
                                            default:
                                                stst = form.jazyk.Windows_Search_Probiha;
                                                barvaP = Properties.Settings.Default.probiha;
                                                break;
                                        }
                                        string casDoString = "";
                                        if ((new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm") != "0:00")
                                            casDoString = (new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm");

                                        string terp = "";
                                        if (ti["Terp"] != null && ti["Terp"].StringValue != "")
                                            terp = ti["Terp"].StringValue;
                                        else
                                            terp = Zakaznici.GetTerp(c.Name);

                                        if (ti["Task"] != null && ti["Task"].StringValue != "")
                                            terp += " | " + ti["Task"].StringValue;
                                        else
                                            terp += " | " + DejTask(ti["ID"].StringValue);

                                        ListViewItem lvi = new ListViewItem(new string[] { "", ti.Get<NbtString>("PC").Value, ti.Get<NbtString>("ID").Value, c.Name, ti.Get<NbtString>("Popis").Value, ti.Get<NbtString>("Kontakt").Value, new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value).ToString("dd.M.yyyy"), terp, stst, ti.Get<NbtString>("Poznamky").Value });
                                        lvi.BackColor = barvaP;
                                        lvi.ForeColor = form.ContrastColor(barvaP);
                                        lvi.Tag = new Tag(new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value), ti.Get<NbtLong>("IDlong").Value);
                                        listView1.Items.Add(lvi);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NbtCompound rok in form.file.RootTag.Get<NbtCompound>("Zakaznici"))
                    {
                        foreach (NbtTag c in rok)
                        {
                            if (c.TagType != NbtTagType.Compound)
                                continue;
                            if (c.Name.ToLower().Contains(zakaznik.Text.ToLower()))
                            {
                                foreach (NbtCompound cc in (NbtCompound)c)
                                {
                                    int mesic = 0;
                                    switch (cc.Name)
                                    {
                                        case "leden":
                                            mesic = 1;
                                            break;
                                        case "unor":
                                            mesic = 2;
                                            break;
                                        case "brezen":
                                            mesic = 3;
                                            break;
                                        case "duben":
                                            mesic = 4;
                                            break;
                                        case "kveten":
                                            mesic = 5;
                                            break;
                                        case "cerven":
                                            mesic = 6;
                                            break;
                                        case "cervenec":
                                            mesic = 7;
                                            break;
                                        case "srpen":
                                            mesic = 8;
                                            break;
                                        case "zari":
                                            mesic = 9;
                                            break;
                                        case "rijen":
                                            mesic = 10;
                                            break;
                                        case "listopad":
                                            mesic = 11;
                                            break;
                                        case "prosinec":
                                            mesic = 12;
                                            break;

                                    }
                                    foreach (NbtCompound ti in cc.Get<NbtList>("Tickety"))
                                    {
                                        if (ti.Get<NbtString>("ID").Value.ToLower().Contains(id.Text.ToLower()) &&
                                            ti.Get<NbtString>("Kontakt").Value.ToLower().Contains(kontakt.Text.ToLower()) &&
                                            ti.Get<NbtString>("PC").Value.ToLower().Contains(pc.Text.ToLower()) &&
                                            ti.Get<NbtString>("Popis").Value.ToLower().Contains(popis.Text.ToLower()) &&
                                            ti.Get<NbtString>("Poznamky").Value.ToLower().Contains(poznamka.Text.ToLower())
                                            )
                                        {
                                            string stst = "";
                                            Color barvaP;

                                            switch (ti.Get<NbtByte>("Stav").Value)
                                            {
                                                case (byte)Ticket.Stav.Probiha:
                                                    stst = form.jazyk.Windows_Search_Probiha;
                                                    barvaP = Properties.Settings.Default.probiha;
                                                    break;
                                                case (byte)Ticket.Stav.Ceka_se:
                                                    stst = form.jazyk.Windows_Search_CekaSe;
                                                    barvaP = Properties.Settings.Default.ceka;
                                                    break;
                                                case (byte)Ticket.Stav.RDP:
                                                    stst = form.jazyk.Windows_Search_RDP;
                                                    barvaP = Properties.Settings.Default.rdp;
                                                    break;
                                                case (byte)Ticket.Stav.Ceka_se_na_odpoved:
                                                    stst = form.jazyk.Windows_Search_CekaSeNa;
                                                    barvaP = Properties.Settings.Default.odpoved;
                                                    break;
                                                case (byte)Ticket.Stav.Vyreseno:
                                                    stst = form.jazyk.Windows_Search_Hotovo;
                                                    if (ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.Prescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.CustomPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripcePrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityPrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                        barvaP = Properties.Settings.Default.prescas;
                                                    else
                                                        barvaP = Properties.Settings.Default.vyreseno;
                                                    break;
                                                default:
                                                    stst = form.jazyk.Windows_Search_Probiha;
                                                    barvaP = Properties.Settings.Default.probiha;
                                                    break;
                                            }
                                            string casDoString = "";
                                            if ((new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm") != "0:00")
                                                casDoString = (new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm");
                                            string terp = "";
                                            if (ti["Terp"] != null && ti["Terp"].StringValue != "")
                                                terp = ti["Terp"].StringValue;
                                            else
                                                terp = Zakaznici.GetTerp(c.Name);

                                            if (ti["Task"] != null && ti["Task"].StringValue != "")
                                                terp += " | " + ti["Task"].StringValue;
                                            else
                                                terp += " | " + DejTask(ti["ID"].StringValue);

                                            ListViewItem lvi = new ListViewItem(new string[] { "", ti.Get<NbtString>("PC").Value, ti.Get<NbtString>("ID").Value, c.Name, ti.Get<NbtString>("Popis").Value, ti.Get<NbtString>("Kontakt").Value, new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value).ToString("dd.M.yyyy"), terp, stst, ti.Get<NbtString>("Poznamky").Value });
                                            lvi.BackColor = barvaP;
                                            lvi.ForeColor = form.ContrastColor(barvaP);
                                            lvi.Tag = new Tag(new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value), ti.Get<NbtLong>("IDlong").Value);
                                            listView1.Items.Add(lvi);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    //regexy tu
                    Regex regexPC = new Regex(pc.Text, RegexOptions.IgnoreCase);
                    Regex regexID = new Regex(id.Text, RegexOptions.IgnoreCase);
                    Regex regexZakaznik = new Regex(zakaznik.Text, RegexOptions.IgnoreCase);
                    Regex regexKontakt = new Regex(kontakt.Text, RegexOptions.IgnoreCase);
                    Regex regexPopis = new Regex(popis.Text, RegexOptions.IgnoreCase);
                    Regex regexPoznamka = new Regex(poznamka.Text, (RegexOptions.IgnoreCase | RegexOptions.Multiline));

                    if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                    {
                        foreach (NbtTag c in form.file.RootTag.Get<NbtCompound>("Zakaznici"))
                        {
                            if (c.TagType != NbtTagType.Compound)
                                continue;

                            Match matchZakaznik = regexZakaznik.Match(c.Name);

                            if (matchZakaznik.Success)
                            {
                                foreach (NbtCompound cc in (NbtCompound)c)
                                {
                                    int mesic = 0;
                                    switch (cc.Name)
                                    {
                                        case "leden":
                                            mesic = 1;
                                            break;
                                        case "unor":
                                            mesic = 2;
                                            break;
                                        case "brezen":
                                            mesic = 3;
                                            break;
                                        case "duben":
                                            mesic = 4;
                                            break;
                                        case "kveten":
                                            mesic = 5;
                                            break;
                                        case "cerven":
                                            mesic = 6;
                                            break;
                                        case "cervenec":
                                            mesic = 7;
                                            break;
                                        case "srpen":
                                            mesic = 8;
                                            break;
                                        case "zari":
                                            mesic = 9;
                                            break;
                                        case "rijen":
                                            mesic = 10;
                                            break;
                                        case "listopad":
                                            mesic = 11;
                                            break;
                                        case "prosinec":
                                            mesic = 12;
                                            break;

                                    }
                                    foreach (NbtCompound ti in cc.Get<NbtList>("Tickety"))
                                    {
                                        Match matchPC = regexPC.Match(ti.Get<NbtString>("PC").Value);
                                        Match matchKontakt = regexKontakt.Match(ti.Get<NbtString>("Kontakt").Value);
                                        Match matchID = regexID.Match(ti.Get<NbtString>("ID").Value);
                                        Match matchPopis = regexPopis.Match(ti.Get<NbtString>("Popis").Value);
                                        Match matchPoznamka = regexPoznamka.Match(ti.Get<NbtString>("Poznamky").Value);

                                        if (matchPC.Success && matchKontakt.Success && matchID.Success && matchPopis.Success && matchPoznamka.Success)
                                        {
                                            string stst = "";
                                            Color barvaP;

                                            switch (ti.Get<NbtByte>("Stav").Value)
                                            {
                                                case (byte)Ticket.Stav.Probiha:
                                                    stst = form.jazyk.Windows_Search_Probiha;
                                                    barvaP = Properties.Settings.Default.probiha;
                                                    break;
                                                case (byte)Ticket.Stav.Ceka_se:
                                                    stst = form.jazyk.Windows_Search_CekaSe;
                                                    barvaP = Properties.Settings.Default.ceka;
                                                    break;
                                                case (byte)Ticket.Stav.RDP:
                                                    stst = form.jazyk.Windows_Search_RDP; ;
                                                    barvaP = Properties.Settings.Default.rdp;
                                                    break;
                                                case (byte)Ticket.Stav.Ceka_se_na_odpoved:
                                                    stst = form.jazyk.Windows_Search_CekaSeNa;
                                                    barvaP = Properties.Settings.Default.odpoved;
                                                    break;
                                                case (byte)Ticket.Stav.Vyreseno:
                                                    stst = form.jazyk.Windows_Search_Hotovo;
                                                    if (ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.Prescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.CustomPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripcePrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityPrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                        barvaP = Properties.Settings.Default.prescas;
                                                    else
                                                        barvaP = Properties.Settings.Default.vyreseno;
                                                    break;
                                                default:
                                                    stst = form.jazyk.Windows_Search_Probiha;
                                                    barvaP = Properties.Settings.Default.probiha;
                                                    break;
                                            }
                                            string casDoString = "";
                                            if ((new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm") != "0:00")
                                                casDoString = (new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm");
                                            string terp = "";
                                            if (ti["Terp"] != null && ti["Terp"].StringValue != "")
                                                terp = ti["Terp"].StringValue;
                                            else
                                                terp = Zakaznici.GetTerp(c.Name);

                                            if (ti["Task"] != null && ti["Task"].StringValue != "")
                                                terp += " | " + ti["Task"].StringValue;
                                            else
                                                terp += " | " + DejTask(ti["ID"].StringValue);
                                            ListViewItem lvi = new ListViewItem(new string[] { "", ti.Get<NbtString>("PC").Value, ti.Get<NbtString>("ID").Value, c.Name, ti.Get<NbtString>("Popis").Value, ti.Get<NbtString>("Kontakt").Value, new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value).ToString("dd.M.yyyy"), terp, stst, ti.Get<NbtString>("Poznamky").Value });
                                            lvi.BackColor = barvaP;
                                            lvi.ForeColor = form.ContrastColor(barvaP);
                                            lvi.Tag = new Tag(new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value), ti.Get<NbtLong>("IDlong").Value);
                                            listView1.Items.Add(lvi);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (NbtCompound rok in form.file.RootTag.Get<NbtCompound>("Zakaznici"))
                        {
                            foreach (NbtTag c in rok)
                            {
                                if (c.TagType != NbtTagType.Compound)
                                    continue;

                                Match matchZakaznik = regexZakaznik.Match(c.Name);

                                if (matchZakaznik.Success)
                                {
                                    foreach (NbtCompound cc in (NbtCompound)c)
                                    {
                                        int mesic = 0;
                                        switch (cc.Name)
                                        {
                                            case "leden":
                                                mesic = 1;
                                                break;
                                            case "unor":
                                                mesic = 2;
                                                break;
                                            case "brezen":
                                                mesic = 3;
                                                break;
                                            case "duben":
                                                mesic = 4;
                                                break;
                                            case "kveten":
                                                mesic = 5;
                                                break;
                                            case "cerven":
                                                mesic = 6;
                                                break;
                                            case "cervenec":
                                                mesic = 7;
                                                break;
                                            case "srpen":
                                                mesic = 8;
                                                break;
                                            case "zari":
                                                mesic = 9;
                                                break;
                                            case "rijen":
                                                mesic = 10;
                                                break;
                                            case "listopad":
                                                mesic = 11;
                                                break;
                                            case "prosinec":
                                                mesic = 12;
                                                break;

                                        }
                                        foreach (NbtCompound ti in cc.Get<NbtList>("Tickety"))
                                        {
                                            Match matchPC = regexPC.Match(ti.Get<NbtString>("PC").Value);
                                            Match matchKontakt = regexKontakt.Match(ti.Get<NbtString>("Kontakt").Value);
                                            Match matchID = regexID.Match(ti.Get<NbtString>("ID").Value);
                                            Match matchPopis = regexPopis.Match(ti.Get<NbtString>("Popis").Value);
                                            Match matchPoznamka = regexPoznamka.Match(ti.Get<NbtString>("Poznamky").Value);

                                            if (matchPC.Success && matchKontakt.Success && matchID.Success && matchPopis.Success && matchPoznamka.Success)
                                            {
                                                string stst = "";
                                                Color barvaP;

                                                switch (ti.Get<NbtByte>("Stav").Value)
                                                {
                                                    case (byte)Ticket.Stav.Probiha:
                                                        stst = form.jazyk.Windows_Search_Probiha;
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                    case (byte)Ticket.Stav.Ceka_se:
                                                        stst = form.jazyk.Windows_Search_CekaSe;
                                                        barvaP = Properties.Settings.Default.ceka;
                                                        break;
                                                    case (byte)Ticket.Stav.RDP:
                                                        stst = form.jazyk.Windows_Search_RDP; ;
                                                        barvaP = Properties.Settings.Default.rdp;
                                                        break;
                                                    case (byte)Ticket.Stav.Ceka_se_na_odpoved:
                                                        stst = form.jazyk.Windows_Search_CekaSeNa;
                                                        barvaP = Properties.Settings.Default.odpoved;
                                                        break;
                                                    case (byte)Ticket.Stav.Vyreseno:
                                                        stst = form.jazyk.Windows_Search_Hotovo;
                                                        if (ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.Prescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.CustomPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripcePrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityPrescas || ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                        ti.Get<NbtByte>("Prace").Value == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                            barvaP = Properties.Settings.Default.prescas;
                                                        else
                                                            barvaP = Properties.Settings.Default.vyreseno;
                                                        break;
                                                    default:
                                                        stst = form.jazyk.Windows_Search_Probiha;
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                }
                                                string casDoString = "";
                                                if ((new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm") != "0:00")
                                                    casDoString = (new DateTime(1, 1, 1, ti.Get<NbtByte>("Do h").Value, ti.Get<NbtByte>("Do m").Value, 0)).ToString("H:mm");
                                                string terp = "";
                                                if (ti["Terp"] != null && ti["Terp"].StringValue != "")
                                                    terp = ti["Terp"].StringValue;
                                                else
                                                    terp = Zakaznici.GetTerp(c.Name);

                                                if (ti["Task"] != null && ti["Task"].StringValue != "")
                                                    terp += " | " + ti["Task"].StringValue;
                                                else
                                                    terp += " | " + DejTask(ti["ID"].StringValue);
                                                ListViewItem lvi = new ListViewItem(new string[] { "", ti.Get<NbtString>("PC").Value, ti.Get<NbtString>("ID").Value, c.Name, ti.Get<NbtString>("Popis").Value, ti.Get<NbtString>("Kontakt").Value, new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value).ToString("dd.M.yyyy"), terp, stst, ti.Get<NbtString>("Poznamky").Value });
                                                lvi.BackColor = barvaP;
                                                lvi.ForeColor = form.ContrastColor(barvaP);
                                                lvi.Tag = new Tag(new DateTime(ti.Get<NbtShort>("Rok").Value, mesic, ti.Get<NbtByte>("Den").Value), ti.Get<NbtLong>("IDlong").Value);
                                                listView1.Items.Add(lvi);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { MessageBox.Show(form.jazyk.Error_RegexError); }
            }
            listView1.EndUpdate();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                vybranyIndex = listView1.SelectedIndices[0];

                string[] datum = listView1.SelectedItems[0].SubItems[6].Text.Split('.');

                string mesic = "";
                switch (datum[1])
                {
                    case "1":
                        mesic = "leden";
                        break;
                    case "2":
                        mesic = "unor";
                        break;
                    case "3":
                        mesic = "brezen";
                        break;
                    case "4":
                        mesic = "duben";
                        break;
                    case "5":
                        mesic = "kveten";
                        break;
                    case "6":
                        mesic = "cerven";
                        break;
                    case "7":
                        mesic = "cervenec";
                        break;
                    case "8":
                        mesic = "srpen";
                        break;
                    case "9":
                        mesic = "zari";
                        break;
                    case "10":
                        mesic = "rijen";
                        break;
                    case "11":
                        mesic = "listopad";
                        break;
                    case "12":
                        mesic = "prosinec";
                        break;
                }

                form.vybranyMesic = mesic;

                foreach (TabPage tp in form.tabControl1.Controls)
                {
                    if (tp.Controls.ContainsKey(form.vybranyMesic))
                    {
                        foreach (ListViewItem lvi in ((ListView)tp.Controls[form.vybranyMesic]).Items)
                        {
                            
                            if(((Tag)lvi.Tag).Compare((Tag)listView1.SelectedItems[0].Tag))
                            {
                                form.posledniVybrany = lvi.Index;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string puvodniRok = (string)form.rokVyber.SelectedItem;
            if (((Tag)listView1.SelectedItems[0].Tag).Datum.Year.ToString() != (string)form.rokVyber.SelectedItem)
                form.rokVyber.SelectedItem = ((Tag)listView1.SelectedItems[0].Tag).Datum.Year.ToString();
            TicketWindow ticketWindow = new TicketWindow(form, false, true, listView1.SelectedItems[0]);
            ticketWindow.StartPosition = FormStartPosition.Manual;
            ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            ticketWindow.Text = form.jazyk.Windows_Search_JenProCteni;
            ticketWindow.idTicketu.ReadOnly = true;
            ticketWindow.zakaznik.Enabled = false;
            ticketWindow.pocitac.ReadOnly = true;
            ticketWindow.kontakt.ReadOnly = true;
            ticketWindow.popis.ReadOnly = true;
            ticketWindow.zacatek.ReadOnly = true;
            ticketWindow.konec.ReadOnly = true;
            ticketWindow.pauzaOd.ReadOnly = true;
            ticketWindow.pauzaDo.ReadOnly = true;
            ticketWindow.pridat.Enabled = false;
            ticketWindow.upravit.Enabled = false;
            ticketWindow.smazat.Enabled = false;
            ticketWindow.stavTicketu.Enabled = false;
            ticketWindow.normalni.Enabled = false;
            ticketWindow.nahradni.Enabled = false;
            ticketWindow.prescas.Enabled = false;
            ticketWindow.volno.Enabled = false;
            ticketWindow.richTextBox1.ReadOnly = true;
            ticketWindow.listView1.Enabled = false;
            ticketWindow.ok.Enabled = false;
            ticketWindow.cas.Enabled = false;
            ticketWindow.button1.Enabled = false;
            ticketWindow.datum.Enabled = false;
            ticketWindow.ShowDialog();
            form.rokVyber.SelectedItem = puvodniRok;
        }

        private void Search_FormClosing(object sender, FormClosingEventArgs e)
        {
            form.vybranyMesic = fmPuvodni;
            form.posledniVybrany = fiPuvodni;
        }

        private void pc_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1_Click(sender, e);
            }
        }

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                form.Kopirovat(((Tag)listView1.SelectedItems[0].Tag).IDlong, ((Tag)listView1.SelectedItems[0].Tag).Datum, listView1.SelectedItems[0].SubItems[3].Text, false, true);
            }
        }

        bool canChange = true;
        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (canChange)
            {
                canChange = false;
                if (e.ColumnIndex == 0)
                {
                    listView1.Columns[0].Width = 0;
                }
                canChange = true;
            }

        }

        private string DejTask(string id)
        {
            if (id.StartsWith("INC") || id.StartsWith("RIT") || id.StartsWith("ITASK") || id.StartsWith("RTASK") || id.StartsWith("TASK"))
            {
                return Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
            }
            else if (id.StartsWith("PRB") || id.StartsWith("PTASK"))
                return Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;
            else return "-";
        }
    }
}
