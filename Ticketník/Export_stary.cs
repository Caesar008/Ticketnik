using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Excel = ClosedXML.Excel;

namespace Ticketník
{
    public partial class Export : Form
    {
        private void Export_Stary()
        {
            if (!InvokeRequired)
                form.timer_ClearInfo.Stop();
            else
                this.BeginInvoke(new Action(() => form.timer_ClearInfo.Stop()));
            form.infoBox.Text = form.jazyk.Message_Exportuji;
            form.Update();

            DateTime start;
            DateTime konec;
            over = false;
            SortedDictionary<DateTime, Dictionary<Ticket.TerpTyp, Dictionary<Ticket, DateTime>>> dny = new SortedDictionary<DateTime, Dictionary<Ticket.TerpTyp, Dictionary<Ticket, DateTime>>>();

            outputCSV = "START_HEADER\r\nOverriding Approver,,,,,,,,,,,,,,,,,\r\nComments,,,,,,,,,,,,,,,,,\r\nSTOP_HEADER\r\n,,,,,,,,,,,,,,,,,\r\nSTART_TEMPLATE\r\nProject,Task,Type,Mon,CommentText,Tue,CommentText,Wed,CommentText,Thu,CommentText,Fri,CommentText,Sat,CommentText,Sun,CommentText,END_COLUMN\r\n";

            int odec = 0;
            if (DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).DayOfWeek == DayOfWeek.Sunday && (int)DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).DayOfWeek == 0)
                odec = 1;

