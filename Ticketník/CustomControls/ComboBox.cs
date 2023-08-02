using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Ticketník.CustomControls
{
    public partial class ComboBox : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private bool _mouseDown = false;
        private Rectangle dropDown;
        private DropDownList list;
        protected DateTime lastFocusLost = DateTime.Now;
        internal int _scrollPosition = 0;
        internal int _markedItem = -1;
        private System.Windows.Forms.TextBox textBox;
        private bool _mouseInTextBox = false;

        public ComboBox():base()
        {
            CloseUp += ComboBox_CloseUp;

            textBox = new System.Windows.Forms.TextBox();
            textBox.Location = new Point(3, 3);
            textBox.Multiline = false;
            textBox.Width = this.Width - 6;
            textBox.BackColor = BackColor;
            textBox.ForeColor = ForeColor;
            textBox.BorderStyle = BorderStyle.None;
            textBox.MouseEnter += TextBox_MouseEnter;
            textBox.MouseHover += TextBox_MouseHover;
            textBox.MouseMove += TextBox_MouseMove;
            textBox.MouseLeave += TextBox_MouseLeave;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
            textBox.Text = this.Text;
            textBox.Font = this.Font;
            textBox.Tag = "CustomColor:Ignore";
            if(DropDownStyle == ComboBoxStyle.DropDownList)
                textBox.Visible = false;
            else
                textBox.Visible = true;
            Controls.Add(textBox);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            textBox.Text = Text;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            textBox.Size = new Size(Width - 6 -18, Height - 6);
            Invalidate();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            textBox.BackColor = BackColor;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Text = textBox.Text;
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            _mouseInTextBox = false;
            if (!Focused)
                _mouseIn = false;
            Invalidate();
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            _mouseInTextBox = true;
            Invalidate();
        }

        private void TextBox_MouseLeave(object sender, EventArgs e)
        {
            _mouseInTextBox = false;
            Invalidate();
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseInTextBox)
            {
                _mouseInTextBox = true;
                Invalidate();
            }
        }

        private void TextBox_MouseHover(object sender, EventArgs e)
        {
            if (!_mouseInTextBox)
            {
                _mouseInTextBox = true;
                Invalidate();
            }
        }

        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            _mouseInTextBox = true;
            Invalidate();
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
                    textBox.ForeColor = ForeColor;
                    Invalidate();
                }
            }
        }

        Color selectedItemForeColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color SelectedItemForeColor
        {
            get
            {
                return selectedItemForeColor;
            }
            set
            {
                selectedItemForeColor = value;
            }
        }

        Color selectedItemBackColor = Color.DodgerBlue;
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color SelectedItemBackColor
        {
            get
            {
                return selectedItemBackColor;
            }
            set
            {
                selectedItemBackColor = value;
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

        private List<KeyValuePair<int, string>> dataSource = new List<KeyValuePair<int, string>>();
        public List<KeyValuePair<int, string>> DataSource
        {
            get { return dataSource; }
            set
            {
                dataSource = value;
                items = new List<object>(new object[dataSource.Count]);
                foreach(KeyValuePair<int, string> kvp in dataSource)
                {
                    items[kvp.Key] = kvp.Value;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (list != null && !list.IsOpen)
            {
                if (keyData == Keys.Up)
                {
                    if (selectedIndex > 0)
                    {
                        selectedIndex -= 1;
                        Invalidate();
                        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if (keyData == Keys.Down)
                {
                    if (selectedIndex < items.Count - 1)
                    {
                        selectedIndex += 1;
                        Invalidate();
                        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                Debug.WriteLine("HWnd: " + msg.HWnd.ToString() + " | LParam: " + msg.LParam.ToString() + " | WParam: " +
                    msg.WParam.ToString() + " | Result: " + msg.Result.ToString() + " | Msg: " + msg.Msg.ToString());
                
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        internal void SendMesg(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            SendMessage(hWnd, msg, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //base.OnMouseWheel(e);
            if (e.Delta > 0)
            {
                if (selectedIndex > 0)
                {
                    SelectedIndex -= 1;
                }
            }
            else if (e.Delta < 0)
            {
                if (selectedIndex < items.Count - 1)
                {
                    SelectedIndex += 1;
                }
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
                    Rectangle area = new Rectangle(0, 0, Width - 1, Height - 1);
                    dropDown = new Rectangle(Width - 18, area.X + 1, 17, Height - 2);

                    if (DropDownStyle == ComboBoxStyle.DropDownList || (DropDownStyle == ComboBoxStyle.DropDown && dropDown.Contains(e.Location)))
                    {
                        if (list == null || !list.IsOpen || !list.Visible)
                        {
                            list = new DropDownList(this);
                            if (!DropDownAutoSize)
                                list.SetWidth(DropDownWidth);
                            Point relativeLocation = ControlLocation.RelativeToWindowLocation(this);
                            if(list.FitDown(this.FindForm().Location.Y + relativeLocation.Y + Height + 31))
                                list.Location = new Point(this.FindForm().Location.X + relativeLocation.X + 8, this.FindForm().Location.Y + relativeLocation.Y + Height + 31);
                            else
                                list.Location = new Point(this.FindForm().Location.X + relativeLocation.X + 8, this.FindForm().Location.Y + relativeLocation.Y + Height + 31 - this.Height - list.Height);

                            list.Show();
                            list.BringToFront();
                            list.Focus();
                            list.IsOpen = true;
                            DropDown?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            list.Hide();
                            list.IsOpen = false;
                            lastFocusLost = DateTime.Now;
                            CloseUp?.Invoke(Parent, EventArgs.Empty);
                        }
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

        private ComboBoxStyle comboBoxStyle = ComboBoxStyle.DropDown;
        [DefaultValue(typeof(ComboBoxStyle), "DropDown")]
        public ComboBoxStyle DropDownStyle
        { 
            get { return comboBoxStyle; }
            set
            {
                comboBoxStyle = value;
                if (value == ComboBoxStyle.DropDown)
                    textBox.Visible = true;
                else
                    textBox.Visible = false;
                Invalidate();
            }
        }

        public event EventHandler SelectedIndexChanged;
        public event EventHandler CloseUp;

        private int selectedIndex = -1;
        public object SelectedItem
        { 
            get 
            {
                return selectedIndex == -1 ? null : Items[selectedIndex];
            }
            set
            {
                int x = -1;

                if (Items != null)
                {
                    //bug (82115)
                    if (value != null)
                        x = Items.IndexOf(value);
                    else
                        selectedIndex = -1;
                }

                if (x != -1)
                {
                    selectedIndex = x;
                    Text = items[x] as string;
                    if (list != null)
                    {
                        _markedItem = x;
                    }
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private Point selection = new Point(0, 0);
        public void Select(int start, int length)
        {
            selection = new Point(start, length);
            //Invalidate();
        }

        int dropDownWidth = 20;
        public int DropDownWidth
        {
            get
            {
                return dropDownWidth;
            }
            set
            {
                DropDownAutoSize = false;
                dropDownWidth = value;
                if(list != null) 
                    list.SetWidth(value);
            }
        }

        private bool sorted = false;
        public bool Sorted
        {
            get { return sorted = true; }
            set
            {
                sorted = value;
                if(sorted)
                {
                    items.Sort();
                }
            }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value >= 0 && value < Items.Count)
                {
                    selectedIndex = value;
                    Text = items[value] as string;
                    textBox.Text = Text;
                    if (list != null)
                    {
                        _markedItem = value;
                    }
                    
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
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
                if (selectedIndex == -1 && items.Count > 0)
                    selectedIndex = 0;
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
                using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || _mouseInTextBox || this.Focused || textBox.Focused || (list != null ? list.IsOpen : false)) ? ButtonColorMouseOver : ButtonColor) : ButtonColorDisabled))
                {
                    bg.Graphics.FillRectangle(b, dropDown);
                }
                //dropdown arrow
                using (SolidBrush b = new SolidBrush(Enabled ? ((_mouseIn || _mouseInTextBox || this.Focused || textBox.Focused || (list != null ? list.IsOpen : false)) ? ArrowColorMouseOver : ArrowColor) : SystemColors.ControlDark))
                {
                    bg.Graphics.FillPolygon(b, arrow);
                }
                //rámeček
                using (Pen p = new Pen((_mouseIn || _mouseInTextBox || this.Focused || textBox.Focused || (list != null ? list.IsOpen : false)) ? BorderColorMouseOver : BorderColor))
                {
                    bg.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                }

                if(DropDownStyle == ComboBoxStyle.DropDownList)
                {

                    Size textSize = Size.Empty;
                    string text = "";
                    if (selectedIndex == -1)
                        textSize = TextRenderer.MeasureText("A", Font);
                    else
                    {
                        textSize = TextRenderer.MeasureText(Items[selectedIndex] as string, Font);
                        text = Items[selectedIndex] as string;
                    }
                    TextRenderer.DrawText(bg.Graphics, text, Font, new Rectangle(1, (Height/2) - (textSize.Height/2)-1, Width - 18, Height - 6), Enabled ? foreColor : foreColorDisabled, BackColor, TextFormatFlags.EndEllipsis);
                }
                bg.Render();
            }

            base.OnPaint(e);
        }
    }
}
