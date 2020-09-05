using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class NotificationMessageBox : Form
    {
        public NotificationMessageBox()
        {
            InitializeComponent();
        }

        public void Set(string text, string hlava, Image image)
        {
            richTextBox1.Text = text;
            Text = hlava;
            pictureBox1.Image = image;
        }
    }
}
