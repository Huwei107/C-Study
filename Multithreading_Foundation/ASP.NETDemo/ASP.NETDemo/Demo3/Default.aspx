<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP.NET_Demo2.Demo3.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal ID="lta" runat="server">您是本网站第0位访客！</asp:Literal>
            <br />
            <br />
            <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click"
                Text="清除当前用户的Session信息" />
        </div>
    </form>
</body>
</html>
