using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník.CustomControls
{
    public static class Messages
    {
        public const int WM_PAINT = 0xF;
        public const int WM_NCPAINT = 0x85;
        public const int WM_ERASEBKGND = 0x0014;

        public static int OnPaint { get { return WM_PAINT; } }
        public static int OnFramePaint {  get { return WM_NCPAINT; } }
        public static int OnEraseBackground { get { return WM_ERASEBKGND; } }
    }
}
