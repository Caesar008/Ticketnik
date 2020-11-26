using System;
using System.IO;
using System.Windows.Forms;
using fNbt;
using System.Threading;
using Microsoft.Win32;
using System.Security;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ticketník
{
    public partial class Form1 : Form
    {
        WebBrowser terpLoaderBrowser = new WebBrowser();
        bool terpLoaderReady = false;
        string result = "";
        internal NbtFile terpFile;
        //string output = "";

        public void AktualizujTerpyTasky()
        {
            if (vlaknoTerp == null || !vlaknoTerp.IsAlive)
            {
                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask"))
                    vlaknoTerp = new Thread(new ThreadStart(CreateTerpTaskFile));
                else
                    vlaknoTerp = new Thread(new ThreadStart(UpdateTerpTaskFile));
                vlaknoTerp.SetApartmentState(ApartmentState.STA);
                vlaknoTerp.Start();
            }
        }

        public void SetIE()
        {
            Logni("Nastavuji IE11 a JSON", LogMessage.INFO);
            if (!WBEmulator.IsBrowserEmulationSet(this))
            {
                WBEmulator.SetBrowserEmulationVersion(this);
            }
            SetIEJson();

            terpLoaderBrowser.DocumentCompleted += TerpLoaderBrowser_DocumentCompleted;
            terpLoaderBrowser.ScriptErrorsSuppressed = true;
            terpLoaderBrowser.AllowNavigation = true;
        }

        public List<MyTimeTerp> GetAllMyTerps()
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/by_number?mode=my&term="));
            //output += "1\r\n";
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled") || result.Contains("Your session has expired"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                //output += "2\r\n";
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/by_number?mode=my&term="));
                //output += "3\r\n";
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
                //output += result;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            string tmpId = "", tmpName = "", tmpLabel = "", tmpNumber = "";
            List<MyTimeTerp> myTimeTerpList = new List<MyTimeTerp>();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                    {
                        reader.Read();
                        tmpId = reader.Value.ToString();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_name")
                    {
                        reader.Read();
                        tmpName = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        tmpLabel = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_number")
                    {
                        reader.Read();
                        tmpNumber = reader.Value.ToString();
                    }

                    if (tmpId != "" && tmpLabel != "" && tmpName != "" && tmpNumber != "")
                    {
                        myTimeTerpList.Add(new MyTimeTerp(tmpId, tmpLabel, tmpName, tmpNumber));
                        tmpId = tmpLabel = tmpName = tmpNumber = "";
                    }
                }
            }

            for (int i = 0; i < myTimeTerpList.Count; i++)
            {
                myTimeTerpList[i].Tasks = GetTerpTasks(myTimeTerpList[i].ID);
            }

            return myTimeTerpList;
        }

        public MyTimeTerp GetTerpData(string terpID)
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/by_number?mode=all&term=" + terpID));
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled") || result.Contains("Your session has expired"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;

                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/by_number?mode=all&term=" + terpID));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            string tmpId = "", tmpName = "", tmpLabel = "", tmpNumber = "";
            MyTimeTerp myTimeTerp = null;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                    {
                        reader.Read();
                        tmpId = reader.Value.ToString();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_name")
                    {
                        reader.Read();
                        tmpName = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        tmpLabel = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_number")
                    {
                        reader.Read();
                        tmpNumber = reader.Value.ToString();
                    }

                    if (tmpId != "" && tmpLabel != "" && tmpName != "")
                    {
                        myTimeTerp = new MyTimeTerp(tmpId, tmpLabel, tmpName, tmpNumber);
                        myTimeTerp.Tasks = GetTerpTasks(myTimeTerp.ID);
                        tmpId = tmpLabel = tmpName = "";
                    }
                }
            }

            return myTimeTerp;
        }

        public List<MyTimeTask> GetTerpTasks(string terpID)
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term="));
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled") || result.Contains("Your session has expired"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;

                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term="));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            string tmpId = "", tmpName = "", tmpLabel = "";
            List<MyTimeTask> myTimeTaskList = new List<MyTimeTask>();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                    {
                        reader.Read();
                        tmpId = reader.Value.ToString();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "task_description")
                    {
                        reader.Read();
                        tmpName = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        tmpLabel = (string)reader.Value;
                    }

                    if (tmpId != "" && tmpLabel != "" && tmpName != "")
                    {
                        myTimeTaskList.Add(new MyTimeTask(tmpId, tmpLabel, tmpName));
                        tmpId = tmpLabel = tmpName = "";
                    }
                }
            }

            for (int i = 0; i < myTimeTaskList.Count; i++)
            {
                myTimeTaskList[i].TypeLabels = GetTerpTaskTypes(terpID, myTimeTaskList[i].ID);
            }

            return myTimeTaskList;
        }

        public MyTimeTask GetTerpTaskData(string terpID, string taskID)
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=" + taskID));
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled") || result.Contains("Your session has expired"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;

                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=" + taskID));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            string tmpId = "", tmpName = "", tmpLabel = "";
            MyTimeTask myTimeTask = null;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                    {
                        reader.Read();
                        tmpId = reader.Value.ToString();
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "task_description")
                    {
                        reader.Read();
                        tmpName = (string)reader.Value;
                    }
                    else if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        tmpLabel = (string)reader.Value;
                    }

                    if (tmpId != "" && tmpLabel != "" && tmpName != "")
                    {
                        myTimeTask = new MyTimeTask(tmpId, tmpLabel, tmpName);
                        tmpId = tmpLabel = tmpName = "";
                    }
                }
            }
            myTimeTask.TypeLabels = GetTerpTaskTypes(terpID, myTimeTask.ID);

            return myTimeTask;
        }

        public List<string> GetTerpTaskTypes(string terpID, string taskID)
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term="));
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled") || result.Contains("Your session has expired"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;

                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term="));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            
            List<string> myTimeTerpTaskTypeList = new List<string>();

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        myTimeTerpTaskTypeList.Add((string)reader.Value);
                    }
                }
            }

            return myTimeTerpTaskTypeList;
        }

        public string GetTerpTaskTypeData(string terpID, string taskID, string typeLabel)
        {
            terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=" + typeLabel));
            while (!terpLoaderReady)
            {
                Thread.Sleep(100);
            }
            terpLoaderReady = false;

            if (result.Contains("Access denied") || result.Contains("Navigation to the webpage was canceled"))
            {
                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/winlogin?utf8=%E2%9C%93&commit=Log+in"));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;

                terpLoaderBrowser.Navigate(new Uri("https://mytime.tieto.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=" + typeLabel));
                while (!terpLoaderReady)
                {
                    Thread.Sleep(100);
                }
                terpLoaderReady = false;
            }

            JsonTextReader reader = new JsonTextReader(new StringReader(result));

            string myTimeTerpTaskType = "";

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        myTimeTerpTaskType = (string)reader.Value;
                    }
                }
            }

            return myTimeTerpTaskType;
        }

        private void TerpLoaderBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if (e.Url.AbsoluteUri == (sender as WebBrowser).Url.AbsoluteUri)
                {
                    if ((sender as WebBrowser).Document != null)
                    {
                        if ((sender as WebBrowser).Document.Body.All[0].InnerText != null)
                            result = (sender as WebBrowser).Document.Body.All[0].InnerText.Replace("<span>", "").Replace("</span>", "");
                    }
                    terpLoaderReady = true;
                }
            }
            catch (Exception ex)
            {
                Logni("Došlo k chybě při načítání " + e.Url.AbsoluteUri + "\r\n\r\n" + ex.Message, Form1.LogMessage.WARNING);
                terpLoaderReady = true;
            }
        }

        public void SetIEJson()
        {
            if (Registry.CurrentUser.OpenSubKey(@"Software\Classes\MIME\Database\Content Type\application/json") == null || Registry.CurrentUser.OpenSubKey(@"Software\Classes\MIME\Database\Content Type\application/json").GetValue("CLSID") == null || Registry.CurrentUser.OpenSubKey(@"Software\Classes\MIME\Database\Content Type\application/json").GetValue("CLSID").ToString() != "{25336920-03F9-11cf-8FD0-00AA00686F13}")
            {
                Logni("Nastavuji JSON", LogMessage.INFO);
                RegistryKey rKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\MIME\Database\Content Type\application/json");
                rKey.SetValue("Extension", ".json");
                rKey.SetValue("CLSID", "{25336920-03F9-11cf-8FD0-00AA00686F13}");
                rKey.SetValue("Encoding", 0x08000000, RegistryValueKind.DWord);
                Logni("IE nastaven na čtení JSON, klíč HKCU\\Software\\Classes\\MIME\\Database\\Content Type\\application/json", Form1.LogMessage.INFO);
            }
        }

        public void CreateTerpTaskFile()
        {
            while (vlakno != null && vlakno.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
            {
                Thread.Sleep(50);
            }
            if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;

            if (!terpTaskFileLock)
                terpTaskFileLock = true;
            else
            {
                Thread.Sleep(250);
            }
            try
            {
                Logni("Vytvářím terpTask soubor", Form1.LogMessage.INFO);
                terpFile = new NbtFile(new NbtCompound("Terpy"));

                foreach(MyTimeTerp mtt in GetAllMyTerps())
                {
                    terpFile.RootTag.Add(new NbtCompound(mtt.Label));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("ID", mtt.ID));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Label", mtt.Label));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Name", mtt.Name == null ? "" : mtt.Name));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Number", mtt.Number));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtCompound("Tasks"));

                    foreach (MyTimeTask mtta in mtt.Tasks)
                    {
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(mtta.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("ID", mtta.ID));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Label", mtta.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Name", mtta.Name == null ? "" : mtta.Name));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtList("Types", NbtTagType.String));

                        foreach (string mtty in mtta.TypeLabels)
                        {
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtList>("Types").Add(new NbtString(mtty));
                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor vytvořen", Form1.LogMessage.INFO);
                LoadTerptaskFile();
            }
            catch (Exception e)
            {
                Logni("Vytváření terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Vytváření terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
                /*terpFile.SaveToFile("DebugterpTask", NbtCompression.GZip);
                File.WriteAllText("result.txt", result + output);*/
            }
            terpTaskFileLock = false;
            if (vlakno.Status != System.Threading.Tasks.TaskStatus.Running || vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingForActivation ||
                vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingForChildrenToComplete || vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingToRun)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public void UpdateTerpTaskFile()
        {
            while (vlakno != null && vlakno.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
                Thread.Sleep(50);
            if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;

            //updatovat tasky podle načtení a přidat ty, co původně jsou uložené navíc
            while (terpTaskFileLock)
                Thread.Sleep(250);
            try
            {
                Logni("Updatuji terpTask soubor", Form1.LogMessage.INFO);
                terpFile = new NbtFile();
                terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");

                foreach (MyTimeTerp mtt in GetAllMyTerps())
                {
                    if (terpFile.RootTag.Get<NbtCompound>(mtt.Label) == null)
                    {
                        terpFile.RootTag.Add(new NbtCompound(mtt.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("ID", mtt.ID));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Label", mtt.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Name", mtt.Name == null ? "" : mtt.Name));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Number", mtt.Number));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtCompound("Tasks"));
                    }

                    foreach (MyTimeTask mtta in mtt.Tasks)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label) == null)
                        {
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(mtta.Label));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("ID", mtta.ID));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Label", mtta.Label));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Name", mtta.Name == null ? "" : mtta.Name));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtList("Types", NbtTagType.String));
                        }

                        foreach (string mtty in mtta.TypeLabels)
                        {
                            bool found = false;
                            foreach (NbtString mttys in terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtList>("Types"))
                            {
                                if (mttys.Value == mtty)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtList>("Types").Add(new NbtString(mtty));
                        }
                    }
                }

                if (terpFile.RootTag.Get<NbtCompound>("Custom") != null)
                {
                    foreach (NbtCompound customTerpy in terpFile.RootTag.Get<NbtCompound>("Custom").Tags)
                    {
                        MyTimeTerp customTerp = GetTerpData(customTerpy.Get<NbtString>("ID").Value);
                        foreach (MyTimeTask customTask in customTerp.Tasks)
                        {
                            if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>(customTask.Label) == null)
                            {
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                            }

                            foreach(string customType in customTask.TypeLabels)
                            {
                                if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types")[customType] == null)
                                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));
                            }
                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip); 
                Logni("TerpTask soubor aktualizován", Form1.LogMessage.INFO);

                LoadTerptaskFile();
            }
            catch (Exception e)
            {
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
            }
            terpTaskFileLock = false;

            if (vlakno.Status != System.Threading.Tasks.TaskStatus.Running || vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingForActivation ||
                vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingForChildrenToComplete || vlakno.Status != System.Threading.Tasks.TaskStatus.WaitingToRun ||
                vlakno.Status != System.Threading.Tasks.TaskStatus.Created)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public void UpdateTerpTaskFile(string terpNumber)
        {
            while (vlakno != null && vlakno.Status != System.Threading.Tasks.TaskStatus.RanToCompletion)
                Thread.Sleep(50);
            if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;
            //vzít file jak je a updatovat nebo přidat jen task z parametru
            while (terpTaskFileLock)
                Thread.Sleep(250);
            terpTaskFileLock = true;

            try
            {
                Logni("Updatuji terpTask soubor, terp " + terpNumber, Form1.LogMessage.INFO);
                terpFile = new NbtFile();
                terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");

                if (terpFile.RootTag.Get<NbtCompound>("Custom") == null)
                    terpFile.RootTag.Add(new NbtCompound("Custom"));

                //file.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Number", mtt.Number));

                foreach (NbtCompound customTerpy in terpFile.RootTag.Get<NbtCompound>("Custom").Tags)
                {
                    if (customTerpy.Get<NbtString>("Number").Value != terpNumber)
                        continue;

                    MyTimeTerp customTerp = GetTerpData(terpNumber);
                    foreach (MyTimeTask customTask in customTerp.Tasks)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>(customTask.Label) == null)
                        {
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                        }

                        foreach (string customType in customTask.TypeLabels)
                        {
                            if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types")[customType] == null)
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));
                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor aktualizován", Form1.LogMessage.INFO);
            }
            catch (Exception e)
            {
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
            }
            terpTaskFileLock = false;
        }

        public Dictionary<string, MyTimeTerp> Terpy { get; private set; }
        public void LoadTerptaskFile()
        {
            while (terpTaskFileLock)
                Thread.Sleep(50);
            if (!terpTaskFileLock)
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask"))
                {
                    terpTaskFileLock = true;
                    terpFile = new NbtFile();
                    terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");

                    foreach (NbtCompound mtt in terpFile.RootTag.Tags)
                    {
                        MyTimeTerp myterp = new MyTimeTerp(mtt.Get<NbtString>("ID").Value, mtt.Get<NbtString>("Label").Value, mtt.Get<NbtString>("Name").Value,
                                mtt.Get<NbtString>("Number").Value);

                        foreach (NbtCompound mtta in mtt.Get<NbtCompound>("Tasks").Tags)
                        {
                            MyTimeTask mytask = new MyTimeTask(mtta.Get<NbtString>("ID").Value, mtta.Get<NbtString>("Label").Value, mtta.Get<NbtString>("Name").Value);

                            foreach (NbtString mtty in mtta.Get<NbtList>("Types"))
                            {
                                mytask.TypeLabels.Add(mtty.Value);
                            }
                            myterp.Tasks.Add(mytask);
                        }

                        if (!Terpy.ContainsKey(mtt.Get<NbtString>("Label").Value))
                            Terpy.Add(myterp.Label, myterp);
                        else
                            Terpy[myterp.Label] = myterp;
                    }

                    if (terpFile.RootTag.Get<NbtCompound>("Custom") != null)
                    {
                        foreach (NbtCompound customTerpy in terpFile.RootTag.Get<NbtCompound>("Custom").Tags)
                        {
                            MyTimeTerp myterp = new MyTimeTerp(customTerpy.Get<NbtString>("ID").Value, customTerpy.Get<NbtString>("Label").Value, customTerpy.Get<NbtString>("Name").Value,
                                customTerpy.Get<NbtString>("Number").Value);

                            foreach (NbtCompound mtta in customTerpy.Get<NbtCompound>("Tasks").Tags)
                            {
                                MyTimeTask mytask = new MyTimeTask(mtta.Get<NbtString>("ID").Value, mtta.Get<NbtString>("Label").Value, mtta.Get<NbtString>("Name").Value);

                                foreach (NbtString mtty in mtta.Get<NbtList>("Types"))
                                {
                                    mytask.TypeLabels.Add(mtty.Value);
                                }
                                myterp.Tasks.Add(mytask);
                            }

                            if (!Terpy.ContainsKey(customTerpy.Get<NbtString>("Label").Value))
                                Terpy.Add(myterp.Label, myterp);
                            else
                                Terpy[myterp.Label] = myterp;
                        }
                    }

                    terpTaskFileLock = false;
                }
            }
        }
    }

    //pro nejnovější IE místo defaultního IE7
    public enum BrowserEmulationVersion
    {
        Default = 0,
        Version7 = 7000,
        Version8 = 8000,
        Version8Standards = 8888,
        Version9 = 9000,
        Version9Standards = 9999,
        Version10 = 10000,
        Version10Standards = 10001,
        Version11 = 11000,
        Version11Edge = 11001
    }

    public static class WBEmulator
    {
        private const string InternetExplorerRootKey = @"Software\Microsoft\Internet Explorer";

        public static int GetInternetExplorerMajorVersion(Form1 form)
        {
            int result;

            result = 0;

            try
            {
                RegistryKey key;

                key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey);

                if (key != null)
                {
                    object value;

                    value = key.GetValue("svcVersion", null) ?? key.GetValue("Version", null);

                    if (value != null)
                    {
                        string version;
                        int separator;

                        version = value.ToString();
                        separator = version.IndexOf('.');
                        if (separator != -1)
                        {
                            int.TryParse(version.Substring(0, separator), out result);
                        }
                    }
                }
            }
            catch (SecurityException)
            {
                form.Logni("Selhalo čtení registrů pro IE verzi", Form1.LogMessage.WARNING);
            }
            catch (UnauthorizedAccessException)
            {
                form.Logni("Chybí práva čtení registrů pro IE verzi", Form1.LogMessage.WARNING);
            }

            return result;
        }
        private const string BrowserEmulationKey = InternetExplorerRootKey + @"\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

        public static BrowserEmulationVersion GetBrowserEmulationVersion(Form1 form)
        {
            BrowserEmulationVersion result;

            result = BrowserEmulationVersion.Default;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);
                if (key != null)
                {
                    string programName;
                    object value;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                    value = key.GetValue(programName, null);

                    if (value != null)
                    {
                        result = (BrowserEmulationVersion)Convert.ToInt32(value);
                    }
                }
            }
            catch (SecurityException)
            {
                form.Logni("Selhalo čtení registrů pro IE verzi", Form1.LogMessage.WARNING);
            }
            catch (UnauthorizedAccessException)
            {
                form.Logni("Chybí práva čtení registrů pro IE verzi", Form1.LogMessage.WARNING);
            }

            return result;
        }
        public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion, Form1 form)
        {
            bool result;

            result = false;

            try
            {
                RegistryKey key;

                key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, true);

                if (key != null)
                {
                    string programName;

                    programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

                    if (browserEmulationVersion != BrowserEmulationVersion.Default)
                    {
                        // if it's a valid value, update or create the value
                        key.SetValue(programName, (int)browserEmulationVersion, RegistryValueKind.DWord);
                        form.Logni("IE pro Ticketník nastaveno na verzi " + (int)browserEmulationVersion, Form1.LogMessage.INFO);
                    }
                    else
                    {
                        // otherwise, remove the existing value
                        key.DeleteValue(programName, false);
                    }

                    result = true;
                }
            }
            catch (SecurityException)
            {
                form.Logni("Selhal zápis registrů pro IE verzi", Form1.LogMessage.WARNING);
            }
            catch (UnauthorizedAccessException)
            {
                form.Logni("Chybí práva zápisu registrů pro IE verzi", Form1.LogMessage.WARNING);
            }

            return result;
        }

        public static bool SetBrowserEmulationVersion(Form1 form)
        {
            int ieVersion;
            BrowserEmulationVersion emulationCode;

            ieVersion = GetInternetExplorerMajorVersion(form);

            if (ieVersion >= 11)
            {
                emulationCode = BrowserEmulationVersion.Version11;
            }
            else
            {
                switch (ieVersion)
                {
                    case 10:
                        emulationCode = BrowserEmulationVersion.Version10;
                        break;
                    case 9:
                        emulationCode = BrowserEmulationVersion.Version9;
                        break;
                    case 8:
                        emulationCode = BrowserEmulationVersion.Version8;
                        break;
                    default:
                        emulationCode = BrowserEmulationVersion.Version7;
                        break;
                }
            }

            return SetBrowserEmulationVersion(emulationCode, form);
        }
        public static bool IsBrowserEmulationSet(Form1 form)
        {
            return GetBrowserEmulationVersion(form) != BrowserEmulationVersion.Default;
        }
    }


    public class MyTimeTask
    {
        public MyTimeTask(string id, string label, string name)
        {
            ID = id;
            Label = label;
            Name = name;
            TypeLabels = new List<string>();
        }
        public List<string> TypeLabels { get; set; }
        public string ID { get; }
        public string Label { get; }
        public string Name { get; }
    }

    public class MyTimeTerp
    {
        public MyTimeTerp(string id, string label, string name, string number)
        {
            ID = id;
            Label = label;
            Name = name;
            Number = number;
            Tasks = new List<MyTimeTask>();
        }
        public List<MyTimeTask> Tasks { get; set; }
        public string ID { get; }
        public string Label { get; }
        public string Name { get; }
        public string Number { get; }
    }
}

