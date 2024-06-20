using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

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
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
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

        List<ExportRow> exportRadky = new List<ExportRow>();

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ExportToMyTime = checkBox1.Checked;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.NovyExport)
                Export_Stary();
            else
                exportRadky = Export_Novy();

            if (checkBox1.Checked)
            {
                form.Logni("Nahrávám tickety do MyTime", Form1.LogMessage.INFO);
                UploadToMytime();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FileName != "")
            {
                if (saveFileDialog1.FileName.EndsWith("csv"))
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, outputCSV);
                else if (saveFileDialog1.FileName.EndsWith("xlsx"))
                {
                    File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx", saveFileDialog1.FileName, true);
                    
                }
            }
        }

        private async void UploadToMytime()
        {
            if (checkBox1.Checked)
            {
                if (!InvokeRequired)
                    this.form.timer_ClearInfo.Stop();
                else
                    this.BeginInvoke(new Action(() => form.timer_ClearInfo.Stop()));
                this.form.infoBox.Text = form.jazyk.Message_Uploading;
                this.form.Update();
                try
                {
                    string url = "https://mytime.tietoevry.com/time_cards/" + year + "/week/" + weekNumber + "/import";
                    this.form.Logni("MyTime url: " + url, Form1.LogMessage.INFO);
                    try
                    {
                        if (form.edge.SessionId != null)
                            form.edge.Quit();
                        if (form.edge == null || form.edge.SessionId == null)
                        {
                            form.Logni("Startuji Selenium Edge pro přihlášení k MyTime", Form1.LogMessage.INFO);
                            form.service = EdgeDriverService.CreateDefaultService(form.options);
                            form.service.HideCommandPromptWindow = true;
                            form.edge = new EdgeDriver(form.service, form.options);
                            form.edge.Manage().Window.Minimize();
                            form.edge.Manage().Timeouts().PageLoad.Add(TimeSpan.FromMinutes(5));
                        }

                        form.edge.Navigate().GoToUrl(url);
                        if (form.edge.PageSource.ToLower().Contains("access denied"))
                        {
                            form.Logni("Vyžadováno příhlášení MS", Form1.LogMessage.INFO);
                            form.Logni("Naviguji na \"https://mytime.tietoevry.com/auth/microsoft-identity-platform?button=\"", Form1.LogMessage.INFO);
                            form.edge.Navigate().GoToUrl("https://mytime.tietoevry.com/auth/microsoft-identity-platform?button=");
                            form.edge.Manage().Window.Maximize();

                            while (true)
                            {
                                if (form.edge == null || form.edge.SessionId == null)
                                    break;

                                if (!form.edge.PageSource.Contains("My Time"))
                                {
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(100);
                                }
                                else break;
                            }
                            form.edge.Manage().Window.Minimize();
                            form.Logni("Naviguji na \""+ url +"\"", Form1.LogMessage.INFO);
                            form.edge.Navigate().GoToUrl(url);
                            IWebElement uploadfile = form.edge.FindElement(By.Id("uploaded_file"));
                            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx";
                            uploadfile.SendKeys(filePath);
                            form.Logni("Uploaduji tickety do MyTime.", Form1.LogMessage.INFO);
                            form.edge.FindElement(By.Name("commit")).Click();
                            while (true)
                            {
                                if (form.edge == null || form.edge.SessionId == null)
                                    break;

                                if (!form.edge.PageSource.Contains("<input type=\"submit\" name=\"action_submit\" value=\"Submit\""))
                                {
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(100);
                                }
                                else break;
                            }

                            if (form.edge.PageSource.Contains("<div class=\"flash flash_error\">") || form.edge.PageSource.Contains("<li class=\"row row_with_errors\">"))
                            {
                                form.Logni("Při uploadu do MyTime došlo k problému s některými terpy nebo tasky.", Form1.LogMessage.WARNING);
                                form.edge.Manage().Window.Maximize();
                            }
                            else
                            {
                                form.Logni("Úspěšně nahráno do MyTime, rok " + year + ", týden " + weekNumber + ".", Form1.LogMessage.INFO);
                                DialogResult subConf = CustomControls.MessageBox.Show(form.jazyk.Windows_Export_ConfirmSubmit, form.jazyk.Windows_Export_Nazev, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (subConf == DialogResult.Yes)
                                {
                                    form.edge.Manage().Window.Maximize();
                                }
                                else
                                {
                                    form.Logni("Potvrzuji timecard v MyTime.", Form1.LogMessage.INFO);
                                    var els = form.edge.FindElements(By.Name("action_submit"));
                                    els[els.Count - 1].Click();
                                    form.edge.Quit();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        form.Logni("Při připojování k MyTime došlo k chybě.", Form1.LogMessage.WARNING);
                        form.Logni("Při připojování k MyTime došlo k chybě.\r\n\r\n" + e.Message, Form1.LogMessage.ERROR);
                    }
                }
                catch (Exception ex)
                { //zpráva že failed
                    this.form.Logni("Nahrávání do MyTime selhalo. Rok " + year + ", týden " + weekNumber + ".", Form1.LogMessage.WARNING);
                    this.form.Logni("Nahrávání do MyTime selhalo. Rok " + year + ", týden " + weekNumber + ".\r\n\r\n" + ex.Message + "\r\n" + ex.InnerException, Form1.LogMessage.ERROR);
                    CustomControls.MessageBox.Show(this.form.jazyk.Windows_Export_NahratDoMyTimeFailed, this.form.jazyk.Windows_Export_Nazev, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                form.infoBox.Text = "";
            }
        }
    }
}
