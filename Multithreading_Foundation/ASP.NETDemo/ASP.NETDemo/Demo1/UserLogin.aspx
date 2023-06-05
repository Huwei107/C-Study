<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="ASP.NET_Demo2.Demo04.UserLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
            <tr>
                <th colspan="2">
                    用户登录
                </th>
            </tr>
            <tr>
                <td>
                    用户名：
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    密&nbsp; 码：
                </td>
                <td>
                    <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Literal ID="lInfo" runat="server"></asp:Literal>
                    &nbsp;<asp:Button ID="btnLogin" runat="server" Text="登录" OnClick="btnLogin_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
