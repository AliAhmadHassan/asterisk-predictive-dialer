using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Silver.Common;
using System.IO;
using System.Configuration;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class ConsultaGravacao : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarCampanhas();
                Master.NomeAplicacao = "Consulta Gravação";
                Master.DescricaoAplicacao = "Página de consulta de gravações";

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
            }
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewObjeto_Preencher(0);
        }

        protected void GridViewObjeto_Preencher(int intPageIndex)
        {
            if (string.IsNullOrEmpty(txt_cpfcnpj.Text.Trim()) && string.IsNullOrEmpty(txt_ramaloperador.Text.Trim()))
            {
                lbl_mensagem.Text = "Informe um Cpf/CNPJ ou um Ramal/Operador";
                return;
            }

            DateTime var_teste = new DateTime();
            if (!DateTime.TryParse(txt_inicio.Text.Trim(), out var_teste))
            {
                lbl_mensagem.Text = "Data de início está inválida!";
                txt_inicio.Focus();
                return;
            }

            if (!DateTime.TryParse(txt_fim.Text.Trim(), out var_teste))
            {
                lbl_mensagem.Text = "Data final está inválida!";
                txt_fim.Focus();
                return;
            }

            List<DTO.BilhetagemGravacaoGrid> fonte_dados = new List<DTO.BilhetagemGravacaoGrid>();
            if (string.IsNullOrEmpty(txt_cpfcnpj.Text))
                fonte_dados = new BLL.BilhetagemGravacao().ListarPeloRamal(txt_ramaloperador.Text.Trim(), Convert.ToDateTime(txt_inicio.Text), Convert.ToDateTime(txt_fim.Text));
            else
                fonte_dados = new BLL.BilhetagemGravacao().ListarPeloCpfCnpj(txt_cpfcnpj.Text.Trim(), Convert.ToDateTime(txt_inicio.Text), Convert.ToDateTime(txt_fim.Text));

            GridViewObjeto.PageIndex = intPageIndex;
            GridViewObjeto.DataSource = fonte_dados;
            GridViewObjeto.DataBind();
            lbl_totalregistros.Text = fonte_dados.Count.ToString("0000");
        }

        protected void GridViewObjeto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridViewObjeto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewObjeto_Preencher(e.NewPageIndex);
        }

        protected void CarregarCampanhas()
        {
            ddl_campanha.DataSource = new Silver.BLL.Campanha().Obter(true);
            ddl_campanha.DataTextField = "Nome";
            ddl_campanha.DataValueField = "Id";
            ddl_campanha.DataBind();
        }

        private string ObterPathArquivo(Silver.DTO.Bilhetagem bilhetagem)
        {
            string path_diretorio = ConfigurationManager.AppSettings["Application.Path.Gravacao"];
            string nome_arquivo = Path.GetFileNameWithoutExtension(bilhetagem.pathgravacao);
            string path_download = string.Empty;
            string[] path_split = bilhetagem.pathgravacao.Split('/');

            path_download += path_diretorio;
            for (int i = 3; i < path_split.Length - 1; i++)
                path_download = Path.Combine(path_download, path_split[i]);

            string data = nome_arquivo.Split('.')[1].Split('-')[0];
            string hora = nome_arquivo.Split('.')[1].Split('-')[1];

            string data_format = string.Empty;
            char[] data_formatada = data.ToCharArray();

            int contador = 1;
            for (int i = 0; i < data_formatada.Length; i++)
            {
                if (contador % 2 == 0)
                    data_format += data_formatada[i] + "/";
                else
                    data_format += data_formatada[i];

                ++contador;
            }
            data_format = data_format.Remove(data_format.LastIndexOf('/'), 1);
            string hora_format = string.Empty;
            char[] hora_formatada = hora.ToCharArray();

            contador = 1;
            for (int i = 0; i < hora_formatada.Length; i++)
            {
                if (contador % 2 == 0)
                    hora_format += hora_formatada[i] + ":";
                else
                    hora_format += hora_formatada[i];

                ++contador;
            }

            hora_format = hora_format.Remove(hora_format.LastIndexOf(':'), 1);
            DateTime data_hora = Convert.ToDateTime(data_format + " " + hora_format);
            path_download = Path.Combine(path_download, nome_arquivo + ".WAV");
            
            if (!File.Exists(path_download))
            {
                string dir_base = Path.GetDirectoryName(path_download);
                string[] arquivos_no_dirbase = Directory.GetFiles(dir_base);
                string nome_arquivo_proximo = nome_arquivo.Substring(0, nome_arquivo.Length - 2);
                string arquivo_proximo = arquivos_no_dirbase.Where(a => a.Contains(nome_arquivo_proximo)).FirstOrDefault();

                if (arquivo_proximo != null)
                {
                    path_download = Path.Combine(Path.GetDirectoryName(path_download), Path.GetFileNameWithoutExtension(arquivo_proximo) + ".WAV");
                    return path_download;
                }
                else
                    return null;
            }
            else
                return path_download;
        }

        protected void GridViewObjeto_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var bilhetagem_id = GridViewObjeto.Rows[e.NewSelectedIndex].Cells[0].Text.ToInt64();
            var bilhetagem_completa = new Silver.BLL.Bilhetagem().Obter(bilhetagem_id);
            string arquivo_download = ObterPathArquivo(bilhetagem_completa);

            try
            {
                if (string.IsNullOrEmpty(arquivo_download))
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("<script>alert('{0}');</script>", "Arquivo não encontrado! Verifique se o arquivo existe manualmente."));
                    return;
                }

                using (StreamReader sr = new StreamReader(arquivo_download))
                {
                    using (BinaryReader br = new BinaryReader(sr.BaseStream))
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", Path.GetFileName(arquivo_download)));
                        Response.AddHeader("Content-Length", sr.BaseStream.Length.ToString());
                        Response.BinaryWrite(br.ReadBytes((int)sr.BaseStream.Length));
                        Response.End();
                        Response.Flush();
                        Response.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), string.Format("<script>alert('{0}');</script>", ex.Message));
            }
        }

        protected void ddl_campanha_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_campanha_selecionada.Text = ddl_campanha.SelectedItem.Text.ToUpper();
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow) return;
                DTO.BilhetagemGravacaoGrid registro = (DTO.BilhetagemGravacaoGrid)e.Row.DataItem;
                e.Row.Cells[4].Text = registro.dstchannel.Split('-')[0].Split('/')[1];
                e.Row.Cells[6].Text = TimeSpan.FromSeconds(registro.duration).ToString();
                e.Row.Cells[7].Text = TimeSpan.FromSeconds(registro.billsec).ToString();
                e.Row.Cells[8].Text = new Silver.BLL.Campanha().Obter(registro.idcampanha).Descricao;
                e.Row.Cells[9].Text = Path.GetFileNameWithoutExtension(registro.pathgravacao);
            }
            catch { }
        }

        protected void GridViewObjeto_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}