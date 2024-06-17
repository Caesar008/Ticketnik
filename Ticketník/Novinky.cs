using System.Drawing;
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

            cz.AppendText("Novinky ve verzi 2.1.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@" - Opravena chyba #24-001
    - Opravena chyba #24-002
    - Opravena chyba #24-003
    - Opravena chyba #23-006
    - Odstraněn autosave");

            en.AppendText("News in version 2.1.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Fixed #24-001
    - Fixed #24-002
    - Fixed #24-003
    - Fixed #23-006
    - Removed autosave");
            Motiv.SetMotiv(this);
        }

        private void Novinky_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastUpdateNotif = form.program / 10000;
        }
    }
}
