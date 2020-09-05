using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using fNbt;

namespace Ticketník
{
    internal class Zakaznici
    {
        NbtFile zak;
        Form1 form;
        public Zakaznici(Form1 form)
        {
            this.form = form;
            ZakazniciS = new SortedDictionary<string, byte>();
            if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici"))
            {
                Zak = zak = new NbtFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici");
                form.verze = zak.RootTag.Get<NbtInt>("verze").Value;
                foreach(NbtTag c in zak.RootTag)
                {
                    if(c.TagType == NbtTagType.Compound)
                        ZakazniciS.Add(((NbtCompound)c).Get<NbtString>("Jmeno").Value, ((NbtCompound)c).Get<NbtByte>("Velikost").Value);
                }

                if (zak.RootTag.Get<NbtInt>("verze").Value >= 17)
                {
                    if (zak.RootTag.Get<NbtList>("Terpy") != null)
                        Terpy = (NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0];
                    else
                    {
                        form.Aktualizace(true);
                    }
                }
            }
            else
            {
                zak = new NbtFile();
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik");
                zak.RootTag.Add(new NbtInt("verze", 1));
                zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            }
        }
        internal static SortedDictionary<string, byte> ZakazniciS { get; set; }

        internal SortedDictionary<string, byte> DejZakazniky()
        {
            return ZakazniciS;
        }

