using System;
using System.IO;
using fNbt;
using System.Text;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class ReportWindow : Form
    {
        Form1 form;
        NbtFile reportFile;

        public ReportWindow(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            reportFile = new NbtFile();
            openFileDialog1.Filter = "Crash report|*.report";
            label1.Text = "";
            label2.Text = "";
            Motiv.SetMotiv(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                reportFile.LoadFromFile(openFileDialog1.FileName);
                ticketni_log_text.Text = Encoding.UTF8.GetString(reportFile.RootTag.Get<NbtByteArray>("Ticketnik.log").Value, 0, reportFile.RootTag.Get<NbtByteArray>("Ticketnik.log").Value.Length);
                error_log_text.Text = Encoding.UTF8.GetString(reportFile.RootTag.Get<NbtByteArray>("Error.log").Value, 0, reportFile.RootTag.Get<NbtByteArray>("Error.log").Value.Length);
                user_config_file.Text = Encoding.UTF8.GetString(reportFile.RootTag.Get<NbtByteArray>("user.config").Value, 0, reportFile.RootTag.Get<NbtByteArray>("user.config").Value.Length);
                label2.Text = reportFile.RootTag.Get<NbtString>("user.config cesta").Value;
                label1.Text = reportFile.RootTag.Get<NbtString>("Cesta k tic").Value;
                File.WriteAllBytes(Path.GetTempPath() + "\\TicketnikCrashFile", reportFile.RootTag.Get<NbtByteArray>("Soubor").Value);
                
                form.jmenoSouboru = Path.GetTempPath() + "\\TicketnikCrashFile";
                try
                {
                    form.LoadFile();
                }
                catch (Exception fileEx)
                {
                    CustomControls.MessageBox.Show("Tic soubor je poškozený. Je třeba ho zkontrolovat ručně v NBT editoru.");
                    CustomControls.MessageBox.Show(fileEx.Message);
                }
            }
        }
    }

}
