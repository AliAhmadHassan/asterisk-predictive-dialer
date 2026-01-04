<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbrePopUp.aspx.cs" Inherits="Silver.UI.Web.Presentation.Pages.AbrePopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        function abrir_popup(url, title, w, h) {
            var _url = 'http://sql/Logados/Acionamento/Atendimento/Detalhes_do_Devedor.aspx?CPFCNPJ=' + url
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            return window.open(_url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }

        function mensagem_teste(cpf) {
            alert('Atendo o cliente de cpf ' + cpf);
        }
    </script>
  </head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManagerRequest" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="TimerRequest" Interval="5000" runat="server" 
            OnTick="TimerRequest_Tick" Enabled="False">
        </asp:Timer>
    </div>
    </form>
</body>
</html>
