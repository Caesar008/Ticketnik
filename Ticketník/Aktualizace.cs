using System;
using System.IO;
using System.Windows.Forms;
using fNbt;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Net;
using System.Net.Http;

namespace Ticketník
{
    partial class Form1 : Form
    {
        internal async void Aktualizace(bool force = false)
        {
            updateRunning = true;
            if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));

            infoBox.Text = jazyk.Message_VyhledavamAktualizace;
            try
            {
                XmlDocument updates = new XmlDocument();
                //updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                try
                {
                    //výchozí cesta v síti
                    if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                    {
                        updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                        Logni("Kontroluji updaty na " + Properties.Settings.Default.updateCesta + "\\ticketnik.xml", LogMessage.INFO);
                    }
                    else
                    {
                        try
                        {
                            //WebClient wc = new WebClient();
                            //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", Path.GetTempPath() + "\\ticketnik.xml");
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
                            updates.Load(Path.GetTempPath() + "\\ticketnik.xml");
                            Logni("Kontroluji updaty na " + Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", LogMessage.INFO);
                        }
                        catch (Exception e)
                        {
                            if (Properties.Settings.Default.pouzivatZalozniUpdate)
                            {
                                try
                                {
                                    updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                                    Logni("Kontroluji updaty na " + Properties.Settings.Default.updateCesta + "\\ticketnik.xml", LogMessage.INFO);
                                }
                                catch { Logni("Nelze načíst aktualizační zdroj v síti.", LogMessage.WARNING); }
                            }
                            Logni("Nelze načíst žádný zdroj aktualizací.\r\n" + e.Message, LogMessage.WARNING);
                            //why? Lepší je return. throw new Exception("Nelze vyhledat žádný zdroj aktualizací");
                            return;
                        }
                    }
                }
                catch
                {
                    //backup download z netu
                    try
                    {
                        //WebClient wc = new WebClient();
                        //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", Path.GetTempPath() + "\\ticketnik.xml");
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

                        updates.Load(Path.GetTempPath() + "\\ticketnik.xml");
                        Logni("Kontroluji updaty na " + Properties.Settings.Default.ZalozniUpdate + "/ticketnik.xml", LogMessage.INFO);
                    }
                    catch (Exception e)
                    {
                        if (Properties.Settings.Default.pouzivatZalozniUpdate)
                        {
                            try
                            {
                                updates.Load(Properties.Settings.Default.updateCesta + "\\ticketnik.xml");
                                Logni("Kontroluji updaty na " + Properties.Settings.Default.updateCesta + "\\ticketnik.xml", LogMessage.INFO);
                            }
                            catch { Logni("Nelze načíst aktualizační zdroj v síti.", LogMessage.WARNING); }
                        }
                        Logni("Nelze načíst žádný zdroj aktualizací.\r\n" + e.Message, LogMessage.WARNING);
                        //why? Lepší je return. throw new Exception("Nelze vyhledat žádný zdroj aktualizací");
                        return;
                    }
                }

                if (verze < int.Parse(updates.DocumentElement.SelectSingleNode("Zakosi").InnerText) || force)
                {
                    //DoCommand("copy " + Properties.Settings.Default.updateCesta + "\\zakaznici " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici");
                    try
                    {
                        //výchozí cesta v síti
                        if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                        {
                            File.Copy(Properties.Settings.Default.updateCesta + "\\zakaznici", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici", true);

                            Logni("Stahuji soubor zakaznici z " + Properties.Settings.Default.updateCesta + "\\zakaznici", LogMessage.INFO);
                        }
                        else
                        {
                            try
                            {
                                //WebClient wc = new WebClient();
                                //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/zakaznici", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici");
                                using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                {
                                    AllowAutoRedirect = true
                                }))
                                {
                                    using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/zakaznici").ConfigureAwait(false))
                                    {
                                        using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici", FileMode.Create))
                                        {
                                            await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                        }
                                    }
                                }
                                Logni("Stahuji soubor zakaznici z " + Properties.Settings.Default.ZalozniUpdate + "/zakaznici", LogMessage.INFO);
                            }
                            catch (Exception e)
                            {
                                Logni("Stažení souboru zakaznici selhalo.\r\n" + e.Message, LogMessage.WARNING);
                            }
                        }
                    }
                    catch
                    {
                        //backup download na netu
                        try
                        {
                            //WebClient wc = new WebClient();
                            //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/zakaznici", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici");
                            using (HttpClient hc = new HttpClient(new HttpClientHandler()
                            {
                                AllowAutoRedirect = true
                            }))
                            {
                                using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/zakaznici").ConfigureAwait(false))
                                {
                                    using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici", FileMode.Create))
                                    {
                                        await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                    }
                                }
                            }
                            Logni("Stahuji soubor zakaznici z " + Properties.Settings.Default.ZalozniUpdate + "/zakaznici", LogMessage.INFO);
                        }
                        catch (Exception e)
                        {
                            Logni("Stažení souboru zakaznici selhalo.\r\n" + e.Message, LogMessage.WARNING);
                        }
                    }
                    NbtFile tmpZak = new NbtFile();
                    tmpZak.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici");

                    NbtFile zak = new NbtFile();
                    zak.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici");

                    foreach (NbtTag ncz in tmpZak.RootTag)
                    {
                        if (vcl.IsCancellationRequested)
                            return;
                        if (ncz.TagType == NbtTagType.Compound)
                        {
                            if (!zak.RootTag.Contains(ncz.Name))
                            {
                                zak.RootTag.Add((NbtCompound)ncz.Clone());
                            }
                            else if (zak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtByte>("Velikost").Value != tmpZak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtByte>("Velikost").Value)
                            {
                                zak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtByte>("Velikost").Value = tmpZak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtByte>("Velikost").Value;
                            }

                            if (tmpZak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtString>("Terp") != null)
                            {
                                if (zak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtString>("Terp") == null)
                                    zak.RootTag.Get<NbtCompound>(ncz.Name).Add(new NbtString("Terp", tmpZak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtString>("Terp").Value));
                                else
                                    zak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtString>("Terp").Value = tmpZak.RootTag.Get<NbtCompound>(ncz.Name).Get<NbtString>("Terp").Value;
                            }
                        }
                        else if (ncz.TagType == NbtTagType.List)
                        {
                            if (!zak.RootTag.Contains(ncz.Name))
                            {
                                zak.RootTag.Add((NbtList)ncz.Clone());
                            }
                            else
                            {
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Remove("Task");
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Add((NbtCompound)((NbtCompound)tmpZak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Task").Clone());
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Remove("Typ");
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Add((NbtCompound)((NbtCompound)tmpZak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Typ").Clone());
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Remove("Velikost");
                                ((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Add((NbtCompound)((NbtCompound)tmpZak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Velikost").Clone());

                                if (zak.RootTag["Terpy"][0]["Custom"] == null)
                                    ((NbtCompound)zak.RootTag["Terpy"][0]).Add(new NbtCompound("Custom"));
                                if (zak.RootTag["Terpy"][0]["Custom"]["Task"] == null)
                                    ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]).Add(new NbtList("Task", NbtTagType.String));
                                foreach (NbtString nbs in (NbtList)tmpZak.RootTag["Terpy"][0]["Custom"]["Task"])
                                {
                                    if (vcl.IsCancellationRequested)
                                        return;
                                    bool nal = false;
                                    foreach (NbtString nbss in (NbtList)zak.RootTag["Terpy"][0]["Custom"]["Task"])
                                    {
                                        if (vcl.IsCancellationRequested)
                                            return;
                                        if (nbss.Value == nbs.Value)
                                        {
                                            nal = true;
                                            break;
                                        }
                                    }
                                    if (!nal)
                                        ((NbtList)zak.RootTag["Terpy"][0]["Custom"]["Task"]).Add(new NbtString(nbs.Value));
                                }

                                if (zak.RootTag["Terpy"][0]["Custom"]["Terp"] == null)
                                    ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]).Add(new NbtList("Terp", NbtTagType.String));
                                foreach (NbtString nbs in (NbtList)tmpZak.RootTag["Terpy"][0]["Custom"]["Terp"])
                                {
                                    if (vcl.IsCancellationRequested)
                                        return;
                                    bool nal = false;
                                    foreach (NbtString nbss in (NbtList)zak.RootTag["Terpy"][0]["Custom"]["Terp"])
                                    {
                                        if (vcl.IsCancellationRequested)
                                            return;
                                        if (nbss.Value == nbs.Value)
                                        {
                                            nal = true;
                                            break;
                                        }
                                    }
                                    if (!nal)
                                        ((NbtList)zak.RootTag["Terpy"][0]["Custom"]["Terp"]).Add(new NbtString(nbs.Value));
                                }

                                if (zak.RootTag["Terpy"][0]["Custom"]["TerpPopis"] == null)
                                    ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]).Add(new NbtCompound("TerpPopis"));
                                foreach (NbtString nbs in (NbtCompound)tmpZak.RootTag["Terpy"][0]["Custom"]["TerpPopis"])
                                {
                                    if (vcl.IsCancellationRequested)
                                        return;
                                    if (zak.RootTag["Terpy"][0]["Custom"]["TerpPopis"][nbs.Name] == null)
                                    {
                                        ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]["TerpPopis"]).Add(new NbtString(nbs.Name, nbs.Value));
                                    }
                                    else
                                        ((NbtString)zak.RootTag["Terpy"][0]["Custom"]["TerpPopis"][nbs.Name]).Value = nbs.Value;

                                }
                                if (zak.RootTag["Terpy"][0]["Custom"]["TaskPopis"] == null)
                                    ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]).Add(new NbtCompound("TaskPopis"));
                                foreach (NbtString nbs in (NbtCompound)tmpZak.RootTag["Terpy"][0]["Custom"]["TaskPopis"])
                                {
                                    if (vcl.IsCancellationRequested)
                                        return;
                                    if (zak.RootTag["Terpy"][0]["Custom"]["TaskPopis"][nbs.Name] == null)
                                    {
                                        ((NbtCompound)zak.RootTag["Terpy"][0]["Custom"]["TaskPopis"]).Add(new NbtString(nbs.Name, nbs.Value));
                                    }
                                    else
                                        ((NbtString)zak.RootTag["Terpy"][0]["Custom"]["TaskPopis"][nbs.Name]).Value = nbs.Value;

                                }

                            }
                        }
                    }

