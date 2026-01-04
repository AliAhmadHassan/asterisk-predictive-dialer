<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Operador.aspx.cs"
    Inherits="Silver.UI.Web.Presentation.Pages.Operator.Operador" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Styles/Silver.Operador.css" rel="stylesheet" type="text/css" />
    <script src="../Js/jquery.js" type="text/javascript"></script>
    <script src="../Js/Plugs/jquery.ui/jquery-ui.js" type="text/javascript"></script>
    <script src="../Js/Silver.Operador.js" type="text/javascript"></script>
    <style type="text/css">
        .ButtonLogout
        {
        }
        .style1
        {
            font-weight: bold;
        }
        .style4
        {
            width: 100%;
            vertical-align:middle;
        }
        .style5
        {
        }
        .style6
        {
        }
        .Panel_Form
        {
        }
        .style9
        {
            font-size: small;
        }
        .style10
        {
            font-weight: bold;
            font-size: small;
        }
        .style11
        {
            font-size: 11pt;
        }
        .style12
        {
            font-size: x-small;
        }
        .style13
        {
            width: 118px;
        }
        .style14
        {
            font-size: medium;
            font-weight: bold;
            color: #003399;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManagerRequest" runat="server">
    </asp:ScriptManager>
    <div class="header">
        <asp:Image ID="ImgLogo" runat="server" ImageUrl="~/Imagens/Logo.png" Width="85px"
            ImageAlign="Middle" />
        <asp:Label ID="lbTitulo" Text="" runat="server" Style="color: #FFFFFF" />
        &nbsp;Versão: beta - 1.0.0
    </div>
    <table class="style4" style="text-align:center" align="center">
        <tr>
            <td valign="top" style="text-align:left;">
                <div id="tabs" style="width:50%">
                    <ul>
                        <li><a href="#tabs-1">Operador</a></li>
                        <li><a href="#tabs-2">Log de Atividades</a></li>
                        <li><a href="#tabs-3">Checkout</a></li>
                    </ul>
                    <div id="tabs-1" style="background-color: #fff; text-align: left;">
                        
                            <table class="style4" cellpadding="0" cellspacing="0" align="center">
                                <tr>
                                    <td class="style6">
                                        <ul style="list-style-type: circle; margin: 2px 2px 10px 2px; list-style-position: outside;
                                            list-style: none; margin-left: 0px; padding: 0px; margin: 0px;">
                                            <li style="border-bottom: 1px solid #EFEFEF;">
                                                <img src="../../Imagens/User2.png" alt="Usuário" id="img_usuario" title="Usuário logado"  width="20px"/>&nbsp;&nbsp;
                                                <asp:Label ID="lbNome" Text="" runat="server" CssClass="style10" />
                                            </li>
                                            <li style="border-bottom: 1px solid #EFEFEF;">
                                                <img src="../../Imagens/talk.png" alt="Ramal" title="Ramal conectado" 
                                                    width="20" />&nbsp;&nbsp;
                                                <asp:Label ID="lbRamal" Text="" runat="server" CssClass="style10" />
                                            </li>
                                            <li style="border-bottom: 1px solid #EFEFEF;">
                                                <asp:ImageButton ID="imb_logout" ClientIDMode="Static" runat="server" ImageUrl="~/Imagens/logout_2.png"
                                                    AlternateText="Logout" title="Sair do sistema" OnClick="imb_logout_Click" 
                                                    OnClientClick="Logout(this)" Width="20px" />&nbsp;&nbsp;
                                                <asp:Label ID="lbl_sair" runat="server" CssClass="style10">Sair</asp:Label>
                                            </li>
                                        </ul>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style6">
                                        <hr />
                                        <table class="style4">
                                            <tr>
                                                <td class="style13">
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style13" 
                                                    style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                <asp:ImageButton ID="imb_logout0" ClientIDMode="Static" runat="server" ImageUrl="~/Imagens/Ico/bank_cards.png"
                                                    AlternateText="Logout" title="Sair do sistema" OnClick="imb_logout_Click" 
                                                        OnClientClick="Logout(this)" Width="20px" />
                                                &nbsp;CPF Cliente:
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                    <asp:Label ID="lbl_proximo_cpf" runat="server" Style="font-family: 'Trebuchet MS'"
                                                        CssClass="style14"></asp:Label>
                                                </td>
                                                <td align="right" valign="middle" style="border-bottom-style: solid; border-bottom-width: 1px;
                                                    border-bottom-color: #EFEFEF">
                                                    <a href="#" id="lnk_atender" style="color:#f72e2e">[Atender]</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style13" 
                                                    style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                <img src="../../Imagens/User2.png" alt="Usuário" id="img_usuario0" 
                                                        title="Usuário logado"  width="20px"/> Cliente:</td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                    <asp:Label ID="lbl_proximo_nome" runat="server" Style="font-family: 'Trebuchet MS'"
                                                        CssClass="style14"></asp:Label>
                                                </td>
                                                <td align="right" valign="middle" style="border-bottom-style: solid; border-bottom-width: 1px;
                                                    border-bottom-color: #EFEFEF">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="style13" 
                                                    style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                    <span class="style11">
                                                <asp:ImageButton ID="imb_logout1" ClientIDMode="Static" runat="server" ImageUrl="~/Imagens/Ico/bookmark.png"
                                                    AlternateText="Logout" title="Sair do sistema" OnClick="imb_logout_Click" 
                                                        OnClientClick="Logout(this)" Width="20px" />
                                                &nbsp;Telefone</span>:
                                                </td>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                    <asp:Label ID="lbl_proximo_telefone" runat="server" Style="font-family: 'Trebuchet MS'"
                                                        CssClass="style14"></asp:Label>
                                                </td>
                                                <td align="right" valign="middle" style="border-bottom-style: solid; border-bottom-width: 1px;
                                                    border-bottom-color: #EFEFEF">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style5" align="left">
                                        <asp:UpdatePanel ID="pnlUsuario" runat="server">
                                            <ContentTemplate>
                                                <table width="100%" style="text-align: left; border: 1PX SOLID #d9dddd;" align="left">
                                                    <tr>
                                                        <td style="width: 30%; background-color: #0c669c; color: #FFFFFF; font-weight: bolder;
                                                            font-style: normal; font-variant: inherit;">
                                                            Registrar Pausa
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF;" 
                                                            class="style9">
                                                            Selecione uma pausa:</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF;">
                                                            <asp:DropDownList ID="lstPausas" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF;" 
                                                            align="left">
                                                            <asp:UpdateProgress ID="upd_progress_01" runat="server">
                                                                <ProgressTemplate>
                                                                    <img src="../Imagens/ajax-loader-operador.gif" alt="" />
                                                                    <span class="style12">Por favor, aguarde...</span>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" 
                                                            style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                            <asp:Button ID="btIniciarPausa" Text="Ok" runat="server" OnClick="btIniciarPausa_Click"
                                                                Enabled="true" Width="150px" BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px"
                                                                SkinID="btn_tela_operador" />
                                                            <asp:Button ID="btRetornarPausa" Text="Retornar da Pausa" runat="server" Enabled="false"
                                                                Width="150px" OnClick="btRetornarPausa_Click" BorderColor="#333333" BorderStyle="Solid"
                                                                BorderWidth="1px" SkinID="btn_tela_operador" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #EFEFEF">
                                                            <asp:Label ID="lbl_mensagem" runat="server" Style="color: #009933; font-family: 'Trebuchet MS'"
                                                                CssClass="style9"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        
                    </div>
                    <div id="tabs-2" style="background-color: #fff; text-align: left;">
                        <ul id="lista_cpf_atendidos" style="list-style-type: decimal" class="lista_operador">
                        </ul>
                    </div>
                    <div id="tabs-3" style="background-color: #fff;">
                        <ul id="lista_log" style="list-style-type: decimal" class="lista_operador">
                        </ul>
                    </div>
                </div>
                <table>
                    <tr>
                        <td align="left" valign="middle">
                            <h1 align="left">
                                &nbsp;</h1>
                        </td>
                        <td align="right">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" style="" align="center">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Panel ID="PanelMensagem" Width="686px" Height="169px" runat="server" BorderStyle="Groove"
        CssClass="Panel_Form" Visible="false">
        <table style="vertical-align: middle; width: 100%">
            <tr>
                <td align="center" style="text-align: center; background-color: #eee9e9;" class="style1">
                    ATENÇÃO!
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center;" valign="top">
                    <asp:Label ID="lbMensagem" runat="server" Font-Italic="True" ForeColor="#CC3300"
                        Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="text-align: center;" valign="top">
                    <asp:Button ID="btOkMensagem" runat="server" CssClass="ButtonOk" OnClick="btOkMensagem_Click"
                        Text="" Width="34px" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </asp:Panel>
    </form>
</body>
</html>
