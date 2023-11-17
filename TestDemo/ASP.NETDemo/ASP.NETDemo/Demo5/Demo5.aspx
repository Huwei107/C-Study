<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo05.aspx.cs" Inherits="WebApp.Demo5" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="普通按钮" OnClick="Button1_Click" />
        <br />
        <br />
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">这个是超链接按钮</asp:LinkButton>
        <br />
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/btn.png" OnClick="ImageButton1_Click" />
    </div>
    </form>
</body>
</html>
