<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo09_UpLoad.aspx.cs" Inherits="WebApp.Demo09_UpLoad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    请选择要上传的文件：<asp:FileUpload ID="ful" runat="server" />
        &nbsp;&nbsp;
        <asp:Button ID="btnUpload" runat="server" Text="开始上传" OnClick="btnUpload_Click" />
        <br />
        <br />
        <asp:Literal ID="ltaMsg" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
