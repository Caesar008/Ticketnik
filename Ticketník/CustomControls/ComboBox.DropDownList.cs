using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace Ticketník.CustomControls
{
    public partial class ComboBox : System.Windows.Forms.Control
    {
        protected sealed class DropDownList : System.Windows.Forms.Form
        {
            private ScrollBar vScrollBar;
            private Color borderColor = Color.Gray;
            private int _scrollPosition = 0;
            private Point _mousePos = Point.Empty;
            internal int _markedItem = -1;
            private bool _scrolling = false;

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
                this.vScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAlignment.Vertical, this);
                this.vScrollBar.Visible = false;
                this.vScrollBar.ParentBorderColor = BorderColor;
                this.vScrollBar.RespectParentBorder = true;
                this.vScrollBar.Scrolled += VScrollBar_Scrolled;
                Motiv.SetControlColor(vScrollBar);
            }

            private void VScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
            {
                this._scrollPosition = e.NewPosition;
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                if (VScrollBarVisible && vScrollBar.Bounds.Contains(e.Location))
                    _scrolling = true;
            }

            protected override void OnLostFocus(EventArgs e)
            {
                //base.OnLostFocus(e)
                if (Parent != null)
                {
                    this.Hide();
                    this.isOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                    //_markedItem = -1;
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
                    VScrollBarVisible = vScrollBar.Visible = true;
                    vScrollBar.TotalItems = Parent.Items.Count;
                    vScrollBar.VisibleItems = radky;
                }
                foreach (object s in Parent.Items)
                {
                    Size velikost = TextRenderer.MeasureText(s as string, Font);
                    if (w < velikost.Width)
                        w = VScrollBarVisible ? velikost.Width + vScrollBar.Width : velikost.Width; 
                }

                if (AutoSize)
                {
                    MaximumSize = new Size(w, radky * h + 2);
                    Height = radky * h + 2;
                    SetWidth(w);
                }
                vScrollBar.Visible = VScrollBarVisible;
                EnsureVisible(_markedItem);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                if (VScrollBarVisible && vScrollBar.Bounds.Contains(e.Location))
                {
                    
                }
                else if(!_scrolling)
                {
                    _mousePos = e.Location;
                    int vyska = TextRenderer.MeasureText("A", Font).Height;
                    int itemNum = e.Location.Y / vyska;
                    if (itemNum >= Parent.Items.Count)
                        itemNum = Parent.Items.Count - 1;
                    if (itemNum != _markedItem)
                    {
                        _markedItem = itemNum + _scrollPosition;
                        Invalidate();
                    }
                }
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                if (VScrollBarVisible && vScrollBar.Bounds.Contains(e.Location))
                {
                    
                }
                else
                {
                    _mousePos = e.Location;
                    int vyska = TextRenderer.MeasureText("A", Font).Height;
                    int itemNum = (e.Location.Y / vyska) + _scrollPosition;
                    Parent.SelectedIndex = itemNum;

                    this.Hide();
                    this.isOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                    //_markedItem = -1;
                    Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                }
                _scrolling = false;
            }

            Rectangle item;

            private bool VScrollBarVisible
            {
                get; set;
            }

            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                if (keyData == Keys.Up)
                {
                    if (Parent.SelectedIndex > 0)
                    {
                        Parent.SelectedIndex -= 1;
                    }
                }
                else if (keyData == Keys.Down)
                {
                    if (Parent.SelectedIndex < Parent.Items.Count - 1)
                    {
                        Parent.SelectedIndex += 1;
                    }
                }
                _markedItem = Parent.SelectedIndex;
                EnsureVisible(_markedItem);
                //Invalidate();
                return true;
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
               // base.OnPaintBackground(e);
            }

            private DateTime mouseWheelStep = DateTime.Now;

            protected override void OnMouseWheel(MouseEventArgs e)
            {
                //base.OnMouseWheel(e);
                if (mouseWheelStep.AddMilliseconds(75) < DateTime.Now)
                {
                    int posun = e.Delta;
                    if (posun > maxVisibleItems / 3)
                        posun = maxVisibleItems / 3 + 1;
                    else if (posun < -(maxVisibleItems / 3))
                        posun = -(maxVisibleItems / 3 +1);
                    int maxScroll = Parent.Items.Count - MaxVisibleItems;
                    if (maxScroll < 0)
                        maxScroll = 0;
                    int newScroll = _scrollPosition - posun;
                    if (newScroll > maxScroll)
                        newScroll = maxScroll;
                    else if (newScroll < 0)
                        newScroll = 0;
                    _scrollPosition = newScroll;
                    vScrollBar.ScrollPosition = newScroll;
                    Invalidate();
                    mouseWheelStep = DateTime.Now;
                }
            }

            public void EnsureVisible(int index)
            {
                int maxVisible = MaxVisibleItems + _scrollPosition;
                if(index < _scrollPosition && index >= 0)
                {
                    _scrollPosition = index;
                }
                else if(index < Parent.Items.Count && index > maxVisible-1)
                { 
                    _scrollPosition = index - MaxVisibleItems+1;
                }
                vScrollBar.ScrollPosition = _scrollPosition;
                Invalidate();
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                vScrollBar.Visible = VScrollBarVisible;
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
                        if (velikost == Size.Empty)
                            velikost = TextRenderer.MeasureText("A" as string, Font);
                        item = new Rectangle(0, (i-_scrollPosition) * velikost.Height +1, VScrollBarVisible ? Width - vScrollBar.Width : Width, velikost.Height);

                        Rectangle marked = new Rectangle(0, (_markedItem - _scrollPosition) * velikost.Height +1, VScrollBarVisible ? Width - vScrollBar.Width : Width, velikost.Height);
                        
                        if (item.IntersectsWith(marked))
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
