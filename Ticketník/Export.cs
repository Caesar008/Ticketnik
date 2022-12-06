using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Ticketník
{
    public partial class Export : Form
    {
        Form1 form;
        string outputCSV = "";
        bool over = false;
        public Export(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Today;
            dateTimePicker2.MaxDate = DateTime.Today;
            this.Text = form.jazyk.Windows_Export_Nazev;
            this.button1.Text = form.jazyk.Windows_Export_Exportovat;
            this.radioButton1.Text = form.jazyk.Windows_Export_TentoTyden;
            this.radioButton2.Text = form.jazyk.Windows_Export_MinulyTyden;
            this.radioButton3.Text = form.jazyk.Windows_Export_VybraneObdobi;
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
            if (!Properties.Settings.Default.NovyExport)
                Export_Stary();
            else
                Export_Novy();
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
    }
}
