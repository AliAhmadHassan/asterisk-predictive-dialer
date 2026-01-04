<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true"
    CodeBehind="HistoricoRequisicao.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.HistoricoRequisicao" %>
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
                                &nbsp;</td>
                            <td>
                            </td>
                            <td>
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
    <asp:GridView ID="grid_historico" runat="server" AutoGenerateColumns="False" 
        onpageindexchanging="grid_historico_PageIndexChanging" 
        onrowdatabound="grid_historico_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Evento" HeaderText="Evento" />
            <asp:BoundField DataField="Valor" HeaderText="Parâmetro" />
            <asp:BoundField DataField="Situacao" HeaderText="Status" />
            <asp:BoundField DataField="DtHrExecucao" HeaderText="Hora Execução" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="Porcentagem" HeaderText="Andamento" />
        </Columns>
    </asp:GridView>
</asp:Content>
