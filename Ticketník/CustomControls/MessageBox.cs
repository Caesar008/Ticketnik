using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
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
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(MessageBoxButtons.OK);
            DetectLink();
            AdjustSize(true);
        }

        public MessageBoxInternal(string message, string caption, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(MessageBoxButtons.OK);
            DetectLink();
            AdjustSize(true);
        }

        public MessageBoxInternal(string message, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            InitializeComponent();
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(buttons);
            DetectLink();
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
            DetectLink();
            AdjustSize(true);
        }
        public MessageBoxInternal(string message, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = "";
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(MessageBoxButtons.OK);
            SetIcon(icon);
            DetectLink();
            AdjustSize();
        }

        public MessageBoxInternal(string message, string caption, MessageBoxIcon icon, bool clickableLinks = true)
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            ClickableLinks = clickableLinks;
            richTextBox1.Text = message;
            this.Text = caption;
            richTextBox1.Height = Height - 45 - 12 - 23 - 3; //velikost okna - prvky okna - odsazení odspodu - button - odsazení buttonu
            SetButtons(MessageBoxButtons.OK);
            SetIcon(icon);
            DetectLink();
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
            SetIcon(icon);
            DetectLink();
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
            SetIcon(icon);
            DetectLink();
            AdjustSize(true);
        }

        private void SetIcon(MessageBoxIcon icon)
        {
            Icon i = GetIcon(icon);
            richTextBox1.Width -= 60;
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(48, richTextBox1.Height);
            pictureBox.Location = new Point(8, 2);
            pictureBox.Image = i.ToBitmap();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            richTextBox1.Location = new Point(62, 2);
            pictureBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            this.Controls.Add(pictureBox);
        }

        private Icon GetIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Information: return SystemIcons.Information;
                case MessageBoxIcon.Error: return SystemIcons.Error;
                case MessageBoxIcon.Exclamation: return SystemIcons.Exclamation;
                case MessageBoxIcon.Question: return SystemIcons.Question;
                default: return null;
            }
        }

        private void DetectLink()
        {
            if(!ClickableLinks)
            {
                richTextBox1.LinkArea = new LinkArea(0, 0);
                return;
            }
            string pattern = @"(?:(?:https?|ftp|file)://|www\d?\.|ftp\.)(?:\([-A-Za-z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Za-z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Za-z0-9+&@#/%=~_|$?!:,.]*\)|[A-Za-z0-9+&@#/%=~_|$])";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(richTextBox1.Text);
            if (matches.Count > 0)
            {
                richTextBox1.LinkColor = richTextBox1.VisitedLinkColor = richTextBox1.ActiveLinkColor = richTextBox1.ForeColor;
                richTextBox1.LinkClicked += RichTextBox1_LinkClicked;

                foreach (Match match in matches)
                {
                    int linkStart = match.Index;
                    int length = match.Value.Length;
                    richTextBox1.Links.Add(match.Index, match.Length, match.Value);
                }
            }
            else
                richTextBox1.LinkArea = new LinkArea(0, 0);
        }

        private void RichTextBox1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData.ToString());
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
                case MessageBoxButtons.AbortRetryIgnore:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_Abort;
                    b1.DialogResult = DialogResult.Abort;
                    b1.Location = new Point(Width - 22 - 3 - 75 - 75 - 3 - 75 - 3, Height - 45 - 3 - 23);
                    b1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b2 = new CC.Button();
                    b2.Text = j.Buttons_Retry;
                    b2.DialogResult = DialogResult.Retry;
                    b2.Location = new Point(Width - 22 - 3 - 75 - 75 - 3, Height - 45 - 3 - 23);
                    b2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;

                    b3 = new CC.Button();
                    b3.Text = j.Buttons_Ignore;
                    b3.DialogResult = DialogResult.Ignore;
                    b3.Location = new Point(Width - 22 - 3 - 75, Height - 45 - 3 - 23);
                    b3.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    this.Controls.Add(b1);
                    this.Controls.Add(b2);
                    this.Controls.Add(b3);
                    break;
                case MessageBoxButtons.RetryCancel:
                    b1 = new CC.Button();
                    b1.Text = j.Buttons_Retry;
                    b1.DialogResult = DialogResult.Retry;
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
                default: break;
            }
        }

        private void AdjustSize(bool buttons = false)
        {
            string[] slova = richTextBox1.Text.Split(' ');
            int add = 0;
            foreach(string s in slova)
            {
                Size smallSize = TextRenderer.MeasureText(s, richTextBox1.Font);
                if (smallSize.Width > richTextBox1.Width)
                    add+=smallSize.Height * ((smallSize.Width / richTextBox1.Width) + 1);
            }
            Size velikost = TextRenderer.MeasureText(richTextBox1.Text, richTextBox1.Font, richTextBox1.Size, TextFormatFlags.TextBoxControl | TextFormatFlags.ExpandTabs | TextFormatFlags.WordBreak);
            int textHight = velikost.Height + add;// * radky;

            if (textHight > richTextBox1.Height)
            {
                //screen ratio
                double ratio = Math.Round((double)Screen.PrimaryScreen.Bounds.Width / (double)Screen.PrimaryScreen.Bounds.Height, 3);
                double skd = Math.Round(16d / 9d, 3);
                double ckt = Math.Round(4d / 3d, 3);
                double hkd = Math.Round(16d / 10d, 3);

                if (ratio == skd)
                {
                    if(Width < Screen.PrimaryScreen.Bounds.Width - 16)
                        Width += 16;
                    Height += 9;
                }
                else if(ratio == ckt)
                {
                    if (Width < Screen.PrimaryScreen.Bounds.Width - 4)
                        Width += 4;
                    Height += 3;
                }
                else if (ratio == hkd)
                {
                    if (Width < Screen.PrimaryScreen.Bounds.Width - 16)
                        Width += 16;
                    Height += 10;
                }
                else
                {
                    if (Width < Screen.PrimaryScreen.Bounds.Width - 16)
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

        private void richTextBox1_ForeColorChanged(object sender, EventArgs e)
        {
            richTextBox1.LinkColor = richTextBox1.VisitedLinkColor = richTextBox1.ActiveLinkColor = richTextBox1.ForeColor;
        }
    }


    public static class MessageBox
    {
        public static DialogResult Show(string message, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, buttons, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, buttons, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, MessageBoxIcon icon, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, icon, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, MessageBoxIcon icon, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, icon, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, MessageBoxButtons buttons, MessageBoxIcon icon, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, buttons, icon, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, bool clickableLinks = true)
        {
            MessageBoxInternal messageBox = new MessageBoxInternal(message, caption, buttons, icon, clickableLinks);
            messageBox.StartPosition = FormStartPosition.CenterScreen;
            Motiv.SetMotiv(messageBox);
            return messageBox.ShowDialog();
        }
    }

}
