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
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public Prilohy(Form1 form, long ticketID)
        {
            InitializeComponent();
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
            if (!Directory.Exists(appdata + "\\Ticketnik\\Prilohy"))
            {
                Directory.CreateDirectory(appdata + "\\Ticketnik\\Prilohy");
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
                SHA256 sHA256 = SHA256.Create();
                string hash = Convert.ToBase64String(sHA256.ComputeHash(File.ReadAllBytes(cesta)));
                sHA256.Dispose();
            }
        }
    }
}
