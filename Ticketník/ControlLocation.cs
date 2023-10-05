using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ticketník
{
    internal static class ControlLocation
    {
        public static Point RelativeToWindowLocation(Control control)
        {
            int x = 0;
            int y = 0;
            while(control.Parent != null)
            {
                x += control.Location.X;
                y+= control.Location.Y;
                control = control.Parent;
            }
            return new Point(x, y);
        }
    }
}
