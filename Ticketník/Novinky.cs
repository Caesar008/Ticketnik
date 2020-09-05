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
    public partial class Novinky : Form
    {
        Form1 form;
        public Novinky(Form1 form)
        {
            InitializeComponent();
            this.form = form;

            cz.AppendText("Novinky ve verzi 1.7.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@" - Načítání terpů a tasků z MyTime
 - Terpy a tasky jsou slinkované
 - Přepracován systém výběru terpů a tasků pro ticket");

            en.AppendText("News in version 1.7.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Loading Terp and Task codes from MyTime
 - Terps and tasks are linked together
 - System for setting terp and task for tickets reworked");
        }

        private void Novinky_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastUpdateNotif = form.program / 10000;
        }
    }
}
