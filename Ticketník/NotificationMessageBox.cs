using System.Drawing;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class NotificationMessageBox : Form
    {
        public NotificationMessageBox()
        {
            InitializeComponent();
            Motiv.SetMotiv(this);
        }

        public void Set(string text, string hlava, Image image)
        {
            richTextBox1.Text = text;
            Text = hlava;
            pictureBox1.Image = image;
        }
    }
}
