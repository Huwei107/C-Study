<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true"
    CodeBehind="UpLoadImage.aspx.cs" Inherits="StudentManagerPro.Students.UpLoadImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/AddStudent.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //检查用户是否选择照片
        function CheckChoseImage() {
            var fulImage = document.getElementById("<%=fulStuImage.ClientID%>");
            if (fulImage.value == "") {
                alert("请选择照片！");
                return false;
            } else {
                return true;
            }
        }
        //检查上传图片格式是否符合要求
        function CheckImage(fileUpLoad) {
            var fulvalue = fileUpLoad.value;
            fulvalue = fulvalue.toLowerCase().substr(fulvalue.lastIndexOf("."));
            if (fulvalue != ".jpg") {
                fileUpLoad.value = "";
                alert("上传照片仅支持jpg格式！");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="StuInfoTable">
        <caption>
            第二步：上传学员照片</caption>     
        <tr>
            <td colspan="2">
                <asp:FileUpload ID="fulStuImage" runat="server" onchange="CheckImage(this)" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnUpLoadImage"
                    runat="server" OnClientClick="return CheckChoseImage()"
                    Text="上传照片" />
            </td>
        </tr>
    </table>
    <asp:Literal ID="ltaMsg" runat="server"></asp:Literal>
</asp:Content>
