﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ticketník.Properties;

namespace Ticketník
{
    public class Paleta : ProfessionalColorTable
    {
        public override Color MenuBorder
        {
            get
            {
                return Motiv.GetMenuBarvy("buttonBorder");
            }
        }

        public override Color MenuStripGradientBegin
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }

        public override Color MenuStripGradientEnd
        {
            get
            {
                return Motiv.GetMenuBarvy("checkBox");
            }
        }
        public override Color ToolStripDropDownBackground
        {
            get
            {
                return Motiv.GetMenuBarvy("checkBox");
            }
        }
        public override Color ImageMarginGradientBegin
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ImageMarginGradientEnd
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ImageMarginGradientMiddle
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color MenuItemPressedGradientBegin
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadíControlPush");
            }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadíControlPush");
            }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadíControlPush");
            }
        }
        public override Color MenuItemSelected
        {
            get
            {
                return Motiv.GetMenuBarvy("controlOver");
            }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get
            {
                return Motiv.GetMenuBarvy("controlOver");
            }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get
            {
                return Motiv.GetMenuBarvy("controlOver");
            }
        }
        public override Color SeparatorDark
        {
            get
            {
                return Motiv.GetMenuBarvy("buttonBorder");
            }
        }
        public override Color SeparatorLight
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ToolStripGradientBegin
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ToolStripGradientEnd
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ToolStripGradientMiddle
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
        public override Color ToolStripBorder
        {
            get
            {
                return Motiv.GetMenuBarvy("pozadí");
            }
        }
    }
    public class MySR : ToolStripSystemRenderer
    {
        public MySR() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);
        }
    }
    internal static class Motiv
    {
        internal static readonly Dictionary<string, Dictionary<string, Color>> barvy = new Dictionary<string, Dictionary<string, Color>>()
        {
            { "světlý", new Dictionary<string, Color>()
                {
                    { "pozadí", Color.FromArgb(240, 240, 240)/*SystemColors.Control*/ },
                    { "text", Color.Black/*SystemColors.ControlText*/ },
                    { "rámeček", Color.Gainsboro },
                    { "pozadíControl", Color.White/*SystemColors.Window*/ },
                    { "arrow", Color.Black },
                    { "controlRámeček", Color.LightGray },
                    { "controlOver", Color.FromArgb(215, 228, 242)/*SystemColors.GradientInactiveCaption*/ },
                    { "pozadíControlPush", Color.FromArgb(185, 209, 234)/*SystemColors.GradientActiveCaption*/ },
                    { "pozadíDisabled", Color.FromArgb(240, 240, 240)/*SystemColors.Control*/ },
                    { "checkBox", Color.White/*SystemColors.Window*/ },
                    { "checkBoxRámeček", Color.Gray },
                    { "checkBoxChecked", Color.FromArgb(0, 95, 184) },
                    { "checkBoxCheckedOver", Color.FromArgb(25, 110, 191) },
                    { "disabledText", Color.FromArgb(160,160,160)/*SystemColors.ControlDark*/ },
                    { "button", Color.Gainsboro },
                    { "buttonBorder", Color.DarkGray }
                }
            },
            { "tmavý", new Dictionary<string, Color>()
                {
                    { "pozadí", Color.FromArgb(30, 30, 30) },
                    { "text", Color.FromArgb(240, 240, 240)/*SystemColors.Control*/ },
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
                    { "disabledText", Color.FromArgb(105, 105, 105 ) /*SystemColors.ControlDarkDark*/},
                    { "button", Color.FromArgb(50, 50, 50) },
                    { "buttonBorder", Color.DimGray }
                }
            }
        };

        internal static void SetControlColor(object c)
        {
            string sMotiv = GetMotiv();
            
            if (c.GetType() == typeof(CustomControls.Button))
            {
                ((CustomControls.Button)c).BackColor = barvy[sMotiv]["button"];
                ((CustomControls.Button)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.Button)c).FlatStyle = FlatStyle.Flat;
                ((CustomControls.Button)c).FlatAppearance.BorderColor = barvy[sMotiv]["buttonBorder"];
                ((CustomControls.Button)c).FlatAppearance.BorderSize = 1;
                ((CustomControls.Button)c).FlatAppearance.MouseOverBackColor = barvy[sMotiv]["controlOver"];
                ((CustomControls.Button)c).FlatAppearance.MouseDownBackColor = barvy[sMotiv]["pozadíControlPush"];
                ((CustomControls.Button)c).BorderColorMouseOver = Color.DodgerBlue;
            }
            else if (c.GetType() == typeof(CustomControls.ComboBox))
            {
                //((CustomControls.ComboBox)c).BackColor = barvy[sMotiv]["button"];
                ((CustomControls.ComboBox)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.ComboBox)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.ComboBox)c).FlatStyle = FlatStyle.Flat;
                ((CustomControls.ComboBox)c).BorderColor = barvy[sMotiv]["buttonBorder"];
                ((CustomControls.ComboBox)c).ButtonColor = barvy[sMotiv]["button"];
                ((CustomControls.ComboBox)c).ArrowColor = barvy[sMotiv]["arrow"];
                ((CustomControls.ComboBox)c).BorderColorMouseOver = Color.DodgerBlue;
                ((CustomControls.ComboBox)c).ButtonColorMouseOver = barvy[sMotiv]["controlOver"];
            }
            else if (c.GetType() == typeof(CustomControls.NumericUpDown))
            {
                ((CustomControls.NumericUpDown)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.NumericUpDown)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.NumericUpDown)c).ButtonHighlightColor = barvy[sMotiv]["controlOver"];
                ((CustomControls.NumericUpDown)c).BorderColor = barvy[sMotiv]["controlRámeček"];
                ((CustomControls.NumericUpDown)c).ButtonHighlightColorDisabled = barvy[sMotiv]["pozadíDisabled"];
                ((CustomControls.NumericUpDown)c).ArrowColor = barvy[sMotiv]["arrow"];

            }
            else if (c.GetType() == typeof(CustomControls.CheckBox))
            {
                ((CustomControls.CheckBox)c).BorderColor = barvy[sMotiv]["checkBoxRámeček"];
                ((CustomControls.CheckBox)c).BoxColor = barvy[sMotiv]["checkBox"];
                ((CustomControls.CheckBox)c).CheckedColor = barvy[sMotiv]["checkBoxChecked"];
                ((CustomControls.CheckBox)c).BoxColorMouseOver = barvy[sMotiv]["controlOver"];
                ((CustomControls.CheckBox)c).CheckedColorMouseOver = barvy[sMotiv]["checkBoxCheckedOver"];
            }
            else if (c.GetType() == typeof(CustomControls.RadioButton))
            {
                ((CustomControls.RadioButton)c).BorderColor = barvy[sMotiv]["checkBoxRámeček"];
                ((CustomControls.RadioButton)c).BoxColor = barvy[sMotiv]["checkBox"];
                ((CustomControls.RadioButton)c).BoxColorMouseOver = barvy[sMotiv]["controlOver"];
            }
            else if (c.GetType() == typeof(CustomControls.TextBox))
            {
                //((CustomControls.TextBox)c).BorderStyle = BorderStyle.FixedSingle;
                ((CustomControls.TextBox)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((CustomControls.TextBox)c).ForeColor = barvy[sMotiv]["text"];
                ((CustomControls.TextBox)c).BorderColor = barvy[sMotiv]["controlRámeček"];
                ((CustomControls.TextBox)c).BorderColorMouseOver = Color.DodgerBlue;
            }
            else if (c.GetType() == typeof(ListView))
            {
                ((ListView)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((ListView)c).ForeColor = barvy[sMotiv]["text"];
                ((ListView)c).BorderStyle = BorderStyle.None;
            }
            else if (c.GetType() == typeof(MenuStrip))
            {
                ((MenuStrip)c).ForeColor = barvy[sMotiv]["text"];
                // ((MenuStrip)c).BackColor = barvy[sMotiv]["pozadí"];
                // ((MenuStrip)c).ForeColor = barvy[sMotiv]["text"];
                 foreach (object tsdi in ((MenuStrip)c).Items)
                 {
                     if (tsdi.GetType() == typeof(ToolStripMenuItem) || tsdi.GetType() == typeof(ToolStripComboBox))
                     {
                         SetControlColor(tsdi);
                     }
                 }
            }
            else if (c.GetType() == typeof(ToolStripDropDownItem))
            {
                ((ToolStripDropDownItem)c).ForeColor = barvy[sMotiv]["text"];
                //((ToolStripDropDownItem)c).BackColor = barvy[sMotiv]["pozadí"];
                foreach (object tsdi in ((ToolStripDropDownItem)c).DropDownItems)
                {
                    SetControlColor(tsdi);
                }
            }
            else if (c.GetType() == typeof(ToolStripMenuItem))
            {
                ((ToolStripMenuItem)c).ForeColor = barvy[sMotiv]["text"];
                //((ToolStripMenuItem)c).BackColor = barvy[sMotiv]["pozadí"];
                ((ToolStripMenuItem)c).ForeColor = barvy[sMotiv]["text"];
                foreach (object tsdi in ((ToolStripMenuItem)c).DropDownItems)
                {
                    SetControlColor(tsdi);
                }
            }
            else if (c.GetType() == typeof(ToolStripSeparator))
            {
                //((ToolStripSeparator)c).BackColor = barvy[sMotiv]["pozadí"];
                //((ToolStripSeparator)c).ForeColor = barvy[sMotiv]["text"];
            }
            else if (c.GetType() == typeof(ToolStripComboBox))
            {
                ((ToolStripComboBox)c).BackColor = barvy[sMotiv]["pozadíControl"];
                ((ToolStripComboBox)c).ForeColor = barvy[sMotiv]["text"];
            }
            else if (c.GetType() == typeof(ToolStripLabel))
            {
                ((ToolStripLabel)c).ForeColor = barvy[sMotiv]["disabledText"];
            }
            else if (c.GetType() == typeof(ToolStrip))
            {
                ((ToolStrip)c).BackColor = barvy[sMotiv]["pozadí"];
                foreach(object tsdi in ((ToolStrip)c).Items)
                {
                    if(tsdi.GetType() != typeof(ToolStripButton))
                        SetControlColor(tsdi);
                }
            }
            else
            {
                ((Control)c).BackColor = barvy[sMotiv]["pozadí"];
                ((Control)c).ForeColor = barvy[sMotiv]["text"];
                
            }

            if (c.GetType() != typeof(ToolStripMenuItem) && c.GetType() != typeof(ToolStripSeparator) &&
                c.GetType() != typeof(ToolStripDropDownItem) && c.GetType() != typeof(ToolStripComboBox))
            {
                foreach (Control cc in ((Control)c).Controls)
                {
                    //barvy v nastavení
                    if (cc.GetType() != typeof(Label) && cc.Name != "vyreseno" && cc.Name != "ceka" && cc.Name != "odpoved" && cc.Name != "rdp" && cc.Name != "probiha" &&
                                cc.Name != "prescas" && cc.Name != "textLow" && cc.Name != "textMid" && cc.Name != "textHigh" && cc.Name != "textOK")
                        SetControlColor(cc);
                }
            }
        }

        internal static Color GetMenuBarvy(string barva)
        {
            string sMotiv = Motiv.GetMotiv();
            return barvy[sMotiv][barva];
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

            if(form.GetType() == typeof(Form1))
            {
                if(sMotiv == "světlý")
                {
                    ((Form1)form).exportovatToolStripMenuItem.Image = Resources.export;
                    ((Form1)form).ukončitToolStripMenuItem.Image = Resources.off;
                }
                else
                {
                    ((Form1)form).exportovatToolStripMenuItem.Image = Resources.exportW;
                    ((Form1)form).ukončitToolStripMenuItem.Image = Resources.offW;
                }
            }

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
