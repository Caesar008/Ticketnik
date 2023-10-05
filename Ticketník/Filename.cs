using System;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class Filename : Form
    {
        Form1 f;
        public Filename(Form1 form)
        {
            InitializeComponent();
            f = form;
            this.label1.Text = form.jazyk.Windows_JmenoSouboru;
            this.Text = form.jazyk.Windows_NovySoubor;
            Motiv.SetMotiv(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.jmenoSouboru = textBox1.Text;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
