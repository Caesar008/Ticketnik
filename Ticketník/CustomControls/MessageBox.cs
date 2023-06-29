using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CC = Ticketník.CustomControls;

namespace Ticketník.CustomControls
{
    internal partial class MessageBoxInternal : Form
    {
        //22;45 - menší rozměry textboxu v základu
        //button default 75;23
        public MessageBoxInternal(string message, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            richTextBox1.Text = message;
            this.Text = "";
            ClickableLinks = clickableLinks;
            richTextBox1.WordWrap = true;
            AdjustSize();
        }

        public MessageBoxInternal(string message, string caption, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.WordWrap = true;
            AdjustSize();
        }

        public MessageBoxInternal(string message, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(buttons);
            richTextBox1.WordWrap = true;
            AdjustSize(true);
        }

        public MessageBoxInternal(string message, string caption, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(buttons);
            richTextBox1.WordWrap = true;
            AdjustSize(true);
        }
        public MessageBoxInternal(string message, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.WordWrap = true;
            AdjustSize();
        }

        public MessageBoxInternal(string message, string caption, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.WordWrap = true;
            AdjustSize();
        }

        public MessageBoxInternal(string message, MessageBoxButtons buttons, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(buttons);
            richTextBox1.WordWrap = true;
            AdjustSize(true);
        }

        public MessageBoxInternal(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(buttons);
            richTextBox1.WordWrap = true;
            AdjustSize(true);
        }

        private void SetButtons(MessageBoxButtons buttons)
        {
            CC.Button b1;
            CC.Button b2;
            CC.Button b3;
            Jazyk j = new Jazyk();
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_OK;
                    b1.DialogResult = DialogResult.OK;
                    b1.Location = new Point(Width - 22 - 3 - 75, Height - 45 - 3 - 23);
                    b1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    this.Controls.Add(b1);
                    break;
                case MessageBoxButtons.OKCancel:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_OK;
                    b1.DialogResult = DialogResult.OK;
                    b1.Location = new Point(Width - 22 - 3 - 75 - 75 - 3, Height - 45 - 3 - 23);
                    b1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b2 = new CC.Button();
                    b2.Text = j.Buttons_Cancel;
                    b2.DialogResult = DialogResult.Cancel;
                    b2.Location = new Point(Width - 22 - 3 - 75, Height - 45 - 3 - 23);
                    b2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    this.Controls.Add(b1);
                    this.Controls.Add(b2);
                    break;
                case MessageBoxButtons.YesNo:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_Yes;
                    b1.DialogResult = DialogResult.Yes;
                    b1.Location = new Point(Width - 22 - 3 - 75 - 75 - 3, Height - 45 - 3 - 23);
                    b1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b2 = new CC.Button();
                    b2.Text = j.Buttons_No;
                    b2.DialogResult = DialogResult.No;
                    b2.Location = new Point(Width - 22 - 3 - 75, Height - 45 - 3 - 23);
                    b2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    this.Controls.Add(b1);
                    this.Controls.Add(b2);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_Yes;
                    b1.DialogResult = DialogResult.Yes;
                    b1.Location = new Point(Width - 22 - 3 - 75 - 75 - 3 -75 -3, Height - 45 - 3 - 23);
                    b1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b2 = new CC.Button();
                    b2.Text = j.Buttons_No;
                    b2.DialogResult = DialogResult.No;
                    b2.Location = new Point(Width - 22 - 3 - 75 -75 -3, Height - 45 - 3 - 23);
                    b2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b3 = new CC.Button();
                    b3.Text = j.Buttons_Cancel;
                    b3.DialogResult = DialogResult.Cancel;
                    b3.Location = new Point(Width - 22 - 3 - 75, Height - 45 - 3 - 23);
                    b3.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    this.Controls.Add(b1);
                    this.Controls.Add(b2);
                    this.Controls.Add(b3);
                    break;
                default: break;
            }
        }

        private void AdjustSize(bool buttons = false)
        {
            int but = 0;
            if(buttons)
            {
                but = 23 + 3 + 12;
            }
            int textHight = TextRenderer.MeasureText(richTextBox1.Text, richTextBox1.Font).Height * (richTextBox1.GetLineFromCharIndex(richTextBox1.Text.Length - 1) + 1);
            /*if (textHight + 4 < Height)
            {
                richTextBox1.Height = ((Height - 45 - but) / 2);
            }*/

            if (textHight > richTextBox1.Height - 4 && Width < Screen.PrimaryScreen.Bounds.Width - 16)
            {
                //screen ratio
                double ratio = Math.Round((double)Screen.PrimaryScreen.Bounds.Width / (double)Screen.PrimaryScreen.Bounds.Height, 3);
                double skd = Math.Round(16d / 9d, 3);
                double ckt = Math.Round(4d / 3d, 3);

                if (ratio == skd)
                {
                    Width += 16;
                    Height += 9;
                }

                AdjustSize(buttons);
            }
        }

        private bool clickableLinks = true;
        public bool ClickableLinks
        {
            get { return clickableLinks; }
            set { clickableLinks = value; }
        }
    }


    public static class MessageBox
    {
        public static DialogResult Show(string message, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, clickableLinks);
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, clickableLinks);
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, buttons, clickableLinks);
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, buttons, clickableLinks);
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }
    }

}
