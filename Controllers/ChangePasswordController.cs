using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMS.Models;
using VMS.Views;
using VMS;

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
            View.InitializedView(Model);
        }

        public void RequestUpdate(ChangePasswordView view)
        {
            if (Model != null)
            {
                if (UpdateModel(view.OldPassword, view.NewPassword, view.ConfirmNewPassword))
                    this.NotifyView();
            }
        }

        private void NotifyView()
        {
            View.Update(Model);
        }

        private bool UpdateModel(string oldPwd, string newPwd, string confirmNewPwd)
        {
            Model.OldPassword = oldPwd;
            Model.NewPassword = newPwd;
            Model.ConfirmNewPassword = confirmNewPwd;
            string result = DBProcs.ChangePassword(Model);
            if (result == "0") return true;
            else return false;
        }
    }
}