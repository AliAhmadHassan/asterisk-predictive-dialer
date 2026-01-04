<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Usuario.aspx.cs" Inherits="Silver.UI.Web.Presentation.Usuario" %>

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
                                <asp:ModalPopupExtender ID="Panel_Cadastrar_ModalPopupExtender" runat="server" TargetControlID="btNovo"
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
        <legend>Usuários Cadstrados</legend>
        <asp:GridView ID="GridViewObjeto" runat="server" OnRowCommand="GridViewObjeto_RowCommand"
            OnPageIndexChanging="GridViewObjeto_PageIndexChanging" AutoGenerateColumns="False"
            EmptyDataText="Registros não encontrados" ShowFooter="false" OnRowDataBound="GridViewObjeto_RowDataBound">
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
                <asp:BoundField DataField="Ramal" HeaderText="Ramal">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdGrupo" HeaderText="Grupo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IdCampanha" HeaderText="Campanha">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Operador" HeaderText="Operador">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Ativo" HeaderText="Ativo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="header_grid" />
        </asp:GridView>
    </fieldset>
    <div>
        <asp:Panel runat="server" ID="Panel_Cadastrar" Visible="true" CssClass="modalPopup">
            <div class="header_popup">
                &nbsp;
            </div>
            <table style="font-size: 12px" width="100%">
                <tr>
                    <td>
                        Ramal:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbRamal" MaxLength="10" ValidationGroup="form"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvRamal" runat="server" ControlToValidate="tbRamal"
                            Display="Dynamic" ErrorMessage="* Favor preencher o Ramal" ForeColor="#CC3300"
                            Font-Bold="False" Font-Italic="True">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nome:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbNome" MaxLength="255" ValidationGroup="form"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvNome" runat="server" ControlToValidate="tbNome"
                            ErrorMessage="* Favor preencher o Nome" ForeColor="#CC3300" Font-Bold="False"
                            Font-Italic="True" Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Senha:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbSenha" MaxLength="10" ValidationGroup="form" TextMode="Password"
                            title="Informe uma senha com no mínimo 6 caracteres"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvSenha" runat="server" ControlToValidate="tbSenha"
                            ErrorMessage="* Favor preencher a Senha" ForeColor="#CC3300" Font-Bold="False"
                            Font-Italic="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Confirmar Senha:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbConfirma" MaxLength="10" ValidationGroup="form"
                            TextMode="Password" title="Confirme a senha informada no campo anterior"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvConfirma" runat="server" ControlToValidate="tbConfirma"
                            ErrorMessage="* Favor preencher a Senha" ForeColor="#CC3300" Font-Bold="False"
                            Font-Italic="True" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvConfirma" runat="server" ControlToValidate="tbConfirma"
                            ControlToCompare="tbSenha" ErrorMessage="* As senhas estão diferentes" ForeColor="#CC3300"
                            Font-Bold="False" Font-Italic="True" Display="Dynamic"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Grupo:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:CheckBoxList ID="lstGrupo" runat="server" CellPadding="0" CellSpacing="0" Height="80px"
                                RepeatColumns="5" RepeatDirection="Horizontal" Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Campanha:
                    </td>
                    <td>
                        <div class="CheckBoxList">
                            <asp:CheckBoxList ID="lstCampanha" runat="server" CellPadding="0" CellSpacing="0"
                                Height="80px" RepeatColumns="5" RepeatDirection="Horizontal" Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td style="height: 5px;">
                        <asp:CheckBox ID="cbOperador" runat="server" Text="Operador" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="cbAtivo" runat="server" Text="Ativo" />
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
                <tr>
                    <td>
                    </td>
                    <td style="height: 5px;">
                        <asp:HiddenField ID="IdCadastrar" runat="server" />
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
