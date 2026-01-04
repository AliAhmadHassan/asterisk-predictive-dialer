<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Silver.UI.Web.Presentation.Login2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <title>Login - Silver</title>
    <link href="Styles/Silver.Login.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Silver.Mensagem.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery.js" type="text/javascript"></script>
    <script src="../Js/Plugs/jquery.ui/jquery-ui.js" type="text/javascript"></script>
    <script src="Js/Silver.Common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div-login">
        <div id="div-login-content">
            <table id="table-login">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="middle">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Usuário / Ramal
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="tbRamal" runat="server" Width="300px" title="Informe seu Ramal"
                            MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvRamal" runat="server" ControlToValidate="tbRamal"
                            ErrorMessage="* Favor preencher o ramal" Font-Italic="True" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Senha
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="tbSenha" runat="server" Width="300px" TextMode="Password" title="Informe sua senha"
                            MaxLength="80"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rvSenha" runat="server" ControlToValidate="tbSenha"
                            ErrorMessage="* Favor preencher a senha" ForeColor="Red" Font-Bold="False" Font-Italic="True"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbMensagem" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btOkMensagem" runat="server" CssClass="botao-submit" OnClick="btLogin_Click"
                            SkinID="btnLogin" ToolTip="Clique para entrar no sistema" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="font-size:11px; height:50px; padding:1px;" valign="bottom">
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    Orcozol Assessoria e Consultoria de Cobranças Ltda. Copyright® <asp:Label ID="lbl_copyright" runat="server"></asp:Label>
                                </td>
                                <td style="padding-right:55px;">
                                    Versão:
                                    <asp:Label ID="lbl_versao" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
