using System;
using System.Collections.Generic;
using fNbt;

namespace Ticketník
{
    internal class Ticket
    {
        internal enum Stav
        {
            Probiha = 0,
            Ceka_se = 1,
            RDP = 2,
            Ceka_se_na_odpoved = 3,
            Vyreseno = 4
        }

        internal enum TerpTyp
        {
            VelkyNormal,
            StredniNormal,
            MalyNormal,
            VelkyNormalPrescas,
            StredniNormalPrescas,
            MalyNormalPrescas,
            VelkyNormalNahradni,
            StredniNormalNahradni,
            MalyNormalNahradni,
            VelkyHolyday,
            StredniHolyday,
            MalyHolyday,
            VelkyProblem,
            StredniProblem,
            MalyProblem,
            MalyProblemPrescas,
            VelkyProblemPrescas,
            StredniProblemPrescas,
            VelkyProblemNahradni,
            StredniProblemNahradni,
            MalyProblemNahradni,
            VelkyProblemHolyday,
            StredniProblemHolyday,
            MalyProblemHolyday,
            Enkripce,
            EnkripceProblem,
            EnkripceProblemPrescas,
            EnkripceProblemNahradni,
            EnkripceProblemHoliday,
            EnkripcePrescas,
            EnkripceNahradni,
            EnkripceHoliday,
            Mobility,
            MobilityProblem,
            MobilityProblemPrescas,
            MobilityProblemNahradni,
            MobilityProblemHoliday,
            MobilityPrescas,
            MobilityNahradni,
            MobilityHoliday,
            Custom,
            CustomPrescas,
            CustomNahradni,
            CustomHoliday,
            OnlineTerpTask
        }

        internal enum TerpKod
        {
            VelkyNormal = 0,
            StredniNormal = 1,
            MalyNormal = 2,
            VelkyHolyday = 3,
            StredniHolyday = 4,
            MalyHolyday = 5,
            VelkyProblem = 6,
            StredniProblem = 7,
            MalyProblem = 8,
            Enkripce = 9,
            EnkripceProblem = 10,
            EnkripceHoliday = 11,
            Mobility = 12,
            MobilityProblem = 13,
            MobilityHoliday = 14,
            Custom = 15,
            CustomHoliday = 16,
            OnlineTerp = 17
        }

        internal enum TypTicketu
        {
            Normalni = 0,
            PraceOPrazdniny = 1,
            ProblemTicket = 2,
            Prescas = 3,
            NahradniVolno = 4,
            ProblemPrescas = 5,
            ProblemOPrazdniny = 6,
            ProblemNahradniVolno = 7,
            Enkripce = 8,
            EnkripcePrescas = 9,
            EnkripceOPrazdniny = 10,
            EnkripceNahradniVolno = 11,
            EnkripceProblem = 12,
            EnkripceProblemPrescas = 13,
            EnkripceProblemNahradni = 14,
            EnkripceProblemOPrazdniny = 15,
            Mobility = 16,
            MobilityPrescas = 17,
            MobilityOPrazdniny = 18,
            MobilityNahradniVolno = 19,
            MobilityProblem = 20,
            MobilityProblemPrescas = 21,
            MobilityProblemNahradni = 22,
            MobilityProblemOPrazdniny = 23,
            Custom = 24,
            CustomPrescas = 25,
            CustomNahradni = 26,
            CustomOPrazdniny = 27,
            OnlineTyp = 28
        }

