using System;

namespace Ticketník
{
    static class Pouzivani
    {
       internal static void Zapis()
        {
            string komp = Environment.MachineName;
            string user = Environment.UserDomainName;
            string datum = DateTime.Now.ToString("d.M.yyyy");
            //načíst nbt soubor s používáním
            //v síti bude přímo, na netu bude přes asi php, nebo tak něco do databáze možná
        }
    }
}
