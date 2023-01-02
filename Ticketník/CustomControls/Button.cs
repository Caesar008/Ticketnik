using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.ComponentModel;

namespace Ticketník.CustomControls
{
    internal class Button : System.Windows.Forms.Button
    {
        private bool _mouseIn = false;
        private Color BackgroundColor
        {
            get
            {
                return this.Parent.BackColor;
            }
        }
        private Color borderColorMouseOver = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color BorderColorMouseOver
        {
            get { return borderColorMouseOver; }
            set
            {
                if (borderColorMouseOver != value)
                {
                    borderColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _mouseIn = true;
            base.OnMouseEnter(eventargs);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _mouseIn = false;
            base.OnMouseLeave(eventargs);
            Invalidate();
        }
        
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Messages.OnPaint && FlatStyle == FlatStyle.Flat)
            {
                var dc = GetWindowDC(Handle);

                //umazat starý rámeček a kousek pozdí, umístění obrázku když je na střed
                using (Pen p = new Pen(BackgroundColor, FlatAppearance.BorderSize + 1))
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    {
                        g.SmoothingMode = SmoothingMode.None;
                        //překreslení obrázku
                        if (Image != null)
                        {
                            using (SolidBrush b = new SolidBrush(_mouseIn ? FlatAppearance.MouseOverBackColor : BackColor))
                            {
                                g.FillRectangle(b, new Rectangle(1, 1, Width - 2, Height - 2));
                            }
                            if(ImageAlign == ContentAlignment.MiddleCenter)
                                g.DrawImage(Image, (Width/2) - (Image.Width / 2), (Height / 2) - (Image.Height / 2));
                        }
                        //mazání rámečku
                        g.DrawPath(p, RoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 3, 3, 3, 3));
                        g.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
                        
                    }
                }
                //kulatý rámeček
                using (Pen p = new Pen(_mouseIn ? BorderColorMouseOver : FlatAppearance.BorderColor, FlatAppearance.BorderSize))
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawPath(p, RoundedRect(new Rectangle(1, 1, Width-3, Height-3), 3, 3, 3, 3));
                    }
                }
                ReleaseDC(Handle, dc);
            }
        }

        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

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
