<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebDemo.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="ltaInfo" runat="server"></asp:Literal><br /><br />
        <asp:Button ID="btnGetCount" runat="server" Text="获取1班和2班的学生总数" OnClick="btnGetCount_Click" />
    </form>
</body>
</html>
