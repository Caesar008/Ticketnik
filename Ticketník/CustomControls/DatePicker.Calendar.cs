using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;

namespace Ticketník.CustomControls
{
    internal partial class DatePicker
    {
        protected sealed class Calendar : System.Windows.Forms.Form
        {
            Rectangle header;
            Rectangle buttonL;
            Rectangle buttonR;

            bool _mouseInLB = false;
            bool _mouseInRB = false;
            bool _mouseInHeader = false;

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
            private Color headerColor = Color.White;
            public Color HeaderColor
            {
                get { return headerColor; }
                set
                {
                    if (headerColor != value)
                    {
                        headerColor = value;
                        Invalidate();
                    }
                }
            }
            private Color headerMouseOverColor = Color.White;
            public Color HeaderMouseOverColor
            {
                get { return headerMouseOverColor; }
                set
                {
                    if (headerMouseOverColor != value)
                    {
                        headerMouseOverColor = value;
                        Invalidate();
                    }
                }
            }
            private Color dayHeaderColor = Color.White;
            public Color DayHeaderColor
            {
                get { return dayHeaderColor; }
                set
                {
                    if (dayHeaderColor != value)
                    {
                        dayHeaderColor = value;
                        Invalidate();
                    }
                }
            }
            private Color dayListColor = Color.White;
            public Color DayListColor
            {
                get { return dayListColor; }
                set
                {
                    if (dayListColor != value)
                    {
                        dayListColor = value;
                        Invalidate();
                    }
                }
            }
            private Color todayButtonColor = Color.White;
            public Color TodayButtonColor
            {
                get { return todayButtonColor; }
                set
                {
                    if (todayButtonColor != value)
                    {
                        todayButtonColor = value;
                        Invalidate();
                    }
                }
            }
            private Color headerForeColor = Color.Black;
            public Color HeaderForeColor
            {
                get { return headerForeColor; }
                set
                {
                    if (headerForeColor != value)
                    {
                        headerForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color headerMouseOverForeColor = Color.DodgerBlue;
            public Color HeaderMouseOverForeColor
            {
                get { return headerMouseOverForeColor; }
                set
                {
                    if (headerMouseOverForeColor != value)
                    {
                        headerMouseOverForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color dayHeaderForeColor = Color.Black;
            public Color DayHeaderForeColor
            {
                get { return dayHeaderForeColor; }
                set
                {
                    if (dayHeaderForeColor != value)
                    {
                        dayHeaderForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color dayListForeColor = Color.Black;
            public Color DayListForeColor
            {
                get { return dayListForeColor; }
                set
                {
                    if (dayListForeColor != value)
                    {
                        dayListForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color todayButtonForeColor = Color.Black;
            public Color TodayButtonForeColor
            {
                get { return todayButtonForeColor; }
                set
                {
                    if (todayButtonForeColor != value)
                    {
                        todayButtonForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color trailingForeColor = Color.Gray;
            public Color TrailingForeColor
            {
                get { return trailingForeColor; }
                set
                {
                    if (trailingForeColor != value)
                    {
                        trailingForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color buttonArrowColor = Color.Black;
            public Color ButtonArrowColor
            {
                get { return buttonArrowColor; }
                set
                {
                    if (buttonArrowColor != value)
                    {
                        buttonArrowColor = value;
                        Invalidate();
                    }
                }
            }
            private Color buttonBackColor = Color.White;
            public Color ButtonBackColor
            {
                get { return buttonBackColor; }
                set
                {
                    if (buttonBackColor != value)
                    {
                        buttonBackColor = value;
                        Invalidate();
                    }
                }
            }
            private Color buttonBorderColor = Color.White;
            public Color ButtonBorderColor
            {
                get { return buttonBorderColor; }
                set
                {
                    if (buttonBorderColor != value)
                    {
                        buttonBorderColor = value;
                        Invalidate();
                    }
                }
            }
            private Color separatorColor = Color.Gray;
            public Color SeparatorColor
            {
                get { return separatorColor; }
                set
                {
                    if (separatorColor != value)
                    {
                        separatorColor = value;
                        Invalidate();
                    }
                }
            }
            private Color buttonMoseOverColor = Color.FromArgb(229, 243, 255);
            public Color ButonMouseOverColor
            {
                get { return buttonMoseOverColor; }
                set
                {
                    if (buttonMoseOverColor != value)
                    {
                        buttonMoseOverColor = value;
                        Invalidate();
                    }
                }
            }
            private Color buttonBorderMouseOverColor = Color.DodgerBlue;
                public Color ButonBorderMouseOverColor
            {
                get { return buttonBorderMouseOverColor; }
                set
                {
                    if (buttonBorderMouseOverColor != value)
                    {
                        buttonBorderMouseOverColor = value;
                        Invalidate();
                    }
                }
            }

            private Color arrowMouseOverColor = Color.DodgerBlue;
            public Color ArrowMouseOverColor
            {
                get { return arrowMouseOverColor; }
                set
                {
                    if (arrowMouseOverColor != value)
                    {
                        arrowMouseOverColor = value;
                        Invalidate();
                    }
                }
            }
            private Color selectMouseOverColor = Color.FromArgb(229, 243, 255);
            public Color SelectMouseOverColor
            {
                get { return selectMouseOverColor; }
                set
                {
                    if (selectMouseOverColor != value)
                    {
                        selectMouseOverColor = value;
                        Invalidate();
                    }
                }
            }
            private Color selectedColor = Color.FromArgb(204, 232, 255);
            public Color SelectedColor
            {
                get { return selectedColor; }
                set
                {
                    if (selectedColor != value)
                    {
                        selectedColor = value;
                        Invalidate();
                    }
                }
            }
            private Color todayButtonBackColor = SystemColors.Control;
            public Color TodayButtonBackColor
            {
                get { return todayButtonBackColor; }
                set
                {
                    if (todayButtonBackColor != value)
                    {
                        todayButtonBackColor = value;
                        Invalidate();
                    }
                }
            }
            private DateTime actualDate = DateTime.Now;
            public DateTime ActualDate
            {
                get { return actualDate; }
                set
                {
                    if (actualDate != value)
                    {
                        actualDate = value;
                        Invalidate();
                    }
                }
            }
            private DateTime selectedDate = DateTime.Now;
            public DateTime SelectedDate
            {
                get { return selectedDate; }
                set
                {
                    if (selectedDate != value)
                    {
                        selectedDate = value;
                        Invalidate();
                    }
                }
            }

            DatePicker dp;
            public DatePicker Parent
            {
                get { return dp; }
                set { dp = value; }
            }

            private bool isOpen = false;
            public bool IsOpen
            {
                get { return isOpen; }
                set { isOpen= value; }
            }

            public enum View
            {
                Days = 1,
                Months = 2,
                Decades = 3,
                Centuries = 4
            }
            private View view = Calendar.View.Days;
            public View CurrentView
            {
                get { return view; }
                set
                {
                    if (value != view)
                        view = value;
                    Invalidate();
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
                //base.OnLostFocus(e);
                //this.Hide();
                CurrentView = View.Days;
                ActualDate = SelectedDate;
                if (Parent != null)
                {
                    this.Hide();
                    this.isOpen = false;
                    Parent.lastFocusLost = DateTime.Now;
                }
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                if(e.Button == MouseButtons.Left)
                {
                    if (header != null && header.Contains(e.Location) && (int)CurrentView < 4)
                            CurrentView++;
                    else if (buttonL != null && buttonL.Contains(e.Location))
                    {
                        switch(CurrentView)
                        {
                            case View.Days: ActualDate = ActualDate.AddMonths(-1); break;
                            case View.Months: ActualDate = ActualDate.AddYears(-1); break;
                            case View.Decades: ActualDate = ActualDate.AddYears(-10); break;
                            case View.Centuries: ActualDate = ActualDate.AddYears(-100); break;
                        }
                    }
                    else if (buttonR != null && buttonR.Contains(e.Location))
                    {
                        switch (CurrentView)
                        {
                            case View.Days: ActualDate = ActualDate.AddMonths(1); break;
                            case View.Months: ActualDate = ActualDate.AddYears(1); break;
                            case View.Decades: ActualDate = ActualDate.AddYears(10); break;
                            case View.Centuries: ActualDate = ActualDate.AddYears(100); break;
                        }
                    }
                    Invalidate();
                }
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                if (buttonL.Contains(e.Location) && !_mouseInLB)
                {
                    _mouseInLB = true;
                    Invalidate();
                }
                else if (_mouseInLB && !buttonL.Contains(e.Location))
                {
                    _mouseInLB = false;
                    Invalidate();
                }

                if (buttonR.Contains(e.Location) && !_mouseInRB)
                {
                    _mouseInRB = true;
                    Invalidate();
                }
                else if (_mouseInRB && !buttonR.Contains(e.Location))
                {
                    _mouseInRB = false;
                    Invalidate();
                }

                if (header.Contains(e.Location) && !_mouseInHeader)
                {
                    _mouseInHeader = true;
                    Invalidate();
                }
                else if (_mouseInHeader && !header.Contains(e.Location))
                {
                    _mouseInHeader = false;
                    Invalidate();
                }
            }

            //protected override bool ShowWithoutActivation => true;

            protected override void OnPaint(PaintEventArgs e)
            {
                //base.OnPaint(e);
                header = new Rectangle(16, 1, Width - 32, 29);
                buttonL = new Rectangle(1, 1, 14, 28);
                buttonR = new Rectangle(Width - 16, 1, 14, 28);

                using (Graphics g = e.Graphics)
                {
                    Rectangle drawArea = new Rectangle(0, 0, Width - 1, Height - 1);
                    Point middleL = new Point(buttonL.Left + buttonL.Width / 2,
                    buttonL.Top + buttonL.Height / 2);
                    Point middleR = new Point(buttonR.Left + buttonR.Width / 2,
                    buttonR.Top + buttonR.Height / 2);
                    Point[] arrowL = new Point[]
                    {
                        new Point(middleL.X + 2, middleL.Y - 4),
                        new Point(middleL.X + 2, middleL.Y + 4),
                        new Point(middleL.X - 2, middleL.Y)
                    };
                    Point[] arrowR = new Point[]
                    {
                        new Point(middleR.X - 1, middleR.Y - 4),
                        new Point(middleR.X - 1, middleR.Y + 4),
                        new Point(middleR.X + 3, middleR.Y)
                    };

                    //pozadí
                    using (SolidBrush b = new SolidBrush(BackgroundColor))
                    {
                        g.FillPath(b, RoundedRect(drawArea, 3, 3, 3, 3));
                    }

                    //header
                    using (SolidBrush b = new SolidBrush(_mouseInHeader ? HeaderMouseOverColor : HeaderColor))
                    {
                        g.FillRectangle(b, header);
                    }

                    string mr = "";
                    switch (CurrentView)
                    {
                        case View.Days: mr = ActualDate.ToString("MMMM yyyy"); break;
                        case View.Months: mr = ActualDate.ToString("yyyy"); break;
                        case View.Decades: mr = ActualDate.Year / 10 * 10 + " - " + (ActualDate.Year / 10) + 9; break;
                        case View.Centuries: mr = ActualDate.Year / 100 * 100 + " - " + (ActualDate.Year / 100) + 99; break;
                    }
                    Size mrSize = TextRenderer.MeasureText(mr, Font);
                    TextRenderer.DrawText(g, mr, Font, new Point((header.Width / 2) - (mrSize.Width / 2) + header.Location.X, 15 - (mrSize.Height / 2) + header.Location.Y), _mouseInHeader ? HeaderMouseOverForeColor : HeaderForeColor);

                    //rámeček
                    using (Pen p = new Pen(BorderColor, 1))
                    {
                        g.DrawPath(p, RoundedRect(drawArea, 3, 3, 3, 3));
                    }

                    //tlačítka
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? buttonMoseOverColor : ButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInLB ? buttonBorderMouseOverColor : ButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? ArrowMouseOverColor : ButtonArrowColor))
                    {
                        g.FillPolygon(b, arrowL);
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? buttonMoseOverColor : ButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInRB ? buttonBorderMouseOverColor : ButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? ArrowMouseOverColor : ButtonArrowColor))
                    {
                        g.FillPolygon(b, arrowR);
                    }

                    //názvy dnů - ještě posunout na střed rect ty názvy
                    Rectangle poRect = new Rectangle(2, header.Bottom, 20, 15);
                    string po = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Monday);
                    TextRenderer.DrawText(g, po, Font, poRect, DayHeaderForeColor);
                    Rectangle utRect = new Rectangle(poRect.Right, header.Bottom, 20, 15);
                    string ut = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Tuesday);
                    TextRenderer.DrawText(g, ut, Font, utRect, DayHeaderForeColor);
                    Rectangle stRect = new Rectangle(utRect.Right, header.Bottom, 20, 15);
                    string st = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Wednesday);
                    TextRenderer.DrawText(g, st, Font, stRect, DayHeaderForeColor);
                    Rectangle ctRect = new Rectangle(stRect.Right, header.Bottom, 20, 15);
                    string ct = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Thursday);
                    TextRenderer.DrawText(g, ct, Font, ctRect, DayHeaderForeColor);
                    Rectangle paRect = new Rectangle(ctRect.Right, header.Bottom, 20, 15);
                    string pa = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Friday);
                    TextRenderer.DrawText(g, pa, Font, paRect, DayHeaderForeColor);
                    Rectangle soRect = new Rectangle(paRect.Right, header.Bottom, 20, 15);
                    string so = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Saturday);
                    TextRenderer.DrawText(g, so, Font, soRect, DayHeaderForeColor);
                    Rectangle neRect = new Rectangle(soRect.Right, header.Bottom, 20, 15);
                    string ne = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Sunday);
                    TextRenderer.DrawText(g, ne, Font, neRect, DayHeaderForeColor);
                }

                //měsíc/rok - 146*30
                //hlavička dnů 146*13 - 13. řádek je dělící čára
                //dny 146*90, 6 řádků dnů, jeden den má 20*15
                //položka dnes 146*20

                //rok mřížka 4*3
                //měsíce 35*35

                //desetiletí stejné jako měsíce.první a poslední je trailing z xxx9 a xxx0

                //stejně řešeno i století

                //animace je řešena do aktuálního čtverce výběru, cca 1s
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


            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                    //cp.ClassStyle |= 0x00020000; //Win32Message.CS_DROPSHADOW
                    return cp;
                }
            }
        }
    }
}
