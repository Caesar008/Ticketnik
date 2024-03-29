﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    public class CheckBox : System.Windows.Forms.CheckBox
    {
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
        private Color boxColorMouseOver = Color.FromArgb(70,70,100);
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
        private Color checkedColor = Color.FromArgb(0, 95, 184);
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
        private Color checkMarkColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color CheckMarkColor
        {
            get { return checkMarkColor; }
            set
            {
                if (checkMarkColor != value)
                {
                    checkMarkColor = value;
                    Invalidate();
                }
            }
        }
        private Color checkMarkColorMouseOver = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color CheckMarkColorMouseOver
        {
            get { return checkMarkColorMouseOver; }
            set
            {
                if (checkMarkColorMouseOver != value)
                {
                    checkMarkColorMouseOver = value;
                    Invalidate();
                }
            }
        }
        private Color checkedColorMouseOver = Color.FromArgb(25, 110, 191);
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
            _mouseIn= false;
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
                    Point topLeft = new Point(0, 0);
                    if (!AutoSize)
                    {
                        switch(CheckAlign)
                        {
                            case ContentAlignment.TopLeft: topLeft.Y = 1; break;
                            case ContentAlignment.MiddleLeft: topLeft.Y = (Height / 2) - 7; break;
                            case ContentAlignment.BottomLeft: topLeft.Y = Height - 15; break;
                            case ContentAlignment.TopCenter: topLeft = new Point((Width/2)-7, 1); break;
                            case ContentAlignment.MiddleCenter: topLeft = new Point((Width / 2) - 7, (Height / 2) - 7); break;
                            case ContentAlignment.BottomCenter: topLeft = new Point((Width / 2) - 7, Height - 15); break;
                            case ContentAlignment.TopRight: topLeft = new Point(Width - 15, 1); break;
                            case ContentAlignment.MiddleRight: topLeft = new Point(Width - 15, (Height / 2) - 7); break;
                            case ContentAlignment.BottomRight: topLeft = new Point(Width - 15, Height - 15); break;
                            default: break;
                        }
                    }
                    //vymazat původní čtverec
                    if (!Checked)
                    {
                        GraphicsPath gp = new GraphicsPath();
                        gp.AddRectangle(new Rectangle(0 + topLeft.X, 0+topLeft.Y, 14, 14));
                        e.Graphics.FillPath(new SolidBrush(BackColor), gp);
                        //vyplnit
                        using (SolidBrush brush = new SolidBrush(_mouseIn ? BoxColorMouseOver : BoxColor))
                        {
                            e.Graphics.FillPath(brush, RoundedRect(new Rectangle(0 + topLeft.X, 1 + topLeft.Y, 12, 12), 2, 2, 2, 2));
                        }
                    }
                    else
                    {
                        GraphicsPath gp = new GraphicsPath();
                        gp.AddRectangle(new Rectangle(0 + topLeft.X, 0 + topLeft.Y, 14, 14));
                        e.Graphics.FillPath(new SolidBrush(BackColor), gp);
                        //vyplnit
                        using (SolidBrush brush = new SolidBrush(_mouseIn ? CheckedColorMouseOver : CheckedColor))
                        {
                            e.Graphics.FillPath(brush, RoundedRect(new Rectangle(0 + topLeft.X, 1 + topLeft.Y, 12, 12), 2, 2, 2, 2));
                        }
                        //udělat checkmark
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        using (Pen chm = new Pen(_mouseIn ? CheckMarkColorMouseOver : CheckMarkColor, 1))
                        {
                            e.Graphics.DrawLine(chm, 3 + topLeft.X, 7 + topLeft.Y, 5 + topLeft.X, 9 + topLeft.Y);
                            e.Graphics.DrawLine(chm, 5 + topLeft.X, 9 + topLeft.Y, 9 + topLeft.X, 5 + topLeft.Y);
                        }
                    }
                    //vytvořit rámeček
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawPath(p, RoundedRect(new Rectangle(0 + topLeft.X, 1 + topLeft.Y, 12, 12), 2, 2, 2, 2));
                }

            }
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
    }
}
