using System;
using System.Collections.Generic;
using System.IO;
using fNbt;

namespace Ticketník
{
    class UpozorneniCls : IComparable
    {
        public UpozorneniCls(DateTime datum, string popis, Typ typ)
        {
            Datum = datum;
            Popis = popis;
            TypUpozorneni = typ;

        }
        public enum Typ
        {
            RDP,
            Upozorneni
        }
        internal DateTime Datum { get; set; }
        internal string Popis { get; set; }
        internal Typ TypUpozorneni { get; set; }

        internal static void Add(UpozorneniCls upozorneni)
        {
            NbtFile file = new NbtFile();
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni"))
            {
                file.LoadFromFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni");
            }
            else
            {
                file.RootTag = new NbtCompound("Upozorneni");
                file.RootTag.Add(new NbtList("Upozorneni", NbtTagType.Compound));
            }
            NbtCompound newUpo = new NbtCompound();
            short typUpo = 0;
            if(upozorneni.TypUpozorneni == Typ.RDP)
                typUpo = 1;
            else if (upozorneni.TypUpozorneni == Typ.Upozorneni)
                typUpo = 0;
            newUpo.Add(new NbtShort("Typ", typUpo));
            newUpo.Add(new NbtLong("Datum", upozorneni.Datum.Ticks));
            newUpo.Add(new NbtString("Popis", upozorneni.Popis));

            file.RootTag.Get<NbtList>("Upozorneni").Add(newUpo);
            file.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni", NbtCompression.GZip);
        }

        internal static void Remove(UpozorneniCls upozorneni)
        {
            NbtFile file;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni"))
            {
                file = new NbtFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni");
                List<UpozorneniCls> list = new List<UpozorneniCls>();
                int index = 0;
                foreach (NbtCompound c in file.RootTag.Get<NbtList>("Upozorneni"))
                {
                    Typ t;
                    switch (c["Typ"].ShortValue)
                    {
                        case 1:
                            t = Typ.RDP;
                            break;
                        case 0:
                            t = Typ.Upozorneni;
                            break;
                        default:
                            t = Typ.Upozorneni;
                            break;
                    }

                    if (upozorneni.TypUpozorneni == t && upozorneni.Popis == c["Popis"].StringValue && upozorneni.Datum == new DateTime(c["Datum"].LongValue))
                    {
                        break;
                    }
                    index++;
                }
                if (index < file.RootTag.Get<NbtList>("Upozorneni").Count)
                {
                    file.RootTag.Get<NbtList>("Upozorneni").RemoveAt(index);
                    file.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni", NbtCompression.GZip);
                }
            }
        }

        internal static void Upravit(UpozorneniCls puvodni, UpozorneniCls nove)
        {
            NbtFile file;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni"))
            {
                file = new NbtFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni");
                List<UpozorneniCls> list = new List<UpozorneniCls>();
                int index = 0;
                foreach (NbtCompound c in file.RootTag.Get<NbtList>("Upozorneni"))
                {
                    Typ t;
                    switch (c["Typ"].ShortValue)
                    {
                        case 1:
                            t = Typ.RDP;
                            break;
                        case 0:
                            t = Typ.Upozorneni;
                            break;
                        default:
                            t = Typ.Upozorneni;
                            break;
                    }

                    if (puvodni.TypUpozorneni == t && puvodni.Popis == c["Popis"].StringValue && puvodni.Datum == new DateTime(c["Datum"].LongValue))
                    {
                        break;
                    }
                    index++;
                }
                if (index < file.RootTag.Get<NbtList>("Upozorneni").Count)
                {
                    ((NbtCompound)file.RootTag.Get<NbtList>("Upozorneni")[index]).Get<NbtString>("Popis").Value = nove.Popis;
                    ((NbtCompound)file.RootTag.Get<NbtList>("Upozorneni")[index]).Get<NbtLong>("Datum").Value = nove.Datum.Ticks;

                    switch (nove.TypUpozorneni)
                    {
                        case Typ.RDP:
                            ((NbtCompound)file.RootTag.Get<NbtList>("Upozorneni")[index]).Get<NbtShort>("Typ").Value = 1;
                            break;
                        case Typ.Upozorneni:
                            ((NbtCompound)file.RootTag.Get<NbtList>("Upozorneni")[index]).Get<NbtShort>("Typ").Value = 0;
                            break;
                        default:
                            ((NbtCompound)file.RootTag.Get<NbtList>("Upozorneni")[index]).Get<NbtShort>("Typ").Value = 0;
                            break;
                    }

                    file.SaveToFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni", NbtCompression.GZip);
                }
            }
        }

        internal static List<UpozorneniCls> UpozorneniList
        {
            get
            {
                NbtFile file;
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni"))
                {
                    file = new NbtFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Ticketnik\\upozorneni");
                    List<UpozorneniCls> list = new List<UpozorneniCls>();
                    foreach (NbtCompound c in file.RootTag.Get<NbtList>("Upozorneni"))
                    {
                        Typ t;
                        switch (c["Typ"].ShortValue)
                        {
                            case 1:
                                t = Typ.RDP;
                                break;
                            case 0:
                                t = Typ.Upozorneni;
                                break;
                            default:
                                t = Typ.Upozorneni;
                                break;
                        }
                        list.Add(new UpozorneniCls(new DateTime(c["Datum"].LongValue), c["Popis"].StringValue, t));
                        list.Sort();
                    }
                    return list;
                }
                else
                {
                    return new List<UpozorneniCls>();
                }
            }
        }

        public int CompareTo(object u)
        {
            return this.Datum.CompareTo(((UpozorneniCls)u).Datum);

        }
    }
}
