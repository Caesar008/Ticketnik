﻿using System;
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
            Rectangle _lastActiveRect;

            bool _mouseInLB = false;
            bool _mouseInRB = false;
            bool _mouseInHeader = false;
            bool _mouseInTB = false;
            bool _mouseDown = false;

            int _mouseInCal = -1;

            KeyValuePair<Rectangle,DateTime?>[] dny = new KeyValuePair<Rectangle, DateTime?>[42];
            KeyValuePair<Rectangle, DateTime?>[] mesice = new KeyValuePair<Rectangle, DateTime?>[12];

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
                get { return /*selectedDate*/Parent.Value; }
                set
                {
                    if (/*selectedDate*/Parent.Value != value)
                    {
                        Parent.Value = ActualDate = value;//selectedDate = value;
                        Invalidate();
                    }
                }
            }

            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                if (keyData == Keys.Enter || keyData == Keys.Escape)
                {
                    CurrentView = View.Days;
                    if(keyData == Keys.Enter)
                        ActualDate = SelectedDate;
                    if (Parent != null)
                    {
                        this.Hide();
                        this.isOpen = false;
                        Parent.lastFocusLost = DateTime.Now;
                        Parent.CloseUp?.Invoke(Parent, EventArgs.Empty);
                        this.Close();
                    }
                }
                return true;
            }

            DateTimePicker dp;
            new public DateTimePicker Parent
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
            public Calendar(CustomControls.DateTimePicker dateTimePicker)
            {
                this.Parent = dateTimePicker;
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
                    this.Close();
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
                                    if (ActualDate.Year - (ActualDate.Year % 10) > Parent.MinDate.Year - (Parent.MinDate.Year % 10))
                                    {
                                        ActualDate = ActualDate.AddYears(-10);
                                    }
                                    break;
                                case View.Centuries:
                                    if (ActualDate.Year - (ActualDate.Year % 100) > Parent.MinDate.Year - (Parent.MinDate.Year % 100))
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
                                    if (ActualDate.Year - (ActualDate.Year % 10) < Parent.MaxDate.Year - (Parent.MaxDate.Year % 10))
                                    {
                                        ActualDate = ActualDate.AddYears(10);
                                    }
                                    break;
                                case View.Centuries:
                                    if (ActualDate.Year - (ActualDate.Year % 100) < Parent.MaxDate.Year - (Parent.MaxDate.Year % 100)/*ActualDate.Year < Parent.MaxDate.Year*/)
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
                                ActualDate = SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                                    Parent.Value.Hour, Parent.Value.Minute, Parent.Value.Second, Parent.Value.Millisecond);//DateTime.Today;
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
                                    if (dny[i].Value != null)
                                    {
                                        if (SelectedDate.Day != ((DateTime)dny[i].Value).Day || SelectedDate.Month != ((DateTime)dny[i].Value).Month || SelectedDate.Year != ((DateTime)dny[i].Value).Year)
                                        {
                                            ActualDate = SelectedDate = (DateTime)dny[i].Value;
                                            ValueChanged?.Invoke(this, EventArgs.Empty);
                                        }
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
                                    if (mesice[i].Value != null)
                                    {
                                        _mouseInCal = -1;
                                        _mouseInHeader = false;
                                        _mouseInTB = false;
                                        _mouseInRB = false;
                                        _mouseInLB = false;
                                        ActualDate = (DateTime)mesice[i].Value;
                                        CurrentView -= 1;
                                    }
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
            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);

                if (!FitDown(this.Location.Y))
                {
                    this.Location = new Point(this.Location.X, this.Location.Y - this.Height - Parent.Height);
                }
            }

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
                else
                {
                    if (!FitDown(this.Location.Y))
                    {
                        this.Location = new Point(this.Location.X, this.Location.Y - this.Height - Parent.Height);
                    }
                }
            }

            internal bool FitDown(int proposedYCoord)
            {
                int height = Screen.FromHandle(this.Handle).WorkingArea.Height - 1;
                if (proposedYCoord + this.Height > height)
                    return false;
                return true;
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
                    using (SolidBrush b = new SolidBrush(Parent.MonthBackColor))
                    {
                        g.FillPath(b, RoundedRect(drawArea, 3, 3, 3, 3));
                    }

                    //header
                    using (SolidBrush b = new SolidBrush(_mouseInHeader ? Parent.MonthHeaderMouseOverBackColor : Parent.MonthHeaderBackColor))
                    {
                        g.FillRectangle(b, header);
                    }

                    string mr = "";
                    int minY;
                    switch (CurrentView)
                    {
                        case View.Days: mr = ActualDate.ToString("MMMM yyyy"); break;
                        case View.Months: mr = ActualDate.ToString("yyyy"); break;
                        case View.Decades:
                            minY = ActualDate.Year / 10 * 10;
                            if (minY < refMinDate.Year)
                                minY = refMinDate.Year;
                            mr = minY + " - " + (ActualDate.Year / 10) + 9; 
                            break;
                        case View.Centuries:
                            minY = ActualDate.Year / 100 * 100;
                            if (minY < refMinDate.Year)
                                minY = refMinDate.Year;
                            mr = minY + " - " + (ActualDate.Year / 100) + 99; break;
                    }
                    Size mrSize = TextRenderer.MeasureText(mr, Font);
                    TextRenderer.DrawText(g, mr, Font, new Point((header.Width / 2) - (mrSize.Width / 2) + header.Location.X, 15 - (mrSize.Height / 2) + header.Location.Y), _mouseInHeader ? Parent.MonthHeaderMouseOverForeColor : Parent.MonthHeaderForeColor);

                    //rámeček
                    using (Pen p = new Pen(Parent.MonthBorderColor, 1))
                    {
                        g.DrawPath(p, RoundedRect(drawArea, 3, 3, 3, 3));
                    }

                    //tlačítka
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? Parent.MonthButtonMouseOverColor : Parent.MonthBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInLB ? Parent.MonthButtonBorderMouseOverColor : Parent.MonthButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonL, 2, 0, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInLB ? Parent.MonthButtonMouseOverForeColor : Parent.MonthButtonForeColor))
                    {
                        g.FillPolygon(b, arrowL);
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? Parent.MonthButtonMouseOverColor : Parent.MonthBackColor))
                    {
                        g.FillPath(b, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (Pen pb = new Pen(_mouseInRB ? Parent.MonthButtonBorderMouseOverColor : Parent.MonthButtonBorderColor, 1))
                    {
                        g.DrawPath(pb, RoundedRect(buttonR, 0, 2, 0, 0));
                    }
                    using (SolidBrush b = new SolidBrush(_mouseInRB ? Parent.MonthButtonMouseOverForeColor : Parent.MonthButtonForeColor))
                    {
                        g.FillPolygon(b, arrowR);
                    }

                    if (CurrentView == View.Days)
                    {
                        //názvy dnů - ještě posunout na střed rect ty názvy
                        poRect = new Rectangle(3, header.Bottom + 1, 20, 13);
                        string po = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Monday);
                        TextRenderer.DrawText(g, po, Font, poRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle utRect = new Rectangle(poRect.Right, header.Bottom + 1, 20, 13);
                        string ut = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Tuesday);
                        TextRenderer.DrawText(g, ut, Font, utRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle stRect = new Rectangle(utRect.Right, header.Bottom + 1, 20, 13);
                        string st = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Wednesday);
                        TextRenderer.DrawText(g, st, Font, stRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle ctRect = new Rectangle(stRect.Right, header.Bottom + 1, 20, 13);
                        string ct = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Thursday);
                        TextRenderer.DrawText(g, ct, Font, ctRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle paRect = new Rectangle(ctRect.Right, header.Bottom + 1, 20, 13);
                        string pa = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Friday);
                        TextRenderer.DrawText(g, pa, Font, paRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle soRect = new Rectangle(paRect.Right, header.Bottom + 1, 20, 13);
                        string so = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Saturday);
                        TextRenderer.DrawText(g, so, Font, soRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);
                        Rectangle neRect = new Rectangle(soRect.Right, header.Bottom + 1, 20, 13);
                        string ne = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(DayOfWeek.Sunday);
                        TextRenderer.DrawText(g, ne, Font, neRect, Parent.MonthDayHeaderForeColor, TextFormatFlags.Right);

                        //dělící čára
                        using (Pen ps = new Pen(Parent.MonthSeparatorColor, 1))
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
                                dny[ii] = new KeyValuePair<Rectangle, DateTime?>(dny[ii].Key, null);
                                tmp = tmp.AddDays(1);
                                continue;
                            }
                            dny[ii] = new KeyValuePair<Rectangle, DateTime?>(dny[ii].Key, tmp);
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            if (tmp.Day == SelectedDate.Day && tmp.Month == SelectedDate.Month && tmp.Year == SelectedDate.Year)
                            {
                                using (SolidBrush b = new SolidBrush(Parent.MonthSelectedColor))
                                {
                                    g.FillPath(b, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                                using (Pen p = new Pen(Parent.MonthSelectedDayBorderColor, 1))
                                {
                                    g.DrawPath(p, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                            }
                            else if (ii == _mouseInCal)
                            {
                                using (SolidBrush b = new SolidBrush(Parent.MonthSelectMouseOverColor))
                                {
                                    g.FillPath(b, RoundedRect(new Rectangle(dny[ii].Key.Left + 3, dny[ii].Key.Top + poRect.Bottom + 3, dny[ii].Key.Width, dny[ii].Key.Height), 2, 2, 2, 2));
                                }
                            }
                            g.SmoothingMode = SmoothingMode.None;
                            string datum = tmp.ToString("%d");
                            Size ds = TextRenderer.MeasureText(datum, Font);
                            TextRenderer.DrawText(g, datum, Font, new Point(dny[ii].Key.Left + 13 - (ds.Width / 2), dny[ii].Key.Top + 10 - (ds.Height / 2) + poRect.Bottom),
                                ActualDate.Month == tmp.Month ? Parent.MonthForeColor : Parent.MonthTrailintForeColor);

                            tmp = tmp.AddDays(1);
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
                        DateTime tmp = new DateTime(year, 1, 31);
                        for (int ii = 0; ii < mesice.Count(); ii++)
                        //foreach (KeyValuePair<Rectangle, DateTime?> kp in dny)
                        {
                            if (tmp < refMinDate || tmp > refMaxDate)
                            {
                                mesice[ii] = new KeyValuePair<Rectangle, DateTime?>(mesice[ii].Key, null);
                                if (CurrentView == View.Months)
                                {
                                    if(tmp.Month == refMinDate.Month)
                                        tmp = tmp.AddDays(1);
                                    else
                                        tmp = tmp.AddMonths(1);
                                }
                                else
                                {
                                    if (tmp.Year == refMinDate.Year && tmp.Month != refMinDate.Month)
                                        tmp = tmp.AddMonths(1);
                                    else if (tmp.Year == refMinDate.Year && tmp.Month == refMinDate.Month)
                                        tmp = tmp.AddDays(1);
                                    else
                                        tmp = tmp.AddYears(modifier);
                                }
                                continue;
                            }
                            mesice[ii] = new KeyValuePair<Rectangle, DateTime?>(mesice[ii].Key, tmp);

                            if (CurrentView == View.Months)
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (tmp.Month == SelectedDate.Month && tmp.Year == SelectedDate.Year)
                                {
                                    using (SolidBrush b = new SolidBrush(Parent.MonthSelectedColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                    using (Pen p = new Pen(Parent.MonthSelectedDayBorderColor, 1))
                                    {
                                        g.DrawPath(p, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                else if (ii == _mouseInCal)
                                {
                                    using (SolidBrush b = new SolidBrush(Parent.MonthSelectMouseOverColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                g.SmoothingMode = SmoothingMode.None;
                                string datum = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(tmp.Month);//tmp.ToString("%d");
                                Size ds = TextRenderer.MeasureText(datum, Font);
                                if (tmp <= refMaxDate && tmp >= refMinDate)
                                {
                                    TextRenderer.DrawText(g, datum, Font, new Point(mesice[ii].Key.Left + 20 - (ds.Width / 2), mesice[ii].Key.Top + 18 - (ds.Height / 2) + header.Bottom),
                                    Parent.MonthForeColor);

                                    tmp = tmp.AddMonths(1);
                                }
                            }
                            else
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                if (tmp.Year - (tmp.Year % modifier) == SelectedDate.Year - (SelectedDate.Year % modifier))
                                {
                                    using (SolidBrush b = new SolidBrush(Parent.MonthSelectedColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                    using (Pen p = new Pen(Parent.MonthSelectedDayBorderColor, 1))
                                    {
                                        g.DrawPath(p, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                else if (ii == _mouseInCal)
                                {
                                    using (SolidBrush b = new SolidBrush(Parent.MonthSelectMouseOverColor))
                                    {
                                        g.FillPath(b, RoundedRect(new Rectangle(mesice[ii].Key.Left + 3, mesice[ii].Key.Top + header.Bottom + 1, mesice[ii].Key.Width, mesice[ii].Key.Height), 2, 2, 2, 2));
                                    }
                                }
                                g.SmoothingMode = SmoothingMode.None;
                                string datum = tmp.Year.ToString();
                                if (CurrentView == View.Centuries)
                                {
                                    minY = tmp.Year / modifier * modifier;
                                    if (minY < refMinDate.Year)
                                        minY = refMinDate.Year;
                                    datum = minY + " -\r\n" + (tmp.Year / modifier) + 9;
                                }
                                Size ds = TextRenderer.MeasureText(datum, Font);
                                TextRenderer.DrawText(g, datum, Font, new Point(mesice[ii].Key.Left + 20 - (ds.Width / 2), mesice[ii].Key.Top + 18 - (ds.Height / 2) + header.Bottom),
                                (ii == 0 || ii == 11) ? Parent.MonthTrailintForeColor : Parent.MonthForeColor);

                                if (tmp >= refMinDate && tmp <= refMaxDate)
                                    tmp = tmp.AddYears(modifier);

                            }
                        }

                    }

                    //tlačítko Dnes
                    Rectangle todayBlueRect = new Rectangle(todayRect.Width / 2 - 5, todayRect.Top + 4, 17, 12);
                    using (SolidBrush b = new SolidBrush(Parent.MonthTodayButtonBackColor))
                    {
                        g.FillPath(b, RoundedRect(todayRect, 0, 0, 2, 2));
                    }
                    string datumDnes = DateTime.Now.ToString("%d.M.yyyy");
                    Size datumDnesVelikost = TextRenderer.MeasureText(datumDnes, Font);
                    todayBlueRect.X -= (datumDnesVelikost.Width / 2);
                    int centerV = todayRect.Top + (todayRect.Height / 2) - datumDnesVelikost.Height / 2;
                    int centerH = todayRect.Right - (todayRect.Width / 2) - datumDnesVelikost.Width / 2;
                    TextRenderer.DrawText(g, datumDnes, Font, new Point(centerH + 13, centerV),
                        _mouseInTB ? Parent.MonthTodayButtonMouseOverForeColor : Parent.MonthTodayButtonForeColor);

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawPath(new Pen(Parent.MonthTodayButtonColor, 1), RoundedRect(todayBlueRect, 2, 2, 2, 2));
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
