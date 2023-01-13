using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    internal class DatePicker : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private bool _dayEdit = false;
        private bool _monthEdit = false;
        private bool _yearEdit = false;

        private Rectangle dropDown;
        private Rectangle denNameRect;
        private Rectangle denRect;
        private Rectangle mesicRect;
        private Rectangle rokRect;

        private Color backColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of DatePicker"), Category("Appearance")]
        /*public override Color BackColor
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
        }*/
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
            Description("Border color of DatePicker"), Category("Appearance")]
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
            Description("Border color of DatePicker when mouse is over control"), Category("Appearance")]
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
        private DateTime maxDate = DateTime.MaxValue;
        [DefaultValue(typeof(DateTime), "31.12.9999"),
            Description("Sets max date available in calendar"), Category("Data")]
        public DateTime MaxDate
        {
            get { return maxDate; }
            set
            {
                if (maxDate != value)
                {
                    maxDate = value;
                    if(Value > maxDate)
                        Value = maxDate;
                }
            }
        }
        private DateTime minDate = DateTime.MinValue;
        [DefaultValue(typeof(DateTime), "1.1.0001"),
            Description("Sets min date available in calendar"), Category("Data")]
        public DateTime MinDate
        {
            get { return minDate; }
            set
            {
                if (minDate != value)
                {
                    minDate = value;
                    if(Value < minDate)
                        Value = minDate;
                }
            }
        }
        private DateTime _value;
        [Description("Value of DatePicker"), Category("Data")]
        public DateTime Value
        {
            get { return _value != null ? _value : DateTime.Today; }
            set
            {
                if (_value != value)
                {
                    if (value <= MaxDate || value >= MinDate)
                        _value = value;
                    else if (value > maxDate)
                        _value = MaxDate;
                    else if(value < minDate)
                        _value = MinDate;
                    Invalidate();
                    if(onValueChanged != null)
                        onValueChanged(this, EventArgs.Empty);
                }
            }
        }

        private string customFormat;
        [Description("Custom format of DatePicker date and time"), Category("Format")]
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
        [Description("Specify format of DatePicker"), Category("Format")]
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
        private EventHandler onCloseUp;
        private EventHandler onValueChanged;
        private EventHandler onDropDown;
        [Category("Action")]
        public event EventHandler CloseUp
        {
            add { onCloseUp += value; }
            remove { onCloseUp -= value; }
        }
        [Category("Action")]
        public event EventHandler ValueChanged
        {
            add { onValueChanged += value;}
            remove { onValueChanged -= value; }
        }
        [Category("Action")]
        public event EventHandler DropDown
        {
            add { onDropDown += value; }
            remove
            {
                onDropDown -= value;
            }
        }

        public DatePicker():base()
        {
            Value = DateTime.Today;
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
            _mouseIn = _dayEdit = _monthEdit = _yearEdit = false;
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            _dayEdit = false;
            _monthEdit = false;
            _yearEdit = false;

            base.OnMouseClick(e);
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

            else if (dropDown != null && dropDown.Contains(e.Location))
            {
                //otevřít výběr dne
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                Rectangle area = new Rectangle(0, 0, Width -1, Height - 1);
                dropDown = new Rectangle(Width -18, area.X +1, 17, Height-2);
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
                DateTimeFormatInfo info = CultureInfo.GetCultureInfo(CultureInfo.CurrentCulture.Name).DateTimeFormat;
                string mesic = info.MonthGenitiveNames[new DateTime(Value.Ticks).Month - 1];
                Size mesicSize = TextRenderer.MeasureText(mesic, Font);
                mesicRect = new Rectangle(new Point(denRect.Right, denRect.Top), mesicSize);
                string rok = new DateTime(Value.Ticks).ToString("yyyy");
                Size rokSize = TextRenderer.MeasureText(rok, Font);
                rokRect = new Rectangle(new Point(mesicRect.Right, mesicRect.Top), rokSize);
                if (Format == DateTimePickerFormat.Custom)
                {
                    denName = "";
                }
                using (Pen p = new Pen(Enabled ? ((Focused || _mouseIn) ? BorderColorMouseOver : BorderColor) : BorderColorDisabled))
                {
                    //text
                    //jméno dne
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
    }
}
