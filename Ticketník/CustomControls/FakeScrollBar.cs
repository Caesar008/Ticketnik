using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    internal class FakeScrollBar : System.Windows.Forms.Control
    {
        public FakeScrollBar(SizeModes sizeMode, ScrollBarAllignment scrollBarAllignment, System.Windows.Forms.Control parent) : base()
        {
            Parent = parent;
            Allignment = scrollBarAllignment;
            SizeMode = sizeMode;
            if (Allignment == ScrollBarAllignment.Vertical)
            {
                Width = 17;
                Height = Parent.Height;
            }
            else if (Allignment == ScrollBarAllignment.Horizontal)
            {
                Width = Parent.Width;
                Height = 17;
            }
        }

        public enum ScrollBarAllignment
        {
            Vertical,
            Horizontal
        }

        public enum SizeModes
        {
            Automatic,
            Custom
        }

        public ScrollBarAllignment Allignment { get; private set; }
        public SizeModes SizeMode { get; private set; }

        private System.Drawing.Size dragSize = new System.Drawing.Size(0, 0);
        public System.Drawing.Size DragSize
        {
            get
            {
                return dragSize;
            }
            set
            {
                int x = value.Width;
                int y = value.Height;
                if (value.Width > Width)
                    x = Width;
                if (value.Height > Height)
                    y = Height;
                dragSize = new System.Drawing.Size(x, y);
            }
        }
        public int ScrollPosition { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Visible)
                return;
            //base.OnPaint(e);
            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
            {
                using (SolidBrush b = new SolidBrush(BackColor))
                {
                    bg.Graphics.FillRectangle(b, e.ClipRectangle);
                }
            }
        }

    }
}
