using System;
using System.Web.UI.WebControls;
using System.IO;
using Silver.Common;
using System.Collections.Generic;
using System.Web.UI;
using System.Configuration;
using Silver.DTO;
using System.Text;
using System.Reflection;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Carga : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.NomeAplicacao = "Carga";
            Master.DescricaoAplicacao = "Configuração e manutenção de Carga";

            if (!IsPostBack)
            {
                CarregarCampanhas();
                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();
            }
        }

        private void CarregarCampanhas()
        {
            var campanhas = new BLL.Campanha().Obter(true);
            ddlCampanhas.SelectedIndexChanged -= ddlCampanhas_SelectedIndexChanged;

            ddlCampanhas.DataSource = campanhas;
            ddlCampanhas.DataTextField = "Nome";
            ddlCampanhas.DataValueField = "Id";
            ddlCampanhas.DataBind();
            ddlCampanhas.SelectedIndexChanged += ddlCampanhas_SelectedIndexChanged;
            ddlCampanhas.Items.Insert(0, string.Empty);
            ddlCampanhas.SelectedIndex = 0;

            lstCampanha.DataSource = campanhas;
            lstCampanha.DataValueField = "Id";
            lstCampanha.DataTextField = "Nome";
            lstCampanha.DataBind();
        }

        protected void GridViewObjeto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewObjeto_Preencher(e.NewPageIndex);
        }

        protected void GridViewObjeto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Excluir")
            {
                Excluir(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty)));
            }
        }

        protected void GridViewObjeto_Preencher(int intPageIndex)
        {
            var Carga = new BLL.Carga();
            var result = new List<DTO.Carga>();

            //Apenas Paginação
            if (intPageIndex > 0)
            {
                result = new Silver.BLL.Carga().SelectPeloHistorico(grid_historico.SelectedRow.Cells[1].Text.ToInt64());

                GridViewObjeto.PageIndex = intPageIndex;
                GridViewObjeto.DataSource = result;
                GridViewObjeto.DataBind();

                return;
            }


            switch (rdo_campo_busca.SelectedValue.ToLower())
            {
                case "idcampanha":
                    result = Carga.SelectPelaCampanha(tbBuscar.Text.ToInt64());
                    break;
                case "chave1":
                    result = Carga.SelecionaPelaChave1(tbBuscar.Text.Trim());
                    break;
                case "chave2":
                    result = Carga.SelectPelaChave2(tbBuscar.Text.Trim());
                    break;
            }

            GridViewObjeto.DataSource = result;
            GridViewObjeto.DataBind();
        }

        protected void btBuscar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBuscar.Text)) return;
            GridViewObjeto_Preencher(0);
        }

        protected void btNovo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            LimparCampos();
            PopularCampos();
            Panel_Cadastrar.Visible = true;
        }

        protected void LimparCampos()
        {
            lstCampanha.ClearSelection();
            fuArquivo.Attributes.Clear();
        }

        protected void PopularCampos()
        {
            lstCampanha.DataSource = new BLL.Campanha().Obter(true);
            lstCampanha.DataTextField = "Nome";
            lstCampanha.DataValueField = "Id";
            lstCampanha.DataBind();
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            if (!fuArquivo.HasFile)
            {
                Lb_Mensagem.Text = "Você precisa indicar um arquivo";
                Panel_Mensagem.Visible = true;
            }
            else
            {
                #region MyRegion
                //using (var sr = new StreamReader(fuArquivo.FileContent))
                //{
                //    var tipos_telefones = new SortedList<string, string>();

                //    var servico_carga = new BLL.Carga();
                //    var servico_carga_telefone = new BLL.CargaTelefone();
                //    var servico_carga_telefone_tipo = new BLL.CargaTelefoneTipo();

                //    var linha = sr.ReadLine();
                //    while (!string.IsNullOrEmpty(linha))
                //    {
                //        var registro = new DTO.Carga();
                //        linha = linha.Replace("\"", string.Empty);
                //        if (linha.Substring(linha.Length - 1, 1) == ";")
                //        {
                //            linha = linha.Substring(0, linha.Length - 1);
                //        }
                //        var campos = linha.Split(';');

                //        registro.Chave1 = campos[0];
                //        registro.Chave2 = campos[1];
                //        registro.DtCarga = DateTime.Now;
                //        registro.IdCampanha = lstCampanha.SelectedValue.ToInt64();
                //        registro.Ativo = true;

                //        registro.Id = servico_carga.Cadastrar(registro);

                //        for (var i = 2; i < campos.Length; i += 4)
                //        {
                //            try
                //            {
                //                var ddd = campos[i + 1].Length <= 2 ? campos[i + 1] : campos[i + 1].Substring(0, 3);

                //                var t = new DTO.CargaTelefone() { TelId = campos[i], Ddd = ddd, Telefone = campos[i + 2], IdCarga = registro.Id };

                //                var _tipo_telefone = campos[i + 3].Trim().ToUpper();

                //                if (!tipos_telefones.ContainsKey(_tipo_telefone))
                //                {
                //                    var tipo_telefone = servico_carga_telefone_tipo.Buscar(_tipo_telefone);
                //                    if (tipo_telefone == null)
                //                    {
                //                        servico_carga_telefone_tipo.Cadastrar(new DTO.CargaTelefoneTipo() { Ativo = true, Descricao = _tipo_telefone });
                //                        tipo_telefone = servico_carga_telefone_tipo.Buscar(_tipo_telefone);
                //                        tipos_telefones.Add(_tipo_telefone, tipo_telefone.Id.ToString());
                //                    }
                //                    else
                //                    {
                //                        tipos_telefones.Add(_tipo_telefone, tipo_telefone.Id.ToString());
                //                    }
                //                }

                //                t.IdTipo = tipos_telefones[_tipo_telefone].ToInt64();
                //                servico_carga_telefone.Cadastrar(t);
                //            }
                //            catch
                //            {
                //            }
                //        }

                //        linha = sr.ReadLine();
                //    }
                //}
                #endregion

                try
                {
                    DTO.HistoricoCarga historico = new DTO.HistoricoCarga();
                    historico.Ativo = true;
                    historico.Descricao = txt_decricao.Text.Trim();
                    historico.IdCampanha = lstCampanha.SelectedValue.ToInt32();
                    historico.IdOperador = (Session["Usuario"] as DTO.Usuario).Id.ToInt32();
                    historico.NomeArquivo = Path.GetFileName(fuArquivo.PostedFile.FileName);
                    historico.Status = 1;
                    historico.Tamanho = fuArquivo.PostedFile.ContentLength / 1024;

                    long id_historico = new BLL.HistoricoCarga().Cadastrar(historico);

                    var nome_arquivo = Path.Combine(ConfigurationManager.AppSettings["Application.Paths.Carga"], string.Format("{0}_{1}.dat", lstCampanha.SelectedValue, id_historico));
                    fuArquivo.SaveAs(nome_arquivo);

                    Silver.BLL.ControleSistema.IncluirEvento(DTO.EventoControleSistema.Processar_Carga, new DTO.ControleSistema { Valor = nome_arquivo, Solicitante = (Session["Usuario"] as DTO.Usuario).Id, Situacao = (int)DTO.SitucaoEventoControleSistema.Aguardando, Campanha = lstCampanha.SelectedValue.ToInt64() });

                    LimparCampos();
                    Panel_Cadastrar.Visible = false;
                    Lb_Mensagem.Text = "Carga enviada para o servidor, acompanhe o processamento pelo Dashboard";
                    Panel_Mensagem.Visible = true;
                }
                catch (Exception ex)
                {
                    Lb_Mensagem.Text = "Falha ao processar aquivo, a mensagem do sistema foi: " + ex.Message;
                }
            }
        }

        protected void btCadastrar_Nao_Click(object sender, EventArgs e)
        {
            LimparCampos();
            Panel_Cadastrar.Visible = false;
        }

        protected long BuscarTipo(string Tipo)
        {
            var BLLCargaTelefoneTipo = new BLL.CargaTelefoneTipo();
            DTO.CargaTelefoneTipo DTOCargaTelefoneTipo = null;
            DTOCargaTelefoneTipo = BLLCargaTelefoneTipo.Buscar(Tipo);
            if (DTOCargaTelefoneTipo == null)
            {
                DTOCargaTelefoneTipo = new DTO.CargaTelefoneTipo();
                DTOCargaTelefoneTipo.Descricao = Tipo;
                DTOCargaTelefoneTipo.Ativo = true;
                BLLCargaTelefoneTipo.Cadastrar(DTOCargaTelefoneTipo);
            }
            return DTOCargaTelefoneTipo.Id;
        }

        protected void Excluir(int intExcluir)
        {
            IdExcluir.Value = intExcluir.ToString();
            Panel_Excluir.Visible = true;
        }

        protected void btExcluir_Sim_Click(object sender, EventArgs e)
        {
            var Carga = new BLL.Carga();
            Carga.Ativar(int.Parse(IdExcluir.Value), false);
            var CargaTelefone = new BLL.CargaTelefone();
            CargaTelefone.Excluir(int.Parse(IdExcluir.Value));
            Panel_Excluir.Visible = false;
            Lb_Mensagem.Text = "Item inativado com sucesso";
            Panel_Mensagem.Visible = true;
        }

        protected void btExcluir_Nao_Click(object sender, EventArgs e)
        {
            Panel_Excluir.Visible = false;
        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
            Panel_Mensagem.Visible = false;
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            Session.RemoveAll();
            Response.Redirect("../Login.aspx");
        }

        protected void ddlCampanhas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampanhas.SelectedIndex == 0) return;
            if (ddlCampanhas.SelectedItem == null) return;

            lbl_total_carga.Text = "00000";
            lbl_total_historico.Text = "00000";
            lbl_total_telefone.Text = "00000";

            PreencherGridHistorico(ddlCampanhas.SelectedItem.Value.ToInt32());

            GridViewObjeto.DataSource = null;
            GridViewObjeto.DataBind();
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if (e.Row.Cells[8].Text.Trim().Equals("False"))
                e.Row.Cells[8].Text = "Não";
            else
                e.Row.Cells[8].Text = "Sim";

            e.Row.Cells[7].Text = Enum.GetName(typeof(SilverStatus), e.Row.Cells[7].Text.Trim().ToInt32());
        }

        protected void GridViewObjeto_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            PreencherGridTelefones(GridViewObjeto.Rows[e.NewSelectedIndex].Cells[2].Text.ToInt64());
        }

        private void PreencherGridTelefones(long codigoCarga)
        {
            var result = new BLL.CargaTelefone().ObterPelaCarga(codigoCarga);
            lbl_total_telefone.Text = result.Count.ToString("000");
            GridViewTelefone.DataSource = result;
            GridViewTelefone.DataBind();
        }

        protected void GridViewTelefone_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if (e.Row.Cells[5].Text.Trim().Equals(0))
                e.Row.Cells[5].Text = "Não";
            else
                e.Row.Cells[5].Text = "Sim";

            var tipoTelefone = new BLL.CargaTelefoneTipo().Buscar(e.Row.Cells[4].Text.ToInt64());
            if (tipoTelefone != null)
                e.Row.Cells[4].Text = tipoTelefone.Descricao;

            e.Row.Cells[3].Text = Enum.GetName(typeof(SilverStatus), e.Row.Cells[3].Text.Trim().ToInt32());
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void grid_historico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PreencherGridHistorico(ddlCampanhas.SelectedValue.ToInt64(), e.NewPageIndex);
        }

        private void PreencherGridHistorico(long id_campanha, int page_index = 0)
        {
            var result = new BLL.HistoricoCarga().ListarCampanha(id_campanha);
            grid_historico.DataSource = result;
            grid_historico.PageIndex = page_index;
            grid_historico.DataBind();
            lbl_total_historico.Text = result.Count.ToString("00000");
        }

        protected void grid_historico_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            var result = new Silver.BLL.Carga().SelectPeloHistorico(grid_historico.Rows[e.NewSelectedIndex].Cells[1].Text.ToInt64());
            GridViewObjeto.DataSource = result;
            GridViewObjeto.DataBind();

            GridViewTelefone.DataSource = null;
            GridViewTelefone.DataBind();

            lbl_total_carga.Text = result.Count.ToString("00000");
        }

        protected void grid_historico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            SilverStatus status = (SilverStatus)e.Row.Cells[8].Text.Trim().ToInt32();
            e.Row.Cells[8].Text = Enum.GetName(typeof(SilverStatus), status).ToUpper().Replace('_', ' ');
        }

        protected void grid_historico_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grid_historico_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.csv", "Carga"));
            Response.ContentType = "application/text";

            //Arquivo de saida
            StringBuilder arquivo_csv = new StringBuilder();

            var cargas = new Silver.BLL.Carga().SelectPeloHistorico(grid_historico.Rows[e.RowIndex].Cells[1].Text.ToInt64());
            //Cache de tipos de telefone
            SortedList<long, string> cache_tipos_telefone = new SortedList<long, string>();
            foreach (var c in cargas)
            {
                arquivo_csv.Append(string.Format("\"{0}\";", c.Chave1));
                arquivo_csv.Append(string.Format("\"{0}\";", c.Chave2));

                var telefones = new Silver.BLL.CargaTelefone().ObterPelaCarga(c.Id);
                foreach (var t in telefones)
                {
                    string tp_telefone = string.Empty;
                    if (cache_tipos_telefone.ContainsKey(t.IdTipo))
                        tp_telefone = cache_tipos_telefone[t.IdTipo];
                    else
                    {
                        var tipo_telefone_base = new Silver.BLL.CargaTelefoneTipo().Buscar(t.IdTipo);
                        tp_telefone = tipo_telefone_base.Descricao;
                    }

                    arquivo_csv.Append(string.Format("\"{0}\";\"{2}\";\"{3}\";\"{4}\";", t.TelId, t.Ddd, t.Telefone, tp_telefone, Enum.GetName(typeof(SilverStatus), t.Status)));
                }
                arquivo_csv.Append("\n");
            }

            Response.Write(arquivo_csv.ToString());
            Response.End();
            e.Cancel = true;
        }
    }
}
