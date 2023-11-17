<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo4.aspx.cs" Inherits="WebApp.Demo4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            学号：  
            <asp:TextBox ID="TextBox1" runat="server" Style="width: 148px"></asp:TextBox>
            <asp:Button ID="btnDel" OnClientClick="return confirm('确认删除吗？')"
                runat="server" Text="删除" OnClick="btnDel_Click" />
            <asp:Literal ID="ltamsg" runat="server"></asp:Literal>

        </div>
    </form>
</body>
</html>
