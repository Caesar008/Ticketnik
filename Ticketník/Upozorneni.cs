using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ticketník
{
    
    public partial class Upozorneni : Form
    {
        Form1 form;
        internal DateTime datum;
        internal string typ, popis;
        public Upozorneni(Form1 form)
        {
            InitializeComponent();
            this.form = form;

            this.Text = form.jazyk.Windows_Upozorneni_Upozorneni;
            listView1.Columns[1].Text = form.jazyk.Windows_Upozorneni_Datum;
            listView1.Columns[2].Text = form.jazyk.Windows_Upozorneni_Cas;
            listView1.Columns[0].Text = form.jazyk.Windows_Upozorneni_Typ;
            listView1.Columns[3].Text = form.jazyk.Windows_Upozorneni_Popis;
            noveUpozorneni.Text = form.jazyk.Windows_Upozorneni_NoveUpozorneni;
            upravitUpozorneni.Text = form.jazyk.Windows_Upozorneni_UpravitUpozorneni;
            smazatUpozorneni.Text = form.jazyk.Windows_Upozorneni_SmazatUpozorneni;
            upravitUpozorneni.Enabled = false;
            smazatUpozorneni.Enabled = false;

            listView1.SmallImageList = new ImageList();
            listView1.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            listView1.SmallImageList.Images.Add(Properties.Resources.bell_16);
            listView1.SmallImageList.Images.Add(Properties.Resources.remote);

            LoadUpozorneni();
        }

        private void LoadUpozorneni()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (UpozorneniCls upo in form.upozorneni)
            {
                ListViewItem lvi = new ListViewItem(new string[] { "", upo.Datum.ToString("d.M.yyyy"), upo.Datum.ToString("H:mm"), upo.Popis });
                if (upo.TypUpozorneni == UpozorneniCls.Typ.Upozorneni)
                {
                    lvi.ImageIndex = 0;
                    lvi.ToolTipText = form.jazyk.Windows_Upozorneni_Upo;
                }
                else
                {
                    lvi.ImageIndex = 1;
                    lvi.ToolTipText = form.jazyk.Windows_Upozorneni_RDP;
                }
                lvi.Tag = new Tag(upo.Datum, upo.Popis, upo.TypUpozorneni);
                listView1.Items.Add(lvi);
            }
            listView1.EndUpdate();
        }

        private void noveUpozorneni_Click(object sender, EventArgs e)
        {
            NoveUpozorneni upozorneni = new NoveUpozorneni(form, this);
            upozorneni.StartPosition = FormStartPosition.Manual;
            upozorneni.Location = new Point(this.Location.X + 50, this.Location.Y+50);
            if (upozorneni.ShowDialog() == DialogResult.OK)
            {

                UpozorneniCls.Typ typT;

                if (typ == form.jazyk.Windows_Upozorneni_RDP)
                {
                    typT = UpozorneniCls.Typ.RDP;
                }
                else
                {
                    typT = UpozorneniCls.Typ.Upozorneni;
                }
                UpozorneniCls.Add(new UpozorneniCls(datum, popis, typT));
                form.upozorneni = UpozorneniCls.UpozorneniList;
                LoadUpozorneni();
                upravitUpozorneni.Enabled = false;
                smazatUpozorneni.Enabled = false;

            }
        }
        internal void noveUpozorneni_OnDemand(Form1 form1, Upozorneni upozorneni1, DateTime datum1, string typ1, string popis1)
        {
            NoveUpozorneni upozorneni = new NoveUpozorneni(form, this);
            upozorneni.StartPosition = FormStartPosition.Manual;
            upozorneni.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            upozorneni.dateTimePicker1.Value = datum1;
            upozorneni.comboBox1.SelectedItem = typ1;
            upozorneni.textBox1.Text = popis1;
            upozorneni.comboBox1.Enabled = false;

            if (upozorneni.ShowDialog() == DialogResult.OK)
            {

                UpozorneniCls.Typ typT;

                if (typ == form.jazyk.Windows_Upozorneni_RDP)
                {
                    typT = UpozorneniCls.Typ.RDP;
                }
                else
                {
                    typT = UpozorneniCls.Typ.Upozorneni;
                }
                UpozorneniCls.Add(new UpozorneniCls(datum, popis, typT));
                form.upozorneni = UpozorneniCls.UpozorneniList;
                LoadUpozorneni();
                upravitUpozorneni.Enabled = false;
                smazatUpozorneni.Enabled = false;

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedIndices.Count > 0)
            {
                upravitUpozorneni.Enabled = true;
                smazatUpozorneni.Enabled = true;

            }
            else
            {
                upravitUpozorneni.Enabled = false;
                smazatUpozorneni.Enabled = false;
            }
        }

        private void upravitUpozorneni_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                NoveUpozorneni nupo = new NoveUpozorneni(form, this);
                nupo.StartPosition = FormStartPosition.Manual;
                nupo.Location = new Point(this.Location.X + 50, this.Location.Y + 50);

                if (nupo.dateTimePicker1.MinDate > ((Tag)listView1.SelectedItems[0].Tag).Datum)
                    nupo.dateTimePicker1.MinDate = ((Tag)listView1.SelectedItems[0].Tag).Datum;

                nupo.dateTimePicker1.Value = ((Tag)listView1.SelectedItems[0].Tag).Datum;
                switch(((Tag)listView1.SelectedItems[0].Tag).Typ)
                {
                    case UpozorneniCls.Typ.RDP:
                        nupo.comboBox1.SelectedItem = form.jazyk.Windows_Upozorneni_RDP;
                        break;
                    case UpozorneniCls.Typ.Upozorneni:
                        nupo.comboBox1.SelectedItem = form.jazyk.Windows_Upozorneni_Upo;
                        break;
                    default:
                        nupo.comboBox1.SelectedItem = form.jazyk.Windows_Upozorneni_Upo;
                        break;
                }
                nupo.textBox1.Text = ((Tag)listView1.SelectedItems[0].Tag).Popis;
                if (nupo.ShowDialog() == DialogResult.OK)
                {

                    foreach (UpozorneniCls upo in form.upozorneni)
                    {
                        if (upo.Datum == ((Tag)listView1.SelectedItems[0].Tag).Datum && upo.Popis == ((Tag)listView1.SelectedItems[0].Tag).Popis && upo.TypUpozorneni == ((Tag)listView1.SelectedItems[0].Tag).Typ)
                        {
                            UpozorneniCls.Typ typT;

                            if (typ == form.jazyk.Windows_Upozorneni_RDP)
                            {
                                typT = UpozorneniCls.Typ.RDP;
                            }
                            else
                            {
                                typT = UpozorneniCls.Typ.Upozorneni;
                            }

                            UpozorneniCls.Upravit(upo, new UpozorneniCls(datum, popis, typT));
                            form.upozorneni = UpozorneniCls.UpozorneniList;
                            LoadUpozorneni();
                            break;
                        }
                    }
                    upravitUpozorneni.Enabled = false;
                    smazatUpozorneni.Enabled = false;
                }
            }
        }

        private void smazatUpozorneni_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                foreach (UpozorneniCls upo in form.upozorneni)
                {
                    if (upo.Datum == ((Tag)listView1.SelectedItems[0].Tag).Datum && upo.Popis == ((Tag)listView1.SelectedItems[0].Tag).Popis && upo.TypUpozorneni == ((Tag)listView1.SelectedItems[0].Tag).Typ)
                    {
                        UpozorneniCls.Remove(upo);
                        form.upozorneni = UpozorneniCls.UpozorneniList;
                        LoadUpozorneni();
                        break;
                    }
                }
                upravitUpozorneni.Enabled = false;
                smazatUpozorneni.Enabled = false;

            }
        }

        private void Upozorneni_FormClosing(object sender, FormClosingEventArgs e)
        {
            form.upozozrneniMuze = true;
        }

        
    }
}
