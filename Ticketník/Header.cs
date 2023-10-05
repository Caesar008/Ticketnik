using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ticketník.CustomControls;
using System.Runtime.InteropServices;

namespace Ticketník
{
    public partial class Form1
    {
        //tohle je na překreslení rámečku a záhlaví okna
        /*protected override void WndProc(ref Message m)
        {
            if (m.Msg == Messages.WM_NCPAINT)
            {
                IntPtr hdc = GetWindowDC(m.HWnd);
                if ((int)hdc != 0)
                {
                    Graphics g = Graphics.FromHdc(hdc);
                    g.FillRectangle(Brushes.Violet, g.ClipBounds);
                    g.Flush();
                    ReleaseDC(m.HWnd, hdc);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);*/
    }
}
