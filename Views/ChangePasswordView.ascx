<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordView.ascx.cs" Inherits="VMS.Views.ChangePasswordView" %>
<div class="row">
<div class="form-group col-xs-3">
    <label for="txtOldPwd">Old Password:</label>
    <asp:TextBox ID="txtOldPwd" runat="server" class="form-control" placeholder="Enter old password"></asp:TextBox>
</div>
    </div>
<div class="row">
<div class="form-group col-xs-3">
    <label for="txtNewPwd">New Password:</label>
    <asp:TextBox ID="txtNewPwd" TextMode="Password" runat="server" class="form-control" placeholder="Enter new password"></asp:TextBox>

</div>
</div>
<div class="row">
<div class="form-group col-xs-3">
    <label for="txtCofirmPwd">Confirm Password:</label>
    <asp:TextBox ID="txtCofirmPwd" TextMode="Password" runat="server" class="form-control" placeholder="Confirm new password"></asp:TextBox>

</div>
</div>
<asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-default" OnClick="btnSubmit_Click" />