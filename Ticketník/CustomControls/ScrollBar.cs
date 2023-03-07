using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    internal class ScrollBar : System.Windows.Forms.Control
    {
        Rectangle sliderRectForDrag;
        bool dragScroll= false, mouseDown = false;

        public ScrollBar(SizeModes sizeMode, ScrollBarAllignment scrollBarAllignment, System.Windows.Forms.Control parent) : base()
        {
            Parent = parent;
            Allignment = scrollBarAllignment;
            SizeMode = sizeMode;
            if (Allignment == ScrollBarAllignment.Vertical)
            {
                Width = 17;
                Height = Parent.Height;
                Location = new Point(Parent.Width - Width, 0);
            }
            else if (Allignment == ScrollBarAllignment.Horizontal)
            {
                Width = Parent.Width;
                Height = 17;
                Location = new Point(0, Parent.Height - Height);
            }
            Parent.SizeChanged += Parent_SizeChanged;
            
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (Allignment == ScrollBarAllignment.Vertical)
            {
                Width = 17;
                Height = Parent.Height;
                Location = new Point(Parent.Width - Width, 0);
                Invalidate();
            }
            else if (Allignment == ScrollBarAllignment.Horizontal)
            {
                Width = Parent.Width;
                Height = 17;
                Location = new Point(0, Parent.Height - Height);
                Invalidate();
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

        private bool bothVisible = false;
        internal bool BothVisible
        {
            get { return bothVisible; }
            set { bothVisible = value; }
        }

        public ScrollBarAllignment Allignment { get; private set; }
        public SizeModes SizeMode { get; private set; }

        private System.Drawing.Size sliderSize = new System.Drawing.Size(0, 0);
        public System.Drawing.Size SliderSize
        {
            get
            {
                return sliderSize;
            }
            set
            {
                int x = value.Width;
                int y = value.Height;
                if (Allignment == ScrollBarAllignment.Vertical)
                {
                    if (value.Width > Width)
                        x = Width;
                    if (value.Height > UsableHight - 2)
                        y = UsableHight - 2;
                }
                else if (Allignment == ScrollBarAllignment.Horizontal)
                {
                    if (value.Width > UsableHight - 2)
                        x = UsableHight - 2;
                    if (value.Height > Height)
                        y = Height;
                }
                sliderSize = new System.Drawing.Size(x, y);
                //Invalidate();
            }
        }
        private int scrollPosition = 0;
        public int ScrollPosition 
        { 
            get
            {
                return scrollPosition;
            }
            set
            {
                if (scrollPosition != value)
                {
                    scrollPosition = value;
                    Invalidate();
                }
            }
        }
        private Color separatorColor = Color.WhiteSmoke;
        public Color SeparatorColor 
        { 
            get { return separatorColor; }
            set { if (separatorColor != value) { separatorColor = value; Invalidate(); } }
        }

        public int UsableHight
        {
            get
            {
                if(Allignment == ScrollBarAllignment.Vertical)
                    return bothVisible? Size.Height - 37-18 : Size.Height-37;
                else
                    return bothVisible ? Size.Width - 37 - 18 : Size.Width - 37;
            }
        }

        private int max = 1;
        public int Max { 
            get { return max; } 
            set 
            { 
                if (value > 0 && max != value) 
                    max = value; 
                Invalidate(); 
            } 
        }

        private float ratio = 1;
        public float ScrollbarRatio
        {
            get { return ratio; }
            set
            {
                if (ratio != value)
                {
                    ratio = value;
                }
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(sliderRectForDrag.Contains(e.Location))
            {
                if (!dragScroll)
                {
                    dragScroll = true;
                }
            }
            else if(Allignment == ScrollBarAllignment.Vertical && (new Rectangle(0, 0, Width, 17).Contains(e.Location)))
            {
                //vertical up
                if(!mouseDown)
                {
                    mouseDown = true;
                    //scroll o 1
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(dragScroll)
            {

            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragScroll)
            {
                dragScroll = false;
            }
            if(mouseDown)
            {
                mouseDown = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Visible)
                return;
            //base.OnPaint(e);

            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
            {
                using (SolidBrush b = new SolidBrush(BackColor))
                {
                    bg.Graphics.FillRectangle(b, new Rectangle(0, 0, Width, Height));
                }
                using (Pen p = new Pen(SeparatorColor, 1))
                {
                    if (Allignment == ScrollBarAllignment.Vertical)
                    {
                        SliderSize = new Size(6, (int)((UsableHight) * ratio));
                        if (SliderSize.Height < 6)
                            SliderSize = new Size(6, 6);
                        int max = UsableHight - SliderSize.Height;
                        float step = (float)max / Max;
                        int scrollPositionInner = (int)Math.Round((ScrollPosition * step), MidpointRounding.AwayFromZero);

                        Rectangle slider = new Rectangle((Width / 2) - (sliderSize.Width / 2), scrollPositionInner + 18, sliderSize.Width, sliderSize.Height);
                        sliderRectForDrag = new Rectangle(1, scrollPositionInner + 18, Width - 2, sliderSize.Height);

                        bg.Graphics.DrawLine(p, 0, 0, 0, Height);
                        bg.Graphics.DrawLine(p, 0, 16, Width, 16);
                        bg.Graphics.DrawLine(p, 0, Height - 17 - (bothVisible ? 17 : 0), Width, Height - 17 - (bothVisible ? 17 : 0));
                        if (bothVisible)
                            bg.Graphics.DrawLine(p, 0, Height - 17, Width, Height - 17);
                        using (SolidBrush b = new SolidBrush(ForeColor))
                        {
                            bg.Graphics.FillPolygon(b, new Point[] { new Point(3, 10), new Point(13, 10), new Point(8, 4) });
                            bg.Graphics.FillPolygon(b, new Point[] { new Point(4, Height - 10 - (bothVisible ? 17 : 0)), new Point(13, Height - 10 - (bothVisible ? 17 : 0)), new Point(8, Height - 5 - (bothVisible ? 17 : 0)) });
                            bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            bg.Graphics.FillPath(b, RoundedRect(slider, 3, 3, 3, 3));
                            bg.Graphics.SmoothingMode = SmoothingMode.None;
                            //tohle je jen pro test
                            //bg.Graphics.DrawRectangle(new Pen(Color.Violet, 1), sliderRectForDrag);
                        }
                    }
                    else if (Allignment == ScrollBarAllignment.Horizontal)
                    {
                        SliderSize = new Size((int)((UsableHight) * ratio), 6);
                        if (SliderSize.Width < 6)
                            SliderSize = new Size(6, 6);
                        int max = UsableHight - SliderSize.Width;
                        float step = (float)max / Max;
                        int scrollPositionInner = (int)Math.Round((ScrollPosition * step), MidpointRounding.AwayFromZero);

                        Rectangle slider = new Rectangle(scrollPositionInner + 18, (Height / 2) - (sliderSize.Height / 2), sliderSize.Width, sliderSize.Height);
                        sliderRectForDrag = new Rectangle(scrollPositionInner + 18, 1, sliderSize.Width, Height - 2);

                        bg.Graphics.DrawLine(p, 0, 0, Width, 0); 
                        bg.Graphics.DrawLine(p, 16, 0, 16, Height);
                        bg.Graphics.DrawLine(p, Width - 17 - (bothVisible ? 17 : 0), 0, Width - 17 - (bothVisible ? 17 : 0), Height); 
                        if (bothVisible)
                            bg.Graphics.DrawLine(p, Width - 17, 0, Width - 17, Height);
                        using (SolidBrush b = new SolidBrush(ForeColor))
                        {
                            bg.Graphics.FillPolygon(b, new Point[] { new Point(5, 8), new Point(10, 3), new Point(10, 13) });
                            bg.Graphics.FillPolygon(b, new Point[] { new Point(Width - 10 - (bothVisible ? 17 : 0), 3), new Point(Width - 5 - (bothVisible ? 17 : 0), 8), new Point(Width - 10 - (bothVisible ? 17 : 0), 13) });
                            bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            bg.Graphics.FillPath(b, RoundedRect(slider, 3, 3, 3, 3));
                            bg.Graphics.SmoothingMode = SmoothingMode.None;
                            //tohle je jen pro test
                            //bg.Graphics.DrawRectangle(new Pen(Color.Violet, 1), sliderRectForDrag);
                        }
                    }
                }
                bg.Render();
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius1, int radius2, int radius3, int radius4)
        {
            int diameter1 = radius1 * 2;
            int diameter2 = radius2 * 2;
            int diameter3 = radius3 * 2;
            int diameter4 = radius4 * 2;

            Rectangle arc1 = new Rectangle(bounds.Location, new Size(diameter1, diameter1));
            Rectangle arc2 = new Rectangle(bounds.Location, new Size(diameter2, diameter2));
            Rectangle arc3 = new Rectangle(bounds.Location, new Size(diameter3, diameter3));
            Rectangle arc4 = new Rectangle(bounds.Location, new Size(diameter4, diameter4));
            GraphicsPath path = new GraphicsPath();

            // top left arc  
            if (radius1 == 0)
            {
                path.AddLine(arc1.Location, arc1.Location);
            }
            else
            {
                path.AddArc(arc1, 180, 90);
            }

            // top right arc  
            arc2.X = bounds.Right - diameter2;
            if (radius2 == 0)
            {
                path.AddLine(arc2.Location, arc2.Location);
            }
            else
            {
                path.AddArc(arc2, 270, 90);
            }

            // bottom right arc  

            arc3.X = bounds.Right - diameter3;
            arc3.Y = bounds.Bottom - diameter3;
            if (radius3 == 0)
            {
                path.AddLine(arc3.Location, arc3.Location);
            }
            else
            {
                path.AddArc(arc3, 0, 90);
            }

            // bottom left arc 
            arc4.X = bounds.Right - diameter4;
            arc4.Y = bounds.Bottom - diameter4;
            arc4.X = bounds.Left;
            if (radius4 == 0)
            {
                path.AddLine(arc4.Location, arc4.Location);
            }
            else
            {
                path.AddArc(arc4, 90, 90);
            }

            path.CloseFigure();
            return path;
        }

    }
}
