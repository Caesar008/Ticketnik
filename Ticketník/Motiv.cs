using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Ticketník
{
    internal static class Motiv
    {
        private static readonly Dictionary<string, Dictionary<string, Color>> barvy = new Dictionary<string, Dictionary<string, Color>>()
        {
            { "světlý", new Dictionary<string, Color>()
                {
                    { "pozadí", SystemColors.Control },
                    { "text", SystemColors.ControlText },
                    { "rámeček", Color.Gainsboro },
                    { "tlačítka", SystemColors.Window },
                    { "tlačítkaRámeček", Color.LightGray },
                    { "tlačítkaOver", SystemColors.GradientInactiveCaption },
                    { "tlačítkaPush", SystemColors.GradientActiveCaption }
                }
            },
            { "tmavý", new Dictionary<string, Color>()
                {
                    { "pozadí", Color.FromArgb(30, 30, 30) },
                    { "text", SystemColors.Control },
                    { "rámeček", Color.FromArgb(70, 70, 70) },
                    { "tlačítka", Color.FromArgb(50, 50, 50) },
                    { "tlačítkaRámeček", Color.DimGray },
                    { "tlačítkaOver", Color.FromArgb(70, 70, 100) },
                    { "tlačítkaPush", Color.FromArgb(90, 90, 120) }
                }
            }
        };

        private static void SetControlColor(Control c)
        {
            string sMotiv = "světlý";
            switch(Properties.Settings.Default.motiv)
            { 
                case 0: 
                    sMotiv = "světlý";
                    break;
                case 1: sMotiv = "tmavý";
                    break;
            }
            if (c.GetType() != typeof(Button) && c.GetType() != typeof(ComboBox) && c.GetType() != typeof(NumericUpDown)
                && c.GetType() != typeof(ListView) )
            {
                c.BackColor = barvy[sMotiv]["pozadí"];
                c.ForeColor = barvy[sMotiv]["text"];
                foreach (Control cc in c.Controls)
                {
                    //barvy v nastavení
                    if (cc.Name != "vyreseno" && cc.Name != "ceka" && cc.Name != "odpoved" && cc.Name != "rdp" && cc.Name != "probiha" &&
                                cc.Name != "prescas" && cc.Name != "textLow" && cc.Name != "textMid" && cc.Name != "textHigh" && cc.Name != "textOK")
                        SetControlColor(cc);
                }
            }
            else
            {
                c.BackColor = barvy[sMotiv]["tlačítka"];
                c.ForeColor = barvy[sMotiv]["text"];
                if (c.GetType() == typeof(Button))
                {
                    ((Button)c).FlatStyle = FlatStyle.Flat;
                    ((Button)c).FlatAppearance.BorderColor = barvy[sMotiv]["tlačítkaRámeček"];
                    ((Button)c).FlatAppearance.MouseOverBackColor = barvy[sMotiv]["tlačítkaOver"];
                    ((Button)c).FlatAppearance.MouseDownBackColor = barvy[sMotiv]["tlačítkaPush"];
                }
            }
        }

        internal static void SetGroupBoxRamecek(GroupBox groupBox, PaintEventArgs e)
        {
            string sMotiv = "světlý";
            switch (Properties.Settings.Default.motiv)
            {
                case 0:
                    sMotiv = "světlý";
                    break;
                case 1:
                    sMotiv = "tmavý";
                    break;
            }
            Graphics gfx = e.Graphics;
            Pen p = new Pen(barvy[sMotiv]["rámeček"], 1);
            gfx.DrawLine(p, 0, 6, 0, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, 0, 6, 6, 6);
            gfx.DrawLine(p, System.Windows.Forms.TextRenderer.MeasureText(groupBox.Text, groupBox.Font).Width + 4, 6, e.ClipRectangle.Width - 2, 6);
            gfx.DrawLine(p, e.ClipRectangle.Width - 1, 6, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
        }

        internal static void SetMotiv(Form form)
        {
            string sMotiv = "světlý";
            switch (Properties.Settings.Default.motiv)
            {
                case 0:
                    sMotiv = "světlý";
                    break;
                case 1:
                    sMotiv = "tmavý";
                    break;
            }
            form.BackColor = barvy[sMotiv]["pozadí"];
            form.ForeColor = barvy[sMotiv]["text"];

            foreach (Control c in form.Controls)
            {
                SetControlColor(c);
            }

        }
    }
}
