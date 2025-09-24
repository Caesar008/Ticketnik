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

        };

        public static void Zpracuj(Form1 form)
        {
            if (Registry.CurrentUser.OpenSubKey(RegistryKey) == null)
            {
                form.Logni("Vytvářím registry KIR klíč " + RegistryKey, Form1.LogMessage.INFO);
                Registry.CurrentUser.CreateSubKey(RegistryKey).Close();
            }
            RegistryKey kir = Registry.CurrentUser.OpenSubKey(RegistryKey);
            string[] valueNames = kir.GetValueNames();
            foreach (string name in valueNames)
            {
                if (!KIRList.ContainsKey(name))
                {
                    form.Logni("Odebírám zastaralý KIR klíč " + name, Form1.LogMessage.INFO);
                    Registry.CurrentUser.OpenSubKey(RegistryKey).DeleteValue(name, false);
                }
            }

            try
            {
                form.Logni("Stahuji KIR data z " + Properties.Settings.Default.ZalozniUpdate + "/KIR.xml", Form1.LogMessage.INFO);
                StahniKIR();
                XmlDocument kirXml = new XmlDocument();
                kirXml.Load(Path.GetTempPath() + "\\KIR.xml");
                
                foreach(XmlNode node in kirXml.DocumentElement.SelectNodes("Disable/ID"))
                {
                    if(KIRList.ContainsKey(node.InnerText) && (int)Registry.CurrentUser.OpenSubKey(RegistryKey).GetValue(node.InnerText, 1) == 1)
                    {
                        Registry.CurrentUser.OpenSubKey(RegistryKey).SetValue(node.InnerText, 1, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
                form.Logni("Nebylo možno stáhnout KIR data.", Form1.LogMessage.WARNING);
            }
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
