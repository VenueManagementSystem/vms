<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VMS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login</title>

    <link href="Website/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
    <link href="Website/dist/css/sb-admin-2.css" rel="stylesheet" />
    <link href="Website/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="Css/Captcha.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" data-toggle="validator">
        <asp:HiddenField ID="hdnSalt" runat="server" />
        <div class="container">
            <div class="row">
                <div class="col-md-4 col-md-offset-4">
                    <div class="login-panel panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Sign In</h3>
                        </div>
                        <div class="panel-body">
                            <div class="alert <%# _errMsg.PageMessageType %>">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                <%# _errMsg.PageMessage %>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtUserId" runat="server" placeholder="User Id" data-error="Please enter user id" class="form-control" required></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPwd" TextMode="Password" runat="server" placeholder="Password" data-error="Please enter password" class="form-control" required></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtCaptcha" runat="server" data-error="Please enter captcha" class="form-control" placeholder="Enter captcha" required></asp:TextBox>
                                <p>
                                    <br />
                                    <asp:Image ID="imgCaptcha" runat="server" Width="90" CssClass="lg_refresh" ImageUrl="~/CaptchaImage.aspx" />

                                    <a href="#" onclick="javascript:RefreshCaptcha('<%= txtCaptcha.ClientID %>','<%= imgCaptcha.ClientID %>');">
                                        <img id="Img1" runat="server" src="images/refresh.png" class="capt" alt="" />
                                        <%--  --%>
                                    </a>
                                </p>
                            </div>
                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary btn-block" OnClick="btnLogin_Click" OnClientClick="getHash256()" />
                            <p>
                                <br />
                                <a href="ForgotPassword.aspx">Forgot Password?</a>
                            </p>
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
    <script src="Scripts/Captcha.js"></script>
    <script src="Scripts/sha256.js"></script>
    <script type="text/javascript">
        RefreshCaptcha('<%= txtCaptcha.ClientID %>', '<%= imgCaptcha.ClientID %>');
        function getHash256() {
            var UserId = document.getElementById('<%=txtUserId.ClientID%>');
            var password = document.getElementById('<%=txtPwd.ClientID%>');
            var hdnSalt = document.getElementById('<%=hdnSalt.ClientID%>');
            var pwd;
            if (password.value != "") {
                pwd = CryptoJS.SHA256(UserId.value + password.value);
                pwd = CryptoJS.SHA256(hdnSalt.value + pwd);
                password.value = pwd;
            }

        }
    </script>


</asp:Content>
