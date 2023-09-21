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
    //zkusit si pohrát s NativeWindow nebo s https://johnespiritu.info/blog/winforms-custom-dropdown/ ToolStripDropDown
    public partial class ComboBox : System.Windows.Forms.Control
    {
        protected sealed class DropDownList : System.Windows.Forms.Form
        {
            private ScrollBar vScrollBar;
            private Point _mousePos = Point.Empty;
            private bool _scrolling = false;

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
            public DropDownList(CustomControls.ComboBox parent) 
            {
                this.Parent = parent;
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
                this.Tag = "CustomColor:Ignore";
                this.vScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAlignment.Vertical, this);
                this.vScrollBar.Visible = false;
                this.vScrollBar.ParentBorderColor = Parent.BorderColorMouseOver;
                this.vScrollBar.RespectParentBorder = true;
                this.vScrollBar.Scrolled += VScrollBar_Scrolled;
                Motiv.SetControlColor(vScrollBar);
            }

            internal bool FitDown(int proposedYCoord)
            {
                int height = Screen.FromHandle(this.Handle).WorkingArea.Height - 1;
                if (proposedYCoord + this.Height > height)
                    return false;
                return true;
            }

            private void VScrollBar_Scrolled(object sender, ScrollBar.ScrollEventArgs e)
            {
                Parent._scrollPosition = e.NewPosition;
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
                if (Parent != null && this.isOpen && Parent.DropDownStyle != ComboBoxStyle.DropDown)
                {
                    this.Hide();
                    this.isOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                    //_markedItem = -1;
                    this.Close();
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
                Parent._markedItem = Parent.SelectedIndex;
                EnsureVisible(Parent._markedItem);
                if(!FitDown(this.Location.Y))
                {
                    this.Location = new Point(this.Location.X, this.Location.Y - this.Height - Parent.Height);
                }

                if(Parent.DropDownStyle == ComboBoxStyle.DropDown && Parent.textBox.Text.Length > 0)
                {
                    Parent.textBox.BackColor = Color.FromArgb(0, 120, 215);
                }
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
                    if (itemNum != Parent._markedItem)
                    {
                        Parent._markedItem = itemNum + Parent._scrollPosition;
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
                    int itemNum = (e.Location.Y / vyska) + Parent._scrollPosition;
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
                else if(keyData == Keys.Enter || keyData == Keys.Escape)
                {
                    Hide();
                    IsOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                    Parent.Text = Parent.items[Parent.SelectedIndex] as string;
                    Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                Parent._markedItem = Parent.SelectedIndex;
                EnsureVisible(Parent._markedItem);
                //Invalidate();
                return true;
            }

            bool backspace = false;

            protected override void OnKeyDown(KeyEventArgs e)
            {
                base.OnKeyDown(e);
                if (Parent.textBox.BackColor == Color.FromArgb(0, 120, 215))
                {
                    Parent.textBox.BackColor = Parent.BackColor;
                    Parent.textBox.Text = "";
                }
                if (e.KeyCode == Keys.Back)
                {
                    backspace = true;
                    if(Parent.textBox.Text.Length > 0) 
                        Parent.textBox.Text = Parent.textBox.Text.Remove(Parent.textBox.Text.Length - 1);
                }
            }

            DateTime lastSearch = DateTime.MinValue;
            string search = "";

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                base.OnKeyPress(e);
                if (!backspace)
                {
                    if (Parent.DropDownStyle == ComboBoxStyle.DropDown)
                    {
                        if (Parent.textBox.BackColor == Color.FromArgb(0, 120, 215))
                        {
                            Parent.textBox.BackColor = Parent.BackColor;
                            Parent.textBox.Text = "";
                        }
                        Parent.textBox.Text += e.KeyChar;
                    }
                    else if (Parent.DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        if (lastSearch.AddSeconds(1) < DateTime.Now)
                            search = "";
                        search += e.KeyChar;
                        for (int i = 0; i < Parent.Items.Count; i++)
                        {
                            if ((Parent.Items[i] as string).ToLower().StartsWith(search.ToLower()))
                            {
                                Parent.SelectedIndex = i;
                                Parent._markedItem = Parent.SelectedIndex;
                                EnsureVisible(Parent._markedItem);
                                break;
                            }
                        }
                        lastSearch = DateTime.Now;
                    }
                }
                backspace = false;
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
                    int newScroll = Parent._scrollPosition - posun;
                    if (newScroll > maxScroll)
                        newScroll = maxScroll;
                    else if (newScroll < 0)
                        newScroll = 0;
                    Parent._scrollPosition = newScroll;
                    vScrollBar.ScrollPosition = newScroll;
                    Invalidate();
                    mouseWheelStep = DateTime.Now;
                }
            }

            public void EnsureVisible(int index)
            {
                int maxVisible = MaxVisibleItems + Parent._scrollPosition;
                if(index < Parent._scrollPosition && index >= 0)
                {
                    Parent._scrollPosition = index;
                }
                else if(index < Parent.Items.Count && index > maxVisible-1)
                { 
                    Parent._scrollPosition = index - MaxVisibleItems+1;
                }
                vScrollBar.ScrollPosition = Parent._scrollPosition;
                Invalidate();
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                vScrollBar.Visible = VScrollBarVisible;

                if (Visible && !FitDown(this.Location.Y))
                {
                    this.Location = new Point(this.Location.X, this.Location.Y - this.Height - Parent.Height);
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                //base.OnPaint(e);
                using(BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
                {
                    bg.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    //pozadí
                    using(Brush b = new SolidBrush(Parent.BackColor))
                    {
                        bg.Graphics.FillRectangle(b, new Rectangle(0, 0, Width, Height));
                    }

                    //itemy
                    for (int i = 0; i < Parent.Items.Count; i++)
                    {
                        Size velikost = TextRenderer.MeasureText(Parent.Items[i] as string, Font);
                        if (velikost == Size.Empty)
                            velikost = TextRenderer.MeasureText("A" as string, Font);
                        item = new Rectangle(0, (i-Parent._scrollPosition) * velikost.Height +1, VScrollBarVisible ? Width - vScrollBar.Width : Width, velikost.Height);

                        Rectangle marked = new Rectangle(0, (Parent._markedItem - Parent._scrollPosition) * velikost.Height +1, VScrollBarVisible ? Width - vScrollBar.Width : Width, velikost.Height);
                        
                        if (item.IntersectsWith(marked))
                        {
                            //označení
                            using (Brush b = new SolidBrush(Parent.SelectedItemBackColor))
                            {
                                bg.Graphics.FillRectangle(b, item);
                            }
                            TextRenderer.DrawText(bg.Graphics, Parent.Items[i] as string, Font, item, Parent.SelectedItemForeColor, TextFormatFlags.EndEllipsis);
                        }
                        else
                        {
                            TextRenderer.DrawText(bg.Graphics, Parent.Items[i] as string, Font, item, Parent.ForeColor, TextFormatFlags.EndEllipsis);
                        }
                    }

                //rámeček
                    using(Pen p = new Pen(Parent.BorderColorMouseOver))
                    {
                        bg.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    }
                    bg.Render();
                }
            }
        }
    }
}
