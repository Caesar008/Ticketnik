using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class KIRForm : Form
    {
        public KIRForm()
        {
            InitializeComponent();
        }
    }

    internal static class KIR
    {
        public static readonly string RegistryKey = "SOFTWARE\\Caesar\\Ticketnik\\KIR";
        public static readonly Dictionary<string, string> KIRList = new Dictionary<string, string>()
        {
            { "ReturnPreklad", "Oprava pádu programu při aktualizaci jazyka, když je zároveň dostupná aktualizace programu" }
        };

        public static void Zpracuj()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DateTime dt;
            if (Registry.CurrentUser.OpenSubKey(RegistryKey) == null)
            {
                dt = DateTime.Now;
                string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "] [INFO] Vytvářím registry KIR klíč " + RegistryKey + "\r\n");
                Registry.CurrentUser.CreateSubKey(RegistryKey).Close();
            }
            RegistryKey kir = Registry.CurrentUser.OpenSubKey(RegistryKey);
            string[] valueNames = kir.GetValueNames();
            foreach (string name in valueNames)
            {
                if (!KIRList.ContainsKey(name))
                {
                    dt = DateTime.Now;
                    string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                    File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "] [INFO] Odebírám zastaralý KIR klíč " + name + "\r\n");
                    Registry.CurrentUser.OpenSubKey(RegistryKey).DeleteValue(name, false);
                }
            }

            try
            {
                dt = DateTime.Now;
                string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "] [INFO] Stahuji KIR data z " + Properties.Settings.Default.ZalozniUpdate + "/KIR.xml\r\n");
                StahniKIR();
                XmlDocument kirXml = new XmlDocument();
                kirXml.Load(Path.GetTempPath() + "\\KIR.xml");
                
                foreach(XmlNode node in kirXml.DocumentElement.SelectNodes("Disable/ID"))
                {
                    if(KIRList.ContainsKey(node.InnerText) && (int)Registry.CurrentUser.OpenSubKey(RegistryKey).GetValue(node.InnerText, 1) == 1)
                    {
                        Registry.CurrentUser.OpenSubKey(RegistryKey).SetValue(node.InnerText, 1, RegistryValueKind.DWord);
                        dt = DateTime.Now;
                        dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                        File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "] [INFO] KIR: Používám rollback na " + node.InnerText + "\r\n");
                    }
                }
            }
            catch
            {
                dt = DateTime.Now;
                string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "] [INFO] Nebylo možno stáhnout KIR data.\r\n");
            }
        }

        internal static bool RollbackActive(string id)
        {
            if (Registry.CurrentUser.OpenSubKey(RegistryKey) == null)
            {
                Zpracuj();
            }
            return (int)Registry.CurrentUser.OpenSubKey(RegistryKey).GetValue(id, 0) == 1;
        }

        private static async void StahniKIR()
        {
            using (HttpClient hc = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = true
            }))
            {
                using (var result = await hc.GetAsync(Properties.Settings.Default.ZalozniUpdate + "/KIR.xml").ConfigureAwait(false))
                {
                    using (FileStream fs = new FileStream(Path.GetTempPath() + "\\KIR.xml", FileMode.Create))
                    {
                        await result.Content.CopyToAsync(fs).ConfigureAwait(false);

                    }
                }
            }
        }
    }
}
