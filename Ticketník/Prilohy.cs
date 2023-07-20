using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using fNbt;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Ticketník
{
    public partial class Prilohy : Form
    {
        private long ticketID = -1;
        static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        Form1 form;
        public Prilohy(Form1 form, long ticketID)
        {
            InitializeComponent();
            listView1.Columns.Add("", listView1.Width - 20);
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.FullRowSelect = true;
            listView1.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            richTextBox1.Text = "";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Motiv.SetMotiv(this);
            this.Text = form.jazyk.Windows_Prilohy_Prilohy;
            this.addBtn.Text = form.jazyk.Windows_Prilohy_Pridat;
            this.delBtn.Text = form.jazyk.Windows_Prilohy_Smazat;
            this.findBtn.Text = form.jazyk.Windows_Prilohy_Najit;
            this.ticketID = ticketID;
            this.delBtn.Enabled = false;
            this.findBtn.Enabled = false;
            this.form = form;
            NactiPrilohy(ticketID);
        }

        private void NactiPrilohy(long ticketID)
        {
            listView1.Items.Clear();
            this.form.Logni("Načítám seznam příloh", Form1.LogMessage.INFO);
            if (!Directory.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1)))
            {
                this.form.Logni("Vytvářím složku příloh i s dat souborem pro " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1), Form1.LogMessage.INFO);
                Directory.CreateDirectory(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1));
                NbtFile prilohyFile = new NbtFile(new NbtCompound("Přílohy"));
                prilohyFile.RootTag.Add(new NbtCompound("Seznam příloh"));
                prilohyFile.RootTag.Add(new NbtCompound("Tickety"));
                prilohyFile.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
                return;
            }

            if (!File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat"))
            {
                this.form.Logni("Vytvářím prilohy.dat pro " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1), Form1.LogMessage.INFO);
                NbtFile prilohyFile = new NbtFile(new NbtCompound("Přílohy"));
                prilohyFile.RootTag.Add(new NbtCompound("Seznam příloh"));
                prilohyFile.RootTag.Add(new NbtCompound("Tickety"));
                prilohyFile.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
                return;
            }

            NbtFile prilohyDat = new NbtFile();
            prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
            if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Contains(ticketID.ToString()))
            {
                listView1.BeginUpdate();
                foreach (NbtString prilohaHash in prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()))
                {
                    NbtCompound priloha = prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(prilohaHash.Value);

                    ListViewItem lvi = new ListViewItem(priloha.Get<NbtString>("Jméno").Value)
                    {
                        Tag = priloha
                    };
                    listView1.Items.Add(lvi);
                }
                listView1.EndUpdate();
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                pictureBox1.Visible = false;
                richTextBox1.Visible = false;
                richTextBox1.Text = "";
                pictureBox1.Image = null;

                string cesta = openFileDialog1.FileName;
                string fileName = openFileDialog1.SafeFileName;
                SHA256 sHA256 = SHA256.Create();
                string hash = Convert.ToBase64String(sHA256.ComputeHash(File.ReadAllBytes(cesta)));
                string folderName = hash.Substring(0, 2);
                this.form.Logni("Přidávám přílohu " + fileName + " k ticketu ID " + ticketID.ToString() + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1), Form1.LogMessage.INFO);

                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName);
                }
                if (!File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName + "\\" + fileName))
                {
                    File.Copy(cesta, appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName + "\\" + fileName);
                }
                else
                {
                    string testHash = Convert.ToBase64String(sHA256.ComputeHash(File.ReadAllBytes(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName + "\\" + fileName)));
                    if (testHash != hash)
                    {
                        fileName = fileName.Remove(fileName.LastIndexOf(".")) + " (" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ")" +
                            fileName.Remove(0, fileName.LastIndexOf(".") + 1);
                        File.Copy(cesta, appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName + "\\" + fileName);
                    }
                }

                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash) == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Add(new NbtCompound(hash));
                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtString>("Jméno") == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Add(new NbtString("Jméno", fileName));
                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtString>("Cesta") == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Add(new NbtString("Cesta", appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory\\" + folderName + "\\" + fileName));
                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtList>("Tickety") == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Add(new NbtList("Tickety", NbtTagType.Long));
                bool found = false;
                foreach (NbtLong index in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtList>("Tickety"))
                {
                    if (index.Value == ticketID)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtList>("Tickety").Add(new NbtLong(ticketID));

                if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()) == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Add(new NbtList(ticketID.ToString(), NbtTagType.String));
                bool foundStr = false;
                foreach (NbtString hashString in prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()))
                {
                    if (hashString.Value == hash)
                    {
                        foundStr = true;
                        break;
                    }
                }
                if (!foundStr)
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).Add(new NbtString(hash));
                sHA256.Dispose();
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
                NactiPrilohy(ticketID);
            }
        }

        public static void ZrusPrilohy(Form1 form)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
                foreach (NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    List<int> index = new List<int>();
                    if (prilohy.Get<NbtList>("Tickety").Count > 0)
                    {
                        foreach (NbtLong nlong in prilohy.Get<NbtList>("Tickety"))
                        {
                            if (nlong.Value < 0)
                            {
                                index.Add(prilohy.Get<NbtList>("Tickety").IndexOf(nlong));
                            }
                        }
                    }

                    foreach (int i in index)
                    {
                        prilohy.Get<NbtList>("Tickety").RemoveAt(i);
                    }
                }

                List<string> tr = new List<string>();

                foreach (NbtList nl in prilohyDat.RootTag.Get<NbtCompound>("Tickety"))
                {
                    if (nl.Name.StartsWith("-"))
                        tr.Add(nl.Name);
                }

                foreach (string trs in tr)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Remove(trs);
                }

                List<string> toRemove = new List<string>();

                foreach (NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    if (prilohy.Get<NbtList>("Tickety").Count == 0)
                    {
                        toRemove.Add(prilohy.Name);
                    }
                }

                foreach (string s in toRemove)
                {
                    string cesta = prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(s).Get<NbtString>("Cesta").Value;
                    File.Delete(cesta);
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Remove(s);
                    form.Logni("Mažu přílohu " + cesta + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1), Form1.LogMessage.INFO);
                }

                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);

                foreach (string s in Directory.GetDirectories(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\Soubory"))
                {
                    if (Directory.GetFiles(s).Length == 0)
                    {
                        Directory.Delete(s);
                        form.Logni("Mažu složku " + s, Form1.LogMessage.INFO);
                    }
                }
            }
        }

        public static void ZrusPrilohy(Form1 form, long ticketID)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
                foreach (NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    List<int> index = new List<int>();
                    if (prilohy.Get<NbtList>("Tickety").Count > 0)
                    {
                        foreach (NbtLong nlong in prilohy.Get<NbtList>("Tickety"))
                        {
                            if (nlong.Value == ticketID)
                            {
                                index.Add(prilohy.Get<NbtList>("Tickety").IndexOf(nlong));
                            }
                        }
                    }

                    foreach (int i in index)
                    {
                        prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value = -ticketID;
                        form.Logni("Označuji přílohy z ticketu ID " + ticketID + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + " pro smazání", Form1.LogMessage.INFO);
                    }
                }
                if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()) != null)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).Name = (-ticketID).ToString();
                }
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
            }
        }

        public static void ObnovPrilohy(Form1 form)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
                foreach (NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    List<int> index = new List<int>();
                    if (prilohy.Get<NbtList>("Tickety").Count > 0)
                    {
                        foreach (NbtLong nlong in prilohy.Get<NbtList>("Tickety"))
                        {
                            if (nlong.Value < 0)
                            {
                                index.Add(prilohy.Get<NbtList>("Tickety").IndexOf(nlong));
                            }
                        }
                    }

                    foreach (int i in index)
                    {
                        prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value = -prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value;
                        form.Logni("Obnovuji přílohy z ticketu ID " + -prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1), Form1.LogMessage.INFO);

                    }
                }

                List<string> indexy = new List<string>();

                foreach (NbtList nl in prilohyDat.RootTag.Get<NbtCompound>("Tickety"))
                {
                    if (nl.Name.StartsWith("-"))
                        indexy.Add(nl.Name);
                }

                foreach (string str in indexy)
                {
                    if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(str) != null)
                    {
                        prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(str).Name = str.Remove(0, 1);
                    }
                }


                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
            }
        }

        public static void PropojPrilohy(Form1 form, long ticketID)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
                foreach (NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    List<int> index = new List<int>();
                    if (prilohy.Get<NbtList>("Tickety").Count > 0)
                    {
                        foreach (NbtLong nlong in prilohy.Get<NbtList>("Tickety"))
                        {
                            if (nlong.Value == -1)
                            {
                                index.Add(prilohy.Get<NbtList>("Tickety").IndexOf(nlong));
                            }
                        }
                    }

                    foreach (int i in index)
                    {
                        prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value = ticketID;
                        form.Logni("Propojuji přílohu " + prilohy.Get<NbtString>("Jméno").Value + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + " s ticketem " + ticketID.ToString(), Form1.LogMessage.INFO);
                    }
                }
                if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>("-1") != null)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>("-1").Name = ticketID.ToString();
                }
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
            }
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.Width - 20;
        }

        private void findBtn_Click(object sender, EventArgs e)
        {
            string p = ((NbtCompound)listView1.SelectedItems[0].Tag).Get<NbtString>("Cesta").Value;
            string args = string.Format("/select, \"{0}\"", p);

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "explorer";
            info.Arguments = args;
            Process.Start(info);
        }

        private void ListView1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                try
                {
                    findBtn.Enabled = true;
                    delBtn.Enabled = true;
                    string cesta = ((NbtCompound)listView1.SelectedItems[0].Tag).Get<NbtString>("Cesta").Value;
                    string extension = Path.GetExtension(cesta);
                    richTextBox1.Text = "";
                    pictureBox1.Image = null;
                    if (extension.ToLower() == ".txt" || extension.ToLower() == ".cs" || extension.ToLower() == ".ps1" || extension.ToLower() == ".json" || extension.ToLower() == ".js" || extension.ToLower() == ".css" || extension.ToLower() == ".html" || extension.ToLower() == ".htm" || extension.ToLower() == ".reg")
                    {
                        pictureBox1.Visible = false;
                        richTextBox1.Visible = true;
                        richTextBox1.Text = File.ReadAllText(cesta);
                    } 
                    else if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg" || extension.ToLower() == ".bmp" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".tif" || extension.ToLower() == ".tiff" || extension.ToLower() == ".gif")
                    {
                        pictureBox1.Visible = true;
                        richTextBox1.Visible = false;

                        System.Drawing.Image img;
                        using (var bmpTemp = new System.Drawing.Bitmap(cesta))
                        {
                            img = new System.Drawing.Bitmap(bmpTemp);
                        }
                        pictureBox1.Image = img;
                    }
                    else
                    {
                        pictureBox1.Visible = false;
                        richTextBox1.Visible = false;
                    }
                }
                catch
                {
                    CustomControls.MessageBox.Show(form.jazyk.Message_WrongExt, MessageBoxIcon.Warning);
                }
            }
            else if (listView1.SelectedItems.Count > 1)
            {
                findBtn.Enabled = false;
                delBtn.Enabled = true;
            }
            else
            {
                findBtn.Enabled = false;
                delBtn.Enabled = false;
                pictureBox1.Visible = false;
                richTextBox1.Visible = false;
            }
        }

        private void SmazatPrilohu(NbtCompound priloha)
        {
            NbtFile prilohyDat = new NbtFile();
            prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
            int index = -1;

            form.Logni("Mažu přílohu " + priloha.Get<NbtString>("Jméno").Value + " ze souboru " + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + " v ticketu " + ticketID.ToString(), Form1.LogMessage.INFO);
            foreach (NbtString hash in prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()))
            {
                if (hash.Value == priloha.Name)
                {
                    index = prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).IndexOf(hash);
                    break;
                }
            }
            if (index >= 0)
                prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).RemoveAt(index);

            index = -1;
            foreach (NbtLong id in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(priloha.Name).Get<NbtList>("Tickety"))
            {
                if (id.Value == ticketID)
                {
                    index = prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(priloha.Name).Get<NbtList>("Tickety").IndexOf(id);
                    break;
                }
            }

            if (index >= 0)
                prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(priloha.Name).Get<NbtList>("Tickety").RemoveAt(index);
            prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
            NactiPrilohy(ticketID);
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                pictureBox1.Visible = false;
                richTextBox1.Visible = false;
                richTextBox1.Text = "";
                pictureBox1.Image = null;
                SmazatPrilohu((NbtCompound)item.Tag);
            }
            findBtn.Enabled = false;
            delBtn.Enabled = false;
        }

        public static void KopirovatPrilohy(Form1 form, long staryTicketID, long novyTicketID)
        {
            NbtFile prilohyDat = new NbtFile();
            prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
            List<string> prilohyHash = new List<string>();

            foreach (NbtCompound priloha in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
            {
                bool found = false;
                foreach (NbtLong id in priloha.Get<NbtList>("Tickety"))
                {
                    if (id.Value == staryTicketID)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                    prilohyHash.Add(priloha.Name);
            }
            foreach (string sHash in prilohyHash)
            {
                prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(sHash).Get<NbtList>("Tickety").Add(new NbtLong(novyTicketID));
                if(prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(novyTicketID.ToString()) == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Add(new NbtList(novyTicketID.ToString(), NbtTagType.String));
                prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(novyTicketID.ToString()).Add(new NbtString(sHash));
            }
            prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat", NbtCompression.GZip);
        }

        public static int PocetPriloh(Form1 form, long ticketID)
        {
            NbtFile prilohyDat = new NbtFile();
            prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\" + form.jmenoSouboru.Remove(0, form.jmenoSouboru.LastIndexOf('\\') + 1) + "\\prilohy.dat");
                        
            return prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()) == null ? 0 : prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).Count;
        }
    }
}
