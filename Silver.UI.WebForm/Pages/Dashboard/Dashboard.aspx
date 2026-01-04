<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Dashboard.Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <script src="../../Js/jquery.js" type="text/javascript"></script>
    <link href="style/redmond/jquery.ui.base.css" rel="stylesheet" type="text/css" />
    <link href="style/redmond/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="style/redmond/jquery.ui.all.css" rel="stylesheet" type="text/css" />
     <link href="../../Js/Plugs/checkstyle/prettyCheckable.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/Plugs/checkstyle/prettyCheckable.js" type="text/javascript"></script>
    <script src="js/Dashboard.js" type="text/javascript"></script>
    <title>Dashboard</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <table style="width: 98.5%; background-color: #f5f9f9; border: 1px solid #EFEFEF;
        padding: 5px; margin: 0px" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="middle">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="border-right-style: solid; border-right-width: 1px; border-right-color: #53aad1">
                            <table style="float: right;">
                                <tr>
                                    <td style="padding: 5px;">
                                        <asp:ImageButton ID="btn_grupos" runat="server" ImageUrl="~/Imagens/dash_grupo.png"
                                            title="Selecionar grupos de visualização" />
                                        <div id="div_grupos" class="div_poopup">
                                            <div class="Dash_ModalPopupHeader">
                                                Grupos:
                                            </div>
                                            <fieldset style="width: 94%;">
                                                <legend>Grupo Hierárquico:</legend>
                                                <asp:TreeView ID="trv_grupos" runat="server" Width="100%" OnSelectedNodeChanged="trv_grupos_SelectedNodeChanged"
                                                    ShowLines="True">
                                                    <NodeStyle Width="100%" />
                                                    <RootNodeStyle Font-Bold="False" Font-Underline="False" HorizontalPadding="10px" />
                                                </asp:TreeView>
                                            </fieldset>
                                        </div>
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:ImageButton ID="btn_opcoes" runat="server" title="Configurações da Campanha"
                                            ImageUrl="~/Imagens/dash_config.png" />
                                        <div id="div_opcoes" class="div_poopup">
                                            <div class="Dash_ModalPopupHeader">
                                                Opções
                                            </div>
                                            <fieldset style="width: 94%;">
                                                <legend>Selecione uma campanha:</legend>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <div class="CheckBoxList">
                                                                <asp:RadioButtonList ID="ddl_campanhas" runat="server" CellPadding="0" CellSpacing="0"
                                                                    Height="80px" RepeatColumns="3" CssClass="checkbox_style" RepeatDirection="Horizontal"
                                                                    Width="100%" Font-Bold="true">
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <table style="text-align: center;">
                                                <tr>
                                                    <td style="padding: 5px" valign="middle">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btn_iniciar_campanha" runat="server" ImageUrl="~/Imagens/Ico/iniciar.png"
                                                                        OnClick="btn_iniciar_campanha_Click" Width="16px" />
                                                                </td>
                                                                <td>
                                                                    Iniciar
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 5px" valign="middle">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btn_parar_campanha" runat="server" ImageUrl="~/Imagens/Ico/parar.png"
                                                                        OnClick="btn_parar_campanha_Click" Width="16px" />
                                                                </td>
                                                                <td>
                                                                    Parar
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 5px" valign="middle">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:ImageButton ID="btn_reiniciar" runat="server" ImageUrl="~/Imagens/Ico/continuar.png"
                                                                        OnClick="btn_reiniciar_Click" Width="16px" />
                                                                </td>
                                                                <td>
                                                                    Reiniciar
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 5px" valign="middle">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <a href="HistoricoRequisicao.aspx">Histórico de solicitações</a>
                                                                </td>
                                                                <td>
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:ImageButton ID="btn_carga" runat="server" ImageUrl="~/Imagens/dash_paste.png"
                                            title="Selecionar cargas para visualização" />
                                        <div id="div_carga" class="div_poopup" style="width: 600px; height: 300px;">
                                            <div class="Dash_ModalPopupHeader">
                                                Cargas
                                            </div>
                                            <fieldset style="width: 95%; height: 230px">
                                                <legend>Selecione as cargas:</legend>
                                                <div class="CheckBoxList" style="height: 100%;">
                                                    <asp:CheckBoxList ID="lst_carga" Font-Size="10pt" runat="server" CellPadding="0"
                                                        CellSpacing="0" CssClass="checkbox_style" RepeatColumns="2" RepeatDirection="Horizontal"
                                                        Width="100%">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                    <td style="padding: 5px;">
                                        <asp:ImageButton ID="btn_filtrar" runat="server" ImageUrl="~/Imagens/dash_confirmar.png"
                                            title="Clique para aplicar os filtros" OnClick="btn_filtrar_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="middle" style="border-right-style: solid; border-right-width: 1px; border-right-color: #53aad1">
                            <table>
                                <tr>
                                    <td valign="middle">
                                        <asp:ImageButton ID="imb_logout" ClientIDMode="Static" runat="server" ImageUrl="~/Imagens/logout_2.png"
                                            AlternateText="Logout" title="Sair do sistema" OnClick="imb_logout_Click" OnClientClick="Logout(this)" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="lblSair" runat="server" Text="Sair"></asp:Label>
                                    </td>
                                    <td valign="middle">
                                        <img src="../../Imagens/User2.png" alt="Usuário" id="img_usuario" title="Usuário logado" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="lbNome" runat="server"> Projeto Discador Silver V1.0.0</asp:Label>
                                    </td>
                                    <td valign="middle">
                                        <img src="../../Imagens/talk.png" alt="Ramal" title="Ramal conectado" />
                                    </td>
                                    <td valign="middle">
                                        <asp:Label ID="lbRamal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" style="border-right-style: solid; border-right-width: 1px; border-right-color: #53aad1">
                            <table>
                                <tr>
                                    <td style="font-size: medium; font-weight: 700">
                                        Grupo Selecionado:
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_gruposelecionado" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <hr style="background-color: #fff; border: none; border-bottom: 1px solid #EFEFEF;
        width: 100%; text-align: center;" />
    <div id="wrapper">

        <div class="ui-widget-content ui-corner-all" id="Hierarquia" style="height: 99%;
            position: relative; float: left; width: 33%; overflow: hidden; height: 300px;
            padding: 5px;">
            <asp:Literal ID="lit_carga" runat="server"></asp:Literal>
        </div>
        <div class="ui-widget-content ui-corner-all" id="Div2" style="height: 99%; position: relative;
            height: 300px; float: left; width: 33%; overflow-y: none; padding: 5px;">
            <asp:Literal ID="lit_campanha" runat="server"></asp:Literal>
        </div>
        <div class="ui-widget-content ui-corner-all" id="Div4" style="height: 99%; position: relative;
            height: 300px; float: left; width: 30%; overflow: auto; overflow-y: none; padding: 5px;">
            <asp:DataList ID="dtl_agressividade" runat="server" RepeatColumns="2" OnItemDataBound="dtl_agressividade_ItemDataBound"
                Width="98%" CellPadding="0" GridLines="Both" RepeatDirection="Horizontal" RepeatLayout="Flow">
                <ItemStyle Width="50%" />
                <ItemTemplate>
                    <asp:Panel ID="pnl_agressividade" runat="server">
                    </asp:Panel>
                </ItemTemplate>
            </asp:DataList>
            <asp:Literal ID="lit_agressividade" runat="server"></asp:Literal>
        </div>

        <div class="ui-widget-content ui-corner-all" id="Div1" style="height: 99%; position: relative;
            height: 300px; float: left; width: 33%; overflow: auto; padding: 5px; text-align: center;">
            <h3 style="text-align: center">
                <b>Custos</b>
            </h3>
            <img alt="imagem" src="../../Imagens/homens-trabalhando.gif" align="middle" width="100px" />
        </div>
         <div class="ui-widget-content ui-corner-all" id="Div5" style="height: 99%; position: relative;
            height: 300px; float: left; width: 33%; overflow: hidden; padding: 5px;">
            <asp:Literal ID="lit_discagemoperador" runat="server"></asp:Literal>
        </div>

        <div class="ui-widget-content ui-corner-all" id="Div3" style="height: 99%; position: relative;
            height: 300px; float: left; width: 30%; overflow: hidden; padding: 5px;">
            <asp:Literal ID="lit_resultado" runat="server"></asp:Literal>
        </div>
        
        <div class="ui-widget-content ui-corner-all" id="Div8" style="height: 99%; position: relative;
            height: 300px; float: left; width: 33%; overflow: hidden; padding: 5px;">
            <asp:Literal ID="lit_idle" runat="server"></asp:Literal>
        </div>
        <div class="ui-widget-content ui-corner-all" id="Div7" style="height: 99%; position: relative;
            height: 300px; float: left; width: 33%; overflow: hidden; padding: 5px;">
            <asp:Literal ID="lit_tempologado" runat="server"></asp:Literal>
        </div>
        <div class="ui-widget-content ui-corner-all" id="Div9" style="height: 99%; position: relative; text-align:center;
            height: 300px; float: left; width: 30%; overflow: hidden; padding: 5px;">
            <h3 style="text-align: center">
                <b>TME</b>
            </h3>
            <br />
            <asp:Literal ID="lit_tempoespera" runat="server" Visible="False"></asp:Literal>
            <br />
            <br />
            <img alt="imagem" src="../../Imagens/homens-trabalhando.gif" align="middle" width="100px" /></div>

        <div class="ui-widget-content ui-corner-all" id="Div6" style="height: 99%; position: relative;
            height: 300px; float: left; width: 98%; overflow: hidden; padding: 5px;">
            <asp:Literal ID="lit_linhatempo" runat="server"></asp:Literal>
        </div>
    </div>
    </form>
</body>
</html>
