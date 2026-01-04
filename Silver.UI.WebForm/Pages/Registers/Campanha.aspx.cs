using System;
using System.IO;
using Silver.DTO;
using Silver.Common;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Silver.UI.Web.Presentation.Pages.Registers
{
    public partial class Campanha : PaginaBase
    {
        public List<Estrategia> Estrategias = new List<Estrategia>();

        private void CarregarEstrategias()
        {
            Estrategias.Add(new Estrategia() { Id = 0, Descricao = "Tocar todos os agentes simultaneamente" });             //ringall
            Estrategias.Add(new Estrategia() { Id = 1, Descricao = "Tocar os agentes randomicamente" });                    //random
            Estrategias.Add(new Estrategia() { Id = 2, Descricao = "Tocar os agentes na ordem dos ramais" });               //linear
            Estrategias.Add(new Estrategia() { Id = 3, Descricao = "Tocar o agente com maior tempo sem ligações" });        //leastrecent
            Estrategias.Add(new Estrategia() { Id = 4, Descricao = "Tocar o agente com menos ligações" });                  //fewestcalls
            Estrategias.Add(new Estrategia() { Id = 5, Descricao = "Tocar os agentes um a um, sequencialmente" });          //rrmemory

            ddlEstrategia.DataSource = Estrategias;
            ddlEstrategia.DataTextField = "Descricao";
            ddlEstrategia.DataValueField = "Id";
            ddlEstrategia.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ModalPopupExtenderScheduler.Hide();
            CarregarEstrategias();
            if (!IsPostBack)
            {
                tbNome.Attributes.Add("onkeypress", "if (event.keyCode == 32) {event.keyCode = 0;}");
                tbQtdToques.Attributes.Add("onkeypress", "if (event.keyCode < 48 || event.keyCode > 58) {event.keyCode = 0;}");
                tbTempoEspera.Attributes.Add("onkeypress", "if (event.keyCode < 48 || event.keyCode > 58) {event.keyCode = 0;}");

                Master.NomeAplicacao = "Campanha";
                Master.DescricaoAplicacao = "Configuração e manutenção das Campanhas";

                var usuarioSession = (DTO.Usuario)Session["Usuario"];
                lbNome.Text = usuarioSession.Nome.ToString();
                lbRamal.Text = usuarioSession.Ramal.ToString();

                GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
                CarregarPausas();
                PreencherGrupos();
            }
        }

        protected void GridViewObjeto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewObjeto_Preencher(e.NewPageIndex);
        }

        protected void GridViewObjeto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Alterar")
                Cadastrar(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty)));

            if (e.CommandName == "Excluir")
                Excluir(int.Parse(GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty)));

            switch (e.CommandName.ToLower())
            {
                case "scheduler":
                    PanelScheduler.Visible = true;
                    var campanha = GridViewObjeto.Rows[int.Parse(e.CommandArgument.ToString())].Cells[2].Text.Replace("&nbsp;", string.Empty).ToInt64();
                    cboCampanhaScheduler.SelectedValue = campanha.ToString();
                    PreencherSchedulerCampanha(new BLL.CampanhaTarefa().Obter(campanha));
                    ModalPopupExtenderScheduler.Show();
                    break;
            }
        }

        protected void GridViewObjeto_Preencher(int intPageIndex, string filtro = "")
        {
            GridViewObjeto.PageIndex = intPageIndex;

            var Campanha = new BLL.Campanha();
            var campanhas = new List<DTO.Campanha>();

            if (string.IsNullOrEmpty(filtro))
                campanhas = Campanha.Listar();
            else
                campanhas = Campanha.Buscar(filtro);

            GridViewObjeto.DataSource = campanhas;
            GridViewObjeto.DataBind();

            cboCampanhaScheduler.DataSource = Campanha.Listar();
            cboCampanhaScheduler.DataTextField = "Nome";
            cboCampanhaScheduler.DataValueField = "Id";
            cboCampanhaScheduler.DataBind();

            cboCampanhaScheduler.Items.Insert(0, "");
        }

        protected void CarregaDataGrid(string filtro)
        {

        }

        protected void btBuscar_Click(object sender, EventArgs e)
        {
            var Campanha = new BLL.Campanha();
            GridViewObjeto.DataSource = Campanha.Buscar(tbBuscar.Text);
            GridViewObjeto.DataBind();
        }

        protected void btNovo_Click(object sender, EventArgs e)
        {
            LimparCampos();
            CarregarPausas();
        }

        protected void Cadastrar(int intID)
        {
            LimparCampos();
            var Campanha = new BLL.Campanha().Obter(intID);

            IdCadastrar.Value = Campanha.Id.ToString();
            tbNome.Text = Campanha.Nome.ToString();
            tbDescricao.Text = Campanha.Descricao.ToString();
            tbAgressividade.Text = Campanha.Agressividade.ToString();
            tbQtdToques.Text = Campanha.QtdToques.ToString();
            tbTempoEspera.Text = Campanha.TempoEspera.ToString();

            ddlEstrategia.SelectedValue = Campanha.Estrategia.ToString();

            var pausa_campanha = new BLL.CampanhaPausa().SelectPelaCampanha(Campanha.Id);
            foreach (ListItem item in lstPausa.Items)
                if (pausa_campanha.Where(c => c.IdPausa.Equals(item.Value.ToInt64())).FirstOrDefault() != null)
                    item.Selected = true;

            if (Campanha.IdGrupo > 0)
                lstGrupo.SelectedValue = Campanha.IdGrupo.ToString();

            //if (Campanha.MusicaEspera.Length > 0)
            //    lbMusicaEspera.Text = Path.GetFileName(Campanha.MusicaEspera);

            //if (Campanha.Anuncio.Length > 0)
            //    lbAnuncio.Text = Path.GetFileName(Campanha.Anuncio);

            if (Campanha.Ativo.Equals(1))
                cbAtivo.Checked = true;

            Panel_Cadastrar_ModalPopupExtender.Show();
        }

        protected void LimparCampos()
        {
            IdCadastrar.Value = string.Empty;
            tbNome.Text = string.Empty;
            tbDescricao.Text = string.Empty;

            //fuMusicaEspera.Attributes.Clear();
            //if (lbMusicaEspera.Text != string.Empty)
            //    if (File.Exists(lbMusicaEspera.Text))
            //        File.Delete(lbMusicaEspera.Text);

            ddlEstrategia.ClearSelection();
            tbQtdToques.Text = string.Empty;
            tbTempoEspera.Text = string.Empty;

            //fuAnuncio.Attributes.Clear();

            //if (lbAnuncio.Text != string.Empty)
            //    if (File.Exists(lbAnuncio.Text))
            //        File.Delete(lbAnuncio.Text);

            lstPausa.ClearSelection();
            cbAtivo.Checked = false;
        }

        protected void CarregarPausas()
        {
            lstPausa.DataSource = new BLL.Pausa().Obter(true);
            lstPausa.DataTextField = "Descricao";
            lstPausa.DataValueField = "Id";
            lstPausa.DataBind();
        }

        protected void btCadastrar_Sim_Click(object sender, EventArgs e)
        {
            var Continua = true;
            bool novo_cadastro = false;
            if (Continua)
            {
                var DTOCadastrar = new DTO.Campanha();
                var BLLCadastrar = new BLL.Campanha();
                Lb_Mensagem.Text = string.Empty;

                if (IdCadastrar.Value != string.Empty)
                    DTOCadastrar = BLLCadastrar.Obter(int.Parse(IdCadastrar.Value));

                if (DTOCadastrar == null)
                    novo_cadastro = true;

                DTOCadastrar.Nome = tbNome.Text;
                DTOCadastrar.Descricao = tbDescricao.Text;
                DTOCadastrar.Estrategia = Convert.ToInt32(ddlEstrategia.SelectedValue);
                DTOCadastrar.QtdToques = Convert.ToInt32(tbQtdToques.Text);
                DTOCadastrar.TempoEspera = Convert.ToInt32(tbTempoEspera.Text);
                DTOCadastrar.Agressividade = tbAgressividade.Text.ToInt32();

                if (lstGrupo.SelectedValue != null)
                    DTOCadastrar.IdGrupo = lstGrupo.SelectedValue.ToInt32();

                DTOCadastrar.Ativo = cbAtivo.Checked.ToInt32();
                BLLCadastrar.Cadastrar(DTOCadastrar, (Session["Usuario"] as DTO.Usuario).Id);

                var lst = new List<long>();
                foreach (ListItem itemPausa in lstPausa.Items)
                    if (itemPausa.Selected)
                        lst.Add(long.Parse(itemPausa.Value));

                new BLL.CampanhaPausa().Cadastrar(DTOCadastrar.Id, lst, TipoID.Campanha);

                LimparCampos();
                Lb_Mensagem.Text = "Item cadastrado com sucesso";

                Silver.BLL.ControleSistema.IncluirEvento(EventoControleSistema.Recarregar_Queue,
                    new ControleSistema()
                    {
                        Campanha = DTOCadastrar.Id,
                        Valor = "0",
                        Evento = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), SitucaoEventoControleSistema.Aguardando),
                        Solicitante = (Session["Usuario"] as DTO.Usuario).Id,
                        Porcentagem = 0,
                        Situacao = (long)SitucaoEventoControleSistema.Aguardando
                    });

                string mensagem_sistema = "Nova Campanha Cadastrada. Nome:<b>{0}</b>";
                if (!novo_cadastro)
                    mensagem_sistema = "Campanha Atualizada. Nome:<b>{0}</b>";

                Silver.BLL.MensagemSistema.Cadastrar(
                    new MensagemSistema()
                    {
                        DataHora = DateTime.Now,
                        IdCampanha = DTOCadastrar.Id,
                        Mensagem = string.Format(mensagem_sistema, DTOCadastrar.Descricao),
                        Visualizada = false,
                        TipoMensagem = (int)DTO.TipoMensagemSistema.Campanha_Cadastrada
                    });
            }

            Panel_Mensagem.Visible = true;
        }

        protected void btCadastrar_Nao_Click(object sender, EventArgs e)
        {
            LimparCampos();

        }

        protected void Excluir(int intExcluir)
        {
            IdExcluir.Value = intExcluir.ToString();
            Panel_Excluir.Visible = true;
        }

        protected void btExcluir_Sim_Click(object sender, EventArgs e)
        {
            var Campanha = new BLL.Campanha();
            Campanha.Ativar(IdExcluir.Value.ToInt32(), false);
            Lb_Mensagem.Text = "Item inativado com sucesso";
            Panel_Mensagem.Visible = true;
            Panel_Excluir.Visible = false;
            ModalPopupExtenderScheduler.Hide();
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);

            Silver.BLL.ControleSistema.IncluirEvento(EventoControleSistema.Recarregar_Queue,
                    new ControleSistema()
                    {
                        Campanha = IdExcluir.Value.ToInt32(),
                        Valor = "0",
                        Evento = Enum.GetName(typeof(DTO.SitucaoEventoControleSistema), SitucaoEventoControleSistema.Aguardando),
                        Solicitante = (Session["Usuario"] as DTO.Usuario).Id,
                        Porcentagem = 0,
                        Situacao = (long)SitucaoEventoControleSistema.Aguardando
                    });

            Silver.BLL.MensagemSistema.Cadastrar(
                new MensagemSistema()
                {
                    DataHora = DateTime.Now,
                    IdCampanha = IdExcluir.Value.ToInt32(),
                    Mensagem = string.Format("Campanha Bloqueada. ID:<b>{0}</b>", IdExcluir.Value.ToInt32()),
                    Visualizada = false,
                    TipoMensagem = (int)DTO.TipoMensagemSistema.Campanha_Bloqueada
                });
        }

        protected void btExcluir_Nao_Click(object sender, EventArgs e)
        {
            Panel_Excluir.Visible = false;
        }

        protected void btMensagem_Click(object sender, EventArgs e)
        {
            GridViewObjeto_Preencher(GridViewObjeto.PageIndex);
            ModalPopupExtenderScheduler.Hide();
            Panel_Mensagem.Visible = false;
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            var usuarioSession = (DTO.Usuario)Session["Usuario"];
            new BLL.UsuarioLogin().RegistrarSaida(usuarioSession.Id);
            Session.RemoveAll();
            Response.Redirect("../Login.aspx");
        }

        protected void GridViewObjeto_RowCreated(object sender, GridViewRowEventArgs e)
        {
        }

        protected void GridViewObjeto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if (e.Row.Cells[11].Text.Trim().Equals("0"))
                e.Row.Cells[11].Text = "Não";
            else
                e.Row.Cells[11].Text = "Sim";

            if (!string.IsNullOrEmpty(e.Row.Cells[12].Text.Trim()))
                e.Row.Cells[12].Text = new BLL.Grupo().Obter(e.Row.Cells[12].Text.ToInt32()).Nome;

            e.Row.Cells[6].Text = Estrategias.Single(c => c.Id.Equals(e.Row.Cells[6].Text.ToInt32())).Descricao;
        }

        protected void btNovo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
        }

        protected void imb_logout_Click(object sender, ImageClickEventArgs e)
        {
            this.Logout();
        }

        protected void lnk_logout_Click(object sender, EventArgs e)
        {
            this.Logout();
        }

        protected void btScheduler_Click(object sender, ImageClickEventArgs e)
        {
            PanelScheduler.Visible = true;
        }

        protected void cboCampanhaScheduler_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_mensagem_scheduler.Text = string.Empty;
            if (cboCampanhaScheduler.SelectedIndex == 0) return;
            PreencherSchedulerCampanha(new BLL.CampanhaTarefa().Obter(cboCampanhaScheduler.SelectedValue.ToInt64()));
        }

        private void PreencherSchedulerCampanha(DTO.CampanhaTarefa tarefa_campanha)
        {
            txt_dom_fim.Text = tarefa_campanha.DomingoFim != null ? tarefa_campanha.DomingoFim.Value.ToString("HH:mm") : string.Empty;
            txt_dom_inicio.Text = tarefa_campanha.DomingoInicio != null ? tarefa_campanha.DomingoInicio.Value.ToString("HH:mm") : string.Empty;
            txt_seg_inicio.Text = tarefa_campanha.SegundaInicio != null ? tarefa_campanha.SegundaInicio.Value.ToString("HH:mm") : string.Empty;
            txt_seg_fim.Text = tarefa_campanha.SegundaFim != null ? tarefa_campanha.SegundaFim.Value.ToString("HH:mm") : string.Empty;
            txt_ter_inicio.Text = tarefa_campanha.TercaInicio != null ? tarefa_campanha.TercaInicio.Value.ToString("HH:mm") : string.Empty;
            txt_ter_fim.Text = tarefa_campanha.TercaFim != null ? tarefa_campanha.TercaFim.Value.ToString("HH:mm") : string.Empty;
            txt_qua_inicio.Text = tarefa_campanha.QuartaInicio != null ? tarefa_campanha.QuartaInicio.Value.ToString("HH:mm") : string.Empty;
            txt_qua_fim.Text = tarefa_campanha.QuartaFim != null ? tarefa_campanha.QuartaFim.Value.ToString("HH:mm") : string.Empty;
            txt_qui_inicio.Text = tarefa_campanha.QuartaFim != null ? tarefa_campanha.QuintaInicio.Value.ToString("HH:mm") : string.Empty;
            txt_qui_fim.Text = tarefa_campanha.QuintaFim != null ? tarefa_campanha.QuintaFim.Value.ToString("HH:mm") : string.Empty;
            txt_sex_inicio.Text = tarefa_campanha.SextaInicio != null ? tarefa_campanha.SextaInicio.Value.ToString("HH:mm") : string.Empty;
            txt_sex_fim.Text = tarefa_campanha.SextaFim != null ? tarefa_campanha.SextaFim.Value.ToString("HH:mm") : string.Empty;
            txt_sab_inicio.Text = tarefa_campanha.SabadoInicio != null ? tarefa_campanha.SabadoInicio.Value.ToString("HH:mm") : string.Empty;
            txt_sab_fim.Text = tarefa_campanha.SabadoFim != null ? tarefa_campanha.SabadoFim.Value.ToString("HH:mm") : string.Empty;
            txt_dom_inicio.Text = tarefa_campanha.DomingoInicio != null ? tarefa_campanha.DomingoInicio.Value.ToString("HH:mm") : string.Empty;
            txt_dom_fim.Text = tarefa_campanha.DomingoFim != null ? tarefa_campanha.DomingoFim.Value.ToString("HH:mm") : string.Empty;

            chk_tarefa.Checked = tarefa_campanha.Ativo;
        }

        protected void btCadastrar_Scheduler_Click(object sender, EventArgs e)
        {
            lbl_mensagem_scheduler.Text = string.Empty;
            try
            {
                var tarefa_campanha = new CampanhaTarefa()
                {
                    IdCampanha = cboCampanhaScheduler.SelectedValue.ToInt64(),
                    Ativo = chk_tarefa.Checked,

                    DomingoFim = string.IsNullOrEmpty(txt_dom_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_dom_fim.Text).ToString()),
                    DomingoInicio = string.IsNullOrEmpty(txt_dom_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_dom_inicio.Text).ToString()),

                    SegundaFim = string.IsNullOrEmpty(txt_seg_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_seg_fim.Text).ToString()),
                    SegundaInicio = string.IsNullOrEmpty(txt_seg_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_seg_inicio.Text).ToString()),

                    TercaFim = string.IsNullOrEmpty(txt_ter_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_ter_fim.Text).ToString()),
                    TercaInicio = string.IsNullOrEmpty(txt_ter_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_ter_inicio.Text).ToString()),

                    QuartaFim = string.IsNullOrEmpty(txt_qua_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_qua_fim.Text).ToString()),
                    QuartaInicio = string.IsNullOrEmpty(txt_qua_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_qua_inicio.Text).ToString()),

                    QuintaFim = string.IsNullOrEmpty(txt_qui_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_qui_fim.Text).ToString()),
                    QuintaInicio = string.IsNullOrEmpty(txt_qui_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_qui_inicio.Text).ToString()),

                    SextaFim = string.IsNullOrEmpty(txt_sex_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_sex_fim.Text).ToString()),
                    SextaInicio = string.IsNullOrEmpty(txt_sex_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_sex_inicio.Text).ToString()),

                    SabadoFim = string.IsNullOrEmpty(txt_sab_fim.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_sab_fim.Text).ToString()),
                    SabadoInicio = string.IsNullOrEmpty(txt_sab_inicio.Text) ? null : (DateTime?)Convert.ToDateTime(TimeSpan.Parse(txt_sab_inicio.Text).ToString()),
                };

                new BLL.CampanhaTarefa().Incluir(tarefa_campanha);
                lbl_mensagem_scheduler.Text = "Tarefa atualizada com sucesso!";
            }
            catch (Exception ex)
            {
                lbl_mensagem_scheduler.Text = ex.Message;
            }
        }

        private void PreencherGrupos()
        {
            lstGrupo.DataSource = new BLL.Grupo().Listar();
            lstGrupo.DataTextField = "Nome";
            lstGrupo.DataValueField = "Id";
            lstGrupo.DataBind();
        }

        protected void btBuscar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewObjeto_Preencher(0, tbBuscar.Text.Trim());
        }
    }

    public class Estrategia
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}