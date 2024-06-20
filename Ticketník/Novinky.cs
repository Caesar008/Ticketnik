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

            cz.AppendText("Novinky ve verzi 2.2.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@" - Opravena chyba #22-002
    - Opravena chyba #23-007 (zrušením autosave v předchozí verzi)
    - Opravena chyba #24-004
    - Přepracováno spojení s MyTime
    - Zrušena možnost aktualizace z umístění UNC

    DŮLEŽITÉ!
    V průběhu spojení s MyTime na obrazovce problikne cmd okno a otevře se samostatné okno Edge. Standardně toto okno bude minimalizované, a maximalizuje se jen v případě, že bude potřeba interakce. Občas se může krátce maximalizovat a hned zase minimalizovat.");

            en.AppendText("News in version 2.2.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Fixed #22-002
    - Fixed #23-007 (by removing autousave in last version)
    - Fixed #24-004
    - Reworked connection to MyTime
    - Removed update option from UNC path

    IMPORTANT!
    During connection with MyTime there will show cmd window for short priod of time. Also standalone Edge window will open. By default this window is minimized and will maximize only when interaction is needed. It may maximize and then minimize sometimes.");
            Motiv.SetMotiv(this);
        }

        private void Novinky_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastUpdateNotif = form.program / 10000;
        }
    }
}
