using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using fNbt;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace Ticketník
{
    //udělat tlačítka ve správci jazyka opět viditelná

    /*interní changelog 1.7.0.8
    - Do seznamu bylo přidáváno i "Search in All Projects"
    */

    public partial class Form1 : Form
    {
        internal Jazyk jazyk = new Jazyk();
        //Skryté věci - Report + skryté nastavení
        internal bool devtest = false;

        internal readonly int saveFileVersion = 10101, langVersion = 6;
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal string jmenoSouboru = "";
        internal string zakaznik = "";
        internal NbtFile file;
        List<NbtCompound> zakaznici = new List<NbtCompound>();
        bool ulozeno = true;
        Dictionary<string, List<Ticket>> tickety = new Dictionary<string, List<Ticket>>();
        internal SortedDictionary<DateTime, Dictionary<string, List<Ticket>>> poDnech = new SortedDictionary<DateTime, Dictionary<string, List<Ticket>>>();
        internal string vybranyMesic = "leden";
        long maxID = 0;
        //internal Task vlakno;
        internal CancellationTokenSource vlaknoCancel;
        internal CancellationToken vcl;
        internal Thread vlaknoTerp;
        internal Zakaznici list;
        internal byte velikost = 0;
        internal int posledniVybrany = 0;
        internal string tempZak = "";
        internal int program = 1070007;
        internal int verze = 0;
        NbtCompound copy = null;
        internal string zakaznikVlozit = "";
        internal bool helpOpen = false;
        bool muze = false;
        internal string zakaznikTerp = "";
        internal List<UpozorneniCls> upozorneni = new List<UpozorneniCls>(UpozorneniCls.UpozorneniList);
        internal bool upozozrneniMuze = true;
        internal bool terpTaskFileLock = false;
        internal bool updateRunning = false;

        public Form1()
        {
            CheckLog();

            //při updatu na 1.7 zapnout záložní update pro defaultní stahování z githubu
            if (Properties.Settings.Default.lastUpdateNotif < 107 && !Properties.Settings.Default.pouzivatZalozniUpdate)
            {
                Properties.Settings.Default.pouzivatZalozniUpdate = true;
                Properties.Settings.Default.ZalozniUpdate = "https://github.com/Caesar008/Ticketnik/raw/master/Ticketn%C3%ADk/bin/Release";
                Properties.Settings.Default.Save();
            }

            this.Location = Properties.Settings.Default.umisteni;
            
            canChange = false;

            InitializeComponent();
            infoBox.Text = "";
            SetJazyk();

            //schování tlačítek v menu
            toolStripMenuItem6.Visible = false;
            reportToolStripMenuItem.Visible = false;
            oddToolStripMenuItem.Visible = false;
            převéstNaFormátMilleniumToolStripMenuItem.Visible = false;
            rokVyber.Enabled = false;
            //dostupnéJazykyToolStripMenuItem.Visible = false;
            terpTaskFailedRetry.Tick += TerpTaskFailedRetry_Tick;
            terpTaskFailedRetry.Interval = 600000;
            terpToolStripMenuItem.Visible = přidatTERPKódToolStripMenuItem.Visible = upravitTERPKódToolStripMenuItem.Visible = smazatTERPKódToolStripMenuItem.Visible = !Properties.Settings.Default.onlineTerp;
            dokumentaceToolStripMenuItem.Visible = false;

            //vytvoření cancelation tokenu
            vlaknoCancel = new CancellationTokenSource();
            CancellationToken vcl = vlaknoCancel.Token;

            AddColumns();
            canChange = true;
            NastavSirku();
            list = new Zakaznici(this);
            if (!Directory.Exists(appdata + "\\Ticketnik"))
            {
                Directory.CreateDirectory(appdata + "\\Ticketnik");
            }
            if (Properties.Settings.Default.velikost.Height != 0 && Properties.Settings.Default.velikost.Width != 0)
                this.Size = Properties.Settings.Default.velikost;
            this.WindowState = Properties.Settings.Default.maximized ? FormWindowState.Maximized : FormWindowState.Normal;
            jmenoSouboru = Properties.Settings.Default.filePath;

            try
            {
                LoadFile(true);
            }
            catch
            {
                Logni("Soubor .tic je poškozen. " + jmenoSouboru, LogMessage.WARNING);
                MessageBox.Show(jazyk.Error_DamagedTicFile, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Aktualizace(devtest);

            //nastavení IE11 pro WebBrowser + nastavení JSON pro IE
            SetIE();
            AktualizujTerpyTasky();

            Autosave();

            if (!IsOnScreen(this))
            {
                Logni(jazyk.Message_WindowMoved + " (" + this.Location.X + ";" + this.Location.Y + ").", LogMessage.INFO);
                Properties.Settings.Default.umisteni = new System.Drawing.Point(0, 0);
                this.Location = new Point(0, 0);
            }

            timerUpozorneni.Start();

            //zobrazení novinek v nové verzi
            if(program / 10000 > Properties.Settings.Default.lastUpdateNotif)
            {
                Novinky n = new Novinky(this);
                n.StartPosition = FormStartPosition.Manual;
                n.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                n.Show();
                n.TopMost = true;
                n.BringToFront();
            }

            Terpy = new Dictionary<string, MyTimeTerp>();
            //načtení terpů
            LoadTerptaskFile();
        }

        private void NastavSirku()
        {
            canChange = false;
            foreach (TabPage tp in tabControl1.Controls)
            {
                if (tp.Controls.ContainsKey("leden"))
                {
                    ((ListView)tp.Controls["leden"]).BeginUpdate();
                    ((ListView)tp.Controls["leden"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["leden"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["leden"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["leden"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["leden"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["leden"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["leden"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["leden"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["leden"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["leden"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["leden"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["leden"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["leden"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("unor"))
                {
                    ((ListView)tp.Controls["unor"]).BeginUpdate();
                    ((ListView)tp.Controls["unor"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["unor"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["unor"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["unor"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["unor"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["unor"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["unor"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["unor"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["unor"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["unor"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["unor"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["unor"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["unor"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("brezen"))
                {
                    ((ListView)tp.Controls["brezen"]).BeginUpdate();
                    ((ListView)tp.Controls["brezen"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["brezen"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["brezen"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["brezen"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["brezen"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["brezen"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["brezen"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["brezen"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["brezen"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["brezen"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["brezen"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["brezen"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["brezen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("duben"))
                {
                    ((ListView)tp.Controls["duben"]).BeginUpdate();
                    ((ListView)tp.Controls["duben"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["duben"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["duben"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["duben"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["duben"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["duben"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["duben"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["duben"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["duben"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["duben"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["duben"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["duben"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["duben"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("kveten"))
                {
                    ((ListView)tp.Controls["kveten"]).BeginUpdate();
                    ((ListView)tp.Controls["kveten"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["kveten"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["kveten"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["kveten"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["kveten"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["kveten"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["kveten"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["kveten"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["kveten"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["kveten"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["kveten"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["kveten"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["kveten"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("cerven"))
                {
                    ((ListView)tp.Controls["cerven"]).BeginUpdate();
                    ((ListView)tp.Controls["cerven"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["cerven"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["cerven"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["cerven"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["cerven"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["cerven"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["cerven"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["cerven"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["cerven"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["cerven"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["cerven"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["cerven"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["cerven"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("cervenec"))
                {
                    ((ListView)tp.Controls["cervenec"]).BeginUpdate();
                    ((ListView)tp.Controls["cervenec"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["cervenec"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["cervenec"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["cervenec"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["cervenec"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["cervenec"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["cervenec"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["cervenec"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["cervenec"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["cervenec"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["cervenec"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["cervenec"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["cervenec"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("srpen"))
                {
                    ((ListView)tp.Controls["srpen"]).BeginUpdate();
                    ((ListView)tp.Controls["srpen"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["srpen"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["srpen"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["srpen"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["srpen"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["srpen"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["srpen"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["srpen"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["srpen"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["srpen"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["srpen"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["srpen"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["srpen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("zari"))
                {
                    ((ListView)tp.Controls["zari"]).BeginUpdate();
                    ((ListView)tp.Controls["zari"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["zari"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["zari"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["zari"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["zari"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["zari"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["zari"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["zari"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["zari"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["zari"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["zari"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["zari"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["zari"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("rijen"))
                {
                    ((ListView)tp.Controls["rijen"]).BeginUpdate();
                    ((ListView)tp.Controls["rijen"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["rijen"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["rijen"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["rijen"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["rijen"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["rijen"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["rijen"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["rijen"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["rijen"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["rijen"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["rijen"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["rijen"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["rijen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("listopad"))
                {
                    ((ListView)tp.Controls["listopad"]).BeginUpdate();
                    ((ListView)tp.Controls["listopad"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["listopad"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["listopad"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["listopad"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["listopad"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["listopad"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["listopad"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["listopad"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["listopad"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["listopad"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["listopad"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["listopad"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["listopad"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("prosinec"))
                {
                    ((ListView)tp.Controls["prosinec"]).BeginUpdate();
                    ((ListView)tp.Controls["prosinec"]).Columns[1].Width = Properties.Settings.Default.colPC;
                    ((ListView)tp.Controls["prosinec"]).Columns[1].DisplayIndex = Properties.Settings.Default.colPCPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[2].Width = Properties.Settings.Default.colID;
                    ((ListView)tp.Controls["prosinec"]).Columns[2].DisplayIndex = Properties.Settings.Default.colIDPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[3].Width = Properties.Settings.Default.colZak;
                    ((ListView)tp.Controls["prosinec"]).Columns[3].DisplayIndex = Properties.Settings.Default.colZakPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[4].Width = Properties.Settings.Default.colPop;
                    ((ListView)tp.Controls["prosinec"]).Columns[4].DisplayIndex = Properties.Settings.Default.colPopPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[5].Width = Properties.Settings.Default.colKon;
                    ((ListView)tp.Controls["prosinec"]).Columns[5].DisplayIndex = Properties.Settings.Default.colKonPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[6].Width = Properties.Settings.Default.colOd;
                    ((ListView)tp.Controls["prosinec"]).Columns[6].DisplayIndex = Properties.Settings.Default.colTerpPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[7].Width = Properties.Settings.Default.colDo;
                    ((ListView)tp.Controls["prosinec"]).Columns[7].DisplayIndex = Properties.Settings.Default.colTaskPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[8].Width = Properties.Settings.Default.colPau;
                    ((ListView)tp.Controls["prosinec"]).Columns[8].DisplayIndex = Properties.Settings.Default.colCasPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[9].Width = Properties.Settings.Default.colStav;
                    ((ListView)tp.Controls["prosinec"]).Columns[9].DisplayIndex = Properties.Settings.Default.colStavPoradi;
                    ((ListView)tp.Controls["prosinec"]).Columns[10].Width = Properties.Settings.Default.colPoz;
                    ((ListView)tp.Controls["prosinec"]).Columns[10].DisplayIndex = Properties.Settings.Default.colPozPoradi;
                    ((ListView)tp.Controls["prosinec"]).EndUpdate();
                }
            }
            canChange = true;
        }

        private void AddColumns()
        {
            this.unor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.brezen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.duben.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.kveten.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.cerven.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.cervenec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.srpen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.zari.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.rijen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.listopad.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
            this.prosinec.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            (ColumnHeader)this.columnHeader11.Clone(),
            (ColumnHeader)this.columnHeader1.Clone(),
            (ColumnHeader)this.columnHeader2.Clone(),
            (ColumnHeader)this.columnHeader3.Clone(),
            (ColumnHeader)this.columnHeader4.Clone(),
            (ColumnHeader)this.columnHeader5.Clone(),
            (ColumnHeader)this.columnHeader6.Clone(),
            (ColumnHeader)this.columnHeader7.Clone(),
            (ColumnHeader)this.columnHeader8.Clone(),
            (ColumnHeader)this.columnHeader9.Clone(),
            (ColumnHeader)this.columnHeader10.Clone()});
        }

        internal void LoadFile(bool first = false)
        {
            muze = false;
            if (jmenoSouboru != null && jmenoSouboru != "")
            {
                leden.BeginUpdate();
                unor.BeginUpdate();
                brezen.BeginUpdate();
                duben.BeginUpdate();
                kveten.BeginUpdate();
                cerven.BeginUpdate();
                cervenec.BeginUpdate();
                srpen.BeginUpdate();
                zari.BeginUpdate();
                rijen.BeginUpdate();
                listopad.BeginUpdate();
                prosinec.BeginUpdate();

                if (File.Exists(jmenoSouboru))
                {
                    //čištění
                    file = new NbtFile();
                    zakaznici = new List<NbtCompound>();
                    file.LoadFromFile(jmenoSouboru);

                    if (file.RootTag.Get<NbtInt>("verze").Value <= saveFileVersion)
                    {
                        if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                        {
                            rokVyber.Items.Clear();
                            rokVyber.Enabled = false;
                            //save file verze 10000, před Ticketníkem 1.4.0.0
                            this.Text = jazyk.Header_Tickety + ": " + jmenoSouboru.Remove(0, jmenoSouboru.LastIndexOf('\\') + 1).Replace(".tic", "");
                            tickety.Clear();
                            převéstNaFormátMilleniumToolStripMenuItem.Enabled = true;
                            převéstNaFormátMilleniumToolStripMenuItem.Visible = true;
                            oddToolStripMenuItem.Visible = true;

                            foreach (TabPage tp in tabControl1.Controls)
                            {
                                ((ListView)tp.Controls[0]).Items.Clear();
                            }

                            zakaznici.Clear();
                            tickety.Clear();
                            poDnech.Clear();
                            toolStripButton1.Enabled = true;
                            maxID = file.RootTag.Get<NbtLong>("MaxID").Value;

                            //práce
                            foreach (NbtCompound c in file.RootTag.Get<NbtCompound>("Zakaznici"))
                            {
                                if (!tickety.ContainsKey(c.Name))
                                {
                                    tickety.Add(c.Name, new List<Ticket>());
                                    if (!list.ContainsKey(c.Name))
                                    {
                                        list.PridejZakaznika(c.Name, 0);
                                    }
                                }
                                if (c.Count > 0)
                                {
                                    foreach (NbtCompound mes in c)
                                    {
                                        int mesic = 0;
                                        switch (mes.Name)
                                        {
                                            case "leden":
                                                mesic = 1;
                                                break;
                                            case "unor":
                                                mesic = 2;
                                                break;
                                            case "brezen":
                                                mesic = 3;
                                                break;
                                            case "duben":
                                                mesic = 4;
                                                break;
                                            case "kveten":
                                                mesic = 5;
                                                break;
                                            case "cerven":
                                                mesic = 6;
                                                break;
                                            case "cervenec":
                                                mesic = 7;
                                                break;
                                            case "srpen":
                                                mesic = 8;
                                                break;
                                            case "zari":
                                                mesic = 9;
                                                break;
                                            case "rijen":
                                                mesic = 10;
                                                break;
                                            case "listopad":
                                                mesic = 11;
                                                break;
                                            case "prosinec":
                                                mesic = 12;
                                                break;

                                        }
                                        foreach (NbtCompound tic in mes.Get<NbtList>("Tickety"))
                                        {
                                            DateTime den = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue);
                                            DateTime denOd = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Od h"].ByteValue, tic["Od m"].ByteValue, 0);
                                            DateTime denDo = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Do h"].ByteValue, tic["Do m"].ByteValue, 0);
                                            List<DateTime> pauzyOd = new List<DateTime>();
                                            List<DateTime> pauzyDo = new List<DateTime>();

                                            for (int i = 0; i < tic["Pauza od h"].ByteArrayValue.Length; i++)
                                            {
                                                pauzyOd.Add(new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Pauza od h"].ByteArrayValue[i], tic["Pauza od m"].ByteArrayValue[i], 0));
                                                pauzyDo.Add(new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Pauza do h"].ByteArrayValue[i], tic["Pauza do m"].ByteArrayValue[i], 0));
                                            }

                                            Ticket.Stav st;
                                            switch (tic["Stav"].ByteValue)
                                            {
                                                case 0:
                                                    st = Ticket.Stav.Probiha;
                                                    break;
                                                case 1:
                                                    st = Ticket.Stav.Ceka_se;
                                                    break;
                                                case 2:
                                                    st = Ticket.Stav.RDP;
                                                    break;
                                                case 3:
                                                    st = Ticket.Stav.Ceka_se_na_odpoved;
                                                    break;
                                                case 4:
                                                    st = Ticket.Stav.Vyreseno;
                                                    break;
                                                default:
                                                    st = Ticket.Stav.Probiha;
                                                    break;
                                            }

                                            Ticket.TypTicketu tty;
                                            if (tic["Terp"] != null && tic["Terp"].StringValue != "" || tic["Task"] != null && tic["Task"].StringValue != "")
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Custom)
                                                    tty = Ticket.TypTicketu.Custom;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomPrescas)
                                                    tty = Ticket.TypTicketu.CustomPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomNahradni)
                                                    tty = Ticket.TypTicketu.CustomNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomOPrazdniny)
                                                    tty = Ticket.TypTicketu.CustomOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.Custom;
                                            }
                                            else if (tic["ID"].StringValue.StartsWith("INC") || tic["ID"].StringValue.StartsWith("RIT") || tic["ID"].StringValue.StartsWith("ITASK") || tic["ID"].StringValue.StartsWith("RTASK") || tic["ID"].StringValue.StartsWith("TASK"))
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Normalni)
                                                    tty = Ticket.TypTicketu.Normalni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.PraceOPrazdniny)
                                                    tty = Ticket.TypTicketu.PraceOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Prescas)
                                                    tty = Ticket.TypTicketu.Prescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.NahradniVolno)
                                                    tty = Ticket.TypTicketu.NahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Enkripce)
                                                    tty = Ticket.TypTicketu.Enkripce;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceNahradniVolno)
                                                    tty = Ticket.TypTicketu.EnkripceNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceOPrazdniny)
                                                    tty = Ticket.TypTicketu.EnkripceOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripcePrescas)
                                                    tty = Ticket.TypTicketu.EnkripcePrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Mobility)
                                                    tty = Ticket.TypTicketu.Mobility;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityNahradniVolno)
                                                    tty = Ticket.TypTicketu.MobilityNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityOPrazdniny)
                                                    tty = Ticket.TypTicketu.MobilityOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityPrescas)
                                                    tty = Ticket.TypTicketu.MobilityPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Custom)
                                                    tty = Ticket.TypTicketu.Custom;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomPrescas)
                                                    tty = Ticket.TypTicketu.CustomPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomNahradni)
                                                    tty = Ticket.TypTicketu.CustomNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomOPrazdniny)
                                                    tty = Ticket.TypTicketu.CustomOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.Normalni;
                                            }
                                            else if (tic["ID"].StringValue.StartsWith("PRB") || tic["ID"].StringValue.StartsWith("PTASK"))
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Normalni || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemTicket)
                                                    tty = Ticket.TypTicketu.ProblemTicket;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.PraceOPrazdniny || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemOPrazdniny)
                                                    tty = Ticket.TypTicketu.ProblemOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Prescas || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                    tty = Ticket.TypTicketu.ProblemPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.NahradniVolno || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemNahradniVolno)
                                                    tty = Ticket.TypTicketu.ProblemNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Enkripce || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblem)
                                                    tty = Ticket.TypTicketu.EnkripceProblem;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceNahradniVolno || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemNahradni)
                                                    tty = Ticket.TypTicketu.EnkripceProblemNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceOPrazdniny || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemOPrazdniny)
                                                    tty = Ticket.TypTicketu.EnkripceProblemOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripcePrescas || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemPrescas)
                                                    tty = Ticket.TypTicketu.EnkripceProblemPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.ProblemTicket;
                                            }
                                            else
                                                tty = Ticket.TypTicketu.Normalni;

                                            string onlineTyp = "";
                                            if (tic["OnlineTyp"] != null)
                                                onlineTyp = tic["OnlineTyp"].StringValue;

                                            if (tickety.ContainsKey(c.Name))
                                            {
                                                string cterp = "";
                                                if (tic["Terp"] != null)
                                                    cterp = tic["Terp"].StringValue;

                                                string ctask = "";
                                                if (tic["Task"] != null)
                                                    ctask = tic["Task"].StringValue;
                                                tickety[c.Name].Add(new Ticket(tic["IDlong"].LongValue, mes["Mesic"].StringValue, den, denOd, denDo, pauzyOd, pauzyDo, st, tic["ID"].StringValue, tic["Kontakt"].StringValue, tic["PC"].StringValue, tic["Popis"].StringValue, tic["Poznamky"].StringValue, tty, c.Name, cterp, ctask, onlineTyp));
                                            }
                                            else
                                            {
                                                string cterp = "";
                                                if (tic["Terp"] != null)
                                                    cterp = tic["Terp"].StringValue;

                                                string ctask = "";
                                                if (tic["Task"] != null)
                                                    ctask = tic["Task"].StringValue;
                                                tickety.Add(c.Name, new List<Ticket>());
                                                tickety[c.Name].Add(new Ticket(tic["IDlong"].LongValue, mes["Mesic"].StringValue, den, denOd, denDo, pauzyOd, pauzyDo, st, tic["ID"].StringValue, tic["Kontakt"].StringValue, tic["PC"].StringValue, tic["Popis"].StringValue, tic["Poznamky"].StringValue, tty, c.Name, cterp, ctask, onlineTyp));

                                            }
                                        }
                                    }
                                }
                            }
                            foreach (string s in tickety.Keys)
                            {
                                foreach (Ticket t in tickety[s])
                                {

                                    if (!poDnech.ContainsKey(t.Datum))
                                    {
                                        Dictionary<string, List<Ticket>> dt = new Dictionary<string, List<Ticket>>();
                                        poDnech.Add(t.Datum, dt);
                                    }

                                    if (!poDnech[t.Datum].ContainsKey(s))
                                    {
                                        poDnech[t.Datum].Add(s, new List<Ticket>());
                                    }

                                    poDnech[t.Datum][s].Add(t);

                                }
                            }
                            DateTime veDniCelkem = new DateTime();


                            foreach (DateTime dny in poDnech.Keys)
                            {

                                ListViewItem den = new ListViewItem(new string[] { "", dny.ToString("d.M.yyyy"), "", "", "", "", "", "", "", "", "" });
                                string veDni = veDniCelkem.ToString("H:mm");
                                veDniCelkem = new DateTime();
                                ListViewItem empty = new ListViewItem();
                                int pocetTicetuVeDni = 0;
                                int celkemDny = 0;
                                int counTic = 0;

                                foreach (KeyValuePair<string, List<Ticket>> dict in poDnech[dny])
                                {
                                    celkemDny++;
                                    pocetTicetuVeDni += dict.Value.Count;
                                    foreach (Ticket t in dict.Value)
                                    {
                                        counTic++;
                                        foreach (TabPage tp in tabControl1.Controls)
                                        {
                                            string mesic = "";
                                            switch (t.Mesic)
                                            {
                                                case "Leden":
                                                    mesic = "leden";
                                                    break;
                                                case "Únor":
                                                    mesic = "unor";
                                                    break;
                                                case "Březen":
                                                    mesic = "brezen";
                                                    break;
                                                case "Duben":
                                                    mesic = "duben";
                                                    break;
                                                case "Květen":
                                                    mesic = "kveten";
                                                    break;
                                                case "Červen":
                                                    mesic = "cerven";
                                                    break;
                                                case "Červenec":
                                                    mesic = "cervenec";
                                                    break;
                                                case "Srpen":
                                                    mesic = "srpen";
                                                    break;
                                                case "Září":
                                                    mesic = "zari";
                                                    break;
                                                case "Říjen":
                                                    mesic = "rijen";
                                                    break;
                                                case "Listopad":
                                                    mesic = "listopad";
                                                    break;
                                                case "Prosinec":
                                                    mesic = "prosinec";
                                                    break;
                                            }

                                            if (tp.Controls.ContainsKey(mesic))
                                            {
                                                if (den != null)
                                                {
                                                    ((ListView)tp.Controls[mesic]).Items.Add(den);
                                                    den.Tag = new Tag(t.Datum, -1);
                                                    den = null;
                                                }

                                                string stst = "";
                                                Color barvaP;

                                                switch (t.StavT)
                                                {
                                                    case Ticket.Stav.Probiha:
                                                        stst = jazyk.Status_Probiha;
                                                        //barvaP = Color.FromArgb(255, 255, 255, 160);
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                    case Ticket.Stav.Ceka_se:
                                                        stst = jazyk.Status_CekaSe;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.ceka;
                                                        break;
                                                    case Ticket.Stav.RDP:
                                                        stst = jazyk.Status_RDP;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.rdp;
                                                        break;
                                                    case Ticket.Stav.Ceka_se_na_odpoved:
                                                        stst = jazyk.Status_CekaSeNO;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.odpoved;
                                                        break;
                                                    case Ticket.Stav.Vyreseno:
                                                        stst = jazyk.Status_Vyreseno;
                                                        //barvaP = Color.FromArgb(255, 0, 200, 0);
                                                        if(t.TypPrace == (byte)Ticket.TypTicketu.Prescas || t.TypPrace == (byte)Ticket.TypTicketu.CustomPrescas || 
                                                            t.TypPrace == (byte)Ticket.TypTicketu.EnkripcePrescas || t.TypPrace == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                            t.TypPrace == (byte)Ticket.TypTicketu.MobilityPrescas || t.TypPrace == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                            t.TypPrace == (byte)Ticket.TypTicketu.ProblemPrescas || t.OnlineTyp.ToLower().Contains("overtime"))
                                                            barvaP = Properties.Settings.Default.prescas;
                                                        else
                                                            barvaP = Properties.Settings.Default.vyreseno;
                                                        break;
                                                    default:
                                                        stst = jazyk.Status_Probiha;
                                                        //barvaP = Color.FromArgb(255, 255, 255, 160);
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                }

                                                string[] terpTask = DejTerp(t);
                                                ListViewItem lvi = new ListViewItem(new string[] { "", t.PC, t.ID, dict.Key, t.Popis, t.Kontakt, terpTask[0], terpTask[1], Cas(t), stst, t.Poznamky });
                                                lvi.BackColor = barvaP;
                                                lvi.ForeColor = ContrastColor(barvaP);
                                                lvi.Tag = new Tag(t.Datum, t.IDtick);
                                                ((ListView)tp.Controls[mesic]).Items.Add(lvi);
                                                string[] casy = Cas(t).Split(':');
                                                veDniCelkem = veDniCelkem.AddHours(double.Parse(casy[0]));
                                                veDniCelkem = veDniCelkem.AddMinutes(double.Parse(casy[1]));

                                                if (counTic == pocetTicetuVeDni && poDnech[dny].Count == celkemDny)
                                                {
                                                    veDni = veDniCelkem.ToString("H:mm");

                                                    if (Properties.Settings.Default.pouzivatCasy)
                                                    {
                                                        empty = new ListViewItem(new string[] { "", "", "", "", "", "", "", "", veDni, "", "" });
                                                        empty.UseItemStyleForSubItems = false;
                                                        Color c;
                                                        if (veDniCelkem.Hour < 4)
                                                            c = Properties.Settings.Default.timeLow;
                                                        else if (veDniCelkem.Hour >= 4 && veDniCelkem.Hour < 8)
                                                            c = Properties.Settings.Default.timeMid;
                                                        else if (veDniCelkem.Hour == 8 && veDniCelkem.Minute == 0)
                                                            c = Properties.Settings.Default.timeOK;
                                                        else
                                                            c = Properties.Settings.Default.timeLong;
                                                        empty.SubItems[8].BackColor = c;
                                                        empty.SubItems[8].ForeColor = ContrastColor(empty.SubItems[8].BackColor);
                                                    }
                                                    else
                                                        empty = new ListViewItem(new string[] { "", "", "", "", "", "", "", "", "", "", "" });
                                                    ((ListView)tp.Controls[mesic]).Items.Add(empty);
                                                    empty.Tag = new Tag(t.Datum, -1);
                                                }

                                                break;
                                            }

                                        }

                                    }
                                }
                            }

                            if (first)
                            {
                                tabControl1.SelectedIndex = (DateTime.Now.Month - 1);
                                if (((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count != 0)
                                {
                                    ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1];
                                    ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1);
                                }

                                vybranyMesic = tabControl1.SelectedTab.Name.Replace("T", "");
                            }
                            else
                            {
                                if (((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count != 0)
                                {
                                    try
                                    {
                                        ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[posledniVybrany];
                                        ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[posledniVybrany];
                                            ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(posledniVybrany);
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //save file verze 10100, od Ticketníku 1.4.0.0
                            if (file.RootTag.Get<NbtInt>("verze").Value < saveFileVersion)
                                file.RootTag.Get<NbtInt>("verze").Value = saveFileVersion;
                            this.Text = jazyk.Header_Tickety + ": " + jmenoSouboru.Remove(0, jmenoSouboru.LastIndexOf('\\') + 1).Replace(".tic", "");
                            tickety.Clear();
                            převéstNaFormátMilleniumToolStripMenuItem.Enabled = false;
                            převéstNaFormátMilleniumToolStripMenuItem.Visible = false;
                            oddToolStripMenuItem.Visible = false;

                            foreach (TabPage tp in tabControl1.Controls)
                            {
                                ((ListView)tp.Controls[0]).Items.Clear();
                            }

                            zakaznici.Clear();
                            tickety.Clear();
                            poDnech.Clear();
                            toolStripButton1.Enabled = true;
                            maxID = file.RootTag.Get<NbtLong>("MaxID").Value;

                            string vybranyRok = null;

                            if(rokVyber.SelectedItem != null)
                                vybranyRok = rokVyber.SelectedItem.ToString();

                            rokVyber.Items.Clear();
                            rokVyber.Items.Add(DateTime.Now.Year.ToString());
                            rokVyber.Enabled = true;

                            foreach (NbtCompound rok in file.RootTag.Get<NbtCompound>("Zakaznici").Tags)
                            {
                                if (!rokVyber.Items.Contains(rok.Name))
                                    rokVyber.Items.Add(rok.Name);
                            }

                            if (vybranyRok != null && rokVyber.Items.Contains(vybranyRok))
                                rokVyber.SelectedItem = vybranyRok;
                            else
                                rokVyber.SelectedItem = DateTime.Now.Year.ToString();


                            if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()) == null)
                                file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(rokVyber.SelectedItem.ToString()));
                            //práce
                            foreach (NbtCompound c in file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()))
                            {
                                if (!tickety.ContainsKey(c.Name))
                                {
                                    tickety.Add(c.Name, new List<Ticket>());
                                    if (!list.ContainsKey(c.Name))
                                    {
                                        list.PridejZakaznika(c.Name, 0);
                                    }
                                }
                                if (c.Count > 0)
                                {
                                    foreach (NbtCompound mes in c)
                                    {
                                        int mesic = 0;
                                        switch (mes.Name)
                                        {
                                            case "leden":
                                                mesic = 1;
                                                break;
                                            case "unor":
                                                mesic = 2;
                                                break;
                                            case "brezen":
                                                mesic = 3;
                                                break;
                                            case "duben":
                                                mesic = 4;
                                                break;
                                            case "kveten":
                                                mesic = 5;
                                                break;
                                            case "cerven":
                                                mesic = 6;
                                                break;
                                            case "cervenec":
                                                mesic = 7;
                                                break;
                                            case "srpen":
                                                mesic = 8;
                                                break;
                                            case "zari":
                                                mesic = 9;
                                                break;
                                            case "rijen":
                                                mesic = 10;
                                                break;
                                            case "listopad":
                                                mesic = 11;
                                                break;
                                            case "prosinec":
                                                mesic = 12;
                                                break;

                                        }
                                        foreach (NbtCompound tic in mes.Get<NbtList>("Tickety"))
                                        {
                                            DateTime den = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue);
                                            DateTime denOd = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Od h"].ByteValue, tic["Od m"].ByteValue, 0);
                                            DateTime denDo = new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Do h"].ByteValue, tic["Do m"].ByteValue, 0);
                                            List<DateTime> pauzyOd = new List<DateTime>();
                                            List<DateTime> pauzyDo = new List<DateTime>();

                                            for (int i = 0; i < tic["Pauza od h"].ByteArrayValue.Length; i++)
                                            {
                                                pauzyOd.Add(new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Pauza od h"].ByteArrayValue[i], tic["Pauza od m"].ByteArrayValue[i], 0));
                                                pauzyDo.Add(new DateTime(tic["Rok"].ShortValue, mesic, (int)tic["Den"].ByteValue, tic["Pauza do h"].ByteArrayValue[i], tic["Pauza do m"].ByteArrayValue[i], 0));
                                            }

                                            Ticket.Stav st;
                                            switch (tic["Stav"].ByteValue)
                                            {
                                                case 0:
                                                    st = Ticket.Stav.Probiha;
                                                    break;
                                                case 1:
                                                    st = Ticket.Stav.Ceka_se;
                                                    break;
                                                case 2:
                                                    st = Ticket.Stav.RDP;
                                                    break;
                                                case 3:
                                                    st = Ticket.Stav.Ceka_se_na_odpoved;
                                                    break;
                                                case 4:
                                                    st = Ticket.Stav.Vyreseno;
                                                    break;
                                                default:
                                                    st = Ticket.Stav.Probiha;
                                                    break;
                                            }

                                            Ticket.TypTicketu tty;
                                            if (tic["Terp"] != null && tic["Terp"].StringValue != "" || tic["Task"] != null && tic["Task"].StringValue != "")
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Custom)
                                                    tty = Ticket.TypTicketu.Custom;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomPrescas)
                                                    tty = Ticket.TypTicketu.CustomPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomNahradni)
                                                    tty = Ticket.TypTicketu.CustomNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomOPrazdniny)
                                                    tty = Ticket.TypTicketu.CustomOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.Custom;
                                            }
                                            else if (tic["ID"].StringValue.StartsWith("INC") || tic["ID"].StringValue.StartsWith("RIT") || tic["ID"].StringValue.StartsWith("ITASK") || tic["ID"].StringValue.StartsWith("RTASK") || tic["ID"].StringValue.StartsWith("TASK"))
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Normalni)
                                                    tty = Ticket.TypTicketu.Normalni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.PraceOPrazdniny)
                                                    tty = Ticket.TypTicketu.PraceOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Prescas)
                                                    tty = Ticket.TypTicketu.Prescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.NahradniVolno)
                                                    tty = Ticket.TypTicketu.NahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Enkripce)
                                                    tty = Ticket.TypTicketu.Enkripce;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceNahradniVolno)
                                                    tty = Ticket.TypTicketu.EnkripceNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceOPrazdniny)
                                                    tty = Ticket.TypTicketu.EnkripceOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripcePrescas)
                                                    tty = Ticket.TypTicketu.EnkripcePrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Mobility)
                                                    tty = Ticket.TypTicketu.Mobility;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityNahradniVolno)
                                                    tty = Ticket.TypTicketu.MobilityNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityOPrazdniny)
                                                    tty = Ticket.TypTicketu.MobilityOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.MobilityPrescas)
                                                    tty = Ticket.TypTicketu.MobilityPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Custom)
                                                    tty = Ticket.TypTicketu.Custom;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomPrescas)
                                                    tty = Ticket.TypTicketu.CustomPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomNahradni)
                                                    tty = Ticket.TypTicketu.CustomNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.CustomOPrazdniny)
                                                    tty = Ticket.TypTicketu.CustomOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.Normalni;
                                            }
                                            else if (tic["ID"].StringValue.StartsWith("PRB") || tic["ID"].StringValue.StartsWith("PTASK"))
                                            {
                                                if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Normalni || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemTicket)
                                                    tty = Ticket.TypTicketu.ProblemTicket;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.PraceOPrazdniny || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemOPrazdniny)
                                                    tty = Ticket.TypTicketu.ProblemOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Prescas || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemPrescas)
                                                    tty = Ticket.TypTicketu.ProblemPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.NahradniVolno || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.ProblemNahradniVolno)
                                                    tty = Ticket.TypTicketu.ProblemNahradniVolno;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.Enkripce || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblem)
                                                    tty = Ticket.TypTicketu.EnkripceProblem;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceNahradniVolno || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemNahradni)
                                                    tty = Ticket.TypTicketu.EnkripceProblemNahradni;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceOPrazdniny || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemOPrazdniny)
                                                    tty = Ticket.TypTicketu.EnkripceProblemOPrazdniny;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripcePrescas || tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.EnkripceProblemPrescas)
                                                    tty = Ticket.TypTicketu.EnkripceProblemPrescas;
                                                else if (tic["Prace"].ByteValue == (byte)Ticket.TypTicketu.OnlineTyp)
                                                    tty = Ticket.TypTicketu.OnlineTyp;
                                                else
                                                    tty = Ticket.TypTicketu.ProblemTicket;
                                            }
                                            else
                                                tty = Ticket.TypTicketu.Normalni;

                                            string onlineTyp = "";
                                            if (tic["OnlineTyp"] != null)
                                                onlineTyp = tic["OnlineTyp"].StringValue;

                                            if (tickety.ContainsKey(c.Name))
                                            {
                                                string cterp = "";
                                                if (tic["Terp"] != null)
                                                    cterp = tic["Terp"].StringValue;

                                                string ctask = "";
                                                if (tic["Task"] != null)
                                                    ctask = tic["Task"].StringValue;
                                                tickety[c.Name].Add(new Ticket(tic["IDlong"].LongValue, mes["Mesic"].StringValue, den, denOd, denDo, pauzyOd, pauzyDo, st, tic["ID"].StringValue, tic["Kontakt"].StringValue, tic["PC"].StringValue, tic["Popis"].StringValue, tic["Poznamky"].StringValue, tty, c.Name, cterp, ctask, onlineTyp));
                                            }
                                            else
                                            {
                                                string cterp = "";
                                                if (tic["Terp"] != null)
                                                    cterp = tic["Terp"].StringValue;

                                                string ctask = "";
                                                if (tic["Task"] != null)
                                                    ctask = tic["Task"].StringValue;
                                                tickety.Add(c.Name, new List<Ticket>());
                                                tickety[c.Name].Add(new Ticket(tic["IDlong"].LongValue, mes["Mesic"].StringValue, den, denOd, denDo, pauzyOd, pauzyDo, st, tic["ID"].StringValue, tic["Kontakt"].StringValue, tic["PC"].StringValue, tic["Popis"].StringValue, tic["Poznamky"].StringValue, tty, c.Name, cterp, ctask, onlineTyp));

                                            }



                                        }
                                    }
                                }
                            }
                            foreach (string s in tickety.Keys)
                            {
                                foreach (Ticket t in tickety[s])
                                {

                                    if (!poDnech.ContainsKey(t.Datum))
                                    {
                                        Dictionary<string, List<Ticket>> dt = new Dictionary<string, List<Ticket>>();
                                        poDnech.Add(t.Datum, dt);
                                    }

                                    if (!poDnech[t.Datum].ContainsKey(s))
                                    {
                                        poDnech[t.Datum].Add(s, new List<Ticket>());
                                    }

                                    poDnech[t.Datum][s].Add(t);

                                }
                            }
                            DateTime veDniCelkem = new DateTime();


                            foreach (DateTime dny in poDnech.Keys)
                            {

                                ListViewItem den = new ListViewItem(new string[] { "", dny.ToString("d.M.yyyy"), "", "", "", "", "", "", "", "", "" });
                                string veDni = veDniCelkem.ToString("H:mm");
                                veDniCelkem = new DateTime();
                                ListViewItem empty = new ListViewItem();
                                int pocetTicetuVeDni = 0;
                                int celkemDny = 0;
                                int counTic = 0;

                                foreach (KeyValuePair<string, List<Ticket>> dict in poDnech[dny])
                                {
                                    celkemDny++;
                                    pocetTicetuVeDni += dict.Value.Count;
                                    foreach (Ticket t in dict.Value)
                                    {
                                        counTic++;
                                        foreach (TabPage tp in tabControl1.Controls)
                                        {
                                            string mesic = "";
                                            switch (t.Mesic)
                                            {
                                                case "Leden":
                                                    mesic = "leden";
                                                    break;
                                                case "Únor":
                                                    mesic = "unor";
                                                    break;
                                                case "Březen":
                                                    mesic = "brezen";
                                                    break;
                                                case "Duben":
                                                    mesic = "duben";
                                                    break;
                                                case "Květen":
                                                    mesic = "kveten";
                                                    break;
                                                case "Červen":
                                                    mesic = "cerven";
                                                    break;
                                                case "Červenec":
                                                    mesic = "cervenec";
                                                    break;
                                                case "Srpen":
                                                    mesic = "srpen";
                                                    break;
                                                case "Září":
                                                    mesic = "zari";
                                                    break;
                                                case "Říjen":
                                                    mesic = "rijen";
                                                    break;
                                                case "Listopad":
                                                    mesic = "listopad";
                                                    break;
                                                case "Prosinec":
                                                    mesic = "prosinec";
                                                    break;
                                            }

                                            if (tp.Controls.ContainsKey(mesic))
                                            {
                                                if (den != null)
                                                {
                                                    ((ListView)tp.Controls[mesic]).Items.Add(den);
                                                    den.Tag = new Tag(t.Datum, -1);
                                                    den = null;
                                                }

                                                string stst = "";
                                                Color barvaP;

                                                switch (t.StavT)
                                                {
                                                    case Ticket.Stav.Probiha:
                                                        stst = jazyk.Status_Probiha;
                                                        //barvaP = Color.FromArgb(255, 255, 255, 160);
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                    case Ticket.Stav.Ceka_se:
                                                        stst = jazyk.Status_CekaSe;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.ceka;
                                                        break;
                                                    case Ticket.Stav.RDP:
                                                        stst = jazyk.Status_RDP;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.rdp;
                                                        break;
                                                    case Ticket.Stav.Ceka_se_na_odpoved:
                                                        stst = jazyk.Status_CekaSeNO;
                                                        //barvaP = Color.Yellow;
                                                        barvaP = Properties.Settings.Default.odpoved;
                                                        break;
                                                    case Ticket.Stav.Vyreseno:
                                                        stst = jazyk.Status_Vyreseno;
                                                        //barvaP = Color.FromArgb(255, 0, 200, 0);
                                                        if (t.TypPrace == (byte)Ticket.TypTicketu.Prescas || t.TypPrace == (byte)Ticket.TypTicketu.CustomPrescas ||
                                                            t.TypPrace == (byte)Ticket.TypTicketu.EnkripcePrescas || t.TypPrace == (byte)Ticket.TypTicketu.EnkripceProblemPrescas ||
                                                            t.TypPrace == (byte)Ticket.TypTicketu.MobilityPrescas || t.TypPrace == (byte)Ticket.TypTicketu.MobilityProblemPrescas ||
                                                            t.TypPrace == (byte)Ticket.TypTicketu.ProblemPrescas || t.OnlineTyp.ToLower().Contains("overtime"))
                                                            barvaP = Properties.Settings.Default.prescas;
                                                        else
                                                            barvaP = Properties.Settings.Default.vyreseno;
                                                        break;
                                                    default:
                                                        stst = jazyk.Status_Probiha;
                                                        //barvaP = Color.FromArgb(255, 255, 255, 160);
                                                        barvaP = Properties.Settings.Default.probiha;
                                                        break;
                                                }

                                                string[] terpTask = DejTerp(t);
                                                ListViewItem lvi = new ListViewItem(new string[] { "", t.PC, t.ID, dict.Key, t.Popis, t.Kontakt, terpTask[0], terpTask[1], Cas(t), stst, t.Poznamky });
                                                lvi.BackColor = barvaP;
                                                lvi.ForeColor = ContrastColor(barvaP);
                                                lvi.Tag = new Tag(t.Datum, t.IDtick);
                                                ((ListView)tp.Controls[mesic]).Items.Add(lvi);
                                                string[] casy = Cas(t).Split(':');
                                                veDniCelkem = veDniCelkem.AddHours(double.Parse(casy[0]));
                                                veDniCelkem = veDniCelkem.AddMinutes(double.Parse(casy[1]));

                                                if (counTic == pocetTicetuVeDni && poDnech[dny].Count == celkemDny)
                                                {
                                                    veDni = veDniCelkem.ToString("H:mm");

                                                    if (Properties.Settings.Default.pouzivatCasy)
                                                    {
                                                        empty = new ListViewItem(new string[] { "", "", "", "", "", "", "", "", veDni, "", "" });
                                                        empty.UseItemStyleForSubItems = false;
                                                        Color c;
                                                        if (veDniCelkem.Hour < 4)
                                                            c = Properties.Settings.Default.timeLow;
                                                        else if (veDniCelkem.Hour >= 4 && veDniCelkem.Hour < 8)
                                                            c = Properties.Settings.Default.timeMid;
                                                        else if (veDniCelkem.Hour == 8 && veDniCelkem.Minute == 0)
                                                            c = Properties.Settings.Default.timeOK;
                                                        else
                                                            c = Properties.Settings.Default.timeLong;
                                                        empty.SubItems[8].BackColor = c;
                                                        empty.SubItems[8].ForeColor = ContrastColor(empty.SubItems[8].BackColor);
                                                    }
                                                    else
                                                        empty = new ListViewItem(new string[] { "", "", "", "", "", "", "", "", "", "", "" });
                                                    ((ListView)tp.Controls[mesic]).Items.Add(empty);
                                                    empty.Tag = new Tag(t.Datum, -1);
                                                }

                                                break;
                                            }

                                        }

                                    }
                                }
                            }

                            if (first)
                            {
                                tabControl1.SelectedIndex = (DateTime.Now.Month - 1);
                                if (((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count != 0)
                                {
                                    ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1];
                                    ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1);
                                }

                                vybranyMesic = tabControl1.SelectedTab.Name.Replace("T", "");
                            }
                            else
                            {
                                if (((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count != 0)
                                {
                                    try
                                    {
                                        ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[posledniVybrany];
                                        ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(((ListView)tabControl1.SelectedTab.Controls[0]).Items.Count - 1);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            ((ListView)tabControl1.SelectedTab.Controls[0]).TopItem = ((ListView)tabControl1.SelectedTab.Controls[0]).Items[posledniVybrany];
                                            ((ListView)tabControl1.SelectedTab.Controls[0]).EnsureVisible(posledniVybrany);
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                    else
                        MessageBox.Show(jazyk.Error_NovějsiVerze, jazyk.Error_Verze, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    Properties.Settings.Default.filePath = jmenoSouboru = "";
                    Properties.Settings.Default.Save();
                    toolStripButton1.Enabled = false;
                }

                leden.EndUpdate();
                unor.EndUpdate();
                brezen.EndUpdate();
                duben.EndUpdate();
                kveten.EndUpdate();
                cerven.EndUpdate();
                cervenec.EndUpdate();
                srpen.EndUpdate();
                zari.EndUpdate();
                rijen.EndUpdate();
                listopad.EndUpdate();
                prosinec.EndUpdate();
            }
            muze = true;
        }

        private void načístToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = appdata + "\\Ticketnik";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = jazyk.Windows_SouborTicketu + "|*.tic;*.old";
            openFileDialog1.FileName = "";
            openFileDialog1.Title = jazyk.Windows_Otevrit;

            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                Properties.Settings.Default.filePath = jmenoSouboru = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
                try
                {
                    LoadFile();
                    File.Copy(jmenoSouboru, jmenoSouboru + ".bak", true);
                }
                catch
                {
                    Logni("Soubor .tic je poškozen. " + jmenoSouboru, LogMessage.WARNING);
                    MessageBox.Show(jazyk.Error_DamagedTicFile, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        }

        private void ukončitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1(this);
            ab.ShowDialog();
        }

        private void novýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filename fn = new Filename(this);
            fn.StartPosition = FormStartPosition.Manual;
            fn.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            if (DialogResult.OK == fn.ShowDialog())
            {
                file = new NbtFile();
                file.RootTag.Add(new NbtCompound("Zakaznici"));
                file.RootTag.Add(new NbtLong("MaxID", 0));
                file.RootTag.Add(new NbtInt("verze", saveFileVersion));
                file.SaveToFile(appdata + "\\Ticketnik\\" + jmenoSouboru + ".tic", NbtCompression.GZip);
                Properties.Settings.Default.filePath = jmenoSouboru = appdata + "\\Ticketnik\\" + jmenoSouboru + ".tic";
                Properties.Settings.Default.Save();
                LoadFile();
                NeulozenoMetoda();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Zakaznik zak = new Zakaznik(this);
            zak.StartPosition = FormStartPosition.Manual;
            zak.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            if (DialogResult.OK == zak.ShowDialog())
            {
                if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                    file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(zakaznik));
                else
                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()).Add(new NbtCompound(zakaznik));

                list.PridejZakaznika(zakaznik, velikost, zakaznikTerp);
            }
        }

        internal void NeulozenoMetoda()
        {
            if (ulozeno)
            {
                ulozeno = false;
                this.Text += " (" + jazyk.Header_Neulozeno + ")";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try 
            { 
                terpLoaderClient.CancelPendingRequests();
            } 
            catch { }

            if (this.WindowState != FormWindowState.Maximized)
            {
                if (this.WindowState != FormWindowState.Normal)
                    this.WindowState = FormWindowState.Normal;
                Properties.Settings.Default.velikost = this.Size;
                Properties.Settings.Default.umisteni = this.Location;
            }
            Properties.Settings.Default.maximized = (this.WindowState == FormWindowState.Maximized) ? true : false;
            Properties.Settings.Default.Save();

            if (!ulozeno)
            {
                if (e.CloseReason != CloseReason.WindowsShutDown)
                {
                    DialogResult dr = MessageBox.Show(jazyk.Message_Ulozit, jazyk.Message_Neulozeno, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        ulozeno = true;
                        timer1.Stop();
                        if(updateRunning)
                        {
                            vlaknoCancel.Cancel();
                        }

                        if (vlaknoTerp.IsAlive)
                            vlaknoTerp.Abort();
                        vlaknoTerp = null;

                    }
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                        e.Cancel = true;
                    else
                    {
                        uložitToolStripMenuItem_Click(sender, e);
                        timer1.Stop();

                        if (vlaknoTerp.IsAlive)
                            vlaknoTerp.Abort();
                        vlaknoTerp = null;

                        if (updateRunning)
                        {
                            vlaknoCancel.Cancel();
                        }

                    }
                }
                else
                {
                    uložitToolStripMenuItem_Click(sender, e);
                    timer1.Stop();

                    if (vlaknoTerp.IsAlive)
                        vlaknoTerp.Abort();
                    vlaknoTerp = null;

                    if (updateRunning)
                    {
                        vlaknoCancel.Cancel();
                    }
                }
            }
            else
            {
                timer1.Stop();

                if (vlaknoTerp.IsAlive)
                    vlaknoTerp.Abort();
                vlaknoTerp = null;

                if (updateRunning)
                {
                    vlaknoCancel.Cancel();
                }
            }
        }

        internal void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (jmenoSouboru != null && jmenoSouboru != "")
            {
                file.SaveToFile(jmenoSouboru, NbtCompression.GZip);
                this.Text = this.Text.Replace(" (" + jazyk.Header_Neulozeno + ")", "");
                ulozeno = true;
                if (Properties.Settings.Default.pouzivatCasy)
                    LoadFile();
            }
        }

        internal void leden_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedIndices.Count != 0 && ((Tag)((ListView)sender).SelectedItems[0].Tag).IDlong != -1)
            {
                toolStripButton2.Enabled = true;
                toolStripButton3.Enabled = true;
            }
            else
            {
                toolStripButton2.Enabled = false;
                toolStripButton3.Enabled = false;
            }
            if (((ListView)sender).SelectedIndices.Count != 0)
                posledniVybrany = ((ListView)sender).SelectedIndices[0];
        }

        //mazání značek dnů
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            foreach (TabPage tp in tabControl1.Controls)
            {
                if (tp.Controls.ContainsKey(vybranyMesic))
                {
                    Ticket refer = null;
                    Dictionary<string, List<Ticket>> tempD = poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum];
                    foreach (Ticket t in tempD[((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text])
                    {
                        long id = ((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).IDlong;

                        if (t.IDtick == id)
                        {
                            refer = t;
                            break;
                        }
                    }
                    NbtCompound referC = null;
                    if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                    {
                        foreach (NbtCompound c in file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety"))
                        {
                            if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                            {
                                referC = c;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (NbtCompound c in file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()).Get<NbtCompound>(((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety"))
                        {
                            if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                            {
                                referC = c;
                                break;
                            }
                        }
                    }

                    if (refer != null)
                    {
                        int index = poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum][((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text].IndexOf(refer);
                        poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum][((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text].RemoveAt(index);
                        if (poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum][((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text].Count == 0)
                            poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum].Remove(((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text);
                    }
                    if (referC != null)
                    {
                        if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                            file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety").Remove(referC);
                        else
                            file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()).Get<NbtCompound>(((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety").Remove(referC);
                    }
                    Tag ta = (Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag;
                    int i1 = -1, i2 = -1;
                    int zbyva = 0;
                    if ((zbyva = poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum].Count) == 0)
                    {
                        foreach (ListViewItem lvi in ((ListView)tp.Controls[vybranyMesic]).Items)
                        {
                            if (((Tag)lvi.Tag).Datum == ta.Datum && ((Tag)lvi.Tag).IDlong == -1)
                            {
                                if (i1 == -1)
                                    i1 = ((ListView)tp.Controls[vybranyMesic]).Items.IndexOf(lvi);
                                else
                                    i2 = ((ListView)tp.Controls[vybranyMesic]).Items.IndexOf(lvi);

                            }
                        }
                    }

                    ((ListView)tp.Controls[vybranyMesic]).Items.RemoveAt(((ListView)tp.Controls[vybranyMesic]).SelectedIndices[0]);
                    if (zbyva == 0)
                    {
                        ((ListView)tp.Controls[vybranyMesic]).Items.RemoveAt(i2 - 1);
                        ((ListView)tp.Controls[vybranyMesic]).Items.RemoveAt(i1);
                    }
                    break;
                }
            }
            NeulozenoMetoda();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            SeznamZak szk = new SeznamZak(this);
            szk.StartPosition = FormStartPosition.Manual;
            szk.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            if (DialogResult.OK == szk.ShowDialog())
            {
                if (DialogResult.Yes == MessageBox.Show(jazyk.Message_SmazatZakaznika + " " + tempZak + "?", jazyk.Message_OpravduSmazat, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    list.SmazZakaznika(tempZak);
            }
        }

        internal void Vlozit()
        {
            if (copy == null)
            {
                string[] text = Clipboard.GetText().Replace("\r", "").Split('\n');

                int preskoceno = 0;
                foreach (string s in text)
                {
                    if (s == "")
                        continue;
                    Regex incidenty = new Regex(@"^(INC|PRB|PTASK|RITM|ITASK|CTASK|TASK|RTASK|CHG)\d+?$", RegexOptions.IgnoreCase);
                    Match m = incidenty.Match(s);
                    if (m.Success)
                    {
                        string mesic = "", tMesic = "";
                        switch (DateTime.Today.Month)
                        {
                            case 1:
                                mesic = "leden";
                                tMesic = "Leden";
                                break;
                            case 2:
                                mesic = "unor";
                                tMesic = "Únor";
                                break;
                            case 3:
                                mesic = "brezen";
                                tMesic = "Březen";
                                break;
                            case 4:
                                mesic = "duben";
                                tMesic = "Duben";
                                break;
                            case 5:
                                mesic = "kveten";
                                tMesic = "Květen";
                                break;
                            case 6:
                                mesic = "cerven";
                                tMesic = "Červen";
                                break;
                            case 7:
                                mesic = "cervenec";
                                tMesic = "Červenec";
                                break;
                            case 8:
                                mesic = "srpen";
                                tMesic = "Srpen";
                                break;
                            case 9:
                                mesic = "zari";
                                tMesic = "Září";
                                break;
                            case 10:
                                mesic = "rijen";
                                tMesic = "Říjen";
                                break;
                            case 11:
                                mesic = "listopad";
                                tMesic = "Listopad";
                                break;
                            case 12:
                                mesic = "prosinec";
                                tMesic = "Prosinec";
                                break;
                        }

                        DateTime casOdD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
                        DateTime casDoD = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

                        Ticket ticket = ticket = new Ticket(-1, tMesic, DateTime.Today, DateTime.Today, DateTime.Today, new List<DateTime>(), new List<DateTime>(), Ticket.Stav.Probiha, "", "", "", "", "", Ticket.TypTicketu.Normalni);
                        ticket.PC = "";
                        ticket.Kontakt = "";
                        ticket.Poznamky = "";
                        ticket.Od = casOdD;
                        ticket.Do = casDoD;
                        ticket.Popis = "";
                        ticket.IDtick = file.RootTag.Get<NbtLong>("MaxID").Value++;
                        ticket.ID = s;

                        SeznamZak sz = new SeznamZak(this);
                        sz.StartPosition = FormStartPosition.Manual;
                        sz.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                        if (DialogResult.OK == sz.ShowDialog())
                            ticket.Zakaznik = tempZak;

                        if (ticket.Zakaznik != "")
                        {
                            if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                            {
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik) == null)
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(ticket.Zakaznik));
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic) == null)
                                {
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound(mesic));
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtString("Mesic", ticket.Mesic));
                                }
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety") == null)
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtList("Tickety", NbtTagType.Compound));
                                file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());
                            }
                            else
                            {
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()) == null)
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(DateTime.Now.Year.ToString()));
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik) == null)
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Add(new NbtCompound(ticket.Zakaznik));
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic) == null)
                                {
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Add(new NbtCompound(mesic));
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtString("Mesic", ticket.Mesic));
                                }
                                if (file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety") == null)
                                    file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Add(new NbtList("Tickety", NbtTagType.Compound));

                                file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(DateTime.Now.Year.ToString()).Get<NbtCompound>(ticket.Zakaznik).Get<NbtCompound>(mesic).Get<NbtList>("Tickety").Add(ticket.GetNbtObject());

                            }
                            uložitToolStripMenuItem_Click(this, null);
                            LoadFile();
                            tempZak = "";
                        }
                        else
                            preskoceno++;
                    }
                    else
                    {
                        preskoceno++;
                    }
                }
                if (preskoceno != 0)
                    MessageBox.Show(preskoceno + " " + jazyk.Message_Preskoceno);
            }
            else
            {
                if (!terpTaskFileLock)
                {
                    TicketWindow ticketWindow = new TicketWindow(this, true);
                    ticketWindow.StartPosition = FormStartPosition.Manual;
                    ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                    ticketWindow.Text = jazyk.Windows_NovyTicket;
                    ticketWindow.zacatek.Text = DateTime.Now.ToString("H:mm");
                    ticketWindow.idTicketu.Text = copy.Get<NbtString>("ID").Value;
                    ticketWindow.zakaznik.SelectedItem = zakaznikVlozit;
                    ticketWindow.popis.Text = copy.Get<NbtString>("Popis").Value;
                    ticketWindow.richTextBox1.Text = copy.Get<NbtString>("Poznamky").Value;
                    ticketWindow.kontakt.Text = copy.Get<NbtString>("Kontakt").Value;
                    ticketWindow.pocitac.Text = copy.Get<NbtString>("PC").Value;
                    if (copy.Get<NbtString>("Terp") != null)
                        ticketWindow.terpt = copy.Get<NbtString>("Terp").Value;
                    if (copy.Get<NbtString>("Task") != null)
                        ticketWindow.task = copy.Get<NbtString>("Task").Value;
                    ticketWindow.terpKod.Text = ticketWindow.DejTerp();
                    ticketWindow.ShowDialog();
                    copy = null;
                    zakaznikVlozit = "";
                }
                else
                    MessageBox.Show(jazyk.Message_TerpUpdate, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            vybranyMesic = tabControl1.SelectedTab.Name.Replace("T", "");
            foreach (TabPage tp in tabControl1.Controls)
            {
                if (tp.Controls.ContainsKey(vybranyMesic))
                {
                    if (((ListView)tp.Controls[vybranyMesic]).SelectedIndices.Count == 0)
                    {
                        toolStripButton2.Enabled = false;
                        toolStripButton3.Enabled = false;
                    }
                    else if (((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).IDlong == -1)
                    {
                        toolStripButton2.Enabled = false;
                        toolStripButton3.Enabled = false;
                    }
                    else
                    {
                        toolStripButton2.Enabled = true;
                        toolStripButton3.Enabled = true;
                    }

                    break;
                }
            }
        }

        private void nastaveníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nastaveni nast = new Nastaveni(this);
            nast.StartPosition = FormStartPosition.Manual;
            nast.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            nast.ShowDialog();

            timer1.Stop();
            Autosave();
        }

        internal void Autosave()
        {
            timer1.Interval = (int)Properties.Settings.Default.minuty * 1000 * 60;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.autosave)
            {
                if (InvokeRequired)
                {
                    int selected = -1;
                    foreach (TabPage tp in tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey(vybranyMesic))
                        {
                            selected = ((ListView)tp.Controls[vybranyMesic]).SelectedIndices[0];
                        }
                    }
                    this.BeginInvoke(new Action(() => uložitToolStripMenuItem_Click(sender, e)));
                    if (selected != -1)
                    {
                        foreach (TabPage tp in tabControl1.Controls)
                        {
                            if (tp.Controls.ContainsKey(vybranyMesic))
                            {
                                this.BeginInvoke(new Action(() => ((ListView)tp.Controls[vybranyMesic]).Items[selected].Selected = true));
                            }
                        }

                    }
                }
                else
                {
                    int selected = -1;
                    foreach (TabPage tp in tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey(vybranyMesic))
                        {
                            if (((ListView)tp.Controls[vybranyMesic]).SelectedIndices.Count > 0)
                                selected = ((ListView)tp.Controls[vybranyMesic]).SelectedIndices[0];
                        }
                    }
                    uložitToolStripMenuItem_Click(sender, e);
                    if (selected != -1)
                    {
                        foreach (TabPage tp in tabControl1.Controls)
                        {
                            if (tp.Controls.ContainsKey(vybranyMesic))
                            {
                                ((ListView)tp.Controls[vybranyMesic]).Items[selected].Selected = true;
                            }
                        }

                    }
                }
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!terpTaskFileLock)
            {
                TicketWindow ticketWindow = new TicketWindow(this, true);
                ticketWindow.StartPosition = FormStartPosition.Manual;
                ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                ticketWindow.Text = jazyk.Windows_NovyTicket;
                ticketWindow.zacatek.Text = DateTime.Now.ToString("H:mm");
                ticketWindow.ShowDialog();
            }
            else
                MessageBox.Show(jazyk.Message_TerpUpdate, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        internal void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!terpTaskFileLock)
            {
                TicketWindow ticketWindow = new TicketWindow(this, false);
                ticketWindow.StartPosition = FormStartPosition.Manual;
                ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                ticketWindow.Text = jazyk.Windows_UpravitTicket;
                ticketWindow.ShowDialog();
            }
            else
                MessageBox.Show(jazyk.Message_TerpUpdate, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void zmenZakaznika_Click(object sender, EventArgs e)
        {
            SeznamZak szk = new SeznamZak(this);
            szk.StartPosition = FormStartPosition.Manual;
            szk.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            if (DialogResult.OK == szk.ShowDialog())
            {
                Zakaznik z = new Zakaznik(this);
                z.StartPosition = FormStartPosition.Manual;
                z.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                z.textBox1.Text = tempZak;
                string velikostS = "";
                switch (Zakaznici.DejVelikost(tempZak))
                {
                    case 127:
                        velikostS = "Custom";
                        break;
                    case 2:
                        velikostS = jazyk.Windows_Mala;
                        break;
                    case 1:
                        velikostS = jazyk.Windows_Stredni;
                        break;
                    case 0:
                        velikostS = jazyk.Windows_Velka;
                        break;
                }
                if (velikostS != "Custom")
                    z.comboBox1.SelectedItem = velikostS;
                else
                {
                    z.comboBox1.SelectedItem = null;
                    if (!z.comboBox2.Items.Contains(Zakaznici.GetTerp(tempZak)))
                        z.comboBox2.Items.Add(Zakaznici.GetTerp(tempZak));
                    z.comboBox2.SelectedItem = Zakaznici.GetTerp(tempZak);
                }

                if (DialogResult.OK == z.ShowDialog())
                {

                    if (zakaznik == tempZak)
                        list.ZmenZakaznika(zakaznik, velikost, zakaznikTerp);
                    else
                    {
                        list.ZmenZakaznika(tempZak, zakaznik);
                        list.ZmenZakaznika(zakaznik, velikost, zakaznikTerp);
                    }
                }
            }
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportWindow reporty = new ReportWindow(this);
            reporty.StartPosition = FormStartPosition.Manual;
            reporty.Location = new Point(this.Location.X + 50, this.Location.Y);
            reporty.Show();
        }

        private void leden_DoubleClick(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedIndices.Count != 0 && ((Tag)((ListView)sender).SelectedItems[0].Tag).IDlong != -1)
            {
                toolStripButton2_Click(sender, e);
            }
        }

        internal DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        private void exportovatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //exportovat za určité období - minulý týden, tento týden, vybraná doba
            
            Export export = new Export(this);
            export.StartPosition = FormStartPosition.Manual;
            export.Location = new Point(this.Location.X + 50, this.Location.Y + 60);
            export.ShowDialog();
        }

        private void hledat_Click(object sender, EventArgs e)
        {
            //vyhledávání ticketu podle kritérií - nová verze
           
            Search s = new Search(this);
            s.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            s.ShowDialog();
        }

        bool canChange = true;

        private void leden_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (canChange)
            {
                canChange = false;
                if (e.ColumnIndex != 0)
                {
                    int sirka = 0;
                    foreach (TabPage tp in tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey(vybranyMesic))
                        {
                            sirka = ((ListView)tp.Controls[vybranyMesic]).Columns[e.ColumnIndex].Width;
                            break;
                        }
                    }

                    switch(e.ColumnIndex)
                    {
                        case 1 :
                            Properties.Settings.Default.colPC = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 2:
                            Properties.Settings.Default.colID = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 3:
                            Properties.Settings.Default.colZak = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 4:
                            Properties.Settings.Default.colPop = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 5:
                            Properties.Settings.Default.colKon = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 6:
                            Properties.Settings.Default.colOd = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 7:
                            Properties.Settings.Default.colDo = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 8:
                            Properties.Settings.Default.colPau = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 9:
                            Properties.Settings.Default.colStav = sirka;
                            Properties.Settings.Default.Save();
                            break;
                        case 10:
                            Properties.Settings.Default.colPoz = sirka;
                            Properties.Settings.Default.Save();
                            break;
                    }

                    foreach (TabPage tp in tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey("leden"))
                        {
                            ((ListView)tp.Controls["leden"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("unor"))
                        {
                            ((ListView)tp.Controls["unor"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("brezen"))
                        {
                            ((ListView)tp.Controls["brezen"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("duben"))
                        {
                            ((ListView)tp.Controls["duben"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("kveten"))
                        {
                            ((ListView)tp.Controls["kveten"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("cerven"))
                        {
                            ((ListView)tp.Controls["cerven"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("cervenec"))
                        {
                            ((ListView)tp.Controls["cervenec"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("srpen"))
                        {
                            ((ListView)tp.Controls["srpen"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("zari"))
                        {
                            ((ListView)tp.Controls["zari"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("rijen"))
                        {
                            ((ListView)tp.Controls["rijen"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("listopad"))
                        {
                            ((ListView)tp.Controls["listopad"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                        if (tp.Controls.ContainsKey("prosinec"))
                        {
                            ((ListView)tp.Controls["prosinec"]).Columns[e.ColumnIndex].Width = sirka;
                        }
                    }
                }
                else
                    foreach (TabPage tp in tabControl1.Controls)
                    {
                        if (tp.Controls.ContainsKey(vybranyMesic))
                        {
                            ((ListView)tp.Controls[vybranyMesic]).Columns[e.ColumnIndex].Width = 0;
                            break;
                        }
                    }

                canChange = true;
            }
        }
        internal void Kopirovat(long idt = -2, DateTime dt = new DateTime(), string text = "", bool soubor = false, bool search = false)
        {
            if (!search)
            {
                foreach (TabPage tp in tabControl1.Controls)
                {
                    if (tp.Controls.ContainsKey(vybranyMesic))
                    {
                        if (((ListView)tp.Controls[vybranyMesic]).SelectedItems.Count != 0)
                        {
                            if (((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).IDlong == -1)
                                return;
                            Ticket refer = null;
                            Dictionary<string, List<Ticket>> tempD;
                            if (dt.CompareTo(new DateTime()) == 0)
                                tempD = poDnech[((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).Datum];
                            else
                                tempD = poDnech[dt];
                            string txt;
                            if (text == "")
                                txt = ((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].SubItems[3].Text;
                            else
                                txt = text;
                            foreach (Ticket t in tempD[txt])
                            {
                                long id;
                                if (idt != -2)
                                    id = idt;
                                else
                                    id = ((Tag)((ListView)tp.Controls[vybranyMesic]).SelectedItems[0].Tag).IDlong;

                                if (t.IDtick == id)
                                {
                                    refer = t;
                                    break;
                                }
                            }

                            NbtList list;
                            if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                                list = file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(txt).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety");
                            else
                                list = file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(rokVyber.SelectedItem.ToString()).Get<NbtCompound>(txt).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety");

                            foreach (NbtCompound c in list)
                            {
                                if (c.Get<NbtLong>("IDlong").Value == refer.IDtick)
                                {
                                    if (!soubor)
                                    {
                                        string tmpTerp = "";
                                        string tmpTask = "";
                                        if (c.Get<NbtString>("Terp") != null)
                                            tmpTerp = c.Get<NbtString>("Terp").Value;
                                        if (c.Get<NbtString>("Task") != null)
                                            tmpTask = c.Get<NbtString>("Task").Value;
                                        TicketWindow ticketWindow = new TicketWindow(this, true, false, null, tmpTerp, tmpTask);
                                        ticketWindow.StartPosition = FormStartPosition.Manual;
                                        ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                                        ticketWindow.Text = jazyk.Windows_NovyTicket;
                                        ticketWindow.zacatek.Text = DateTime.Now.ToString("H:mm");
                                        ticketWindow.idTicketu.Text = c.Get<NbtString>("ID").Value;
                                        ticketWindow.zakaznik.SelectedItem = txt;
                                        ticketWindow.popis.Text = c.Get<NbtString>("Popis").Value;
                                        ticketWindow.richTextBox1.Text = c.Get<NbtString>("Poznamky").Value;
                                        ticketWindow.kontakt.Text = c.Get<NbtString>("Kontakt").Value;
                                        ticketWindow.pocitac.Text = c.Get<NbtString>("PC").Value;
                                        
                                        ticketWindow.terpKod.Text = ticketWindow.DejTerp();
                                        if (Properties.Settings.Default.onlineTerp)
                                        {
                                            tmpTerp = ticketWindow.terpKod.Text.Split('|')[0].Remove(ticketWindow.terpKod.Text.Split('|')[0].Length - 1); ;
                                            tmpTask = ticketWindow.terpKod.Text.Split('|')[1].Remove(0, 1);
                                            foreach (string s in ticketWindow.onlineTerpDropDown.Items)
                                            {
                                                if (s.StartsWith(tmpTerp + " - ") || tmpTerp == s)
                                                {
                                                    ticketWindow.onlineTerpDropDown.SelectedItem = s;
                                                    break;
                                                }
                                            }
                                            foreach (string s in ticketWindow.onlineTaskComboBox.Items)
                                            {
                                                if (s.StartsWith(tmpTask + " - ")|| tmpTask == s)
                                                {
                                                    ticketWindow.onlineTaskComboBox.SelectedItem = s;
                                                    break;
                                                }
                                            }
                                        }
                                        ticketWindow.ShowDialog();
                                    }
                                    else
                                    {
                                        copy = refer.GetNbtObject();
                                        zakaznikVlozit = refer.Zakaznik;
                                        MessageBox.Show(jazyk.Message_Zkopirovan);
                                        Clipboard.SetText(refer.ID + "\t" + refer.Zakaznik + "\t" + refer.PC + "\t" + refer.Popis);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //kopírování ze search
                
                NbtList list;
                if (file.RootTag.Get<NbtInt>("verze").Value < 10100)
                    list = file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety");
                else
                    list = file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(dt.Year.ToString()).Get<NbtCompound>(text).Get<NbtCompound>(vybranyMesic).Get<NbtList>("Tickety");

                foreach (NbtCompound c in list)
                {
                    if (c.Get<NbtLong>("IDlong").Value == idt)
                    {
                        if (!soubor)
                        {
                            string tmpTerp = "";
                            string tmpTask = "";
                            if (c.Get<NbtString>("Terp") != null)
                                tmpTerp = c.Get<NbtString>("Terp").Value;
                            if (c.Get<NbtString>("Task") != null)
                                tmpTask = c.Get<NbtString>("Task").Value;
                            TicketWindow ticketWindow = new TicketWindow(this, true, false, null, tmpTerp, tmpTask);
                            ticketWindow.StartPosition = FormStartPosition.Manual;
                            ticketWindow.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
                            ticketWindow.Text = jazyk.Windows_NovyTicket;
                            ticketWindow.zacatek.Text = DateTime.Now.ToString("H:mm");
                            ticketWindow.idTicketu.Text = c.Get<NbtString>("ID").Value;
                            ticketWindow.zakaznik.SelectedItem = text;
                            ticketWindow.popis.Text = c.Get<NbtString>("Popis").Value;
                            ticketWindow.richTextBox1.Text = c.Get<NbtString>("Poznamky").Value;
                            ticketWindow.kontakt.Text = c.Get<NbtString>("Kontakt").Value;
                            ticketWindow.pocitac.Text = c.Get<NbtString>("PC").Value;
                            ticketWindow.terpKod.Text = ticketWindow.DejTerp();
                            if (Properties.Settings.Default.onlineTerp)
                            {
                                tmpTerp = ticketWindow.terpKod.Text.Split('|')[0].Remove(ticketWindow.terpKod.Text.Split('|')[0].Length-1);
                                tmpTask = ticketWindow.terpKod.Text.Split('|')[1].Remove(0,1);
                                foreach (string s in ticketWindow.onlineTerpDropDown.Items)
                                {
                                    if (s.StartsWith(tmpTerp + " - ") || tmpTerp == s)
                                    {
                                        ticketWindow.onlineTerpDropDown.SelectedItem = s;
                                        break;
                                    }
                                }
                                foreach (string s in ticketWindow.onlineTaskComboBox.Items)
                                {
                                    if (s.StartsWith(tmpTask + " - ") || tmpTask == s)
                                    {
                                        ticketWindow.onlineTaskComboBox.SelectedItem = s;
                                        break;
                                    }
                                }
                            }
                            ticketWindow.ShowDialog();
                        }
                        break;
                    }
                }
            }
            
        }

        private void přidatTERPKódToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TERP terp = new TERP(this);
            terp.Text = jazyk.Windows_NovyTerp;
            terp.StartPosition = FormStartPosition.Manual;
            terp.Size = new System.Drawing.Size(300, 117);
            terp.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            terp.ShowDialog();
        }

        private void upravitTERPKódToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TERP terp = new TERP(this, 1);
            terp.Text = jazyk.Windows_UpravitTerp;
            terp.StartPosition = FormStartPosition.Manual;
            terp.Size = new System.Drawing.Size(371, 117);
            terp.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            terp.ShowDialog();
        }

        private void smazatTERPKódToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TERP terp = new TERP(this, 2);
            terp.Text = jazyk.Windows_SmazatTerp;
            terp.StartPosition = FormStartPosition.Manual;
            terp.Size = new System.Drawing.Size(300, 117);
            terp.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            terp.ShowDialog();
        }

        private void knownIssuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "";
                if (!Properties.Settings.Default.onlineTerp)
                    text = File.ReadAllText(Properties.Settings.Default.updateCesta + @"\..\known_errors.txt");
                else
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    text = wc.DownloadString(Properties.Settings.Default.ZalozniUpdate + @"\known_errors.txt");
                }
                TextWindow tw = new TextWindow();
                tw.richTextBox1.Text = text;
                tw.Text = jazyk.Menu_ZnameProblemy;
                tw.ShowDialog();
            }
            catch 
            { 
                MessageBox.Show(jazyk.Error_NejdeOtevritSoubor, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Logni("Nemohu otevřít soubor chyb.", LogMessage.WARNING);
            }
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "";
                if (!Properties.Settings.Default.onlineTerp)
                    text = File.ReadAllText(Properties.Settings.Default.updateCesta + @"\..\Changelog.txt");
                else
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    text = wc.DownloadString(Properties.Settings.Default.ZalozniUpdate + @"\Changelog.txt");
                }
                TextWindow tw = new TextWindow();
                tw.richTextBox1.Text = text;
                tw.Text = jazyk.Menu_Changelog;
                tw.ShowDialog();
            }
            catch 
            { 
                MessageBox.Show(jazyk.Error_NejdeOtevritSoubor, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Logni("Nemohu otevřít soubor changelogu.", LogMessage.WARNING);
            }
        }

        private void plányDoBudoucnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "";
                if (!Properties.Settings.Default.onlineTerp)
                    text = File.ReadAllText(Properties.Settings.Default.updateCesta + @"\..\Future.txt");
                else
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    text = wc.DownloadString(Properties.Settings.Default.ZalozniUpdate + @"\Future.txt");
                }
                TextWindow tw = new TextWindow();
                tw.richTextBox1.Text = text;
                tw.Text = jazyk.Menu_Plany;
                tw.ShowDialog();
            }
            catch 
            { 
                MessageBox.Show(jazyk.Error_NejdeOtevritSoubor, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Logni("Nemohu otevřít soubor plánů.", LogMessage.WARNING);
            }
        }

        private string Cas(Ticket t)
        {
            DateTime pauzaCelkem;

            int phod = 0, pmin = 0, index = 0;
            foreach (DateTime dt in t.PauzyOd)
            {
                DateTime tmp = new DateTime(t.Datum.Year, t.Datum.Month, t.Datum.Day, phod, pmin, 0).AddHours(t.PauzyDo[index].Hour - t.PauzyOd[index].Hour);
                tmp = tmp.AddMinutes(t.PauzyDo[index].Minute - t.PauzyOd[index].Minute);

                phod = tmp.Hour;
                pmin = tmp.Minute;
                index++;
            }
            pauzaCelkem = new DateTime(t.Datum.Year, t.Datum.Month, t.Datum.Day, phod, pmin, 0);

            try
            {
                string[] casOd = t.Od.ToString("H:mm").Split(':');
                string[] casDo = t.Do.ToString("H:mm").Split(':');

                DateTime odpracovanoCiste = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(casDo[0]) - int.Parse(casOd[0]), 0, 0);
                int min = int.Parse(casDo[1]) - int.Parse(casOd[1]);
                odpracovanoCiste = odpracovanoCiste.AddMinutes(min);
                odpracovanoCiste = odpracovanoCiste.AddHours(-pauzaCelkem.Hour);
                odpracovanoCiste = odpracovanoCiste.AddMinutes(-pauzaCelkem.Minute);

                DateTime odpracovanoEdit = RoundUp(odpracovanoCiste, TimeSpan.FromMinutes(30));

                return odpracovanoEdit.ToString("H:mm");

            }
            catch
            {
                return "0:00";
            }
        }

        internal string[] DejTerp(Ticket t)
        {
            string[] terp = new string[2]{ "-", "-"};

            if (t.CustomTerp == "" && t.CustomTask == "")
            {
                if (t.ID.StartsWith("INC") || t.ID.StartsWith("RIT") || t.ID.StartsWith("ITASK") || t.ID.StartsWith("RTASK") || t.ID.StartsWith("TASK"))
                {
                    terp[1] = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                }
                else if (t.ID.StartsWith("PRB") || t.ID.StartsWith("PTASK"))
                    terp[1] = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;

                
                switch (Zakaznici.DejVelikost(t.Zakaznik))
                {
                    case 0:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                        break;
                    case 1:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                        break;
                    case 2:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                        break;
                    case 127:
                        terp[0] = Zakaznici.GetTerp(t.Zakaznik);
                        break;
                    default:
                        terp[0] = "-";
                        break;
                        
                }
                if (t.Terp == Ticket.TerpKod.Enkripce || t.Terp == Ticket.TerpKod.EnkripceHoliday || t.Terp == Ticket.TerpKod.EnkripceProblem)
                    terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Encrypce").StringValue;
                else if (t.Terp == Ticket.TerpKod.Mobility || t.Terp == Ticket.TerpKod.MobilityHoliday || t.Terp == Ticket.TerpKod.MobilityProblem)
                    terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Mobility").StringValue;
            }
            else if (t.CustomTerp == "")
            {
                terp[1] = t.CustomTask;

                switch (Zakaznici.DejVelikost(t.Zakaznik))
                {
                    case 0:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                        break;
                    case 1:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                        break;
                    case 2:
                        terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                        break;
                    case 127:
                        terp[0] = Zakaznici.GetTerp(t.Zakaznik);
                        break;
                    default:
                        terp[0] = "-";
                        break;

                }
                if (t.Terp == Ticket.TerpKod.Enkripce || t.Terp == Ticket.TerpKod.EnkripceHoliday || t.Terp == Ticket.TerpKod.EnkripceProblem)
                    terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Encrypce").StringValue;
                else if (t.Terp == Ticket.TerpKod.Mobility || t.Terp == Ticket.TerpKod.MobilityHoliday || t.Terp == Ticket.TerpKod.MobilityProblem)
                    terp[0] = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Mobility").StringValue;
            }
            else if (t.CustomTask == "")
            {
                if (t.ID.StartsWith("INC") || t.ID.StartsWith("RIT") || t.ID.StartsWith("ITASK") || t.ID.StartsWith("RTASK") || t.ID.StartsWith("TASK"))
                {
                    terp[1] = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                }
                else if (t.ID.StartsWith("PRB") || t.ID.StartsWith("PTASK"))
                    terp[1] = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;


                terp[0] = t.CustomTerp;
            }
            else
            {
                terp[0] = t.CustomTerp;
                terp[1] = t.CustomTask;
            }

            return terp;
        }
        

        private void dokumentaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(@"\\10.14.18.19\shareforyou\tools\Ticketnik\Ticketník.docx");
            }
            catch 
            { 
                MessageBox.Show(jazyk.Error_NejdeOtevritSoubor, jazyk.Error_NejdeOtevrit, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Logni("Nemohu otevřít soubor dokumentace.", LogMessage.WARNING);
            }
        }

        private bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Rectangle formRectangle = new Rectangle(form.Left, form.Top, 400, form.Height);

                if (screen.WorkingArea.Contains(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            keyDown = false;
        }

        internal Color ContrastColor(Color color)
        {
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            //Součet násobků je 0.999
            //původní hodnota
            //double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
            //hodnota upravená pro původní barvy
            double a = 1 - (0.26 * color.R + 0.638 * color.G + 0.102 * color.B) / 255;

            if (a < 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return Color.FromArgb(d, d, d);
        }

        internal enum LogMessage
        {
            ERROR,
            WARNING,
            INFO
        }

        internal void Logni(string zprava, LogMessage typ)
        {
            if (!Directory.Exists(appdata + "\\Ticketnik"))
            {
                Directory.CreateDirectory(appdata + "\\Ticketnik");
            }
            if (!Directory.Exists(appdata + "\\Ticketnik\\Logs"))
            {
                Directory.CreateDirectory(appdata + "\\Ticketnik\\Logs");
            }
            CheckLog();
            DateTime dt = DateTime.Now;
            string dat = dt.ToString("dd.MM.yyyy H:mm:ss.") + dt.Millisecond.ToString("000");
            string t = " ";
            if (typ == LogMessage.INFO)
                t = " [INFO] ";
            else if (typ == LogMessage.WARNING)
                t = " [WARNING] ";

            if (typ == LogMessage.ERROR)
            {
                t = " [ERROR] ";
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", Application.ProductVersion + "\r\n");
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "[" + dat + "]" + t + zprava + "\r\n\r\n");
            }
            else
            {
                File.AppendAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "[" + dat + "]" + t + zprava + "\r\n\r\n");
            }
        }

        private void toolStripMenu_Napoveda_Click(object sender, EventArgs e)
        {
            if (!helpOpen)
            {
                try
                {
                    Napoveda napoveda = new Napoveda(this);
                    napoveda.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(jazyk.Error_NejdeOtevritNapoveda);
                    Logni("Nelze otevřít nápovědu\r\n\r\n" + ex.Message + "\r\n\r\n" + ex.StackTrace, LogMessage.ERROR);
                }
            }
        }

        //kopírování right klikem podle sloupoců
        //PC, ID, Zak, Pop, Kon, Od, Do, Pau, Stav, Poz
        private void leden_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (((Tag)((ListView)sender).SelectedItems[0].Tag).IDlong != -1)
                {
                    try
                    {
                        ListViewHitTestInfo click = ((ListView)sender).HitTest(e.X, e.Y);
                        if (Control.ModifierKeys == Keys.Shift)
                            Clipboard.SetText(Clipboard.GetText() + "\t" + click.SubItem.Text);
                        else if (Control.ModifierKeys == Keys.Control)
                            Clipboard.SetText(Clipboard.GetText() + "\r\n" + click.SubItem.Text);
                        else
                            Clipboard.SetText(click.SubItem.Text);
                        
                    }
                    catch 
                    { 
                        //pokud je text prázdný (vyhodí exception) a vloží prázdný text

                        if (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control)
                        {
                            Clipboard.SetText(" "); 
                        }
                    }
                }
            }
        }

        private void CheckLog()
        {
            if (Directory.Exists(appdata + "\\Ticketnik\\Logs"))
            {
                if(File.Exists(appdata + "\\Ticketnik\\Logs\\Error.log"))
                {
                    FileInfo fi = new FileInfo(appdata + "\\Ticketnik\\Logs\\Error.log");
                    if (fi.Length > 200000)
                    {

                        File.Copy(appdata + "\\Ticketnik\\Logs\\Error.log", appdata + "\\Ticketnik\\Logs\\Error_" + DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss") + ".log");
                        File.Delete(appdata + "\\Ticketnik\\Logs\\Error.log");
                        File.WriteAllText(appdata + "\\Ticketnik\\Logs\\Error.log", "");

                        
                    }
                    DirectoryInfo directory = new DirectoryInfo(appdata + "\\Ticketnik\\Logs");
                    FileInfo[] query = directory.GetFiles("Error*", SearchOption.TopDirectoryOnly);
                    foreach (FileInfo file in query.OrderByDescending(file => file.LastWriteTime).Skip(3))
                    {
                        file.Delete();
                    }
                }
                if(File.Exists(appdata + "\\Ticketnik\\Logs\\Ticketnik.log"))
                {
                    FileInfo fi = new FileInfo(appdata + "\\Ticketnik\\Logs\\Ticketnik.log");
                    if (fi.Length > 200000)
                    {

                        File.Copy(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", appdata + "\\Ticketnik\\Logs\\Ticketnik" + DateTime.Now.ToString("yyyy-MM-dd_H-mm-ss") + ".log");
                        File.Delete(appdata + "\\Ticketnik\\Logs\\Ticketnik.log");
                        File.WriteAllText(appdata + "\\Ticketnik\\Logs\\Ticketnik.log", "");

                        
                    }
                    DirectoryInfo directory = new DirectoryInfo(appdata + "\\Ticketnik\\Logs");
                    FileInfo[] query = directory.GetFiles("Ticketnik*", SearchOption.TopDirectoryOnly);
                    foreach (FileInfo file in query.OrderByDescending(file => file.LastWriteTime).Skip(3))
                    {
                        file.Delete();
                    }
                }
            }
        }

        internal void SetJazyk()
        {
            this.souborToolStripMenuItem.Text = jazyk.Menu_Soubor;
            this.novýToolStripMenuItem.Text = jazyk.Menu_Novy;
            this.načístToolStripMenuItem.Text = jazyk.Menu_Otevrit;
            this.uložitToolStripMenuItem.Text = jazyk.Menu_Ulozit;
            this.exportovatToolStripMenuItem.Text = jazyk.Menu_Exportovat;
            this.ukončitToolStripMenuItem.Text = jazyk.Menu_Zavrit;
            this.možnostiToolStripMenuItem.Text = jazyk.Menu_Moznosti;
            this.nastaveníToolStripMenuItem.Text = jazyk.Menu_Nastaveni;
            this.přidatZákazníkaToolStripMenuItem.Text = jazyk.Menu_PridatZakaznika;
            this.upravitZákazníkaToolStripMenuItem.Text = jazyk.Menu_UpravitZakaznika;
            this.smazatZákazníkaToolStripMenuItem.Text = jazyk.Menu_SmazatZakaznika;
            this.přidatTERPKódToolStripMenuItem.Text = jazyk.Menu_PridatTerp;
            this.upravitTERPKódToolStripMenuItem.Text = jazyk.Menu_UpravitTerp;
            this.smazatTERPKódToolStripMenuItem.Text = jazyk.Menu_SmazatTerp;
            this.hledatToolStripMenuItem.Text = jazyk.Menu_Hledat;
            this.reportToolStripMenuItem.Text = jazyk.Menu_Report;
            this.sourceToolStripMenuItem.Text = jazyk.Menu_Source;
            this.knownIssuesToolStripMenuItem.Text = jazyk.Menu_ZnameProblemy;
            this.changelogToolStripMenuItem.Text = jazyk.Menu_Changelog;
            this.plányDoBudoucnaToolStripMenuItem.Text = jazyk.Menu_Plany;
            this.dokumentaceToolStripMenuItem.Text = jazyk.Menu_Dokumentace;
            this.oProgramuToolStripMenuItem.Text = jazyk.Menu_About;
            this.toolStripMenu_Napoveda.Text = jazyk.Menu_Help;
            this.toolStripMenu_Napoveda.ToolTipText = jazyk.Menu_Help;
            this.toolStripButton1.Text = jazyk.SideMenu_PridatZaznam;
            this.toolStripButton2.Text = jazyk.SideMenu_UpravitZaznam;
            this.toolStripButton3.Text = jazyk.SideMenu_SmazatZaznam;
            this.toolStripButton4.Text = jazyk.SideMenu_PridatZakaznika;
            this.zmenZakaznika.Text = jazyk.SideMenu_UpravitZakaznika;
            this.toolStripButton6.Text = jazyk.SideMenu_SmazatZakaznika;
            this.hledat.Text = jazyk.SideMenu_Hledat;
            this.toolStripButton_Napoveda.Text = jazyk.SideMenu_Napoveda;
            this.ledenT.Text = jazyk.Month_Leden;
            this.columnHeader1.Text = jazyk.Header_PC;
            this.columnHeader2.Text = jazyk.Header_TicketID;
            this.columnHeader3.Text = jazyk.Header_Zakaznik;
            this.columnHeader4.Text = jazyk.Header_Popis;
            this.columnHeader5.Text = jazyk.Header_Kontakt;
            this.columnHeader6.Text = jazyk.Header_Terp;
            this.columnHeader7.Text = jazyk.Header_Task;
            this.columnHeader8.Text = jazyk.Header_Cas;
            this.columnHeader9.Text = jazyk.Header_Stav;
            this.columnHeader10.Text = jazyk.Header_Poznamka;
            this.unorT.Text = jazyk.Month_Unor;
            this.brezenT.Text = jazyk.Month_Brezen;
            this.dubenT.Text = jazyk.Month_Duben;
            this.kvetenT.Text = jazyk.Month_Kveten;
            this.cervenT.Text = jazyk.Month_Cerven;
            this.cervenecT.Text = jazyk.Month_Cervenec;
            this.srpenT.Text = jazyk.Month_Srpen;
            this.zariT.Text = jazyk.Month_Zari;
            this.rijenT.Text = jazyk.Month_Rijen;
            this.listopadT.Text = jazyk.Month_Listopad;
            this.prosinecT.Text = jazyk.Month_Prosinec;
            this.Text = jazyk.Header_Tickety;// +": " + jmenoSouboru.Remove(0, jmenoSouboru.LastIndexOf('\\') + 1).Replace(".tic", "");
            this.převéstNaFormátMilleniumToolStripMenuItem.Text = jazyk.Menu_PrevestNaMillenium;
            this.upozorněníToolStripMenuItem.Text = toolStripButton8.Text = jazyk.Menu_Upozorneni;
            this.dostupnéJazykyToolStripMenuItem.Text = jazyk.Menu_DostupneJazyky;
            this.vyhledatAktualizaceToolStripMenuItem.Text = jazyk.Menu_HledejAktualizace;
            this.aktualizovatVšechnyTerpyToolStripMenuItem.Text = jazyk.Menu_AktualizovatTerpyOnline;
        }

        private void rokVyber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (muze)
            {
                uložitToolStripMenuItem_Click(this, null);
                LoadFile();
            }
        }

        private void převéstNaFormátMilleniumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //převod z původního formátu na Millenium
            File.Copy(jmenoSouboru, jmenoSouboru.Replace(".tic", ".old"), true);

            NbtFile newFile = new NbtFile();
            newFile = new NbtFile();
            newFile.RootTag.Add(new NbtCompound("Zakaznici"));
            newFile.RootTag.Add(new NbtLong("MaxID", file.RootTag.Get<NbtLong>("MaxID").Value));
            newFile.RootTag.Add(new NbtInt("verze", saveFileVersion));

            foreach(NbtCompound convZak in file.RootTag.Get<NbtCompound>("Zakaznici"))
            {
                //int rok = 0;
                string zakaznik = convZak.Name;

                foreach(NbtCompound convMes in convZak)
                {
                    string mesicTag = convMes.Name;
                    string mesicName = convMes.Get<NbtString>("Mesic").Value;

                    if(convMes.Get<NbtList>("Tickety") != null)
                    {
                        foreach(NbtCompound convTic in convMes.Get<NbtList>("Tickety"))
                        {
                            short newRok = convTic.Get<NbtShort>("Rok").Value;
                            if (newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()) == null)
                                newFile.RootTag.Get<NbtCompound>("Zakaznici").Add(new NbtCompound(newRok.ToString()));
                            if (newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik) == null)
                                newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Add(new NbtCompound(zakaznik));

                            if (newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(mesicTag) == null)
                            {
                                newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik).Add(new NbtCompound(mesicTag));
                                newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(mesicTag).Add(new NbtString("Mesic", mesicName));
                                newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(mesicTag).Add(new NbtList("Tickety", NbtTagType.Compound));

                            }
                            NbtCompound newTic = new NbtCompound(convTic);
                            newFile.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(newRok.ToString()).Get<NbtCompound>(zakaznik).Get<NbtCompound>(mesicTag).Get<NbtList>("Tickety").Add(newTic);
                        }
                    }
                }
                
            }
            file = newFile;
            uložitToolStripMenuItem_Click(this, null);
            LoadFile();
            MessageBox.Show(jazyk.Message_FormatNovy + jmenoSouboru.Remove(0, jmenoSouboru.LastIndexOf('\\') + 1).Replace(".tic", ".old"));
        }

        private void leden_ColumnReordered(object sender, ColumnReorderedEventArgs e)
        {
            if (e.NewDisplayIndex != 0)
            {
                foreach(ColumnHeader ch in ((ListView)tabControl1.Controls["ledenT"].Controls["leden"]).Columns)
                {
                    if (ch.Text != e.Header.Text && ch.DisplayIndex != 0)
                    {
                        int i = 0;
                        if (ch.DisplayIndex > e.OldDisplayIndex && ch.DisplayIndex <= e.NewDisplayIndex)
                            i = -1;
                        else if (ch.DisplayIndex < e.OldDisplayIndex && ch.DisplayIndex >= e.NewDisplayIndex)
                            i = 1;
                        
                        if (ch.Text == jazyk.Header_TicketID)
                            Properties.Settings.Default.colIDPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_PC)
                            Properties.Settings.Default.colPCPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Zakaznik)
                            Properties.Settings.Default.colZakPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Popis)
                            Properties.Settings.Default.colPopPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Kontakt)
                            Properties.Settings.Default.colKonPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Terp)
                            Properties.Settings.Default.colTerpPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Task)
                            Properties.Settings.Default.colTaskPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Cas)
                            Properties.Settings.Default.colCasPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Stav)
                            Properties.Settings.Default.colStavPoradi = ch.DisplayIndex + i;
                        else if (ch.Text == jazyk.Header_Poznamka)
                            Properties.Settings.Default.colPozPoradi = ch.DisplayIndex + i;
                    }
                    else if(ch.Text == e.Header.Text)
                    {
                        if (ch.Text == jazyk.Header_TicketID)
                            Properties.Settings.Default.colIDPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_PC)
                            Properties.Settings.Default.colPCPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Zakaznik)
                            Properties.Settings.Default.colZakPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Popis)
                            Properties.Settings.Default.colPopPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Kontakt)
                            Properties.Settings.Default.colKonPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Terp)
                            Properties.Settings.Default.colTerpPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Task)
                            Properties.Settings.Default.colTaskPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Cas)
                            Properties.Settings.Default.colCasPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Stav)
                            Properties.Settings.Default.colStavPoradi = e.NewDisplayIndex;
                        else if (ch.Text == jazyk.Header_Poznamka)
                            Properties.Settings.Default.colPozPoradi = e.NewDisplayIndex;
                    }
                }

                Properties.Settings.Default.Save();
                e.Cancel = true;
                NastavSirku();
            }
            else
                e.Cancel = true;
        }

        private void upozorněníToolStripMenuItem_Click(object sender, EventArgs e)
        {
            upozozrneniMuze = false;
            Upozorneni upozorneni = new Upozorneni(this);
            upozorneni.StartPosition = FormStartPosition.Manual;
            upozorneni.Location = new Point(this.Location.X + 50, this.Location.Y+50);
            upozorneni.ShowDialog();
        }

        private void timerUpozorneni_Tick(object sender, EventArgs e)
        {
            if (upozozrneniMuze)
            {
                upozozrneniMuze = false;
                foreach (UpozorneniCls upo in upozorneni)
                {
                    string text = "";
                    Image i = Properties.Resources.bell_128; ;
                    if (upo.Datum <= DateTime.Now)
                    {
                        switch (upo.TypUpozorneni)
                        {
                            case UpozorneniCls.Typ.RDP:
                                text = jazyk.Windows_Upozorneni_RDP;
                                i = Properties.Resources.remote_128;
                                break;
                            case UpozorneniCls.Typ.Upozorneni:
                                text = jazyk.Windows_Upozorneni_Upo;
                                i = Properties.Resources.bell_128;
                                break;
                            default:
                                text = jazyk.Windows_Upozorneni_Upo;
                                i = Properties.Resources.bell_128;
                                break;
                        }

                        NotificationMessageBox nmb = new NotificationMessageBox();
                        nmb.Set(upo.Popis, text, i);
                        nmb.StartPosition = FormStartPosition.Manual;
                        
                        Screen[] screens = Screen.AllScreens;
                        foreach (Screen screen in screens)
                        {
                            Rectangle formRectangle = new Rectangle(this.Left, this.Top, 1, 1);

                            if (screen.WorkingArea.Contains(formRectangle))
                            {
                                nmb.Location = new Point((screen.WorkingArea.Width / 2) - (nmb.Width / 2), (screen.WorkingArea.Height / 2) - (nmb.Height / 2));
                            }
                        }


                        if (DialogResult.OK == nmb.ShowDialog())
                        {
                            UpozorneniCls.Remove(upo);
                            upozorneni = UpozorneniCls.UpozorneniList;
                        }
                        else
                        {
                            UpozorneniCls.Upravit(upo, new UpozorneniCls(DateTime.Now.AddMinutes(10), upo.Popis, upo.TypUpozorneni));
                            upozorneni = UpozorneniCls.UpozorneniList;
                        }
                    }

                    
                }
                upozozrneniMuze = true;
            }
        }

        private void dostupnéJazykyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravceJazyka spravce = new SpravceJazyka(this);
            spravce.StartPosition = FormStartPosition.Manual;
            spravce.Location = new Point(this.Location.X + 50, this.Location.Y + 50);
            spravce.ShowDialog();
        }

        private void Timer_ClearInfo_Tick(object sender, EventArgs e)
        {
            infoBox.Text = "";
            timer_ClearInfo.Stop();
        }

        private void aktualizovatVšechnyTerpyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AktualizujTerpyTasky();
        }

        private void vyhledatAktualizaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!updateRunning)
            {
                Aktualizace(devtest);
            }
        }
    }
}
