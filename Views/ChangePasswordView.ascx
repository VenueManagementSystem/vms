<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePasswordView.ascx.cs" Inherits="VMS.Views.ChangePasswordView" %>
<form id="form1" runat="server" data-toggle="validator">
    <asp:HiddenField ID="hdnUserId" runat="server" />
    <script src="../Scripts/validator.js"></script>
    
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
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-default" OnClick="btnSubmit_Click" OnClientClick="getHash256()" />
    <script src="Scripts/sha256.js"></script>
    <script type="text/javascript">

        function getHash256() {
            var txtOld = document.getElementById('<%=txtOldPwd.ClientID%>');
            var txtNew = document.getElementById('<%=txtNewPwd.ClientID%>');
            var txtConfirmNew = document.getElementById('<%=txtCofirmPwd.ClientID%>');
            var oldpwd; var newpwd; var confirmpwd;
            if (password.value != "") {
                oldpwd = CryptoJS.SHA256(hdnUserId.value + txtOld.value);
                newpwd = CryptoJS.SHA256(hdnUserId.value + txtNew.value);
                confirmpwd = CryptoJS.SHA256(hdnUserId.value + txtConfirmNew.value);

                txtOld.value = oldpwd;
                txtNew.value = newpwd;
                txtConfirmNew.value = confirmpwd;
            }

        }

    </script>
</form>



