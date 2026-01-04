<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/MasterPages/Operador.Master" AutoEventWireup="true" CodeBehind="Operador.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Operator.Operador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div>
    <table width="100%">
        <tr>
            <td style="width:30%;">
                <asp:Label ID="lbNome" Text="" runat="server" />         
            </td>
            <td align="center" rowspan="2" >
                <asp:Label ID="lbTitulo" Text="" runat="server" /> 
            </td>
            <td align="right" style="width:30%;">
                <asp:Button ID="btSair" Text="Sair" runat="server" onclick="btSair_Click" />
            </td>        
        </tr>
        <tr> 
            <td>
                <asp:Label ID="lbRamal" Text="" runat="server" />
            </td>        
        </tr>
    </table>
</div>
<div>
    <table width="100%" style="text-align:center;">
        <tr>
            <td colspan="2" style="width:30%"></td>
            <td rowspan="5">
                <asp:Panel ID="PanelMensagem" Width="250px" Height="100px" runat="server" Visible="false">
                </asp:Panel>
            </td>
            <td colspan="2" rowspan="5" style="width:30%"></td>
        </tr> 
        <tr>
            <td colspan="2" style="width:30%"> Pausas </td>
        </tr>
        <tr>
            <td colspan="2" style="width:30%">
                <asp:ListBox ID="lstPausas" runat="server" Width="100%"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr> 
        <tr>
            <td style="width:15%">
                <asp:Button ID="btIniciarPausa" Text="Iniciar Pausa" runat="server" onclick="btIniciarPausa_Click" Enabled="true" Width="150px" />
            </td>
            <td>
                <asp:Button ID="btRetornarPausa" Text="Retornar da Pausa" runat="server" 
                    Enabled="false" Width="150px" onclick="btRetornarPausa_Click" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>
