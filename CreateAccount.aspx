<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="VMS.CreateAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" data-toggle="validator" runat="server">
        
        <script src="Scripts/validator.js"></script>
        <div class="container">
            <div class="row">
                <div class="col-md-5 col-md-offset-6" style="top: 60px">
                    <h1>
                        <p class="glyphicon glyphicon-hand-down"></p>
                    </h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5 col-md-offset-4" style="top: 70px">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Create Account</h3>
                        </div>
                        <div class="panel-body">
                            <div class="alert <%# _errMsg.PageMessageType %>">
                                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                                <%# _errMsg.PageMessage %>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtUserId" runat="server" placeholder="Pick User Id" data-error="Please enter user id" class="form-control" required></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Email Id" TextMode="Email" data-error="Please enter email" class="form-control" required></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPassword" runat="server" placeholder="Choose Password" TextMode="Password" data-error="Please enter password" class="form-control" required></asp:TextBox>
                                <div class="help-block with-errors"></div>
                            </div>

                            <div class="form-group">
                                <asp:TextBox ID="txtCaptcha" runat="server" placeholder="Enter captcha" data-error="Please enter captcha" class="form-control" required></asp:TextBox>
                                <br />
                                <asp:Image ID="imgCaptcha" runat="server" Width="90" CssClass="lg_refresh" ImageUrl="~/CaptchaImage.aspx" />

                                <a href="#" onclick="javascript:RefreshCaptcha('<%= txtCaptcha.ClientID %>','<%= imgCaptcha.ClientID %>');">
                                    <img id="Img1" runat="server" src="images/refresh.png" class="capt" alt="" />
                                    <%--  --%>
                                </a>
                                <div class="help-block with-errors"></div>
                            </div>

                            <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" class="btn btn-primary btn-block" OnClick="btnSignUp_Click" OnClientClick="getHash256()" />

                        </div>
                    </div>
                </div>
            </div>
            <div class="row"><br /><br /><br /></div>
            <div class="row">
                <div class="col-md-5 col-md-offset-4">
                    <div class="well well-sm">
                        <p>Already have an account? <a href="Login.aspx">Login here.</a></p>
                    </div>
                </div>
            </div>
        </div>
        <script src="Scripts/sha256.js"></script>
        <script src="Scripts/Captcha.js"></script>
        <script type="text/javascript">

            function getHash256() {
                var UserId = document.getElementById('<%=txtUserId.ClientID%>');
                var password = document.getElementById('<%=txtPassword.ClientID%>');
                
                var pwd;
                if (password.value != "") {
                    pwd = CryptoJS.SHA256(UserId.value + password.value);
                    
                    password.value = pwd;
                }

            }
            RefreshCaptcha('<%= txtCaptcha.ClientID %>', '<%= imgCaptcha.ClientID %>');
        </script>
    </form>
</asp:Content>
