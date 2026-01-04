using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Silver.RoboDiscagem
{
    public partial class FTelefonar : Form
    {
        public FTelefonar()
        {
            InitializeComponent();

        }

        private void btn_discar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_ddd.Text.Trim()) || string.IsNullOrEmpty(txt_telefone.Text.Trim()) || ddl_campanha.SelectedItem == null)
            {
                lbl_msg.Text = "Todos os campos são obrigatórios!";
                return;
            }

            AsteriskClient.AsteriskCommand cmd = new AsteriskClient.AsteriskCommand();
            var campanha = (Silver.DTO.Campanha)ddl_campanha.SelectedItem;
            cmd.Discar(new AsteriskClient.DTO.Discagem() { Campanha = campanha.Nome.Replace(' ', '_'), IdCampanha = campanha.Id.ToString(), IdTelefone = "0000", Protocolo = "3", Telefone = txt_ddd.Text + txt_telefone.Text.Trim(), TipoTelefone = "Celular" });

        }

        private void FTelefonar_Load(object sender, EventArgs e)
        {
            var campanhas = new Silver.BLL.Campanha().Obter(true);
            ddl_campanha.DataSource = campanhas;
            ddl_campanha.DisplayMember = "Nome";
            ddl_campanha.ValueMember = "Id";
        }
    }
}