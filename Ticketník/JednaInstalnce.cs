using System.Diagnostics;
using System.Threading;
using System;

namespace Ticketník
{
    class JednaInstance
    {
        static Mutex mutex = new Mutex(false, "Caesaruv Ticketnik");
        public static bool Bezi(TimeSpan cekaciDoba, bool update = false)
        {
            if (!update && !mutex.WaitOne(cekaciDoba, false))
            {
                return true;
            }
            return false;
        }

        public static void UvolniProstredek()
        {
            try
            {
                mutex.ReleaseMutex();
            }
            catch { }
        }
    }
}
