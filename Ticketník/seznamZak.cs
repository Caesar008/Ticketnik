using System;
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
            Motiv.SetMotiv(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.tempZak = (string)zakaznik.SelectedItem;

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
