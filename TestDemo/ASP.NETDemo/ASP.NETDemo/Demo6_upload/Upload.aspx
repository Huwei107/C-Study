<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ASP.NETDemo.Demo6.Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    请选择需要上传的文件：
        <asp:FileUpload ID="ful" runat="server" Width="226px" />     
        &nbsp;&nbsp;&nbsp;     
        <asp:Button ID="btnUpload" runat="server" Text="开始上传" OnClick="btnUpload_Click" />
        <br />
        <br />
        <asp:Literal ID="ltaMsg" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
