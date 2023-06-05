<%@ Page Title="" Language="C#" MasterPageFile="~/MainPage.Master" AutoEventWireup="true"
    CodeBehind="StudentManage.aspx.cs" Inherits="StudentManagerPro.Students.StudentManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Styles/StudentManage.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="QueryDiv">
        请选择要查询的班级：<asp:DropDownList ID="ddlClass" Width="120px" runat="server">
        </asp:DropDownList>
        <asp:Button ID="btnQuery" runat="server" Text="提交查询" />
        <asp:Literal ID="ltaMsg" runat="server"></asp:Literal>
    </div>

    <div class="stuInfo">
        <div class="stuImg">
            <asp:Image ID="imgStu" runat="server" Height="136px" Width="150px" ImageUrl="~/Images/Student/100001.jpg" />
        </div>
        <div class="stuItem">
            <span>姓名：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>性别：待定</span>
        </div>
        <div class="stuItem">
            <span>班级：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>出生日期：待定</span>
        </div>
        <div class="stuItem">
            <span>家庭住址：</span><span style="width: 80px;">待定</span>
        </div>
        <div class="stuItem">
            <asp:HyperLink ID="HyperLink1"
                runat="server" ForeColor="Blue">修改信息</asp:HyperLink>&nbsp;
            <asp:LinkButton ID="btnDel"
                runat="server">删除学员</asp:LinkButton>
        </div>
    </div>
    <div class="stuInfo">
        <div class="stuImg">
            <asp:Image ID="Image1" runat="server" Height="136px" Width="150px" ImageUrl="~/Images/Student/100002.jpg" />
        </div>
        <div class="stuItem">
            <span>姓名：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>性别：待定</span>
        </div>
        <div class="stuItem">
            <span>班级：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>出生日期：待定</span>
        </div>
        <div class="stuItem">
            <span>家庭住址：</span><span style="width: 80px;">待定</span>
        </div>
        <div class="stuItem">
            <asp:HyperLink ID="HyperLink2"
                runat="server" ForeColor="Blue">修改信息</asp:HyperLink>&nbsp;
            <asp:LinkButton ID="LinkButton1"
                runat="server">删除学员</asp:LinkButton>
        </div>
    </div>
        <div class="stuInfo">
        <div class="stuImg">
            <asp:Image ID="Image2" runat="server" Height="136px" Width="150px" ImageUrl="~/Images/Student/100001.jpg" />
        </div>
        <div class="stuItem">
            <span>姓名：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>性别：待定</span>
        </div>
        <div class="stuItem">
            <span>班级：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>出生日期：待定</span>
        </div>
        <div class="stuItem">
            <span>家庭住址：</span><span style="width: 80px;">待定</span>
        </div>
        <div class="stuItem">
            <asp:HyperLink ID="HyperLink3"
                runat="server" ForeColor="Blue">修改信息</asp:HyperLink>&nbsp;
            <asp:LinkButton ID="LinkButton2"
                runat="server">删除学员</asp:LinkButton>
        </div>
    </div>
    <div class="stuInfo">
        <div class="stuImg">
            <asp:Image ID="Image3" runat="server" Height="136px" Width="150px" ImageUrl="~/Images/Student/100002.jpg" />
        </div>
        <div class="stuItem">
            <span>姓名：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>性别：待定</span>
        </div>
        <div class="stuItem">
            <span>班级：</span><span style="width: 80px;">待定</span>&nbsp;&nbsp;<span>出生日期：待定</span>
        </div>
        <div class="stuItem">
            <span>家庭住址：</span><span style="width: 80px;">待定</span>
        </div>
        <div class="stuItem">
            <asp:HyperLink ID="HyperLink4"
                runat="server" ForeColor="Blue">修改信息</asp:HyperLink>&nbsp;
            <asp:LinkButton ID="LinkButton3"
                runat="server">删除学员</asp:LinkButton>
        </div>
    </div>

</asp:Content>
