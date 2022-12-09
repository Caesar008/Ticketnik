using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Ticketník
{
    internal static class Motiv
    {
        internal static readonly Dictionary<string, Dictionary<string, Color>> barvy = new Dictionary<string, Dictionary<string, Color>>()
        {
            { "světlý", new Dictionary<string, Color>()
                {
                    { "pozadí", SystemColors.Control },
                    { "text", SystemColors.ControlText },
                    { "rámeček", Color.Gainsboro },
                    { "pozadíControl", SystemColors.Window },
                    { "arrow", Color.Black },
                    { "controlRámeček", Color.LightGray },
                    { "controlOver", SystemColors.GradientInactiveCaption },
                    { "pozadíControlPush", SystemColors.GradientActiveCaption },
                    { "pozadíDisabled", SystemColors.Control },
                    { "checkBox", SystemColors.Window },
                    { "checkBoxRámeček", Color.Gray },
                    { "checkBoxChecked", Color.FromArgb(0, 95, 184) },
                    { "checkBoxCheckedOver", Color.FromArgb(25, 110, 191) },
                    { "disabledText", SystemColors.ControlDark }
                }
            },
            { "tmavý", new Dictionary<string, Color>()
                {
                    { "pozadí", Color.FromArgb(30, 30, 30) },
                    { "text", SystemColors.Control },
                    { "rámeček", Color.FromArgb(70, 70, 70) },
                    { "pozadíControl", Color.FromArgb(50, 50, 50) },
                    { "arrow", Color.DimGray },
                    { "controlRámeček", Color.DimGray },
                    { "controlOver", Color.FromArgb(70, 70, 100) },
                    { "pozadíControlPush", Color.FromArgb(90, 90, 120) },
                    { "pozadíDisabled", Color.FromArgb(50, 50, 50) },
                    { "checkBox", Color.FromArgb(50, 50, 50) },
                    { "checkBoxRámeček", Color.DimGray },
                    { "checkBoxChecked", Color.FromArgb(0, 95, 184) },
                    { "checkBoxCheckedOver", Color.FromArgb(25, 110, 191) },
                    { "disabledText", SystemColors.ControlDarkDark }
                }
            }
        };

        internal static void SetControlColor(object c)
        {
            string sMotiv = GetMotiv();

            if (c.GetType() == typeof(Button))
            {
                ((Button)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((Button)c).ForeColor = barvy[sMotiv]["text"];
                ((Button)c).FlatStyle = FlatStyle.Flat;
                ((Button)c).FlatAppearance.BorderColor = barvy[sMotiv]["controlRámeček"];
                ((Button)c).FlatAppearance.MouseOverBackColor = barvy[sMotiv]["controlOver"];
                ((Button)c).FlatAppearance.MouseDownBackColor = barvy[sMotiv]["pozadíControlPush"];

            }
            else if (c.GetType() == typeof(CustomControls.ComboBox))
            {
                ((CustomControls.ComboBox)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.ComboBox)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.ComboBox)c).FlatStyle = FlatStyle.Flat;
                ((CustomControls.ComboBox)c).BorderColor = barvy[sMotiv]["controlRámeček"];
                ((CustomControls.ComboBox)c).ButtonColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.ComboBox)c).ArrowColor = barvy[sMotiv]["arrow"];
            }
            else if (c.GetType() == typeof(CustomControls.NumericUpDown))
            {
                ((CustomControls.NumericUpDown)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.NumericUpDown)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.NumericUpDown)c).ButtonHighlightColor = barvy[sMotiv]["controlRámeček"];
                ((CustomControls.NumericUpDown)c).BorderColor = barvy[sMotiv]["controlRámeček"];
                ((CustomControls.NumericUpDown)c).ButtonHighlightColorDisabled = barvy[sMotiv]["pozadíDisabled"];

            }
            else if (c.GetType() == typeof(CustomControls.CheckBox))
            {
                ((CustomControls.CheckBox)c).BorderColor = barvy[sMotiv]["checkBoxRámeček"];
                ((CustomControls.CheckBox)c).BoxColor = barvy[sMotiv]["checkBox"];
                ((CustomControls.CheckBox)c).CheckedColor = barvy[sMotiv]["checkBoxChecked"];
                ((CustomControls.CheckBox)c).BoxColorMouseOver = barvy[sMotiv]["controlOver"];
                ((CustomControls.CheckBox)c).CheckedColorMouseOver = barvy[sMotiv]["checkBoxCheckedOver"];
            }
            else if (c.GetType() == typeof(ListView))
            {
                ((ListView)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((ListView)c).ForeColor = barvy[sMotiv]["text"];
                ((ListView)c).BorderStyle = BorderStyle.None;
            }
            else if (c.GetType() == typeof(MenuStrip))
            {
                ((MenuStrip)c).BackColor = barvy[sMotiv]["pozadí"];
                ((MenuStrip)c).ForeColor = barvy[sMotiv]["text"];
                foreach (object tsdi in ((MenuStrip)c).Items)
                {
                    if (tsdi.GetType() == typeof(ToolStripMenuItem))
                    {
                        SetControlColor(tsdi);
                    }
                }
            }
            else if (c.GetType() == typeof(ToolStripDropDownItem))
            {
                ((ToolStripDropDownItem)c).BackColor = barvy[sMotiv]["pozadí"];
                ((ToolStripDropDownItem)c).ForeColor = barvy[sMotiv]["text"];
                foreach (object tsdi in ((ToolStripDropDownItem)c).DropDownItems)
                {
                    SetControlColor(tsdi);
                }
            }
            else if (c.GetType() == typeof(ToolStripMenuItem))
            {
                ((ToolStripMenuItem)c).BackColor = barvy[sMotiv]["pozadí"];
                ((ToolStripMenuItem)c).ForeColor = barvy[sMotiv]["text"];
                foreach (object tsdi in ((ToolStripMenuItem)c).DropDownItems)
                {
                    SetControlColor(tsdi);
                }
            }
            else if (c.GetType() == typeof(ToolStripSeparator))
            {
                ((ToolStripSeparator)c).BackColor = barvy[sMotiv]["pozadí"];
                ((ToolStripSeparator)c).ForeColor = barvy[sMotiv]["text"];
            }
            else
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadí"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
                foreach (Control cc in ((Control)c).Controls)
                {
                    //barvy v nastavení
                    if (cc.Name != "vyreseno" && cc.Name != "ceka" && cc.Name != "odpoved" && cc.Name != "rdp" && cc.Name != "probiha" &&
                                cc.Name != "prescas" && cc.Name != "textLow" && cc.Name != "textMid" && cc.Name != "textHigh" && cc.Name != "textOK")
                        SetControlColor(cc);
                }
            }
        }

        private static string GetMotiv()
        {
            switch (Properties.Settings.Default.motiv)
            {
                case 0:
                    return "světlý";
                case 1:
                    return "tmavý";
                case 2:
                    try
                    {
                        return (int)Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1) == 0 ? "tmavý" : "světlý";
                    }
                    catch
                    {
                        return "světlý";
                    }
                default:
                    return "světlý";
            }
        }

        internal static void SetControlColorOver(object c)
        {
            string sMotiv = GetMotiv();

            if (c.GetType() == typeof(Button))
            {
                ((Button)c).FlatAppearance.BorderColor = Color.DodgerBlue; //barvy[sMotiv]["controlRámeček"];
                ((Button)c).FlatAppearance.MouseOverBackColor = barvy[sMotiv]["controlOver"];
                ((Button)c).FlatAppearance.MouseDownBackColor = barvy[sMotiv]["pozadíControlPush"];

            }
            else if (c.GetType() == typeof(CustomControls.ComboBox))
            {
                ((CustomControls.ComboBox)c).BorderColor = Color.DodgerBlue;
                ((CustomControls.ComboBox)c).ButtonColor = barvy[sMotiv]["controlOver"];
            }
        }

        internal static void SetGroupBoxRamecek(GroupBox groupBox, PaintEventArgs e)
        {
            string sMotiv = GetMotiv();
            using (Graphics gfx = e.Graphics)
            {
                Pen p = new Pen(barvy[sMotiv]["rámeček"], 1);
                gfx.DrawLine(p, 0, 6, 0, e.ClipRectangle.Height - 2);
                gfx.DrawLine(p, 0, 6, 6, 6);
                gfx.DrawLine(p, System.Windows.Forms.TextRenderer.MeasureText(groupBox.Text, groupBox.Font).Width + 4, 6, e.ClipRectangle.Width - 2, 6);
                gfx.DrawLine(p, e.ClipRectangle.Width - 1, 6, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 2);
                gfx.DrawLine(p, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);

            }
        }

        internal static void SetMotiv(Form form)
        {
            string sMotiv = GetMotiv();
            form.BackColor = barvy[sMotiv]["pozadí"];
            form.ForeColor = barvy[sMotiv]["text"];

            foreach (Control c in form.Controls)
            {
                SetControlColor(c);
            }
            if(form.MainMenuStrip != null)
            {
                SetControlColor(form.MainMenuStrip);
            }
        }
    }
}
