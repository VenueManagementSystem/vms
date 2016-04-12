using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Config;
using Helper;

namespace VMS
{
    public partial class CreateAccount : BlankBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _errMsg.PageMessage = "Please fill your details";
            _errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Info];
            if(!IsPostBack)
            {
                //string salt = Config.Login.generateSalt();
                //hdnSalt.Value = salt;
                //Session["salt"] = salt;
            }

        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckData())
                {
                    if(!CheckCaptcha())
                    {
                        _errMsg.setMessage(Constants.PageMessageType.Captcha, _errMsg);
                        return;
                    }
                    
                    DBProcs.CreateAccount(txtUserId.Text, txtEmail.Text, txtPassword.Text, _objSession, _errMsg);
                    Reset();
                }
                else
                    _errMsg.setMessage(Constants.PageMessageType.Validation, _errMsg);
            }
            catch (Exception ex)
            {
                _errMsg.PageMessage = _errMsg.getMessage((_errMsg.SaveError(_objSession, (_errMsg.dsResultSet.Tables.Contains("Error") ? _errMsg.dsResultSet.Tables["Error"] : null), ex) ?? 0L), "0", ref _errMsg.PageMessageType);
            }
        }

        private void Reset()
        {
            txtCaptcha.Text = "";
            txtUserId.Text = "";
            txtEmail.Text = "";
        }

        protected bool CheckCaptcha()
        {
            _errMsg._strFuncName = "CheckCaptcha";
            bool _flag = false;
            
            string aa = Session["CaptchaImage"] == null ? string.Empty : Session["CaptchaImage"].ToString();
            if (aa == txtCaptcha.Text)
                _flag = true;
            else
                txtCaptcha.Text = "";

            if (Session["CaptchaImage"] != null)
                Session.Remove("CaptchaImage");
            return _flag;
        }

        private bool CheckData()
        {
            bool _flag = true;
            if (!Utility.IsValidData(txtEmail, "EM", "70", true)) _flag = false;
            if (!Utility.IsValidData(txtUserId, "AD", "70", true)) _flag = false;
            if (!Utility.IsValidData(txtEmail, "EM", "70", true)) _flag = false;
            return _flag;
        }
    }
}