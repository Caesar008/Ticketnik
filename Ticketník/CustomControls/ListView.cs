using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ticketník.CustomControls
{
    internal class ListView : System.Windows.Forms.ListView
    {

        private Color headerBackColor = Color.White;
        [DefaultValue(typeof(Color), "White"),
             Category("Appearance")]
        public Color HeaderBackColor
        {
            get { return headerBackColor; }
            set
            {
                if (headerBackColor != value)
                {
                    headerBackColor = value;
                    fillColl.BackColor = value;
                    Invalidate();
                }
            }
        }
        private Color headerForeColor = Color.White;
        [DefaultValue(typeof(Color), "White"),
             Category("Appearance")]
        public Color HeaderForeColor
        {
            get { return headerForeColor; }
            set
            {
                if (headerForeColor != value)
                {
                    headerForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color headerSeparatorColor = Color.White;
        [DefaultValue(typeof(Color), "White"),
            Category("Appearance")]
        public Color HeaderSeparatorColor
        {
            get { return headerSeparatorColor; }
            set
            {
                if (headerSeparatorColor != value)
                {
                    headerSeparatorColor = value;
                    fillColl.BorderColor = value;
                    Invalidate();
                }
            }
        }
        private Color gridLinesColor = Color.White;
        [DefaultValue(typeof(Color), "White"),
            Category("Appearance")]
        public Color GridLinesColor
        {
            get { return gridLinesColor; }
            set
            {
                if (gridLinesColor != value)
                {
                    gridLinesColor = value;
                    Invalidate();
                }
            }
        }
        private bool allinglLastColumnLeft = true;
        [DefaultValue(true),
            Category("Appearance")]
        public bool AllinglLastColumnLeft
        {
            get { return allinglLastColumnLeft; }
            set
            {
                if (allinglLastColumnLeft != value)
                {
                    allinglLastColumnLeft = value;
                    Invalidate();
                }
            }
        }

        private HeaderTrailingSpaceFill headerFill = HeaderTrailingSpaceFill.FillDummyColumn;
        [DefaultValue(HeaderTrailingSpaceFill.FillDummyColumn)]
        public HeaderTrailingSpaceFill HeaderFillMethod
        {
            get { return headerFill; }
            set { headerFill = value; Invalidate(); }
        }

        public enum HeaderTrailingSpaceFill
        {
            FillDummyColumn,
            ExtendLastColumn
        }

        private class HeaderFill : Label
        {
            public HeaderFill():base()
            {
                AutoSize = false;
                Location = new Point(0, 0);
                Width = 0;
                Height = 24;
            }

            private Color borderColor  = Color.White;
            public Color BorderColor
            {
                get { return borderColor; }
                set { borderColor = value; Invalidate(); }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                //base.OnPaint(e);
                using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.ClipRectangle))
                {
                    using (Brush b = new SolidBrush(BackColor))
                    {
                        bg.Graphics.FillRectangle(b, e.ClipRectangle);
                        using (Pen p = new Pen(BorderColor, 1))
                        {
                            bg.Graphics.DrawLine(p, 0, 0, 0, Height - 1);
                            bg.Graphics.DrawLine(p, 0, Height - 1, Width, Height - 1);
                        }
                    }
                    bg.Render();
                }
            }
        }

        HeaderFill fillColl = new HeaderFill();

        private ScrollBar VScrollBar;
        private ScrollBar HScrollBar;

        public ListView(Control parent) : base()
        {
            OwnerDraw = true;
            //pro nc space, protože WM_cnpaint strašně bliká a zatěžuje cpu
            fillColl.BackColor = HeaderBackColor;
            fillColl.BorderColor = HeaderSeparatorColor;
            Controls.Add(fillColl) ;
            DoubleBuffered = true;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            VScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Vertical, this);
            HScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Horizontal, this);
            VScrollBar.BackColor = BackColor;
            HScrollBar.BackColor = BackColor;
            VScrollBar.ForeColor = ForeColor;
            HScrollBar.ForeColor = ForeColor;
            VScrollBar.Visible = VScrollBarVisible;
            HScrollBar.Visible = HScrollBarVisible;
            VScrollBar.Scrolled += VScrollBar_Scrolled;
            HScrollBar.Scrolled += HScrollBar_Scrolled;
            if(VScrollBarVisible && HScrollBarVisible)
            {
                VScrollBar.BothVisible = HScrollBar.BothVisible = true;
            }
            Controls.Add(HScrollBar);
            Controls.Add(VScrollBar);
        }

        private void HScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
        {
            SendMessage(this.Handle, Messages.LVM_SCROLL, e.ScrolledBy * 5, 0);
            Invalidate(new Rectangle(HScrollBar.Location, HScrollBar.Size));
        }

        float posun = 0;

        private void VScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
        {
            if (e.ScrollDirection == ScrollBar.ScrollDirection.DragDown || e.ScrollDirection == ScrollBar.ScrollDirection.DragUp)
            {
                posun += e.ScrolledBy / e.ScrollbarRatio;
            }
            else
            {
                posun = e.ScrolledBy*17;
            }
            if (posun >= 17 || posun <= -17)
            {
                SendMessage(this.Handle, Messages.LVM_SCROLL, 0, (int)posun);
                posun = 0;
            }
            Invalidate(new Rectangle(VScrollBar.Location, VScrollBar.Size));
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
            private set
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
                    VScrollBar.Visible= value; 
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = false;
                    }
                    VScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                    FillHeaderSpace();
                }
            }
        }

        bool canCallSizeChange = true;
        bool canCallColumnChange = true;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (canCallSizeChange)
            {
                canCallColumnChange= false;
                FillHeaderSpace();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (canCallSizeChange)
            {
                FillHeaderSpace();
                canCallColumnChange = true;
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            if (canCallSizeChange)
            {
                FillHeaderSpace();
                canCallColumnChange = true;
            }
        }

        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            if (canCallColumnChange)
            {
                FillHeaderSpace();
                canCallSizeChange = true;
            }
        }
        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            base.OnColumnWidthChanging(e);
            if (canCallColumnChange)
            {
                canCallSizeChange = false;
                FillHeaderSpace(true);
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            base.OnDrawColumnHeader(e);
            using (SolidBrush b = new SolidBrush(HeaderBackColor))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }
            using (Pen p = new Pen(HeaderSeparatorColor, 1))
            {
                string[] headerTags = ((string)e.Header.Tag)?.Split(';');
                string separatorTag = "";
                if (headerTags != null)
                {
                    foreach (string tag in headerTags)
                    {
                        if (tag.StartsWith("Separator:"))
                        {
                            separatorTag = tag;
                        }
                    }
                }
                
                if(!separatorTag.Contains("NoLeft"))
                    e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
                if (!separatorTag.Contains("NoRight"))
                    e.Graphics.DrawLine(p, e.Bounds.Right, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom); 
                if (!separatorTag.Contains("NoBottom"))
                    e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }
            //TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, HeaderForeColor);
            TextFormatFlags tf = TextFormatFlags.Default;
            
            switch (e.Header.TextAlign)
            {
                case HorizontalAlignment.Center: tf = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter; break;
                case HorizontalAlignment.Left: tf = TextFormatFlags.Left | TextFormatFlags.VerticalCenter; break;
                case HorizontalAlignment.Right: tf = TextFormatFlags.Right | TextFormatFlags.VerticalCenter; break;
            }
            if(AllinglLastColumnLeft && e.Header.DisplayIndex == Columns.Count-1)
                tf = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, HeaderForeColor, tf);
        }

        public int HeaderWidth
        {
            get 
            {
                int w = 0;
                foreach (ColumnHeader ch in Columns)
                {
                    w += ch.Width;
                }
                return w;
            }
        }

        private int VisibleItems
        {
            get
            {
                return (Height - (HScrollBarVisible ? 17 : 0) - 23) / 17;
            }
        }

        private void FillHeaderSpace(bool forceDummy = false)
        {
            if (HeaderFillMethod == HeaderTrailingSpaceFill.ExtendLastColumn && !forceDummy)
            {
                fillColl.Width = 0;
                fillColl.Location = new Point(0, 0);
                fillColl.BackColor = HeaderBackColor;
                if (Columns.Count > 0)
                {
                    if (HeaderWidth < Width - (VScrollBarVisible ? 17 : 0) || (HeaderWidth <= Width && VScrollBarVisible && HeaderWidth > Width-17))
                    {
                        foreach (ColumnHeader ch in Columns)
                        {
                            if(ch.DisplayIndex == Columns.Count-1)
                                ch.Width += Width - HeaderWidth - (VScrollBarVisible ? 17 : 0);
                        }
                    }
                }
            }
            else if (HeaderFillMethod == HeaderTrailingSpaceFill.FillDummyColumn || forceDummy)
            {
                fillColl.Location = new Point(HeaderWidth, 0);
                fillColl.Width = Width - HeaderWidth - (VScrollBarVisible ? 17 : 0);
                fillColl.BackColor = HeaderBackColor;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Messages.OnPaint)
            {
                base.WndProc(ref m);
                GetVisibleScrollbars(Handle);
                int hScroll = GetScrollPos(Handle, 0 /*0 - horizontal, 1- vertical*/); //počet pixelů, default 15 krok
                int vScroll = GetScrollPos(Handle, 1); // počet itemů scrollnutých

                if (HeaderWidth == Width - (VScrollBarVisible ? 17 : 0) && HScrollBarVisible)
                {
                    SendMessage(this.Handle, Messages.LVM_SCROLL, 0, 0);
                    Refresh();
                }
                if (GridLines && View == View.Details)
                {
                    VScrollBar.ScrollbarRatio = (float)VisibleItems / (float)Items.Count;
                    VScrollBar.Max = Items.Count - VisibleItems;
                    if(vScroll <= VScrollBar.Max)
                        VScrollBar.ScrollPosition = vScroll;
                    else
                        VScrollBar.ScrollPosition = VScrollBar.Max;

                    HScrollBar.ScrollbarRatio = (float)(Width-(VScrollBarVisible ? 17 : 0)) /(float)HeaderWidth;
                    HScrollBar.Max = HeaderWidth - Width + (VScrollBarVisible ? 17 : 0);
                    if (hScroll <= HScrollBar.Max)
                        HScrollBar.ScrollPosition = hScroll;
                    else
                        HScrollBar.ScrollPosition = HScrollBar.Max;

                    using (Pen p = new Pen(GridLinesColor, 1))
                    {
                        using (Graphics g = CreateGraphics())
                        {
                            for (int y = 23; y < this.Size.Height; y += 17)
                            {
                                g.DrawLine(p, 0, y, this.Size.Width, y);
                            }
                            foreach (ColumnHeader col in Columns)
                            {
                                string[] headerTags = ((string)col.Tag)?.Split(';');
                                string separatorTag = "";
                                if (headerTags != null)
                                {
                                    foreach (string tag in headerTags)
                                    {
                                        if (tag.StartsWith("Separator:"))
                                        {
                                            separatorTag = tag;
                                        }
                                    }
                                }
                                if (!separatorTag.Contains("NoLeft"))
                                    g.DrawLine(p, col.DisplayIndex > 0 ? GetColumnLeft(col) - hScroll : 0, 0, col.DisplayIndex > 0 ? GetColumnLeft(col) - hScroll : 0, Height);
                                if (!separatorTag.Contains("NoRight") && col.DisplayIndex == Columns.Count-1)
                                    g.DrawLine(p, HeaderWidth - hScroll, 0,HeaderWidth - hScroll , Height);
                            }
                        }
                    }
                }
            }
            else if(m.Msg == Messages.OnScrollBarDraw)
            {
                //nekresli původní scrollbary
            }
            else if(m.Msg == Messages.OnVerticalScroll)
            {
                base.WndProc(ref m);
            }
            else
                base.WndProc(ref m);
        }

        private int GetColumnLeft(ColumnHeader column)
        {
            ColumnHeader[] columns = new ColumnHeader[Columns.Count];
            for(int c = 0; c < Columns.Count; c++)
            {
                columns[Columns[c].DisplayIndex] = Columns[c];
            }
            int left = 0;
            for(int i = 0; i< column.DisplayIndex; i++)
            {
                left += columns[i].Width;
            }
            return left;
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
        [DllImport("user32.dll")]
        static extern int SetScrollInfo(IntPtr hwnd, int fnBar, [In] ref SCROLLINFO lpsi, bool fRedraw); 
        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, UIntPtr msg, IntPtr wParam, IntPtr lParam);

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

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault= true;
            base.OnDrawSubItem(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawItem(e);
        }
    }
}
