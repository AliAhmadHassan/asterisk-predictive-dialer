<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="RelCampanha.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Reports.RelCampanha" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: 12px;
            color: #000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#img_acompanhemento')
              .hover(function () {
                  $("#div_acompanhamento").show("fast", function () { height: 500; width: 200; });
              }, function () {
                  return;
              });

            $('#div_acompanhamento')
              .hover(function () {
                  return;
              }, function () {
                  $("#div_acompanhamento").slideUp("fast", function () { });
              });
        });
    </script>
    <asp:Literal ID="script_jquery" runat="server"></asp:Literal>
    <div class="box-panel">
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
                    <asp:DropDownList ID="ddlCampanhas" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCampanhas_SelectedIndexChanged"
                        Width="300px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:ImageButton ID="btn_filtrar" runat="server" ImageUrl="~/Imagens/dash_confirmar.png"
                        title="Clique para aplicar os filtros" OnClick="btn_filtrar_Click" />
                </td>
                <td align="center" valign="middle" style="cursor: pointer;">
                    <img src="../../Imagens/computer.png" alt="Acompanhamento" id="img_acompanhemento" />
                    <asp:Literal ID="lit_script_andamento" runat="server"></asp:Literal>
                    <asp:UpdatePanel ID="upd_panel_acompanhamento" runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <img src="../../Imagens/ajax-loader.gif" alt="" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <div id="div_acompanhamento" class="div_poopup" style="min-width: 650px; min-height: 300px;">
                                <div class="Dash_ModalPopupHeader">
                                    Solicitações
                                </div>
                                <fieldset style="min-width: 95%; min-height: 230px">
                                    <legend>Acompanhamento de solicitações: [<asp:Label ID="lbl_opcao_campanha" runat="server">Orcozol Assessoria</asp:Label>]</legend>
                                    <table style="width: 100%" id="tbl_opcao">
                                        <tr>
                                            <td>
                                                <b>Discador:</b>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_status_discador" runat="server" ForeColor="Green" Font-Size="11px">Não Verificado</asp:Label>
                                            </td>
                                            <td>
                                                <b>Últ. Registro:</b>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_atualizacao" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #EFEFEF">
                                            <th style="width: 50px;">
                                                Código
                                            </th>
                                            <th>
                                                Evento
                                            </th>
                                            <th>
                                                Dt/hr Início
                                            </th>
                                            <th>
                                                Situação
                                            </th>
                                            <th align="left">
                                                Andamento
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_id_1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_evento_1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_1" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_situacao_1" runat="server" />
                                            </td>
                                            <td align="center">
                                                <div id="div_progresso_1" style="height: 20px; width: 100px; float: left;">
                                                </div>
                                                <asp:Label ID="lbl_andamento_1" runat="server" />%
                                            </td>
                                        </tr>
                                        <tr style="background-color: #f5f5f5">
                                            <td>
                                                <asp:Label ID="lbl_id_2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_evento_2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_2" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_situacao_2" runat="server" />
                                            </td>
                                            <td align="center">
                                                <div id="div_progresso_2" style="height: 20px; width: 100px; float: left;">
                                                </div>
                                                <asp:Label ID="lbl_andamento_2" runat="server" />%
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_id_3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_evento_3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_3" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_situacao_3" runat="server" />
                                            </td>
                                            <td align="center">
                                                <div id="div_progresso_3" style="height: 20px; width: 100px; float: left;">
                                                </div>
                                                <asp:Label ID="lbl_andamento_3" runat="server" />%
                                            </td>
                                        </tr>
                                        <tr style="background-color: #efefef">
                                            <td>
                                                <asp:Label ID="lbl_id_4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_evento_4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_4" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_situacao_4" runat="server" />
                                            </td>
                                            <td align="center">
                                                <div id="div_progresso_4" style="height: 20px; width: 100px; float: left;">
                                                </div>
                                                <asp:Label ID="lbl_andamento_4" runat="server" />%
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_id_5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_evento_5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_hora_5" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_situacao_5" runat="server" />
                                            </td>
                                            <td align="center">
                                                <div id="div_progresso_5" style="height: 20px; width: 100px; float: left;">
                                                </div>
                                                <asp:Label ID="lbl_andamento_5" runat="server" />%
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td align="center" valign="middle">
                    <asp:ImageButton ID="btn_iniciar_campanha" runat="server" ImageUrl="~/Imagens/Ico/iniciar.png"
                        OnClick="btn_iniciar_campanha_Click" Width="16px" />
                    <br />
                    Iniciar
                </td>
                <td align="center" valign="middle">
                    <asp:ImageButton ID="btn_parar_campanha" runat="server" ImageUrl="~/Imagens/Ico/parar.png"
                        OnClick="btn_parar_campanha_Click" Width="16px" />
                    <br />
                    Parar
                </td>
                <td align="center" valign="middle">
                    <asp:ImageButton ID="btn_reiniciar" runat="server" ImageUrl="~/Imagens/Ico/continuar.png"
                        OnClick="btn_reiniciar_Click" Width="16px" Visible="False" />
                    <br />
                </td>
                <td align="center" valign="bottom">
                    <a href="../Dashboard/HistoricoRequisicao.aspx" target="_parent">Histórico de solicitações</a>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_dashboard">
        <div class="silver-boxer" style="background: #0cb0f6;">
            <table>
                <tr>
                    <td colspan="2" style="vertical-align: middle">
                        <strong><span class="style1">Carga:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="lbl_id_carga" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong><br />
                        <strong><span class="style1">Situação:
                            <asp:Label ID="lbl_nome_carga" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong>,
                        <b>[<asp:Label ID="lbl_porcentagem_carga" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label>]</b>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #f0c06b;">
            <table>
                <tr>
                    <td colspan="2" style="vertical-align: super">
                        <strong><span class="style1">Mailing:&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_id_mailing"
                            runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong><br />
                        <strong><span class="style1">Hr. Início:&nbsp;<asp:Label ID="lbl_inicio_mailing"
                            runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong><br />
                        <strong><span class="style1">Situação:&nbsp;
                            <asp:Label ID="lbl_situacao_mailing" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong>,
                        <b>[<asp:Label ID="lbl_porcentagem_mailing" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label>]</b>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #000; color: #fff;">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1" style="color: #fff;">Campanha:
                            <asp:Label ID="lbl_id_campanha" runat="server" Font-Bold="False" Font-Size="11px"></asp:Label></span></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_nome_campanha" runat="server" Font-Bold="False" Font-Size="16px"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #6bf0c0;">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1">Conectados</span></strong>
                    </td>
                </tr>
                <tr style="text-align: center">
                    <td>
                        <table style="border: none" id="Table2">
                            <tr>
                                <td>
                                    <img src="../../Imagens/User2.png" alt="Usuário" id="img1" title="Total de operadores na fila" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_total_conectados" runat="server" Font-Bold="True" Font-Size="16px">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #E45353;">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1">Desconectados</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table style="border: none" id="Table6">
                            <tr>
                                <td>
                                    <img src="../../Imagens/User2.png" alt="Usuário" id="img5" title="Total de operadores aguardando na fila" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_total_desconectado" runat="server" Font-Bold="True" Font-Size="16px">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #AA55FF;">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1">Em Pausa</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table style="border: none" id="Table5">
                            <tr>
                                <td>
                                    <img src="../../Imagens/User2.png" alt="Usuário" id="img4" title="Total de operadores aguardando na fila" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_total_empausa" runat="server" Font-Bold="True" Font-Size="16px">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #FDD850">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1">Atendendo</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table style="border: none" id="Table3">
                            <tr>
                                <td>
                                    <img src="../../Imagens/User2.png" alt="Usuário" id="img2" title="Total de operadores em atendimento na fila" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_total_atendendo" runat="server" Font-Bold="True" Font-Size="16px">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #54A5E1">
            <table>
                <tr>
                    <td>
                        <strong><span class="style1">Aguardando</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table style="border: none" id="Table4">
                            <tr>
                                <td>
                                    <img src="../../Imagens/User2.png" alt="Usuário" id="img3" title="Total de operadores aguardando na fila" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_total_aguardando" runat="server" Font-Bold="True" Font-Size="16px">0</asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #0ed0db;">
            <table style="width: 100%; text-align: center">
                <tr>
                    <th style="font-size: 17px; font-weight: bolder" title="Tempo de Espera">
                        <img src="../../Imagens/clock.png" alt="" />
                        T.E
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TempoEspera" runat="server" Font-Bold="True" Font-Size="12px">00:00:00</asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="silver-boxer" style="background: #f5984b;">
            <table style="width: 100%; text-align: center">
                <tr>
                    <th style="font-size: 17px; font-weight: bolder">
                        <img src="../../Imagens/clock.png" alt="" />
                        Idle
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_idle" runat="server" Font-Bold="True" Font-Size="13px">00:00:00</asp:Label><br />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="tabs_acompanhamento" style="margin-top: 15px;">
        <ul>
            <li><a href="#tabs-1">Campanha:
                <asp:Label ID="lbl_nomecampanha" runat="server"></asp:Label>, &nbsp;&nbsp;&nbsp;&nbsp;
                L.E:
                <asp:Label ID="lblLigEntregue" runat="server" Style="font-weight: bold"></asp:Label>,
                &nbsp;&nbsp;&nbsp;&nbsp; Abandono:
                <asp:Label ID="lblAbandono" runat="server" Style="font-weight: bold"></asp:Label>,&nbsp;&nbsp;&nbsp;&nbsp;
                SLA:
                <asp:Label ID="lblTaxaSLA" runat="server" Style="font-weight: bold"></asp:Label>
            </a></li>
        </ul>
        <div id="tabs-1" class="tabs-acompanhamento">
            <asp:Literal ID="ltlMapa" runat="server"></asp:Literal>
        </div>
    </div>
    <div id="tabs_ligacoes" class="tabs-acompanhamento">
        <ul>
            <li><a href="#tabs-2">Ligações em espera</a></li>
        </ul>
        <div id="tabs-2">
            <asp:Literal ID="ltlAguardando" runat="server"></asp:Literal>
        </div>
    </div>
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
