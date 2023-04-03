using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Principal;
using System.Globalization;
using System.Runtime.InteropServices;
using static System.Windows.Forms.TabControl;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics.CodeAnalysis;

namespace Ticketník.CustomControls
{
    internal class TabControl : System.Windows.Forms.Control
    {
        private Color borderColor = Color.LightGray;
        [DefaultValue(typeof(Color), "Gray")]
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

        private Color headerBackColor = Color.FromArgb(244, 244, 244);
        [DefaultValue("#F4F4F4")]
        public Color HeaderBackColor
        {
            get { return headerBackColor; }
            set
            {
                if (headerBackColor != value)
                {
                    headerBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color headerActiveBackColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color HeaderActiveBackColor
        {
            get { return headerActiveBackColor; }
            set
            {
                if (headerActiveBackColor != value)
                {
                    headerActiveBackColor = value;
                    Invalidate();
                }
            }
        }

        bool _mouseDown = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //base.OnMouseDown(e);
            if (!_mouseDown)
            {
                _mouseDown = true;
                for (int i = 0; i < TabPages.Count; i++)
                {
                    if (GetTabRect(i).Contains(e.Location))
                    {
                        SelectedIndex = i;
                        selectedTab = TabPages[SelectedIndex];
                        break;
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _mouseDown = false;
        }

        private CustomControls.TabPage selectedTab = null;
        public CustomControls.TabPage SelectedTab
        {
            get { return selectedTab; }
            set
            {
                if (selectedTab != value)
                {
                    selectedTab = value;
                    for (int i = 0; i < TabPages.Count; i++)
                    {
                        if (TabPages[i] == selectedTab)
                        {
                            selectedIndex = i;
                            TabPages[i].Visible = true;
                            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);

                            Rectangle newTabRect = GetTabRect(selectedIndex);
                            Size newSelSize = MeasureHeader(TabPages[selectedIndex]);
                            Rectangle newTabRectSel = new Rectangle(newTabRect.X - 2, newTabRect.Y - 2, newSelSize.Width + 5, newSelSize.Height + 3);

                            Rectangle lastTabRect = GetTabRect(lastSelected);
                            Size lastSelSize = MeasureHeader(TabPages[lastSelected]);
                            Rectangle lastTabRectSel = new Rectangle(lastTabRect.X - 2, lastTabRect.Y - 2, lastSelSize.Width + 5, lastSelSize.Height + 3);

                            Invalidate(newTabRectSel);
                            Invalidate(lastTabRectSel);
                            lastSelected = selectedIndex;
                            SelectedTab.Refresh();
                            break;
                        }
                    }
                }
            }
        }

        private int selectedIndex = -1;
        private int lastSelected = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value >= 0 && value < TabPages.Count)
                {
                    selectedIndex = value;
                    selectedTab = TabPages[value];
                    TabPages[selectedIndex].Visible = true;
                    //int tmpI = 0;
                    for (int i = 0; i < TabPages.Count; i++)
                    {
                        if (i != selectedIndex)
                        {
                            TabPages[i].Visible = false;
                        }
                    }
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);

                    Rectangle newTabRect = GetTabRect(selectedIndex);
                    Size newSelSize = MeasureHeader(TabPages[selectedIndex]);
                    Rectangle newTabRectSel = new Rectangle(newTabRect.X - 2, newTabRect.Y - 2, newSelSize.Width + 5, newSelSize.Height + 3);
                    
                    Rectangle lastTabRect = GetTabRect(lastSelected);
                    Size lastSelSize = MeasureHeader(TabPages[lastSelected]);
                    Rectangle lastTabRectSel = new Rectangle(lastTabRect.X - 2, lastTabRect.Y - 2, lastSelSize.Width + 5, lastSelSize.Height + 3);

                    Invalidate(newTabRectSel);
                    Invalidate(lastTabRectSel);
                    lastSelected = selectedIndex;
                    SelectedTab.Refresh();
                }
            }
        }

