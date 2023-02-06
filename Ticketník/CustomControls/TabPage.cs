using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník.CustomControls
{
    internal class TabPage : System.Windows.Forms.Control
    {
        public TabPage(): base()
        {
            Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom | 
                System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
        }

        /*new public Point Location
        {
            get { return new Point(1, ((TabControl)Parent).HeaderHight); }
        }

        new public int Width
        {
            get { return Parent.Width - 2; }
        }

        new public int Height
        {
            get { return Parent.Height - 200 - ((TabControl)Parent).HeaderHight; }
        }*/
    }
}
