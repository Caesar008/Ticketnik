﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    public partial class ComboBox : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private bool _mouseDown = false;
        private Rectangle dropDown;
        private DropDownList list;
        protected DateTime lastFocusLost = DateTime.Now;

        public ComboBox():base()
        {
            list = new DropDownList(BorderColorMouseOver, BackColor);
            CloseUp += ComboBox_CloseUp;
        }

        private void ComboBox_CloseUp(object sender, EventArgs e)
        {
            this.Focus();
            _mouseIn = true;
            Invalidate();
        }

        private Color borderColor = Color.Gray;
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
        private Color borderColorMouseOver = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue")]
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
        [DefaultValue(typeof(SystemColors), "GradientInactiveCaption")]
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
        [DefaultValue(typeof(Color), "DodgerBlue")]
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
        [DefaultValue(typeof(Color), "LightGray")]
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
        [DefaultValue(typeof(Color), "Gray")]
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
        private Color buttonHighlightColorDisabled = Color.Gray;
        [DefaultValue(typeof(Color), "Gray")]
        public Color ButtonHighlightColorDisabled
        {
            get { return buttonHighlightColorDisabled; }
            set
            {
                if (buttonHighlightColorDisabled != value)
                {
                    buttonHighlightColorDisabled = value;
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
        private Color foreColor = Color.Black;
        [DefaultValue(typeof(Color), "Black")]
        public override Color ForeColor
        {
            get { return foreColor; }
            set
            {
                if (foreColor != value)
                {
                    foreColor = value;
                    if (list != null)
                        list.ForeColor = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public Color SelectedItemForeColor
        {
            get
            {
                if (list != null)
                    return list.SelectedItemForeColor;
                return Color.White;
            }
            set
            {
                if (list != null)
                {
                    list.SelectedItemForeColor = value;
                }
            }
        }
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color SelectedItemBackColor
        {
            get
            {
                if (list != null)
                    return list.SelectedItemBackColor;
                return Color.DodgerBlue;
            }
            set
            {
                if (list != null)
                {
                    list.SelectedItemBackColor = value;
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
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _mouseIn = true;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseIn = false;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _mouseIn = true;
            Invalidate();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _mouseIn = false;
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if(e.KeyCode == Keys.Up)
            {
                if (items.IndexOf(selectedItem) > 0)
                    SelectedItem = items[items.IndexOf(selectedItem)-1];
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (items.IndexOf(selectedItem) < items.Count -1)
                    SelectedItem = items[items.IndexOf(selectedItem) + 1];
            }
        }

        public event EventHandler DropDown;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!_mouseDown)
            {
                _mouseDown = true;

                base.OnMouseDown(e);
                if (lastFocusLost.AddMilliseconds(10) < DateTime.Now)
                {
                    this.Focus();

                    if (DropDownStyle == ComboBoxStyle.DropDownList)
                    {
                        list.BorderColor = this.BorderColorMouseOver;
                        list.BackgroundColor = this.BackColor;
                        list.Show();
                        list.Location = new Point(this.FindForm().Location.X + this.Left + 8, this.FindForm().Location.Y + this.Bottom + 31);
                        list.Parent = this;
                        list.BringToFront();
                        list.IsOpen = true;
                        DropDown?.Invoke(this, EventArgs.Empty);
                    }
                }

                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _mouseDown = false;
        }

        [DefaultValue(typeof(bool), "True")]
        public bool DropDownAutoSize
        {
            get
            {
                if (list != null)
                    return list.AutoSize;
                return false;
            }
            set
            {
                if(list != null)
                    list.AutoSize = value;
            }
        }

        [DefaultValue(typeof(int), "30")]
        public int MaxVisibleItems
        {
            get
            {
                if (list != null)
                    return list.MaxVisibleItems;
                return 0;
            }
            set
            {
                if(list != null)
                    list.MaxVisibleItems = value;
            }
        }

        [DefaultValue(typeof(bool), "True")]
        public bool AllowSelection
        {
            get; set;
        }

        private FlatStyle flatStyle = FlatStyle.Flat;
        [DefaultValue(typeof(FlatStyle), "Flat")]
        public FlatStyle FlatStyle
        {
            get { return flatStyle; } 
            set { flatStyle = value; Invalidate(); }
        }

        [DefaultValue(typeof(bool), "True")]
        public bool FormattingEnabled
        {
            get; set;
        }

        private ComboBoxStyle comboBoxStyle = ComboBoxStyle.DropDownList;
        [DefaultValue(typeof(ComboBoxStyle), "DropDownList")]
        public ComboBoxStyle DropDownStyle
        { 
            get { return comboBoxStyle; }
            set
            {
                comboBoxStyle = value; 
                Invalidate();
            }
        }

        public event EventHandler SelectedItemChanged;
        public event EventHandler CloseUp;

        private object selectedItem;
        public object SelectedItem
        { 
            get { return selectedItem; }
            set
            {
                if(selectedItem != value)
                {
                    selectedItem = value;
                    SelectedItemChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        List<object> items = new List<object>();
        public List<object> Items
        {
            get { return items; }
            private set 
            { 
                items = value;
                if (selectedItem == null && items.Count > 0)
                    SelectedItem = items[0];
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //tlačítko šířka 18 (i s rámečkem)
            //text je 3,5 levý horní roh

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

            using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
            {
                bg.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                //pozadí
                using (SolidBrush b = new SolidBrush(BackColor))
                {
                    bg.Graphics.FillRectangle(b, 0, 0, Width, Height);
                }
                //dropdown button
                using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || this.Focused || (list != null ? list.IsOpen : false)) ? ButtonColorMouseOver : ButtonColor) : ButtonColorDisabled))
                {
                    bg.Graphics.FillRectangle(b, dropDown);
                }
                //dropdown arrow
                using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || this.Focused || (list != null ? list.IsOpen : false)) ? ArrowColorMouseOver : ArrowColor) : SystemColors.ControlDark))
                {
                    bg.Graphics.FillPolygon(b, arrow);
                }
                //rámeček
                using (Pen p = new Pen((_mouseIn || this.Focused || (list != null ? list.IsOpen : false)) ? BorderColorMouseOver : BorderColor))
                {
                    bg.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                }

                if(DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Size textSize = TextRenderer.MeasureText(selectedItem as string, Font);
                    TextRenderer.DrawText(bg.Graphics, selectedItem as string, Font, new Rectangle(1, (Height/2) - (textSize.Height/2)-1, Width - 18, Height - 6), Enabled ? foreColor : foreColorDisabled, BackColor, TextFormatFlags.EndEllipsis);
                }
                bg.Render();
            }

            base.OnPaint(e);
        }
    }
}
