using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int retry = 0;
            string[] toRemove;
            if(File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "ToRemove")))
            {
                toRemove = File.ReadAllLines(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "ToRemove"));
                foreach(string line in toRemove)
                {
                    if (File.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", line)))
                        File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", line));
                }
                File.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "ToRemove"));
            }
            try
            {
                if (Directory.Exists(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "Update")))
                {
                    foreach(string file in Directory.GetFiles(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "Update")))
                    {
                        retry = 0;
                        string fileName = Path.GetFileName(file);
                        while (IsFileLocked(new FileInfo(file)))
                        {
                            if (retry >= 100)
                                throw new IOException("File " + fileName + " cannot be updated - it is being used by another process.");
                            System.Threading.Thread.Sleep(120);
                            retry++;
                        }
                        System.Threading.Thread.Sleep(1000);
                        File.Copy(file, System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", fileName), true);
                    }
                    System.Threading.Thread.Sleep(1000);
                    try
                    {
                        Directory.Delete(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Updater.exe", "Update"), true);
                    }
                    catch
                    {

                    }
                    System.Diagnostics.Process.Start(args[0]);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }
            catch (IOException ex) { MessageBox.Show(ex.StackTrace); }
        }

        static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
