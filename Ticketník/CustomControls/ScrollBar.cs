using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Rectangle sliderRegionRectUp;
        Rectangle sliderRegionRectDown;
        bool dragScroll= false, mouseDown = false;
        int mouseStartDrag = -1;
        int posun = 0;

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
            mouseDownTimer = new Timer()
            {
                Enabled = false,
                Interval = 75
            };
            mouseDownTimer.Tick += MouseDownTimer_Tick;
            dragTimer = new Timer()
            {
                Enabled = false,
                Interval = 10
            };
            dragTimer.Tick += DragTimer_Tick;
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
                if (scrollPosition > UsableHight - SliderSize.Height)
                    return UsableHight - SliderSize.Height;
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

        private double ratio = 1;
        public double ScrollbarRatio
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        public sealed class ScrollEventArgs : EventArgs
        {
            internal ScrollEventArgs(ScrollBarAllignment scrollBarAllignment, int scrolledBy, ScrollDirection scrollDirection, double scrollbarRatio) 
            { 
                ScrollBarAllignment = scrollBarAllignment;
                ScrolledBy = scrolledBy;
                ScrollDirection = scrollDirection;
                ScrollbarRatio = scrollbarRatio;
            }
            public ScrollBarAllignment ScrollBarAllignment { get; private set; }
            public int ScrolledBy { get; private set; }
            public ScrollDirection ScrollDirection { get; private set; }
            public double ScrollbarRatio { get; private set; }

        }

        public event EventHandler<ScrollEventArgs> Scrolled;

        Timer mouseDownTimer;
        byte firstScroll = 0;
        private void MouseDownTimer_Tick(object sender, EventArgs e)
        {
            if(firstScroll == 5)
            {
                if(direction == ScrollDirection.Up)
                {
                    if (scrollPosition > 0)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -(int)scrollStep, ScrollDirection.Up, ScrollbarRatio));
                }
                else if (direction == ScrollDirection.Down)
                {
                    if (scrollPosition < UsableHight - SliderSize.Height)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, (int)scrollStep, ScrollDirection.Down, ScrollbarRatio));
                }
                else if (direction == ScrollDirection.Left)
                {
                    if (scrollPosition > 0)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -(int)scrollStep, ScrollDirection.Left, ScrollbarRatio));
                }
                else if (direction == ScrollDirection.Right)
                {
                    if (scrollPosition < Max)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, (int)scrollStep, ScrollDirection.Right, ScrollbarRatio));
                }
            }
            else
            {
                firstScroll += 1;
            }
        }

        Timer dragTimer;

        private void DragTimer_Tick(object sender, EventArgs e)
        {
            if (dragScroll)
            {
                if (Allignment == ScrollBarAllignment.Vertical)
                {
                    int currentMousePosY = MousePosition.Y;
                    posun = mouseStartDrag - currentMousePosY;

                    mouseStartDrag = currentMousePosY;

                    if (posun == 0)
                    {
                        direction = ScrollDirection.No;
                    }
                    else if (posun > 0)
                    {
                        if (scrollPosition == UsableHight - SliderSize.Height && PointToClient(MousePosition).Y > UsableHight)
                        {
                            direction = ScrollDirection.No;
                        }
                        else
                        {
                            direction = ScrollDirection.DragUp;
                            if (scrollPosition - posun > 0)
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -posun, ScrollDirection.DragUp, ScrollbarRatio));
                            }
                            else
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -scrollPosition - 1, ScrollDirection.DragUp, ScrollbarRatio));
                            }
                        }
                    }
                    else if (posun < 0)
                    {
                        if (scrollPosition == 0 && PointToClient(MousePosition).Y < 17)
                        {
                            direction = ScrollDirection.No;
                        }
                        else
                        {
                            direction = ScrollDirection.DragDown;
                            if (scrollPosition - posun <= UsableHight - SliderSize.Height)
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -posun, ScrollDirection.DragDown, ScrollbarRatio));
                            }
                            else
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, UsableHight - SliderSize.Height - scrollPosition + 1, ScrollDirection.DragDown, ScrollbarRatio));
                            }
                        }
                    }
                }
                else if (Allignment == ScrollBarAllignment.Horizontal)
                {
                    int currentMousePosX = MousePosition.X;
                    posun = mouseStartDrag - currentMousePosX;

                    mouseStartDrag = currentMousePosX;

                    if (posun == 0)
                    {
                        direction = ScrollDirection.No;
                    }
                    else if (posun > 0)
                    {
                        if (scrollPosition == UsableHight - SliderSize.Height && PointToClient(MousePosition).X > UsableHight)
                        {
                            direction = ScrollDirection.No;
                        }
                        else
                        {
                            direction = ScrollDirection.DragUp;
                            if (scrollPosition - posun > 0)
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -posun, ScrollDirection.DragLeft, ScrollbarRatio));
                            }
                            else
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -scrollPosition - 1, ScrollDirection.DragLeft, ScrollbarRatio));
                            }
                        }
                    }
                    else if (posun < 0)
                    {
                        if (scrollPosition == 0 && PointToClient(MousePosition).X < 17)
                        {
                            direction = ScrollDirection.No;
                        }
                        else
                        {
                            direction = ScrollDirection.DragDown;
                            if (scrollPosition - posun <= UsableHight - SliderSize.Height)
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -posun, ScrollDirection.DragRight, ScrollbarRatio));
                            }
                            else
                            {
                                Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, UsableHight - SliderSize.Height - scrollPosition + 1, ScrollDirection.DragRight, ScrollbarRatio));
                            }
                        }
                    }
                }
            }
        }


        public enum ScrollDirection
        {
            Up, Down, Left, Right, No, DragUp, DragDown, DragLeft, DragRight
        }
        private ScrollDirection direction = ScrollDirection.No;
        public enum ScrollStep
        {
            Small = 1,
            Medium = 3
        }
        private ScrollStep scrollStep = ScrollStep.Small;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (sliderRectForDrag.Contains(e.Location))
            {
                if (!dragScroll)
                {
                    dragScroll = true;
                    mouseStartDrag = Allignment == ScrollBarAllignment.Vertical ? MousePosition.Y : MousePosition.X;
                    dragTimer.Enabled = true;
                    dragTimer.Start();
                }
            }
            else if (Allignment == ScrollBarAllignment.Vertical && (new Rectangle(0, 0, Width, 17).Contains(e.Location)))
            {
                //vertical up
                if (!mouseDown)
                {
                    direction = ScrollDirection.Up;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o -1
                    if (scrollPosition > 0)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -(int)ScrollStep.Small, ScrollDirection.Up, ScrollbarRatio));

                }
            }
            else if (Allignment == ScrollBarAllignment.Vertical && (new Rectangle(0, Height - 17 - (bothVisible ? 17 : 0), Width, 17).Contains(e.Location)))
            {
                //vertical down
                if (!mouseDown)
                {
                    direction = ScrollDirection.Down;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o 1
                    if (scrollPosition < UsableHight - SliderSize.Height)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, (int)ScrollStep.Small, ScrollDirection.Down, ScrollbarRatio));
                }
            }
            else if (sliderRegionRectUp.Contains(e.Location))
            {
                if(Allignment == ScrollBarAllignment.Vertical)
                {
                    direction= ScrollDirection.Up;
                    scrollStep = ScrollStep.Medium;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    if (scrollPosition >= (int)ScrollStep.Medium)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -(int)ScrollStep.Medium, ScrollDirection.Up, ScrollbarRatio));
                    else if (scrollPosition > 0)
                            Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, -scrollPosition, ScrollDirection.Up, ScrollbarRatio));
                }
                else if (Allignment == ScrollBarAllignment.Horizontal)
                {
                    direction = ScrollDirection.Left;
                    scrollStep = ScrollStep.Medium;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    if (scrollPosition >= (int)ScrollStep.Medium)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -(int)ScrollStep.Medium, ScrollDirection.Left, ScrollbarRatio));
                    else if (scrollPosition > 0)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -scrollPosition, ScrollDirection.Left, ScrollbarRatio));
                }
            }
            else if (sliderRegionRectDown.Contains(e.Location))
            {
                if(Allignment ==  ScrollBarAllignment.Vertical)
                {
                    direction = ScrollDirection.Down;
                    scrollStep = ScrollStep.Medium;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    if (scrollPosition + (int)ScrollStep.Medium < UsableHight - SliderSize.Height)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, (int)ScrollStep.Medium, ScrollDirection.Down, ScrollbarRatio));
                    else if (scrollPosition < UsableHight - SliderSize.Height)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Vertical, UsableHight - SliderSize.Height, ScrollDirection.Down, ScrollbarRatio));
                }
                else if (Allignment == ScrollBarAllignment.Horizontal)
                {
                    direction = ScrollDirection.Right;
                    scrollStep = ScrollStep.Medium;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    if (scrollPosition + (int)ScrollStep.Medium < Max)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, (int)ScrollStep.Medium, ScrollDirection.Right, ScrollbarRatio));
                    else if (scrollPosition < Max)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, UsableHight - SliderSize.Height, ScrollDirection.Right, ScrollbarRatio));
                }
            }
            else if (Allignment == ScrollBarAllignment.Horizontal && (new Rectangle(0, 0, 17, Height).Contains(e.Location)))
            {
                if (!mouseDown)
                {
                    direction = ScrollDirection.Left;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o -1
                    if (scrollPosition > 0)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, -(int)ScrollStep.Small, ScrollDirection.Left, ScrollbarRatio));

                }
            }
            else if (Allignment == ScrollBarAllignment.Horizontal && (new Rectangle(Width - 17 - (bothVisible ? 17 : 0), 0, 17, Height).Contains(e.Location)))
            {
                if (!mouseDown)
                {
                    direction = ScrollDirection.Right;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o 1
                    if (scrollPosition < Max)
                        Scrolled?.Invoke(this, new ScrollEventArgs(ScrollBarAllignment.Horizontal, (int)ScrollStep.Small, ScrollDirection.Right, ScrollbarRatio));
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (dragScroll)
            {
                dragScroll = false;
                mouseStartDrag = -1;
                dragTimer.Stop();
                dragTimer.Enabled = false;
            }
            if(mouseDown)
            {
                mouseDown = false;
                mouseDownTimer.Stop();
                mouseDownTimer.Enabled = false;
                direction = ScrollDirection.No;
                firstScroll = 0;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Visible)
                return;
            //base.OnPaint(e);
            Debug.WriteLine(Allignment.ToString() + " " + bothVisible.ToString());
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
                        if(sliderRectForDrag.Contains(PointToClient(MousePosition)))
                        {
                            mouseDown = false;
                            mouseDownTimer.Stop();
                            mouseDownTimer.Enabled = false;
                            direction = ScrollDirection.No;
                            firstScroll = 0;
                        }
                        SliderSize = new Size(6, (int)((UsableHight) * ratio));
                        if (SliderSize.Height < 6)
                            SliderSize = new Size(6, 6);

                        Rectangle slider = new Rectangle((Width / 2) - (sliderSize.Width / 2), ScrollPosition + 18, sliderSize.Width, sliderSize.Height);
                        sliderRectForDrag = new Rectangle(1, ScrollPosition + 18, Width - 2, sliderSize.Height);
                        sliderRegionRectUp = new Rectangle(1, 17, Width -2, ScrollPosition);
                        sliderRegionRectDown = new Rectangle(1, sliderRectForDrag.Bottom +2, Width - 2, Height - sliderRectForDrag.Bottom - 19 - (bothVisible ? 18 : 0));

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
                            /*bg.Graphics.DrawRectangle(new Pen(Color.Violet, 1), sliderRegionRectUp);
                            bg.Graphics.DrawRectangle(new Pen(Color.Violet, 1), sliderRegionRectDown);*/
                        }
                    }
                    else if (Allignment == ScrollBarAllignment.Horizontal)
                    {
                        SliderSize = new Size(UsableHight - Max, 6);   //new Size((int)((UsableHight) * ratio), 6);
                        if (SliderSize.Width < 6)
                            SliderSize = new Size(6, 6);

                        Rectangle slider = new Rectangle(/*scrollPositionInner*/scrollPosition + 18, (Height / 2) - (sliderSize.Height / 2), sliderSize.Width, sliderSize.Height);
                        sliderRectForDrag = new Rectangle(/*scrollPositionInner*/scrollPosition + 18, 1, sliderSize.Width, Height - 2);
                        sliderRegionRectUp = new Rectangle(17, 1, /*scrollPositionInner*/scrollPosition, Height -2);
                        sliderRegionRectDown = new Rectangle(sliderRectForDrag.Right + 2, 1, Width - sliderRectForDrag.Right - 19 - (bothVisible ? 18 : 0), Height -2);

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
