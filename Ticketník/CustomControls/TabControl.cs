using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == Messages.OnPaint)
            {
                var handle = Handle;
                Size headers = MeasureHeaders(TabPages);
                Rectangle allHeaders = new Rectangle(0, 0, headers.Width+2, headers.Height);
                var dc = GetWindowDC(handle);
                using (Pen p = new Pen(BorderColor, 1))
                {
                    using (SolidBrush b = new SolidBrush(Parent.BackColor))
                    {
                        using (Graphics g = Graphics.FromHdc(dc))
                        {
                            //rámeček celý
                            Rectangle tabCR = new Rectangle(0, MeasureHeaders(TabPages).Height, this.Width-1, this.Height - MeasureHeaders(TabPages).Height - 1);
                            g.DrawRectangle(p, tabCR);
                            using(Pen v = new Pen(HeaderActiveBackColor, 1))
                                {
                                //vnitřní vyplnění
                                Rectangle tabCRIn = new Rectangle(1, MeasureHeaders(TabPages).Height+1, this.Width - 3, this.Height - MeasureHeaders(TabPages).Height - 3);
                                g.DrawRectangle(v, tabCRIn);
                            }

                            //taby
                            int index = 0;
                            g.FillRectangle(b, allHeaders);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            //vykreslit vzadu
                            foreach (TabPage tp in TabPages)
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

                                        g.SmoothingMode = SmoothingMode.None;
                                        g.FillPath(abr, headerRectFill);
                                        TextRenderer.DrawText(g, TabPages[index].Text, TabPages[index].Font, new Point(headerRect.X+2, headerRect.Y+2), this.FindForm().ForeColor);
                                    }
                                    g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.DrawPath(p, RoundedRect(new Rectangle(headerRect.X, headerRect.Y,
                                            headerRect.Width, headerRect.Height), 1, 1, 0, 0));
                                }
                                index++;
                            }
                            //a teď vybraný do popředí
                            Size headerSel = MeasureHeader(SelectedTab);
                            Rectangle tabRect = GetTabRect(SelectedIndex);
                            Rectangle headerRectSel = new Rectangle(tabRect.X-2,tabRect.Y - 2, headerSel.Width + 4, headerSel.Height + 3);
                            using (SolidBrush abr = new SolidBrush(HeaderActiveBackColor))
                            {
                                GraphicsPath headerRectSelFill = RoundedRect(new Rectangle(tabRect.X - 2, tabRect.Y - 2, 
                                    headerSel.Width + 4, headerSel.Height + 4), 1, 1, 0, 0);

                                g.SmoothingMode = SmoothingMode.None;
                                g.FillPath(abr, headerRectSelFill);

                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                g.DrawPath(p, RoundedRect(headerRectSel, 1, 1, 0, 0));
                                g.SmoothingMode = SmoothingMode.None;
                                TextRenderer.DrawText(g, SelectedTab.Text, SelectedTab.Font, new Point(headerRectSel.X + 4, headerRectSel.Y + 3), Parent.ForeColor);
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
            Rectangle tr = ((TabControl)pages[pages.Count-1].Parent).GetTabRect(pages.Count - 1);
            return new Size(tr.X + tr.Width, tr.Y + tr.Height);
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



        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}
