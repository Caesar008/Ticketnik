using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    internal class DatePicker : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private Color backColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true),
            Description("Bacground color of DatePicker"), Category("Appearance")]
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
                    Invalidate();
                }
            }
        }
        private DateTime minDate = DateTime.MaxValue;
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
                    Invalidate();
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
                    _value = value;
                    Invalidate();
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
            _mouseIn = false;
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                Rectangle area = new Rectangle(0, 0, Width -1, Height - 1);
                Rectangle dropDown = new Rectangle(Width -18, area.X +1, 17, Height-2);
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
                string den = new DateTime(Value.Ticks).ToString("d");
                string mesic = new DateTime(Value.Ticks).ToString("MMMM");
                string rok = new DateTime(Value.Ticks).ToString("yyyy");
                if(Format == DateTimePickerFormat.Custom)
                {
                    denName = "";
                }
                using (Pen p = new Pen(Enabled ? ((Focused || _mouseIn) ? BorderColorMouseOver : BorderColor) : SystemColors.ControlDark))
                {
                    //text
                    //jméno dne
                    if (denNameSize.Width <= area.Width - 22 && denNameSize.Height <= area.Height-2 && denName != "")
                    {
                        TextRenderer.DrawText(g, denName, Font, new Point(1, (Height /2)-(denNameSize.Height/2)-1), ForeColor);
                    }

                    //den

                    //jméno měsíce

                    //rok
                    //dropdown button
                    using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || this.Focused) ? ButtonColorMouseOver : ButtonColor) : SystemColors.Control))
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
