using System;
using System.Windows.Forms;
using System.Collections.Generic;
using fNbt;
using System.Drawing;

namespace Ticketník
{
    partial class Form1 : Form
    {
        bool keyDown = false;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N && e.Modifiers == Keys.Control)
            {
                novýToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyCode == Keys.O && e.Modifiers == Keys.Control)
            {
                načístToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                uložitToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyCode == Keys.E && e.Modifiers == Keys.Control)
            {
                exportovatToolStripMenuItem_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
            {
                hledat_Click(sender, e);
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                Vlozit();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (toolStripButton3.Enabled)
                    toolStripButton3_Click(sender, e);
            }
            else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                if (!terpTaskFileLock)
                {
                    Kopirovat();
                }
                else
                    MessageBox.Show(jazyk.Message_TerpUpdate, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (e.KeyCode == Keys.NumPad0 && e.Modifiers == Keys.Control)
            {
                Properties.Settings.Default.umisteni = new System.Drawing.Point(0, 0);
                this.Location = new Point(0, 0);
            }
            else if (e.KeyCode == Keys.Insert)
            {
                toolStripButton1_Click(sender, e);
            }
            else if (e.KeyCode == Keys.C && e.Modifiers == (Keys.Control | Keys.Shift))
            {
                Kopirovat(-2, new DateTime(), "", true);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                foreach (TabPage tp in tabControl1.Controls)
                {
                    if (tp.Controls.ContainsKey(vybranyMesic))
                    {
                        if (((ListView)tp.Controls[vybranyMesic]).SelectedIndices.Count != 0 && ((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).IDlong != -1)
                            toolStripButton2_Click(sender, e);
                        break;
                    }
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (!keyDown)
                {
                    keyDown = true;
                    if (tabControl1.SelectedIndex != 0)
                        tabControl1.SelectedIndex--;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (!keyDown)
                {
                    keyDown = true;
                    if (tabControl1.SelectedIndex != 11)
                        tabControl1.SelectedIndex++;
                }
            }
            else if (e.KeyCode == Keys.F1)
            {
                toolStripMenu_Napoveda_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F5 || (e.KeyCode == Keys.R && e.Modifiers == Keys.Control))
            {
                jmenoSouboru = Properties.Settings.Default.filePath;
                LoadFile();
            }
            else if (e.KeyCode == Keys.Q && e.Modifiers == (Keys.Control | Keys.Shift))
            {
                //vývojová zkratka

                devtest = !devtest;
                toolStripMenuItem6.Visible = devtest;
                reportToolStripMenuItem.Visible = devtest;
                dostupnéJazykyToolStripMenuItem.Visible = devtest;
                Logni("Použita vývojová zkratka Ctrl+Shift+Q. DevMode: " + (devtest ? "On" : "Off"), LogMessage.INFO);
            }
            else if (e.KeyCode == Keys.E && e.Modifiers == (Keys.Control | Keys.Shift))
            {
                //vývojová zkratka
                Logni("Používám testovací crash.", LogMessage.INFO);
                throw new Exception("Test exception");
            }
            else if (e.KeyCode == Keys.I && e.Modifiers == (Keys.Control | Keys.Shift))
            {
                //vývojová zkratka
                // jméno, umístění, počet ticketů, last ID, verze, nejstarší rok, nejmladší rok.
                if (file != null)
                {
                    List<int> roky = new List<int>();
                    int pocet = 0;

                    if (file.RootTag.Get<NbtInt>("verze").Value >= 10100)
                    {
                        foreach (NbtCompound rok in file.RootTag.Get<NbtCompound>("Zakaznici").Tags)
                        {
                            roky.Add(int.Parse(rok.Name));
                            foreach (NbtCompound zakosi in rok.Tags)
                            {
                                foreach (NbtCompound mesice in zakosi.Tags)
                                {
                                    pocet += ((NbtList)mesice.Get<NbtList>("Tickety")).Count;
                                }
                            }
                        }
                    }
                    else
                        roky.Add(0);
                    roky.Sort();
                    int max = roky.Count - 1;
                    MessageBox.Show("Verze: " + file.RootTag.Get<NbtInt>("verze").Value + "\r\n" +
                    "Max ID: " + file.RootTag.Get<NbtLong>("MaxID").Value.ToString("n0") + "\r\n" +
                    "Počet ticketů: " + pocet.ToString("n0") + "\r\n" +
                    "Kapacita: " + int.MaxValue.ToString("n0") + "\r\n" +
                    "Min rok: " + roky[0] + "\r\n" +
                    "Max rok: " + roky[max] + "\r\n" +
                    "Cesta: " + jmenoSouboru + "\r\n\r\n" + 
                    "Konfig: " + System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.Replace("\\" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version + "\\user.config", "").Replace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Ticketník\\", "")
                    , "Info o souboru", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
        }
    }
}
