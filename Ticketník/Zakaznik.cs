using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fNbt;

namespace Ticketník
{
    public partial class Zakaznik : Form
    {
        Form1 f;
        bool canChange = true;
        public Zakaznik(Form1 form)
        {
            InitializeComponent();
            f = form;
            this.Text = label1.Text = f.jazyk.Windows_Zakaznik;
            label2.Text = f.jazyk.Windows_Zakaznik_Velikost;
            label3.Text = f.jazyk.Windows_Zakaznik_Nebo;
            label4.Text = f.jazyk.Windows_Zakaznik_TerpKod;

            this.comboBox1.Items.AddRange(new object[] {
            f.jazyk.Windows_Mala,
            f.jazyk.Windows_Stredni,
            f.jazyk.Windows_Velka});

            foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
            {
                string add = "";
                if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value] != null)
                    add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value].StringValue;
                comboBox2.Items.Add(ns.Value + add);
            }
            comboBox2.Sorted = true;
            comboBox2.DropDownWidth = ComboWidth(comboBox2);
        }

        private int ComboWidth(ComboBox cb)
        {
            ComboBox senderComboBox = (ComboBox)cb;
            int width = senderComboBox.DropDownWidth;
            Graphics g = senderComboBox.CreateGraphics();
            Font font = senderComboBox.Font;
            int vertScrollBarWidth =
                (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                ? SystemInformation.VerticalScrollBarWidth : 0;

            int newWidth;
            foreach (string s in ((ComboBox)cb).Items)
            {
                newWidth = (int)g.MeasureString(s, font).Width
                    + vertScrollBarWidth;
                if (width < newWidth)
                {
                    width = newWidth;
                }
            }
            return width;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                f.zakaznik = textBox1.Text;
                /*switch ((string)comboBox1.SelectedItem)
                {
                    case f.jazyk.Windows_Mala:
                        f.velikost = 2;
                        break;
                    case f.jazyk.Windows_Stredni:
                        f.velikost = 1;
                        break;
                    case f.jazyk.Windows_Velka:
                        f.velikost = 0;
                        break;
                }*/

                if((string)comboBox1.SelectedItem == f.jazyk.Windows_Mala)
                    f.velikost = 2;
                else if ((string)comboBox1.SelectedItem == f.jazyk.Windows_Stredni)
                    f.velikost = 1;
                else if ((string)comboBox1.SelectedItem == f.jazyk.Windows_Velka)
                    f.velikost = 0;
                f.zakaznikTerp = "";
            }
            else
            {
                f.velikost = 127;
                f.zakaznikTerp = ((string)comboBox2.SelectedItem).Split(new[] { ' ' }, 2)[0];
                f.zakaznik = textBox1.Text;
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0 && (comboBox1.SelectedItem != null || comboBox2.SelectedItem != null))
                button2.Enabled = true;
            else
                button2.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (canChange)
            {
                canChange = false;
                if (textBox1.Text.Length != 0 && comboBox1.SelectedItem != null)
                    button2.Enabled = true;
                else
                    button2.Enabled = false;
                comboBox2.SelectedItem = null;
                canChange = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (canChange)
            {
                canChange = false;
                if (textBox1.Text.Length != 0 && comboBox2.SelectedItem != null)
                    button2.Enabled = true;
                else
                    button2.Enabled = false;
                comboBox1.SelectedItem = null;
                canChange = true;
            }
        }
    }
}
