<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Menu.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Registers.Menu" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <legend>Menus Cadastrados:</legend>
        <asp:GridView ID="GridViewObjeto" runat="server" OnRowCommand="GridViewObjeto_RowCommand"
            OnPageIndexChanging="GridViewObjeto_PageIndexChanging" AutoGenerateColumns="False"
            OnRowDataBound="GridViewObjeto_RowDataBound">
            <Columns>
                <asp:ButtonField ButtonType="Image" CommandName="Alterar" Text="Atualizar" HeaderText=""
                    ImageUrl="~/Imagens/Ico/grid_atualizar.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" CommandName="Excluir" Text="Excluir" HeaderText=""
                    ImageUrl="~/Imagens/Ico/grid_deletar.png">
                    <ControlStyle Width="16px" />
                    <ItemStyle Width="16px" />
                </asp:ButtonField>
                <asp:BoundField DataField="Id" HeaderText="Id" />
                <asp:BoundField DataField="IdMenu" HeaderText="Menu Pai" />
                <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                <asp:BoundField DataField="Url" HeaderText="Url" />
                <asp:BoundField DataField="GrupoMenu" HeaderText="Grupo Menu" />
                <asp:BoundField DataField="Ativo" HeaderText="Ativo" />
                <asp:TemplateField HeaderText="Icone">
                    <ItemTemplate>
                        <asp:Image Width="25px" Height="25px" runat="server" ID="img_icone" align="left" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
    <div>
        <asp:ModalPopupExtender ID="Panel_Cadastrar_ModalPopupExtender" runat="server" TargetControlID="btNovo"
            CancelControlID="btCadastrar_Nao" BackgroundCssClass="modalBackground" Enabled="True"
            PopupControlID="Panel_Cadastrar">
        </asp:ModalPopupExtender>
        <asp:Panel runat="server" ID="Panel_Cadastrar" Visible="true" CssClass="modalPopup">
            <div class="header_popup">
                &nbsp;
            </div>
            <table>
                <tr>
                    <td>
                        Menu acima:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:RadioButtonList ID="lstMenuAcima" runat="server" RepeatColumns="4" Width="100%">
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rvDescricao0" runat="server" ControlToValidate="lstMenuAcima"
                                Display="Dynamic" ErrorMessage="* Favor selecionar o menu superior a este" Font-Bold="False"
                                Font-Italic="True" ForeColor="#CC3300"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Descrição:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbDescricao" MaxLength="150" CssClass="TextBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvDescricao" runat="server" ControlToValidate="tbDescricao"
                            ErrorMessage="* Favor preencher a Descrição" ForeColor="#CC3300" Font-Bold="False"
                            Font-Italic="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        URL:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbUrl" MaxLength="250" CssClass="TextBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvUrl" runat="server" ControlToValidate="tbUrl" ErrorMessage="* Favor preencher a URL"
                            ForeColor="#CC3300" Font-Bold="False" Font-Italic="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Icone:
                    </td>
                    <td>
                        <asp:FileUpload ID="fuIcone" runat="server" />&nbsp;<asp:Image ID="imgIcone" runat="server"
                            Visible="false" /><asp:Label ID="lbIcone" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Grupo Menu:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_grupomenu" runat="server">
                            <asp:ListItem Value="0">Selecione um grupo</asp:ListItem>
                            <asp:ListItem Value="1">Cadastros</asp:ListItem>
                            <asp:ListItem Value="2">Relatórios</asp:ListItem>
                            <asp:ListItem Value="3">Telefonia</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Grupo Acesso:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:CheckBoxList ID="lstGrupo" runat="server" CellPadding="0" CellSpacing="0" Height="80px"
                                RepeatColumns="5" RepeatDirection="Horizontal" Width="100%">
                            </asp:CheckBoxList>
                            <asp:RequiredFieldValidator ID="rvDescricao1" runat="server" 
                                ControlToValidate="lstMenuAcima" Display="Dynamic" 
                                ErrorMessage="* Favor selecionar o grupo " Font-Bold="False" Font-Italic="True" 
                                ForeColor="#CC3300"></asp:RequiredFieldValidator>
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
                    </td>
                    <td style="height: 5px;">
                        <asp:HiddenField ID="IdCadastrar" runat="server" />
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
