<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoggedInMaster.Master" AutoEventWireup="true" CodeBehind="CreateClassProc.aspx.cs" Inherits="VMS.Module.Misc.CreateClassProc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" data-toggle="validator">
        <script src="../../Scripts/validator.js"></script>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Create Class Procedure</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <div class="form-group">
                    <asp:TextBox ID="txtTable" runat="server" placeholder="Enter table code" data-error="Please enter table code" class="form-control" required></asp:TextBox>
                    <div class="help-block with-errors"></div>
                </div>
                <asp:Button ID="btnCreate" runat="server" Text="Create" class="btn btn-primary" OnClick="btnCreate_Click" />
            </div>
        </div>
    </form>
</asp:Content>
