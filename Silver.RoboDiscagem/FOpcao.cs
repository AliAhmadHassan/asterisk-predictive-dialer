using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Silver.RoboDiscagem
{
    public partial class FOpcao : Form
    {
        public FOpcao()
        {
            InitializeComponent();
        }

        private void btn_abrir_d_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();

            if (string.IsNullOrEmpty(op.FileName)) return;
            txt_discador.Text = op.FileName;
        }

        private void btn_abrir_e_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();

            if (string.IsNullOrEmpty(op.FileName)) return;
            txt_escuta.Text = op.FileName;
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_discador.Text) && !string.IsNullOrEmpty(txt_escuta.Text) && !string.IsNullOrEmpty(txt_discagem.Text))
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Discador|" + txt_discador.Text);
                sb.AppendLine("Escuta|" + txt_escuta.Text + "|" + (string.IsNullOrEmpty(txt_parametro.Text.Trim()) ? "" : txt_parametro.Text.Trim()));
                sb.AppendLine("Discagem|" + txt_discagem.Text);

                File.WriteAllText("DiscadorOpcao.sil", sb.ToString());
                MessageBox.Show("Opções gravadas com sucesso!", "Gravação de Opções", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void FOpcao_Load(object sender, EventArgs e)
        {
            if (File.Exists("DiscadorOpcao.sil"))
            {
                string[] opcoes = File.ReadAllLines("DiscadorOpcao.sil");
                txt_discador.Text = opcoes[0].Split('|')[1];
                txt_escuta.Text = opcoes[1].Split('|')[1];
                txt_discagem.Text = opcoes[2].Split('|')[1];
            }
        }

        private void lbl_excluir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists("DiscadorOpcao.sil"))
                File.Delete("DiscadorOpcao.sil");

            txt_discador.Text = string.Empty;
            txt_escuta.Text = string.Empty;
            txt_discagem.Text = string.Empty;
        }

        private void btn_abrir_discagem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.ShowDialog();

            if (string.IsNullOrEmpty(op.FileName)) return;
            txt_discagem.Text = op.FileName;
        }
    }
}
