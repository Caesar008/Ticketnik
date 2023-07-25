using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using static Ticketník.Zakaznici;
using System.ComponentModel;
using System.Diagnostics;

namespace Ticketník.CustomControls
{
    public partial class ComboBox : System.Windows.Forms.Control
    {
        protected sealed class DropDownList : System.Windows.Forms.Form
        {
            private static VScrollBar vScrollBar = new VScrollBar();
            private Color borderColor = Color.Gray;
            private int _scrollPosition = 0;
            private Point _mousePos = Point.Empty;
            private Rectangle _markedItem;
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

            private Color selectedItemBackColor = Color.DodgerBlue;
            [DefaultValue(typeof(Color), "DodgerBlue")]
            public Color SelectedItemBackColor
            {
                get
                {
                    return selectedItemBackColor;
                }
                set { selectedItemBackColor = value; }
            }

            private Color selectedItemForeColor = Color.White;
            [DefaultValue(typeof(Color), "White")]
            public Color SelectedItemForeColor
            {
                get { return selectedItemForeColor; }
                set { selectedItemForeColor = value; }
            }

            private int maxVisibleItems = 30;
            public int MaxVisibleItems
            {
                get { return maxVisibleItems; }
                set { maxVisibleItems = value; }
            }

            private bool autosize = true;
            new public bool AutoSize
            {
                get { return autosize; }
                set { autosize = value; }
            }

            public void SetWidth(int width)
            {
                if (width <= maxWidth && width >= Parent.Width)
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
                    MaximumSize = new Size(Parent.Width, Height);
                    Width = Parent.Width;
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
                    _markedItem = new Rectangle();
                    Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                }
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                int w = Parent.Width;
                int radky = Parent.Items.Count;
                int h = TextRenderer.MeasureText("A", Font).Height;
                if (radky > maxVisibleItems)
                {
                    radky = maxVisibleItems;
                    VScrollBarVisible = true;
                }
                foreach (object s in Parent.Items)
                {
                    Size velikost = TextRenderer.MeasureText(s as string, Font);
                    if (w < velikost.Width)
                        w = VScrollBarVisible ? velikost.Width + vScrollBar.Width : velikost.Width; 
                }

                MaximumSize = new Size(radky * h + 2,w);
                Height = radky * h +2;
                SetWidth(w);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                _mousePos = e.Location;
                int vyska = TextRenderer.MeasureText("A", Font).Height;
                int itemNum = e.Location.Y / vyska;
                if (itemNum >= Parent.Items.Count)
                    itemNum = Parent.Items.Count - 1;
                if (_markedItem == null || !_markedItem.Contains(e.Location))
                {
                    int osaY = (itemNum - _scrollPosition) * vyska +1;
                    _markedItem = new Rectangle(0, osaY, Width, vyska);
                    Invalidate();
                }
            }
            Rectangle item;

            private bool VScrollBarVisible
            {
                get; set;
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

                    //itemy
                    for (int i = 0; i < Parent.Items.Count; i++)
                    {
                        Size velikost = TextRenderer.MeasureText(Parent.Items[i] as string, Font);
                        item = new Rectangle(0, (i-_scrollPosition) * velikost.Height +1, VScrollBarVisible ? Width - vScrollBar.Width : Width, velikost.Height);
                        
                        if ((Parent.SelectedItem == Parent.Items[i] && (_markedItem == null || _markedItem.IsEmpty)) || 
                            item.IntersectsWith(_markedItem))
                        {
                            //označení
                            using (Brush b = new SolidBrush(selectedItemBackColor))
                            {
                                bg.Graphics.FillRectangle(b, item);
                            }
                            TextRenderer.DrawText(bg.Graphics, Parent.Items[i] as string, Font, item, selectedItemForeColor, TextFormatFlags.EndEllipsis);
                        }
                        else
                        {
                            TextRenderer.DrawText(bg.Graphics, Parent.Items[i] as string, Font, item, ForeColor, TextFormatFlags.EndEllipsis);
                        }
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
