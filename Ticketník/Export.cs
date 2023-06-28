using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using HtmlAgilityPack;

namespace Ticketník
{
    public partial class Export : Form
    {
        Form1 form;
        string outputCSV = "";
        bool over = false;
        int weekNumber = 0;
        int year = 0;
        public Export(Form1 form)
        {
            this.form = form;
            DoubleBuffered = true;
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;
            this.Text = form.jazyk.Windows_Export_Nazev;
            this.button1.Text = form.jazyk.Windows_Export_Exportovat;
            this.radioButton1.Text = form.jazyk.Windows_Export_TentoTyden;
            this.radioButton2.Text = form.jazyk.Windows_Export_MinulyTyden;
            this.radioButton3.Text = form.jazyk.Windows_Export_VybraneObdobi;
            this.checkBox1.Text = form.jazyk.Windows_Export_NahratDoMyTime;
            if (Properties.Settings.Default.NovyExport)
            {
                checkBox1.Enabled = true;
                //checkBox1.Checked = true;
                this.checkBox1.Checked = Properties.Settings.Default.ExportToMyTime;
            }
            else
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = false;
            }
            Motiv.SetMotiv(this);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked || radioButton2.Checked)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ExportToMyTime = checkBox1.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.NovyExport)
                Export_Stary();
            else
                Export_Novy();
        }

        private async void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FileName != "")
            {
                if (saveFileDialog1.FileName.EndsWith("csv"))
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, outputCSV);
                else if (saveFileDialog1.FileName.EndsWith("xlsx"))
                {
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx", saveFileDialog1.FileName, true);
                    if(checkBox1.Checked)
                    {
                        form.Logni("Nahrávám tickety do MyTime", Form1.LogMessage.INFO);
                        UploadToMytime();
                    }
                }
            }
        }

        private async void UploadToMytime()
        {
            if (checkBox1.Checked)
            {
                try
                {
                    string url = "https://mytime.tietoevry.com/time_cards/" + year + "/week/" + weekNumber + "/import";
                    HttpClient webClient = new HttpClient(new HttpClientHandler()
                    {
                        AllowAutoRedirect = true,
                        UseDefaultCredentials = true
                    });
                    string page = "";
                    try
                    {
                        //test, jestli jsem lognutý, tohle hodí exception, když ne
                        await webClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=my&term=&page=" + page).ConfigureAwait(false);
                        //načtení samotné stránky
                        page = await webClient.GetStringAsync(url).ConfigureAwait(false);
                    }
                    catch
                    {
                        await webClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                        await webClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=my&term=&page=" + page).ConfigureAwait(false);
                        page = await webClient.GetStringAsync(url).ConfigureAwait(false);
                    }

                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(page);
                    HtmlNode form = html.DocumentNode.SelectSingleNode("//form[contains(@id, 'time_card_import_form')]");
                    HtmlNode token = form.SelectSingleNode("//input[contains(@name, 'authenticity_token')]");
                    string tokenValue = token.Attributes["value"].Value;
                    byte[] file = File.ReadAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx");
                    MultipartFormDataContent formData = new MultipartFormDataContent();
                    formData.Add(new StringContent(tokenValue), "authenticity_token");
                    formData.Add(new StringContent("Import"), "commit");
                    formData.Add(new ByteArrayContent(file), "uploaded_file", "tmp_export.xlsx");
                    HttpResponseMessage response = await webClient.PostAsync(url, formData).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();
                    //zpráva že success

                    webClient.Dispose();
                }
                catch 
                { //zpráva že failed
                }
            }
        }
    }
}
