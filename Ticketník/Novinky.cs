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

            cz.AppendText("Novinky ve verzi 2.3.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@" - Implementován systém KIR (Known Issue Rollback)
    - Online stažení updateru
    - Opraveno ukotvení prvků v okně příloh
    - Přidána položka ""Logy"" v meun programu
    - Opravena chyba zacyklení kvůli zamčenému msedgedriver.exe

    DŮLEŽITÉ!
    V průběhu spojení s MyTime na obrazovce problikne cmd okno a otevře se samostatné okno Edge. Standardně toto okno bude minimalizované, a maximalizuje se jen v případě, že bude potřeba interakce. Občas se může krátce maximalizovat a hned zase minimalizovat.");

            en.AppendText("News in version 2.3.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Implemented system KIR (Known Issue Rollback)
    - Online download of updater
    - Fixed item anchor in attachments window
    - Added item ""Logs"" in program menu
    - Fixed error with deadlock due to locked msedgedriver.exe

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
