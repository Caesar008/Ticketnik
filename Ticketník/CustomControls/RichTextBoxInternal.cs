using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Ticketník.CustomControls
{
    internal class RichTextBoxInternal : System.Windows.Forms.RichTextBox
    {
        internal int HScrollPosition {  get; set; }
        internal int VScrollPosition { get; set; }
        private bool hScrollVisible = false;
        public bool HScrollBarVisible
        {
            get
            {
                return hScrollVisible;
            }
            private set
            {
                if (hScrollVisible != value)
                {
                    hScrollVisible = value;
                    ((CustomControls.RichTextBox)Parent).HScrollBar.Visible = value;
                    ((CustomControls.RichTextBox)Parent).HScrollBarVisible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        ((CustomControls.RichTextBox)Parent).VScrollBar.BothVisible = ((CustomControls.RichTextBox)Parent).HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        ((CustomControls.RichTextBox)Parent).VScrollBar.BothVisible = ((CustomControls.RichTextBox)Parent).HScrollBar.BothVisible = false;
                    }
                    HScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool vScrollVisible = false;
        public bool VScrollBarVisible
        {
            get
            {
                return vScrollVisible;
            }
            private set
            {
                if (vScrollVisible != value)
                {
                    vScrollVisible = value;
                    ((CustomControls.RichTextBox)Parent).VScrollBar.Visible = value;
                    ((CustomControls.RichTextBox)Parent).VScrollBarVisible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        ((CustomControls.RichTextBox)Parent).VScrollBar.BothVisible = ((CustomControls.RichTextBox)Parent).HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        ((CustomControls.RichTextBox)Parent).VScrollBar.BothVisible = ((CustomControls.RichTextBox)Parent).HScrollBar.BothVisible = false;
                    }
                    VScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [Category("Action")]
        public event EventHandler HScrollBarVisibilityChanged;
        [Category("Action")]
        public event EventHandler VScrollBarVisibilityChanged;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == Messages.OnPaint)
            {
                base.WndProc(ref m);
                GetVisibleScrollbars(Handle);
                HScrollPosition = GetScrollPos(Handle, 0 /*0 - horizontal, 1- vertical*/); //počet pixelů, default 15 krok
                VScrollPosition = GetScrollPos(Handle, 1); // počet itemů scrollnutých
            }
            else if (m.Msg == Messages.OnScrollBarDraw)
            {
                //nekresli původní scrollbary
            }
            else
                base.WndProc(ref m);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO lpsi);

        [StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }
        private void GetVisibleScrollbars(IntPtr handle)
        {
            int wndStyle = GetWindowLong(handle, Messages.GWL_STYLE);
            HScrollBarVisible = (wndStyle & Messages.HorizontalScrollbar) != 0;
            VScrollBarVisible = (wndStyle & Messages.VerticalSrollbar) != 0;
        }
    }
}