            if (radioButton3.Checked)
            {
                start = dateTimePicker1.Value;
                konec = dateTimePicker2.Value;
            }
            else if (radioButton1.Checked)
            {
                konec = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                //tohle zrevidovat, jestli by nestačilo jen to Add day v catch bez try
                try
                {
                    start = new DateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Year, DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Month, DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Day + odec);
                }
                catch
                {
                    start = new DateTime(DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Year, DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Month, DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Day);
                    start = start.AddDays(odec);
                }
            }
            else
            {
                start = DateTime.Now.AddDays(-7);
                start = start.AddDays(-(int)(start.DayOfWeek - 1));
                start = new DateTime(start.Year, start.Month, start.Day);

                konec = new DateTime(start.Year, start.Month, start.Day).AddDays(6);
            }


            for (DateTime d = start; d <= konec; d = d.AddDays(1))
            {

                if (!dny.ContainsKey(d))
                    dny.Add(d, new Dictionary<Ticket.TerpTyp, Dictionary<Ticket, DateTime>> {
                    { Ticket.TerpTyp.MalyHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyNormal, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyProblem, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniNormal, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniProblem, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyNormal, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyProblem, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyNormalNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyNormalPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyProblemHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyProblemNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MalyProblemPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniNormalNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniNormalPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniProblemHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniProblemNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.StredniProblemPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyNormalNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyNormalPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyProblemHolyday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyProblemNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.VelkyProblemPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.Enkripce, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceHoliday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripcePrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceProblem, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceProblemHoliday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceProblemNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.EnkripceProblemPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.Mobility, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityHoliday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityProblem, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityProblemHoliday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityProblemNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.MobilityProblemPrescas, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.Custom, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.CustomHoliday, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.CustomNahradni, new Dictionary<Ticket, DateTime>() },
                    { Ticket.TerpTyp.CustomPrescas, new Dictionary<Ticket, DateTime>() }
                    });
                if (form.poDnech.ContainsKey(d))
                {
                    foreach (string s in form.poDnech[d].Keys)
                    {
                        foreach (Ticket t in form.poDnech[d][s])
                        {
                            DateTime pauzaDohromady = new DateTime();
                            DateTime cas = new DateTime();
                            DateTime hrubyCas = new DateTime();

                            for (int i = 0; i < t.PauzyDo.Count; i++)
                            {
                                if (t.PauzyDo[i].ToString("H:mm") != "0:00")
                                    pauzaDohromady = pauzaDohromady.AddHours(t.PauzyDo[i].Hour - t.PauzyOd[i].Hour).AddMinutes(t.PauzyDo[i].Minute - t.PauzyOd[i].Minute);

                            }

                            try
                            {
                                cas = cas.AddHours((t.Do.Hour - t.Od.Hour) - pauzaDohromady.Hour).AddMinutes((t.Do.Minute - t.Od.Minute) - pauzaDohromady.Minute);
                                hrubyCas = form.RoundUp(cas, TimeSpan.FromMinutes(30));

                                dny[d][t.TerpT].Add(t, hrubyCas);
                            }
                            catch
                            {
                                MessageBox.Show(form.jazyk.Windows_Export_Ticket + " " + t.ID + " - " + t.Zakaznik + ", " + form.jazyk.Windows_Export_NaKteremJsiPracoval + " " + t.Datum.ToString("d.MM.yyyy") + ", " + form.jazyk.Windows_Export_Neukoncen);
                            }
                        }
                    }
                }
            }

            string terpCSV = "";
            string tempTerp = "";
            Dictionary<string, string[]> terpDict = new Dictionary<string, string[]>();

            DateTime[] casCelkemDenne = new DateTime[7];
            Dictionary<byte, Dictionary<Ticket.TerpTyp, byte>> poctyTicketu = new Dictionary<byte, Dictionary<Ticket.TerpTyp, byte>>();

            Dictionary<string, int> customTerpyTime = new Dictionary<string, int>();
            Dictionary<string, string> customTerpy = new Dictionary<string, string>();

            foreach (DateTime d in dny.Keys)
            {
                customTerpyTime.Clear();
                string date = "";
                date = d.ToString("d.MM.yyyy") + "\r\n\r\n";

                foreach (Ticket.TerpTyp t in dny[d].Keys)
                {

                    switch (t)
                    {
                        case Ticket.TerpTyp.MalyHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.MalyNormal:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.MalyProblem:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.MalyNormalNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.MalyNormalPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.MalyProblemHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.MalyProblemNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.MalyProblemPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas);
                            break;

                        case Ticket.TerpTyp.StredniHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.StredniNormal:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.StredniProblem:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.StredniNormalNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.StredniNormalPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.StredniProblemHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.StredniProblemNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.StredniProblemPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas);
                            break;

                        case Ticket.TerpTyp.VelkyHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.VelkyNormal:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.VelkyProblem:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.VelkyNormalNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.VelkyNormalPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.VelkyProblemHolyday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.VelkyProblemNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.VelkyProblemPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.Enkripce:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.EnkripceHoliday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.EnkripceNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.EnkripcePrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.EnkripceProblem:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.EnkripceProblemHoliday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.EnkripceProblemNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.EnkripceProblemPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas);
                            break;



                        case Ticket.TerpTyp.Mobility:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.MobilityHoliday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.MobilityNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.MobilityPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas);
                            break;
                        case Ticket.TerpTyp.MobilityProblem:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Normal), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Normal);
                            break;
                        case Ticket.TerpTyp.MobilityProblemHoliday:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek);
                            break;
                        case Ticket.TerpTyp.MobilityProblemNahradni:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni);
                            break;
                        case Ticket.TerpTyp.MobilityProblemPrescas:
                            if (!terpDict.ContainsKey(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)))
                                terpDict.Add(Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas), new string[7]);
                            terpCSV = Zakaznici.GetTerp(Zakaznici.Velikost.Mobility, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas);
                            break;
                    }

                    //udělat dictionary na custom terpy jako je string tickety. Tam se bude ukládat to samé, co tickety, jen to
                    //bude pro případ custom terpů a pak na konci to do csv zaes podmínkama vyplive celý string z dictionary

                    string tickety = "";
                    DateTime hodiny = new DateTime();
                    Dictionary<string, DateTime> hodinyCustom = new Dictionary<string, DateTime>();

                    int i = 0;

                    int pric = 0;

                    if ((int)d.DayOfWeek == 0)
                        pric = 7;

                    foreach (Ticket tik in dny[d][t].Keys)
                    {
                        switch (t)
                        {
                            case Ticket.TerpTyp.Custom:
                                if (!terpDict.ContainsKey(Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal)))
                                    terpDict.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal), new string[7]);
                                terpCSV = Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal);
                                if (!customTerpy.Keys.Contains(Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal)))
                                {
                                    customTerpy.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal), string.Empty);
                                    customTerpyTime.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Normal), 0);
                                }
                                break;
                            case Ticket.TerpTyp.CustomHoliday:
                                if (!terpDict.ContainsKey(Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek)))
                                    terpDict.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek), new string[7]);
                                terpCSV = Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek);
                                if (!customTerpy.Keys.Contains(Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek)))
                                {
                                    customTerpy.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek), string.Empty);
                                    customTerpyTime.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Svatek), 0);
                                }
                                break;
                            case Ticket.TerpTyp.CustomNahradni:
                                if (!terpDict.ContainsKey(Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni)))
                                    terpDict.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni), new string[7]);
                                terpCSV = Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni);
                                if (!customTerpy.Keys.Contains(Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni)))
                                {
                                    customTerpy.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni), string.Empty);
                                    customTerpyTime.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Nahradni), 0);
                                }
                                break;
                            case Ticket.TerpTyp.CustomPrescas:
                                if (!terpDict.ContainsKey(Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas)))
                                    terpDict.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas), new string[7]);
                                terpCSV = Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas);
                                if (!customTerpy.Keys.Contains(Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas)))
                                {
                                    customTerpy.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas), string.Empty);
                                    customTerpyTime.Add(Zakaznici.GetTerp(tik, Zakaznici.Typ.Prescas), 0);
                                }
                                break;
                        }

                        string radek = "";
                        if (i != 0)
                            radek = "\r\n";


                        casCelkemDenne[(int)d.DayOfWeek - 1 + pric] = casCelkemDenne[(int)d.DayOfWeek - 1 + pric].AddHours(dny[d][t][tik].Hour);
                        casCelkemDenne[(int)d.DayOfWeek - 1 + pric] = casCelkemDenne[(int)d.DayOfWeek - 1 + pric].AddMinutes(dny[d][t][tik].Minute);
                        if (!poctyTicketu.ContainsKey((byte)((byte)d.DayOfWeek - 1)))
                        {
                            poctyTicketu.Add((byte)((byte)d.DayOfWeek - 1), new Dictionary<Ticket.TerpTyp, byte>());

                        }
                        if (!poctyTicketu[(byte)((byte)d.DayOfWeek - 1)].ContainsKey(t))
                        {
                            poctyTicketu[(byte)((byte)d.DayOfWeek - 1)].Add(t, 0);
                        }
                        poctyTicketu[(byte)((byte)d.DayOfWeek - 1)][t]++;

                        if (t != Ticket.TerpTyp.Custom && t != Ticket.TerpTyp.CustomHoliday && t != Ticket.TerpTyp.CustomNahradni && t != Ticket.TerpTyp.CustomPrescas)
                        {
                            if ((tickety + radek + tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis).Length > 240)
                            {

                                string minutys = "";
                                if (hodiny.Minute == 30)
                                    minutys = ".5";
                                terpDict[terpCSV][((int)d.DayOfWeek) - 1] = hodiny.Hour + minutys + ",\"" + tickety + "\"";

                                tempTerp = terpCSV = terpCSV + "+";

                                if (!terpDict.ContainsKey(tempTerp))
                                    terpDict.Add(tempTerp, new string[7]);
                                hodiny = new DateTime();
                                tickety = tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis;
                                hodiny = hodiny.AddHours(dny[d][t][tik].Hour);
                                hodiny = hodiny.AddMinutes(dny[d][t][tik].Minute);
                                over = true;
                            }
                            else
                            {
                                tickety += radek + tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis;

                                hodiny = hodiny.AddHours(dny[d][t][tik].Hour);
                                hodiny = hodiny.AddMinutes(dny[d][t][tik].Minute);
                            }
                        }
                        else
                        {
                            if ((customTerpy[terpCSV] + radek + tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis).Length > 240)
                            {

                                string minutys = "";
                                if (hodinyCustom[terpCSV].Minute == 30)
                                    minutys = ".5";
                                terpDict[terpCSV][((int)d.DayOfWeek) - 1] = hodinyCustom[terpCSV].Hour + minutys + ",\"" + customTerpy[terpCSV] + "\"";

                                tempTerp = terpCSV = terpCSV + "+";

                                if (!terpDict.ContainsKey(tempTerp))
                                    terpDict.Add(tempTerp, new string[7]);
                                if (!customTerpy.ContainsKey(terpCSV))
                                    customTerpy.Add(terpCSV, string.Empty);
                                if (!customTerpyTime.ContainsKey(terpCSV))
                                    customTerpyTime.Add(terpCSV, 0);
                                if (!hodinyCustom.ContainsKey(terpCSV))
                                    hodinyCustom.Add(terpCSV, new DateTime());
                                customTerpy[terpCSV] = tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis;
                                customTerpyTime[terpCSV]++;
                                hodinyCustom[terpCSV] = hodinyCustom[terpCSV].AddHours(dny[d][t][tik].Hour);
                                hodinyCustom[terpCSV] = hodinyCustom[terpCSV].AddMinutes(dny[d][t][tik].Minute);
                                over = true;
                            }
                            else
                            {
                                if (!hodinyCustom.ContainsKey(terpCSV))
                                    hodinyCustom.Add(terpCSV, new DateTime());
                                customTerpy[terpCSV] += radek + tik.ID + "\t" + tik.Zakaznik + "\t" + tik.Popis;
                                customTerpyTime[terpCSV]++;

                                hodinyCustom[terpCSV] = hodinyCustom[terpCSV].AddHours(dny[d][t][tik].Hour);
                                hodinyCustom[terpCSV] = hodinyCustom[terpCSV].AddMinutes(dny[d][t][tik].Minute);
                            }
                        }
                        i++;
                    }
                    string minuty = "";
                    if (hodiny.Minute == 30)
                        minuty = ".5";

                    if (t != Ticket.TerpTyp.Custom && t != Ticket.TerpTyp.CustomHoliday && t != Ticket.TerpTyp.CustomNahradni && t != Ticket.TerpTyp.CustomPrescas)
                    {
                        if (tickety.Length != 0)
                        {
                            date = "";

                            if (terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric] == null)
                                terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric] = hodiny.Hour + minuty + ",\"" + tickety + "\"";
                            else
                            {

                                string tmpTime = terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric].Split(',')[0];
                                string tmpText = terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric].Split(',')[1].Replace("\"", "");

                                if (("\"" + tmpText + "\r\n" + tickety + "\"").Length <= 240)
                                {

                                    int tmpHour = int.Parse(tmpTime.Split('.')[0]);
                                    int tmpMinute = 0;
                                    if (tmpTime.Split('.').Length > 1)
                                        tmpMinute = int.Parse(tmpTime.Split('.')[1]);

                                    if (tmpMinute == 5)
                                    {
                                        minuty = "";
                                        hodiny = hodiny.AddHours(1);
                                    }
                                    hodiny = hodiny.AddHours(tmpHour);

                                    terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric] = hodiny.Hour + minuty + ",\"" + tmpText + "\r\n" + tickety + "\"";
                                }
                                else
                                {
                                    terpDict[terpCSV][((int)d.DayOfWeek) - 1 + pric] = tmpTime + ",\"" + tmpText + "\"";

                                    tempTerp = terpCSV = terpCSV + "+";

                                    if (!terpDict.ContainsKey(tempTerp))
                                        terpDict.Add(tempTerp, new string[7]);

                                    terpDict[tempTerp][((int)d.DayOfWeek) - 1 + pric] = hodiny.Hour + minuty + ",\"" + tickety + "\"";

                                    over = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string s in customTerpy.Keys)
                        {
                            if (customTerpy[s].Length != 0)
                            {
                                date = "";

                                if (terpDict[s][((int)d.DayOfWeek) - 1] == null)
                                    terpDict[s][((int)d.DayOfWeek) - 1] = hodinyCustom[s].Hour + minuty + ",\"" + customTerpy[s] + "\"";
                                else
                                {
                                    string tmpTime = terpDict[s][((int)d.DayOfWeek) - 1].Split(',')[0];
                                    string tmpText = terpDict[s][((int)d.DayOfWeek) - 1].Split(',')[1].Replace("\"", "");

                                    if (("\"" + tmpText + "\r\n" + customTerpy[s] + "\"").Length <= 240)
                                    {

                                        int tmpHour = int.Parse(tmpTime.Split('.')[0]);
                                        int tmpMinute = 0;
                                        if (tmpTime.Split('.').Length > 1)
                                            tmpMinute = int.Parse(tmpTime.Split('.')[1]);

                                        if (tmpMinute == 5)
                                        {
                                            minuty = "";
                                            hodinyCustom[s] = hodinyCustom[s].AddHours(1);
                                        }
                                        hodinyCustom[s] = hodinyCustom[s].AddHours(tmpHour);

                                        terpDict[s][((int)d.DayOfWeek) - 1] = hodinyCustom[s].Hour + minuty + ",\"" + tmpText + "\r\n" + customTerpy[s] + "\"";
                                    }
                                    else
                                    {
                                        terpDict[s][((int)d.DayOfWeek) - 1] = tmpTime + ",\"" + tmpText + "\"";

                                        tempTerp = terpCSV = terpCSV + "+";

                                        if (!terpDict.ContainsKey(tempTerp))
                                            terpDict.Add(tempTerp, new string[7]);

                                        terpDict[tempTerp][((int)d.DayOfWeek) - 1] = hodinyCustom[s].Hour + minuty + ",\"" + customTerpy[s] + "\"";

                                        over = true;
                                    }
                                }
                            }
                        }
                        customTerpy.Clear();
                    }
                }

            }

            //rozpočítání času do 8 hodin
            for (byte i = 0; i < 7; i++)
            {
                int pocetTicketu = 0;
                DateTime den = casCelkemDenne[i];

                //nepočítám overtimy záměrně
                if (poctyTicketu.ContainsKey(i))
                {
                    foreach (Ticket.TerpTyp t in poctyTicketu[i].Keys)
                    {
                        if (t != Ticket.TerpTyp.MalyNormalPrescas && t != Ticket.TerpTyp.MalyProblemPrescas &&
                            t != Ticket.TerpTyp.StredniNormalPrescas && t != Ticket.TerpTyp.StredniProblemPrescas &&
                            t != Ticket.TerpTyp.VelkyNormalPrescas && t != Ticket.TerpTyp.VelkyProblemPrescas &&
                            t != Ticket.TerpTyp.MalyNormalNahradni && t != Ticket.TerpTyp.MalyProblemNahradni &&
                            t != Ticket.TerpTyp.StredniNormalNahradni && t != Ticket.TerpTyp.StredniProblemNahradni &&
                            t != Ticket.TerpTyp.VelkyNormalNahradni && t != Ticket.TerpTyp.VelkyProblemNahradni &&
                            t != Ticket.TerpTyp.EnkripceProblemNahradni && t != Ticket.TerpTyp.EnkripceProblemPrescas &&
                            t != Ticket.TerpTyp.EnkripcePrescas && t != Ticket.TerpTyp.EnkripceNahradni &&
                            t != Ticket.TerpTyp.CustomPrescas && t != Ticket.TerpTyp.CustomNahradni)
                            pocetTicketu += poctyTicketu[i][t];
                    }

                    int minutyRozdil = (8 * 60) - ((den.Hour * 60) + (den.Minute));

                    double naTicket = (double)minutyRozdil / (double)pocetTicketu;
                    double celkemSpocitano = 0;
                    int spocitanoTicketu = 0;

                    //custom terpy přepočítávání časů, vymyslet nemazání custom terpů z listu
                    //přerozdělení
                    foreach (Ticket.TerpTyp t in poctyTicketu[i].Keys)
                    {
                        string[] tmp = new string[2] { "", "" };
                        double terpnuto = 0;
                        switch (t)
                        {
                            //123074
                            case Ticket.TerpTyp.Custom:
                                foreach (string s in customTerpyTime.Keys)
                                {
                                    if (terpDict[s][i] != null)
                                    {
                                        tmp = terpDict[s][i].Split(',');
                                        terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))/* / ((double)poctyTicketu[i][t] / (double)customTerpyTime[s])*/;
                                        terpnuto += (customTerpyTime[s] * naTicket) / 60;
                                        spocitanoTicketu += customTerpyTime[s];

                                        celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);
                                        while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                        {
                                            celkemSpocitano += 0.5;
                                            terpnuto += 0.5;
                                        }

                                        while (celkemSpocitano > 8)
                                        {
                                            celkemSpocitano -= 0.5;
                                            terpnuto -= 0.5;
                                        }

                                        tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                        terpDict[s][i] = tmp[0] + "," + tmp[1];
                                    }
                                }
                                break;
                            case Ticket.TerpTyp.CustomHoliday:
                                foreach (string s in customTerpyTime.Keys)
                                {
                                    if (terpDict[s][i] != null)
                                    {
                                        tmp = terpDict[s][i].Split(',');
                                        terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                        terpnuto += (customTerpyTime[s] * naTicket) / 60;
                                        spocitanoTicketu += customTerpyTime[s];


                                        celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);
                                        while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                        {
                                            celkemSpocitano += 0.5;
                                            terpnuto += 0.5;
                                        }

                                        while (celkemSpocitano > 8)
                                        {
                                            celkemSpocitano -= 0.5;
                                            terpnuto -= 0.5;
                                        }

                                        tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                        terpDict[s][i] = tmp[0] + "," + tmp[1];
                                    }
                                }
                                break;
                            case Ticket.TerpTyp.CustomNahradni:
                                foreach (string s in customTerpyTime.Keys)
                                {
                                    if (terpDict[s][i] != null)
                                    {
                                        tmp = terpDict[s][i].Split(',');
                                        terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                        celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);


                                        tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                        terpDict[s][i] = tmp[0] + "," + tmp[1];
                                    }
                                }
                                break;
                            case Ticket.TerpTyp.CustomPrescas:
                                foreach (string s in customTerpyTime.Keys)
                                {
                                    if (terpDict[s][i] != null)
                                    {
                                        tmp = terpDict[s][i].Split(',');
                                        terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                        celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);

                                        tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                        terpDict[s][i] = tmp[0] + "," + tmp[1];
                                    }
                                }
                                break;
                            case Ticket.TerpTyp.MalyNormalNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyNormal:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyProblem:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyNormalPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyProblemHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyProblemNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.MalyProblemPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Maly, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;

                            //165793
                            case Ticket.TerpTyp.StredniNormalNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniNormal:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniProblem:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniNormalPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniProblemHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniProblemNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.StredniProblemPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Stredni, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;

                            //127838
                            case Ticket.TerpTyp.VelkyNormalNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyNormal:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyProblem:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyNormalPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyProblemHolyday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyProblemNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.VelkyProblemPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Velky, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;

                            //123082
                            case Ticket.TerpTyp.EnkripceNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / 2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.Enkripce:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripceProblem:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Normal)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripceHoliday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripcePrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Incident, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripceProblemHoliday:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                    terpnuto += (poctyTicketu[i][t] * naTicket) / 60;
                                    spocitanoTicketu += poctyTicketu[i][t];

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    while (spocitanoTicketu == pocetTicketu && celkemSpocitano < 8 && !over)
                                    {
                                        celkemSpocitano += 0.5;
                                        terpnuto += 0.5;
                                    }

                                    while (celkemSpocitano > 8)
                                    {
                                        celkemSpocitano -= 0.5;
                                        terpnuto -= 0.5;
                                    }

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Svatek)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripceProblemNahradni:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Nahradni)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                            case Ticket.TerpTyp.EnkripceProblemPrescas:
                                if (terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] != null)
                                {
                                    tmp = terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i].Split(',');
                                    terpnuto = double.Parse(tmp[0].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                                    celkemSpocitano += (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2);

                                    tmp[0] = (Math.Round(terpnuto * 2, MidpointRounding.AwayFromZero) / (double)2).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
                                    terpDict[Zakaznici.GetTerp(Zakaznici.Velikost.Encrypce, Zakaznici.Task.Problem, Zakaznici.Typ.Prescas)][i] = tmp[0] + "," + tmp[1];
                                }
                                break;
                        }
                    }

                }
            }

            //pro excel
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx", Properties.Resources.mytime_template);

            Excel.XLWorkbook export = new Excel.XLWorkbook(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx");
            Excel.IXLWorksheet exportSheet = export.Worksheet(1);

            //starý Excel
            /*Excel.Workbook export = null;
            Excel.Application exportApp = null;
            Excel.Worksheet exportSheet = null;
            exportApp = new Excel.Application();
            export = exportApp.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xls");
            exportSheet = export.Sheets[1];*/

            int row = 2;

            foreach (string s in terpDict.Keys)
            {
                if (terpDict[s][0] == null)
                    terpDict[s][0] = ",";
                if (terpDict[s][1] == null)
                    terpDict[s][1] = ",";
                if (terpDict[s][2] == null)
                    terpDict[s][2] = ",";
                if (terpDict[s][3] == null)
                    terpDict[s][3] = ",";
                if (terpDict[s][4] == null)
                    terpDict[s][4] = ",";
                if (terpDict[s][5] == null)
                    terpDict[s][5] = ",";
                if (terpDict[s][6] == null)
                    terpDict[s][6] = ",";

                if (terpDict[s][0] == "," && terpDict[s][1] == "," && terpDict[s][2] == "," && terpDict[s][3] == "," && terpDict[s][4] == "," && terpDict[s][5] == "," && terpDict[s][6] == ",")
                    continue;

                //project
                exportSheet.Cell(row, 1).Value = s.Replace("+", "").Split(',')[0];
                //exportSheet.Cells[row, 1] = s.Replace("+", "").Split(',')[0];
                //project name
                exportSheet.Cell(row, 2).Value = "najdiSiSam";
                //task
                //exportSheet.Cell(row, 3).DataType = Excel.XLDataType.Text;
                //exportSheet.Cell(row, 3).Style.NumberFormat.Format = "@";
                //exportSheet.Cell(row, 3).Value = s.Replace("+", "").Split(',')[1];
                exportSheet.Cell(row, 3).SetValue(s.Replace("+", "").Split(',')[1]);
                //task name
                exportSheet.Cell(row, 4).Value = "TyVisCo";
                //type
                exportSheet.Cell(row, 5).Value = s.Replace("+", "").Split(',')[2];
                //pondělí
                exportSheet.Cell(row, 6).Value = terpDict[s][0].Split(',')[0];
                exportSheet.Cell(row, 7).Value = terpDict[s][0].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //úterý
                exportSheet.Cell(row, 10).Value = terpDict[s][1].Split(',')[0];
                exportSheet.Cell(row, 11).Value = terpDict[s][1].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //středa
                exportSheet.Cell(row, 14).Value = terpDict[s][2].Split(',')[0];
                exportSheet.Cell(row, 15).Value = terpDict[s][2].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //čtvrtek
                exportSheet.Cell(row, 18).Value = terpDict[s][3].Split(',')[0];
                exportSheet.Cell(row, 19).Value = terpDict[s][3].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //pátek
                exportSheet.Cell(row, 22).Value = terpDict[s][4].Split(',')[0];
                exportSheet.Cell(row, 23).Value = terpDict[s][4].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //sobota
                exportSheet.Cell(row, 26).Value = terpDict[s][5].Split(',')[0];
                exportSheet.Cell(row, 27).Value = terpDict[s][5].Split(',')[1].Replace("\t", " ").Replace("\"", "");
                //neděle
                exportSheet.Cell(row, 30).Value = terpDict[s][6].Split(',')[0];
                exportSheet.Cell(row, 31).Value = terpDict[s][6].Split(',')[1].Replace("\t", " ").Replace("\"", "");

                outputCSV += s.Replace("+", "") + "" + terpDict[s][0] + "," + terpDict[s][1] + "," + terpDict[s][2] + "," + terpDict[s][3] + "," + terpDict[s][4] + "," + terpDict[s][5] + "," + terpDict[s][6] + ",\r\n";
                row++;
            }

            outputCSV += "STOP_TEMPLATE,END";
            export.Save();
            export.Dispose();

            form.infoBox.Text = "";

            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel|*.xlsx|" + form.jazyk.Menu_Soubor + " CSV|*.csv";
            saveFileDialog1.FileName = "MyTime Info.xlsx";
            saveFileDialog1.ShowDialog();
        }
    }
}
