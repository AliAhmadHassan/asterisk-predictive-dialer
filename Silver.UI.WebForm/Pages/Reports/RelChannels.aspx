<%@ Page Title="Relatório de canais E1" Language="C#" MasterPageFile="~/Default.Master"
    AutoEventWireup="true" CodeBehind="RelChannels.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Reports.RelChannels" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="box-panel">
        <table style="border: none">
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
                    <asp:ImageButton ID="btn_filtrar" runat="server" ImageUrl="~/Imagens/dash_confirmar.png"
                        OnClick="btn_filtrar_Click" title="Clique para aplicar os filtros" />
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    <div style="width: 98.5%; height: 35px; border: 1px dotted #EFEFEF;">
        <table>
            <tr>
                <td>
                    <b>Atualização automática:</b> <i>10 segundos</i>,
                </td>
                <td>
                    <b>Última Atualização:</b>
                </td>
                <td>
                    <asp:Label ID="lbl_hora_atualizacao" runat="server"><%= DateTime.Now.ToString() %></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:Literal ID="lit_e1" runat="server"></asp:Literal>
    <%--<div id="tabs_acompanhamento">
        <ul>
            <li>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <a href="#tabs-1">Grupo de Discagem:
                            <asp:Label ID="lblGrupo" runat="server"></asp:Label>, &nbsp;&nbsp;&nbsp;&nbsp; Operadora:
                            <asp:Label ID="lblContext" runat="server" Style="font-weight: bold"></asp:Label>,
                            &nbsp;&nbsp;&nbsp;&nbsp; Id. Porta:
                            <asp:Label ID="lblPortId" runat="server" Style="font-weight: bold"></asp:Label>
                            , &nbsp;&nbsp;&nbsp;&nbsp; E1&#39;s Conectados:
                            <asp:Label ID="lblE1Ocupado" runat="server" Style="font-weight: bold"></asp:Label>
                            , &nbsp;&nbsp;&nbsp;&nbsp; E1&#39;s Livres:
                            <asp:Label ID="lblE1Disponivel" runat="server" Style="font-weight: bold"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp; </a>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="timer_update" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </li>
        </ul>
        <div id="tabs-1" class="tabs-acompanhamento">
            <asp:UpdatePanel ID="upd01" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true">
                        <ProgressTemplate>
                            <img src="../../Imagens/ajax-loader.gif" height="18px" alt="carregando..." />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:Literal ID="ltlMapa" runat="server"></asp:Literal>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="timer_update" EventName="Tick" />
                    <asp:AsyncPostBackTrigger ControlID="btn_filtrar" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>--%>
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
</asp:Content>
