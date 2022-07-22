using System;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class NoveUpozorneni : Form
    {
        Upozorneni u;
        public NoveUpozorneni(Form1 form, Upozorneni u)
        {
            InitializeComponent();
            this.Text = form.jazyk.Windows_Upozorneni_NoveUpozorneni;
            this.label1.Text = form.jazyk.Windows_Upozorneni_DatumACas;
            this.u = u;
            dateTimePicker1.MinDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Hour, DateTime.Now.Minute, 0);
            dateTimePicker1.Value = dateTimePicker1.Value.AddSeconds(-dateTimePicker1.Value.Second);
            this.label2.Text = form.jazyk.Windows_Upozorneni_Typ;
            this.comboBox1.Items.Add(form.jazyk.Windows_Upozorneni_RDP);
            this.comboBox1.Items.Add(form.jazyk.Windows_Upozorneni_Upo);
            this.comboBox1.SelectedItem = form.jazyk.Windows_Upozorneni_Upo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            u.datum = dateTimePicker1.Value;
            u.typ = (string)comboBox1.SelectedItem;
            u.popis = textBox1.Text;
      }
    }
}
