<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestChecking.aspx.cs" Inherits="Silver.UI.Web.Presentation.Requests.RequestChecking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    </div>
    <p>
        <asp:Timer ID="timer_checking" runat="server" Interval="5000" 
            ontick="timer_checking_Tick">
        </asp:Timer>
    </p>
    <p>
        &nbsp;</p>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <p>
    </p>
    </form>
</body>
</html>
