<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VMS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login</title>

    <link href="Website/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
    <link href="Website/dist/css/sb-admin-2.css" rel="stylesheet" />
    <link href="Website/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" data-toggle="validator">
        <div class="container">
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="login-panel panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Sign In</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <asp:TextBox ID="txtUserId" runat="server" placeholder="User Id" data-error="Please enter user id" class="form-control" required></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPwd" runat="server" placeholder="Password" data-error="Please enter password" class="form-control" required></asp:TextBox>
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                            <p><br /><a href="ForgotPassword.aspx">Forgot Password?</a></p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="well well-sm">
                        <p>New User? <a href="CreateAccount.aspx">Create an account</a></p>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
