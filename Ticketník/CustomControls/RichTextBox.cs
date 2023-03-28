using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník.CustomControls
{
    internal class RichTextBox : System.Windows.Forms.Control
    {
        internal ScrollBar VScrollBar;
        internal ScrollBar HScrollBar;
        private RichTextBoxInternal rtb;

        public RichTextBox() : base()
        {
            VScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Vertical, this);
            HScrollBar = new ScrollBar(ScrollBar.SizeModes.Automatic, ScrollBar.ScrollBarAllignment.Horizontal, this);
            VScrollBar.BackColor = BackColor;
            HScrollBar.BackColor = BackColor;
            VScrollBar.ForeColor = ForeColor;
            HScrollBar.ForeColor = ForeColor;
            VScrollBar.Visible = VScrollBarVisible;
            HScrollBar.Visible = HScrollBarVisible;
            rtb = new RichTextBoxInternal();
            rtb.Location = new System.Drawing.Point(0, 0);
            rtb.Size = this.Size;
            rtb.Text = Text; 
            rtb.Font = Font;
            rtb.ForeColor = ForeColor;
            rtb.BackColor = BackColor;
            rtb.Parent = this;
            if (VScrollBarVisible && HScrollBarVisible)
            {
                VScrollBar.BothVisible = HScrollBar.BothVisible = true;
            }
            Controls.Add(rtb);
            Controls.Add(HScrollBar);
            Controls.Add(VScrollBar);
        }

        [Category("Action")]
        public event EventHandler HScrollBarVisibilityChanged;
        [Category("Action")]
        public event EventHandler VScrollBarVisibilityChanged;

        private bool hScrollVisible = false;
        public bool HScrollBarVisible
        {
            get
            {
                return hScrollVisible;
            }
            internal set
            {
                if (hScrollVisible != value)
                {
                    hScrollVisible = value;
                    HScrollBar.Visible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = false;
                    }
                    rtb.Height = hScrollVisible ? this.Height - HScrollBar.Height : this.Height;
                    HScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool vScrollVisible = false;
        public bool VScrollBarVisible
        {
            get
            {
                return vScrollVisible;
            }
            internal set
            {
                if (vScrollVisible != value)
                {
                    vScrollVisible = value;
                    VScrollBar.Visible = value;
                    if (VScrollBarVisible && HScrollBarVisible)
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = true;
                    }
                    else
                    {
                        VScrollBar.BothVisible = HScrollBar.BothVisible = false;
                    }
                    rtb.Width = vScrollVisible ? this.Width - VScrollBar.Width : this.Width;
                    VScrollBarVisibilityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return rtb.BorderStyle; }
            set { rtb.BorderStyle = value; }
        }

        public bool ReadOnly
        {
            get { return rtb.ReadOnly; }
            set { rtb.ReadOnly = value; }
        }

        public bool WordWrap
        {
            get { return rtb.WordWrap; }
            set
            {
                rtb.WordWrap = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            rtb.Size = new System.Drawing.Size(this.Size.Width - (VScrollBarVisible ? VScrollBar.Width : 0), this.Size.Height - (HScrollBarVisible ? HScrollBar.Height : 0));
        }

        protected override void OnTextChanged(EventArgs e)
        {
            //base.OnTextChanged(e);
            rtb.Text = Text;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            //base.OnFontChanged(e);
            rtb.Font = Font;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            //base.OnBackColorChanged(e);
            rtb.BackColor = BackColor;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            //base.OnForeColorChanged(e); 
            rtb.ForeColor = ForeColor;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
