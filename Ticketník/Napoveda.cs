using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;

namespace Ticketník
{
    public partial class Napoveda : Form
    {
        Form1 form;
        bool napovedaNebyla = false;
        public Napoveda(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            label1.Visible = false;
            browser.IsWebBrowserContextMenuEnabled = false;
            browser.AllowWebBrowserDrop = false;
            browser.WebBrowserShortcutsEnabled = false;
            menuTree.ImageList = new ImageList();
            menuTree.ImageList.ImageSize = new Size(24, 24);
            menuTree.ImageList.Images.Add(Properties.Resources._1page);
            menuTree.ImageList.Images.Add(Properties.Resources._2book);
            this.Text = form.jazyk.SideMenu_Napoveda;
            this.label1.Text = form.jazyk.Windows_Help_Updating;
            browser.ObjectForScripting = new ScriptManager(this);
            Motiv.SetMotiv(this);
            VytvorMenu(true);
        }

        private async void VytvorMenu(bool first = false)
        {
            if (!Directory.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help"))
                Directory.CreateDirectory(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help");

            try
            {
                if (!InvokeRequired)
                    form.timer_ClearInfo.Stop();
                else
                    this.BeginInvoke(new Action(() => form.timer_ClearInfo.Stop()));
                form.infoBox.Text = form.jazyk.Message_hledamAktualizaciNapovedy;

                if (!InvokeRequired)
                    form.Update();
                else
                    this.BeginInvoke(new Action(() => form.Update()));

                if (!File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml"))
                {
                    napovedaNebyla = true;
                    try
                    {
                        //výchozí cesta v síti
                        if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                        {
                            File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", true);
                            form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.xml", Form1.LogMessage.INFO);
                        }
                        else
                        {
                            try
                            {
                                using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                {
                                    AllowAutoRedirect = true
                                }))
                                {
                                    using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Help.xml").ConfigureAwait(false))
                                    {
                                        using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", FileMode.Create))
                                        {
                                            await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                        }
                                    }
                                }
                                form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/Help.xml", Form1.LogMessage.INFO);
                            }
                            catch (Exception e)
                            {
                                try
                                {
                                    File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", true);
                                    form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.xml", Form1.LogMessage.INFO);
                                }
                                catch { }
                                form.Logni("Nelze vyhledat žádný zdroj aktualizací nápovědy.\r\n" + e.Message, Form1.LogMessage.WARNING);
                                throw new Exception("Nelze vyhledat žádný zdroj aktualizací");
                            }
                        }
                    }
                    catch
                    {
                        //backup download z netu
                        try
                        {
                            using (HttpClient hc = new HttpClient(new HttpClientHandler()
                            {
                                AllowAutoRedirect = true
                            }))
                            {
                                using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/help/Help.xml").ConfigureAwait(false))
                                {
                                    using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", FileMode.Create))
                                    {
                                        await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                    }
                                }
                            }
                            form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/help/Help.xml", Form1.LogMessage.INFO);
                        }
                        catch (Exception e)
                        {
                            if (Properties.Settings.Default.pouzivatZalozniUpdate)
                            {
                                try
                                {
                                    File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", true);
                                    form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.xml", Form1.LogMessage.INFO);
                                }
                                catch { }
                            }
                            form.Logni("Nelze vyhledat žádný zdroj aktualizací nápovědy.\r\n" + e.Message, Form1.LogMessage.WARNING);
                            throw new Exception("Nelze vyhledat žádný zdroj aktualizací");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomControls.MessageBox.Show(form.jazyk.Windows_Help_NejdeStahnout);
                form.Logni(ex.Message + "\r\n\r\n" + ex.StackTrace, Form1.LogMessage.ERROR);
                return;
            }

            form.infoBox.Text = "";

            try
            {
                XmlDocument updates = new XmlDocument();

                try
                {
                    //výchozí cesta v síti
                    if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                    {
                        updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                        form.Logni("Vyhledávám aktualizaci nápovědy na " + Properties.Settings.Default.updateCesta + "\\ticketnik.xml", Form1.LogMessage.INFO);
                    }
                    else
                    {
                        try
                        {
                            using (HttpClient hc = new HttpClient(new HttpClientHandler()
                            {
                                AllowAutoRedirect = true
                            }))
                            {
                                using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml").ConfigureAwait(false))
                                {
                                    using (FileStream fs = new FileStream(Path.GetTempPath() + "\\ticketnik.xml", FileMode.Create))
                                    {
                                        await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                    }
                                }
                            }
                            form.Logni("Vyhledávám aktualizaci nápovědy na " + Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", Form1.LogMessage.INFO);
                            updates.Load(Path.GetTempPath() + "\\ticketnik.xml");
                        }
                        catch (Exception e)
                        {
                            form.Logni("Vyhledání aktualizací nápovědy selhalo.\r\n" + e.Message, Form1.LogMessage.WARNING);
                            throw new Exception("Nelze vyhledat žádný update source");
                        }
                    }
                }
                catch
                {
                    //backup download z netu
                    try
                    {
                        using (HttpClient hc = new HttpClient(new HttpClientHandler()
                        {
                            AllowAutoRedirect = true
                        }))
                        {
                            using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml").ConfigureAwait(false))
                            {
                                using (FileStream fs = new FileStream(Path.GetTempPath() + "\\ticketnik.xml", FileMode.Create))
                                {
                                    await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                }
                            }
                        }
                        form.Logni("Vyhledávám aktualizaci nápovědy na " + Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", Form1.LogMessage.INFO);
                        updates.Load(Path.GetTempPath() + "\\ticketnik.xml");
                    }
                    catch (Exception e)
                    {
                        if (Properties.Settings.Default.pouzivatZalozniUpdate)
                        {
                            try
                            {
                                updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                                form.Logni("Vyhledávám aktualizaci nápovědy na " + Properties.Settings.Default.updateCesta + "\\ticketnik.xml", Form1.LogMessage.INFO);
                            }
                            catch { }
                        }
                        form.Logni("Vyhledání aktualizací nápovědy selhalo.\r\n" + e.Message, Form1.LogMessage.WARNING);
                        throw new Exception("Nelze vyhledat žádný update source");
                    }
                }

                XmlDocument help = new XmlDocument();
                help.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml");

                if (int.Parse(help.DocumentElement.Attributes.GetNamedItem("verze").Value) < int.Parse(updates.DocumentElement.SelectSingleNode("Help").InnerText) || napovedaNebyla)
                {
                    label1.Visible = true;

                    StahniHelp();
                }
                else
                {
                    VytvorMenuPoUpd(first);
                }
            }
            catch (Exception ex)
            {
                VytvorMenuPoUpd(first);
                form.Logni("Nelze ověřit verzi nápovědy\r\n\r\n" + ex.Message, Form1.LogMessage.WARNING);
            }
        }

        private void VytvorMenuPoUpd(bool first = false)
        {
            label1.Visible = false;
            XmlDocument help = new XmlDocument();
            help.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml");
            XmlNode menu = help.DocumentElement.SelectSingleNode("Menu");
            if (!InvokeRequired)
                PridejNode(menu.SelectNodes("Item"), menuTree.Nodes);
            else
                this.BeginInvoke(new Action(() => PridejNode(menu.SelectNodes("Item"), menuTree.Nodes)));

            int retry = 0;
            while(menuTree.Nodes.Count == 0 && retry < 60)
            {
                System.Threading.Thread.Sleep(50);
                retry++;
            }
            if (first && menuTree.Nodes.Count > 0)
            {
                if (!InvokeRequired)
                    menuTree.SelectedNode = menuTree.Nodes[0];
                else
                    this.BeginInvoke(new Action(() => menuTree.SelectedNode = menuTree.Nodes[0]));
            }
        }

        private void PridejNode(XmlNodeList polozky, TreeNodeCollection uzle)
        {
            foreach (XmlNode node in polozky)
            {
                if (node.Attributes.GetNamedItem("subitems").Value == "false")
                {
                    TreeNode tn = new TreeNode(node.Attributes.GetNamedItem("name").Value, 0, 0);
                    if (node.Attributes.GetNamedItem("file").Value != "")
                        tn.Tag = node.Attributes.GetNamedItem("file").Value;
                    uzle.Add(tn);
                }
                else
                {
                    TreeNode tn = new TreeNode(node.Attributes.GetNamedItem("name").Value, 1, 1);
                    if (node.Attributes.GetNamedItem("file").Value != "")
                        tn.Tag = node.Attributes.GetNamedItem("file").Value;
                    PridejNode(node.SelectNodes("Item"), tn.Nodes);
                    uzle.Add(tn);
                }
            }
        }
        private async void StahniHelp()
        {
            foreach (FileInfo file in new DirectoryInfo(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help").GetFiles())
            {
                if (!InvokeRequired)
                    file.Delete();
                else
                    this.BeginInvoke(new Action(() => file.Delete()));
                
            }
            foreach (DirectoryInfo dir in new DirectoryInfo(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help").GetDirectories())
            {
                if (!InvokeRequired)
                    dir.Delete(true);
                else
                    this.BeginInvoke(new Action(() => dir.Delete(true)));
            }

            try
            {
                if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                {
                    File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", true);
                    form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.xml", Form1.LogMessage.INFO);

                }
                else
                {
                    try
                    {
                        using (HttpClient hc = new HttpClient(new HttpClientHandler()
                        {
                            AllowAutoRedirect = true
                        }))
                        {
                            using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Help.xml").ConfigureAwait(false))
                            {
                                using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", FileMode.Create))
                                {
                                    await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                }
                            }
                        }
                        form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/Help.xml", Form1.LogMessage.INFO);
                    }
                    catch (Exception ee)
                    {
                        form.Logni("Stažení nápovědy (xml) selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                    }
                }
            }
            catch
            {
                //backup download z netu
                try
                {
                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                    {
                        AllowAutoRedirect = true
                    }))
                    {
                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Help.xml").ConfigureAwait(false))
                        {
                            using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", FileMode.Create))
                            {
                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                            }
                        }
                    }
                    form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/Help.xml", Form1.LogMessage.INFO);
                }
                catch (Exception ee)
                {
                    if (Properties.Settings.Default.pouzivatZalozniUpdate)
                    {
                        try
                        {
                            File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.xml", true);
                            form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.xml", Form1.LogMessage.INFO);
                        }
                        catch { }
                    }
                    form.Logni("Stažení nápovědy (xml) selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                }
            }

            try
            {
                if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                {
                    File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.zip", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", true);
                    form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.zip", Form1.LogMessage.INFO);
                }
                else
                {
                    try
                    {
                        using (HttpClient hc = new HttpClient(new HttpClientHandler()
                        {
                            AllowAutoRedirect = true
                        }))
                        {
                            using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Help.zip").ConfigureAwait(false))
                            {
                                using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", FileMode.Create))
                                {
                                    await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                }
                            }
                        }
                        form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/Help.zip", Form1.LogMessage.INFO);
                    }
                    catch (Exception ee)
                    {
                        form.Logni("Stažení nápovědy (zip) selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                    }
                }
            }
            catch
            {
                //backup download z netu
                try
                {
                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                    {
                        AllowAutoRedirect = true
                    }))
                    {
                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Help.zip").ConfigureAwait(false))
                        {
                            using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", FileMode.Create))
                            {
                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                            }
                        }
                    }
                    form.Logni("Stahuji " + Properties.Settings.Default.ZalozniUpdate + "/Help.zip", Form1.LogMessage.INFO);
                }
                catch (Exception ee)
                {
                    if (Properties.Settings.Default.pouzivatZalozniUpdate)
                    {
                        try
                        {
                            File.Copy(Properties.Settings.Default.updateCesta + "\\help\\Help.zip", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", true);
                            form.Logni("Stahuji " + Properties.Settings.Default.updateCesta + "\\help\\Help.zip", Form1.LogMessage.INFO);
                        }
                        catch { }
                    }
                    form.Logni("Stažení nápovědy (zip) selhalo.\r\n" + ee.Message, Form1.LogMessage.WARNING);
                }
            }

            if (!InvokeRequired)
                ZipFile.ExtractToDirectory(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help");
            else
                this.BeginInvoke(new Action(() => ZipFile.ExtractToDirectory(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help")));
            if (!InvokeRequired)
                File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip");
            else
                this.BeginInvoke(new Action(() => File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\Help.zip")));


            if (!InvokeRequired)
                VytvorMenuPoUpd();
            else
                this.BeginInvoke(new Action(() => VytvorMenuPoUpd()));
            int retry = 0;
            while (menuTree.Nodes.Count < 1 && retry < 60)
            {
                System.Threading.Thread.Sleep(50);
                Application.DoEvents();
                retry++;
            }
            if (!InvokeRequired)
                menuTree.SelectedNode = menuTree.Nodes[0];
            else
                this.BeginInvoke(new Action(() => menuTree.SelectedNode = menuTree.Nodes[0]));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!InvokeRequired)
                VytvorMenuPoUpd();
            else
                this.BeginInvoke(new Action(() => VytvorMenuPoUpd()));
            menuTree.SelectedNode = menuTree.Nodes[0];
        }

        private void menuTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                try
                {
                    if (!zmena)
                    {
                        string scrollFore = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarFore"));
                        string scrollBack = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarBack"));
                        string scrollSeparator = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarSeparator"));
                        string back = ColorTranslator.ToHtml(Motiv.GetBarvy("pozadíControl"));
                        string fore = ColorTranslator.ToHtml(Motiv.GetBarvy("text"));
                        browser.DocumentText = File.ReadAllText(System.Reflection.Assembly.GetEntryAssembly().Location
                            .Replace("Ticketnik.exe", "") + "help\\" + (string)e.Node.Tag)
                            .Replace("{verze}", Application.ProductVersion)
                            .Replace("{cesta}", System.Reflection.Assembly.GetEntryAssembly().Location)
                            .Replace("Ticketnik.exe", "")
                            .Replace('\\', '/')
                            .Replace("{scrollForeColor}", scrollFore)
                            .Replace("{scrollBackColor}", scrollBack)
                            .Replace("{scrollSeparatorColor}", scrollSeparator)
                            .Replace("{backColor}", back)
                            .Replace("{foreColor}", fore)
                            ;
                        menuTree.SelectedNode = null;
                        
                    }
                }
                catch
                {
                    CustomControls.MessageBox.Show(form.jazyk.Windows_Help_Nenalezeno);
                }
            }
            zmena = true;
        }

        internal void LoadLink(string url)
        {
            try
            {
                if (url.StartsWith("tic://"))
                {
                    string scrollFore = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarFore"));
                    string scrollBack = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarBack"));
                    string scrollSeparator = ColorTranslator.ToHtml(Motiv.GetBarvy("scrollBarSeparator"));
                    string back = ColorTranslator.ToHtml(Motiv.GetBarvy("pozadíControl"));
                    string fore = ColorTranslator.ToHtml(Motiv.GetBarvy("text"));
                    string soub = url.Replace("tic://", "");
                    Expand(soub, menuTree.Nodes);
                    soub = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "help\\" + soub;
                    browser.DocumentText = File.ReadAllText(soub)
                        .Replace("{verze}", Application.ProductVersion)
                        .Replace("{cesta}", System.Reflection.Assembly.GetEntryAssembly().Location)
                        .Replace("Ticketnik.exe", "")
                        .Replace('\\', '/')
                        .Replace("{scrollForeColor}", scrollFore)
                        .Replace("{scrollBackColor}", scrollBack)
                        .Replace("{scrollSeparatorColor}", scrollSeparator)
                        .Replace("{backColor}", back)
                        .Replace("{foreColor}", fore)
                        ;
                }
                else if (url.StartsWith("http"))
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            catch
            {
                CustomControls.MessageBox.Show(form.jazyk.Windows_Help_Nenalezeno);
            }
        }

        private void Expand(string url, TreeNodeCollection nodes)
        {
            foreach(TreeNode n in nodes)
            {

                if(n.Tag == null || n.Tag.ToString() != url)
                {
                    Expand(url, n.Nodes);
                }
                else
                {
                    n.Expand();
                    if(n.Parent != null)
                        ExpandParents(n.Parent);
                    break;
                }
            }
        }
        private void ExpandParents(TreeNode node)
        {
            node.Expand();
            if (node.Parent != null)
                ExpandParents(node.Parent);
        }

        bool zmena;

        private void menuTree_Click(object sender, EventArgs e)
        {
            zmena = false;
        }

        private void menuTree_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                label1.Visible = true;

                StahniHelp();
            }
        }

        private void Napoveda_FormClosing(object sender, FormClosingEventArgs e)
        {
            //form.Logni("Zavírám nápovědu", Form1.LogMessage.INFO);
        }

        bool stisk = false;
        private void menuTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            {
                if (!stisk)
                {
                    stisk = true;
                    label1.Visible = true;
                    menuTree.Nodes.Clear();

                    StahniHelp();
                }
            }
        }

        private void menuTree_KeyUp(object sender, KeyEventArgs e)
        {
            stisk = false;
        }
    }

    [ComVisible(true)]
    public class ScriptManager
    {
        private Napoveda mForm;

        public ScriptManager(Napoveda form)
        {
            mForm = form;
        }

        public void LoadLink(string statusID)
        {
            mForm.LoadLink(statusID);
        }
    }
}
