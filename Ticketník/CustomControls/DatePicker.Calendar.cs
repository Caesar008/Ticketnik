﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace Ticketník.CustomControls
{
    internal partial class DatePicker
    {
        protected sealed class Calendar : System.Windows.Forms.Form
        {
            private Color borderColor = Color.Gray;
            public Color BorderColor
            {
                get { return borderColor; }
                set
                {
                    if (borderColor != value)
                    {
                        borderColor = value;
                        Invalidate();
                    }
                }
            }
            private Color backColor = SystemColors.Control;
            public Color BackgroundColor
            {
                get { return backColor; }
                set
                {
                    if (backColor != value)
                    {
                        backColor = value;
                        Invalidate();
                    }
                }
            }
            public Calendar(Color borderColor, Color backColor)
            {
                this.MinimizeBox= false;
                this.MaximizeBox= false;
                this.ControlBox = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.ShowInTaskbar= false;
                this.ShowIcon= false;
                this.AllowTransparency = true;
                this.BackColor = Color.FromArgb(1, 2, 3);
                this.TransparencyKey = Color.FromArgb(1, 2, 3);
                //146*158
                this.Width = 146;
                this.Height = 158;
                this.MaximumSize = new System.Drawing.Size(146, 158);
                this.BorderColor = borderColor;
                this.BackgroundColor= backColor;
                this.Tag = "CustomColor:Ignore";
            }

            protected override void OnLostFocus(EventArgs e)
            {
                base.OnLostFocus(e);
                this.Hide();
            }

            //protected override bool ShowWithoutActivation => true;

            protected override void OnPaint(PaintEventArgs e)
            {
                //base.OnPaint(e);
                using (Graphics g = e.Graphics)
                {
                    Rectangle drawArea = new Rectangle(0, 0, Width - 1, Height - 1);
                    
                    using (SolidBrush b = new SolidBrush(BackgroundColor))
                    {
                        g.FillPath(b, RoundedRect(drawArea, 3, 3, 3, 3));
                    }
                    using (Pen p = new Pen(BorderColor, 1))
                    {
                        g.DrawPath(p, RoundedRect(drawArea, 3, 3, 3, 3));
                    }
                    g.SmoothingMode = SmoothingMode.AntiAlias;
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
}
