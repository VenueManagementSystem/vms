<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedInMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="VMS.ChangePassword" %>

<%@ Register Src="Views/ChangePasswordView.ascx" TagPrefix="changepwd" TagName="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Change Password</h1>
        </div>
    </div>
    <changepwd:ChangePassword runat="server" ID="changePwd" />
</asp:Content>
