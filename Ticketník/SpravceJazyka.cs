using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;

namespace Ticketník
{
    public partial class SpravceJazyka : Form
    {
        Form1 form;
        public SpravceJazyka(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            stahnout.Enabled = false;
            odebrat.Enabled = false;
            listView1.SmallImageList = new ImageList();
            listView1.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            listView1.SmallImageList.Images.Add(Properties.Resources.green_ball);
            listView1.SmallImageList.Images.Add(Properties.Resources.yellow_ball);
            listView1.SmallImageList.Images.Add(Properties.Resources.red_ball);

            this.Text = form.jazyk.Windows_Spravce_Spravce;
            stahnout.Text = form.jazyk.Windows_Spravce_Stahnout;
            odebrat.Text = form.jazyk.Windows_Spravce_Odebrat;
            listView1.Columns[1].Text = form.jazyk.Windows_Spravce_Jazyk;
            listView1.Columns[2].Text = form.jazyk.Windows_Spravce_Zkratka;
            listView1.Columns[3].Text = form.jazyk.Windows_Spravce_Verze;

            UpdateLang();
            Motiv.SetMotiv(this);
        }

        private async void UpdateLang()
        {
            if (!InvokeRequired)
            {
                listView1.BeginUpdate();
                listView1.Items.Clear();
            }
            else
            {
                this.BeginInvoke(new Action(() => listView1.BeginUpdate()));
                this.BeginInvoke(new Action(() => listView1.Items.Clear()));
            }
            string[] seznamArr = Directory.GetFiles(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang");
            
            try
            {
                //download z netu
                try
                {
                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                    {
                        AllowAutoRedirect = true
                    }))
                    {
                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/jazyky.xml").ConfigureAwait(false))
                        {
                            using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\jazyky.xml", FileMode.Create))
                            {
                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                            }
                        }
                    }
                    form.Logni("Kontroluji jazyky na " + Properties.Settings.Default.ZalozniUpdate + "/jazyky.xml", Form1.LogMessage.INFO);

                    form.Logni("Kontroluji jazyky na " + Properties.Settings.Default.ZalozniUpdate + "/jazyky.xml", Form1.LogMessage.INFO);
                }
                catch (Exception ee)
                {
                    form.Logni("Vyhledání aktualizací jazyků selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                    return;//throw new Exception("Nelze vyhledat žádný update source");
                }
                

                XmlDocument jazyky = new XmlDocument();
                jazyky.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\jazyky.xml");
                XmlNodeList jazlist = jazyky.SelectNodes("Jazyky/Jazyk");
                List<string> seznam = new List<string>();

                foreach(string s in seznamArr)
                {
                    seznam.Add(Path.GetFileName(s));
                }

                foreach(XmlNode jazyk in jazlist)
                {
                    ListViewItem lvi = new ListViewItem(new string[] {"", jazyk.Attributes["jmeno"].InnerText, jazyk.Attributes["zkratka"].InnerText, jazyk.Attributes["verze"].InnerText + "." + jazyk.Attributes["revize"].InnerText, ""});
                    lvi.UseItemStyleForSubItems = false;

                    if(form.langVersion <= int.Parse(jazyk.Attributes["verze"].InnerText))
                        lvi.ImageIndex = 0;
                    else if (form.langVersion == int.Parse(jazyk.Attributes["verze"].InnerText) +1)
                        lvi.ImageIndex = 1;
                    else 
                        lvi.ImageIndex = 2;
                    Tag tag = new Tag(jazyk.Attributes["zkratka"].InnerText + ".xml", false);

                    if (seznam.Contains(jazyk.Attributes["zkratka"].InnerText + ".xml"))
                    {
                        lvi.SubItems[4].BackColor = Color.Green;
                        tag.Instalován = true;
                    }
                    lvi.Tag = tag;

                    if (!InvokeRequired)
                    {
                        listView1.Items.Add(lvi);
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() => listView1.Items.Add(lvi)));
                    }
                }
            }
            catch (Exception ex)
            {
                form.Logni("Nelze načíst aktuální seznam jazyků. " + ex.Message, Form1.LogMessage.WARNING);

                foreach(string s in seznamArr)
                {
                    XmlDocument jazyk = new XmlDocument();
                    jazyk.Load(s);

                    ListViewItem lvi = new ListViewItem(new string[] { "", jazyk.SelectSingleNode("Language").Attributes["name"].InnerText, jazyk.SelectSingleNode("Language").Attributes["shortcut"].InnerText, jazyk.SelectSingleNode("Language").Attributes["version"].InnerText + "." + jazyk.SelectSingleNode("Language").Attributes["revision"].InnerText, "" });
                    lvi.UseItemStyleForSubItems = false;

                    if (form.langVersion <= int.Parse(jazyk.SelectSingleNode("Language").Attributes["version"].InnerText))
                        lvi.ImageIndex = 0;
                    else if (form.langVersion == int.Parse(jazyk.SelectSingleNode("Language").Attributes["version"].InnerText) + 1)
                        lvi.ImageIndex = 1;
                    else
                        lvi.ImageIndex = 2;

                    lvi.SubItems[4].BackColor = Color.Green;
                    Tag tag = new Tag(jazyk.SelectSingleNode("Language").Attributes["shortcut"].InnerText + ".xml", true);
                    lvi.Tag = tag;

                    if (!InvokeRequired)
                    {
                        listView1.Items.Add(lvi);
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() => listView1.Items.Add(lvi)));
                    }
                }
            }
            
            if (!InvokeRequired)
            {
                listView1.EndUpdate();
                pictureBox1.Visible = false;
                pictureBox1.Dispose();
                pictureBox1 = null;
            }
            else
            {
                this.BeginInvoke(new Action(() => pictureBox1.Visible = false));
                this.BeginInvoke(new Action(() => pictureBox1.Dispose()));
                this.BeginInvoke(new Action(() => pictureBox1 = null));
                this.BeginInvoke(new Action(() => listView1.EndUpdate()));
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (((Tag)listView1.SelectedItems[0].Tag).Instalován == true)
                {
                    if (((Tag)listView1.SelectedItems[0].Tag).Soubor != "CZ.xml")
                        odebrat.Enabled = true;
                    stahnout.Enabled = false;
                }
                else
                {
                    odebrat.Enabled = false;
                    stahnout.Enabled = true;
                }
            }
            else
            {
                odebrat.Enabled = false;
                stahnout.Enabled = false;
            }
        }

        private void odebrat_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if(form.jazyk.Zkratka == ((Tag)listView1.SelectedItems[0].Tag).Soubor.Replace(".xml", ""))
                {
                    File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + ((Tag)listView1.SelectedItems[0].Tag).Soubor);
                    form.jazyk = new Jazyk();
                    this.Text = form.jazyk.Windows_Spravce_Spravce;
                    stahnout.Text = form.jazyk.Windows_Spravce_Stahnout;
                    odebrat.Text = form.jazyk.Windows_Spravce_Odebrat;
                    listView1.Columns[1].Text = form.jazyk.Windows_Spravce_Jazyk;
                    listView1.Columns[2].Text = form.jazyk.Windows_Spravce_Zkratka;
                    listView1.Columns[3].Text = form.jazyk.Windows_Spravce_Verze;
                    form.jazyk.Reload(form);
                    listView1.SelectedItems[0].UseItemStyleForSubItems = true;
                    listView1.SelectedItems[0].SubItems[4].BackColor = SystemColors.Window;
                    ((Tag)listView1.SelectedItems[0].Tag).Instalován = false;
                }
                else
                {
                    File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + ((Tag)listView1.SelectedItems[0].Tag).Soubor);
                    listView1.SelectedItems[0].UseItemStyleForSubItems = true;
                    listView1.SelectedItems[0].SubItems[4].BackColor = SystemColors.Window;
                    ((Tag)listView1.SelectedItems[0].Tag).Instalován = false;
                }
            }
            odebrat.Enabled = false;
            stahnout.Enabled = true;
        }

        private async void stahnout_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                    {
                        AllowAutoRedirect = true
                    }))
                    {
                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/lang/" + ((Tag)listView1.SelectedItems[0].Tag).Soubor).ConfigureAwait(false))
                        {
                            using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + ((Tag)listView1.SelectedItems[0].Tag).Soubor, FileMode.Create))
                            {
                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                            }
                        }
                    }
                    form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "\\lang\\" + ((Tag)listView1.SelectedItems[0].Tag).Soubor, Form1.LogMessage.INFO);
                }
                catch (Exception ee)
                {
                    form.Logni("Stažení jazyka selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                }
                
                listView1.SelectedItems[0].UseItemStyleForSubItems = false;
                listView1.SelectedItems[0].SubItems[4].BackColor = Color.Green;
                ((Tag)listView1.SelectedItems[0].Tag).Instalován = true;
            }
            stahnout.Enabled = false;
            odebrat.Enabled = true;
        }
    }
}
