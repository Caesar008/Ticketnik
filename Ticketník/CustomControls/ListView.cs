using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
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

        public ListView() : base()
        {
            OwnerDraw = true;
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
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
                e.Graphics.DrawLine(p, e.Bounds.Right, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
                e.Graphics.DrawLine(p, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }
            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, HeaderForeColor);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawItem(e);
        }
    }
}
