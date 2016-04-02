using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VMS.Controllers;
using VMS.Models;

namespace VMS.Views
{
    public partial class ChangePasswordView : System.Web.UI.UserControl
    {
        private ChangePasswordController Controller;
        private ChangePasswordModel Model = new ChangePasswordModel("", "", "");
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = new ChangePasswordController(Model, this);
            Controller.InitializedComponent(IsPostBack);
        }

        public void InitializedView(Models.ChangePasswordModel Model)
        {
            ;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OldPassword = txtOldPwd.Text;
            NewPassword = txtNewPwd.Text;
            ConfirmNewPassword = txtCofirmPwd.Text;
            Controller.RequestUpdate(this);
        }

        public void Update(ChangePasswordModel Model)
        {
            if (!string.IsNullOrEmpty(Model.NewPassword))
                ResetControls();
            //show message
        }

        private void ResetControls()
        {
            txtCofirmPwd.Text = string.Empty;
            txtNewPwd.Text = string.Empty;
            txtOldPwd.Text = string.Empty;
        }
    }
}