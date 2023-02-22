using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Ticketník.CustomControls
{
    internal partial class DateTimePicker : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private bool _dayEdit = false;
        private bool _monthEdit = false;
        private bool _yearEdit = false;
        private bool _hourEdit = false;
        private bool _minuteEdit = false;
        private bool _keyDown = false;
        private bool _mouseDOwn = false;
        private bool _dateChanging = false;

        private string _keybuffer = "";

        private Rectangle dropDown;
        private Rectangle denNameRect;
        private Rectangle denRect;
        private Rectangle mesicRect;
        private Rectangle rokRect;
        private Rectangle minutyRect;
        private Rectangle hodinyRect;

        private Calendar calendar;

        protected DateTime lastFocusLost = DateTime.Now;

        private Color backColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of DateTimePicker"), Category("Appearance")]
        public override Color BackColor
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
        private Color foreColorDisabled = SystemColors.ControlDark;
        [DefaultValue(typeof(SystemColors), "ControlDark")]
        public Color ForeColorDisabled
        {
            get { return foreColorDisabled; }
            set
            {
                if (foreColorDisabled != value)
                {
                    foreColorDisabled = value;
                    Invalidate();
                }
            }
        }

        private Color borderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray"),
            Description("Border color of DateTimePicker"), Category("Appearance")]
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
        private Color monthBorderColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray"),
            Description("Border color of selector"), Category("Appearance")]
        public Color MonthBorderColor
        {
            get { return monthBorderColor; }
            set
            {
                if (monthBorderColor != value)
                {
                    monthBorderColor = value;
                    if (calendar != null)
                        calendar.BorderColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthBackColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of selector"), Category("Appearance")]
        public Color MonthBackColor
        {
            get { return monthBackColor; }
            set
            {
                if (monthBackColor != value)
                {
                    monthBackColor = value;
                    if (calendar != null)
                        calendar.BackgroundColor = value;
                    Invalidate();
                }
            }
        }
        private Color dayHeaderForeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Color of day names text"), Category("Appearance")]
        public Color MonthDayHeaderForeColor
        {
            get { return dayHeaderForeColor; }
            set
            {
                if (dayHeaderForeColor != value)
                {
                    dayHeaderForeColor = value;
                    if (calendar != null)
                        calendar.DayHeaderForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthHeaderBackColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of selector header"), Category("Appearance")]
        public Color MonthHeaderBackColor
        {
            get { return monthHeaderBackColor; }
            set
            {
                if (monthHeaderBackColor != value)
                {
                    monthHeaderBackColor = value;
                    if (calendar != null)
                        calendar.HeaderColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthHeaderMouseOverBackColor = Color.FromArgb(229, 243, 255);
        [DefaultValue(typeof(Color), "#e5f3ff"), Browsable(true),
            Description("Bacground color of selector header when mouse is over"), Category("Appearance")]
        public Color MonthHeaderMouseOverBackColor
        {
            get { return monthHeaderMouseOverBackColor; }
            set
            {
                if (monthHeaderMouseOverBackColor != value)
                {
                    monthHeaderMouseOverBackColor = value;
                    if (calendar != null)
                        calendar.HeaderMouseOverColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthHeaderForeColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of selector header"), Category("Appearance")]
        public Color MonthHeaderForeColor
        {
            get { return monthHeaderForeColor; }
            set
            {
                if (monthHeaderForeColor != value)
                {
                    monthHeaderForeColor = value;
                    if (calendar != null)
                        calendar.HeaderForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthHeaderMouseOverForeColor = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"), Browsable(true),
            Description("Bacground color of selector header when mouse is over"), Category("Appearance")]
        public Color MonthHeaderMouseOverForeColor
        {
            get { return monthHeaderMouseOverForeColor; }
            set
            {
                if (monthHeaderMouseOverForeColor != value)
                {
                    monthHeaderMouseOverForeColor = value;
                    if (calendar != null)
                        calendar.HeaderMouseOverForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonForeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Color of arrow inside month navigating buttons"), Category("Appearance")]
        public Color MonthButtonForeColor
        {
            get { return monthButtonForeColor; }
            set
            {
                if (monthButtonForeColor != value)
                {
                    monthButtonForeColor = value;
                    if (calendar != null)
                        calendar.ButtonArrowColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color month navigating buttons"), Category("Appearance")]
        public Color MonthButtonColor
        {
            get { return monthButtonColor; }
            set
            {
                if (monthButtonColor != value)
                {
                    monthButtonColor = value;
                    if (calendar != null)
                        calendar.ButtonBackColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonBorderColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of border of month navigating buttons"), Category("Appearance")]
        public Color MonthButtonBorderColor
        {
            get { return monthButtonBorderColor; }
            set
            {
                if (monthButtonBorderColor != value)
                {
                    monthButtonBorderColor = value;
                    if (calendar != null)
                        calendar.ButtonBorderColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthTodayButtonBackColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of month today button"), Category("Appearance")]
        public Color MonthTodayButtonBackColor
        {
            get { return monthTodayButtonBackColor; }
            set
            {
                if (monthTodayButtonBackColor != value)
                {
                    monthTodayButtonBackColor = value;
                    if (calendar != null)
                        calendar.TodayButtonBackColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthTodayButtonColor = Color.FromArgb(0, 102, 204);
        [DefaultValue(typeof(Color), "0, 102, 204"), Browsable(true),
            Description("Color of rectangle inside month today button"), Category("Appearance")]
        public Color MonthTodayButtonColor
        {
            get { return monthTodayButtonColor; }
            set
            {
                if (monthTodayButtonColor != value)
                {
                    monthTodayButtonColor = value;
                    if (calendar != null)
                        calendar.TodayButtonColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthTodayButtonForeColor = Color.FromArgb(0, 102, 204);
        [DefaultValue(typeof(Color), "0, 102, 204"), Browsable(true),
            Description("Fore color of month today button"), Category("Appearance")]
        public Color MonthTodayButtonForeColor
        {
            get { return monthTodayButtonForeColor; }
            set
            {
                if (monthTodayButtonForeColor != value)
                {
                    monthTodayButtonForeColor = value;
                    if (calendar != null)
                        calendar.TodayButtonForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthForeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Fore color of calendar"), Category("Appearance")]
        public Color MonthForeColor
        {
            get { return monthForeColor; }
            set
            {
                if (monthForeColor != value)
                {
                    monthForeColor = value;
                    if (calendar != null)
                        calendar.ForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthSelectedMouseOverForeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Fore color of calendar"), Category("Appearance")]
        public Color MonthSelectedMouseOverForeColor
        {
            get { return monthSelectedMouseOverForeColor; }
            set
            {
                if (monthSelectedMouseOverForeColor != value)
                {
                    monthSelectedMouseOverForeColor = value;
                    if (calendar != null)
                        calendar.SelectedMouseOverForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthSelectedDayBorderColor = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"), Browsable(true),
            Description("Fore color of calendar"), Category("Appearance")]
        public Color MonthSelectedDayBorderColor
        {
            get { return monthSelectedDayBorderColor; }
            set
            {
                if (monthSelectedDayBorderColor != value)
                {
                    monthSelectedDayBorderColor = value;
                    if (calendar != null)
                        calendar.SelectedDayBorderColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthSelectedColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Fore color of calendar"), Category("Appearance")]
        public Color MonthSelectedColor
        {
            get { return monthSelectedColor; }
            set
            {
                if (monthSelectedColor != value)
                {
                    monthSelectedColor = value;
                    if (calendar != null)
                        calendar.SelectedColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthSelectMouseOverColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Fore color of calendar"), Category("Appearance")]
        public Color MonthSelectMouseOverColor
        {
            get { return monthSelectMouseOverColor; }
            set
            {
                if (monthSelectMouseOverColor != value)
                {
                    monthSelectMouseOverColor = value;
                    if (calendar != null)
                        calendar.SelectMouseOverColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthTrailingForeColor = Color.Black;
        [DefaultValue(typeof(Color), "Black"), Browsable(true),
            Description("Fore color of calendar trailing"), Category("Appearance")]
        public Color MonthTrailintForeColor
        {
            get { return monthTrailingForeColor; }
            set
            {
                if (monthTrailingForeColor != value)
                {
                    monthTrailingForeColor = value;
                    if (calendar != null)
                        calendar.TrailingForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthTodayButtonMouseOverForeColor = Color.FromArgb(0, 102, 204);
        [DefaultValue(typeof(Color), "0, 102, 204"), Browsable(true),
            Description("Fore color of month today button"), Category("Appearance")]
        public Color MonthTodayButtonMouseOverForeColor
        {
            get { return monthTodayButtonMouseOverForeColor; }
            set
            {
                if (monthTodayButtonMouseOverForeColor != value)
                {
                    monthTodayButtonMouseOverForeColor = value;
                    if (calendar != null)
                        calendar.TodayButtonMouseOverForeColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonMouseOverColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of month navigating buttons when mouse is over"), Category("Appearance")]
        public Color MonthButtonMouseOverColor
        {
            get { return monthButtonMouseOverColor; }
            set
            {
                if (monthButtonMouseOverColor != value)
                {
                    monthButtonMouseOverColor = value;
                    if (calendar != null)
                        calendar.ButonMouseOverColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonBorderMouseOverColor = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"), Browsable(true),
            Description("Bacground color of border of month navigating buttons when mouse is over"), Category("Appearance")]
        public Color MonthButtonBorderMouseOverColor
        {
            get { return monthButtonBorderMouseOverColor; }
            set
            {
                if (monthButtonBorderMouseOverColor != value)
                {
                    monthButtonBorderMouseOverColor = value;
                    if (calendar != null)
                        calendar.ButonBorderMouseOverColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthButtonMouseOverForeColor = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"), Browsable(true),
            Description("Color of arrow in month navigating buttons when mouse is over"), Category("Appearance")]
        public Color MonthButtonMouseOverForeColor
        {
            get { return monthButtonMouseOverForeColor; }
            set
            {
                if (monthButtonMouseOverForeColor != value)
                {
                    monthButtonMouseOverForeColor = value;
                    if (calendar != null)
                        calendar.ArrowMouseOverColor = value;
                    Invalidate();
                }
            }
        }
        private Color borderColorDisabled = SystemColors.ControlDark;
        [DefaultValue(typeof(SystemColors), "ControlDark")]
        public Color BorderColorDisabled
        {
            get { return borderColorDisabled; }
            set
            {
                if (borderColorDisabled != value)
                {
                    borderColorDisabled = value;
                    Invalidate();
                }
            }
        }
        private Color borderColorMouseOver = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"),
            Description("Border color of DateTimePicker when mouse is over control"), Category("Appearance")]
        public Color BorderColorMouseOver
        {
            get { return borderColorMouseOver; }
            set
            {
                if (borderColorMouseOver != value)
                {
                    borderColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        private Color buttonColorMouseOver = SystemColors.GradientInactiveCaption;
        [DefaultValue(typeof(SystemColors), "GradientInactiveCaption"),
            Description("Color of dropdown button when mose is over control"), Category("Appearance")]
        public Color ButtonColorMouseOver
        {
            get { return buttonColorMouseOver; }
            set
            {
                if (buttonColorMouseOver != value)
                {
                    buttonColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        private Color arrowColorMouseOver = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue"),
            Description("Color of arrow inside dropdown button when mouse is over control"), Category("Appearance")]
        public Color ArrowColorMouseOver
        {
            get { return arrowColorMouseOver; }
            set
            {
                if (arrowColorMouseOver != value)
                {
                    arrowColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        private Color buttonColor = Color.LightGray;
        [DefaultValue(typeof(Color), "LightGray"),
            Description("Color of dropdown button"), Category("Appearance")]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                if (buttonColor != value)
                {
                    buttonColor = value;
                    Invalidate();
                }
            }
        }
        private Color buttonColorDisabled = SystemColors.Control;
        [DefaultValue(typeof(SystemColors), "Control")]
        public Color ButtonColorDisabled
        {
            get { return buttonColorDisabled; }
            set
            {
                if (buttonColorDisabled != value)
                {
                    buttonColorDisabled = value;
                    Invalidate();
                }
            }
        }
        private Color arrowColor = Color.Gray;
        [DefaultValue(typeof(Color), "Gray"),
            Description("Color of arrow inside dropdown button"), Category("Appearance")]
        public Color ArrowColor
        {
            get { return arrowColor; }
            set
            {
                if (arrowColor != value)
                {
                    arrowColor = value;
                    Invalidate();
                }
            }
        }
        private Color monthSeparatorColor = Color.Gainsboro;
        [DefaultValue(typeof(Color), "Gainsboro"),
            Description("Color of dropdown button"), Category("Appearance")]
        public Color MonthSeparatorColor
        {
            get { return monthSeparatorColor; }
            set
            {
                if (monthSeparatorColor != value)
                {
                    monthSeparatorColor = value;
                    if (calendar != null)
                        calendar.SeparatorColor = value;
                    Invalidate();
                }
            }
        }
        private DateTime maxDate = new DateTime(8999, 12, 31);
        [DefaultValue(typeof(DateTime), "31.12.8999"),
            Description("Sets max date available in calendar"), Category("Data")]
        public DateTime MaxDate
        {
            get { return maxDate; }
            set
            {
                if (maxDate != value)
                {
                    if(value.Year > 8999)
                        maxDate = new DateTime(8999, 12, 31);
                    else
                        maxDate = value;
                    if(Value > maxDate)
                        Value = maxDate;
                }
            }
        }
        private DateTime minDate = new DateTime(1753, 1, 1);
        [DefaultValue(typeof(DateTime), "1.1.1753"),
            Description("Sets min date available in calendar"), Category("Data")]
        public DateTime MinDate
        {
            get { return minDate; }
            set
            {
                if (minDate != value)
                {
                    if(value.Year < 1753)
                        minDate = new DateTime(1753, 1, 1);
                    else
                        minDate = value;
                    if(Value < minDate)
                        Value = minDate;
                }
            }
        }
        private DateTime _value;
        private DateTime _TmpValue;
        [Description("Value of DateTimePicker"), Category("Data")]
        public DateTime Value
        {
            get { 
                if(!_dateChanging)
                    return _value != null ? _value : DateTime.Today;
                return _TmpValue != null ? _TmpValue : _value;
            }
            set
            {
                if (_dateChanging)
                {
                    if (_TmpValue != value)
                    {
                        if (value <= MaxDate || value >= MinDate)
                            _TmpValue = value;
                        else if (value > maxDate)
                            _TmpValue = MaxDate;
                        else if (value < minDate)
                            _TmpValue = MinDate;
                        Invalidate();
                    }
                }
                else
                {
                    if (_value != value)
                    {
                        if (value <= MaxDate || value >= MinDate)
                            _TmpValue = _value = value;
                        else if (value > maxDate)
                            _TmpValue = _value = MaxDate;
                        else if (value < minDate)
                            _TmpValue = _value = MinDate;
                        Invalidate();
                        ValueChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private string customFormat;
        [Description("Custom format of DateTimePicker date and time"), Category("Format")]
        public string CustomFormat
        {
            get { return customFormat; }
            set
            {
                if (customFormat != value)
                {
                    customFormat = value;
                    Invalidate();
                }
            }
        }
        private DateTimePickerFormat format;
        [Description("Specify format of DateTimePicker"), Category("Format")]
        public DateTimePickerFormat Format
        {
            get { return format; }
            set
            {
                if (format != value)
                {
                    format = value;
                    Invalidate();
                }
            }
        }

        [Category("Action")]
        public event EventHandler CloseUp;
        [Category("Action")]
        public event EventHandler ValueChanged;
        [Category("Action")]
        public event EventHandler DropDown;

        public DateTimePicker():base()
        {
            Value = DateTime.Today;
            calendar = new Calendar(MonthBorderColor, MonthBackColor);
            calendar.ValueChanged += Calendar_ValueChanged;
        }

        public bool IsDateChanging => _dateChanging;

        private void Calendar_ValueChanged(object sender, EventArgs e)
        {
            this.Value = calendar.SelectedDate;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _mouseIn = true;
            base.OnMouseEnter(e);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseIn = false;
            base.OnMouseLeave(e);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _mouseIn = true;
            base.OnGotFocus(e);
            Invalidate();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            _keyDown = _dateChanging = _mouseIn = _dayEdit = _monthEdit = _yearEdit = _minuteEdit = _hourEdit = false;
            base.OnLostFocus(e);
            Invalidate();
            /*if (calendar != null && _calendarOpen)
            {
                calendar.Hide();
                _calendarOpen = false;
            }*/
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            byte value = 10;
            if (!_keyDown)
            {
                _keyDown = true;
                switch (e.KeyCode)
                {
                    case Keys.NumPad0: value = 0; break;
                    case Keys.NumPad1: value = 1; break;
                    case Keys.NumPad2: value = 2; break;
                    case Keys.NumPad3: value = 3; break;
                    case Keys.NumPad4: value = 4; break;
                    case Keys.NumPad5: value = 5; break;
                    case Keys.NumPad6: value = 6; break;
                    case Keys.NumPad7: value = 7; break;
                    case Keys.NumPad8: value = 8; break;
                    case Keys.NumPad9: value = 9; break;
                    case Keys.Enter: value = 254; break;
                    default: value = 10; break;
                }
            }
            if (value < 10)
            {
                if (_dayEdit)
                {
                    if (_keybuffer.Length == 0 && value != 0)
                    {
                        _dateChanging = true;
                        _keybuffer = value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, int.Parse(_keybuffer), Value.Hour, Value.Minute, Value.Second);
                    }
                    else if (int.Parse(_keybuffer + value.ToString()) <= DateTime.DaysInMonth(Value.Year, Value.Month))
                    {
                        _dateChanging = true;
                        _keybuffer = _keybuffer + value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, int.Parse(_keybuffer), Value.Hour, Value.Minute, Value.Second);
                    }
                    else
                    {
                        if (value != 0)
                        {
                            _dateChanging = true;
                            _keybuffer = value.ToString();
                            Value = new DateTime(Value.Year, Value.Month, int.Parse(_keybuffer), Value.Hour, Value.Minute, Value.Second);
                        }
                    }
                
                }
                if (_monthEdit)
                {
                    if (_keybuffer.Length == 0 && value != 0)
                    {
                        _dateChanging = true;
                        _keybuffer = value.ToString();
                        Value = new DateTime(Value.Year, int.Parse(_keybuffer), 
                            Value.Day > DateTime.DaysInMonth(Value.Year,int.Parse(_keybuffer)) ? DateTime.DaysInMonth(Value.Year,int.Parse(_keybuffer)) : Value.Day, 
                            Value.Hour, Value.Minute, Value.Second);
                    }
                    else if (int.Parse(_keybuffer + value.ToString()) <= 12)
                    {
                        _dateChanging = true;
                        _keybuffer = _keybuffer + value.ToString();
                        Value = new DateTime(Value.Year, int.Parse(_keybuffer),
                            Value.Day > DateTime.DaysInMonth(Value.Year, int.Parse(_keybuffer)) ? DateTime.DaysInMonth(Value.Year, int.Parse(_keybuffer)) : Value.Day,
                            Value.Hour, Value.Minute, Value.Second);
                    }
                    else
                    {
                        if (value != 0)
                        {
                            _dateChanging = true;
                            _keybuffer = value.ToString();
                            Value = new DateTime(Value.Year, int.Parse(_keybuffer),
                            Value.Day > DateTime.DaysInMonth(Value.Year, int.Parse(_keybuffer)) ? DateTime.DaysInMonth(Value.Year, int.Parse(_keybuffer)) : Value.Day,
                            Value.Hour, Value.Minute, Value.Second);
                        }
                    }

                }
                if (_yearEdit)
                {
                    if (_keybuffer.Length == 0 && value != 0)
                    {
                        _dateChanging = true;
                        _keybuffer = value.ToString();
                        Value = new DateTime(int.Parse(_keybuffer), Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second);
                    }
                    else if (_keybuffer.Length < 4)
                    {
                        _dateChanging = true;
                        _keybuffer = _keybuffer + value.ToString();
                        Value = new DateTime(int.Parse(_keybuffer), Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second);
                    }
                    else
                    {
                        if (value != 0)
                        {
                            _dateChanging = true;
                            _keybuffer = value.ToString();
                            Value = new DateTime(int.Parse(_keybuffer), Value.Month, Value.Day, Value.Hour, Value.Minute, Value.Second);
                        }
                    }
                }
                if (_hourEdit)
                {
                    if (int.Parse(_keybuffer + value.ToString()) < 60)
                    {
                        _dateChanging = true;
                        _keybuffer = _keybuffer + value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, Value.Day, int.Parse(_keybuffer), Value.Minute, Value.Second);
                    }
                    else
                    {
                        _dateChanging = true;
                        _keybuffer = value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, Value.Day, int.Parse(_keybuffer), Value.Minute, Value.Second);

                    }

                }
                if (_minuteEdit)
                {
                    if (int.Parse(_keybuffer + value.ToString()) < 60)
                    {
                        _dateChanging = true;
                        _keybuffer = _keybuffer + value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, int.Parse(_keybuffer), Value.Second);
                    }
                    else
                    {
                        _dateChanging = true;
                        _keybuffer = value.ToString();
                        Value = new DateTime(Value.Year, Value.Month, Value.Day, Value.Hour, int.Parse(_keybuffer), Value.Second);

                    }
                }
                Invalidate();
            }
            else if (value == 254)
            {
                DateTime tmpDateTime = Value;
                if (tmpDateTime.Year < MinDate.Year)
                    tmpDateTime = new DateTime(MinDate.Year, tmpDateTime.Month, tmpDateTime.Day, tmpDateTime.Hour, tmpDateTime.Minute, tmpDateTime.Second);
                else if (tmpDateTime.Year > MaxDate.Year)
                    tmpDateTime = new DateTime(MaxDate.Year, tmpDateTime.Month, tmpDateTime.Day, tmpDateTime.Hour, tmpDateTime.Minute, tmpDateTime.Second);
                _dateChanging = false;
                _keybuffer = "";
                Value = tmpDateTime;
                calendar.ActualDate = calendar.SelectedDate = tmpDateTime;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            _keyDown = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_mouseDOwn)
            {
                _mouseDOwn = true;
                _dayEdit = false;
                _monthEdit = false;
                _yearEdit = false;
                _minuteEdit = false;
                _hourEdit = false;
                _keybuffer = "";
                if (_dateChanging)
                {
                    DateTime tmpDateTime = Value;
                    if (tmpDateTime.Year < MinDate.Year)
                        tmpDateTime = new DateTime(MinDate.Year, tmpDateTime.Month, tmpDateTime.Day, tmpDateTime.Hour, tmpDateTime.Minute, tmpDateTime.Second);
                    else if (tmpDateTime.Year > MaxDate.Year)
                        tmpDateTime = new DateTime(MaxDate.Year, tmpDateTime.Month, tmpDateTime.Day, tmpDateTime.Hour, tmpDateTime.Minute, tmpDateTime.Second);
                    _dateChanging = false;
                    _keybuffer = "";
                    Value = tmpDateTime;
                    calendar.ActualDate = calendar.SelectedDate = tmpDateTime;
                }

                base.OnMouseDown(e);
                this.Focus();
                if (denRect != null && denRect.Contains(e.Location))
                {
                    _dayEdit = true;
                }
                else if (mesicRect != null && mesicRect.Contains(e.Location))
                {
                    _monthEdit = true;
                }
                else if (rokRect != null && rokRect.Contains(e.Location))
                {
                    _yearEdit = true;
                }
                else if (minutyRect != null && minutyRect.Contains(e.Location))
                {
                    _minuteEdit = true;
                }
                else if (hodinyRect != null && hodinyRect.Contains(e.Location))
                {
                    _hourEdit = true;
                }
                else if (dropDown != null && dropDown.Contains(e.Location))
                {
                    if (lastFocusLost.AddMilliseconds(10) < DateTime.Now)
                    {
                        calendar.BorderColor = this.BorderColor;
                        calendar.BackgroundColor = this.BackColor;
                        calendar.Show();
                        calendar.Location = new Point(this.FindForm().Location.X + this.Left + 8, this.FindForm().Location.Y + this.Bottom + 31);
                        calendar.Parent = this;

                        //this.Focus();
                        calendar.BringToFront();
                        calendar.IsOpen = true;
                        DropDown?.Invoke(this, EventArgs.Empty);
                    }
                }
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _mouseDOwn= false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                Rectangle area = new Rectangle(0, 0, Width - 1, Height - 1);
                dropDown = new Rectangle(Width - 18, area.X + 1, 17, Height - 2);
                Point middle = new Point(dropDown.Left + dropDown.Width / 2,
                    dropDown.Top + dropDown.Height / 2);
                Point[] arrow = new Point[]
                {
                    new Point(middle.X - 3, middle.Y - 2),
                    new Point(middle.X + 4, middle.Y - 2),
                new Point(middle.X, middle.Y + 2)
                };
                string denName = new DateTime(Value.Ticks).ToString("dddd");
                Size denNameSize = TextRenderer.MeasureText(denName, Font);
                denNameRect = new Rectangle(1, (Height / 2) - (denNameSize.Height / 2) - 1, denNameSize.Width, denNameSize.Height);
                string den = new DateTime(Value.Ticks).ToString("%d") + ".";
                Size denSize = TextRenderer.MeasureText(den, Font);
                denRect = new Rectangle(new Point(denNameRect.Right, denNameRect.Top), denSize);
                //DateTimeFormatInfo info = CultureInfo.GetCultureInfo(CultureInfo.CurrentCulture.Name).DateTimeFormat;
                string mesic = DateTimeFormatInfo.CurrentInfo.MonthGenitiveNames[new DateTime(Value.Ticks).Month - 1];
                Size mesicSize = TextRenderer.MeasureText(mesic, Font);
                mesicRect = new Rectangle(new Point(denRect.Right, denRect.Top), mesicSize);
                string rok = new DateTime(Value.Ticks).ToString("yyyy");
                Size rokSize = TextRenderer.MeasureText(rok, Font);
                rokRect = new Rectangle(new Point(mesicRect.Right, mesicRect.Top), rokSize);
                using (Pen p = new Pen(Enabled ? ((Focused || _mouseIn || _dayEdit || _monthEdit || _yearEdit) ? BorderColorMouseOver : BorderColor) : BorderColorDisabled))
                {
                    //text
                    //jméno dne
                    if (Format == DateTimePickerFormat.Long || Format == 0)
                    {
                        if (denNameRect.Right <= area.Width - 20 && denNameRect.Bottom <= area.Height - 2 && denName != "")
                        {

                            TextRenderer.DrawText(g, denName, Font, denNameRect, Enabled ? ForeColor : ForeColorDisabled);

                        }

                        //den
                        if (denRect.Right <= area.Width - 20 && denRect.Bottom <= area.Height - 2 && den != "")
                        {
                            if (Enabled && _dayEdit)
                            {
                                using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                {
                                    g.FillRectangle(bb, denRect);
                                    TextRenderer.DrawText(g, den, Font, denRect, Color.White);
                                }
                            }
                            else
                                TextRenderer.DrawText(g, den, Font, denRect, Enabled ? ForeColor : ForeColorDisabled);
                        }

                        //jméno měsíce
                        if (mesicRect.Right <= area.Width - 20 && mesicRect.Bottom <= area.Height - 2 && mesic != "")
                        {
                            if (Enabled && _monthEdit)
                            {
                                using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                {
                                    g.FillRectangle(bb, mesicRect);
                                    TextRenderer.DrawText(g, mesic, Font, mesicRect, Color.White);
                                }
                            }
                            else
                                TextRenderer.DrawText(g, mesic, Font, mesicRect, Enabled ? ForeColor : ForeColorDisabled);
                        }

                        //rok
                        if (rokRect.Right <= area.Width - 20 && rokRect.Bottom <= area.Height - 2 && rok != "")
                        {
                            if (Enabled && _yearEdit)
                            {
                                using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                {
                                    g.FillRectangle(bb, rokRect);
                                    TextRenderer.DrawText(g, rok, Font, rokRect, Color.White);
                                }
                            }
                            else
                                TextRenderer.DrawText(g, rok, Font, rokRect, Enabled ? ForeColor : ForeColorDisabled);
                        }
                    }
                    else if (Format == DateTimePickerFormat.Custom)
                    {
                        List<CustomFormatStruct> castiFormatu = new List<CustomFormatStruct>();
                        CustomFormatStruct formatPart = new CustomFormatStruct();
                        formatPart.format = "";
                        string last = "";
                        string dtFormatLetters = "dMfFghHKmstyz";
                        
                        int i = 0;
                        while ( i< CustomFormat.Length)
                        {
                            string substr = CustomFormat.Substring(i, 1);
                            //tady musim pořešit čtení a rozdělení na části znaky a pak formát jednotlivých částí data
                            if (substr == "%")
                            {
                                formatPart.format = "%";
                                substr = CustomFormat.Substring(++i, 1);
                            }
                            else
                            {
                                if (dtFormatLetters.Contains(substr))
                                {
                                    while ((last == substr || last == "") && i < CustomFormat.Length)
                                    {
                                        switch (substr)
                                        {
                                            //tohle není komplet seznam, jsou tam jen co používám já
                                            case "d": formatPart.format += "d"; formatPart.rectTyp = RectTyp.Den; break;
                                            case "M": formatPart.format += "M"; formatPart.rectTyp = RectTyp.Mesic; break;
                                            case "h": formatPart.format += "h"; formatPart.rectTyp = RectTyp.Hodina; break;
                                            case "H": formatPart.format += "H"; formatPart.rectTyp = RectTyp.Hodina; break;
                                            case "m": formatPart.format += "m"; formatPart.rectTyp = RectTyp.Minuta; break;
                                            case "s": formatPart.format += "s"; formatPart.rectTyp = RectTyp.Vterina; break;
                                            case "y": formatPart.format += "y"; formatPart.rectTyp = RectTyp.Rok; break;
                                            default: formatPart.format += substr; formatPart.rectTyp = RectTyp.DatumOstatni; break;
                                        }
                                        last = substr;
                                        if (i+1 < customFormat.Length)
                                            substr = CustomFormat.Substring(++i, 1);
                                        else
                                            i++;
                                    }

                                    formatPart.data = Value.ToString(formatPart.format);

                                }
                                else
                                {
                                    last = "";
                                    while (!dtFormatLetters.Contains(substr) && i < CustomFormat.Length)
                                    {
                                        formatPart.data += substr;
                                        formatPart.rectTyp = RectTyp.Ostatni;
                                        if (i+1 < customFormat.Length)
                                            substr = CustomFormat.Substring(++i, 1);
                                        else
                                            i++;
                                    }
                                }

                                Size dataDim = TextRenderer.MeasureText(formatPart.data, Font);
                                int prvni = castiFormatu.Count > 0 ? castiFormatu[castiFormatu.Count - 1].rect.Right : 4;
                                formatPart.rect = new Rectangle(prvni, (Height / 2) - (dataDim.Height / 2) - 1, dataDim.Width-6, dataDim.Height);
                                switch(formatPart.rectTyp)
                                {
                                    case RectTyp.Rok: rokRect = formatPart.rect; break;
                                    case RectTyp.Mesic: mesicRect = formatPart.rect; break;
                                    case RectTyp.Den: denRect = formatPart.rect; break;
                                    case RectTyp.Hodina: hodinyRect = formatPart.rect; break;
                                    case RectTyp.Minuta: minutyRect = formatPart.rect; break;
                                }
                                castiFormatu.Add(formatPart);
                                formatPart = new CustomFormatStruct();
                                formatPart.format = "";
                            }
                        }

                        foreach(CustomFormatStruct cfs in castiFormatu)
                        {
                            if (cfs.rect.Right <= area.Width - 20 && cfs.rect.Bottom <= area.Height - 2 && rok != "")
                            {
                                if (Enabled && (_yearEdit||_monthEdit||_dayEdit||_hourEdit||_minuteEdit))
                                {
                                    if (_yearEdit && cfs.rectTyp == RectTyp.Rok)
                                    {
                                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                        {
                                            g.FillRectangle(bb, rokRect);
                                            TextRenderer.DrawText(g, cfs.data, Font, rokRect, Color.White);
                                        }
                                    }
                                    else if (_monthEdit && cfs.rectTyp == RectTyp.Mesic)
                                    {
                                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                        {
                                            g.FillRectangle(bb, mesicRect);
                                            TextRenderer.DrawText(g, cfs.data, Font, mesicRect, Color.White);
                                        }
                                    }
                                    else if (_dayEdit && cfs.rectTyp == RectTyp.Den)
                                    {
                                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                        {
                                            g.FillRectangle(bb, denRect);
                                            TextRenderer.DrawText(g, cfs.data, Font, denRect, Color.White);
                                        }
                                    }
                                    else if (_hourEdit && cfs.rectTyp == RectTyp.Hodina)
                                    {
                                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                        {
                                            g.FillRectangle(bb, hodinyRect);
                                            TextRenderer.DrawText(g, cfs.data, Font, hodinyRect, Color.White);
                                        }
                                    }
                                    else if (_minuteEdit && cfs.rectTyp == RectTyp.Minuta)
                                    {
                                        using (SolidBrush bb = new SolidBrush(Color.FromArgb(0, 120, 215)))
                                        {
                                            g.FillRectangle(bb, minutyRect);
                                            TextRenderer.DrawText(g, cfs.data, Font, minutyRect, Color.White);
                                        }
                                    }
                                    else
                                        TextRenderer.DrawText(g, cfs.data, Font, cfs.rect, Enabled ? ForeColor : ForeColorDisabled);
                                }
                                else
                                    TextRenderer.DrawText(g, cfs.data, Font, cfs.rect, Enabled ? ForeColor : ForeColorDisabled);
                            }
                        }
                    }

                    //dropdown button
                    using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || this.Focused) ? ButtonColorMouseOver : ButtonColor) : ButtonColorDisabled))
                    {
                        g.FillRectangle(b, dropDown);
                    }
                    //dropdown arrow
                    using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || this.Focused) ? ArrowColorMouseOver : ArrowColor) : SystemColors.ControlDark))
                    {
                        g.FillPolygon(b, arrow);
                    }
                    //rámeček
                    g.DrawRectangle(p, area);
                }

            }
        }

        private enum RectTyp
        {
            Den,
            Mesic,
            Rok,
            Hodina,
            Minuta,
            Vterina,
            DatumOstatni,
            Ostatni
        }
        private struct CustomFormatStruct
        {
            public Rectangle rect;
            public RectTyp rectTyp;
            public string format;
            public string data;
        }
    }
}
