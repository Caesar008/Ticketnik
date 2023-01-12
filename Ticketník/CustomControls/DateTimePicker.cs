using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník.CustomControls
{
    public class DateTimePicker : System.Windows.Forms.DateTimePicker
    {
        private Color backColor = Color.White;
        [DefaultValue(typeof(Color), "White"), Browsable(true)]
        public Color BackgroundColor
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
        private bool _mouseIn = false;
        private bool _lMouseDown = false;

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
        /*protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var clientRect = ClientRectangle;
                var dropDownButtonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
                var outerBorder = new Rectangle(clientRect.Location,
                    new Size(clientRect.Width - 1, clientRect.Height - 1));
                var innerBorder = new Rectangle(outerBorder.X + 2, outerBorder.Y + 2,
                    outerBorder.Width - dropDownButtonWidth - 3, outerBorder.Height - 3);
                var dropDownRect = new Rectangle(innerBorder.Right + 1, innerBorder.Y - 1,
                    dropDownButtonWidth, innerBorder.Height + 2);

                if (e.Location != dropDownRect.Location)
                {
                    MouseEventArgs me = new MouseEventArgs(e.Button, e.Clicks, dropDownRect.Right - dropDownRect.Width / 2, dropDownRect.Top - dropDownRect.Height / 2, e.Delta);
                    base.OnMouseDown(me);
                }
                else
                    base.OnMouseDown(e);
            }
            else
                base.OnMouseDown(e);
        }*/

        protected override void WndProc(ref Message m)
        {
            var clientRect = ClientRectangle;
            var dropDownButtonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
            var outerBorder = new Rectangle(clientRect.Location,
                new Size(clientRect.Width - 1, clientRect.Height - 1));
            var innerBorder = new Rectangle(outerBorder.X + 2, outerBorder.Y + 2,
                outerBorder.Width - dropDownButtonWidth - 3, outerBorder.Height - 3);
            var innerInnerBorder = new Rectangle(innerBorder.X + 1, innerBorder.Y + 1,
                innerBorder.Width - 2, innerBorder.Height - 2);
            var dropDownRect = new Rectangle(innerBorder.Right + 1, innerBorder.Y - 1,
                dropDownButtonWidth, innerBorder.Height + 2);

            if (m.Msg == Messages.OnPaint)
            {
                if (RightToLeft == RightToLeft.Yes)
                {
                    innerBorder.X = clientRect.Width - innerBorder.Right;
                    innerInnerBorder.X = clientRect.Width - innerInnerBorder.Right;
                    dropDownRect.X = clientRect.Width - dropDownRect.Right;
                    dropDownRect.Width += 1;
                }
                var innerBorderColor = Enabled ? BackColor : SystemColors.Control;
                var outerBorderColor = Enabled ? ((_mouseIn || this.Focused) ? BorderColorMouseOver : BorderColor) : SystemColors.ControlDark;
                var arrowColor = Enabled ? ((_mouseIn || this.Focused) ? ArrowColorMouseOver : ArrowColor) : SystemColors.ControlDark;
                var buttonColor = Enabled ? ((_mouseIn || this.Focused) ? ButtonColorMouseOver : ButtonColor) : SystemColors.Control;
                var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
                    dropDownRect.Top + dropDownRect.Height / 2);
                var arrow = new Point[]
                {
                new Point(middle.X - 3, middle.Y - 2),
                new Point(middle.X + 4, middle.Y - 2),
                new Point(middle.X, middle.Y + 2)
                };
                var ps = new PAINTSTRUCT();
                bool shoulEndPaint = false;
                IntPtr dc;
                if (m.WParam == IntPtr.Zero)
                {
                    dc = BeginPaint(Handle, ref ps);
                    m.WParam = dc;
                    shoulEndPaint = true;
                }
                else
                {
                    dc = m.WParam;
                }
                var rgn = CreateRectRgn(innerInnerBorder.Left, innerInnerBorder.Top,
                    innerInnerBorder.Right, innerInnerBorder.Bottom);
                SelectClipRgn(dc, rgn);
                DefWndProc(ref m);
                DeleteObject(rgn);
                rgn = CreateRectRgn(clientRect.Left, clientRect.Top,
                    clientRect.Right, clientRect.Bottom);
                SelectClipRgn(dc, rgn);
                using (var g = Graphics.FromHdc(dc))
                {
                    using (var b = new SolidBrush(buttonColor))
                    {
                        g.FillRectangle(b, dropDownRect);
                    }
                    using (var b = new SolidBrush(arrowColor))
                    {
                        g.FillPolygon(b, arrow);
                    }
                    /*using (var p = new Pen(innerBorderColor, 2))
                    {
                        g.DrawRectangle(p, innerBorder);
                        //g.DrawRectangle(p, innerInnerBorder);
                        using (var b = new SolidBrush(innerBorderColor))
                        {
                            g.FillRectangle(b, innerBorder);
                        }
                    }*/
                    using (var p = new Pen(outerBorderColor))
                    {
                        g.DrawRectangle(p, outerBorder);
                    }
                    //TextRenderer.DrawText(g, Text, Font, new Point(3, 3), ForeColor);*/
                }
                if (shoulEndPaint)
                    EndPaint(Handle, ref ps);
                DeleteObject(rgn);
            }
            else
                base.WndProc(ref m);
        }

        protected override void OnDropDown(EventArgs e)
        {
            var hWnd = SendMessage(this.Handle, Messages.GetMonthCalendar, 0, 0);
            if (hWnd != IntPtr.Zero)
            {
                SetWindowTheme(hWnd, String.Empty, String.Empty);
                Graphics g = Graphics.FromHwnd(hWnd);
                //g.DrawRectangle
                TextRenderer.DrawText(g, "test", Font, new Point(3, 3), Color.Violet);
            }
            else
                base.OnDropDown(e);
        }

        /*protected override void OnHandleCreated(EventArgs e)
        {
            //SetWindowTheme(this.Handle, "", "");
            base.OnHandleCreated(e);
        }*/

        public enum MontControlIndexes : int
        {

            MCSC_TEXT = 1,
            MCSC_TITLEBK = 2,
            MCSC_TITLETEXT = 3,
            MCSC_MONTHBK = 4,
            MCSC_TRAILINGTEXT = 5
        }

        public enum MonCalView : int
        {
            MCMV_MONTH = 0,
            MCMV_YEAR = 1,
            MCMV_DECADE = 2,
            MCMV_CENTURY = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int L, T, R, B;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd,
            [In, Out] ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("gdi32.dll")]
        public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

        [DllImport("user32.dll")]
        public static extern int GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase); 
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);
        public enum RegionFlags
        {
            ERROR = 0,
            NULLREGION = 1,
            SIMPLEREGION = 2,
            COMPLEXREGION = 3,
        }
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);
    }
}

