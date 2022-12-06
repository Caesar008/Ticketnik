using System;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class Preklad : Form
    {
        Jazyk jazyk = new Jazyk(true);
        public Preklad(bool update = false)
        {
            InitializeComponent();
            lbl_about.Text = jazyk.Menu_About;
            lbl_addCustomer.Text = jazyk.Menu_PridatZakaznika;
            lbl_addTerp.Text = jazyk.Menu_PridatTerp;
            lbl_availableLanguages.Text = jazyk.Menu_DostupneJazyky;
            lbl_close.Text = jazyk.Menu_Zavrit;
            lbl_convert.Text = jazyk.Menu_PrevestNaMillenium;
            lbl_deleteCustomer.Text = jazyk.Menu_SmazatZakaznika;
            lbl_deleteTerp.Text = jazyk.Menu_SmazatTerp;
            lbl_documentation.Text = jazyk.Menu_Dokumentace;
            lbl_editCustomer.Text = jazyk.Menu_UpravitZakaznika;
            lbl_editTerp.Text = jazyk.Menu_UpravitTerp;
            lbl_export.Text = jazyk.Menu_Exportovat;
            lbl_File.Text = jazyk.Menu_Soubor;
            lbl_futurePlans.Text = jazyk.Menu_Plany;
            lbl_help.Text = jazyk.Menu_Help;
            lbl_changelog.Text = jazyk.Menu_Changelog;
            lbl_knownIssues.Text = jazyk.Menu_ZnameProblemy;
            lbl_new.Text = jazyk.Menu_Novy;
            lbl_notification.Text = jazyk.Menu_Upozorneni;
            lbl_open.Text = jazyk.Menu_Otevrit;
            lbl_options.Text = jazyk.Menu_Moznosti;
            lbl_report.Text = jazyk.Menu_Report;
            lbl_save.Text = jazyk.Menu_Ulozit;
            lbl_serach.Text = jazyk.Menu_Hledat;
            lbl_settings.Text = jazyk.Menu_Nastaveni;
            lbl_source.Text = jazyk.Menu_Source;
            Motiv.SetMotiv(this);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabList.SelectedTab.Name == "sideMenuItems")
            {
                lbl_sm_addCustomer.Text = jazyk.SideMenu_PridatZakaznika;
                lbl_sm_addRecord.Text = jazyk.SideMenu_PridatZaznam;
                lbl_sm_deleteCustomer.Text = jazyk.SideMenu_SmazatZakaznika;
                lbl_sm_deleteRecord.Text = jazyk.SideMenu_SmazatZaznam;
                lbl_sm_editCustomer.Text = jazyk.SideMenu_UpravitZakaznika;
                lbl_sm_editRecord.Text = jazyk.SideMenu_UpravitZaznam;
                lbl_sm_help.Text = jazyk.SideMenu_Napoveda;
                lbl_sm_search.Text = jazyk.SideMenu_Hledat;
            }
            else if (tabList.SelectedTab.Name == "months")
            {
                lbl_mon_jan.Text = jazyk.Month_Leden;
                lbl_mon_feb.Text = jazyk.Month_Unor;
                lbl_mon_mar.Text = jazyk.Month_Brezen;
                lbl_mon_apr.Text = jazyk.Month_Duben;
                lbl_mon_may.Text = jazyk.Month_Kveten;
                lbl_mon_jun.Text = jazyk.Month_Cerven;
                lbl_mon_jul.Text = jazyk.Month_Cervenec;
                lbl_mon_aug.Text = jazyk.Month_Srpen;
                lbl_mon_sep.Text = jazyk.Month_Zari;
                lbl_mon_oct.Text = jazyk.Month_Rijen;
                lbl_mon_nov.Text = jazyk.Month_Listopad;
                lbl_mon_dec.Text = jazyk.Month_Prosinec;
            }
            else if (tabList.SelectedTab.Name == "headers")
            {
                lbl_head_pc.Text = jazyk.Header_PC;
                lbl_head_customer.Text = jazyk.Header_Zakaznik;
                lbl_head_description.Text = jazyk.Header_Popis;
                lbl_head_kontakt.Text = jazyk.Header_Kontakt;
                lbl_head_note.Text = jazyk.Header_Poznamka;
                lbl_head_notSaved.Text = jazyk.Header_Neulozeno;
                lbl_head_status.Text = jazyk.Header_Stav;
                lbl_head_task.Text = jazyk.Header_Task;
                lbl_head_terp.Text = jazyk.Header_Terp;
                lbl_head_ticketId.Text = jazyk.Header_TicketID;
                lbl_head_tickets.Text = jazyk.Header_Tickety;
                lbl_head_time.Text = jazyk.Header_Cas;
            }
            else if (tabList.SelectedTab.Name == "windows")
            {
                if(tabControl2.SelectedTab.Name == "newFile")
                {
                    lbl_newFile_fileName.Text = jazyk.Windows_JmenoSouboru;
                    lbl_newFile_newFile.Text = jazyk.Windows_NovySoubor;
                } 
                else if (tabControl2.SelectedTab.Name == "open")
                {
                    lbl_open_open.Text = jazyk.Windows_Otevrit;
                    lbl_open_ticketsFile.Text = jazyk.Windows_SouborTicketu;
                }
                else if (tabControl2.SelectedTab.Name == "export")
                {
                    lbl_export_custom.Text = jazyk.Windows_Export_VybraneObdobi;
                    lbl_export_exportToMyTime.Text = jazyk.Windows_Export_Exportovat;
                    lbl_export_lastWeek.Text = jazyk.Windows_Export_MinulyTyden;
                    lbl_export_notEnded.Text = jazyk.Windows_Export_Neukoncen;
                    lbl_export_thisWeek.Text = jazyk.Windows_Export_TentoTyden;
                    lbl_export_ticket.Text = jazyk.Windows_Export_Ticket;
                    lbl_export_youWorkedOn.Text = jazyk.Windows_Export_NaKteremJsiPracoval;
                }
                else if (tabControl2.SelectedTab.Name == "settings")
                {
                    lbl_settings_settings.Text = jazyk.Windows_Nastaveni_Nastaveni;
                    lbl_settings_autosave.Text = jazyk.Windows_Nastaveni_Autosave;
                    lbl_settings_default.Text = jazyk.Windows_Nastaveni_Default;
                    lbl_settings_done.Text = jazyk.Windows_Nastaveni_Vyreseno;
                    lbl_settings_hodin.Text = jazyk.Windows_Nastaveni_Hodin;
                    lbl_settings_hodiny.Text = jazyk.Windows_Nastaveni_Hodiny;
                    lbl_settings_change.Text = jazyk.Windows_Nastaveni_Zmen;
                    lbl_settings_changeLanguage.Text = jazyk.Windows_Nastaveni_ZmenJazyk;
                    lbl_settings_inProgress.Text = jazyk.Windows_Nastaveni_Probiha;
                    lbl_settings_language.Text = jazyk.Windows_Nastaveni_Jazyk;
                    lbl_settings_minutes.Text = jazyk.Windows_Nastaveni_Minut;
                    lbl_settings_over.Text = jazyk.Windows_Nastaveni_Nad;
                    lbl_settings_rdp.Text = jazyk.Windows_Nastaveni_RDP;
                    lbl_settings_saveEvery.Text = jazyk.Windows_Nastaveni_UkladatKazdych;
                    lbl_settings_showTimePerDay.Text = jazyk.Windows_Nastaveni_CasZaDen;
                    lbl_settings_showTimeTotal.Text = jazyk.Windows_Nastaveni_CelkovyCas;
                    lbl_settings_simpleTime.Text = jazyk.Windows_Nastaveni_ZjednodusenyCas;
                    lbl_settings_ticketColor.Text = jazyk.Windows_Nastaveni_BarvyTicketu;
                    lbl_settings_waiting.Text = jazyk.Windows_Nastaveni_CekaSe;
                    lbl_settings_waitingForResponse.Text = jazyk.Windows_Nastaveni_CekaSeNaOdpoved;
                    lbl_settings_onlineTerp.Text = jazyk.Windows_Nastaveni_OnlineTerp;
                }
                else if (tabControl2.SelectedTab.Name == "customer")
                {
                    lbl_customer_customer.Text = jazyk.Windows_Zakaznik;
                    lbl_customer_size.Text = jazyk.Windows_Zakaznik_Velikost;
                    lbl_customer_small.Text = jazyk.Windows_Zakaznik_Maly;
                    lbl_customer_medium.Text = jazyk.Windows_Zakaznik_Stredni;
                    lbl_customer_large.Text = jazyk.Windows_Zakaznik_Velky;
                    lbl_customer_or.Text = jazyk.Windows_Zakaznik_Nebo;
                    lbl_customer_terp.Text = jazyk.Windows_Zakaznik_TerpKod;
                }
                else if (tabControl2.SelectedTab.Name == "terp")
                {
                    lbl_terp_newTerp.Text = jazyk.Windows_NovyTerp;
                    lbl_terp_terpCode.Text = jazyk.Windows_Terp_TerpKod;
                    lbl_terp_task.Text = jazyk.Windows_Terp_Task;
                }
                else if (tabControl2.SelectedTab.Name == "search")
                {
                    lbl_search_search.Text = jazyk.Windows_Search_Hledej;
                    lbl_search_searchCriteria.Text = jazyk.Windows_Search_KriteriaHledani;
                    lbl_search_ticketID.Text = jazyk.Windows_Search_IDTicketu;
                    lbl_search_customer.Text = jazyk.Windows_Search_Zakaznik;
                    lbl_search_description.Text = jazyk.Windows_Search_Popis;
                    lbl_search_contact.Text = jazyk.Windows_Search_Kontakt;
                    lbl_search_note.Text = jazyk.Windows_Search_Poznamka;
                    lbl_search_regex.Text = jazyk.Windows_Search_Regex;
                    lbl_search_date.Text = jazyk.Windows_Search_Datum;
                    lbl_search_terpTask.Text = jazyk.Windows_Search_Pauzy;
                    lbl_search_status.Text = jazyk.Windows_Search_Status;
                    lbl_search_done.Text = jazyk.Windows_Search_Hotovo;
                    lbl_search_waiting.Text = jazyk.Windows_Search_CekaSe;
                    lbl_search_waitingFor.Text = jazyk.Windows_Search_CekaSeNa;
                    lbl_search_rdp.Text = jazyk.Windows_Search_RDP;
                    lbl_search_inProgress.Text = jazyk.Windows_Search_Probiha;
                    lbl_search_searchCriteriaHelp.Text = jazyk.Windows_Search_HledaniNapoveda;
                }
                else if (tabControl2.SelectedTab.Name == "about")
                {
                    lbl_about_version.Text = jazyk.Windows_AboutBox_Verze;
                    lbl_about_searching.Text = jazyk.Windows_AboutBox_Aktualizace;
                    lbl_about_license.Text = jazyk.Windows_AboutBox_Popis;
                }
                else if (tabControl2.SelectedTab.Name == "newTicket")
                {
                    lbl_newTicket_newTicket.Text = jazyk.Windows_NovyTicket;
                    lbl_newTicket_zakladniInformace.Text = jazyk.Windows_Ticket_ZakladniInfo;
                    lbl_newTicket_Pocitac.Text = jazyk.Windows_Ticket_Pocitac;
                    lbl_newTicket_ticketID.Text = jazyk.Windows_Ticket_IDTicketu;
                    lbl_newTicket_customer.Text = jazyk.Windows_Ticket_Zakaznik;
                    lbl_newTicket_description.Text = jazyk.Windows_Ticket_Popis;
                    lbl_newTicket_contact.Text = jazyk.Windows_Ticket_Kontakt;
                    lbl_newTicket_extendedInfo.Text = jazyk.Windows_Ticket_RozsireneInformace;
                    lbl_newTicket_stav.Text = jazyk.Windows_Ticket_StavTicketu;
                    lbl_newTicket_done.Text = jazyk.Windows_Ticket_Hotovo;
                    lbl_newTicket_waiting.Text = jazyk.Windows_Ticket_CekaSe;
                    lbl_newTicket_waitingForResponse.Text = jazyk.Windows_Ticket_CekaSeNaOdpoved;
                    lbl_newTicket_RDP.Text = jazyk.Windows_Ticket_RDP;
                    lbl_newTicket_inProgress.Text = jazyk.Windows_Ticket_Probiha;
                    lbl_newTicket_normal.Text = jazyk.Windows_Ticket_Normalni;
                    lbl_newTicket_overtime.Text = jazyk.Windows_Ticket_Prescas;
                    lbl_newTicket_holiday.Text = jazyk.Windows_Ticket_Volno;
                    lbl_newTicket_compensLeave.Text = jazyk.Windows_Ticket_NahradniVolno;
                    lbl_newTicket_mdm.Text = jazyk.Windows_Ticket_MDM;
                    lbl_newTicket_encryption.Text = jazyk.Windows_Ticket_Enkrypce;
                    lbl_newTicket_timeNetto.Text = jazyk.Windows_Ticket_CisteOdpracovano;
                    lbl_newTicket_time.Text = jazyk.Windows_Ticket_Odpracovano;
                    lbl_newTicket_pause.Text = jazyk.Windows_Ticket_DobaPauzy;
                    lbl_newTicket_customTerp.Text = jazyk.Windows_Ticket_VlastniTerp;
                    lbl_newTicket_terpCode.Text = jazyk.Windows_Ticket_TerpKod;
                    lbl_newTicket_notes.Text = jazyk.Windows_Ticket_Poznamky;
                    lbl_newTicket_ticketTime.Text = jazyk.Windows_Ticket_DobaPrace;
                    lbl_newTicket_minutes.Text = jazyk.Windows_Ticket_Minut;
                    lbl_newTicket_hours.Text = jazyk.Windows_Ticket_Hodiny;
                    lbl_newTicket_hours5.Text = jazyk.Windows_Ticket_Hodin;
                    lbl_newTicket_times.Text = jazyk.Windows_Ticket_Casy;
                    lbl_newTicket_start.Text = jazyk.Windows_Ticket_Zacatek;
                    lbl_newTicket_end.Text = jazyk.Windows_Ticket_Konec;
                }
                else if (tabControl2.SelectedTab.Name == "help")
                {

                }
                else if (tabControl2.SelectedTab.Name == "notification")
                {

                }
                else if (tabControl2.SelectedTab.Name == "languageManager")
                {

                }
            }
            else if (tabList.SelectedTab.Name == "errors")
            {

            }
            else if (tabList.SelectedTab.Name == "messages")
            {

            }
        }

        private void save_Click(object sender, EventArgs e)
        {

        }

        private void Preklad_Load(object sender, EventArgs e)
        {

        }
    }
}