        private int headerHight = 18;
        [DefaultValue(18)]
        public int HeaderHight
        {
            get { return headerHight; }
            set
            {
                if (headerHight != value && value > 2)
                {
                    headerHight = value;
                    HeaderHightChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public TabControl()
        {
            TabPages = new TabPageCollection();
            TabPages.Parent = this;
            ControlAdded += TabControl_ControlAdded;
            ControlRemoved += TabControl_ControlRemoved;
        }

        private void TabControl_SizeChanged(object sender, EventArgs e)
        {
            foreach(CustomControls.TabPage tp in TabPages)
            {
                tp.Size = new Size(Width - 2, Height - 3 - headerHight);
            }
        }

        private void TabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(CustomControls.TabPage))
            {
                TabPages.Remove((CustomControls.TabPage)e.Control);
            }
        }

        private void TabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            if(e.Control.GetType() == typeof(CustomControls.TabPage)) 
            {
                TabPages.Add((CustomControls.TabPage)e.Control);
                e.Control.Location= new Point(1, headerHight+2);
                e.Control.Width = Width - 2;
                e.Control.Height = Height - 3 - headerHight;
            }
        }

        public class TabPageCollection
        {
            List<CustomControls.TabPage> tabs = new List<CustomControls.TabPage>();

            internal TabControl Parent { get; set; }

            public CustomControls.TabPage this[int index]
            {
                get
                {
                    if (index >= 0 && index < tabs.Count)
                        return tabs[index];
                    else
                        throw new ArgumentOutOfRangeException("index");
                }
                set
                {
                    if (index >= 0 && index < tabs.Count)
                        tabs[index] = value;
                    else
                        throw new ArgumentOutOfRangeException("index");
                }
            }
            public int Count => tabs.Count;

            public void Add(CustomControls.TabPage value)
            {
                value.Parent = Parent;
                tabs.Add(value);
            }

            public void Clear()
            {
                tabs.Clear();
            }

            public bool Contains(CustomControls.TabPage value)
            {
                return tabs.Contains(value);
            }

            public IEnumerator GetEnumerator()
            {
                return tabs.GetEnumerator();
            }

            public int IndexOf(CustomControls.TabPage value)
            {
                return tabs.IndexOf(value);
            }

            public void Insert(int index, CustomControls.TabPage value)
            {
                tabs.Insert(index, value);
            }

            public void Remove(CustomControls.TabPage value)
            {
                tabs.Remove(value);
            }

            public void RemoveAt(int index)
            {
                tabs.RemoveAt(index);
            }
        }
        public TabPageCollection TabPages
        {
            get;
            private set;
        }

        public event EventHandler SelectedIndexChanged;
        public event EventHandler HeaderHightChanged;

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Size headers = MeasureHeaders(TabPages);
            Rectangle allHeaders = new Rectangle(0, 0, headers.Width + 3, headers.Height);

