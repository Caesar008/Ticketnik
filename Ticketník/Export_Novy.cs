using System;
using System.Collections.Generic;
using fNbt;
using System.Windows.Forms;
using System.IO;
using Excel = ClosedXML.Excel;
using System.Linq;
using System.Globalization;

namespace Ticketník
{
    public partial class Export : Form
    {
        private void Export_Novy()
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
            List<ExportRow> exportRadky = new List<ExportRow>();

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

            //Projít tickety splňující podmínku a zařadit je správně do exportu
            for (DateTime d = start; d <= konec; d = d.AddDays(1))
            {
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

                                //dny[d][t.TerpT].Add(t, hrubyCas);
                                string den = "";
                                switch (d.DayOfWeek)
                                {
                                    case DayOfWeek.Monday: den = "Pondělí";
                                            break;
                                    case DayOfWeek.Tuesday: den = "Úterý";
                                        break;
                                    case DayOfWeek.Wednesday: den = "Středa";
                                        break;
                                    case DayOfWeek.Thursday: den = "Čtvrtek";
                                        break;
                                    case DayOfWeek.Friday: den = "Pátek";
                                        break;
                                    case DayOfWeek.Saturday: den = "Sobota";
                                        break;
                                    case DayOfWeek.Sunday: den = "Neděle";
                                        break;
                                }

                                ExportTyp et;
                                if (t.TypPrace == 0 || t.TypPrace == 2 || t.TypPrace == 8 || t.TypPrace == 12 || t.TypPrace == 16 || t.TypPrace == 20 || t.TypPrace == 24)
                                {
                                    et = ExportTyp.Normal;
                                }
                                else if (t.TypPrace == 1 || t.TypPrace == 6 || t.TypPrace == 10 || t.TypPrace == 15 || t.TypPrace == 18 || t.TypPrace == 23 || t.TypPrace == 27)
                                {
                                    et = ExportTyp.Holiday;
                                }
                                else if (t.TypPrace == 3 || t.TypPrace == 5 || t.TypPrace == 9 || t.TypPrace == 13 || t.TypPrace == 17 || t.TypPrace == 21 || t.TypPrace == 25)
                                {
                                    et = ExportTyp.Prescas;
                                }
                                else if (t.TypPrace == 4 || t.TypPrace == 7 || t.TypPrace == 11 || t.TypPrace == 14 || t.TypPrace == 19 || t.TypPrace == 22 || t.TypPrace == 26)
                                {
                                    et = ExportTyp.Compens;
                                }
                                else
                                    et = ExportTyp.Normal;

                                if (exportRadky.Count == 0)
                                    exportRadky.Add(new ExportRow());

                                List<int> toSkip = new List<int>();
                                if (t.CustomTerp == "")
                                    t.CustomTerp = Zakaznici.GetTerp(t.Zakaznik);
                                //když není task, tak defaultně incident 1.2.1 - nová verze by neměla umět uložit bez tasku
                                if (t.CustomTask == "")
                                    t.CustomTask = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;

                                //přiřazení ticketu ke správnému řádku a dni
                                for (int i = 0; i< exportRadky.Count; i++)
                                {
                                    if (exportRadky[i].Terp == null)
                                    {
                                        exportRadky[i].Terp = t.CustomTerp;
                                        exportRadky[i].Task = t.CustomTask;
                                        exportRadky[i].Typ = et;
                                        exportRadky[i].Radek[den].Koment = t.ID + " " + t.Zakaznik + " " + t.Popis + "\r\n";
                                        decimal tCas = hrubyCas.Hour;
                                        if (hrubyCas.Minute == 30)
                                            tCas += 0.5m;
                                        exportRadky[i].Radek[den].Cas = tCas;
                                        break;
                                    }
                                    else if (exportRadky[i].Terp == t.CustomTerp && exportRadky[i].Task == t.CustomTask && exportRadky[i].Typ == et)
                                    {
                                        if ((exportRadky[i].Radek[den].Koment.Length + (t.ID + " " + t.Zakaznik + " " + t.Popis + "\r\n").Length < 240))
                                        {
                                            exportRadky[i].Radek[den].Koment += t.ID + " " + t.Zakaznik + " " + t.Popis + "\r\n";
                                            decimal tCas = hrubyCas.Hour;
                                            if (hrubyCas.Minute == 30)
                                                tCas += 0.5m;
                                            exportRadky[i].Radek[den].Cas += tCas;
                                            break;
                                        }
                                        else
                                        {
                                            exportRadky.Add(new ExportRow());
                                            continue;
                                        }
                                    }
                                    else if (i < exportRadky.Count - 1)
                                        continue;
                                    exportRadky.Add(new ExportRow());
                                }
                            }
                            catch
                            {
                                MessageBox.Show(form.jazyk.Windows_Export_Ticket + " " + t.ID + " - " + t.Zakaznik + ", " + form.jazyk.Windows_Export_NaKteremJsiPracoval + " " + t.Datum.ToString("d.MM.yyyy") + ", " + form.jazyk.Windows_Export_Neukoncen);
                            }
                        }
                    }
                }
            }

            //přepočet času na 8h
            //celkový čas normálních ticketů (statní se neupravují)
            Dictionary<string, decimal> casy = new Dictionary<string, decimal> { { "Pondělí", 0 }, { "Úterý", 0 }, { "Středa", 0 }, { "Čtvrtek", 0 }, { "Pátek", 0 }, { "Sobota", 0 }, { "Neděle", 0 } };

            foreach (ExportRow s in exportRadky)
            {
                if(s.Typ == ExportTyp.Normal)
                {
                    casy["Pondělí"] += s.Radek["Pondělí"].Cas;
                    casy["Úterý"] += s.Radek["Úterý"].Cas;
                    casy["Středa"] += s.Radek["Středa"].Cas;
                    casy["Čtvrtek"] += s.Radek["Čtvrtek"].Cas;
                    casy["Pátek"] += s.Radek["Pátek"].Cas;
                    casy["Sobota"] += s.Radek["Sobota"].Cas;
                    casy["Neděle"] += s.Radek["Neděle"].Cas;
                }
            }

            //výběr řádků, co se upraví čas
            foreach(string cs in casy.Keys)
            {
                if (casy[cs] == 8 || casy[cs] == 0)
                    continue;
                else if (casy[cs] < 8)
                {
                    Dictionary<int, decimal> pridat = new Dictionary<int, decimal>();

                    for(int i = 0; i< exportRadky.Count; i++)
                    {
                        if(exportRadky[i].Typ == ExportTyp.Normal && exportRadky[i].Radek[cs].Cas > 0)
                        {
                            pridat.Add(i, exportRadky[i].Radek[cs].Cas);
                        }
                    }

                    pridat = pridat.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    decimal zbyva = 8 - casy[cs];
                    decimal prumerNaRadek = Math.Ceiling((zbyva / pridat.Count) / 0.5m)*0.5m;

                    //úprava času
                    int index = 0;
                    for(decimal d = zbyva; d > 0; d-=prumerNaRadek)
                    {
                        if (d - prumerNaRadek < 0)
                            prumerNaRadek = d;

                        pridat[pridat.Keys.ElementAt(index)] += prumerNaRadek;

                        index++;
                        zbyva -= prumerNaRadek;
                    }
                    foreach(int newI in pridat.Keys)
                    {
                        exportRadky[newI].Radek[cs].Cas = pridat[newI];
                    }
                }
                else if (casy[cs] > 8)
                {
                    Dictionary<int, decimal> ubrat = new Dictionary<int, decimal>();

                    for (int i = 0; i < exportRadky.Count; i++)
                    {
                        if (exportRadky[i].Typ == ExportTyp.Normal && exportRadky[i].Radek[cs].Cas > 0.5m)
                        {
                            ubrat.Add(i, exportRadky[i].Radek[cs].Cas);
                        }
                    }

                    ubrat = ubrat.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    decimal zbyva = casy[cs] - 8;
                    decimal prumerNaRadek = Math.Ceiling((zbyva / ubrat.Count) / 0.5m) * 0.5m;

                    //úprava času
                    int index = 0;
                    for (decimal d = zbyva; d > 0; d -= prumerNaRadek)
                    {
                        if (d - prumerNaRadek < 0)
                            prumerNaRadek = d;

                        ubrat[ubrat.Keys.ElementAt(index)] -= prumerNaRadek;

                        index++;
                        zbyva -= prumerNaRadek;
                    }
                    foreach (int newI in ubrat.Keys)
                    {
                        exportRadky[newI].Radek[cs].Cas = ubrat[newI];
                    }
                }
            }

            //export do souboru
            //pro excel
            File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx", Properties.Resources.mytime_template);

            Excel.XLWorkbook export = new Excel.XLWorkbook(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\tmp_export.xlsx");
            Excel.IXLWorksheet exportSheet = export.Worksheet(1);

            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            int row = 2;
            foreach (ExportRow s in exportRadky)
            {
                if (s.Terp != null)
                {
                    //project
                    exportSheet.Cell(row, 1).Value = s.Terp;
                    //project name
                    exportSheet.Cell(row, 2).Value = "najdiSiSam";
                    //task
                    exportSheet.Cell(row, 3).SetValue(s.Task);
                    //task name
                    exportSheet.Cell(row, 4).Value = "TyVisCo";
                    //type
                    exportSheet.Cell(row, 5).Value = s.GetTyp();
                    //pondělí (čas, comment)
                    exportSheet.Cell(row, 6).Value = s.Radek["Pondělí"].Cas.ToString() == "0" ? "" : s.Radek["Pondělí"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 7).Value = s.Radek["Pondělí"].Koment.Replace("\t", " ").Replace("\"", "");
                    //úterý
                    exportSheet.Cell(row, 10).Value = s.Radek["Úterý"].Cas.ToString() == "0" ? "" : s.Radek["Úterý"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 11).Value = s.Radek["Úterý"].Koment.Replace("\t", " ").Replace("\"", "");
                    //středa
                    exportSheet.Cell(row, 14).Value = s.Radek["Středa"].Cas.ToString() == "0" ? "" : s.Radek["Středa"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 15).Value = s.Radek["Středa"].Koment.Replace("\t", " ").Replace("\"", "");
                    //čtvrtek
                    exportSheet.Cell(row, 18).Value = s.Radek["Čtvrtek"].Cas.ToString() == "0" ? "" : s.Radek["Čtvrtek"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 19).Value = s.Radek["Čtvrtek"].Koment.Replace("\t", " ").Replace("\"", "");
                    //pátek
                    exportSheet.Cell(row, 22).Value = s.Radek["Pátek"].Cas.ToString() == "0" ? "" : s.Radek["Pátek"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 23).Value = s.Radek["Pátek"].Koment.Replace("\t", " ").Replace("\"", "");
                    //sobota
                    exportSheet.Cell(row, 26).Value = s.Radek["Sobota"].Cas.ToString() == "0" ? "" : s.Radek["Sobota"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 27).Value = s.Radek["Sobota"].Koment.Replace("\t", " ").Replace("\"", "");
                    //neděle
                    exportSheet.Cell(row, 30).Value = s.Radek["Neděle"].Cas.ToString() == "0" ? "" : s.Radek["Neděle"].Cas.ToString(nfi);
                    exportSheet.Cell(row, 31).Value = s.Radek["Neděle"].Koment.Replace("\t", " ").Replace("\"", "");

                    row++;
                }
            }

            export.Save();
            export.Dispose();

            form.infoBox.Text = "";

            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel|*.xlsx";
            saveFileDialog1.FileName = "MyTime Info.xlsx";
            saveFileDialog1.ShowDialog();
        }
    }

    class ExportDen
    {
        public ExportDen()
        {
            Koment = "";
            Cas = 0;
        }
        public string Koment { get; set; }
        public decimal Cas { get; set; }
    }

    class ExportRow
    {
        public ExportRow()
        {
            Radek = new Dictionary<string, ExportDen> { { "Pondělí", new ExportDen() }, { "Úterý", new ExportDen() }, { "Středa", new ExportDen() }
            , { "Čtvrtek", new ExportDen() }, { "Pátek", new ExportDen() }, { "Sobota", new ExportDen() }, { "Neděle", new ExportDen() }};
        }
        public Dictionary<string, ExportDen> Radek { get; set; }
        public string Terp { get; set; }
        public string Task { get; set; }
        public ExportTyp Typ { get; set; }
        public string GetTyp()
        {
            switch(Typ)
            {
                case ExportTyp.Normal: return Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Normal").Value;
                case ExportTyp.Compens: return Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Nahradni").Value;
                case ExportTyp.Holiday: return Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Svatek").Value;
                case ExportTyp.Prescas: return Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Prescas").Value;
                default: return Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Normal").Value;
            }
        }
    }
    enum ExportTyp
    {
        Prescas,
        Normal,
        Holiday,
        Compens
    }
}
