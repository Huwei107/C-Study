<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP.NET_Demo2.Demo5.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            请输入用户名：<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
            &nbsp;<asp:Button ID="btnSaveToCookie" runat="server" OnClick="btnSaveToCookie_Click"
                Text="将用户名保存到Cookie" />
        </div>
    </form>
</body>
</html>
