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
        public const int WM_DRAWITEM = 0x002B;

        public static int OnPaint => WM_PAINT;
        public static int OnFramePaint => WM_NCPAINT;
        public static int OnEraseBackground => WM_ERASEBKGND;
        public static int OnDrawItem => WM_DRAWITEM;
    }
}
