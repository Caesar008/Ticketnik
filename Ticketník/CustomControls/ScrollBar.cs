using DocumentFormat.OpenXml.Presentation;
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
        bool dragScroll = false, mouseDown = false;
        int mouseStartDrag = -1;
        int posun = 0;

        public ScrollBar(SizeModes sizeMode, ScrollBarAlignment scrollBarAlignment, System.Windows.Forms.Control parent) : base()
        {
            Parent = parent;
            Alignment = scrollBarAlignment;
            SizeMode = sizeMode;
            if (Alignment == ScrollBarAlignment.Vertical)
            {
                Width = 17;
                Height = Parent.Height;
                Location = new Point(Parent.Width - Width, 0);
            }
            else if (Alignment == ScrollBarAlignment.Horizontal)
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

        private bool canProcessParentSizeChanged = true;

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            if (!canProcessParentSizeChanged)
                return;
            canProcessParentSizeChanged = false;
            if (Alignment == ScrollBarAlignment.Vertical)
            {
                if (Parent.Height < 44)
                    Parent.Height = 44;
                if (Parent.Width < 17)
                    Parent.Width = 17;
                Width = 17;
                Height = Parent.Height;
                Location = new Point(Parent.Width - Width, 0);
                Invalidate();
            }
            else if (Alignment == ScrollBarAlignment.Horizontal)
            {
                if (Parent.Height < 17)
                    Parent.Height = 17;
                if (Parent.Width < 44)
                    Parent.Width = 44;
                Width = Parent.Width;
                Height = 17;
                Location = new Point(0, Parent.Height - Height);
                Invalidate();
            }
            canProcessParentSizeChanged = true;
        }

        public void Relocate()
        {
            if (Alignment == ScrollBarAlignment.Vertical)
            {
                Location = new Point(Parent.Width - Width, 0);
                //Invalidate();
            }
            else if (Alignment == ScrollBarAlignment.Horizontal)
            {
                Location = new Point(0, Parent.Height - Height);
                //Invalidate();
            }
        }

        public enum ScrollBarAlignment
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

        public ScrollBarAlignment Alignment { get; private set; }
        public SizeModes SizeMode { get; private set; }

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
                    int old = scrollPosition;
                    scrollPosition = value;
                    Invalidate();
                }
            }
        }
        public int ScrollPositionInternal
        {
            get
            {
                return scrollPosition;
            }
            set
            {
                if (scrollPosition != value)
                {
                    int old = scrollPosition;
                    scrollPosition = value;
                    ScrollDirection directionToReport = ScrollDirection.No;
                    if (old < value)
                    {
                        if (Alignment == ScrollBarAlignment.Vertical)
                            directionToReport = ScrollDirection.Down;
                        else
                            directionToReport = ScrollDirection.Right;
                    }
                    else if (old > value)
                    {
                        if (Alignment == ScrollBarAlignment.Vertical)
                            directionToReport = ScrollDirection.Up;
                        else
                            directionToReport = ScrollDirection.Left;
                    }
                    Scrolled?.Invoke(this, new ScrollEventArgs(Alignment, old, value, directionToReport));
                    posun = 0;
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

        public bool ScrollToLatest
        {
            get; set;
        }

        private int lastOffset = 1;
        public int LastOffset
        {
            get { return lastOffset; }
            set
            {
                if (value > 0 && lastOffset != value) 
                { 
                    lastOffset = value;
                    Invalidate();
                }
            }
        }

        private int totalItems = 0;
        
        public int TotalItems
        { 
            get 
            { 
                return totalItems;
            }
            set
            {
                if (value >= 0 && totalItems != value)
                    totalItems = value;
                Invalidate();
            }
        }

        private int scrollNasobitel = 1;
        public int ScrollNasobitel
        {
            get { return scrollNasobitel; }
            set
            {
                if(value >0 && scrollNasobitel != value)
                {
                    scrollNasobitel = value;
                }
            }
        }

        private int Max
        {
            get { return ScrollToLatest ? (totalItems - (1 * -lastOffset) < 0 ? 0 : totalItems - (1 * -lastOffset)) : 
                    (totalItems - visibleItems < 0 ? 0 : totalItems - visibleItems); }
        }

        private int visibleItems = 1;
        public int VisibleItems
        {
            get { return visibleItems; }
            set
            {
                if (value > 0 && visibleItems != value)
                    visibleItems = value;
                Invalidate();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        public sealed class ScrollEventArgs : EventArgs
        {
            internal ScrollEventArgs(ScrollBarAlignment scrollBarAlignment, int oldPosition, int newPosition, ScrollDirection scrollDirection)
            {
                ScrollBarAlignment = scrollBarAlignment;
                OldPosition = oldPosition;
                ScrollDirection = scrollDirection;
                NewPosition = newPosition;
            }
            public ScrollBarAlignment ScrollBarAlignment { get; private set; }
            public int OldPosition { get; private set; }
            public int NewPosition { get; private set; }
            public ScrollDirection ScrollDirection { get; private set; }

        }

        public event EventHandler<ScrollEventArgs> Scrolled;

        Timer mouseDownTimer;
        byte firstScroll = 0;
        private void MouseDownTimer_Tick(object sender, EventArgs e)
        {
            if (firstScroll == 5)
            {
                if (direction == ScrollDirection.Up || direction == ScrollDirection.Left)
                {
                    if (scrollPosition > 0)
                    {
                        if (ScrollPositionInternal - ((int)scrollStep*scrollNasobitel) >= 0)
                            ScrollPositionInternal = scrollPosition - ((int)scrollStep * scrollNasobitel);
                        else
                            ScrollPositionInternal = 0;
                    }
                }
                else if (direction == ScrollDirection.Down || direction == ScrollDirection.Right)
                {
                    if (scrollPosition < Max)
                    {
                        if (ScrollPositionInternal + ((int)scrollStep * scrollNasobitel) <= Max)
                            ScrollPositionInternal = scrollPosition + ((int)scrollStep * scrollNasobitel);
                        else
                            ScrollPositionInternal = Max;
                    }
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
                
                if (Alignment == ScrollBarAlignment.Vertical)
                {
                    int currentMousePosY = MousePosition.Y;
                    posun = mouseStartDrag - currentMousePosY;

                    int maxPos = Height - 19 - SliderSize.Height - (bothVisible ? 17 : 0);
                    int dostupnyProstor = maxPos - 19 ;
                    double itemuNaPixel = Math.Round((double)Max / (double)dostupnyProstor, 4, MidpointRounding.AwayFromZero);
                    int novaPozice = (int)(Math.Round(((ScrollPosition / itemuNaPixel + 18)) + 1, 0, MidpointRounding.ToEven) - posun);
                    int novyScroll = (int)Math.Round((novaPozice - 19) * itemuNaPixel, 0, MidpointRounding.ToEven);
                    if (novyScroll < 0)
                        novyScroll = 0;
                    else if(novyScroll > Max) 
                        novyScroll = Max;

                    ScrollPositionInternal = novyScroll;
                    if(posun==0)
                        mouseStartDrag = currentMousePosY;
                }
                else if (Alignment == ScrollBarAlignment.Horizontal)
                {
                    int currentMousePosX = MousePosition.X;
                    posun = mouseStartDrag - currentMousePosX;

                    //tady udělat výpočet o kolik posunout podle poměru Max a velikosti pro posun
                    int maxPos = Width - 19 - SliderSize.Width - (bothVisible ? 17 : 0);
                    int dostupnyProstor = maxPos - 19;
                    double itemuNaPixel = Math.Round((double)Max / (double)dostupnyProstor, 4, MidpointRounding.AwayFromZero);
                    int novaPozice = (int)(Math.Round(((ScrollPosition / itemuNaPixel + 18)) + 1, 0, MidpointRounding.ToEven) - posun);
                    int novyScroll = (int)Math.Round((novaPozice - 19) * itemuNaPixel, 0, MidpointRounding.ToEven);

                    Debug.WriteLine("Max: " + Max);
                    Debug.WriteLine("Nový: " + novyScroll);

                    if (novyScroll < 0)
                        novyScroll = 0;
                    else if (novyScroll > Max)
                        novyScroll = Max;

                    ScrollPositionInternal = novyScroll;
                    if (posun == 0)
                        mouseStartDrag = currentMousePosX;
                }
            }
        }

        public enum ScrollDirection
        {
            Up, Down, Left, Right, No
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
                    mouseStartDrag = Alignment == ScrollBarAlignment.Vertical ? MousePosition.Y : MousePosition.X;
                    dragTimer.Enabled = true;
                    dragTimer.Start();
                }
            }
            else if (Alignment == ScrollBarAlignment.Vertical && (new Rectangle(0, 0, Width, 17).Contains(e.Location)))
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
                    if (scrollPosition - ((int)ScrollStep.Small * scrollNasobitel) >= 0)
                    {
                        ScrollPositionInternal = scrollPosition - ((int)ScrollStep.Small * scrollNasobitel);
                    }
                    else
                        ScrollPositionInternal = 0;
                }
            }
            else if (Alignment == ScrollBarAlignment.Vertical && (new Rectangle(0, Height - 17 - (bothVisible ? 17 : 0), Width, 17).Contains(e.Location)))
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
                    if (scrollPosition + ((int)ScrollStep.Small * scrollNasobitel) <= Max)
                    {
                        ScrollPositionInternal = scrollPosition + ((int)ScrollStep.Small*scrollNasobitel);
                    }
                    else
                        ScrollPositionInternal = Max;
                }
            }
            else if (sliderRegionRectUp.Contains(e.Location))
            {
                if (Alignment == ScrollBarAlignment.Vertical)
                    direction = ScrollDirection.Up;
                if (Alignment == ScrollBarAlignment.Horizontal)
                    direction = ScrollDirection.Left;
                scrollStep = ScrollStep.Medium;
                mouseDownTimer.Enabled = true;
                mouseDownTimer.Start();
                mouseDown = true;
                if (ScrollPositionInternal >= ((int)ScrollStep.Medium*scrollNasobitel))
                    ScrollPositionInternal = scrollPosition - ((int)ScrollStep.Medium*scrollNasobitel);
                else
                    ScrollPositionInternal = 0;
            }
            else if (sliderRegionRectDown.Contains(e.Location))
            {
                if (Alignment == ScrollBarAlignment.Vertical)
                    direction = ScrollDirection.Down;
                if (Alignment == ScrollBarAlignment.Horizontal)
                    direction = ScrollDirection.Right;
                scrollStep = ScrollStep.Medium;
                mouseDownTimer.Enabled = true;
                mouseDownTimer.Start();
                mouseDown = true;
                if (scrollPosition + ((int)ScrollStep.Medium * scrollNasobitel) <= Max)
                    ScrollPositionInternal = scrollPosition + ((int)ScrollStep.Medium * scrollNasobitel);
                else
                    ScrollPositionInternal = Max;

            }
            else if (Alignment == ScrollBarAlignment.Horizontal && (new Rectangle(0, 0, 17, Height).Contains(e.Location)))
            {
                if (!mouseDown)
                {
                    direction = ScrollDirection.Left;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o -1
                    if (scrollPosition - ((int)ScrollStep.Small * scrollNasobitel) >= 0)
                        ScrollPositionInternal = scrollPosition - ((int)ScrollStep.Small * scrollNasobitel);
                    else
                        ScrollPositionInternal = 0;

                }
            }
            else if (Alignment == ScrollBarAlignment.Horizontal && (new Rectangle(Width - 17 - (bothVisible ? 17 : 0), 0, 17, Height).Contains(e.Location)))
            {
                if (!mouseDown)
                {
                    direction = ScrollDirection.Right;
                    scrollStep = ScrollStep.Small;
                    mouseDownTimer.Enabled = true;
                    mouseDownTimer.Start();
                    mouseDown = true;
                    //scroll o 1
                    if (scrollPosition + ((int)ScrollStep.Small * scrollNasobitel) <= Max)
                        ScrollPositionInternal = scrollPosition + ((int)ScrollStep.Small * scrollNasobitel);
                    else
                        ScrollPositionInternal = Max;
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
            if (mouseDown)
            {
                mouseDown = false;
                mouseDownTimer.Stop();
                mouseDownTimer.Enabled = false;
                direction = ScrollDirection.No;
                firstScroll = 0;
            }
        }

        private Size SliderSize
        {
            get
            {
                int zakladPomeru = Max + visibleItems;
                int zakladVelikosti = Alignment == ScrollBarAlignment.Vertical ? this.Height - 38 - (bothVisible ? 17 : 0) : this.Width - 38 - (bothVisible ? 17 : 0);
                double pomer = Math.Round((double)visibleItems / (double)zakladPomeru, 4, MidpointRounding.AwayFromZero);
                if (pomer > 1)
                    pomer = 1;
                int velikost = (int)Math.Round((double)zakladVelikosti * pomer, 0, MidpointRounding.AwayFromZero);
                if(velikost < 6) 
                    velikost = 6;
                if (Alignment == ScrollBarAlignment.Horizontal)
                    return new Size(velikost, 6);
                else
                    return new Size(6, velikost);
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
                    if (Alignment == ScrollBarAlignment.Vertical)
                    {
                        if (sliderRectForDrag.Contains(PointToClient(MousePosition)))
                        {
                            mouseDown = false;
                            mouseDownTimer.Stop();
                            mouseDownTimer.Enabled = false;
                            direction = ScrollDirection.No;
                            firstScroll = 0;
                        }

                        int maxPos = Height - 19 - SliderSize.Height - (bothVisible ? 17 : 0);
                        int dostupnyProstor = maxPos - 19;
                        double itemuNaPixel = Math.Round((double)Max / (double)dostupnyProstor, 4, MidpointRounding.AwayFromZero);
                        if (double.IsNaN(itemuNaPixel))
                        {
                            itemuNaPixel = 1;
                        }
                        /*Debug.WriteLine("__________");
                        Debug.WriteLine("ScrollPosition: " + ScrollPosition.ToString());
                        Debug.WriteLine("Max: " + Max.ToString());
                        Debug.WriteLine("Výpočet pozice: " + Math.Round(((ScrollPosition / itemuNaPixel + 18)) + 1, 0, MidpointRounding.ToEven));
                        Debug.WriteLine("VisibleItems: " + VisibleItems.ToString());
                        Debug.WriteLine("TotalItems: " + TotalItems.ToString());*/

                        int scrollPos = (int)Math.Round(((ScrollPosition / itemuNaPixel + 18)) + 1, 0, MidpointRounding.ToEven)-19;

                        Rectangle slider = new Rectangle((Width / 2) - (SliderSize.Width / 2), scrollPos + 18, SliderSize.Width, SliderSize.Height);
                        sliderRectForDrag = new Rectangle(1, scrollPos + 18, Width - 2, SliderSize.Height);
                        sliderRegionRectUp = new Rectangle(1, 17, Width - 2, scrollPos);
                        sliderRegionRectDown = new Rectangle(1, sliderRectForDrag.Bottom + 2, Width - 2, Height - sliderRectForDrag.Bottom - 19 - (bothVisible ? 18 : 0));
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
                            //bg.Graphics.DrawRectangle(new Pen(Color.Violet, 1), new Rectangle(slider.X, slider.Y, 6, 119));
                        }
                    }
                    else if (Alignment == ScrollBarAlignment.Horizontal)
                    {
                        int maxPos = Width - 19 - SliderSize.Width - (bothVisible ? 17 : 0);
                        int dostupnyProstor = maxPos - 19;
                        double itemuNaPixel = Math.Round((double)Max / (double)dostupnyProstor, 4, MidpointRounding.AwayFromZero);
                        if(double.IsNaN(itemuNaPixel))
                        {
                            itemuNaPixel = 1;
                        }
                        /*Debug.WriteLine("__________");
                        Debug.WriteLine("ScrollPosition: " + ScrollPosition.ToString());
                        Debug.WriteLine("Max: " + Max.ToString());
                        Debug.WriteLine("VisibleItems: " + VisibleItems.ToString());
                        Debug.WriteLine("TotalItems: " + TotalItems.ToString());
                        Debug.WriteLine("Slider position: " + (ScrollPosition + 18));
                        Debug.WriteLine("Max position: " + maxPos);
                        Debug.WriteLine("Itemů na pixel: " + itemuNaPixel);
                        Debug.WriteLine("Test výpočet pozice: " + Math.Round(((ScrollPosition / itemuNaPixel + 18))+1, 0, MidpointRounding.ToEven));*/

                        int scrollPos = (int)Math.Round(((ScrollPosition / itemuNaPixel + 18)) + 1, 0, MidpointRounding.ToEven) - 19;

                        Rectangle slider = new Rectangle(scrollPos + 18, (Height / 2) - (SliderSize.Height / 2), SliderSize.Width, SliderSize.Height);
                        sliderRectForDrag = new Rectangle(scrollPos + 18, 1, SliderSize.Width, Height - 2);
                        sliderRegionRectUp = new Rectangle(17, 1, scrollPos, Height - 2);
                        sliderRegionRectDown = new Rectangle(sliderRectForDrag.Right + 2, 1, Width - sliderRectForDrag.Right - 19 - (bothVisible ? 18 : 0), Height - 2);

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