        internal void PridejZakaznika(string zakaznik, byte velikost, string terp = "")
        {
            if (!ZakazniciS.ContainsKey(zakaznik))
            {
                ZakazniciS.Add(zakaznik, velikost);
                zak.RootTag.Add(new NbtCompound(zakaznik));
                zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtString("Jmeno", zakaznik));
                zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtByte("Velikost", velikost));
                if(velikost == 127)
                    zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtString("Terp", terp));
                zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            }
            if (zak.RootTag.Get<NbtCompound>(zakaznik) == null)
            {
                zak.RootTag.Add(new NbtCompound(zakaznik));
                zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtString("Jmeno", zakaznik));
                zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtByte("Velikost", velikost)); 
                if (velikost == 127)
                    zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtString("Terp", terp));
                zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            
            }
        }

        internal void SmazZakaznika(string zakaznik)
        {
            ZakazniciS.Remove(zakaznik);
            if (form.file.RootTag.Get<NbtInt>("verze").Value < 10100)
                form.file.RootTag.Get<NbtCompound>("Zakaznici").Remove(zakaznik);
            else
                foreach (NbtCompound c in form.file.RootTag.Get<NbtCompound>("Zakaznici").Tags)
                    c.Remove(zakaznik);

            zak.RootTag.Remove(zakaznik);
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            form.uložitToolStripMenuItem_Click(null, null);
            form.LoadFile();
        }

        internal void ZmenZakaznika(string zakaznik, string noveJmeno)
        {
            byte b = ZakazniciS[zakaznik];
            ZakazniciS.Remove(zakaznik);
            ZakazniciS.Add(noveJmeno, b);
            zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtString>("Jmeno").Value = noveJmeno;
            zak.RootTag.Get<NbtCompound>(zakaznik).Name = noveJmeno;
            if (form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(zakaznik) != null)
                form.file.RootTag.Get<NbtCompound>("Zakaznici").Get<NbtCompound>(zakaznik).Name = noveJmeno;
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            form.uložitToolStripMenuItem_Click(null, null);
            form.LoadFile();
        }
        internal void ZmenZakaznika(string zakaznik, byte novaVelikost, string novyTerp = "")
        {
            ZakazniciS[zakaznik] = novaVelikost; 
            zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtByte>("Velikost").Value = novaVelikost;
            if (novaVelikost == 127)
            {
                if(zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtString>("Terp") == null)
                    zak.RootTag.Get<NbtCompound>(zakaznik).Add(new NbtString("Terp", novyTerp));
                else
                    zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtString>("Terp").Value = novyTerp;
            }
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            form.uložitToolStripMenuItem_Click(null, null);
            form.LoadFile();
        }

        internal bool ContainsKey(string key)
        {
            return ZakazniciS.ContainsKey(key);
        }

        internal static byte DejVelikost(string zakaznik)
        {
            if (zakaznik != "")
                return ZakazniciS[zakaznik];
            else return 127;
        }

        private static NbtFile Zak { get; set; }

        internal static NbtCompound Terpy { get; set; }
        internal static string GetTerp(string zakaznik)
        {
            try
            {
                if (Zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtString>("Terp") != null)
                    return Zak.RootTag.Get<NbtCompound>(zakaznik).Get<NbtString>("Terp").Value;
                else
                {
                    switch(Zak.RootTag.Get<NbtCompound>(zakaznik)["Velikost"].ByteValue)
                    {
                        case 0:
                            return Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                        case 1:
                            return Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                        case 2:
                            return Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                        case 3:
                            return Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Encrypce").StringValue;
                        case 4:
                            return Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Mobility").StringValue;
                        default:
                            return "Err";
                    }
                }
            }
            catch
            {
                return "Err";
            }
        }
        internal static string GetTerp(Velikost velikost, Task task, Typ typ)
        {
            string _velikost, _typ, _task;
            _velikost = _task = _typ = string .Empty;

            switch(velikost)
            {
                case Velikost.Maly :
                    _velikost = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Maly").StringValue;
                    break;
                case Velikost.Stredni :
                    _velikost = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Stredni").StringValue;
                    break;
                case Velikost.Velky :
                    _velikost = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Velky").StringValue;
                    break;
                case Velikost.Encrypce :
                    _velikost = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Encrypce").StringValue;
                    break;
                case Velikost.Mobility :
                    _velikost = Zakaznici.Terpy.Get<NbtCompound>("Velikost").Get<NbtInt>("Mobility").StringValue;
                    break;

                default :
                    _velikost = "-";
                    break;
            }

            switch (task)
            {
                case Task.Incident:
                    _task = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Incident").Value;
                    break;
                case Task.Problem:
                    _task = Zakaznici.Terpy.Get<NbtCompound>("Task").Get<NbtString>("Problem").Value;
                    break;
                default:
                    _task = "-";
                    break;
            }

            switch (typ)
            {
                case Typ.Normal:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Normal").Value;
                    break;
                case Typ.Prescas:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Prescas").Value;
                    break;
                case Typ.Svatek:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Svatek").Value;
                    break;
                case Typ.Nahradni:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Nahradni").Value;
                    break;
                default:
                    _typ = "-";
                    break;
            }

            return _velikost + "," + _task + "," + _typ + ",";
        }

        internal static string GetTerp(Ticket ticket, Typ typ)
        {
            string _typ;
            _typ = string.Empty;

            switch (typ)
            {
                case Typ.Normal:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Normal").Value;
                    break;
                case Typ.Prescas:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Prescas").Value;
                    break;
                case Typ.Svatek:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Svatek").Value;
                    break;
                case Typ.Nahradni:
                    _typ = Zakaznici.Terpy.Get<NbtCompound>("Typ").Get<NbtString>("Nahradni").Value;
                    break;
                default:
                    _typ = "-";
                    break;
            }

            return ticket.CustomTerp + "," + ticket.CustomTask + "," + _typ + ",";
        }

        internal void AddTerp(string terpKod, bool load = true)
        {
            if(Zakaznici.Terpy.Get<NbtCompound>("Custom") == null)
            {
                Zakaznici.Terpy.Add(new NbtCompound("Custom"));
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Add(new NbtCompound("Custom"));
            }
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp") == null)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtList("Terp", NbtTagType.String));
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Add(new NbtList("Terp", NbtTagType.String));
            }

            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").ListType != NbtTagType.String)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").ListType = NbtTagType.String;
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Get<NbtList>("Terp").ListType = NbtTagType.String;
            }

            Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").Add(new NbtString(terpKod));
            //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Get<NbtList>("Terp").Add(new NbtString(terpKod));
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            if (load)
            {
                form.uložitToolStripMenuItem_Click(null, null);
                form.LoadFile();
            }
        }

        internal void AddTask(string task, bool load = true)
        {
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom") == null)
            {
                Zakaznici.Terpy.Add(new NbtCompound("Custom"));
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Add(new NbtCompound("Custom"));
            }
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task") == null)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtList("Task", NbtTagType.String));
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Add(new NbtList("Task", NbtTagType.String));
            }

            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").ListType != NbtTagType.String)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").ListType = NbtTagType.String;
                //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Get<NbtList>("Task").ListType = NbtTagType.String;
            }

            Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").Add(new NbtString(task));
            //((NbtCompound)zak.RootTag.Get<NbtList>("Terpy")[0]).Get<NbtCompound>("Custom").Get<NbtList>("Task").Add(new NbtString(task));
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            if (load)
            {
                form.uložitToolStripMenuItem_Click(null, null);
                form.LoadFile();
            }
        }

        internal void Save()
        {
            zak.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\zakaznici", NbtCompression.GZip);
            form.uložitToolStripMenuItem_Click(null, null);
            form.LoadFile();
        }

        internal enum Velikost
        {
            Maly,
            Stredni,
            Velky,
            Encrypce,
            Mobility
        }

        internal enum Task
        {
            Incident,
            Problem
        }

        internal enum Typ
        {
            Normal,
            Nahradni,
            Prescas,
            Svatek
        }
    }
}
