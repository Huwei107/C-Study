<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalDemo.aspx.cs" Inherits="ASP.NETDemo.CalDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtNum1" runat="server"></asp:TextBox>+<asp:TextBox ID="txtNum2" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>
        <asp:Button ID="btnSubmit" runat="server" Text="Button" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
