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

        public ListView() : base()
        {
            OwnerDraw = true;
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
                }
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
            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, HeaderForeColor);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if(m.Msg == Messages.OnPaint)
                GetVisibleScrollbars(Handle);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

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
