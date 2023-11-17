<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demo02_CompareValidator.aspx.cs" Inherits="Demo02_CompareValidator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        用户密码：<asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
     
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPwd" ErrorMessage="请输入密码！" ForeColor="#FF3300"></asp:RequiredFieldValidator>
     
        <br />
        <br />
        密码确认：<asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"></asp:TextBox>
      
      
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConfirmPwd" Display="Dynamic" ErrorMessage="请再次输入密码！" ForeColor="#FF3300"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPwd" ControlToValidate="txtConfirmPwd" Display="Dynamic" ErrorMessage="两次输入密码不正确！" ForeColor="#FF3300"></asp:CompareValidator>
      
      
        <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="提交注册" />
    
    </div>
    </form>
</body>
</html>
