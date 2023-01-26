using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;
using System.Globalization;

namespace Ticketník.CustomControls
{
    internal partial class DateTimePicker
    {
        protected sealed class Calendar : System.Windows.Forms.Form
        {
            Rectangle header;
            Rectangle buttonL;
            Rectangle buttonR;
            Rectangle todayRect;
            Rectangle poRect;
            Rectangle r1;
            Rectangle _lastActiveRect;

            bool _mouseInLB = false;
            bool _mouseInRB = false;
            bool _mouseInHeader = false;
            bool _mouseInTB = false;
            bool _mouseDown = false;

            int _mouseInCal = -1;

            KeyValuePair<Rectangle,DateTime?>[] dny = new KeyValuePair<Rectangle, DateTime?>[42];
            KeyValuePair<Rectangle, DateTime?>[] mesice = new KeyValuePair<Rectangle, DateTime?>[12];

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
            }/*
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
            }*/
            private Color todayButtonColor = Color.FromArgb(0, 102, 204);
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
            /*private Color dayListForeColor = Color.Black;
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
            }*/
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
            private Color todayButtonMouseOverForeColor = Color.Black;
            public Color TodayButtonMouseOverForeColor
            {
                get { return todayButtonMouseOverForeColor; }
                set
                {
                    if (todayButtonMouseOverForeColor != value)
                    {
                        todayButtonMouseOverForeColor = value;
                        Invalidate();
                    }
                }
            }
            private Color selectedDayBorderColor = Color.DodgerBlue;
            public Color SelectedDayBorderColor
            {
                get { return selectedDayBorderColor; }
                set
                {
                    if (selectedDayBorderColor != value)
                    {
                        selectedDayBorderColor = value;
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
            private Color selectedMouseOverForeColor = Color.DodgerBlue;
            public Color SelectedMouseOverForeColor
            {
                get { return selectedMouseOverForeColor; }
                set
                {
                    if (selectedMouseOverForeColor != value)
                    {
                        selectedMouseOverForeColor = value;
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
            private Color buttonMouseOverColor = Color.White;//Color.FromArgb(229, 243, 255);
            public Color ButonMouseOverColor
            {
                get { return buttonMouseOverColor; }
                set
                {
                    if (buttonMouseOverColor != value)
                    {
                        buttonMouseOverColor = value;
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
            private Color todayButtonBackColor = Color.White;
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

            DateTimePicker dp;
            public DateTimePicker Parent
            {
                get { return dp; }
                internal set { dp = value; }
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
                for(int i = 0; i<42;i++)
                {
                    int radek = (i / 7) * 15;
                    int sloupec = (i % 7) * 20;
                    dny[i] = new KeyValuePair<Rectangle, DateTime?>(new Rectangle(sloupec,radek, 19, 14), null);
                }
                for (int i = 0; i < 12; i++)
                {
                    int radek = (i / 4) * 35;
                    int sloupec = (i % 4) * 35;
                    mesice[i] = new KeyValuePair<Rectangle, DateTime?>(new Rectangle(sloupec, radek, 34, 34), null);
                }
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
                    Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                }
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                if(!_mouseDown)
                {
                    _mouseDown = true;
                    if (e.Button == MouseButtons.Left)
                    {
                        if (header != null && header.Contains(e.Location) && (int)CurrentView < 4)
                            CurrentView++;
                        else if (buttonL != null && buttonL.Contains(e.Location))
                        {
                            switch (CurrentView)
                            {
                                case View.Days: if (ActualDate.Year > Parent.MinDate.Year)
                                    {
                                        ActualDate = ActualDate.AddMonths(-1);
                                    }
                                    else if(ActualDate.Month > Parent.MinDate.Month)
                                    {
                                        ActualDate = ActualDate.AddMonths(-1);
                                    }
                                    break;
                                case View.Months: 
                                    if (ActualDate.Year > Parent.MinDate.Year) 
                                    { 
                                        ActualDate = ActualDate.AddYears(-1); 
                                    } 
                                    break;
                                case View.Decades:
                                    if (ActualDate.Year > Parent.MinDate.Year)
                                    {
                                        ActualDate = ActualDate.AddYears(-10);
                                    }
                                    break;
                                case View.Centuries:
                                    if (ActualDate.Year > Parent.MinDate.Year)
                                    {
                                        ActualDate = ActualDate.AddYears(-100);
                                    }
                                    break;
                            }
                        }
                        else if (buttonR != null && buttonR.Contains(e.Location))
                        {
                            switch (CurrentView)
                            {
                                case View.Days:
                                    if (ActualDate.Year < Parent.MaxDate.Year)
                                    {
                                        ActualDate = ActualDate.AddMonths(1);
                                    }
                                    else if (ActualDate.Month < Parent.MaxDate.Month)
                                    {
                                        ActualDate = ActualDate.AddMonths(1);
                                    }
                                    break;
                                case View.Months:
                                    if (ActualDate.Year < Parent.MaxDate.Year)
                                    {
                                        ActualDate = ActualDate.AddYears(1);
                                    }
                                    break;
                                case View.Decades:
                                    if (ActualDate.Year < Parent.MaxDate.Year)
                                    {
                                        ActualDate = ActualDate.AddYears(10);
                                    }
                                    break;
                                case View.Centuries:
                                    if (ActualDate.Year < Parent.MaxDate.Year)
                                    {
                                        ActualDate = ActualDate.AddYears(100);
                                    }
                                    break;
                            }
                        }
                        else if (todayRect != null && todayRect.Contains(e.Location))
                        {
                            if (CurrentView != View.Days || (SelectedDate.Day != DateTime.Today.Day || SelectedDate.Month != DateTime.Today.Month || SelectedDate.Year != DateTime.Today.Year))
                            {
                                CurrentView = View.Days;
                                ActualDate = SelectedDate = DateTime.Today;
                                ValueChanged?.Invoke(this, EventArgs.Empty);
                            }
                        }
                        else if (CurrentView == View.Days)
                        {
                            for (int i = 0; i < dny.Count(); i++)
                            //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                            {
                                Rectangle referenceRect = new Rectangle(dny[i].Key.Left + 3, dny[i].Key.Top + poRect.Bottom + 3, dny[i].Key.Width, dny[i].Key.Height);
                                if (referenceRect.Contains(e.Location))
                                {
                                    if (SelectedDate.Day != ((DateTime)dny[i].Value).Day || SelectedDate.Month != ((DateTime)dny[i].Value).Month || SelectedDate.Year != ((DateTime)dny[i].Value).Year)
                                    {
                                        ActualDate = SelectedDate = (DateTime)dny[i].Value;
                                        ValueChanged?.Invoke(this, EventArgs.Empty);
                                    }
                                    break;
                                }
                            }
                        }
                        else if (CurrentView == View.Months || CurrentView == View.Decades || CurrentView == View.Centuries)
                        {
                            for (int i = 0; i < mesice.Count(); i++)
                            //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                            {
                                Rectangle referenceRect = new Rectangle(mesice[i].Key.Left + 3, mesice[i].Key.Top + header.Bottom + 1, mesice[i].Key.Width, mesice[i].Key.Height);
                                if (referenceRect.Contains(e.Location))
                                {
                                    _mouseInCal = -1;
                                    _mouseInHeader = false;
                                    _mouseInTB = false;
                                    _mouseInRB = false;
                                    _mouseInLB = false;
                                    ActualDate = (DateTime)mesice[i].Value;
                                    CurrentView -= 1;

                                    break;
                                }
                            }
                        }
                        Invalidate();
                    }
                }
            }
            protected override void OnMouseUp(MouseEventArgs e)
            {
                base.OnMouseUp(e);
                if(_mouseDown)
                    _mouseDown= false;
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);
                if (buttonL.Contains(e.Location))
                {
                    if (!_mouseInLB)
                    {
                        _mouseInLB = true;
                        _mouseInTB = false;
                        _mouseInRB = false;
                        _mouseInHeader = false;
                        _mouseInCal = -1;
                        Invalidate(buttonL);
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        _lastActiveRect = buttonL;
                    }
                }
                else if (buttonR.Contains(e.Location))
                {
                    if (!_mouseInRB)
                    {
                        _mouseInRB = true;
                        _mouseInTB = false;
                        _mouseInLB = false;
                        _mouseInHeader = false;
                        _mouseInCal = -1;
                        Invalidate(buttonR);
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        _lastActiveRect = buttonR;
                    }
                }
                else if (header.Contains(e.Location))
                {
                    if (!_mouseInHeader)
                    {
                        _mouseInHeader = true;
                        _mouseInTB = false;
                        _mouseInRB = false;
                        _mouseInLB = false;
                        _mouseInCal = -1;
                        Invalidate(header);
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        _lastActiveRect = header;
                    }
                }
                else if (todayRect.Contains(e.Location))
                {
                    if (!_mouseInTB)
                    {
                        _mouseInTB = true;
                        _mouseInHeader = false;
                        _mouseInRB = false;
                        _mouseInLB = false;
                        _mouseInCal = -1;
                        Invalidate(todayRect);
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        _lastActiveRect = todayRect;
                    }
                }
                else if (CurrentView == View.Days)
                {
                    for (int i = 0; i < dny.Count(); i++)
                    //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                    {
                        Rectangle referenceRect = new Rectangle(dny[i].Key.Left + 3, dny[i].Key.Top + poRect.Bottom + 3, dny[i].Key.Width, dny[i].Key.Height);
                        if (referenceRect.Contains(e.Location))
                        {
                            if (_mouseInCal != i)
                            {
                                _mouseInCal = i;
                                _mouseInHeader = false;
                                _mouseInTB = false;
                                _mouseInRB = false;
                                _mouseInLB = false;
                                Invalidate(referenceRect);
                                if (_lastActiveRect != null)
                                    Invalidate(_lastActiveRect);
                                _lastActiveRect = referenceRect;
                            }
                            return;
                        }
                    }

                    if (_mouseInCal != -1 || _mouseInHeader || _mouseInLB || _mouseInRB || _mouseInTB)
                    {
                        _mouseInCal = -1;
                        _mouseInHeader = false;
                        _mouseInTB = false;
                        _mouseInRB = false;
                        _mouseInLB = false; 
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        return;
                    }
                }
                else if (CurrentView == View.Months || CurrentView == View.Decades || CurrentView == View.Centuries)
                {
                    for (int i = 0; i < mesice.Count(); i++)
                    //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                    {
                        Rectangle referenceRect = new Rectangle(mesice[i].Key.Left + 3, mesice[i].Key.Top + header.Bottom + 1, mesice[i].Key.Width, mesice[i].Key.Height);
                        if (referenceRect.Contains(e.Location))
                        {
                            if (_mouseInCal != i)
                            {
                                _mouseInCal = i;
                                _mouseInHeader = false;
                                _mouseInTB = false;
                                _mouseInRB = false;
                                _mouseInLB = false;
                                Invalidate(referenceRect);
                                if (_lastActiveRect != null)
                                    Invalidate(_lastActiveRect);
                                _lastActiveRect = referenceRect;
                            }
                            return;
                        }
                    }

                    if (_mouseInCal != -1 || _mouseInHeader || _mouseInLB || _mouseInRB || _mouseInTB)
                    {
                        _mouseInCal = -1;
                        _mouseInHeader = false;
                        _mouseInTB = false;
                        _mouseInRB = false;
                        _mouseInLB = false;
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                        return;
                    }
                }
                else
                {
                    if (_mouseInCal != -1 || _mouseInHeader || _mouseInLB || _mouseInRB || _mouseInTB)
                    {
                        _mouseInCal = -1;
                        _mouseInHeader = false;
                        _mouseInTB = false;
                        _mouseInRB = false;
                        _mouseInLB = false;
                        if (_lastActiveRect != null)
                            Invalidate(_lastActiveRect);
                    }
                }
            }

            //protected override bool ShowWithoutActivation => true;

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if(!this.Visible)
                {
                    _mouseInLB = false;
                    _mouseInRB = false;
                    _mouseInHeader = false;
                    _mouseInTB = false;
                    _mouseDown = false;

                    _mouseInCal = -1;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                //udělat double bouffered !!!!!!
                //Doubloe buffered grafika divně problikává, zůstávám u composited.
                //base.OnPaint(e);
                header = new Rectangle(16, 1, Width - 33, 28);
                buttonL = new Rectangle(1, 1, 14, 28);
                buttonR = new Rectangle(Width - 16, 1, 14, 28);
                todayRect = new Rectangle(1, Height - 22, Width - 3, 20);

                DateTime refMinDate = new DateTime(Parent.MinDate.Year, Parent.MinDate.Month, Parent.MinDate.Day);
                DateTime refMaxDate = new DateTime(Parent.MaxDate.Year, Parent.MaxDate.Month, Parent.MaxDate.Day);

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
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? ButonMouseOverColor : ButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInLB ? ButonBorderMouseOverColor : ButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? ArrowMouseOverColor : ButtonArrowColor))
                    {
                        g.FillPolygon(b, arrowL);
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? ButonMouseOverColor : ButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInRB ? ButonBorderMouseOverColor : ButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? ArrowMouseOverColor : ButtonArrowColor))
                    {
                        g.FillPolygon(b, arrowR);
                    }

                    if (CurrentView == View.Days)
                    {
                        //názvy dnů - ještě posunout na střed rect ty názvy
                        poRect = new Rectangle(3, header.Bottom + 1, 20, 13);
                        string po = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Monday);
                        TextRenderer.DrawText(g, po, Font, poRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle utRect = new Rectangle(poRect.Right, header.Bottom + 1, 20, 13);
                        string ut = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Tuesday);
                        TextRenderer.DrawText(g, ut, Font, utRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle stRect = new Rectangle(utRect.Right, header.Bottom + 1, 20, 13);
                        string st = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Wednesday);
                        TextRenderer.DrawText(g, st, Font, stRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle ctRect = new Rectangle(stRect.Right, header.Bottom + 1, 20, 13);
                        string ct = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Thursday);
                        TextRenderer.DrawText(g, ct, Font, ctRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle paRect = new Rectangle(ctRect.Right, header.Bottom + 1, 20, 13);
                        string pa = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Friday);
                        TextRenderer.DrawText(g, pa, Font, paRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle soRect = new Rectangle(paRect.Right, header.Bottom + 1, 20, 13);
                        string so = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Saturday);
                        TextRenderer.DrawText(g, so, Font, soRect, DayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle neRect = new Rectangle(soRect.Right, header.Bottom + 1, 20, 13);
                        string ne = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Sunday);
                        TextRenderer.DrawText(g, ne, Font, neRect, DayHeaderForeColor, TextFormatFlags.Right);

                        //dělící čára
                        using (Pen ps = new Pen(SeparatorColor, 1))
                        {
                            g.DrawLine(ps, 1, poRect.Bottom + 1, Width - 2, poRect.Bottom + 1);
                        }

                        //dny
                        DateTime tmp = new DateTime(ActualDate.Year, ActualDate.Month, 1);

                        int prvniDenMesice = ((int)tmp.DayOfWeek + 6) % 7;
                        if (prvniDenMesice == 0)
                            prvniDenMesice = 7;
                        tmp = tmp.AddDays(-prvniDenMesice);
                        for (int ii = 0; ii < dny.Count(); ii++)
                        //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                        {
                            if (tmp < refMinDate || tmp > refMaxDate)
                            {
                                tmp = tmp.AddDays(1);
                                continue;
                            }
                            dny[ii] = new KeyValuePair<Rectangle, DateTime?>(dny[ii].Key, tmp);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            if (tmp.Day == SelectedDate.Day && tmp.Month == SelectedDate.Month && tmp.Year == SelectedDate.Year)
                            {
                                using (SolidBrush b = new SolidBrush(SelectedColor))
                                {
                                    g.FillPath(b, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                                using (Pen p = new Pen(SelectedDayBorderColor, 1))
                                {
                                    g.DrawPath(p, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                            }
                            else if (ii == _mouseInCal)
                            {
                                using (SolidBrush b = new SolidBrush(SelectMouseOverColor))
                                {
                                    g.FillPath(b, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                            }
                            g.SmoothingMode = SmoothingMode.None;
                            string datum = tmp.ToString("%d");
                            Size ds = TextRenderer.MeasureText(datum, Font);
                            if (tmp < Parent.MaxDate || tmp > Parent.MinDate)
                            {
                                TextRenderer.DrawText(g, datum, Font, new Point(dny[ii].Key.Left + 13 - (ds.Width / 2), dny[ii].Key.Top + 10 - (ds.Height / 2) + poRect.Bottom),
                                    ActualDate.Month == tmp.Month ? ForeColor : TrailingForeColor);

                                tmp = tmp.AddDays(1);
                            }
                        }
                    }
                    else
                    {
                        int year = 0;
                        int modifier = 0;
                        switch (CurrentView)
                        {
                            case View.Months: year = ActualDate.Year; break;
                            case View.Decades: year = ActualDate.Year - (ActualDate.Year % 10) - 1; modifier = 1; break;
                            case View.Centuries: year = ActualDate.Year - (ActualDate.Year % 100) - 1; modifier = 10; break;
                            default: year = ActualDate.Year; break;
                        }
                        DateTime tmp = new DateTime(year, 1, 1);
                        for (int ii = 0; ii < mesice.Count(); ii++)
                        //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                        {
                            if (tmp < refMinDate || tmp > refMaxDate)
                            {
                                if(CurrentView == View.Months)
                                    tmp = tmp.AddMonths(1);
                                else
                                    tmp = tmp.AddYears(modifier);
                                continue;
                            }
                            mesice[ii] = new KeyValuePair<Rectangle, DateTime?>(mesice[ii].Key, tmp);

                            if (CurrentView == View.Months)
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (tmp.Month == SelectedDate.Month && tmp.Year == SelectedDate.Year)
                                {
                                    using (SolidBrush b = new SolidBrush(SelectedColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                    using (Pen p = new Pen(SelectedDayBorderColor, 1))
                                    {
                                        g.DrawPath(p, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                else if (ii == _mouseInCal)
                                {
                                    using (SolidBrush b = new SolidBrush(SelectMouseOverColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                g.SmoothingMode = SmoothingMode.None;
                                string datum = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(tmp.Month);//tmp.ToString("%d");
                                Size ds = TextRenderer.MeasureText(datum, Font);
                                if (tmp < Parent.MaxDate && tmp > Parent.MinDate)
                                {
                                    TextRenderer.DrawText(g, datum, Font, new Point(mesice[ii].Key.Left + 20 - (ds.Width / 2), mesice[ii].Key.Top + 18 - (ds.Height / 2) + header.Bottom),
                                    ForeColor);

                                    tmp = tmp.AddMonths(1);
                                }
                            }
                            else
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (tmp.Year - (tmp.Year % modifier) == SelectedDate.Year - (SelectedDate.Year % modifier))
                                {
                                    using (SolidBrush b = new SolidBrush(SelectedColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                    using (Pen p = new Pen(SelectedDayBorderColor, 1))
                                    {
                                        g.DrawPath(p, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                else if (ii == _mouseInCal)
                                {
                                    using (SolidBrush b = new SolidBrush(SelectMouseOverColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                g.SmoothingMode = SmoothingMode.None;
                                string datum = tmp.Year.ToString();
                                if (CurrentView == View.Centuries || CurrentView == View.Centuries)
                                {
                                    datum = tmp.Year / modifier * modifier + " -\r\n" + (tmp.Year / modifier) + 9;
                                }
                                Size ds = TextRenderer.MeasureText(datum, Font);
                                TextRenderer.DrawText(g, datum, Font, new Point(mesice[ii].Key.Left + 20 - (ds.Width / 2), mesice[ii].Key.Top + 18 - (ds.Height / 2) + header.Bottom),
                                (ii == 0 || ii == 11) ? TrailingForeColor : ForeColor);

                                if (tmp.Year > Parent.MinDate.Year && tmp.Year < Parent.MaxDate.Year)
                                    tmp = tmp.AddYears(modifier);

                            }
                        }

                    }

                    //tlačítko Dnes
                    Rectangle todayBlueRect = new Rectangle(todayRect.Width / 2 - 5, todayRect.Top + 4, 17, 12);
                    using (SolidBrush b = new SolidBrush(TodayButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(todayRect, 0, 0, 2, 2));
                    }
                    string datumDnes = DateTime.Now.ToString("%d.M.yyyy");
                    Size datumDnesVelikost = TextRenderer.MeasureText(datumDnes, Font);
                    todayBlueRect.X -= (datumDnesVelikost.Width / 2);
                    int centerV = todayRect.Top + (todayRect.Height / 2) - datumDnesVelikost.Height / 2;
                    int centerH = todayRect.Right - (todayRect.Width / 2) - datumDnesVelikost.Width / 2;
                    TextRenderer.DrawText(g, datumDnes, Font, new Point(centerH + 13, centerV),
                        _mouseInTB ? TodayButtonMouseOverForeColor : TodayButtonForeColor);

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawPath(new Pen(TodayButtonColor, 1), RoundedRect(todayBlueRect, 2, 2, 2, 2));
                    g.SmoothingMode = SmoothingMode.None;
                }

                //měsíc/rok - 146*30
                //hlavička dnů 146*13 - 13. řádek je dělící čára
                //dny 146*90, 6 řádků dnů, jeden den má 20*15
                //položka dnes 146*20, čtvereček 18*13

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

            internal event EventHandler ValueChanged;

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
