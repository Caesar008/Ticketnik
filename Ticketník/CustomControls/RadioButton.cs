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
    public class RadioButton : System.Windows.Forms.RadioButton
    {
        //celé 13 i s rámečkem, vnitřní 5
        private Color _borderTemp = Color.Gray;
        private Color _boxTemp = SystemColors.Window;
        private Color _checkedTemp = Color.LimeGreen;
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
            if (Checked)
            {
                if (_mouseIn)
                {
                    CheckedColor = CheckedColorMouseOver;
                    BoxColor = BoxColorMouseOver;
                }
                else
                {
                    CheckedColor = _checkedTemp;
                    BoxColor = _boxTemp;
                }
            }
            else
            {
                if (_mouseIn)
                {
                    BoxColor = BoxColorMouseOver;
                }
                else
                {
                    BoxColor = _boxTemp;
                }
            }
        }
        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _mouseIn = true;
            base.OnMouseEnter(eventargs);
            _checkedTemp = CheckedColor;
            CheckedColor = CheckedColorMouseOver;
            _boxTemp = BoxColor;
            BoxColor = BoxColorMouseOver;
            _boxTemp = BoxColor;
            _checkedTemp = CheckedColor;
            BoxColor = BoxColorMouseOver;
            _borderTemp = BorderColor;
            BorderColor = BorderColorMouseOver;
        }
        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _mouseIn = false;
            base.OnMouseLeave(eventargs); 
            CheckedColor = _checkedTemp;
            BoxColor = _boxTemp;
            BorderColor = _borderTemp;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
        }
    }
}
