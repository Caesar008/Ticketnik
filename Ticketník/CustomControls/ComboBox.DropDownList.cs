using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník.CustomControls
{
    public partial class ComboBox : System.Windows.Forms.Control
    {
        protected sealed class DropDownList : System.Windows.Forms.Form
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
            private Color backColor = Color.White;
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
            private bool isOpen = false;
            public bool IsOpen
            {
                get { return isOpen; }
                set { isOpen = value; }
            }
            ComboBox cb;
            new public ComboBox Parent
            {
                get { return cb; }
                internal set { cb = value; }
            }

            private int maxWidth = 800;
            public int MaxWidth
            {
                get { return maxWidth; }
                set
                {
                    if (maxWidth != value)
                    {
                        maxWidth = value;
                        Invalidate();
                    }
                }
            }

            private bool autosize = true;
            new public bool AutoSize
            {
                get { return autosize; }
                set { autosize = value; }
            }

            public void SetWidth(int width)
            {
                if (width <= maxWidth && width >= 20)
                {
                    MaximumSize = new Size(width, Height);
                    Width = width;
                }
                else if (width > maxWidth)
                {
                    MaximumSize = new Size(maxWidth, Height);
                    Width = maxWidth;
                }
                else
                {
                    MaximumSize = new Size(20, Height);
                    Width = 20;
                }
                Invalidate();
            }
            public DropDownList(Color borderColor, Color backColor) 
            {
                this.MinimizeBox = false;
                this.MaximizeBox = false;
                this.ControlBox = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.ShowIcon = false;
                this.AllowTransparency = true;
                this.BackColor = Color.FromArgb(1, 2, 3);
                this.TransparencyKey = Color.FromArgb(1, 2, 3);
                this.Width = 20;
                int minHeight = TextRenderer.MeasureText("A", Font).Height + 4;
                this.Height = minHeight;
                this.MaximumSize = new System.Drawing.Size(20, minHeight);
                this.BorderColor = borderColor;
                this.BackgroundColor = backColor;
                this.Tag = "CustomColor:Ignore";
            }

            protected override void OnLostFocus(EventArgs e)
            {
                //base.OnLostFocus(e)
                if (Parent != null)
                {
                    this.Hide();
                    this.isOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                    Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                }
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                int w = 20;
                int radky = Parent.Items.Count;
                int h = TextRenderer.MeasureText("A", Font).Height;
                foreach (object s in Parent.Items)
                {
                    Size velikost = TextRenderer.MeasureText(s as string, Font);
                    if (w < velikost.Width)
                        w = velikost.Width;
                }

                MaximumSize = new Size(radky * h + 2,w);
                Height = radky * h;
                SetWidth(w);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                //base.OnPaint(e);
                using(BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
                {
                    bg.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    //pozadí
                    using(Brush b = new SolidBrush(BackgroundColor))
                    {
                        bg.Graphics.FillRectangle(b, new Rectangle(0, 0, Width, Height));
                    }

                    //rámeček
                    using(Pen p = new Pen(BorderColor))
                    {
                        bg.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    }
                    bg.Render();
                }
            }
        }
    }
}
