using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
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

        Label fillColl = new Label()
        {
            AutoSize = false,
            Location = new Point(0, 0),
            Width = 0
        } ;

        public ListView() : base()
        {
            OwnerDraw = true;
            //pro nc space, protože WM_cnpaint strašně bliká a zatěžuje cpu
            fillColl.BackColor = HeaderBackColor;
            Controls.Add(fillColl) ;
            DoubleBuffered = true;
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
                fillColl.Location = new Point(HeaderWidth+1, 0);
                fillColl.Width = Width - HeaderWidth - (VScrollBarVisible ? 17 : 0);
                fillColl.BackColor = HeaderBackColor;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Messages.OnPaint)
            {
                GetVisibleScrollbars(Handle);
                if(HeaderWidth == Width - (VScrollBarVisible ? 17 : 0) && HScrollBarVisible)
                {
                    SendMessage(this.Handle, Messages.LVM_SCROLL, 0, 0);
                    Refresh();
                }
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);

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
