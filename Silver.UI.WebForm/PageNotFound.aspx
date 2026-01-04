<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="PageNotFound.aspx.cs" Inherits="Silver.UI.Web.Presentation.PageNotFound" %>

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
                                <img src="../../Imagens/User2.png" alt="Usuário" />
                            </td>
                            <td>
                                <asp:Label ID="lbNome" runat="server"></asp:Label>
                            </td>
                            <td>
                                <img src="../../Imagens/talk.png" alt="Ramal" />
                            </td>
                            <td>
                                <asp:Label ID="lbRamal" runat="server"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
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
        <legend>Página não encontrada:</legend>
        <br />
        <br />
        <br />
        <table style="text-align: left; width: 65%; padding: 15px; margin: 10px;" align="center">
            <tr>
                <td class="style7" align="center">
                    <table Align="center">
                        <tr>
                            <td align="center" class="style5">
                                <strong><span class="style6">A página que você está solicitando não foi 
                                encontrada!</span></strong>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="style5">
                                <img alt="imagem" class="style2" src="Imagens/homens-trabalhando.gif" align="middle" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <em>O Administrador do sistema foi notificado com este problema.</em>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <strong>Caso você entenda que esta página deveria ser exibida, entre em contato 
                                com o administrador do sistema.</strong>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
