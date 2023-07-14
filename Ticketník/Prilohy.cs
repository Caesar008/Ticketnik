using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fNbt;
using System.Security.Cryptography;
using System.Security.Policy;

namespace Ticketník
{
    /*
     * Složka %Appdata%\Ticketnik\Prilohy
     * tam soubor prilohy.dat
     * ten obsahuje compoudy 'Tickety' a 'Seznam příloh'
     * Tickety: jméno je ID ticketu, obsahuje NbtList stringů s hashi příloh
     * 
     * Seznam příloh: jmené je hash souboru. Obsahuje Jméno, Cesta a NbtList longů se seznamem ID ticketů, 
     * kteří ten soubor mají jako přílohu (kvůli případnému mazání).
     */
    public partial class Prilohy : Form
    {
        private long ticketID = -1;
        static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public Prilohy(Form1 form, long ticketID)
        {
            InitializeComponent();
            listView1.Columns.Add("", listView1.Width - 20);
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            listView1.FullRowSelect = true;
            Motiv.SetMotiv(this);
            this.Text = form.jazyk.Windows_Prilohy_Prilohy;
            this.addBtn.Text = form.jazyk.Windows_Prilohy_Pridat;
            this.delBtn.Text = form.jazyk.Windows_Prilohy_Smazat;
            this.findBtn.Text = form.jazyk.Windows_Prilohy_Najit;
            this.ticketID = ticketID;
            this.delBtn.Enabled = false;
            this.findBtn.Enabled = false;
            NactiPrilohy(ticketID);
        }

        private void NactiPrilohy(long ticketID)
        {
            listView1.Items.Clear();
            if (!Directory.Exists(appdata + "\\Ticketnik\\Prilohy"))
            {
                Directory.CreateDirectory(appdata + "\\Ticketnik\\Prilohy");
                NbtFile prilohyFile = new NbtFile(new NbtCompound("Přílohy"));
                prilohyFile.RootTag.Add(new NbtCompound("Seznam příloh"));
                prilohyFile.RootTag.Add(new NbtCompound("Tickety"));
                prilohyFile.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
                return;
            }

            if(!File.Exists(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat"))
            {
                NbtFile prilohyFile = new NbtFile(new NbtCompound("Přílohy"));
                prilohyFile.RootTag.Add(new NbtCompound("Seznam příloh"));
                prilohyFile.RootTag.Add(new NbtCompound("Tickety"));
                prilohyFile.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
                return;
            }

            NbtFile prilohyDat = new NbtFile();
            prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");
            if(prilohyDat.RootTag.Get<NbtCompound>("Tickety").Contains(ticketID.ToString()))
            {
                listView1.BeginUpdate();
                foreach(NbtString prilohaHash in prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()))
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
            if(DialogResult.OK == openFileDialog1.ShowDialog())
            {
                string cesta = openFileDialog1.FileName;
                string fileName = openFileDialog1.SafeFileName;
                SHA256 sHA256 = SHA256.Create();
                string hash = Convert.ToBase64String(sHA256.ComputeHash(File.ReadAllBytes(cesta)));
                string folderName = hash.Substring(0, 2);

                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName);
                }
                if (!File.Exists(appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName + "\\" + fileName))
                {
                    File.Copy(cesta, appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName + "\\" + fileName);
                }
                else
                {
                    string testHash = Convert.ToBase64String(sHA256.ComputeHash(File.ReadAllBytes(appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName + "\\" + fileName)));
                    if(testHash != hash)
                    {
                        fileName = fileName.Remove(fileName.LastIndexOf(".")) + " (" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ")" + 
                            fileName.Remove(0, fileName.LastIndexOf(".") + 1);
                        File.Copy(cesta, appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName + "\\" + fileName);
                    }
                }

                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash) == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Add(new NbtCompound(hash));
                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtString>("Jméno") == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Add(new NbtString("Jméno", fileName));
                if (prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Get<NbtString>("Cesta") == null)
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(hash).Add(new NbtString("Cesta", appdata + "\\Ticketnik\\Prilohy\\Soubory\\" + folderName + "\\" + fileName));
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
                foreach(NbtString hashString in prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()))
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
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
                NactiPrilohy(ticketID);
            }
        }

        public static void ZrusPrilohy()
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");
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

                foreach(NbtList nl in prilohyDat.RootTag.Get<NbtCompound>("Tickety"))
                {
                    if (nl.Name.StartsWith("-"))
                        tr.Add(nl.Name);
                }

                /*if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>("-1") != null)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Remove("-1");
                }*/

                foreach(string trs in tr)
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

                foreach(string s in toRemove)
                {
                    string cesta = prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Get<NbtCompound>(s).Get<NbtString>("Cesta").Value;
                    File.Delete(cesta);
                    prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh").Remove(s);
                }

                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
            }
        }

        public static void ZrusPrilohy(long ticketID)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");
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
                    }
                }
                if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()) != null)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(ticketID.ToString()).Name = (-ticketID).ToString();
                }
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
            }
        }

        public static void ObnovPrilohy()
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");
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

                    }
                }

                List<string> indexy = new List<string>();

                foreach(NbtList nl in prilohyDat.RootTag.Get<NbtCompound>("Tickety"))
                {
                    if (nl.Name.StartsWith("-"))
                        indexy.Add(nl.Name);
                }

                foreach(string str in indexy)
                {
                    if (prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(str) != null)
                    {
                        prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>(str).Name = str.Remove(0, 1);
                    }
                }

                
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
            }
        }

        public static void PropojPrilohy(long ticketID)
        {
            if (File.Exists(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat"))
            {
                NbtFile prilohyDat = new NbtFile();
                prilohyDat.LoadFromFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat");
                foreach(NbtCompound prilohy in prilohyDat.RootTag.Get<NbtCompound>("Seznam příloh"))
                {
                    List<int> index = new List<int>();
                    if(prilohy.Get<NbtList>("Tickety").Count > 0)
                    {
                        foreach(NbtLong nlong in prilohy.Get<NbtList>("Tickety"))
                        {
                            if(nlong.Value == -1) 
                            {
                                index.Add(prilohy.Get<NbtList>("Tickety").IndexOf(nlong));
                            }
                        }
                    }

                    foreach(int i in index)
                    {
                        prilohy.Get<NbtList>("Tickety").Get<NbtLong>(i).Value = ticketID;
                    }
                }
                if(prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>("-1") != null)
                {
                    prilohyDat.RootTag.Get<NbtCompound>("Tickety").Get<NbtList>("-1").Name = ticketID.ToString();
                }
                prilohyDat.SaveToFile(appdata + "\\Ticketnik\\Prilohy\\prilohy.dat", NbtCompression.GZip);
            }
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.Width - 20;
        }
    }
}
