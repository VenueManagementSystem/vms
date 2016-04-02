<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedOutMaster.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VMS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login</title>

    <link href="Website/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
    <link href="Website/dist/css/sb-admin-2.css" rel="stylesheet" />
    <link href="Website/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" ng-form="myForm">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Sign In</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <%--<asp:TextBox ID="txtUserId" name="userid" runat="server"></asp:TextBox>--%>
                            <input type="text" class="form-control" placeholder="User Id" name="userid" ng-model="userid" required />
                            <span style="color: red" ng-show="myForm.userid.$dirty && myForm.userid.$invalid">
                                <span ng-show="myForm.userid.$error.required">User Id is required.</span>
                            </span>
                        </div>
                        <div class="form-group">
                            <input class="form-control" placeholder="Password" name="password" type="password" value="" ng-model="password" required />
                            <span style="color: red" ng-show="myForm.password.$dirty && myForm.password.$invalid">
                                <span ng-show="myForm.password.$error.required">Password is required.</span>
                            </span>
                        </div>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary" ng-disabled="myForm.$invalid" OnClick="btnLogin_Click" />
                    
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="Scripts/angular.min.js"></script>
</asp:Content>
