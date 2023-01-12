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
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_SYSKEYDOWN = 0x104; 
        private const int DTM_FIRST = 0x1000;
        public const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        public const int DTM_SETMCSTYLE = DTM_FIRST + 11;
        public const int DTM_GETMCSTYLE = DTM_FIRST + 12;
        public const int DTM_SETMCCOLOR = (0x1000 + 6);
        private const int MCM_FIRST = 0x1000;
        public const int MCM_GETCURRENTVIEW = MCM_FIRST + 22;
        public const int MCM_SETCURRENTVIEW = MCM_FIRST + 32;

        public static int OnPaint => WM_PAINT;
        public static int OnFramePaint => WM_NCPAINT;
        public static int OnEraseBackground => WM_ERASEBKGND;
        public static int OnDrawItem => WM_DRAWITEM;
        public static int OnLMouseDown => WM_LBUTTONDOWN;
        public static int OnLmouseUp => WM_LBUTTONUP;
        public static int GetMonthCalendar => DTM_GETMONTHCAL;
        public static int SetCurrentView => MCM_SETCURRENTVIEW;
        public static int SetCalColor => DTM_SETMCCOLOR;
    }
}
