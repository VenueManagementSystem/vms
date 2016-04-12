using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMS.Models;
using VMS.Views;
using VMS;
using System.Data;
using Config;

namespace VMS.Controllers
{
    public class ChangePasswordController
    {
        private ChangePasswordModel Model;
        private ChangePasswordView View;

        public ChangePasswordController(ChangePasswordModel paramModel, ChangePasswordView paramView)
        {
            Model = paramModel;
            View = paramView;
        }

        public void InitializedComponent(bool isPostBack)
        {
            View._errMsg.PageMessage = "For security reasons please change your password after 15 days.";
            View._errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Info];
            View.InitializedView(Model);
        }

        public void RequestUpdate(ChangePasswordView view)
        {
            if (Model != null)
            {
                UpdateModel(view.OldPassword, view.NewPassword, view.ConfirmNewPassword);
                this.NotifyView();
            }
        }

        private void NotifyView()
        {
            View.Update(Model);
        }

        private void UpdateModel(string oldPwd, string newPwd, string confirmNewPwd)
        {
            Model.OldPassword = oldPwd;
            Model.NewPassword = newPwd;
            Model.ConfirmNewPassword = confirmNewPwd;
            if (newPwd != confirmNewPwd)
            {
                Model.AlertMessage = "New password and confirm password doesn't match.";
                Model.AlertType = Config.Constants.PageMessageType.Error;
                return;
            }
            T001 t001 = new T001(View._objSession);
            t001._T001001 = View._objSession.UserID.ToString();
            t001.fn_getRecordPr(View._objSession, View._errMsg);
            if (Model.OldPassword != t001._T001005)
            {
                Model.AlertMessage = "Old password is incorrect. Please try again";
                Model.AlertType = Config.Constants.PageMessageType.Error;
                return;
            }

            DBProcs.ChangePassword(Model, View, t001);
            Model.AlertMessage = "Password changed successfully.";
            Model.AlertType = Config.Constants.PageMessageType.Success;
            
        }
    }
}