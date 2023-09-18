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

            cz.AppendText("Novinky ve verzi 2.0.0.0\r\n\r\n");
            cz.SelectionStart = 0;
            cz.SelectionLength = cz.TextLength;
            cz.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            cz.AppendText(@"- Podpora světlého a tmavého režimu
    - Ovládací prvky přepsány pro větší kontrolu nad nimi
    - Zmenšení velikosti exe vynecháním knihoven
    - Zrušeno potvrzování změny data ticketu
    - Ctrl+V nyní nastavuje čas začátku na čas vložení místo půlnoci
    - Ctrl+V nyní umí rozpoznat tickety z SM9
    - Automatický upload do MyTime
    - Přidány statusy Zrušeno a Přeřazeno
    - Možnost přidávání příloh k ticketům
    - Oprava chyby #22-003
    - Oprava chyby #23-003
    - Dll knihovny updatovány na novější verze
    - Opravy v překladu");

            en.AppendText("News in version 2.0.0.0\r\n\r\n");
            en.SelectionStart = 0;
            en.SelectionLength = en.TextLength;
            en.SelectionFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold);

            en.AppendText(@" - Support for light and dark mode
    - Controlls has been redone to have more control over them
    - Decreased size of exe by excluding libraries
    - Removed date change confirmation
    - Ctrl+V now sets start time to time of insertion rather than midnight
    - Ctrl+V now recognize tickets from SM9
    - Autoupload to MyTime
    - Added statuses Canceled and Reassigned
    - Abbility to join attachemtns to tickets
    - Fixed error #22-003
    - Fixed error #23-003
    - Dll updated to newer versions
    - Fixes in translation");
            Motiv.SetMotiv(this);
        }

        private void Novinky_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastUpdateNotif = form.program / 10000;
        }
    }
}