        public Ticket(long id, string mesic, DateTime datum, DateTime odCas, DateTime doCas, List<DateTime> pauzyOd, List<DateTime> pauzyDo, Stav stav, string ID, string kontakt, string PC, string popis, string poznamky, TypTicketu typTicketu = TypTicketu.Normalni, string zakaznik = "", string customTerp = "", string customTask = "", string onlineTyp = "")
        {
            Mesic = mesic;
            Datum = datum;
            Od = odCas;
            Do = doCas;
            PauzyOd = pauzyOd;
            PauzyDo = pauzyDo;
            StavT = stav;
            this.ID = ID;
            Kontakt = kontakt;
            this.PC = PC;
            Popis = popis;
            Poznamky = poznamky;
            IDtick = id;
            Zakaznik = zakaznik;
            TypPrace = (byte)typTicketu;
            CustomTask = customTask;
            CustomTerp = customTerp;
            OnlineTyp = onlineTyp;

            if (Od > Do && Do.ToString("H:mm") != "0:00")
            {
                Od = Od.AddHours(-12);
                Do = Do.AddHours(12);
            }

            //udělat seznam velikostí zákazníků
            if(typTicketu == TypTicketu.OnlineTyp)
            {
                Terp = TerpKod.OnlineTerp;
                TerpT = TerpTyp.OnlineTerpTask;
            }
            else if (typTicketu == TypTicketu.Normalni)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyNormal;
                    TerpT = TerpTyp.VelkyNormal;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniNormal;
                    TerpT = TerpTyp.StredniNormal;
                }
                else
                {
                    Terp = TerpKod.MalyNormal;
                    TerpT = TerpTyp.MalyNormal;
                }

            }
            else if (typTicketu == TypTicketu.PraceOPrazdniny)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyHolyday;
                    TerpT = TerpTyp.VelkyHolyday;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniHolyday;
                    TerpT = TerpTyp.StredniHolyday;
                }
                else
                {
                    Terp = TerpKod.MalyHolyday;
                    TerpT = TerpTyp.MalyHolyday;
                }

            }
            else if (typTicketu == TypTicketu.NahradniVolno)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyHolyday;
                    TerpT = TerpTyp.VelkyNormalNahradni;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniHolyday;
                    TerpT = TerpTyp.StredniNormalNahradni;
                }
                else
                {
                    Terp = TerpKod.MalyHolyday;
                    TerpT = TerpTyp.MalyNormalNahradni;
                }

            }
            else if (typTicketu == TypTicketu.ProblemNahradniVolno)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyProblem;
                    TerpT = TerpTyp.VelkyProblemNahradni;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniProblem;
                    TerpT = TerpTyp.StredniProblemNahradni;
                }
                else
                {
                    Terp = TerpKod.MalyProblem;
                    TerpT = TerpTyp.MalyProblemNahradni;
                }

            }
            else if (typTicketu == TypTicketu.Prescas)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyNormal;
                    TerpT = TerpTyp.VelkyNormalPrescas;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniNormal;
                    TerpT = TerpTyp.StredniNormalPrescas;
                }
                else
                {
                    Terp = TerpKod.MalyNormal;
                    TerpT = TerpTyp.MalyNormalPrescas;
                }

            }
            else if (typTicketu == TypTicketu.ProblemOPrazdniny)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyProblem;
                    TerpT = TerpTyp.VelkyProblemHolyday;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniProblem;
                    TerpT = TerpTyp.StredniProblemHolyday;
                }
                else
                {
                    Terp = TerpKod.MalyProblem;
                    TerpT = TerpTyp.MalyProblemHolyday;
                }

            }
            else if (typTicketu == TypTicketu.ProblemPrescas)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyProblem;
                    TerpT = TerpTyp.VelkyProblemPrescas;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniProblem;
                    TerpT = TerpTyp.StredniProblemPrescas;
                }
                else
                {
                    Terp = TerpKod.MalyProblem;
                    TerpT = TerpTyp.MalyProblemPrescas;
                }

            }
            else if (typTicketu == TypTicketu.ProblemTicket)
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                {
                    Terp = TerpKod.VelkyProblem;
                    TerpT = TerpTyp.VelkyProblem;
                }
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                {
                    Terp = TerpKod.StredniProblem;
                    TerpT = TerpTyp.StredniProblem;
                }
                else
                {
                    Terp = TerpKod.MalyProblem;
                    TerpT = TerpTyp.MalyProblem;
                }

            }
            else if (typTicketu == TypTicketu.Enkripce)
            {
                Terp = TerpKod.Enkripce;
                TerpT = TerpTyp.Enkripce;
            }
            else if (typTicketu == TypTicketu.EnkripceNahradniVolno)
            {
                Terp = TerpKod.Enkripce;
                TerpT = TerpTyp.EnkripceNahradni;
            }
            else if (typTicketu == TypTicketu.EnkripceOPrazdniny)
            {
                Terp = TerpKod.EnkripceHoliday;
                TerpT = TerpTyp.EnkripceHoliday;
            }
            else if (typTicketu == TypTicketu.EnkripcePrescas)
            {
                Terp = TerpKod.Enkripce;
                TerpT = TerpTyp.EnkripcePrescas;
            }
            else if (typTicketu == TypTicketu.EnkripceProblem)
            {
                Terp = TerpKod.EnkripceProblem;
                TerpT = TerpTyp.EnkripceProblem;
            }
            else if (typTicketu == TypTicketu.EnkripceProblemNahradni)
            {
                Terp = TerpKod.EnkripceProblem;
                TerpT = TerpTyp.EnkripceProblemNahradni;
            }
            else if (typTicketu == TypTicketu.EnkripceProblemOPrazdniny)
            {
                Terp = TerpKod.EnkripceProblem;
                TerpT = TerpTyp.EnkripceProblemHoliday;
            }
            else if (typTicketu == TypTicketu.EnkripceProblemPrescas)
            {
                Terp = TerpKod.EnkripceProblem;
                TerpT = TerpTyp.EnkripceProblemPrescas;
            }
            else if (typTicketu == TypTicketu.Mobility)
            {
                Terp = TerpKod.Mobility;
                TerpT = TerpTyp.Mobility;
            }
            else if (typTicketu == TypTicketu.MobilityNahradniVolno)
            {
                Terp = TerpKod.Mobility;
                TerpT = TerpTyp.MobilityNahradni;
            }
            else if (typTicketu == TypTicketu.MobilityOPrazdniny)
            {
                Terp = TerpKod.MobilityHoliday;
                TerpT = TerpTyp.MobilityHoliday;
            }
            else if (typTicketu == TypTicketu.MobilityPrescas)
            {
                Terp = TerpKod.Mobility;
                TerpT = TerpTyp.MobilityPrescas;
            }
            else if (typTicketu == TypTicketu.MobilityProblem)
            {
                Terp = TerpKod.MobilityProblem;
                TerpT = TerpTyp.MobilityProblem;
            }
            else if (typTicketu == TypTicketu.MobilityProblemNahradni)
            {
                Terp = TerpKod.MobilityProblem;
                TerpT = TerpTyp.MobilityProblemNahradni;
            }
            else if (typTicketu == TypTicketu.MobilityProblemOPrazdniny)
            {
                Terp = TerpKod.MobilityProblem;
                TerpT = TerpTyp.MobilityProblemHoliday;
            }
            else if (typTicketu == TypTicketu.MobilityProblemPrescas)
            {
                Terp = TerpKod.MobilityProblem;
                TerpT = TerpTyp.MobilityProblemPrescas;
            }
            else if (typTicketu == TypTicketu.Custom)
            {
                Terp = TerpKod.Custom;
                TerpT = TerpTyp.Custom;
            }
            else if (typTicketu == TypTicketu.CustomNahradni)
            {
                Terp = TerpKod.Custom;
                TerpT = TerpTyp.CustomNahradni;
            }
            else if (typTicketu == TypTicketu.CustomOPrazdniny)
            {
                Terp = TerpKod.CustomHoliday;
                TerpT = TerpTyp.CustomHoliday;
            }
            else if (typTicketu == TypTicketu.CustomPrescas)
            {
                Terp = TerpKod.Custom;
                TerpT = TerpTyp.CustomPrescas;
            }
            else
            {
                if (Zakaznici.DejVelikost(zakaznik) == 0)
                    Terp = TerpKod.VelkyProblem;
                else if (Zakaznici.DejVelikost(zakaznik) == 1)
                    Terp = TerpKod.StredniProblem;
                else
                    Terp = TerpKod.MalyProblem;

            }
        }

        internal NbtCompound GetNbtObject()
        {
            NbtCompound ticket = new NbtCompound();

            if (Od > Do && Do.ToString("H:mm") != "0:00")
            {
                Od = Od.AddHours(-12);
                Do = Do.AddHours(12);
            }

            ticket.Add(new NbtByte("Den", (byte)Datum.Day));
            ticket.Add(new NbtByte("Do h", (byte)Do.Hour));
            ticket.Add(new NbtByte("Do m", (byte)Do.Minute));
            ticket.Add(new NbtByte("Od h", (byte)Od.Hour));
            ticket.Add(new NbtByte("Od m", (byte)Od.Minute));
            ticket.Add(new NbtByte("Prace", TypPrace));
            ticket.Add(new NbtByte("Stav", (byte)StavT));
            ticket.Add(new NbtShort("Rok", (short)Datum.Year));
            ticket.Add(new NbtLong("IDlong", IDtick));
            ticket.Add(new NbtString("ID", ID));
            ticket.Add(new NbtString("Kontakt", Kontakt));
            ticket.Add(new NbtString("PC", PC));
            ticket.Add(new NbtString("Popis", Popis));
            ticket.Add(new NbtString("Poznamky", Poznamky));
            ticket.Add(new NbtString("Terp", CustomTerp));
            ticket.Add(new NbtString("Task", CustomTask));
            ticket.Add(new NbtString("OnlineTyp", OnlineTyp));
            List<byte> poh = new List<byte>();
            List<byte> pom = new List<byte>();
            foreach (DateTime b in PauzyOd)
            {
                poh.Add((byte)b.Hour);
                pom.Add((byte)b.Minute);
            }
            List<byte> pdh = new List<byte>();
            List<byte> pdm = new List<byte>();
            foreach (DateTime b in PauzyDo)
            {
                pdh.Add((byte)b.Hour);
                pdm.Add((byte)b.Minute);
            }
            ticket.Add(new NbtByteArray("Pauza do h", pdh.ToArray()));
            ticket.Add(new NbtByteArray("Pauza do m", pdm.ToArray()));
            ticket.Add(new NbtByteArray("Pauza od h", poh.ToArray()));
            ticket.Add(new NbtByteArray("Pauza od m", pom.ToArray()));

            return ticket;
        }

        internal byte TypPrace { get; set; }
        internal string Zakaznik { get; set; }
        internal TerpKod Terp { get; set; }
        internal long IDtick { get; set; }
        internal string Mesic { get; set; }
        internal DateTime Datum { get; set; }
        internal DateTime Od { get; set; }
        internal DateTime Do { get; set; }
        internal List<DateTime> PauzyOd { get; set; }
        internal List<DateTime> PauzyDo { get; set; }
        internal Stav StavT { get; set; }
        internal string ID { get; set; }
        internal string Kontakt { get; set; }
        internal string PC { get; set; }
        internal string Popis { get; set; }
        internal string Poznamky { get; set; }
        internal TerpTyp TerpT { get; set; }
        internal string CustomTerp { get; set; }
        internal string CustomTask { get; set; }
        internal string OnlineTyp { get; set; }

    }

}
