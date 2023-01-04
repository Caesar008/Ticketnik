using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static Ticketník.CustomControls.ComboBox;

namespace Ticketník.CustomControls
{
    public class TabControl : System.Windows.Forms.TabControl
    {
        private bool _mouseIn = false;

        private Color headerBorderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color HeaderBorderColor
        {
            get { return headerBorderColor; }
            set
            {
                if (headerBorderColor != value)
                {
                    headerBorderColor = value;
                    Invalidate();
                }
            }
        }

        private Color headerBackColor = SystemColors.Control;
        [DefaultValue(typeof(SystemColors), "Control")]
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

        /*protected override void OnSelected(TabControlEventArgs e)
        {
            base.OnSelected(e);
            Refresh();
        }*/

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Messages.OnPaint)
            {
                var handle = Handle;
                Size headers = MeasureHeaders(TabPages);
                Rectangle allHeaders = new Rectangle(0, 0, headers.Width+2, headers.Height);
                var dc = GetWindowDC(handle);
                using (Pen p = new Pen(HeaderBorderColor, 1))
                {
                    using (SolidBrush b = new SolidBrush(this.FindForm().BackColor))
                    {
                        using (Graphics g = Graphics.FromHdc(dc))
                        {
                            int index = 0;
                            g.FillRectangle(b, allHeaders);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            //vykreslit vzadu
                            foreach (TabPage tp in TabPages)
                            {
                                if (SelectedIndex != index)
                                {
                                    Size header = MeasureHeader(TabPages[index]);
                                    Rectangle headerRect = GetTabRect(index);/*new Rectangle(HeaderOffset(TabPages, index), 2,
                                        header.Width,header.Height);*/
                                    if (index == TabPages.Count - 1)
                                        headerRect.Width -= 2;
                                    using (SolidBrush abr = new SolidBrush(HeaderBackColor))
                                    {
                                        GraphicsPath headerRectFill = RoundedRect(new Rectangle(headerRect.X, headerRect.Y,
                                            headerRect.Width, headerRect.Height -1), 1, 1, 0, 0);
                                        g.FillPath(abr, headerRectFill);
                                        TextRenderer.DrawText(g, TabPages[index].Text, TabPages[index].Font, new Point(headerRect.X+2, headerRect.Y+2), this.FindForm().ForeColor);
                                    }
                                    g.DrawPath(p, RoundedRect(new Rectangle(headerRect.X, headerRect.Y,
                                            headerRect.Width, headerRect.Height - 1), 1, 1, 1, 1));
                                }
                                index++;
                            }
                            //a teď vybraný do popředí
                            Size headerSel = MeasureHeader(SelectedTab);
                            Rectangle tabRect = GetTabRect(SelectedIndex);
                            Rectangle headerRectSel = new Rectangle(tabRect.X-2,tabRect.Y - 2, headerSel.Width + 4, headerSel.Height + 2);
                            Rectangle bottom = new Rectangle(headerRectSel.X+1, headerSel.Height+1, headerRectSel.Width-1, 3);
                            using (SolidBrush abr = new SolidBrush(HeaderActiveBackColor))
                            {
                                GraphicsPath headerRectSelFill = RoundedRect(new Rectangle(tabRect.X - 2, tabRect.Y - 2, 
                                    headerSel.Width + 4, headerSel.Height + 3), 1, 1, 0, 0);
                                g.FillPath(abr, headerRectSelFill);
                                g.DrawPath(p, RoundedRect(headerRectSel, 1, 1, 0, 0));
                                //tady pak umazat sposdní čáru

                                g.SmoothingMode = SmoothingMode.None;
                                g.FillRectangle(abr, bottom);
                                TextRenderer.DrawText(g, SelectedTab.Text, SelectedTab.Font, new Point(headerRectSel.X + 4, headerRectSel.Y + 3), this.FindForm().ForeColor);
                            }
                        }
                    }
                }
                ReleaseDC(handle, dc);
            }
        }

        private static Size MeasureHeader(TabPage page)
        {
            Size size = TextRenderer.MeasureText(page.Text, page.Font) + new Size(4, 4);
            if (size.Width < 41)
                return new Size(41, size.Height);
            return size;
        }

        private static Size MeasureHeaders(TabPageCollection pages)
        {
            int width = 0;
            int height = 0;
            foreach(TabPage tp in pages)
            {
                Size s = MeasureHeader(tp);
                width += s.Width;
                if(height < s.Height)
                    height = s.Height;
            }
            return new Size(width+(pages.Count-1), height+3);
        }

        private static Size MeasureHeadersFull(TabPageCollection pages)
        {
            int width = 0;
            int height = 0;
            foreach (TabPage tp in pages)
            {
                Size s = MeasureHeader(tp);
                width += s.Width;
                if (height < s.Height)
                    height = s.Height + 2;
            }
            return new Size(width + (pages.Count - 1), height + 3);
        }

        private static int HeaderOffset(TabPageCollection pages,int index)
        {
            int x = 2;
            for(int i = 0; i < index; i++)
            {
                x += MeasureHeader(pages[i]).Width+1;
            }
            return x;
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



        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}
