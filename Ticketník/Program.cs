using System;
using System.Windows.Forms;
using System.IO;

namespace Ticketník
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            //zabránění spuštění z networku
            if (IsNetworkPath(Application.StartupPath))
            {
                MessageBox.Show("Ticketník is not designed to run from network location. Please copy it locally to your computer.", "Network location detected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
            else
            {
                string par;
                if (args.Length < 1)
                    par = "";
                else
                    par = args[0];
                //tohler je pro dev verze
                //System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "")
                if(Properties.Settings.Default.JazykCesta != "" && !Properties.Settings.Default.JazykCesta.Contains(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "")))
                {
                    Properties.Settings.Default.JazykCesta = System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang\\" + Properties.Settings.Default.Jazyk + ".xml";
                    Properties.Settings.Default.Save();
                }
                if (Application.ProductVersion.EndsWith("dev") && !par.Contains("update"))
                    File.WriteAllBytes(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang\\CZ.xml", Properties.Resources.CZ);

                if (!Directory.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang"))
                    Directory.CreateDirectory(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang");

                if (!File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml"))
                    File.WriteAllBytes(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang\\CZ.xml", Properties.Resources.CZ);

                System.Xml.XmlDocument preklad = new System.Xml.XmlDocument();
                System.Xml.XmlDocument attPreklad = new System.Xml.XmlDocument();
                preklad.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang\\CZ.xml");
                attPreklad.LoadXml(System.Text.Encoding.UTF8.GetString(Properties.Resources.CZ));

                if (int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("version").InnerText) < int.Parse(attPreklad.DocumentElement.Attributes.GetNamedItem("version").InnerText))
                {
                    File.WriteAllBytes(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "").Replace("Ticketnik.exe", "") + "lang\\CZ.xml", Properties.Resources.CZ);
                }

                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (!Directory.Exists(appdata + "\\Ticketnik"))
                    Directory.CreateDirectory(appdata + "\\Ticketnik");

                try
                {
                    if (args.Length < 1)
                        par = "";
                    else
                        par = args[0];
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (!JednaInstance.Bezi(TimeSpan.FromSeconds(5)) && !par.Contains("update"))
                    {
                        
                        if (par.Contains("show"))
                            Properties.Settings.Default.umisteni = new System.Drawing.Point(0, 0);
                        else if (par.Contains("default") || Control.ModifierKeys == Keys.Shift)
                        {
                            try
                            {
                                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                                registryKey.DeleteValue("Ticketnik");
                            }
                            catch { }

                            string filename = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                                Properties.Settings.Default.Reload();
                                System.Diagnostics.Process.Start("Ticketnik.exe");
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                            }
                            else
                            {
                                Properties.Settings.Default.filePath = "";
                                Properties.Settings.Default.lastSelected = "";
                                Properties.Settings.Default.velikost = new System.Drawing.Size(850, 460);
                                Properties.Settings.Default.maximized = false;
                                Properties.Settings.Default.umisteni = new System.Drawing.Point(0, 0);
                                Properties.Settings.Default.minuty = 5;
                                Properties.Settings.Default.autosave = false;
                                Properties.Settings.Default.colPC = 68;
                                Properties.Settings.Default.colID = 82;
                                Properties.Settings.Default.colZak = 84;
                                Properties.Settings.Default.colPop = 92;
                                Properties.Settings.Default.colKon = 89;
                                Properties.Settings.Default.colOd = 49;
                                Properties.Settings.Default.colDo = 49;
                                Properties.Settings.Default.colPau = 44;
                                Properties.Settings.Default.colStav = 60;
                                Properties.Settings.Default.colPoz = 149;
                                Properties.Settings.Default.shortTime = true;
                                Properties.Settings.Default.probiha = System.Drawing.Color.FromArgb(255, 255, 160);
                                Properties.Settings.Default.ceka = System.Drawing.Color.Yellow;
                                Properties.Settings.Default.odpoved = System.Drawing.Color.Yellow;
                                Properties.Settings.Default.rdp = System.Drawing.Color.Yellow;
                                Properties.Settings.Default.vyreseno = System.Drawing.Color.FromArgb(0, 200, 0);
                                Properties.Settings.Default.prescas = System.Drawing.Color.Fuchsia;
                                Properties.Settings.Default.updateCesta = @"\\10.14.18.19\Shareforyou\tools\Ticketnik\Update";
                                Properties.Settings.Default.ZalozniUpdate = "https://github.com/Caesar008/Ticketnik/raw/master/Ticketn%C3%ADk/bin/Release";
                                Properties.Settings.Default.pouzivatZalozniUpdate = true;
                                Properties.Settings.Default.NovyExport = true;
                                Properties.Settings.Default.timeLow = System.Drawing.Color.Red;
                                Properties.Settings.Default.timeMid = System.Drawing.Color.Orange;
                                Properties.Settings.Default.timeLong = System.Drawing.Color.Fuchsia;
                                Properties.Settings.Default.timeOK = System.Drawing.Color.FromArgb(0, 200, 0);
                                Properties.Settings.Default.pouzivatCasy = true;
                                Properties.Settings.Default.Jazyk = "";
                                Properties.Settings.Default.JazykCesta = "";
                                Properties.Settings.Default.colIDPoradi = 2;
                                Properties.Settings.Default.colPCPoradi = 1;
                                Properties.Settings.Default.colZakPoradi = 3;
                                Properties.Settings.Default.colPopPoradi = 4;
                                Properties.Settings.Default.colKonPoradi = 5;
                                Properties.Settings.Default.colTerpPoradi = 6;
                                Properties.Settings.Default.colTaskPoradi = 7;
                                Properties.Settings.Default.colCasPoradi = 8;
                                Properties.Settings.Default.colStavPoradi = 9;
                                Properties.Settings.Default.colPozPoradi = 10;
                                Properties.Settings.Default.onlineTerp = true;
                                Properties.Settings.Default.lastUpdateNotif = 0;
                                Properties.Settings.Default.motiv = 2;

                                Properties.Settings.Default.Save();
                            }
                        }
                        if (System.IO.File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe")))
                        {
                            System.Threading.Thread.Sleep(1000);
                            System.IO.File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "_Ticketnik.exe"));

                        }
                        Application.Run(new Form1());
                    }
                    else if (par.Contains("update"))
                    {
                        int i = 0;
                        while (true)
                            try
                            {
                                if (i < 10)
                                {
                                    System.IO.File.Copy(System.Reflection.Assembly.GetEntryAssembly().Location, System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "Ticketnik.exe"), true);
                                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("_Ticketnik.exe", "Ticketnik.exe"));
                                    Application.Exit();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show(new Jazyk().Error_NejdeZavritAAktualizovat, new Jazyk().Error_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    return;
                                }
                            }
                            catch { i++; System.Threading.Thread.Sleep(1000); }
                    }
                    else
                    {
                        if(!Application.ProductVersion.Contains("dev"))
                            MessageBox.Show(new Jazyk().Error_UzBezi, "Ticketník", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                        {
                            if(DialogResult.Ignore == MessageBox.Show(new Jazyk().Error_UzBezi, "Ticketník", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Exclamation))
                                Application.Run(new Form1());
                        }
                    }
                }
                catch (System.Configuration.ConfigurationErrorsException ex)
                {
                    string filename = ((System.Configuration.ConfigurationErrorsException)ex.InnerException).Filename;

                    if (!Directory.Exists(appdata + "\\Ticketnik"))
                    {
                        Directory.CreateDirectory(appdata + "\\Ticketnik");
                    }
                    if (!Directory.Exists(appdata + "\\Ticketnik\\Logs"))
                    {
                        Directory.CreateDirectory(appdata + "\\Ticketnik\\Logs");
                    }
                    DateTime dt = DateTime.Now;
                    string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond;
                    File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "[" + dat + "] Poškozený soubor nastavení " + filename + "\r\n\r\n");

                    CustomControls.MessageBox.Show("Ticketník found damaged settings file.\r\nSettings have to be reset to default.\r\nProgram will be restarted.\r\n\r\nAll saved tickets will be preserved.\r\nYou will need to only open it File ->Open.", "Damaged settings file");

                    File.Delete(filename);
                    Properties.Settings.Default.Reload();
                    System.Diagnostics.Process.Start("Ticketnik.exe");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    CreateReportFIle();
                }
                catch (System.Reflection.TargetInvocationException te)
                {
                    CustomControls.MessageBox.Show(new Jazyk().Error_DosloKChybe + "\r\n" + new Jazyk().Error_Error + ":\r\n" + te.Message + "\r\n\r\n" + te.StackTrace + "\r\n\r\n" + te.InnerException, new Jazyk().Error_KritickaChyba);

                    if (!Directory.Exists(appdata + "\\Ticketnik"))
                    {
                        Directory.CreateDirectory(appdata + "\\Ticketnik");
                    }
                    if (!Directory.Exists(appdata + "\\Ticketnik\\Logs"))
                    {
                        Directory.CreateDirectory(appdata + "\\Ticketnik\\Logs");
                    }
                    DateTime dt = DateTime.Now;
                    string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                    File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", Application.ProductVersion + "\r\n");
                    File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "[" + dat + "] " + te.Message + "\r\n\r\n" + te.StackTrace + "\r\n\r\n" + te.InnerException + "\r\n\r\n");
                    CreateReportFIle();
                }
                catch (Exception e)
                {
                    if (e.InnerException != null && e.InnerException.Source == "System.Configuration")
                    {
                        string filename = ((System.Configuration.ConfigurationErrorsException)(e.InnerException).InnerException).Filename;
                        if (!Directory.Exists(appdata + "\\Ticketnik"))
                        {
                            Directory.CreateDirectory(appdata + "\\Ticketnik");
                        }
                        if (!Directory.Exists(appdata + "\\Ticketnik\\Logs"))
                        {
                            Directory.CreateDirectory(appdata + "\\Ticketnik\\Logs");
                        }
                        DateTime dt = DateTime.Now;
                        string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond;
                        File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "[" + dat + "] Poškozený soubor nastavení " + filename + "\r\n\r\n");

                        CustomControls.MessageBox.Show("Ticketník found damaged settings file.\r\nSettings have to be reset to default.\r\nProgram will be restarted.\r\n\r\nAll saved tickets will be preserved.\r\nYou will need to only open it File ->Open.", "Damaged settings file");

                        File.Delete(filename);
                        Properties.Settings.Default.Reload();
                        System.Diagnostics.Process.Start("Ticketnik.exe");
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                        CreateReportFIle();
                    }
                    else
                    {
                        CustomControls.MessageBox.Show(new Jazyk().Error_DosloKChybe + "\r\n" + new Jazyk().Error_Error + ":\r\n" + e.Message + "\r\n\r\n" + e.StackTrace + "\r\n\r\n" + e.InnerException, new Jazyk().Error_KritickaChyba);

                        if (!Directory.Exists(appdata + "\\Ticketnik"))
                        {
                            Directory.CreateDirectory(appdata + "\\Ticketnik");
                        }
                        if (!Directory.Exists(appdata + "\\Ticketnik\\Logs"))
                        {
                            Directory.CreateDirectory(appdata + "\\Ticketnik\\Logs");
                        }
                        DateTime dt = DateTime.Now;
                        string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                        File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", Application.ProductVersion + "\r\n");
                        File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "[" + dat + "] " + e.Message + "\r\n\r\n" + e.StackTrace + "\r\n\r\n" + e.InnerException + "\r\n\r\n");
                        CreateReportFIle();
                    }
                }
                finally
                {
                    JednaInstance.UvolniProstredek();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
        }

        public static Boolean IsNetworkPath(String path)
        {
            Uri uri = new Uri(path);
            if (uri.IsUnc)
            {
                return true;
            }
            DriveInfo info = new DriveInfo(path);
            if (info.DriveType == DriveType.Network)
            {
                return true;
            }
            return false;
        }

        public static void CreateReportFIle()
        {
            fNbt.NbtFile reportFile = new fNbt.NbtFile(new fNbt.NbtCompound("report"));
            string ticketniLog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\Logs\\Ticketnik.log";
            string errorLog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\Logs\\Error.log";
            string userConfig = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            reportFile.RootTag.Add(new fNbt.NbtByteArray("Ticketnik.log", File.ReadAllBytes(ticketniLog)));
            reportFile.RootTag.Add(new fNbt.NbtByteArray("Error.log", File.ReadAllBytes(errorLog)));
            reportFile.RootTag.Add(new fNbt.NbtString("user.config cesta", userConfig));
            reportFile.RootTag.Add(new fNbt.NbtString("Cesta k tic", Properties.Settings.Default.filePath));
            reportFile.RootTag.Add(new fNbt.NbtByteArray("Soubor", File.ReadAllBytes(Properties.Settings.Default.filePath)));
            reportFile.RootTag.Add(new fNbt.NbtByteArray("user.config", File.ReadAllBytes(userConfig)));

            reportFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Ticketnik_crash.report", fNbt.NbtCompression.GZip);
        }
    }
}
