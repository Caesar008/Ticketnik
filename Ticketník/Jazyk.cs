using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Ticketník
{
    public class Jazyk
    {
        private static string jazyk = Properties.Settings.Default.Jazyk;
        private static string cesta = Properties.Settings.Default.JazykCesta;

        static XmlDocument preklad = new XmlDocument();

        public Jazyk(bool enOnly = false)
        {
            jazyk = Properties.Settings.Default.Jazyk;
            cesta = Properties.Settings.Default.JazykCesta;
            InterniPamet = new Dictionary<string, string>();

            if (!enOnly)
            {
                if (!File.Exists(Properties.Settings.Default.JazykCesta))
                {
                    cesta = Properties.Settings.Default.JazykCesta = "";
                    jazyk = Properties.Settings.Default.Jazyk = "";
                }
                if (cesta == String.Empty)
                {
                    preklad.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml");

                }
                else
                    preklad.Load(Properties.Settings.Default.JazykCesta);
            }
            else
            {
                jazyk = "EN";
                preklad.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml");
            }
            Verze = int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("version").InnerText);
            Jmeno = preklad.DocumentElement.Attributes.GetNamedItem("name").InnerText;
            Zkratka = preklad.DocumentElement.Attributes.GetNamedItem("shortcut").InnerText;
            Revize = int.Parse(preklad.DocumentElement.Attributes.GetNamedItem("revision").InnerText);

            InterniPamet.Add("Windows/Settings/InProgress", ReturnPreklad("Windows/Settings/InProgress"));
            InterniPamet.Add("Windows/Settings/Waiting", ReturnPreklad("Windows/Settings/Waiting"));
            InterniPamet.Add("Windows/Settings/RDP", ReturnPreklad("Windows/Settings/RDP"));
            InterniPamet.Add("Windows/Settings/WaitingForResponse", ReturnPreklad("Windows/Settings/WaitingForResponse"));
            InterniPamet.Add("Windows/Settings/Done", ReturnPreklad("Windows/Settings/Done"));
            InterniPamet.Add("Windows/NewTicket/BasicInfo", ReturnPreklad("Windows/NewTicket/BasicInfo"));
            InterniPamet.Add("Windows/NewTicket/TicketTime", ReturnPreklad("Windows/NewTicket/TicketTime"));
            InterniPamet.Add("Windows/NewTicket/Times", ReturnPreklad("Windows/NewTicket/Times"));
            InterniPamet.Add("Windows/NewTicket/Pauses", ReturnPreklad("Windows/NewTicket/Pauses"));
            InterniPamet.Add("Windows/NewTicket/ExtendedInfo", ReturnPreklad("Windows/NewTicket/ExtendedInfo"));
            InterniPamet.Add("Windows/NewTicket/Notes", ReturnPreklad("Windows/NewTicket/Notes"));
            InterniPamet.Add("Windows/NewTicket/TicketID", ReturnPreklad("Windows/NewTicket/TicketID"));
            InterniPamet.Add("Windows/NewTicket/Customer", ReturnPreklad("Windows/NewTicket/Customer"));
            InterniPamet.Add("Windows/NewTicket/PC", ReturnPreklad("Windows/NewTicket/PC"));
            InterniPamet.Add("Windows/NewTicket/Contact", ReturnPreklad("Windows/NewTicket/Contact"));
            InterniPamet.Add("Windows/NewTicket/Description", ReturnPreklad("Windows/NewTicket/Description"));
            InterniPamet.Add("Windows/NewTicket/Status", ReturnPreklad("Windows/NewTicket/Status"));
            InterniPamet.Add("Windows/NewTicket/TimeNetto", ReturnPreklad("Windows/NewTicket/TimeNetto"));
            InterniPamet.Add("Windows/NewTicket/Time", ReturnPreklad("Windows/NewTicket/Time"));
            InterniPamet.Add("Windows/NewTicket/PauseTime", ReturnPreklad("Windows/NewTicket/PauseTime"));
            InterniPamet.Add("Windows/NewTicket/TerpCode", ReturnPreklad("Windows/NewTicket/TerpCode"));
            InterniPamet.Add("Windows/NewTicket/CustomTerp", ReturnPreklad("Windows/NewTicket/CustomTerp"));
            InterniPamet.Add("Windows/NewTicket/Normal", ReturnPreklad("Windows/NewTicket/Normal"));
            InterniPamet.Add("Windows/NewTicket/Overtime", ReturnPreklad("Windows/NewTicket/Overtime"));
            InterniPamet.Add("Windows/NewTicket/Holiday", ReturnPreklad("Windows/NewTicket/Holiday"));
            InterniPamet.Add("Windows/NewTicket/CompensLeave", ReturnPreklad("Windows/NewTicket/CompensLeave"));
            InterniPamet.Add("Windows/NewTicket/MDM", ReturnPreklad("Windows/NewTicket/MDM"));
            InterniPamet.Add("Windows/NewTicket/Encryption", ReturnPreklad("Windows/NewTicket/Encryption"));
            InterniPamet.Add("Windows/NewTicket/Edit", ReturnPreklad("Windows/NewTicket/Edit"));
            InterniPamet.Add("Windows/NewTicket/Delete", ReturnPreklad("Windows/NewTicket/Delete"));
            InterniPamet.Add("Windows/NewTicket/Add", ReturnPreklad("Windows/NewTicket/Add"));
            InterniPamet.Add("Windows/NewTicket/To", ReturnPreklad("Windows/NewTicket/To"));
            InterniPamet.Add("Windows/NewTicket/From", ReturnPreklad("Windows/NewTicket/From"));
            InterniPamet.Add("Windows/NewTicket/End", ReturnPreklad("Windows/NewTicket/End"));
            InterniPamet.Add("Windows/NewTicket/Start", ReturnPreklad("Windows/NewTicket/Start"));
            InterniPamet.Add("Windows/NewTicket/InProgress", ReturnPreklad("Windows/NewTicket/InProgress"));
            InterniPamet.Add("Windows/NewTicket/Waiting", ReturnPreklad("Windows/NewTicket/Waiting"));
            InterniPamet.Add("Windows/NewTicket/WaitingForResponse", ReturnPreklad("Windows/NewTicket/WaitingForResponse"));
            InterniPamet.Add("Windows/NewTicket/RDP", ReturnPreklad("Windows/NewTicket/RDP"));
            InterniPamet.Add("Windows/NewTicket/Done", ReturnPreklad("Windows/NewTicket/Done"));
            InterniPamet.Add("Windows/NewTicket/NormalDescription", ReturnPreklad("Windows/NewTicket/NormalDescription"));
            InterniPamet.Add("Windows/NewTicket/CompensLeaveDesc", ReturnPreklad("Windows/NewTicket/CompensLeaveDesc"));
            InterniPamet.Add("Windows/NewTicket/TicketTerp", ReturnPreklad("Windows/NewTicket/TicketTerp"));
        }
        //překlady
        #region Main Menu
        internal string Menu_Soubor
        {
            get
            {
                return ReturnPreklad("MainMenuItems/File");
            }
        }
        internal string Menu_Moznosti
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Options");
            }
        }
        internal string Menu_Source
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Source");
            }
        }
        internal string Menu_About
        {
            get
            {
                return ReturnPreklad("MainMenuItems/About");
            }
        }
        internal string Menu_Help
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Help");
            }
        }
        internal string Menu_Novy
        {
            get
            {
                return ReturnPreklad("MainMenuItems/New");
            }
        }
        internal string Menu_Otevrit
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Open");
            }
        }
        internal string Menu_Ulozit
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Safe");
            }
        }
        internal string Menu_Exportovat
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Export");
            }
        }
        internal string Menu_PrevestNaMillenium
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Convert");
            }
        }
        internal string Menu_Zavrit
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Close");
            }
        }
        internal string Menu_Nastaveni
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Settings");
            }
        }
        internal string Menu_PridatZakaznika
        {
            get
            {
                return ReturnPreklad("MainMenuItems/AddCustomer");
            }
        }
        internal string Menu_UpravitZakaznika
        {
            get
            {
                return ReturnPreklad("MainMenuItems/EditCustomer");
            }
        }
        internal string Menu_SmazatZakaznika
        {
            get
            {
                return ReturnPreklad("MainMenuItems/DeleteCustomer");
            }
        }
        internal string Menu_PridatTerp
        {
            get
            {
                return ReturnPreklad("MainMenuItems/AddTerp");
            }
        }
        internal string Menu_UpravitTerp
        {
            get
            {
                return ReturnPreklad("MainMenuItems/EditTerp");
            }
        }
        internal string Menu_SmazatTerp
        {
            get
            {
                return ReturnPreklad("MainMenuItems/DeleteTerp");
            }
        }
        internal string Menu_Hledat
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Search");
            }
        }
        internal string Menu_Report
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Report");
            }
        }
        internal string Menu_ZnameProblemy
        {
            get
            {
                return ReturnPreklad("MainMenuItems/KnownIssues");
            }
        }
        internal string Menu_Changelog
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Changelog");
            }
        }
        internal string Menu_Plany
        {
            get
            {
                return ReturnPreklad("MainMenuItems/FuturePlans");
            }
        }
        internal string Menu_Dokumentace
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Documentation");
            }
        }
        internal string Menu_Upozorneni
        {
            get
            {
                return ReturnPreklad("MainMenuItems/Notification");
            }
        }
        internal string Menu_DostupneJazyky
        {
            get
            {
                return ReturnPreklad("MainMenuItems/AvailableLang");
            }
        }
        internal string Menu_HledejAktualizace
        {
            get
            {
                return ReturnPreklad("MainMenuItems/SearchForUpdates");
            }
        }
        #endregion

        #region Side Menu
        internal string SideMenu_PridatZaznam
        {
            get
            {
                return ReturnPreklad("SideMenuItems/AddRecord");
            }
        }
        internal string SideMenu_UpravitZaznam
        {
            get
            {
                return ReturnPreklad("SideMenuItems/EditRecord");
            }
        }
        internal string SideMenu_SmazatZaznam
        {
            get
            {
                return ReturnPreklad("SideMenuItems/DeleteRecord");
            }
        }
        internal string SideMenu_PridatZakaznika
        {
            get
            {
                return ReturnPreklad("SideMenuItems/AddCustomer");
            }
        }
        internal string SideMenu_UpravitZakaznika
        {
            get
            {
                return ReturnPreklad("SideMenuItems/EditCustomer");
            }
        }
        internal string SideMenu_SmazatZakaznika
        {
            get
            {
                return ReturnPreklad("SideMenuItems/DeleteCustomer");
            }
        }
        internal string SideMenu_Hledat
        {
            get
            {
                return ReturnPreklad("SideMenuItems/Search");
            }
        }
        internal string SideMenu_Napoveda
        {
            get
            {
                return ReturnPreklad("SideMenuItems/Help");
            }
        }
        #endregion

        #region Months
        internal string Month_Leden
        {
            get
            {
                return ReturnPreklad("Months/January");
            }
        }
        internal string Month_Unor
        {
            get
            {
                return ReturnPreklad("Months/February");
            }
        }
        internal string Month_Brezen
        {
            get
            {
                return ReturnPreklad("Months/March");
            }
        }
        internal string Month_Duben
        {
            get
            {
                return ReturnPreklad("Months/April");
            }
        }
        internal string Month_Kveten
        {
            get
            {
                return ReturnPreklad("Months/May");
            }
        }
        internal string Month_Cerven
        {
            get
            {
                return ReturnPreklad("Months/June");
            }
        }
        internal string Month_Cervenec
        {
            get
            {
                return ReturnPreklad("Months/July");
            }
        }
        internal string Month_Srpen
        {
            get
            {
                return ReturnPreklad("Months/August");
            }
        }
        internal string Month_Zari
        {
            get
            {
                return ReturnPreklad("Months/September");
            }
        }
        internal string Month_Rijen
        {
            get
            {
                return ReturnPreklad("Months/October");
            }
        }
        internal string Month_Listopad
        {
            get
            {
                return ReturnPreklad("Months/November");
            }
        }
        internal string Month_Prosinec
        {
            get
            {
                return ReturnPreklad("Months/December");
            }
        }
        #endregion

        #region Headers
        internal string Header_PC
        {
            get
            {
                return ReturnPreklad("Headers/PC");
            }
        }
        internal string Header_TicketID
        {
            get
            {
                return ReturnPreklad("Headers/TicketID");
            }
        }
        internal string Header_Zakaznik
        {
            get
            {
                return ReturnPreklad("Headers/Customer");
            }
        }
        internal string Header_Popis
        {
            get
            {
                return ReturnPreklad("Headers/Description");
            }
        }
        internal string Header_Kontakt
        {
            get
            {
                return ReturnPreklad("Headers/Contact");
            }
        }
        internal string Header_Terp
        {
            get
            {
                return ReturnPreklad("Headers/Terp");
            }
        }
        internal string Header_Task
        {
            get
            {
                return ReturnPreklad("Headers/Task");
            }
        }
        internal string Header_Cas
        {
            get
            {
                return ReturnPreklad("Headers/Time");
            }
        }
        internal string Header_Stav
        {
            get
            {
                return ReturnPreklad("Headers/Status");
            }
        }
        internal string Header_Poznamka
        {
            get
            {
                return ReturnPreklad("Headers/Note");
            }
        }
        internal string Header_Tickety
        {
            get
            {
                return ReturnPreklad("Headers/Tickets");
            }
        }
        internal string Header_Neulozeno
        {
            get
            {
                return ReturnPreklad("Headers/NotSaved");
            }
        }
        #endregion

        #region Messages
        
        internal string Message_TaskCannotBeEmpty
        {
            get
            {
                return ReturnPreklad("Messages/TaskCannotBeEmpty");
            }
        }
        internal string Message_WindowMoved
        {
            get
            {
                return ReturnPreklad("Messages/WindowMoved");
            }
        }
        internal string Message_ZakazniciUpd
        {
            get
            {
                return ReturnPreklad("Messages/FileZakUpd");
            }
        }
        internal string Message_TerpUpdate
        {
            get
            {
                return ReturnPreklad("Messages/TerpUpdate");
            }
        }
        internal string Message_Aktualizace
        {
            get
            {
                return ReturnPreklad("Messages/Update");
            }
        }
        internal string Message_NovaVerze
        {
            get
            {
                return ReturnPreklad("Messages/NewVersionFound").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Message_Ulozit
        {
            get
            {
                return ReturnPreklad("Messages/WantSafe");
            }
        }
        internal string Message_Neulozeno
        {
            get
            {
                return ReturnPreklad("Messages/NotSaved");
            }
        }
        internal string Message_SmazatZakaznika
        {
            get
            {
                return ReturnPreklad("Messages/DelCustomer");
            }
        }
        internal string Message_OpravduSmazat
        {
            get
            {
                return ReturnPreklad("Messages/RealyDelete");
            }
        }
        internal string Message_Preskoceno
        {
            get
            {
                return ReturnPreklad("Messages/Skipped");
            }
        }
        internal string Message_Zkopirovan
        {
            get
            {
                return ReturnPreklad("Messages/TicketCopy");
            }
        }
        internal string Message_ZakaznikNotImplemented
        {
            get
            {
                return ReturnPreklad("Messages/CustomerNotImplemented");
            }
        }
        internal string Message_FormatNovy
        {
            get
            {
                return ReturnPreklad("Messages/FormatUpgrade");
            }
        }
        internal string Message_VytvoritUpozorneni
        {
            get
            {
                return ReturnPreklad("Messages/CreateNotification");
            }
        }
        internal string Message_VyhledavamAktualizace
        {
            get
            {
                return ReturnPreklad("Messages/LookingForUpdates");
            }
        }
        internal string Message_AktualizaceSeNezdarila
        {
            get
            {
                return ReturnPreklad("Messages/UpdateFailed");
            }
        }
        internal string Message_AktualizaceHotova
        {
            get
            {
                return ReturnPreklad("Messages/UpdateComplete");
            }
        }
        internal string Message_Exportuji
        {
            get
            {
                return ReturnPreklad("Messages/ExportingTickets");
            }
        }
        internal string Message_hledamAktualizaciNapovedy
        {
            get
            {
                return ReturnPreklad("Messages/LookingHelpUpdate");
            }
        }

        #endregion

        #region Statusses

        internal string Status_Probiha
        {
            get
            {
                //return ReturnPreklad("Windows/Settings/InProgress");
                return InterniPamet["Windows/Settings/InProgress"];
            }
        }
        internal string Status_CekaSe
        {
            get
            {
                //return ReturnPreklad("Windows/Settings/Waiting");
                return InterniPamet["Windows/Settings/Waiting"];
            }
        }
        internal string Status_RDP
        {
            get
            {
                //return ReturnPreklad("Windows/Settings/RDP");
                return InterniPamet["Windows/Settings/RDP"];
            }
        }
        internal string Status_CekaSeNO
        {
            get
            {
                //return ReturnPreklad("Windows/Settings/WaitingForResponse");
                return InterniPamet["Windows/Settings/WaitingForResponse"];
            }
        }
        internal string Status_Vyreseno
        {
            get
            {
                //return ReturnPreklad("Windows/Settings/Done");
                return InterniPamet["Windows/Settings/Done"];
            }
        }

        #endregion

        #region Errors

        internal string Error_NovějsiVerze
        {
            get
            {
                return ReturnPreklad("Errors/TicketsVersionNotMatch");
            }
        }
        internal string Error_Verze
        {
            get
            {
                return ReturnPreklad("Errors/VersionsDoNotMatch");
            }
        }
        internal string Error_NejdeOtevritSoubor
        {
            get
            {
                return ReturnPreklad("Errors/CantOpenFile").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Error_NejdeOtevrit
        {
            get
            {
                return ReturnPreklad("Errors/CantOpen");
            }
        }
        internal string Error_NejdeOtevritNapoveda
        {
            get
            {
                return ReturnPreklad("Errors/HelpError");
            }
        }
        internal string Error_Error
        {
            get
            {
                return ReturnPreklad("Errors/Error");
            }
        }
        internal string Error_NejdeZavritAAktualizovat
        {
            get
            {
                return ReturnPreklad("Errors/CannotCloseAndUpdate").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Error_UzBezi
        {
            get
            {
                return ReturnPreklad("Errors/AlreadyRunning");
            }
        }
        internal string Error_PoskozeneNastaveni
        {
            get
            {
                return ReturnPreklad("Errors/DamagedFile").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Error_PoskozenySouborNastaveni
        {
            get
            {
                return ReturnPreklad("Errors/DamagedSettingsFile");
            }
        }
        internal string Error_DosloKChybe
        {
            get
            {
                return ReturnPreklad("Errors/ErrorOccured");
            }
        }
        internal string Error_KritickaChyba
        {
            get
            {
                return ReturnPreklad("Errors/CriticalError");
            }
        }
        internal string Error_RegexError
        {
            get
            {
                return ReturnPreklad("Errors/SearchRegexError");
            }
        }
        internal string Error_DamagedTicFile
        {
            get
            {
                return ReturnPreklad("Errors/DamagedTicFile");
            }
        }

        #endregion

        #region Windows

        internal string Windows_SouborTicketu
        {
            get
            {
                return ReturnPreklad("Windows/Open/TicketsFile");
            }
        }
        internal string Windows_Otevrit
        {
            get
            {
                return ReturnPreklad("Windows/Open/Open");
            }
        }
        internal string Windows_NovyTicket
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/NewTicket");
            }
        }
        internal string Windows_UpravitTicket
        {
            get
            {
                return ReturnPreklad("Windows/EditTicket/EditTicket");
            }
        }
        internal string Windows_Mala
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Small");
            }
        }
        internal string Windows_Stredni
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Medium");
            }
        }
        internal string Windows_Velka
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Large");
            }
        }
        internal string Windows_NovyTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/NewTerp");
            }
        }
        internal string Windows_UpravitTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/EditTerp");
            }
        }
        internal string Windows_SmazatTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/DeleteTerp");
            }
        }
        internal string Windows_AboutBox_Popis
        {
            get
            {
                return ReturnPreklad("Windows/About/License").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Windows_AboutBox_Verze
        {
            get
            {
                return ReturnPreklad("Windows/About/Version");
            }
        }
        internal string Windows_AboutBox_Aktualizace
        {
            get
            {
                return ReturnPreklad("Windows/About/Searching");
            }
        }
        internal string Windows_Export_Ticket
        {
            get
            {
                return ReturnPreklad("Windows/Export/Ticket");
            }
        }
        internal string Windows_Export_NaKteremJsiPracoval
        {
            get
            {
                return ReturnPreklad("Windows/Export/YouWorkedOn");
            }
        }
        internal string Windows_Export_Neukoncen
        {
            get
            {
                return ReturnPreklad("Windows/Export/NotEnded");
            }
        }
        internal string Windows_Export_Nazev
        {
            get
            {
                return ReturnPreklad("Windows/Export/Header");
            }
        }
        internal string Windows_Export_Exportovat
        {
            get
            {
                return ReturnPreklad("Windows/Export/Export");
            }
        }
        internal string Windows_Export_TentoTyden
        {
            get
            {
                return ReturnPreklad("Windows/Export/ThisWeek");
            }
        }
        internal string Windows_Export_MinulyTyden
        {
            get
            {
                return ReturnPreklad("Windows/Export/LastWeek");
            }
        }
        internal string Windows_Export_VybraneObdobi
        {
            get
            {
                return ReturnPreklad("Windows/Export/Custom");
            }
        }
        internal string Windows_JmenoSouboru
        {
            get
            {
                return ReturnPreklad("Windows/NewFile/FileName");
            }
        }
        internal string Windows_NovySoubor
        {
            get
            {
                return ReturnPreklad("Windows/NewFile/NewFile");
            }
        }
        internal string Windows_Help_NejdeStahnout
        {
            get
            {
                return ReturnPreklad("Windows/Help/CannotDownload");
            }
        }
        internal string Windows_Help_Nenalezeno
        {
            get
            {
                return ReturnPreklad("Windows/Help/NotFound");
            }
        }
        internal string Windows_Help_Updating
        {
            get
            {
                return ReturnPreklad("Windows/Help/UpdatingHelp");
            }
        }
        internal string Windows_Nastaveni_PoStratu
        {
            get
            {
                return ReturnPreklad("Windows/Settings/RunOnStartup");
            }
        }
        internal string Windows_Nastaveni_Autosave
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Autosave");
            }
        }
        internal string Windows_Nastaveni_UkladatKazdych
        {
            get
            {
                return ReturnPreklad("Windows/Settings/SaveEvery");
            }
        }
        internal string Windows_Nastaveni_Minut
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Minutes");
            }
        }
        internal string Windows_Nastaveni_ZjednodusenyCas
        {
            get
            {
                return ReturnPreklad("Windows/Settings/SimpleTimeSettings");
            }
        }
        internal string Windows_Nastaveni_BarvyTicketu
        {
            get
            {
                return ReturnPreklad("Windows/Settings/TicketColor");
            }
        }
        internal string Windows_Nastaveni_Nastaveni
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Settings");
            }
        }
        internal string Windows_Nastaveni_Zmen
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Change");
            }
        }
        internal string Windows_Nastaveni_Vyreseno
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Done");
            }
        }
        internal string Windows_Nastaveni_CekaSe
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Waiting");
            }
        }
        internal string Windows_Nastaveni_CekaSeNaOdpoved
        {
            get
            {
                return ReturnPreklad("Windows/Settings/WaitingForResponse");
            }
        }
        internal string Windows_Nastaveni_RDP
        {
            get
            {
                return ReturnPreklad("Windows/Settings/RDP");
            }
        }
        internal string Windows_Nastaveni_Probiha
        {
            get
            {
                return ReturnPreklad("Windows/Settings/InProgress");
            }
        }
        internal string Windows_Nastaveni_Default
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Default");
            }
        }
        internal string Windows_Nastaveni_CasZaDen
        {
            get
            {
                return ReturnPreklad("Windows/Settings/ShowTimePerDay");
            }
        }
        internal string Windows_Nastaveni_CelkovyCas
        {
            get
            {
                return ReturnPreklad("Windows/Settings/ShowTotalTime");
            }
        }
        internal string Windows_Nastaveni_Hodin
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Hodin");
            }
        }
        internal string Windows_Nastaveni_Hodiny
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Hodiny");
            }
        }
        internal string Windows_Nastaveni_Nad
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Over");
            }
        }
        internal string Windows_Nastaveni_Jazyk
        {
            get
            {
                return ReturnPreklad("Windows/Settings/Language");
            }
        }
        internal string Windows_Nastaveni_ZmenJazyk
        {
            get
            {
                return ReturnPreklad("Windows/Settings/ChangeLanguage");
            }
        }
        internal string Windows_Nastaveni_OnlineTerp
        {
            get
            {
                return ReturnPreklad("Windows/Settings/OnlineTerp");
            }
        }
        internal string Windows_Search_KriteriaHledani
        {
            get
            {
                return ReturnPreklad("Windows/Search/SearchCriteria");
            }
        }
        internal string Windows_Search_PC
        {
            get
            {
                return ReturnPreklad("Windows/Search/PC");
            }
        }
        internal string Windows_Search_IDTicketu
        {
            get
            {
                return ReturnPreklad("Windows/Search/TicketID");
            }
        }
        internal string Windows_Search_Zakaznik
        {
            get
            {
                return ReturnPreklad("Windows/Search/Customer");
            }
        }
        internal string Windows_Search_Popis
        {
            get
            {
                return ReturnPreklad("Windows/Search/Description");
            }
        }
        internal string Windows_Search_Kontakt
        {
            get
            {
                return ReturnPreklad("Windows/Search/Contact");
            }
        }
        internal string Windows_Search_Poznamka
        {
            get
            {
                return ReturnPreklad("Windows/Search/Note");
            }
        }
        internal string Windows_Search_Regex
        {
            get
            {
                return ReturnPreklad("Windows/Search/Regex");
            }
        }
        internal string Windows_Search_Hledej
        {
            get
            {
                return ReturnPreklad("Windows/Search/Search");
            }
        }
        internal string Windows_Search_HledaniNapoveda
        {
            get
            {
                return ReturnPreklad("Windows/Search/SearchCriteriaHelp").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Windows_Search_Datum
        {
            get
            {
                return ReturnPreklad("Windows/Search/Date");
            }
        }
        internal string Windows_Search_Status
        {
            get
            {
                return ReturnPreklad("Windows/Search/Status");
            }
        }
        internal string Windows_Search_Pauzy
        {
            get
            {
                return ReturnPreklad("Windows/Search/Pause");
            }
        }
        internal string Windows_Search_Hotovo
        {
            get
            {
                return ReturnPreklad("Windows/Search/Done");
            }
        }
        internal string Windows_Search_CekaSe
        {
            get
            {
                return ReturnPreklad("Windows/Search/Waiting");
            }
        }
        internal string Windows_Search_CekaSeNa
        {
            get
            {
                return ReturnPreklad("Windows/Search/WaitingForResponse");
            }
        }
        internal string Windows_Search_RDP
        {
            get
            {
                return ReturnPreklad("Windows/Search/RDP");
            }
        }
        internal string Windows_Search_Probiha
        {
            get
            {
                return ReturnPreklad("Windows/Search/InProgress");
            }
        }
        internal string Windows_Search_JenProCteni
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/SearchTicket");
            }
        }
        internal string Windows_Terp_TerpKod
        {
            get
            {
                return ReturnPreklad("Windows/Terp/TerpCode");
            }
        }
        internal string Windows_Terp_UzExistuje
        {
            get
            {
                return ReturnPreklad("Windows/Terp/AlreadyExists");
            }
        }
        internal string Windows_Terp_BylPridan
        {
            get
            {
                return ReturnPreklad("Windows/Terp/WasAdded");
            }
        }

        internal string Windows_Terp_Task
        {
            get
            {
                return ReturnPreklad("Windows/Terp/Task");
            }
        }

        internal string Windows_Terp_Terp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/Terp");
            }
        }
        internal string Windows_Terp_Nastav
        {
            get
            {
                return ReturnPreklad("Windows/Terp/Set");
            }
        }
        internal string Windows_Terp_PridatTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/AddTerp");
            }
        }
        internal string Windows_Terp_PridatTask
        {
            get
            {
                return ReturnPreklad("Windows/Terp/AddTask");
            }
        }
        internal string Windows_Terp_UlozTask
        {
            get
            {
                return ReturnPreklad("Windows/Terp/SafeTask");
            }
        }
        internal string Windows_Terp_Novy
        {
            get
            {
                return ReturnPreklad("Windows/Terp/New");
            }
        }
        internal string Windows_Terp_UlozTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/SafeTerp");
            }
        }
        internal string Windows_Terp_SmazatTerp
        {
            get
            {
                return ReturnPreklad("Windows/Terp/DeleteTerpBtn");
            }
        }
        internal string Windows_Terp_SmazatTask
        {
            get
            {
                return ReturnPreklad("Windows/Terp/DeleteTask");
            }
        }
        internal string Windows_Ticket_ZakladniInfo
        {
            get
            {
                return InterniPamet["Windows/NewTicket/BasicInfo"];
            }
        }
        internal string Windows_Ticket_DobaPrace
        {
            get
            {
                return InterniPamet["Windows/NewTicket/TicketTime"];
            }
        }
        internal string Windows_Ticket_Casy
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Times"];
            }
        }
        internal string Windows_Ticket_Pauzy
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Pauses"];
            }
        }
        internal string Windows_Ticket_RozsireneInformace
        {
            get
            {
                return InterniPamet["Windows/NewTicket/ExtendedInfo"];
            }
        }
        internal string Windows_Ticket_Poznamky
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Notes"];
            }
        }
        internal string Windows_Ticket_IDTicketu
        {
            get
            {
                return InterniPamet["Windows/NewTicket/TicketID"];
            }
        }
        internal string Windows_Ticket_Zakaznik
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Customer"];
            }
        }
        internal string Windows_Ticket_Pocitac
        {
            get
            {
                return InterniPamet["Windows/NewTicket/PC"];
            }
        }
        internal string Windows_Ticket_Kontakt
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Contact"];
            }
        }
        internal string Windows_Ticket_Popis
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Description"];
            }
        }
        internal string Windows_Ticket_StavTicketu
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Status"];
            }
        }
        internal string Windows_Ticket_CisteOdpracovano
        {
            get
            {
                return InterniPamet["Windows/NewTicket/TimeNetto"];
            }
        }
        internal string Windows_Ticket_Odpracovano
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Time"];
            }
        }
        internal string Windows_Ticket_DobaPauzy
        {
            get
            {
                return InterniPamet["Windows/NewTicket/PauseTime"];
            }
        }
        internal string Windows_Ticket_TerpKod
        {
            get
            {
                return InterniPamet["Windows/NewTicket/TerpCode"];
            }
        }
        internal string Windows_Ticket_VlastniTerp
        {
            get
            {
                return InterniPamet["Windows/NewTicket/CustomTerp"];
            }
        }
        internal string Windows_Ticket_Normalni
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Normal"];
            }
        }
        internal string Windows_Ticket_Prescas
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Overtime"];
            }
        }
        internal string Windows_Ticket_Volno
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Holiday"];
            }
        }
        internal string Windows_Ticket_NahradniVolno
        {
            get
            {
                return InterniPamet["Windows/NewTicket/CompensLeave"];
            }
        }
        internal string Windows_Ticket_MDM
        {
            get
            {
                return InterniPamet["Windows/NewTicket/MDM"];
            }
        }
        internal string Windows_Ticket_Enkrypce
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Encryption"];
            }
        }
        internal string Windows_Ticket_Upravit
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Edit"];
            }
        }
        internal string Windows_Ticket_Smazat
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Delete"];
            }
        }
        internal string Windows_Ticket_Pridat
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Add"];
            }
        }
        internal string Windows_Ticket_Do
        {
            get
            {
                return InterniPamet["Windows/NewTicket/To"];
            }
        }
        internal string Windows_Ticket_Od
        {
            get
            {
                return InterniPamet["Windows/NewTicket/From"];
            }
        }
        internal string Windows_Ticket_Konec
        {
            get
            {
                return InterniPamet["Windows/NewTicket/End"];
            }
        }
        internal string Windows_Ticket_Zacatek
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Start"];
            }
        }
        internal string Windows_Ticket_Probiha
        {
            get
            {
                return InterniPamet["Windows/NewTicket/InProgress"];
            }
        }
        internal string Windows_Ticket_CekaSe
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Waiting"];
            }
        }
        internal string Windows_Ticket_CekaSeNaOdpoved
        {
            get
            {
                return InterniPamet["Windows/NewTicket/WaitingForResponse"];
            }
        }
        internal string Windows_Ticket_RDP
        {
            get
            {
                return InterniPamet["Windows/NewTicket/RDP"];
            }
        }
        internal string Windows_Ticket_Hotovo
        {
            get
            {
                return InterniPamet["Windows/NewTicket/Done"];
            }
        }
        internal string Windows_Ticket_NormalniPopis
        {
            get
            {
                return InterniPamet["Windows/NewTicket/NormalDescription"];
            }
        }
        internal string Windows_Ticket_NahradniVolnoPopis
        {
            get
            {
                return InterniPamet["Windows/NewTicket/CompensLeaveDesc"];
            }
        }
        internal string Windows_Ticket_TicketTerp
        {
            get
            {
                return InterniPamet["Windows/NewTicket/TicketTerp"];
            }
        }
        internal string Windows_Ticket_ZmenaData
        {
            get
            {
                return ReturnPreklad("Messages/DateChanged").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Windows_Ticket_ZmenaDataOkno
        {
            get
            {
                return ReturnPreklad("Messages/DateChangedWindow");
            }
        }
        internal string Windows_Ticket_Minut
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/Minutes");
            }
        }
        internal string Windows_Ticket_Hodin
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/Hodin");
            }
        }
        internal string Windows_Ticket_Hodiny
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/Hodiny");
            }
        }
        internal string Windows_Ticket_Hodina
        {
            get
            {
                return ReturnPreklad("Windows/NewTicket/Hour");
            }
        }
        internal string Windows_Zakaznik
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Customer");
            }
        }
        internal string Windows_Zakaznik_Velikost
        {
            get
            {
                return ReturnPreklad("Windows/Customer/CustomerSize");
            }
        }
        internal string Windows_Zakaznik_Maly
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Small");
            }
        }
        internal string Windows_Zakaznik_Stredni
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Medium");
            }
        }
        internal string Windows_Zakaznik_Velky
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Large");
            }
        }
        internal string Windows_Zakaznik_Nebo
        {
            get
            {
                return ReturnPreklad("Windows/Customer/Or").Replace("\\r", "\r").Replace("\\n", "\n");
            }
        }
        internal string Windows_Zakaznik_TerpKod
        {
            get
            {
                return ReturnPreklad("Windows/Customer/TerpCode");
            }
        }

        internal string Windows_Upozorneni_Datum
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Date");
            }
        }
        internal string Windows_Upozorneni_Cas
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Time");
            }
        }
        internal string Windows_Upozorneni_Typ
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Type");
            }
        }
        internal string Windows_Upozorneni_Popis
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Description");
            }
        }
        internal string Windows_Upozorneni_NoveUpozorneni
        {
            get
            {
                return ReturnPreklad("Windows/Notification/NewNotification");
            }
        }
        internal string Windows_Upozorneni_UpravitUpozorneni
        {
            get
            {
                return ReturnPreklad("Windows/Notification/EditNotification");
            }
        }
        internal string Windows_Upozorneni_SmazatUpozorneni
        {
            get
            {
                return ReturnPreklad("Windows/Notification/DeleteNotification");
            }
        }
        internal string Windows_Upozorneni_Upozorneni
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Notifications");
            }
        }
        internal string Windows_Upozorneni_DatumACas
        {
            get
            {
                return ReturnPreklad("Windows/Notification/DateAndTime");
            }
        }
        internal string Windows_Upozorneni_Upo
        {
            get
            {
                return ReturnPreklad("Windows/Notification/Notification");
            }
        }
        internal string Windows_Upozorneni_RDP
        {
            get
            {
                return ReturnPreklad("Windows/Notification/RDP");
            }
        }
        internal string Windows_Spravce_Spravce
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/LanguageManager");
            }
        }
        internal string Windows_Spravce_Jazyk
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/Language");
            }
        }
        internal string Windows_Spravce_Verze
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/Version");
            }
        }
        internal string Windows_Spravce_Zkratka
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/Shortcut");
            }
        }
        internal string Windows_Spravce_Stahnout
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/Download");
            }
        }
        internal string Windows_Spravce_Odebrat
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/Remove");
            }
        }
        internal string Windows_Spravce_NovyPreklad
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/NewTranslations");
            }
        }
        internal string Windows_Spravce_UpravitPreklad
        {
            get
            {
                return ReturnPreklad("Windows/LanguageManager/EditTranslation");
            }
        }

        #endregion

        string ReturnPreklad(string text)
        {
            
            if (jazyk == string.Empty || jazyk == "EN")
                return preklad.DocumentElement.SelectSingleNode(text).Attributes.GetNamedItem("en").InnerText;
            else
            {
                try
                {
                    if (preklad.DocumentElement.SelectSingleNode(text).InnerText != "")
                        return preklad.DocumentElement.SelectSingleNode(text).InnerText;
                    else
                        return preklad.DocumentElement.SelectSingleNode(text).Attributes.GetNamedItem("en").InnerText;
                }
                catch
                {
                    XmlDocument tmpPreklad = new XmlDocument();
                    tmpPreklad.Load(System.Reflection.Assembly.GetEntryAssembly().Location.Replace("Ticketnik.exe", "") + "lang\\CZ.xml");
                    return tmpPreklad.DocumentElement.SelectSingleNode(text).Attributes.GetNamedItem("en").InnerText;
                }
            }
             

            //Tohle jen pro dočasně na bugfix
                //return preklad.DocumentElement.SelectSingleNode(text).InnerText;
        }

        internal void Reload(Form1 form)
        {
            //přenačtení všech textů Form1 vytvářených při startu
            form.souborToolStripMenuItem.Text = form.jazyk.Menu_Soubor;
            form.novýToolStripMenuItem.Text = form.jazyk.Menu_Novy;
            form.načístToolStripMenuItem.Text = form.jazyk.Menu_Otevrit;
            form.uložitToolStripMenuItem.Text = form.jazyk.Menu_Ulozit;
            form.exportovatToolStripMenuItem.Text = form.jazyk.Menu_Exportovat;
            form.ukončitToolStripMenuItem.Text = form.jazyk.Menu_Zavrit;
            form.možnostiToolStripMenuItem.Text = form.jazyk.Menu_Moznosti;
            form.nastaveníToolStripMenuItem.Text = form.jazyk.Menu_Nastaveni;
            form.přidatZákazníkaToolStripMenuItem.Text = form.jazyk.Menu_PridatZakaznika;
            form.upravitZákazníkaToolStripMenuItem.Text = form.jazyk.Menu_UpravitZakaznika;
            form.smazatZákazníkaToolStripMenuItem.Text = form.jazyk.Menu_SmazatZakaznika;
            form.přidatTERPKódToolStripMenuItem.Text = form.jazyk.Menu_PridatTerp;
            form.upravitTERPKódToolStripMenuItem.Text = form.jazyk.Menu_UpravitTerp; 
            form.smazatTERPKódToolStripMenuItem.Text = form.jazyk.Menu_SmazatTerp;
            form.hledatToolStripMenuItem.Text = form.jazyk.Menu_Hledat;
            form.reportToolStripMenuItem.Text = form.jazyk.Menu_Report;
            form.sourceToolStripMenuItem.Text = form.jazyk.Menu_Source;
            form.knownIssuesToolStripMenuItem.Text = form.jazyk.Menu_ZnameProblemy;
            form.changelogToolStripMenuItem.Text = form.jazyk.Menu_Changelog;
            form.plányDoBudoucnaToolStripMenuItem.Text = form.jazyk.Menu_Plany;
            form.dokumentaceToolStripMenuItem.Text = form.jazyk.Menu_Dokumentace;
            form.oProgramuToolStripMenuItem.Text = form.jazyk.Menu_About;
            form.toolStripMenu_Napoveda.Text = form.jazyk.Menu_Help;
            form.toolStripMenu_Napoveda.ToolTipText = form.jazyk.Menu_Help;
            form.toolStripButton1.Text = form.jazyk.SideMenu_PridatZaznam;
            form.toolStripButton2.Text = form.jazyk.SideMenu_UpravitZaznam;
            form.toolStripButton3.Text = form.jazyk.SideMenu_SmazatZaznam;
            form.toolStripButton4.Text = form.jazyk.SideMenu_PridatZakaznika;
            form.zmenZakaznika.Text = form.jazyk.SideMenu_UpravitZakaznika;
            form.toolStripButton6.Text = form.jazyk.SideMenu_SmazatZakaznika;
            form.hledat.Text = form.jazyk.SideMenu_Hledat;
            form.toolStripButton_Napoveda.Text = form.jazyk.SideMenu_Napoveda;
            form.ledenT.Text = form.jazyk.Month_Leden;
            form.unorT.Text = form.jazyk.Month_Unor;
            form.brezenT.Text = form.jazyk.Month_Brezen;
            form.dubenT.Text = form.jazyk.Month_Duben;
            form.kvetenT.Text = form.jazyk.Month_Kveten;
            form.cervenT.Text = form.jazyk.Month_Cerven;
            form.cervenecT.Text = form.jazyk.Month_Cervenec;
            form.srpenT.Text = form.jazyk.Month_Srpen;
            form.zariT.Text = form.jazyk.Month_Zari;
            form.rijenT.Text = form.jazyk.Month_Rijen;
            form.listopadT.Text = form.jazyk.Month_Listopad;
            form.prosinecT.Text = form.jazyk.Month_Prosinec;
            form.upozorněníToolStripMenuItem.Text = form.jazyk.Menu_Upozorneni;
            form.vyhledatAktualizaceToolStripMenuItem.Text = form.jazyk.Menu_HledejAktualizace;
            /*form.columnHeader1.Text = form.jazyk.Header_PC;
            form.columnHeader2.Text = form.jazyk.Header_TicketID;
            form.columnHeader3.Text = form.jazyk.Header_Zakaznik;
            form.columnHeader4.Text = form.jazyk.Header_Popis;
            form.columnHeader5.Text = form.jazyk.Header_Kontakt;
            form.columnHeader6.Text = form.jazyk.Header_Terp;
            form.columnHeader7.Text = form.jazyk.Header_Task;
            form.columnHeader8.Text = form.jazyk.Header_Cas;
            form.columnHeader9.Text = form.jazyk.Header_Stav;
            form.columnHeader10.Text = form.jazyk.Header_Poznamka;*/
            foreach (TabPage tp in form.tabControl1.Controls)
            {
                if (tp.Controls.ContainsKey("leden"))
                {
                    ((ListView)tp.Controls["leden"]).BeginUpdate();
                    ((ListView)tp.Controls["leden"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["leden"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["leden"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["leden"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["leden"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["leden"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["leden"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["leden"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["leden"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["leden"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["leden"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("unor"))
                {
                    ((ListView)tp.Controls["unor"]).BeginUpdate();
                    ((ListView)tp.Controls["unor"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["unor"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["unor"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["unor"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["unor"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["unor"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["unor"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["unor"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["unor"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["unor"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["unor"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("brezen"))
                {
                    ((ListView)tp.Controls["brezen"]).BeginUpdate();
                    ((ListView)tp.Controls["brezen"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["brezen"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["brezen"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["brezen"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["brezen"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["brezen"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["brezen"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["brezen"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["brezen"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["brezen"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["brezen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("duben"))
                {
                    ((ListView)tp.Controls["duben"]).BeginUpdate();
                    ((ListView)tp.Controls["duben"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["duben"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["duben"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["duben"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["duben"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["duben"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["duben"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["duben"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["duben"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["duben"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["duben"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("kveten"))
                {
                    ((ListView)tp.Controls["kveten"]).BeginUpdate();
                    ((ListView)tp.Controls["kveten"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["kveten"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["kveten"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["kveten"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["kveten"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["kveten"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["kveten"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["kveten"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["kveten"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["kveten"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["kveten"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("cerven"))
                {
                    ((ListView)tp.Controls["cerven"]).BeginUpdate();
                    ((ListView)tp.Controls["cerven"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["cerven"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["cerven"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["cerven"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["cerven"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["cerven"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["cerven"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["cerven"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["cerven"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["cerven"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["cerven"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("cervenec"))
                {
                    ((ListView)tp.Controls["cervenec"]).BeginUpdate();
                    ((ListView)tp.Controls["cervenec"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["cervenec"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["cervenec"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["cervenec"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["cervenec"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["cervenec"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["cervenec"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["cervenec"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["cervenec"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["cervenec"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["cervenec"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("srpen"))
                {
                    ((ListView)tp.Controls["srpen"]).BeginUpdate();
                    ((ListView)tp.Controls["srpen"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["srpen"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["srpen"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["srpen"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["srpen"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["srpen"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["srpen"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["srpen"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["srpen"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["srpen"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["srpen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("zari"))
                {
                    ((ListView)tp.Controls["zari"]).BeginUpdate();
                    ((ListView)tp.Controls["zari"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["zari"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["zari"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["zari"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["zari"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["zari"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["zari"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["zari"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["zari"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["zari"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["zari"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("rijen"))
                {
                    ((ListView)tp.Controls["rijen"]).BeginUpdate();
                    ((ListView)tp.Controls["rijen"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["rijen"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["rijen"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["rijen"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["rijen"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["rijen"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["rijen"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["rijen"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["rijen"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["rijen"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["rijen"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("listopad"))
                {
                    ((ListView)tp.Controls["listopad"]).BeginUpdate();
                    ((ListView)tp.Controls["listopad"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["listopad"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["listopad"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["listopad"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["listopad"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["listopad"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["listopad"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["listopad"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["listopad"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["listopad"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["listopad"]).EndUpdate();
                }
                if (tp.Controls.ContainsKey("prosinec"))
                {
                    ((ListView)tp.Controls["prosinec"]).BeginUpdate();
                    ((ListView)tp.Controls["prosinec"]).Columns[1].Text = form.jazyk.Header_PC;
                    ((ListView)tp.Controls["prosinec"]).Columns[2].Text = form.jazyk.Header_TicketID;
                    ((ListView)tp.Controls["prosinec"]).Columns[3].Text = form.jazyk.Header_Zakaznik;
                    ((ListView)tp.Controls["prosinec"]).Columns[4].Text = form.jazyk.Header_Popis;
                    ((ListView)tp.Controls["prosinec"]).Columns[5].Text = form.jazyk.Header_Kontakt;
                    ((ListView)tp.Controls["prosinec"]).Columns[6].Text = form.jazyk.Header_Terp;
                    ((ListView)tp.Controls["prosinec"]).Columns[7].Text = form.jazyk.Header_Task;
                    ((ListView)tp.Controls["prosinec"]).Columns[8].Text = form.jazyk.Header_Cas;
                    ((ListView)tp.Controls["prosinec"]).Columns[9].Text = form.jazyk.Header_Stav;
                    ((ListView)tp.Controls["prosinec"]).Columns[10].Text = form.jazyk.Header_Poznamka;
                    ((ListView)tp.Controls["prosinec"]).EndUpdate();
                }
            }

            form.Text = form.jazyk.Header_Tickety;
            form.převéstNaFormátMilleniumToolStripMenuItem.Text = form.jazyk.Menu_PrevestNaMillenium;
            form.dostupnéJazykyToolStripMenuItem.Text = form.jazyk.Menu_DostupneJazyky;
            form.toolStripButton8.Text = form.jazyk.Menu_Upozorneni;
            form.LoadFile();
        }

        //obecné
        internal string Zkratka { get; private set; }
        internal string Jmeno { get; private set; }
        internal int Verze { get; private set; }
        internal int Revize { get; private set; }
        internal Dictionary<string, string> InterniPamet { get; set; }
    }
}
