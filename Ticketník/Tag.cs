using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník
{
    class Tag
    {
        public Tag(DateTime datum, long IDlong)
        {
            Datum = datum;
            this.IDlong = IDlong;
        }
        public Tag(DateTime datum, string popis, UpozorneniCls.Typ typ)
        {
            Datum = datum;
            Popis = popis;
            Typ = typ;
        }

        public Tag(string filename, bool installed)
        {
            Soubor = filename;
            Instalován = installed;
        }

        internal DateTime Datum { get; set; }
        internal long IDlong { get; set; }

        internal string Popis { get; set; }

        internal string Soubor { get; set; }

        internal bool Instalován { get; set; }

        internal UpozorneniCls.Typ Typ { get; set; }

        internal bool Compare(Tag tagToCompare)
        {
            if (Popis == null)
            {
                if (this.Datum.Day == tagToCompare.Datum.Day && this.Datum.Month == tagToCompare.Datum.Month && this.Datum.Year == tagToCompare.Datum.Year &&
                    this.IDlong == tagToCompare.IDlong)
                    return true;
            }
            else
            {
                if (this.Datum.Day == tagToCompare.Datum.Day && this.Datum.Month == tagToCompare.Datum.Month && this.Datum.Year == tagToCompare.Datum.Year &&
                    this.Datum.Hour == tagToCompare.Datum.Hour && this.Datum.Minute == tagToCompare.Datum.Minute &&
                    this.Popis == tagToCompare.Popis && this.Typ == tagToCompare.Typ)
                    return true;
            }
            return false;
        }
    }
}
