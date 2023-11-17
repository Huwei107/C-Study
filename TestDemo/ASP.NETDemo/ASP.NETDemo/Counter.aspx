<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Counter.aspx.cs" Inherits="ASP.NETDemo.Counter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCount" runat="server" Text="Button" OnClick="btnCount_Click" /><br/>
        您点击了：<asp:Literal ID="ltcount" runat="server"></asp:Literal>次
    </div>
    </form>
</body>
</html>
