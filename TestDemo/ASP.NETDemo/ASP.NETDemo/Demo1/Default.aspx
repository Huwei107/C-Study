<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP.NETDemo.Demo1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBox ID="ckProduct1" runat="server" Text="衬衣"/><br />
        <asp:CheckBox ID="ckProduct2" runat="server" Text="短裤"/><br />
        <asp:CheckBox ID="ckProduct3" runat="server" Text="西裤"/><br />
        <asp:CheckBox ID="ckProduct4" runat="server" Text="短袖"/><br />
        <asp:CheckBox ID="ckProduct5" runat="server" Text="帽子"/><br />
        <asp:CheckBox ID="ckProduct6" runat="server" Text="背心"/><br />
        <asp:Button ID="btnSubmit" runat="server" Text="将所选商品添加到购物车" OnClick="btnSubmit_Click" /><asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <br />
        <asp:Button ID="btnGWC" runat="server" Text="显示购物车" OnClick="btnGWC_Click" />
        <br />
        <br/>
        <asp:Literal ID="ltaMsg" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
