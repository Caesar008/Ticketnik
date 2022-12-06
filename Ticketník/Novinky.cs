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

            cz.AppendText("Novinky ve verzi 1.7.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@" - Načítání Terp a tasků z MyTime
    - Opravena chyba 20-006
    - Přepracován způsob pouštění aktualizací na pozadí
    - Upraven systém aktualizací z internetu
    - Defaultně se updatuje z Github jako první, až pak ze sharu
    - Při úspěšném otevření souboru se vytvoří záloha s .bak
    - Zrušen radiobutton Enkrypce a MDM
    - Updatováno na .NET 4.8");

            en.AppendText("News in version 1.7.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Loading Terp and Task codes from MyTime
    - Fixed error 20-006
    - System of updates on background has been reworked
    - Updated system of updates from Internet
    - Updates are primarly downloaded from Github, then from share
    - After successful file opening backup file with .bak is created
    - Removed radiobutton Encryption and MDM
    - Updated to .NET 4.8");
            Motiv.SetMotiv(this);
        }

        private void Novinky_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastUpdateNotif = form.program / 10000;
        }
    }
}
