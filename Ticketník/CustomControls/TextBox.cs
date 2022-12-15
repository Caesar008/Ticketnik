using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Ticketník.CustomControls
{
    /* Tohle se musí přidat do každého Form, kde je custom TextBox
     *
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
     * */
    public class TextBox : System.Windows.Forms.TextBox
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (IsHandleCreated)
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint/* | ControlStyles.UserPaint*/, true);
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
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCPAINT)
            {
                var dc = GetWindowDC(Handle);
                using (Pen p = new Pen((_mouseIn || this.Focused) ? BorderColorMouseOver : BorderColor, 1))
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    {
                        g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
                    }
                }
                //inner box
                using (Pen p = new Pen(BackColor, 1))
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    {
                        g.DrawRectangle(p, 1, 1, Width - 3, Height - 3);
                    }
                }
                //zvýrazněný řádek jako na W11

                using (Pen p = new Pen(this.Focused ? BorderColorMouseOver : BackColor, 1))
                {
                    using (Graphics g = Graphics.FromHdc(dc))
                    {
                        g.DrawLine(p, 1, Height -2, Width - 2, Height - 2);
                    }
                }

                ReleaseDC(Handle, dc);
            }
        }

        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC); 
        private const int WM_NCPAINT = 0x85;
    }
}
