using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Ticketník.Properties;

namespace Ticketník
{
    public class Paleta : ProfessionalColorTable
    {
        public override Color MenuBorder => Motiv.GetMenuBarvy("buttonBorder");
        public override Color MenuStripGradientBegin => Motiv.GetMenuBarvy("pozadí");
        public override Color MenuStripGradientEnd => Motiv.GetMenuBarvy("checkBox");
        public override Color ToolStripDropDownBackground => Motiv.GetMenuBarvy("checkBox");
        public override Color ImageMarginGradientBegin => Motiv.GetMenuBarvy("pozadí");
        public override Color ImageMarginGradientEnd => Motiv.GetMenuBarvy("pozadí");
        public override Color ImageMarginGradientMiddle => Motiv.GetMenuBarvy("pozadí");
        public override Color MenuItemPressedGradientBegin => Motiv.GetMenuBarvy("pozadíControlPush");
        public override Color MenuItemPressedGradientEnd => Motiv.GetMenuBarvy("pozadíControlPush"); 
        public override Color MenuItemPressedGradientMiddle => Motiv.GetMenuBarvy("pozadíControlPush");
        public override Color MenuItemSelected => Motiv.GetMenuBarvy("controlOver");
        public override Color MenuItemSelectedGradientBegin => Motiv.GetMenuBarvy("controlOver");
        public override Color MenuItemSelectedGradientEnd => Motiv.GetMenuBarvy("controlOver");
        public override Color SeparatorDark => Motiv.GetMenuBarvy("buttonBorder");
        public override Color SeparatorLight => Motiv.GetMenuBarvy("pozadí");
        public override Color ToolStripGradientBegin => Motiv.GetMenuBarvy("pozadí");
        public override Color ToolStripGradientEnd => Motiv.GetMenuBarvy("pozadí");
        public override Color ToolStripGradientMiddle => Motiv.GetMenuBarvy("pozadí");
        public override Color ToolStripBorder => Motiv.GetMenuBarvy("pozadí");
        public override Color ButtonPressedGradientBegin => Motiv.GetMenuBarvy("pozadíControlPush");
        public override Color ButtonPressedGradientEnd => Motiv.GetMenuBarvy("pozadíControlPush");
        public override Color ButtonPressedGradientMiddle => Motiv.GetMenuBarvy("pozadíControlPush");
        public override Color ButtonSelectedGradientBegin => Motiv.GetMenuBarvy("controlOver");
        public override Color ButtonSelectedGradientEnd => Motiv.GetMenuBarvy("controlOver");
        public override Color ButtonSelectedGradientMiddle => Motiv.GetMenuBarvy("controlOver");
    }
    public class MySR : ToolStripProfessionalRenderer
    {
        public MySR(ProfessionalColorTable professionalColorTable) : base(professionalColorTable) { }

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
                    { "buttonBorder", Color.DarkGray },
                    { "tabPozadí", Color.FromArgb(244, 244, 244) },
                    { "tabPozadíAktivní", Color.White },
                    { "dayHeaderText",Color.FromArgb(55, 55, 55) }
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
                    { "pozadíDisabled", Color.FromArgb(30, 30, 30) },
                    { "checkBox", Color.FromArgb(50, 50, 50) },
                    { "checkBoxRámeček", Color.DimGray },
                    { "checkBoxChecked", Color.FromArgb(0, 95, 184) },
                    { "checkBoxCheckedOver", Color.FromArgb(25, 110, 191) },
                    { "disabledText", Color.FromArgb(105, 105, 105 ) /*SystemColors.ControlDarkDark*/},
                    { "button", Color.FromArgb(50, 50, 50) },
                    { "buttonBorder", Color.DimGray },
                    { "tabPozadí", Color.FromArgb(50, 50, 50) },
                    { "tabPozadíAktivní",Color.FromArgb(70, 70, 70) },
                    { "dayHeaderText",Color.FromArgb(200, 200, 200) }
                }
            }
        };

        internal static void SetWindowControlColor(object c)
        {
            string sMotiv = GetMotiv();
            if (c.GetType() == typeof(RichTextBox))
            {
                ((RichTextBox)c).BackColor = barvy[sMotiv]["pozadí"];
                ((RichTextBox)c).ForeColor = barvy[sMotiv]["text"];
            }
        }

        internal static Color GetColorForObject(string typ)
        {
            return barvy[GetMotiv()][typ];
        }

        internal static void SetControlColor(object c)
        {
            if (!(((c as Control)?.Tag as string)?.StartsWith("CustomColor") ?? false))
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
                    foreach (object tsdi in ((ToolStripDropDownItem)c).DropDownItems)
                    {
                        SetControlColor(tsdi);
                    }
                }
                else if (c.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)c).ForeColor = barvy[sMotiv]["text"];
                    ((ToolStripMenuItem)c).ForeColor = barvy[sMotiv]["text"];
                    foreach (object tsdi in ((ToolStripMenuItem)c).DropDownItems)
                    {
                        SetControlColor(tsdi);
                    }
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
                    foreach (object tsdi in ((ToolStrip)c).Items)
                    {
                        if (tsdi.GetType() != typeof(ToolStripButton))
                            SetControlColor(tsdi);
                    }
                }
                else if (c.GetType() == typeof(RichTextBox))
                {
                    ((RichTextBox)c).BackColor = barvy[sMotiv]["pozadíControl"];
                    ((RichTextBox)c).ForeColor = barvy[sMotiv]["text"];
                }
                else if (c.GetType() == typeof(CustomControls.TabControl))
                {
                    ((CustomControls.TabControl)c).HeaderBackColor = barvy[sMotiv]["tabPozadí"];
                    ((CustomControls.TabControl)c).BorderColor = barvy[sMotiv]["controlRámeček"];
                    ((CustomControls.TabControl)c).HeaderActiveBackColor = barvy[sMotiv]["tabPozadíAktivní"];
                    ((CustomControls.TabControl)c).ForeColor = barvy[sMotiv]["text"];
                }
                else if (c.GetType() == typeof(CustomControls.DatePicker))
                {
                    //((CustomControls.DatePicker)c).BackColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).ForeColor = barvy[sMotiv]["text"];
                    ((CustomControls.DatePicker)c).BorderColor = barvy[sMotiv]["buttonBorder"];
                    ((CustomControls.DatePicker)c).ButtonColor = barvy[sMotiv]["button"];
                    ((CustomControls.DatePicker)c).ArrowColor = barvy[sMotiv]["arrow"];
                    ((CustomControls.DatePicker)c).BorderColorMouseOver = Color.DodgerBlue;
                    ((CustomControls.DatePicker)c).ButtonColorMouseOver = barvy[sMotiv]["controlOver"];
                    ((CustomControls.DatePicker)c).BackColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).ButtonColorDisabled = barvy[sMotiv]["pozadíDisabled"];
                    ((CustomControls.DatePicker)c).BorderColorDisabled = barvy[sMotiv]["buttonBorder"];

                    ((CustomControls.DatePicker)c).MonthBorderColor = barvy[sMotiv]["buttonBorder"];
                    ((CustomControls.DatePicker)c).MonthBackColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthHeaderBackColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthHeaderForeColor = barvy[sMotiv]["text"];
                    ((CustomControls.DatePicker)c).MonthButtonColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthButtonBorderColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthButtonForeColor = barvy[sMotiv]["arrow"];
                    ((CustomControls.DatePicker)c).MonthButtonBorderMouseOverColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthButtonMouseOverForeColor = Color.FromArgb(154, 210, 255);
                    ((CustomControls.DatePicker)c).MonthButtonMouseOverColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthHeaderMouseOverBackColor = barvy[sMotiv]["pozadíControl"];
                    ((CustomControls.DatePicker)c).MonthHeaderMouseOverForeColor = Color.FromArgb(154, 210, 255);
                    ((CustomControls.DatePicker)c).MonthDayHeaderForeColor = barvy[sMotiv]["dayHeaderText"];
                }
                else
                {
                    string typ = c.GetType().ToString();
                    if (c as Control != null && !c.GetType().ToString().StartsWith("System.Windows.Forms.UpDownBase+"))
                    {
                        ((Control)c).BackColor = barvy[sMotiv]["pozadí"];
                        ((Control)c).ForeColor = barvy[sMotiv]["text"];
                    }
                }

                if (c as Control != null && !c.GetType().ToString().StartsWith("System.Windows.Forms.UpDownBase+"))
                {
                    foreach (Control cc in ((Control)c).Controls)
                    {
                        //barvy v nastavení
                        SetControlColor(cc);
                    }
                }
            }
            else if ((c as Control)?.Tag as string == "CustomColor:Window")
            {
                SetWindowControlColor(c);
            }
            else if ((c as Control)?.Tag as string == "CustomColor:Ignore")
            {
                //do nothing
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
            form.SuspendLayout();
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
            form.ResumeLayout();
        }
    }
}
