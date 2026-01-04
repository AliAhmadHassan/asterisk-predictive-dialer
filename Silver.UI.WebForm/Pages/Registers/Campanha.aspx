<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Campanha.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Registers.Campanha" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 126px;
        }
        .style3
        {
            width: 100%;
        }
        .style4
        {
            width: 106px;
        }
        .style5
        {
            width: 145px;
        }
        .style6
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="box-panel">
        <table style="width: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="middle">
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="imb_logout" ClientIDMode="Static" runat="server" ImageUrl="~/Imagens/logout_2.png"
                                    AlternateText="Logout" title="Sair do sistema" OnClick="imb_logout_Click" OnClientClick="Logout(this)" />
                            </td>
                            <td>
                                <asp:Label ID="lblSair" runat="server" Text="Sair"></asp:Label>
                            </td>
                            <td>
                                <img src="../../Imagens/User2.png" alt="Usuário" id="img_usuario" title="Usuário logado" />
                            </td>
                            <td>
                                <asp:Label ID="lbNome" runat="server"></asp:Label>
                            </td>
                            <td>
                                <img src="../../Imagens/talk.png" alt="Ramal" title="Ramal conectado" />
                            </td>
                            <td>
                                <asp:Label ID="lbRamal" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                Buscar:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbBuscar" CssClass="TextBox" Width="100%" title="Informe um critério para busca." />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbBuscar"
                                    Display="Dynamic" ErrorMessage="Informe o critério de seleção" ForeColor="Red"
                                    ValidationGroup="form_buscar"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:ImageButton ID="btBuscar" runat="server" CssClass="ButtonBuscar" ImageUrl="~/Imagens/search-icon.png"
                                    title="Iniciar a busca" OnClick="btBuscar_Click" ValidationGroup="form_buscar" />
                            </td>
                            <td>
                                <asp:ImageButton ID="btNovo" runat="server" ImageUrl="~/Imagens/New.png" OnClick="btNovo_Click"
                                    OnClientClick="LimparFormulario(this.form)" title="Iniciar um novo cadastro" />
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btNovo"
                                    CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
                                    PopupControlID="Panel_Cadastrar">
                                </asp:ModalPopupExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="btScheduler" runat="server" ImageUrl="~/Imagens/signature-icon.png"
                                    OnClientClick="LimparFormulario(this.form)" title="Plano de execução da campanha"
                                    CssClass="style6" Width="30px" OnClick="btScheduler_Click" />
                                <asp:ImageButton ID="bt_scheduler" runat="server" ImageUrl="~/Imagens/Tarifas.png"
                                    OnClick="btNovo_Click" OnClientClick="LimparFormulario(this.form)" title="Iniciar um novo cadastro"
                                    Width="1px" Visible="True" />
                                <asp:ModalPopupExtender ID="bt_scheduler_ModalPopupExtender" runat="server" TargetControlID="bt_scheduler"
                                    CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
                                    PopupControlID="Panel_Cadastrar">
                                </asp:ModalPopupExtender>
                                <asp:ModalPopupExtender ID="bt_scheduler_ModalPopupExtender2" runat="server" TargetControlID="bt_scheduler"
                                    CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
                                    PopupControlID="Panel_Cadastrar">
                                </asp:ModalPopupExtender>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <fieldset>
        <legend>Campanhas Cadastradas</legend>
        <asp:GridView ID="GridViewObjeto" runat="server" OnRowCommand="GridViewObjeto_RowCommand"
            OnPageIndexChanging="GridViewObjeto_PageIndexChanging" AutoGenerateColumns="False"
            OnRowCreated="GridViewObjeto_RowCreated" 
            OnRowDataBound="GridViewObjeto_RowDataBound">
            <Columns>
                <asp:ButtonField ButtonType="Image" CommandName="Alterar" Text="Atualizar" HeaderText="#"
                    ImageUrl="~/Imagens/Ico/grid_atualizar.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" CommandName="Excluir" Text="Excluir" HeaderText="#"
                    ImageUrl="~/Imagens/Ico/grid_deletar.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
                <asp:BoundField DataField="Id" HeaderText="Id" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Nome" HeaderText="Nome">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="MusicaEspera" HeaderText="Música Espera" Visible="False">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Estrategia" HeaderText="Estratégia">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Agressividade" HeaderText="Agressividade" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="QtdToques" HeaderText="Qtd. Toques" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="TempoEspera" HeaderText="Tempo Espera" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Anuncio" HeaderText="Anuncio" Visible="False">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ativo" HeaderText="Ativo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdGrupo" HeaderText="Grupo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:ButtonField ButtonType="Image" CommandName="scheduler" Text="Tarefas" HeaderText="#"
                    ImageUrl="~/Imagens/Tarifas.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <div>
        <asp:ModalPopupExtender ID="Panel_Cadastrar_ModalPopupExtender" runat="server" TargetControlID="btNovo"
            CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
            PopupControlID="Panel_Cadastrar">
        </asp:ModalPopupExtender>
        <asp:Panel runat="server" ID="Panel_Cadastrar" CssClass="modalPopup" Visible="true">
            <div class="header_popup">
            </div>
            <table style="width: 100%">
                <tr>
                    <td>
                        Nome:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbNome" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvNome" runat="server" ErrorMessage="* Favor preencher o nome"
                            ForeColor="#CC3300" Font-Italic="True" ControlToValidate="tbNome" Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Descrição:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbDescricao" MaxLength="150"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvDescricao" runat="server" ErrorMessage="* Favor preencher a descrição"
                            ForeColor="#CC3300" Font-Italic="True" ControlToValidate="tbDescricao" Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>
                        Música de espera:
                    </td>
                    <td>
                        <asp:FileUpload ID="fuMusicaEspera" runat="server" />
                        <asp:Label ID="lbMusicaEspera" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Estratégia:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlEstrategia">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rvEstrategia" runat="server" ErrorMessage="* Favor selecionar uma estratégia"
                            Display="Dynamic" ForeColor="#CC3300" Font-Italic="True" ControlToValidate="ddlEstrategia"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Quantidade de toques:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbQtdToques"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvQtdToques" runat="server" ErrorMessage="* Favor informar a quantidade de toques"
                            ForeColor="#CC3300" Font-Italic="True" ControlToValidate="tbQtdToques" Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator16" runat="server"
                            ControlToValidate="tbQtdToques" Display="Dynamic" ErrorMessage="* Somente números são permitidos"
                            Font-Italic="True" ForeColor="#CC3300" SetFocusOnError="True" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tempo de espera:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbTempoEspera"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvTempoEspera" runat="server" ErrorMessage="* Favor informar o tempo de espera"
                            ForeColor="#CC3300" Font-Italic="True" ControlToValidate="tbTempoEspera" Display="Dynamic"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator15" runat="server"
                            ControlToValidate="tbTempoEspera" Display="Dynamic" ErrorMessage="* Somente números são permitidos"
                            Font-Italic="True" ForeColor="#CC3300" SetFocusOnError="True" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Agressividade
                    </td>
                    <td>
                        <asp:TextBox ID="tbAgressividade" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvTempoEspera0" runat="server" ControlToValidate="tbAgressividade"
                            ErrorMessage="* Favor informar a agressividade" Font-Italic="True" ForeColor="#CC3300"
                            Display="Dynamic" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator17" runat="server"
                            ControlToValidate="tbAgressividade" Display="Dynamic" ErrorMessage="* Somente números são permitidos"
                            Font-Italic="True" ForeColor="#CC3300" SetFocusOnError="True" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="display: none">
                    <td>
                        Anuncio:
                    </td>
                    <td>
                        <asp:FileUpload ID="fuAnuncio" runat="server" /><asp:Label ID="lbAnuncio" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Pausa:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:CheckBoxList ID="lstPausa" runat="server" CellPadding="0" CellSpacing="0" Height="80px"
                                RepeatColumns="5" RepeatDirection="Horizontal" Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Grupo:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:RadioButtonList ID="lstGrupo" runat="server" CellPadding="0" CellSpacing="0"
                                Height="80px" RepeatColumns="5" RepeatDirection="Horizontal" Width="100%">
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ativo:
                    </td>
                    <td style="height: 5px;">
                        <asp:CheckBox ID="cbAtivo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td style="height: 5px;">
                        <asp:Button ID="btCadastrar_Sim" runat="server" CssClass="ButtonSalvar" OnClick="btCadastrar_Sim_Click"
                            Text="" Width="79px" />
                        <asp:Button ID="btCadastrar_Nao" runat="server" CssClass="ButtonCancelar" OnClick="btCadastrar_Nao_Click"
                            Text="" Width="79px" />
                        <asp:HiddenField ID="IdCadastrar" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="height: 5px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:DragPanelExtender ID="Panel_Cadastrar_DragPanelExtender" runat="server" DragHandleID="Panel_Cadastrar"
            Enabled="True" TargetControlID="Panel_Cadastrar">
        </asp:DragPanelExtender>
    </div>
    <div>
        <asp:Panel runat="server" ID="Panel_Excluir" Visible="false" CssClass="Panel_Form">
            <table style="width: 100%">
                <tr>
                    <td style="background-color: #eee9e9; text-align: center">
                        <strong><span style="font-size: 10pt; color: #009900">ATENÇÃO!!!</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px;">
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; height: 5px; text-align: center">
                        Deseja excluir este item?
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px;">
                        <asp:HiddenField ID="IdExcluir" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; height: 5px; text-align: center">
                        <asp:Button ID="btExcluir_Sim" runat="server" OnClick="btExcluir_Sim_Click" Text="Sim"
                            Width="79px" ValidationGroup="Excluir" CssClass="btExcluir_Sim" SkinID="btnLogin" />
                        <asp:Button ID="btExcluir_Nao" runat="server" OnClick="btExcluir_Nao_Click" Text="Não"
                            Width="79px" ValidationGroup="Excluir" CssClass="btExcluir_Sim" SkinID="btnLogin" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:DragPanelExtender ID="Panel_Excluir_DragPanelExtender" runat="server" DragHandleID="Panel_Excluir"
            Enabled="True" TargetControlID="Panel_Excluir">
        </asp:DragPanelExtender>
        <asp:DropShadowExtender ID="Panel_Excluir_DropShadowExtender" runat="server" Enabled="True"
            TargetControlID="Panel_Excluir">
        </asp:DropShadowExtender>
    </div>
    <div>
        <asp:Panel runat="server" ID="Panel_Mensagem" Visible="false" CssClass="Panel_Form">
            <table style="width: 100%">
                <tr>
                    <td style="background-color: #eee9e9; text-align: center">
                        <strong><span style="font-size: 10pt; color: #009900">ATENÇÃO!!!</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px;">
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; height: 5px; text-align: center">
                        <asp:Label ID="Lb_Mensagem" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px;">
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; height: 5px; text-align: center">
                        <asp:Button ID="btMensagem" runat="server" OnClick="btMensagem_Click" Text="OK" Width="79px"
                            CssClass="btExcluir_Sim" SkinID="btnLogin" ValidationGroup="Mensagem" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:DropShadowExtender ID="Panel_Mensagem_DropShadowExtender" runat="server" Enabled="True"
            TargetControlID="Panel_Mensagem">
        </asp:DropShadowExtender>
        <asp:DragPanelExtender ID="Panel_Mensagem_DragPanelExtender" runat="server" DragHandleID="Panel_Mensagem"
            Enabled="True" TargetControlID="Panel_Mensagem">
        </asp:DragPanelExtender>
    </div>
    <div id="div_scheduler">
        <asp:ModalPopupExtender ID="ModalPopupExtenderScheduler" runat="server" TargetControlID="bt_scheduler"
            CancelControlID="btNaoCadastrar_Scheduler" BackgroundCssClass="modalBackground"
            Enabled="True" PopupControlID="PanelScheduler">
        </asp:ModalPopupExtender>
        <asp:Panel runat="server" ID="PanelScheduler" CssClass="modalPopup">
            <asp:UpdatePanel ID="pnl_cbo_campanhas" runat="server">
                <ContentTemplate>
                    <div class="header_popup">
                        Plano de Execução da campanha
                    </div>
                    <table style="width: 100%; border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                        border-bottom-width: 1px;">
                        <tr>
                            <td class="style2" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                border-bottom-width: 1px">
                                Campanha:
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="cboCampanhaScheduler" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboCampanhaScheduler_SelectedIndexChanged"
                                    ValidationGroup="form_scheduler" Enabled="False">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style2">
                                Ativo:
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_tarefa" runat="server" Text="Tarefa Ativa" Width="300px" /><asp:UpdateProgress
                                    ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <img src="~/Imagens/ajax-loader.gif" runat="server" id="img_loader" alt="Carregando" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                Plano de Execução:
                            </td>
                            <td>
                                <table class="style3">
                                    <tr>
                                        <td class="style4" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            &nbsp;
                                        </td>
                                        <td class="style5" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Início
                                        </td>
                                        <td style="background-color: #66CCFF; font-weight: bold; color: #333333; font-size: 12px;">
                                            Fim
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Segunda-Feira
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_seg_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_seg_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_seg_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_seg_fim"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Terça-Feira
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_ter_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_ter_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_ter_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_ter_fim"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Quarta-Feira
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_qua_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_qua_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_qua_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_qua_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Quinta-Feira
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_qui_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt_qui_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_qui_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txt_qui_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Sexta-Feira
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_sex_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt_sex_inicio"
                                                Display="Dynamic" ErrorMessage="Hora Inválida." Font-Size="11px" ForeColor="Red"
                                                SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_sex_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                                ControlToValidate="txt_sex_inicio" Display="Dynamic" ErrorMessage="Hora Inválida."
                                                Font-Size="11px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Sábado
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_sab_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                                ControlToValidate="txt_sab_inicio" Display="Dynamic" ErrorMessage="Hora Inválida."
                                                Font-Size="11px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_sab_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                                ControlToValidate="txt_sab_inicio" Display="Dynamic" ErrorMessage="Hora Inválida."
                                                Font-Size="11px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4" style="background-color: #66CCFF; font-weight: bold; color: #333333;
                                            font-size: 12px;">
                                            Domingo
                                        </td>
                                        <td class="style5" style="border-bottom-style: dotted; border-bottom-color: #EFEFEF;
                                            border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_dom_inicio" runat="server" CssClass="txtTime" MaxLength="5"
                                                ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server"
                                                ControlToValidate="txt_dom_inicio" Display="Dynamic" ErrorMessage="Hora Inválida."
                                                Font-Size="11px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                        <td style="border-bottom-style: dotted; border-bottom-color: #EFEFEF; border-bottom-width: 1px">
                                            <asp:TextBox ID="txt_dom_fim" runat="server" CssClass="txtTime" MaxLength="5" ValidationGroup="form_scheduler"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server"
                                                ControlToValidate="txt_dom_inicio" Display="Dynamic" ErrorMessage="Hora Inválida."
                                                Font-Size="11px" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$"
                                                ValidationGroup="form_scheduler"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbl_mensagem_scheduler" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btCadastrar_Scheduler" runat="server" CssClass="ButtonSalvar" OnClick="btCadastrar_Scheduler_Click"
                                    Text="" Width="79px" ValidationGroup="form_scheduler" />
                                <asp:Button ID="btNaoCadastrar_Scheduler" runat="server" CssClass="ButtonCancelar"
                                    Text="" Width="79px" ValidationGroup="form_scheduler" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>
