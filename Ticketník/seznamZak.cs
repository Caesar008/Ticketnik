using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class SeznamZak : Form
    {
        Form1 form;
        public SeznamZak(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            this.Text = form.jazyk.Header_Zakaznik;
            foreach (string s in form.list.DejZakazniky().Keys)
            {
                zakaznik.Items.Add(s);
                zakaznik.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.tempZak = (string)zakaznik.SelectedItem;

        }
    }
}
