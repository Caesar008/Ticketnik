using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník.CustomControls
{
    internal class RichTextBox : System.Windows.Forms.Control
    {
        internal ScrollBar VScrollBar;
        internal ScrollBar HScrollBar;
        private RichTextBoxInternal rtb;

        public RichTextBox() : base()
        {
            VScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Vertical, this);
            HScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Horizontal, this);
            VScrollBar.BackColor = BackColor;
            HScrollBar.BackColor = BackColor;
            VScrollBar.ForeColor = ForeColor;
            HScrollBar.ForeColor = ForeColor;
            VScrollBar.Visible = VScrollBarVisible;
            HScrollBar.Visible = HScrollBarVisible;
            rtb = new RichTextBoxInternal();
            rtb.Location = new System.Drawing.Point(0, 0);
            rtb.Size = this.Size;
            rtb.Text = Text; 
            rtb.Font = Font;
            rtb.ForeColor = ForeColor;
            rtb.BackColor = BackColor;
            rtb.Parent = this;
            rtb.VScrollBarScrollChanged += Rtb_VScrollBarScrollChanged;
            rtb.HScrollBarScrollChanged += Rtb_HScrollBarScrollChanged;
            rtb.SizeChanged += Rtb_SizeChanged;
            VScrollBar.Scrolled += VScrollBar_Scrolled;
            HScrollBar.Scrolled += HScrollBar_Scrolled;
            if (VScrollBarVisible && HScrollBarVisible)
            {
                VScrollBar.BothVisible = HScrollBar.BothVisible = true;
            }
            Controls.Add(rtb);
            Controls.Add(HScrollBar);
            Controls.Add(VScrollBar);
        }

        int scrollPos = 0;

        private void HScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
        {
            /*if (HScrollBar.ScrollPosition + (e.ScrolledBy*5) < HScrollBar.ScrollMax && e.ScrollDirection == ScrollBar.ScrollDirection.Right)
                rtb.Scroll(HScrollBar.ScrollPosition + (e.ScrolledBy * 5), e.ScrollbarRatio, VScrollBar.ScrollPosition, VScrollBar.ScrollbarRatio);
            else if(e.ScrollDirection == ScrollBar.ScrollDirection.Right)
                rtb.Scroll(HScrollBar.Max, e.ScrollbarRatio, VScrollBar.ScrollPosition, VScrollBar.ScrollbarRatio);
            else if (HScrollBar.ScrollPosition + (e.ScrolledBy * 5) > 0 && e.ScrollDirection == ScrollBar.ScrollDirection.Left)
                rtb.Scroll(HScrollBar.ScrollPosition + (e.ScrolledBy * 5), e.ScrollbarRatio, VScrollBar.ScrollPosition, VScrollBar.ScrollbarRatio);
            else if (e.ScrollDirection == ScrollBar.ScrollDirection.Left)
                rtb.Scroll(0, 1, VScrollBar.ScrollPosition, VScrollBar.ScrollbarRatio);*/
            rtb.Scroll(rtb.InnerScroll.X + 5, e.ScrollbarRatio, VScrollBar.ScrollPosition, VScrollBar.ScrollbarRatio);
            Debug.WriteLine("Scroll to: " + scrollPos);
            Debug.WriteLine("ratio: " + e.ScrollbarRatio);
            Debug.WriteLine("děleno: " + ((double)scrollPos / e.ScrollbarRatio));
            Debug.WriteLine("InnerScroll: " + rtb.InnerScroll.X);
        }

        private void VScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
        {
        }

        private void Rtb_SizeChanged(object sender, EventArgs e)
        {
            HScrollBar.ScrollbarRatio = (double)rtb.Width / (double)(rtb.PreferredSize.Width);
            VScrollBar.ScrollbarRatio = (double)rtb.Height / (double)(rtb.PreferredSize.Height);
            HScrollBar.Max = (int)Math.Round((double)(rtb.PreferredSize.Width - rtb.Width - 17) * HScrollBar.ScrollbarRatio, MidpointRounding.AwayFromZero);
            VScrollBar.Max = (int)Math.Round((double)(rtb.PreferredSize.Height - rtb.Height - 17) * VScrollBar.ScrollbarRatio, MidpointRounding.AwayFromZero);

            double vstep = (double)VScrollBar.ScrollMax / (rtb.PreferredSize.Height - rtb.Height - 17);
            int vscrollPositionInner = (int)Math.Round(((double)rtb.VScrollPosition * vstep), MidpointRounding.AwayFromZero);

            double hstep = (double)HScrollBar.ScrollMax / (rtb.PreferredSize.Width - rtb.Width - 17);
            int hscrollPositionInner = (int)Math.Round(((double)rtb.HScrollPosition * hstep), MidpointRounding.AwayFromZero);

            HScrollBar.ScrollPosition = hscrollPositionInner;
            VScrollBar.ScrollPosition = vscrollPositionInner;

            HScrollBar.Invalidate();
            VScrollBar.Invalidate();
        }

        private void Rtb_HScrollBarScrollChanged(object sender, EventArgs e)
        {
            double step = (double)HScrollBar.ScrollMax / (rtb.PreferredSize.Width - rtb.Width - 17);
            int scrollPositionInner = (int)Math.Round(((double)rtb.HScrollPosition * step), MidpointRounding.AwayFromZero);

            Debug.WriteLine("H Max: " + HScrollBar.ScrollMax);
            Debug.WriteLine("H Scroll: " + scrollPositionInner);

            HScrollBar.ScrollPosition = scrollPositionInner;
            HScrollBar.Invalidate();
        }

        private void Rtb_VScrollBarScrollChanged(object sender, EventArgs e)
        {
            double step = (double)VScrollBar.ScrollMax / (rtb.PreferredSize.Height - rtb.Height-17);
            int scrollPositionInner = (int)Math.Round(((double)rtb.VScrollPosition * step), MidpointRounding.AwayFromZero);

            Debug.WriteLine("V Max: " + VScrollBar.ScrollMax);
            Debug.WriteLine("V Scroll: " + scrollPositionInner);

            VScrollBar.ScrollPosition = scrollPositionInner;
            VScrollBar.Invalidate();
        }

        [Category("Action")]
        public event EventHandler HScrollBarVisibilityChanged;
        [Category("Action")]
        public event EventHandler VScrollBarVisibilityChanged;

        private bool hScrollVisible = false;
        public bool HScrollBarVisible
        {
            get
            {
                return hScrollVisible;
            }
            internal set
            {
                if (hScrollVisible != value)
                {
                    hScrollVisible = value;
                    HScrollBar.Visible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = false;
                    }
                    rtb.Height = hScrollVisible ? this.Height - HScrollBar.Height : this.Height;
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
            internal set
            {
                if (vScrollVisible != value)
                {
                    vScrollVisible = value;
                    VScrollBar.Visible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = false;
                    }
                    rtb.Width = vScrollVisible ? this.Width - VScrollBar.Width : this.Width;
                    VScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return rtb.BorderStyle; }
            set { rtb.BorderStyle = value; }
        }

        public bool ReadOnly
        {
            get { return rtb.ReadOnly; }
            set { rtb.ReadOnly = value; }
        }

        public bool WordWrap
        {
            get { return rtb.WordWrap; }
            set
            {
                rtb.WordWrap = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            rtb.Size = new System.Drawing.Size(this.Size.Width - (VScrollBarVisible ? VScrollBar.Width : 0), this.Size.Height - (HScrollBarVisible ? HScrollBar.Height : 0));
        }

        protected override void OnTextChanged(EventArgs e)
        {
            //base.OnTextChanged(e);
            rtb.Text = Text;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            //base.OnFontChanged(e);
            rtb.Font = Font;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            //base.OnBackColorChanged(e);
            rtb.BackColor = BackColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            //base.OnForeColorChanged(e); 
            rtb.ForeColor = ForeColor;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            HScrollBar.ScrollbarRatio = (double)rtb.Width / (double)(rtb.PreferredSize.Width);
            VScrollBar.ScrollbarRatio = (double)rtb.Height / (double)(rtb.PreferredSize.Height);
            HScrollBar.Max = (int)Math.Round((double)(rtb.PreferredSize.Width - rtb.Width - 17) * HScrollBar.ScrollbarRatio, MidpointRounding.AwayFromZero);
            VScrollBar.Max = (int)Math.Round((double)(rtb.PreferredSize.Height - rtb.Height - 17) * VScrollBar.ScrollbarRatio, MidpointRounding.AwayFromZero);

            double vstep = (double)VScrollBar.ScrollMax / (rtb.PreferredSize.Height - rtb.Height - 17);
            int vscrollPositionInner = (int)Math.Round(((double)rtb.VScrollPosition * vstep), MidpointRounding.AwayFromZero);

            double hstep = (double)HScrollBar.ScrollMax / (rtb.PreferredSize.Width - rtb.Width - 17);
            int hscrollPositionInner = (int)Math.Round(((double)rtb.HScrollPosition * hstep), MidpointRounding.AwayFromZero);

            HScrollBar.ScrollPosition = hscrollPositionInner;
            VScrollBar.ScrollPosition = vscrollPositionInner;
            base.OnPaint(e);
        }
    }

    internal class RichTextBoxInternal : System.Windows.Forms.RichTextBox
    {
        private int hScrollPosition = 0;
        internal int HScrollPosition 
        { 
            get { return hScrollPosition; }
            set
            {
                if(hScrollPosition != value)
                {
                    hScrollPosition = value;
                    HScrollBarScrollChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private int vScrollPosition = 0;
        internal int VScrollPosition 
        {
            get { return vScrollPosition; }
            set
            {
                if (vScrollPosition != value)
                {
                    vScrollPosition = value;
                    VScrollBarScrollChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
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

        public Point InnerScroll { get; private set; }

        public void Scroll(int hScroll, double hRatio, int vScroll, double vRatio)
        {
            Point p;
            //p.X = (int)((double)hScroll / hRatio);
            p.Y = (int)((double)vScroll / vRatio);
            //p.X = (int)((double)89 / hRatio); p.Y = vScroll;
            p.X = hScroll;
            SendMessage(this.Handle, Messages.EM_SETSCROLLPOS, 0, ref p);
        }

        [Category("Action")]
        public event EventHandler HScrollBarVisibilityChanged;
        [Category("Action")]
        public event EventHandler VScrollBarVisibilityChanged; 
        [Category("Action")]
        public event EventHandler HScrollBarScrollChanged;
        [Category("Action")]
        public event EventHandler VScrollBarScrollChanged;

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == Messages.OnPaint)
            {
                base.WndProc(ref m);
                GetVisibleScrollbars(Handle);
                HScrollPosition = GetScrollPos(Handle, 0 /*0 - horizontal, 1- vertical*/); //počet pixelů, default 15 krok
                VScrollPosition = GetScrollPos(Handle, 1); // počet itemů scrollnutých

                IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Point)));
                Marshal.StructureToPtr(new Point(), ptr, false);
                SendMessage(this.Handle, Messages.EM_GETSCROLLPOS, IntPtr.Zero, ptr);
                InnerScroll = (Point)Marshal.PtrToStructure(ptr, typeof(Point));
                Marshal.FreeHGlobal(ptr);
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
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, ref Point lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);
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

        internal struct Point
        {
            public int X, Y;
        }
        private void GetVisibleScrollbars(IntPtr handle)
        {
            int wndStyle = GetWindowLong(handle, Messages.GWL_STYLE);
            HScrollBarVisible = (wndStyle & Messages.HorizontalScrollbar) != 0;
            VScrollBarVisible = (wndStyle & Messages.VerticalSrollbar) != 0;
        }
    }
}