            using (Pen p = new Pen(BorderColor, 1))
            {
                using (SolidBrush b = new SolidBrush(Parent.BackColor))
                {
                    using (Graphics g = e.Graphics)
                    {
                        Rectangle cely = new Rectangle(0, headerHight+1, Width, Height-headerHight-1);
                        using (BufferedGraphics cbg = BufferedGraphicsManager.Current.Allocate(g, cely))
                        {
                            cbg.Graphics.DrawLine(p, cely.Left, cely.Top, cely.Left, cely.Bottom-1);
                            cbg.Graphics.DrawLine(p, cely.Left, cely.Bottom-1, cely.Right-1, cely.Bottom-1);
                            cbg.Graphics.DrawLine(p, cely.Right-1, cely.Top - 1, cely.Right-1, cely.Bottom-1);
                            cbg.Graphics.DrawLine(p, cely.Left, cely.Top, cely.Right - 1, cely.Top);


                            cbg.Render();
                        }
                        using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(g, allHeaders))
                        {
                            if (!headers.IsEmpty)
                            {
                                //taby
                                int index = 0;
                                bg.Graphics.FillRectangle(b, allHeaders);
                                bg.Graphics.DrawLine(p, 0, 0 + headers.Height - 1, Width - 1, headers.Height - 1);
                                bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                //vykreslit vzadu
                                foreach (CustomControls.TabPage tp in TabPages)
                                {
                                    if (SelectedIndex != index)
                                    {
                                        Size header = MeasureHeader(TabPages[index]);
                                        Rectangle headerRect = GetTabRect(index);
                                        if (index == TabPages.Count - 1)
                                            headerRect.Width -= 2;
                                        using (SolidBrush abr = new SolidBrush(HeaderBackColor))
                                        {
                                            GraphicsPath headerRectFill = RoundedRect(new Rectangle(headerRect.X, headerRect.Y,
                                                headerRect.Width, headerRect.Height), 1, 1, 0, 0);

                                            bg.Graphics.SmoothingMode = SmoothingMode.None;
                                            bg.Graphics.FillPath(abr, headerRectFill);
                                            bg.Graphics.DrawLine(p, 0, 0 + headerRect.Height + 1, Width - 1, headerRect.Height + 1);
                                            TextRenderer.DrawText(bg.Graphics, TabPages[index].Text, TabPages[index].Font, new Point(headerRect.X + 2, headerRect.Y + 2), this.FindForm().ForeColor);
                                        }
                                        bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                        bg.Graphics.DrawPath(p, RoundedRect(new Rectangle(headerRect.X, headerRect.Y,
                                                headerRect.Width, headerRect.Height), 1, 1, 0, 0));
                                    }
                                    index++;
                                }
                                //a teď vybraný do popředí
                                Size headerSel = MeasureHeader(SelectedTab);
                                Rectangle tabRect = GetTabRect(SelectedIndex);
                                Rectangle headerRectSel = new Rectangle(tabRect.X - 2, tabRect.Y - 2, headerSel.Width + 4, headerSel.Height + 3);
                                using (SolidBrush abr = new SolidBrush(HeaderActiveBackColor))
                                {
                                    GraphicsPath headerRectSelFill = RoundedRect(new Rectangle(tabRect.X - 2, tabRect.Y - 2,
                                        headerSel.Width + 4, headerSel.Height + 4), 1, 1, 0, 0);

                                    bg.Graphics.SmoothingMode = SmoothingMode.None;
                                    bg.Graphics.FillPath(abr, headerRectSelFill);

                                    bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                    bg.Graphics.DrawPath(p, RoundedRect(headerRectSel, 1, 1, 0, 0));
                                    bg.Graphics.SmoothingMode = SmoothingMode.None;
                                    TextRenderer.DrawText(bg.Graphics, SelectedTab.Text, SelectedTab.Font, new Point(headerRectSel.X + 4, headerRectSel.Y + 3), Parent.ForeColor);
                                }
                                bg.Render();
                            }
                        }
                    }
                }

            }
        }
        private Size MeasureHeader(CustomControls.TabPage page)
        {
            Size size = TextRenderer.MeasureText(page.Text, page.Font) + new Size(4, 4);
            if (size.Width < 41)
                return new Size(41, headerHight);

            return new Size(size.Width, headerHight);
        }

        private Size MeasureHeaders(TabPageCollection pages)
        {
            Rectangle tr = ((CustomControls.TabControl)pages[pages.Count - 1].Parent).GetTabRect(pages.Count - 1);
            return new Size(tr.X + tr.Width, tr.Y + tr.Height);
        }

        private Rectangle GetTabRect(int index)
        {
            int width = 0;
            int x = 0;
            for(int i = 0; i < TabPages.Count; i++)
            {
                width = MeasureHeader(TabPages[i]).Width;
                if (i < index)
                {
                    x += width;
                }
                else
                    break;
            }
            return new Rectangle(x+2, 2, width, headerHight);
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


            //nechci spodní čáru. Začátek je tak posunut od jiného rohu
            //path.CloseFigure();
            return path;
        }
    }
}
