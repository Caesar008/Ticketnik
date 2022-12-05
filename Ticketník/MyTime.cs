using System;
using System.IO;
using System.Windows.Forms;
using fNbt;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Drawing.Imaging;
using System.Linq;

namespace Ticketník
{
    public partial class Form1 : Form
    {
        public HttpClient terpLoaderClient = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            UseDefaultCredentials = true
        });
        string result = "";
        internal NbtFile terpFile;
        System.Windows.Forms.Timer terpTaskFailedRetry = new System.Windows.Forms.Timer();

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
            Logni("Nastavuji IE11", LogMessage.INFO);
            if (!WBEmulator.IsBrowserEmulationSet(this))
            {
                WBEmulator.SetBrowserEmulationVersion(this);
            }
        }

        public async Task<List<MyTimeTerp>> GetAllMyTerps()
        {
            int page = 1;
            result = "";
            List<MyTimeTerp> myTimeTerpList = new List<MyTimeTerp>();
            while (result != "[{\"id\":\"switch_to_all\",\"label\":\"Search in All Projects\",\"tooltip\":\"Search in All Projects\",\"data\":null}]")
            {
                try
                {
                    result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=my&term=&page=" + page).ConfigureAwait(false);
                    if (result.Contains("https://login.microsoftonline.com/jsdisabled"))
                    {
                        MSLogin mslLogin = new MSLogin(terpLoaderClient, this);
                        mslLogin.ShowDialog();
                    }
                }
                catch
                {
                    await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                    result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=my&term=&page=" + page).ConfigureAwait(false);
                }

                JsonTextReader reader = new JsonTextReader(new StringReader(result));
                string tmpId = "", tmpName = "", tmpLabel = "", tmpNumber = "";

                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (tmpId == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                        {
                            reader.Read();
                            if (!reader.Value.ToString().EndsWith("@"))
                                tmpId = reader.Value.ToString();
                        }
                        else if (tmpName == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_name")
                        {
                            reader.Read();
                            tmpName = ((string)reader.Value);
                        }
                        else if (tmpLabel == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                        {
                            reader.Read();
                            tmpLabel = ((string)reader.Value);
                        }
                        else if (tmpNumber == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_number")
                        {
                            reader.Read();
                            if (!reader.Value.ToString().EndsWith("@"))
                                tmpNumber = reader.Value.ToString();
                        }

                        if (tmpId != "" && tmpLabel != "" && (tmpName != "" || !result.Contains("project_name")) && (tmpNumber != "" || !result.Contains("project_number")))
                        {
                            if (tmpLabel != "Search in All Projects")
                            {
                                myTimeTerpList.Add(new MyTimeTerp(tmpId, tmpLabel, tmpName, tmpNumber));
                                tmpId = tmpLabel = tmpName = tmpNumber = "";
                            }
                        }
                    }
                }
                page++;
            }
            for (int i = 0; i < myTimeTerpList.Count; i++)
            {
                myTimeTerpList[i].Tasks = GetTerpTasks(myTimeTerpList[i].ID).Result;
            }

            return myTimeTerpList;
        }

        public async Task<MyTimeTerp> GetTerpData(string terpID)
        {
            try
            {
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=all&term=" + terpID).ConfigureAwait(false);
            }
            catch
            {
                await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/by_number?mode=all&term=" + terpID).ConfigureAwait(false);
            }
            JsonTextReader reader = new JsonTextReader(new StringReader(result));
            string tmpId = "", tmpName = "", tmpLabel = "", tmpNumber = "";
            MyTimeTerp myTimeTerp = null;

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (tmpId == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "id")
                    {
                        reader.Read();
                        tmpId = reader.Value.ToString();
                    }
                    else if (tmpName == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_name")
                    {
                        reader.Read();
                        if(!reader.Value.ToString().EndsWith("@"))
                            tmpName = ((string)reader.Value);
                    }
                    else if (tmpLabel == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "label")
                    {
                        reader.Read();
                        tmpLabel = ((string)reader.Value);
                    }
                    else if (tmpNumber == "" && reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "project_number")
                    {
                        reader.Read();
                        if (!reader.Value.ToString().EndsWith("@"))
                            tmpNumber = reader.Value.ToString();
                    }

                    if (tmpId != "" && tmpLabel != "" && tmpName != "" && (tmpNumber != "" || !result.Contains("project_number")))
                    {
                        myTimeTerp = new MyTimeTerp(tmpId, tmpLabel, tmpName, tmpNumber);
                        myTimeTerp.Tasks = GetTerpTasks(myTimeTerp.ID).Result;
                        tmpId = tmpLabel = tmpName = "";
                    }
                }
            }

            return myTimeTerp;
        }

        public async Task<List<MyTimeTask>> GetTerpTasks(string terpID)
        {
            int page = 1;
            List<MyTimeTask> myTimeTaskList = new List<MyTimeTask>();
            result = "";
            while (result != "[{\"id\":\"switch_to_all\",\"label\":\"Search in All Tasks\",\"data\":null}]")
            {
                try
                {
                    result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=&page=" + page).ConfigureAwait(false);
                }
                catch
                {
                    await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                    result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=&page=" + page).ConfigureAwait(false);
                }

                JsonTextReader reader = new JsonTextReader(new StringReader(result));
                string tmpId = "", tmpName = "", tmpLabel = "";

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
                page++;
            }
            for (int i = 0; i < myTimeTaskList.Count; i++)
            {
                myTimeTaskList[i].TypeLabels = GetTerpTaskTypes(terpID, myTimeTaskList[i].ID).Result;
            }

            return myTimeTaskList;
        }

        public async Task<MyTimeTask> GetTerpTaskData(string terpID, string taskID)
        {
            try
            {
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=" + taskID).ConfigureAwait(false);
            }
            catch
            {
                await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks?mode=my&term=" + taskID).ConfigureAwait(false);
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
            myTimeTask.TypeLabels = GetTerpTaskTypes(terpID, myTimeTask.ID).Result;

            return myTimeTask;
        }

        public async Task<List<string>> GetTerpTaskTypes(string terpID, string taskID)
        {
            try
            {
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=").ConfigureAwait(false);
            }
            catch
            {
                await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=").ConfigureAwait(false);
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

        public async Task<string> GetTerpTaskTypeData(string terpID, string taskID, string typeLabel)
        {
            try
            {
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=" + typeLabel).ConfigureAwait(false);
            }
            catch
            {
                await terpLoaderClient.GetAsync("https://mytime.tietoevry.com/winlogin?utf8=%E2%9C%93&commit=Log+in").ConfigureAwait(false);
                result = await terpLoaderClient.GetStringAsync("https://mytime.tietoevry.com/autocomplete/projects/" + terpID + "/tasks/" + taskID + "/expenditure_types?term=" + typeLabel).ConfigureAwait(false);
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

        public void CreateTerpTaskFile()
        {
            while (updateRunning)
            {
                Thread.Sleep(50);
                Application.DoEvents();
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
                Application.DoEvents();
            }
            try
            {
                Logni("Vytvářím terpTask soubor", Form1.LogMessage.INFO);
                terpFile = new NbtFile(new NbtCompound("Terpy"));
                long lastUpd = DateTime.Now.Ticks;
                terpFile.RootTag.Add(new NbtLong("LastUpdate", lastUpd));

                foreach(MyTimeTerp mtt in GetAllMyTerps().Result)
                {
                    if (mtt.Label.EndsWith("@"))
                        continue;
                    Logni("Ukládám TERP " + mtt.Number + " - " + mtt.Label, Form1.LogMessage.INFO);
                    terpFile.RootTag.Add(new NbtCompound(mtt.Label));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("ID", mtt.ID));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Label", mtt.Label));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Name", mtt.Name == null ? "" : mtt.Name));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Number", mtt.Number));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtCompound("Tasks"));
                    terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtLong("LastUpdate", lastUpd));

                    foreach (MyTimeTask mtta in mtt.Tasks)
                    {
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(mtta.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("ID", mtta.ID));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Label", mtta.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Name", mtta.Name == null ? "" : mtta.Name));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtList("Types", NbtTagType.String));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtLong("LastUpdate", lastUpd));

                        foreach (string mtty in mtta.TypeLabels)
                        {
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtList>("Types").Add(new NbtString(mtty));
                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor vytvořen", Form1.LogMessage.INFO);
                terpTaskFileLock = false;
                LoadTerptaskFile();
            }
            catch (ThreadAbortException)
            {
                Logni("Updatování terpTask souboru bylo přerušeno\r\n", Form1.LogMessage.INFO);
            }
            catch (Exception e)
            {
                Logni("Vytváření terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Vytváření terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
                terpTaskFailedRetry.Start();
            }
            terpTaskFileLock = false;
            if (!updateRunning)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public void UpdateTerpTaskFile()
        {
            while (updateRunning)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
                if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;

            //updatovat tasky podle načtení a přidat ty, co původně jsou uložené navíc
            while (terpTaskFileLock)
            {
                Thread.Sleep(250);
                Application.DoEvents();
            }
            if(!terpTaskFileLock)
                terpTaskFileLock = true;
            try
            {
                Logni("Updatuji terpTask soubor", Form1.LogMessage.INFO);
                terpFile = new NbtFile();
                terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");
                long lastUpd = DateTime.Now.Ticks;

                if(terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                {
                    terpFile.RootTag.Get<NbtLong>("LastUpdate").Value = lastUpd;
                }
                else
                    terpFile.RootTag.Add(new NbtLong("LastUpdate", lastUpd));

                foreach (MyTimeTerp mtt in GetAllMyTerps().Result)
                {
                    if (mtt.Label.EndsWith("@"))
                        continue;
                    Logni("Updatuji TERP " + mtt.Number + " - " + mtt.Label, Form1.LogMessage.INFO);
                    if (terpFile.RootTag.Get<NbtCompound>(mtt.Label) == null)
                    {
                        terpFile.RootTag.Add(new NbtCompound(mtt.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("ID", mtt.ID));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Label", mtt.Label));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Name", mtt.Name == null ? "" : mtt.Name));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtString("Number", mtt.Number));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtLong("LastUpdate", lastUpd));
                        terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtCompound("Tasks"));
                    }
                    else
                    {
                        if (terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtLong>("LastUpdate") != null)
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtLong>("LastUpdate").Value = lastUpd;
                        else
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Add(new NbtLong("LastUpdate", lastUpd));
                    }

                    foreach (MyTimeTask mtta in mtt.Tasks)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label) == null)
                        {
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(mtta.Label));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("ID", mtta.ID));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Label", mtta.Label));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtString("Name", mtta.Name == null ? "" : mtta.Name));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtLong("LastUpdate", lastUpd));
                            terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtList("Types", NbtTagType.String));
                        }
                        else
                        {
                            if(terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtLong>("LastUpdate") != null)
                                terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Get<NbtLong>("LastUpdate").Value = lastUpd;
                            else
                                terpFile.RootTag.Get<NbtCompound>(mtt.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(mtta.Label).Add(new NbtLong("LastUpdate", lastUpd));
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
                    foreach (NbtCompound customTerpy in terpFile.RootTag.Get<NbtCompound>("Custom").Tags.OfType<NbtCompound>())
                    {
                        MyTimeTerp customTerp = GetTerpData(customTerpy.Get<NbtString>("Number").Value).Result;
                        if (customTerp.Label.EndsWith("@"))
                            continue;
                        Logni("Updatuji TERP " + customTerp.Number + " - " + customTerp.Label, Form1.LogMessage.INFO);
                        if(terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate") != null)
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate").Value = lastUpd;
                        else
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtLong("LastUpdate", lastUpd));

                        foreach (MyTimeTask customTask in customTerp.Tasks)
                        {
                            if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label) == null)
                            {
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", lastUpd));
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                            }
                            else
                            {
                                if(terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate") != null)
                                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate").Value = lastUpd;
                                else
                                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", lastUpd));
                            }

                            foreach (string customType in customTask.TypeLabels)
                            {
                                List<string> tmpCheck = new List<string>();
                                foreach (NbtString ns in terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types"))
                                {
                                    tmpCheck.Add(ns.Value);
                                }
                                if (!tmpCheck.Contains(customType))
                                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));
                            }
                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor aktualizován", Form1.LogMessage.INFO);

                terpTaskFileLock = false;
                LoadTerptaskFile();
            }
            catch (ThreadAbortException)
            {
                Logni("Updatování terpTask souboru bylo přerušeno\r\n", Form1.LogMessage.INFO);
            }
            catch (Exception e)
            {
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
                terpTaskFailedRetry.Start();
            }
            terpTaskFileLock = false;

            if (!updateRunning)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public void UpdateTerpTaskFile(string terpNumber)
        {
            while (updateRunning)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;
            //vzít file jak je a updatovat nebo přidat jen task z parametru
            while (terpTaskFileLock)
            {
                Thread.Sleep(250);
                Application.DoEvents();
            }
            terpTaskFileLock = true;

            try
            {
                Logni("Updatuji terpTask soubor, terp " + terpNumber, Form1.LogMessage.INFO);
                terpFile = new NbtFile();
                terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");

                if (terpFile.RootTag.Get<NbtCompound>("Custom") == null)
                    terpFile.RootTag.Add(new NbtCompound("Custom"));

                MyTimeTerp customTerp = GetTerpData(terpNumber).Result;

                int timeout = 300;

                while (customTerp == null)
                {
                    if (timeout-- == 0)
                        throw new Exception("Terp search timed out. Terp probably does not exists.");
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label) == null)
                {
                    terpFile.RootTag.Get<NbtCompound>("Custom").Add(new NbtCompound(customTerp.Label));
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtString("ID", customTerp.ID));
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtString("Label", customTerp.Label));
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtString("Name", customTerp.Name == null ? "" : customTerp.Name));
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtString("Number", customTerp.Number));
                    if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtCompound("Tasks"));
                }
                else
                {
                    if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate") == null)
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                        else
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                    } 
                }

                foreach (MyTimeTask customTask in customTerp.Tasks)
                {
                    if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label) == null)
                    {
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                        if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                    }
                    else
                    {
                        if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                        {
                            if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate") == null)
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                            else
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                        }
                    }


                    foreach (string customType in customTask.TypeLabels)
                    {
                        List<string> tmpCheck = new List<string>();
                        foreach(NbtString ns in terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types"))
                        {
                            tmpCheck.Add(ns.Value);
                        }
                        if (!tmpCheck.Contains(customType))
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));
                    }
                }


                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor aktualizován", Form1.LogMessage.INFO);

                terpTaskFileLock = false;
                LoadTerptaskFile();
            }
            catch(ThreadAbortException)
            {
                Logni("Updatování terpTask souboru bylo přerušeno\r\n", Form1.LogMessage.INFO);
            }
            catch (Exception e)
            {
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
            }
            terpTaskFileLock = false;

            if (!updateRunning)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public void UpdateSelected(string terp)
        {
            while (updateRunning)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
                if (!InvokeRequired)
                timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => timer_ClearInfo.Stop()));
            infoBox.Text = jazyk.Message_TerpUpdate;
            while (terpTaskFileLock)
            {
                Thread.Sleep(250);
                Application.DoEvents();
            }
                terpTaskFileLock = true;

            //tady bude updatování vybraného terpu
            try
            {
                Logni("Updatuji terpTask soubor, terp " + terp, Form1.LogMessage.INFO);
                terpFile = new NbtFile();
                terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");


                MyTimeTerp customTerp = GetTerpData(terp).Result;

                int timeout = 300;

                while(customTerp == null)
                {
                    if (timeout-- == 0)
                        throw new Exception("Terp search timed out. Terp probably does not exists.");
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                if (terpFile.RootTag.Get<NbtCompound>("Custom") != null && terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label) != null)
                {
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtString>("ID").Value = customTerp.ID;
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtString>("Label").Value = customTerp.Label;
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtString>("Name").Value = customTerp.Name == null ? "" : customTerp.Name;
                    if(terpFile.RootTag.Get<NbtLong>("LastUpdate") != null && terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate")!= null)
                        terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                    terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtString>("Number").Value = customTerp.Number;

                    //reset last update
                    foreach (NbtCompound tagsCompound in terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Tags.OfType<NbtCompound>())
                    {
                        if(tagsCompound.Get<NbtLong>("LastUpdate") != null)
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(tagsCompound.Name).Get<NbtLong>("LastUpdate").Value = 0;
                    }

                    foreach (MyTimeTask customTask in customTerp.Tasks)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label) == null)
                        {
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                            if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                            
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                        }
                        else
                        {
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("ID").Value = customTask.ID;
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("Label").Value = customTask.Label;
                            terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("Name").Value = customTask.Name == null ? "" : customTask.Name;
                            if (terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate") != null)
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                            else
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                        }


                        foreach (string customType in customTask.TypeLabels)
                        {
                            List<string> tmpCheck = new List<string>();
                            foreach (NbtString ns in terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types"))
                            {
                                tmpCheck.Add(ns.Value);
                            }
                            if (!tmpCheck.Contains(customType))
                                terpFile.RootTag.Get<NbtCompound>("Custom").Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));

                        }
                    }
                }

                if (terpFile.RootTag.Get<NbtCompound>(customTerp.Label) != null)
                {
                    terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtString>("ID").Value = customTerp.ID;
                    terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtString>("Label").Value = customTerp.Label;
                    terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtString>("Name").Value = customTerp.Name == null ? "" : customTerp.Name;
                    terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtString>("Number").Value = customTerp.Number;
                    if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null && terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate") != null)
                        terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;

                    //reset last update
                    foreach (NbtCompound tagsCompound in terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Tags.OfType<NbtCompound>())
                    {
                        if (tagsCompound.Get<NbtLong>("LastUpdate") != null)
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(tagsCompound.Name).Get<NbtLong>("LastUpdate").Value = 0;
                    }

                    foreach (MyTimeTask customTask in customTerp.Tasks)
                    {
                        if (terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label) == null)
                        {
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Add(new NbtCompound(customTask.Label));
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("ID", customTask.ID));
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Label", customTask.Label));
                            if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                                terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtString("Name", customTask.Name == null ? "" : customTask.Name));
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtList("Types", NbtTagType.String));
                        }
                        else
                        {
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("ID").Value = customTask.ID;
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("Label").Value = customTask.Label;
                            terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtString>("Name").Value = customTask.Name == null ? "" : customTask.Name;
                            if (terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate") != null)
                                terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtLong>("LastUpdate").Value = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                            else
                                terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Add(new NbtLong("LastUpdate", terpFile.RootTag.Get<NbtLong>("LastUpdate").Value));
                        }


                        foreach (string customType in customTask.TypeLabels)
                        {
                            List<string> tmpCheck = new List<string>();
                            foreach (NbtString ns in terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types"))
                            {
                                tmpCheck.Add(ns.Value);
                            }
                            if (!tmpCheck.Contains(customType))
                                terpFile.RootTag.Get<NbtCompound>(customTerp.Label).Get<NbtCompound>("Tasks").Get<NbtCompound>(customTask.Label).Get<NbtList>("Types").Add(new NbtString(customType));

                        }
                    }
                }

                terpFile.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask", NbtCompression.GZip);
                Logni("TerpTask soubor aktualizován", Form1.LogMessage.INFO);

                terpTaskFileLock = false;
                LoadTerptaskFile();
            }
            catch (ThreadAbortException)
            {
                Logni("Updatování terpTask souboru bylo přerušeno\r\n", Form1.LogMessage.INFO);
            }
            catch (Exception e)
            {
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message, Form1.LogMessage.WARNING);
                Logni("Updatování terpTask souboru selhalo\r\n" + e.Message + "\r\n\r\n" + e.StackTrace, Form1.LogMessage.ERROR);
            }

            terpTaskFileLock = false;

            if (!updateRunning)
            {
                if (!InvokeRequired)
                    timer_ClearInfo.Start();
                else
                    this.BeginInvoke(new Action(() => timer_ClearInfo.Start()));
            }
        }

        public Dictionary<string, MyTimeTerp> Terpy { get; private set; }
        public long TerpFileUpdate { get; private set; }
        public void LoadTerptaskFile()
        {
            while (terpTaskFileLock)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            if (!terpTaskFileLock)
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask"))
                {
                    terpTaskFileLock = true;
                    terpFile = new NbtFile();
                    terpFile.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\terpTask");

                    if (terpFile.RootTag.Get<NbtLong>("LastUpdate") != null)
                        TerpFileUpdate = terpFile.RootTag.Get<NbtLong>("LastUpdate").Value;
                    else
                        TerpFileUpdate = 0;

                    foreach (NbtCompound mtt in terpFile.RootTag.Tags.OfType<NbtCompound>())
                    {
                        if (mtt.Name != "Custom")
                        {
                            MyTimeTerp myterp = new MyTimeTerp(mtt.Get<NbtString>("ID").Value, mtt.Get<NbtString>("Label").Value, mtt.Get<NbtString>("Name").Value,
                            mtt.Get<NbtString>("Number").Value, mtt.Get<NbtLong>("LastUpdate")?.Value ?? 0);

                            foreach (NbtCompound mtta in mtt.Get<NbtCompound>("Tasks").Tags.OfType<NbtCompound>())
                            {
                                MyTimeTask mytask = new MyTimeTask(mtta.Get<NbtString>("ID").Value, mtta.Get<NbtString>("Label").Value, mtta.Get<NbtString>("Name").Value, mtta.Get<NbtLong>("LastUpdate")?.Value ?? 0);

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
                    }

                    if (terpFile.RootTag.Get<NbtCompound>("Custom") != null)
                    {
                        foreach (NbtCompound customTerpy in terpFile.RootTag.Get<NbtCompound>("Custom").Tags.OfType<NbtCompound>())
                        {
                            MyTimeTerp myterp = new MyTimeTerp(customTerpy.Get<NbtString>("ID").Value, customTerpy.Get<NbtString>("Label").Value, customTerpy.Get<NbtString>("Name").Value,
                                customTerpy.Get<NbtString>("Number").Value, customTerpy.Get<NbtLong>("LastUpdate")?.Value ?? 0);

                            foreach (NbtCompound mtta in customTerpy.Get<NbtCompound>("Tasks").Tags.OfType<NbtCompound>())
                            {
                                MyTimeTask mytask = new MyTimeTask(mtta.Get<NbtString>("ID").Value, mtta.Get<NbtString>("Label").Value, mtta.Get<NbtString>("Name").Value, mtta.Get<NbtLong>("LastUpdate")?.Value ?? 0);

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

        private void TerpTaskFailedRetry_Tick(object sender, EventArgs e)
        {
            terpTaskFailedRetry.Stop();
            AktualizujTerpyTasky();
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
        public MyTimeTask(string id, string label, string name, long lastUpdate = 0)
        {
            ID = id;
            Label = label;
            Name = name;
            TypeLabels = new List<string>();
            LastUpdate= lastUpdate;
        }
        public List<string> TypeLabels { get; set; }
        public string ID { get; }
        public string Label { get; }
        public string Name { get; }
        public long LastUpdate { get; }
    }

    public class MyTimeTerp
    {
        public MyTimeTerp(string id, string label, string name, string number, long lastUpdate = 0)
        {
            ID = id;
            Label = label;
            Name = name;
            Number = number;
            Tasks = new List<MyTimeTask>();
            LastUpdate = lastUpdate;
        }
        public List<MyTimeTask> Tasks { get; set; }
        public string ID { get; }
        public string Label { get; }
        public string Name { get; }
        public string Number { get; }
        public long LastUpdate { get; }
    }
}

