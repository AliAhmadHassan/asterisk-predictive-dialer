<%@ Page Title="Carga" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Carga.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Registers.Carga" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="box-panel">
        <table style="width: 100%; border: none;" cellpadding="0" cellspacing="0">
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
                            <td style="border-left-style: outset; border-left-width: 1px; border-left-color: #EFEFEF">
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
                        </tr>
                        <tr>
                            <td colspan="8">
                                Campanha:
                                <asp:DropDownList ID="ddlCampanhas" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCampanhas_SelectedIndexChanged"
                                    Width="300px">
                                </asp:DropDownList>
                            </td>
                            <td style="border-left-style: outset; border-left-width: 1px; border-left-color: #EFEFEF">
                                <asp:RadioButtonList ID="rdo_campo_busca" runat="server" Height="34px" RepeatColumns="3"
                                    Width="328px">
                                    <asp:ListItem Selected="True" Value="IdCampanha">Cód.Campanha</asp:ListItem>
                                    <asp:ListItem>Chave1</asp:ListItem>
                                    <asp:ListItem>Chave2</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <fieldset style="margin-top: 15px">
        <legend>&nbsp;<img src="../../Imagens/carga_historico.gif" alt="" />
            Histórico da Carga [<asp:Label ID="lbl_total_historico" runat="server" Font-Bold="True"></asp:Label>]</legend>
        <asp:GridView ID="grid_historico" runat="server" OnPageIndexChanging="grid_historico_PageIndexChanging"
            AutoGenerateColumns="False" EmptyDataText="Selecione uma Campanha" Width="100%"
            OnSelectedIndexChanging="grid_historico_SelectedIndexChanging" OnRowDataBound="grid_historico_RowDataBound"
            OnRowCommand="grid_historico_RowCommand" OnRowDeleting="grid_historico_RowDeleting">
            <Columns>
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagens/search-icon.png" SelectText=""
                    ShowSelectButton="True">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:CommandField>
                <asp:BoundField DataField="Id" HeaderText="Id" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdOperador" HeaderText="Operador" Visible="False">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Descricao" HeaderText="Descrição">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="DataHoraInicio" HeaderText="Dt. Hora">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="DataHoraFim" HeaderText="Dt. Hora Proc.">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="NomeArquivo" HeaderText="Nome Arquivo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Tamanho" HeaderText="Tamanho (Kb)">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalCliente" HeaderText="Clientes" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="TotalTelefone" HeaderText="Telefones" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagens/database_export.gif"
                    DeleteText="Exportar" HeaderText="Exportar" ShowDeleteButton="True">
                    <ControlStyle Height="16px" Width="16px" />
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:CommandField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset style="margin-top: 15px">
        <legend>&nbsp;
            <img src="../../Imagens/carga_carga.gif" alt="" />
            Carga [<asp:Label ID="lbl_total_carga" runat="server" Font-Bold="True"></asp:Label>
            ]</legend>
        <asp:GridView ID="GridViewObjeto" runat="server" OnRowCommand="GridViewObjeto_RowCommand"
            OnPageIndexChanging="GridViewObjeto_PageIndexChanging" AutoGenerateColumns="False"
            EmptyDataText="Selecione uma Campanha" OnRowDataBound="GridViewObjeto_RowDataBound"
            Width="100%" OnSelectedIndexChanging="GridViewObjeto_SelectedIndexChanging">
            <Columns>
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagens/search-icon.png" SelectText=""
                    ShowSelectButton="True">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:CommandField>
                <asp:ButtonField ButtonType="Image" CommandName="Excluir" Text="Excluir" HeaderText=""
                    ImageUrl="~/Imagens/Ico/grid_deletar.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
                <asp:BoundField DataField="Id" HeaderText="Id" DataFormatString="{0:000}">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdCampanha" HeaderText="IdCampanha" Visible="False">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Chave1" HeaderText="Chave1">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Chave2" HeaderText="Chave2">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="DtCarga" HeaderText="DtCarga">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ativo" HeaderText="Ativo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <fieldset style="margin-top: 15px">
        <legend>&nbsp;
            <img src="../../Imagens/carga_telefone.gif" alt="" />
            Telefones [<asp:Label ID="lbl_total_telefone" runat="server" Font-Bold="True"></asp:Label>
            ]</legend>
        <asp:GridView ID="GridViewTelefone" runat="server" AutoGenerateColumns="false" Caption="Telefones da Carga"
            OnRowDataBound="GridViewTelefone_RowDataBound" Width="100%">
            <Columns>
                <asp:BoundField DataField="TelId" HeaderText="Tel. Id">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ddd" HeaderText="DDD">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Telefone" HeaderText="Número">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdTipo" HeaderText="Tipo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ativo" HeaderText="Ativo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <div style="text-align: left">
        <asp:ModalPopupExtender ID="Panel_Cadastrar_ModalPopupExtender" runat="server" TargetControlID="btNovo"
            CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
            PopupControlID="Panel_Cadastrar">
        </asp:ModalPopupExtender>
        <asp:Panel runat="server" ID="Panel_Cadastrar" Visible="true" CssClass="modalPopup">
            <div class="header_popup">
                &nbsp;
            </div>
            <table style="width: 100%; font-size: 12px;">
                <tr>
                    <td>
                        Campanha:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:RadioButtonList ID="lstCampanha" runat="server" RepeatColumns="4" Width="100%">
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rvQtdToques0" runat="server" 
                                ControlToValidate="lstCampanha" Display="Dynamic" 
                                ErrorMessage="* Favor selecionar uma campanha" Font-Italic="True" 
                                ForeColor="#CC3300"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Arquivo:
                    </td>
                    <td>
                        <asp:FileUpload ID="fuArquivo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Descrição:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_decricao" runat="server" MaxLength="145"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvQtdToques" runat="server" ControlToValidate="txt_decricao"
                            ErrorMessage="* Favor informar uma descrição para esta carga" Font-Italic="True"
                            ForeColor="#CC3300" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btCadastrar_Sim" runat="server" CssClass="ButtonSalvar" OnClick="btCadastrar_Sim_Click"
                            Text="" Width="79px" />
                        <asp:Button ID="btCadastrar_Nao" runat="server" CssClass="ButtonCancelar" OnClick="btCadastrar_Nao_Click"
                            Text="" ValidationGroup="Cancelar" Width="79px" />
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
</asp:Content>
