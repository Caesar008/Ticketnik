using System;
using System.Windows.Forms;

namespace Ticketník
{
    public partial class SkrytaNastaveni : Form
    {
        Form1 form;
        public SkrytaNastaveni(Form1 form)
        {
            InitializeComponent();
            this.form = form;
            cesta_k_souboru.Text = Properties.Settings.Default.filePath;
            update_cesta.Text = Properties.Settings.Default.updateCesta;
            zalozni_update.Text = Properties.Settings.Default.ZalozniUpdate;
            novyExport.Checked = Properties.Settings.Default.NovyExport;
            zalozniUpdateBox.Checked = Properties.Settings.Default.pouzivatZalozniUpdate = true;
            Motiv.SetMotiv(this);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.filePath = cesta_k_souboru.Text;
            Properties.Settings.Default.updateCesta = update_cesta.Text;
            Properties.Settings.Default.ZalozniUpdate = zalozni_update.Text;
            Properties.Settings.Default.NovyExport = novyExport.Checked;
            Properties.Settings.Default.pouzivatZalozniUpdate = zalozniUpdateBox.Checked;
            Properties.Settings.Default.Save();
            form.Logni("Nové skryté nastavení\r\n\r\nSoubor: " + cesta_k_souboru.Text + "\r\nUpdate (UNC - nepoužito): " +
                update_cesta.Text + "\r\nUpdate (URL): " + zalozni_update.Text + "\r\nPoužít URL první: " + 
                zalozniUpdateBox.Checked + "\r\nNový export: " + novyExport.Checked , Form1.LogMessage.INFO);
        }
    }
}
