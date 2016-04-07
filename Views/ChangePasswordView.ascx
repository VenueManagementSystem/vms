<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordView.ascx.cs" Inherits="VMS.Views.ChangePasswordView" %>
<form id="form1" runat="server" data-toggle="validator">
    <div class="row">
        <div class="form-group col-xs-3">
            <label for="txtOldPwd">Old Password:</label>
            <asp:TextBox ID="txtOldPwd" runat="server" class="form-control" placeholder="Enter old password"
                data-error="Old password is required" required></asp:TextBox>
            <div class="help-block with-errors"></div>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-3">
            <label for="txtNewPwd">New Password:</label>
            <asp:TextBox ID="txtNewPwd" TextMode="Password" runat="server" class="form-control" placeholder="Enter new password"
                data-minlength="8" required></asp:TextBox>
            <div class="help-block">Length of new password should be minimum of 8 characters</div>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-3">
            <label for="txtCofirmPwd">Confirm Password:</label>
            <asp:TextBox ID="txtCofirmPwd" TextMode="Password" runat="server" class="form-control" 
                data-match="<%#txtNewPwd.ClientID %>" placeholder="Confirm new password" data-match-error="Whoops, new password don't match" required></asp:TextBox>
            <div class="help-block with-errors"></div>
        </div>
    </div>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-default" OnClick="btnSubmit_Click" />
</form>


