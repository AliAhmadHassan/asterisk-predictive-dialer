<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Dashboard.Default" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <iframe frameborder="0" height="700px" width="99%" frameborder="0" scrolling="yes" name="dashboard"
        src="<%= ResolveUrl("~/Pages/Dashboard/Dashboard.aspx") %>"></iframe>
    </asp:Content>
