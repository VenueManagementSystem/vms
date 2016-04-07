<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="VMS.CreateAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" data-toggle="validator">
        <div class="container">
            <div class="row">
                <div class="col-md-5 col-md-offset-6" style="top:60px">
                <h1><p class="glyphicon glyphicon-hand-down"></p></h1>
                    </div>
            </div>
            <div class="row">
                <div class="col-md-5 col-md-offset-4" style="top:70px">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">Create Account</h3>
                        </div>
                        <div class="panel-body">
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
                            <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" class="btn btn-primary btn-block" />
                            
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </form>
</asp:Content>
