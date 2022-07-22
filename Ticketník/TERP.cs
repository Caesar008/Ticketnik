using System;
using System.Drawing;
using System.Windows.Forms;
using fNbt;

namespace Ticketník
{
    public partial class TERP : Form
    {
        Form1 form;
        public TERP(Form1 form, byte typ = 0, string terp = "", string task = "")
        {
            InitializeComponent();
            this.form = form;

            label5.Text = form.jazyk.Windows_Terp_Terp;
            label6.Text = form.jazyk.Windows_Terp_Task;
            button1.Text = form.jazyk.Windows_Terp_Nastav;
            label1.Text = form.jazyk.Windows_Terp_TerpKod;
            label2.Text = form.jazyk.Windows_Terp_Task;
            bterp.Text = form.jazyk.Windows_Terp_PridatTerp;
            btask.Text = form.jazyk.Windows_Terp_PridatTask;
            novyTaskB.Text = form.jazyk.Windows_Terp_UlozTask;
            label4.Text = label3.Text = form.jazyk.Windows_Terp_Novy;
            novyTerpB.Text = form.jazyk.Windows_Terp_UlozTerp;
            smazatTask.Text = form.jazyk.Windows_Terp_SmazatTask;
            smazatTerp.Text = form.jazyk.Windows_Terp_SmazatTerp;

            upravitPan.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            
            if (typ == 1)
            {
                upravitPan.Visible = true;
                panel1.Visible = false;
                foreach(NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value].StringValue;
                    staryTerp.Items.Add(ns.Value + add);
                }
                foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value].StringValue;
                    staryTask.Items.Add(ns.Value + add);
                }
                staryTask.Sorted = true;
                staryTerp.Sorted = true;
                staryTask.DropDownWidth = ComboWidth(staryTask);
                staryTerp.DropDownWidth = ComboWidth(staryTerp);
            }
            else if(typ == 2)
            {
                upravitPan.Visible = false;
                panel1.Visible = true;
                panel2.Visible = false;
                foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value].StringValue;
                    terpList.Items.Add(ns.Value + add);
                }
                foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value].StringValue;
                    taskList.Items.Add(ns.Value + add);
                }
                terpList.Sorted = true;
                taskList.Sorted = true;
                terpList.DropDownWidth = ComboWidth(terpList);
                taskList.DropDownWidth = ComboWidth(taskList);
            }
            else if (typ == 3)
            {
                upravitPan.Visible = false;
                panel1.Visible = false;
                panel2.Visible = true;
                terpListTic.Items.Add(""); 
                taskListTic.Items.Add("");
                foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[ns.Value].StringValue;
                    terpListTic.Items.Add(ns.Value + add);
                }
                foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
                {
                    string add = "";
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value] != null)
                        add = " " + Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[ns.Value].StringValue;
                    
                    taskListTic.Items.Add(ns.Value + add);
                }
                terpListTic.Sorted = true;
                taskListTic.Sorted = true;
                terpListTic.SelectedIndex = terpListTic.FindString(terp);
                taskListTic.SelectedIndex = taskListTic.FindString(task);
                terpListTic.DropDownWidth = ComboWidth(terpListTic);
                taskListTic.DropDownWidth = ComboWidth(taskListTic);
            }

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

        private void bterp_Click(object sender, EventArgs e)
        {
            bool nalezeno = false;
            if(Zakaznici.Terpy != null)
            {
                if(Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").Count != 0)
                {
                    foreach(NbtString terpy in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
                    {
                        if (terpy.Value == textBox1.Text.Split(new[] { ' ' }, 2)[0])
                        {
                            nalezeno = true;
                            MessageBox.Show(form.jazyk.Windows_Terp_TerpKod + " " + textBox1.Text + " " + form.jazyk.Windows_Terp_UzExistuje);
                            break;
                        }
                    }
                }

                if (!nalezeno)
                {
                    string[] s = new string[2]{null, null};
                    textBox1.Text.Split(new[] { ' ' }, 2).CopyTo(s,0);
                    form.list.AddTerp(s[0]);
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") == null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TerpPopis"));
                    if (s[1] == null)
                        s[1] = "";
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Add(new NbtString(s[0], s[1]));
                    MessageBox.Show(form.jazyk.Windows_Terp_TerpKod + " " + textBox1.Text + " " + form.jazyk.Windows_Terp_BylPridan);
                    this.Close();
                }
            }
            else
            {
                string[] s = new string[2] { null, null };
                textBox1.Text.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                form.list.AddTerp(s[0]);
                if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") == null)
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TerpPopis"));
                if (s[1] == null)
                    s[1] = "";
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Add(new NbtString(s[0], s[1]));
                MessageBox.Show(form.jazyk.Windows_Terp_TerpKod + " " + textBox1.Text + " " + form.jazyk.Windows_Terp_BylPridan);
                this.Close();
            }
        }

        private void btask_Click(object sender, EventArgs e)
        {
            bool nalezeno = false;
            if (Zakaznici.Terpy != null)
            {
                if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").Count != 0)
                {
                    foreach (NbtString terpy in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
                    {
                        if (terpy.Value == textBox2.Text.Split(new[] { ' ' }, 2)[0])
                        {
                            nalezeno = true;
                            MessageBox.Show(form.jazyk.Windows_Terp_Task + " " + textBox2.Text + " " + form.jazyk.Windows_Terp_UzExistuje);
                            break;
                        }
                    }
                }

                if (!nalezeno)
                {
                    string[] s = new string[2] { null, null };
                    textBox2.Text.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                    form.list.AddTask(s[0]);
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") == null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TaskPopis"));
                    if (s[1] == null)
                        s[1] = "";
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Add(new NbtString(s[0], s[1]));
                    MessageBox.Show(form.jazyk.Windows_Terp_Task + " " + textBox2.Text + " " + form.jazyk.Windows_Terp_BylPridan);
                    this.Close();
                }
            }
            else
            {
                string[] s = new string[2] { null, null };
                textBox2.Text.Split(new[] { ' ' }, 2).CopyTo(s, 0);
                form.list.AddTask(s[0]);
                if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") == null)
                    Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TaskPopis"));
                if (s[1] == null)
                    s[1] = "";
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Add(new NbtString(s[0], s[1]));
                MessageBox.Show(form.jazyk.Windows_Terp_Task + " " + textBox2.Text + " " + form.jazyk.Windows_Terp_BylPridan);
                this.Close();
            }
        }

        private void staryTerp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (novyTerp.Text.Replace(" ", "").Length != 0)
                novyTerpB.Enabled = true;
            else
                novyTerpB.Enabled = false;
        }

        private void novyTerp_TextChanged(object sender, EventArgs e)
        {
            if (staryTerp.SelectedItem != null && novyTerp.Text.Replace(" ", "").Length != 0)
                novyTerpB.Enabled = true;
            else
                novyTerpB.Enabled = false;
        }

        private void staryTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (novyTask.Text.Replace(" ", "").Length != 0)
                novyTaskB.Enabled = true;
            else
                novyTaskB.Enabled = false;
        }

        private void novyTask_TextChanged(object sender, EventArgs e)
        {
            if (staryTask.SelectedItem != null && novyTask.Text.Replace(" ", "").Length != 0)
                novyTaskB.Enabled = true;
            else
                novyTaskB.Enabled = false;
        }

        private void novyTerpB_Click(object sender, EventArgs e)
        {
            int index = 0;
            string[] s = new string[2] { null, null };
            novyTerp.Text.Split(new[] { ' ' }, 2).CopyTo(s, 0);
            foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
            {
                if (ns.Value == ((string)staryTerp.SelectedItem).Split(new[] { ' ' }, 2)[0])
                    break;
                index++;
            }
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") == null)
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TerpPopis"));
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Get<NbtString>(((string)staryTerp.SelectedItem).Split(new[] { ' ' }, 2)[0]) == null && s[1] != null)
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Add(new NbtString(s[0], s[1]));
            else if (s[1] != null)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Get<NbtString>(((string)staryTerp.SelectedItem).Split(new[] { ' ' }, 2)[0]).Value = s[1];
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Get<NbtString>(((string)staryTerp.SelectedItem).Split(new[] { ' ' }, 2)[0]).Name = s[0];
            }
            else
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Remove(((string)staryTerp.SelectedItem).Split(new[] { ' ' }, 2)[0]);
            ((NbtString)Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp")[index]).Value = s[0];
            form.list.Save();
            this.Close();
        }

        private void novyTaskB_Click(object sender, EventArgs e)
        {
            int index = 0;
            string[] s = new string[2] { null, null };
            novyTask.Text.Split(new[] { ' ' }, 2).CopyTo(s, 0);
            foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
            {
                if (ns.Value == ((string)staryTask.SelectedItem).Split(new[] { ' ' }, 2)[0])
                    break;
                index++;
            }
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") == null)
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Add(new NbtCompound("TaskPopis"));
            if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Get<NbtString>(((string)staryTask.SelectedItem).Split(new[] { ' ' }, 2)[0]) == null && s[1] != null)
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Add(new NbtString(s[0], s[1]));
            else if (s[1] != null)
            {
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Get<NbtString>(((string)staryTask.SelectedItem).Split(new[] { ' ' }, 2)[0]).Value = s[1];
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Get<NbtString>(((string)staryTask.SelectedItem).Split(new[] { ' ' }, 2)[0]).Name = s[0];
            }
            else
                Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Remove(((string)staryTask.SelectedItem).Split(new[] { ' ' }, 2)[0]);
            ((NbtString)Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task")[index]).Value = s[0];
            form.list.Save();
            this.Close();
        }

        private void terpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (terpList.SelectedItem != null)
            {
                smazatTerp.Enabled = true;
            }
            else
                smazatTerp.Enabled = false;
        }

        private void taskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (taskList.SelectedItem != null)
            {
                smazatTask.Enabled = true;
            }
            else
                smazatTask.Enabled = false;
        }

        private void smazatTerp_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp"))
            {
                if (ns.Value == ((string)terpList.SelectedItem).Split(new[] { ' ' }, 2)[0])
                {
                    if (Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis") != null && ((string)terpList.SelectedItem).Split(new[] { ' ' }, 2)[0] != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis")[((string)terpList.SelectedItem).Split(new[] { ' ' }, 2)[0]] != null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TerpPopis").Remove(((string)terpList.SelectedItem).Split(new[] { ' ' }, 2)[0]);
                    break;
                }
                index++;
            }
            Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Terp").RemoveAt(index);
            form.list.Save();
            this.Close();
        }

        private void smazatTask_Click(object sender, EventArgs e)
        {
            int index = 0;
            foreach (NbtString ns in Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task"))
            {
                if (ns.Value == ((string)taskList.SelectedItem).Split(new[] { ' ' }, 2)[0])
                { 
                    if(Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis") != null && ((string)taskList.SelectedItem).Split(new[] { ' ' }, 2)[0] != null && Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis")[((string)taskList.SelectedItem).Split(new[] { ' ' }, 2)[0]] != null)
                        Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtCompound>("TaskPopis").Remove(((string)taskList.SelectedItem).Split(new[] { ' ' }, 2)[0]);
                    break; 
                }
                index++;
            }
            Zakaznici.Terpy.Get<NbtCompound>("Custom").Get<NbtList>("Task").RemoveAt(index);
            form.list.Save();
            this.Close();
        }
    }
}
