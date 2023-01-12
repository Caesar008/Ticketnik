using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    public class RadioButton : System.Windows.Forms.RadioButton
    {
        //celé 13 i s rámečkem, vnitřní 5
        private bool _mouseIn = false;

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
        private Color boxColor = SystemColors.Window;
        [DefaultValue(typeof(SystemColors), "Window")]
        public Color BoxColor
        {
            get { return boxColor; }
            set
            {
                if (boxColor != value)
                {
                    boxColor = value;
                    Invalidate();
                }
            }
        }
        private Color boxColorMouseOver = Color.FromArgb(70, 70, 100);
        [DefaultValue("#464664")]
        public Color BoxColorMouseOver
        {
            get { return boxColorMouseOver; }
            set
            {
                if (boxColorMouseOver != value)
                {
                    boxColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        private Color checkedColor = Color.LimeGreen;
        [DefaultValue("#005fb8")]
        public Color CheckedColor
        {
            get { return checkedColor; }
            set
            {
                if (checkedColor != value)
                {
                    checkedColor = value;
                    Invalidate();
                }
            }
        }
        private Color checkedColorMouseOver = Color.LimeGreen;
        [DefaultValue("#196ebf")]
        public Color CheckedColorMouseOver
        {
            get { return checkedColorMouseOver; }
            set
            {
                if (checkedColorMouseOver != value)
                {
                    checkedColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _mouseIn = true;
            base.OnMouseEnter(eventargs);
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _mouseIn = false;
            base.OnMouseLeave(eventargs);
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
            if (FlatStyle == FlatStyle.Standard)
            {
                using (Pen p = new Pen(_mouseIn ? BorderColorMouseOver : BorderColor, 1))
                {
                    //vymazat původní
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(0, 1, 13, 13);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(new SolidBrush(BackColor), -1, 1, 14, 14);
                    //vyplnit
                    e.Graphics.FillEllipse(new SolidBrush(_mouseIn ? BoxColorMouseOver : BoxColor), 0, 1, 13, 13);
                    //rámeček
                    e.Graphics.DrawPath(p, gp);
                    if (Checked)
                        e.Graphics.FillEllipse(new SolidBrush(_mouseIn ? CheckedColorMouseOver : CheckedColor), 3, 4, 7, 7);
                }
            }
        }
    }
}
