<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUsuario.ascx.cs" Inherits="Silver.UI.Web.Presentation.UserControls.ucUsuario" %>
<asp:Panel ID="Panel1" runat="server">
    <table style="width: 100%; margin: 0px;" cellpadding="0px" cellspacing="0px">
        <tr>
            <td style="width:100%">
                <asp:TextBox runat="server" ID="tbBuscar" Width="100%">
                </asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btBuscar" Text="Buscar" runat="server" OnClick="btBuscar_Click" CssClass="ButtonBuscar" />
            </td>
            <td>
                <asp:Button ID="btNovo" Text="Novo" runat="server" CssClass="ButtonNovo"
                    ValidationGroup="Novo" />
            </td>
            <td>
                <a href="#" onclick="abrir_popup('Ajuda/Registers/Campanha.htm','Ajuda da Campanha','550','600')">
                    <asp:Image ID="imgAjuda" runat="server" AlternateText="Ajuda" ImageUrl="~/Imagens/ajuda.png"
                        Width="46px" Height="46px" />
                </a>
            </td>
        </tr>
    </table>
</asp:Panel>