                    zak.RootTag.Get<NbtInt>("verze").Value = tmpZak.RootTag.Get<NbtInt>("verze").Value;

                    byte pokusy = 0;

                aktul:
                    if (pokusy < 10)
                    {
                        if (vcl.IsCancellationRequested)
                            return;
                        try
                        {
                            pokusy++;
                            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
                            list = new Zakaznici(this);
                            Logni(jazyk.Message_ZakazniciUpd + " " + zak.RootTag.Get<NbtInt>("verze").Value, LogMessage.INFO);
                        }
                        catch
                        {
                            Thread.Sleep(500);
                            goto aktul;
                        }
                    }
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\_zakaznici");

                    if (!InvokeRequired)
                        LoadFile();
                    else
                        this.BeginInvoke(new Action(() => LoadFile()));
                }

                foreach (XmlNode Njazyk in updates.DocumentElement.SelectSingleNode("Lang").ChildNodes)
                {
                    if (vcl.IsCancellationRequested)
                        return;
                    if (File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml"))
                    {
                        string[] jverze = Njazyk.InnerText.Split('.');
                        XmlDocument preklad = new XmlDocument();
                        preklad.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml");

                        if ((int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("version").InnerText) < int.Parse(jverze[0])) ||
                            (int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("version").InnerText) == int.Parse(jverze[0]) &&
                            int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("revision").InnerText) < int.Parse(jverze[1])))
                        {
                            try
                            {
                                //výchozí cesta v síti
                                if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                                {
                                    File.Copy(Properties.Settings.Default.updateCesta + "\\lang\\" + Njazyk.Name + ".xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml", true);

                                    Logni("Stahuji soubor jazyka z " + Properties.Settings.Default.updateCesta + "\\lang\\" + Njazyk.Name + ".xml", LogMessage.INFO);
                                }
                                else
                                {
                                    try
                                    {
                                        //WebClient wc = new WebClient();
                                        //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml");
                                        using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                        {
                                            AllowAutoRedirect = true
                                        }))
                                        {
                                            using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml").ConfigureAwait(false))
                                            {
                                                using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml", FileMode.Create))
                                                {
                                                    await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                                }
                                            }
                                        }
                                        Logni("Stahuji soubor jazyka z " + Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml", LogMessage.INFO);
                                    }
                                    catch (Exception e)
                                    {
                                        Logni("Stažení aktualizací jazyka selhalo.\r\n" + e.Message, LogMessage.WARNING);
                                    }
                                }
                            }
                            catch
                            {
                                //backup download z netu
                                try
                                {
                                    //WebClient wc = new WebClient();
                                    //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml");
                                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                    {
                                        AllowAutoRedirect = true
                                    }))
                                    {
                                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml").ConfigureAwait(false))
                                        {
                                            using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\" + Njazyk.Name + ".xml", FileMode.Create))
                                            {
                                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                            }
                                        }
                                    }
                                    Logni("Stahuji soubor jazyka z " + Properties.Settings.Default.ZalozniUpdate + "/lang/" + Njazyk.Name + ".xml", LogMessage.INFO);
                                }
                                catch (Exception e)
                                {
                                    Logni("Stažení aktualizací jazyka selhalo.\r\n" + e.Message, LogMessage.WARNING);
                                }
                            }
                            jazyk = new Jazyk();
                            if (!InvokeRequired)
                                jazyk.Reload(this);
                            else
                                this.BeginInvoke(new Action(() => jazyk.Reload(this)));
                            Logni("Jazyk " + Njazyk.Name + " byl updatován.", LogMessage.INFO);
                        }
                    }
                }

                if (program < int.Parse(updates.DocumentElement.SelectSingleNode("App").InnerText))
                {
                    if (DialogResult.Yes == MessageBox.Show(jazyk.Message_NovaVerze, jazyk.Message_Aktualizace, MessageBoxButtons.YesNo))
                    {
                        try
                        {
                            //výchozí cesta v síti
                            if (!Properties.Settings.Default.pouzivatZalozniUpdate)
                            {
                                File.Copy(Properties.Settings.Default.updateCesta + "\\Ticketnik.exe", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"), true);
                                Logni("Stahuji Ticketnik z " + Properties.Settings.Default.updateCesta + "\\Ticketnik.exe", LogMessage.INFO);
                            }
                            else
                            {
                                try
                                {
                                    //WebClient wc = new WebClient();
                                    //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"));
                                    using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                    {
                                        AllowAutoRedirect = true
                                    }))
                                    {
                                        using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe").ConfigureAwait(false))
                                        {
                                            using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"), FileMode.Create))
                                            {
                                                await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                            }
                                        }
                                    }
                                    Logni("Stahuji Ticketnik z " + Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe", LogMessage.INFO);
                                }
                                catch (Exception e)
                                {
                                    Logni("Stažení aktualizace programu selhalo.\r\n" + e.Message, LogMessage.WARNING);
                                }
                            }
                        }
                        catch
                        {
                            //backup download z netu
                            try
                            {
                                //WebClient wc = new WebClient();
                                //wc.DownloadFile(Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe", System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"));
                                using (HttpClient hc = new HttpClient(new HttpClientHandler()
                                {
                                    AllowAutoRedirect = true
                                }))
                                {
                                    using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe").ConfigureAwait(false))
                                    {
                                        using (FileStream fs = new FileStream(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"), FileMode.Create))
                                        {
                                            await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                                        }
                                    }
                                }
                                Logni("Stahuji Ticketnik z " + Properties.Settings.Default.ZalozniUpdate + "/Ticketnik.exe", LogMessage.INFO);
                            }
                            catch (Exception e)
                            {
                                Logni("Stažení aktualizace programu selhalo.\r\n" + e.Message, LogMessage.WARNING);
                            }
                        }

                        Process.Start("_Ticketnik.exe", "/update");

                        if (!InvokeRequired)
                            this.Close();
                        else
                            this.BeginInvoke(new Action(() => this.Close()));
                    }
                }
                infoBox.Text = jazyk.Message_AktualizaceHotova;
            }
            catch (Exception e)
            {
                infoBox.Text = jazyk.Message_AktualizaceSeNezdarila;
                Logni("Aktualizace se nezdařila\r\n\r\n" + e.Message, LogMessage.WARNING);
            }
            finally
            {
                updateRunning = false;
            }

            if (vlaknoTerp != null && !vlaknoTerp.IsAlive)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }
    }
}
