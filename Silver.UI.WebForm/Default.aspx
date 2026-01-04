<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Silver.UI.Web.Presentation.Comum.Default1" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <center>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Image ID="img_logotipo_aplicacao" runat="server" AlternateText="" /><br />
        <asp:Label ID="lbl_nome_aplicacao" runat="server" Style="font-size: xx-large; font-weight: 700" />
        <br />
        Versão da aplicação:
        <asp:Label ID="lbl_versao_aplicacao" runat="server" /><br />
        <br />
        <br />
        <asp:Label ID="lbl_copyright_aplicacao" runat="server" /><br />
        <br />
    </center>
    <br />
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server"></asp:Label>
</asp:Content>
