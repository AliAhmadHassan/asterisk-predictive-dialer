<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="ConsultaGravacao.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.Registers.ConsultaGravacao" %>

<%@ MasterType VirtualPath="~/Default.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="box-panel">
        <table style="width: auto;" cellpadding="0" cellspacing="0">
            <tr>
                <td valign="middle">
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
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_campanha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_campanha_SelectedIndexChanged"
                                    SkinID="ddl_dashboard" Width="100%" Visible="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:UpdatePanel runat="server" ID="pnl_nome_campanha">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lbl_campanha_selecionada" Text="Nenhuma campanha selecionada"
                                            Visible="false" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddl_campanha" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CalendarExtender ID="txt_inicio_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txt_inicio">
                                </asp:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddl_campanha"
                                    Display="Dynamic" ErrorMessage="Informe o critério de seleção" ForeColor="Red"
                                    ValidationGroup="form_buscar"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:CalendarExtender ID="txt_fim_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txt_fim">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                &nbsp;
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
        <fieldset style="padding: 10px; width: auto; margin-top: 2px">
            <legend>Filtro: </legend>
            <table>
                <tr>
                    <td>
                        Início
                    </td>
                    <td>
                        <asp:TextBox ID="txt_inicio" runat="server" SkinID="txt_consulta_gravacao" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_inicio"
                            Display="Dynamic" ErrorMessage="Informe o critério de seleção" ForeColor="Red"
                            ValidationGroup="form_buscar"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        CPF/CNPJ:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_cpfcnpj" runat="server" Width="129px" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        Fim
                    </td>
                    <td>
                        <asp:TextBox ID="txt_fim" runat="server" SkinID="txt_consulta_gravacao" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_fim"
                            Display="Dynamic" ErrorMessage="Informe o critério de seleção" ForeColor="Red"
                            ValidationGroup="form_buscar"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        Operador/Ramal:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ramaloperador" runat="server" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btBuscar" runat="server" CssClass="ButtonBuscar" ImageUrl="~/Imagens/search-icon.png"
                            title="Iniciar a busca" OnClick="btBuscar_Click" ValidationGroup="form_buscar" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lbl_mensagem" runat="server" ForeColor="Red"></asp:Label>
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
                </tr>
            </table>
        </fieldset>
        <br />
        Total de registros encontrados:
        <asp:Label ID="lbl_totalregistros" runat="server" Style="font-weight: 700" Text="0000"></asp:Label>
        <asp:GridView ID="GridViewObjeto" runat="server" OnPageIndexChanging="GridViewObjeto_PageIndexChanging"
            AutoGenerateColumns="False" OnSelectedIndexChanged="GridViewObjeto_SelectedIndexChanged"
            OnSelectedIndexChanging="GridViewObjeto_SelectedIndexChanging" Width="90%" 
            OnRowDataBound="GridViewObjeto_RowDataBound" 
            onrowcommand="GridViewObjeto_RowCommand">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="Id" DataFormatString="{0:0000000}">
                    <ControlStyle Width="50px" />
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="clid" HeaderText="Telefone">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="calldate" HeaderText="Início">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="end" HeaderText="Fim">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="dstchannel" HeaderText="Ramal / Operador">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="channel" HeaderText="Canal ">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="duration" HeaderText="Duração (seg)" DataFormatString="{0:0000}">
                    <ControlStyle Width="50px" />
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="billsec" HeaderText="Bilhetado  (seg)" DataFormatString="{0:0000}">
                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="idcampanha" HeaderText="Campanha">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="pathgravacao" HeaderText="Arquivo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:CommandField HeaderText="Baixar" SelectText="Baixar" ShowSelectButton="True" ButtonType="Image"
                    SelectImageUrl="~/Imagens/Ico/control_play.png">
                    <HeaderStyle HorizontalAlign="Center" Width="16px" />
                    <ItemStyle HorizontalAlign="Center" Width="15px" Height="15px" />
                </asp:CommandField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
