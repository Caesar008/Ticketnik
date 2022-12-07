﻿using System;
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
                    { "pozadíControl", SystemColors.Window },
                    { "tlačítkaRámeček", Color.LightGray },
                    { "myšOver", SystemColors.GradientInactiveCaption },
                    { "pozadíControlPush", SystemColors.GradientActiveCaption }
                }
            },
            { "tmavý", new Dictionary<string, Color>()
                {
                    { "pozadí", Color.FromArgb(30, 30, 30) },
                    { "text", SystemColors.Control },
                    { "rámeček", Color.FromArgb(70, 70, 70) },
                    { "pozadíControl", Color.FromArgb(50, 50, 50) },
                    { "tlačítkaRámeček", Color.DimGray },
                    { "myšOver", Color.FromArgb(70, 70, 100) },
                    { "pozadíControlPush", Color.FromArgb(90, 90, 120) }
                }
            }
        };

        private static void SetControlColor(object c)
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
            if (c.GetType() != typeof(Button) && c.GetType() != typeof(ComboBox)
                && c.GetType() != typeof(NumericUpDown) && c.GetType() != typeof(ListView)
                && c.GetType() != typeof(MenuStrip) && c.GetType() != typeof(ToolStripDropDownItem)
                && c.GetType() != typeof(ToolStripMenuItem) && c.GetType() != typeof(ToolStripSeparator))
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
            else if (c.GetType() == typeof(Button))
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
                ((Button)c).FlatStyle = FlatStyle.Flat;
                ((Button)c).FlatAppearance.BorderColor = barvy[sMotiv]["tlačítkaRámeček"];
                ((Button)c).FlatAppearance.MouseOverBackColor = barvy[sMotiv]["myšOver"];
                ((Button)c).FlatAppearance.MouseDownBackColor = barvy[sMotiv]["pozadíControlPush"];

            }
            else if (c.GetType() == typeof(ComboBox))
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
                ((ComboBox)c).FlatStyle = FlatStyle.Flat;
            }
            else if (c.GetType() == typeof(NumericUpDown))
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
            }
            else if (c.GetType() == typeof(ListView))
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
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
            if(form.MainMenuStrip != null)
            {
                SetControlColor(form.MainMenuStrip);
            }
        }
    }
}
