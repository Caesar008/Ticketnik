using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    internal class TextBox : System.Windows.Forms.Control
    {
        private bool _mouseIn = false;
        private bool _mouseInTextBox = false;
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

        private System.Windows.Forms.BorderStyle borderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        [DefaultValue(System.Windows.Forms.BorderStyle.FixedSingle)]
        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; Invalidate(); }
        }

        private System.Windows.Forms.HorizontalAlignment textAlign = System.Windows.Forms.HorizontalAlignment.Center;
        public System.Windows.Forms.HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; Invalidate(); }
        }

        private int maxLength = int.MaxValue;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value;}
        }

        private bool @readonly = false;
        public bool ReadOnly
        {
            get { return @readonly; }
            set 
            { 
                @readonly = value;
                textBox.ReadOnly = value;
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
            if (!_mouseInTextBox)
            {
                _mouseIn = false;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            textBox.Focus();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _mouseIn = true;
            base.OnGotFocus(e);
            textBox.Focus();
            Invalidate();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            _mouseIn = false;
            base.OnLostFocus(e);
            if (!_mouseInTextBox)
            {
                _mouseIn = false;
                Invalidate();
            }
        }

        [Browsable(false)]
        new public ControlCollection Controls => base.Controls;

        private Color backColor= Color.White;
        new public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                textBox.BackColor = backColor;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            textBox.Size = new Size(Width -6, Height -6);
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            textBox.ForeColor = ForeColor;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            textBox.Text = Text; 
        }

        private System.Windows.Forms.TextBox textBox;
        public TextBox() : base()
        {
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
            textBox.Text = this.Text;
            textBox.Font = this.Font;
            Controls.Add(textBox);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            Text = textBox.Text;
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            _mouseInTextBox= false;
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
            _mouseInTextBox = true;
            Invalidate();
        }

        private void TextBox_MouseHover(object sender, EventArgs e)
        {
            _mouseInTextBox = true;
            Invalidate();
        }

        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            _mouseInTextBox = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            using (Graphics g = e.Graphics)
            {
                using (BufferedGraphics bg = BufferedGraphicsManager.Current.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height)))
                {
                    bg.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    using (Pen p = new Pen(((_mouseIn || _mouseInTextBox) || (this.Focused || textBox.Focused)) ? BorderColorMouseOver : BorderColor, 1))
                    {
                        bg.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);

                    }
                    //inner box
                    using (SolidBrush b = new SolidBrush(BackColor))
                    {
                        bg.Graphics.FillRectangle(b, 1, 1, Width - 2, Height - 2);
                    }
                    //zvýrazněný řádek jako na W11

                    using (Pen p = new Pen((this.Focused || textBox.Focused) ? BorderColorMouseOver : BackColor, 1))
                    {
                        bg.Graphics.DrawLine(p, 1, Height - 2, Width - 2, Height - 2);

                    }
                    bg.Render();
                }
            }
            base.OnPaint(e);
        }
    }
}
