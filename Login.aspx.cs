using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Config;
using System.Data;
using Helper;

namespace VMS
{
    public partial class Login : BlankBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _errMsg.PageMessage = "Enter credentials";
            _errMsg.PageMessageType = "alert-info";
            if (!IsPostBack)
            {
                string salt = Config.Login.generateSalt();
                hdnSalt.Value = salt;
                Session["salt"] = salt;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckData())
                {
                    if (!CheckCaptcha())
                    {
                        _errMsg.setMessage(Constants.PageMessageType.Captcha, _errMsg);
                        return;
                    }
                    DataSet dtUser = DBProcs.GetUserByLoginId(txtUserId.Text, _objSession, _errMsg);
                    if (dtUser.Tables[0].Rows.Count == 0)
                    {
                        _errMsg.setMessage(Constants.PageMessageType.Warning, "Invalid user.", _errMsg);
                        return;
                    }
                    if (!Security.CheckPassword(txtPwd.Text, dtUser.Tables[0].Rows[0]["T001004"].ToString(), txtUserId.Text, true, (string)Session["salt"]))
                    {
                        _errMsg.setMessage(Constants.PageMessageType.Warning, "Invalid password.", _errMsg);
                        return;
                    }
                    Config.Login.InitializeSession(dtUser, _errMsg);
                    Response.Redirect(ResolveUrl("~/Dashboard.aspx"));
                    //Reset();
                }
                else
                    _errMsg.setMessage(Constants.PageMessageType.Validation, _errMsg);
            }
            catch (Exception ex)
            {
                _errMsg.PageMessage = _errMsg.getMessage((_errMsg.SaveError(_objSession, (_errMsg.dsResultSet.Tables.Contains("Error") ? _errMsg.dsResultSet.Tables["Error"] : null), ex) ?? 0L), "0", ref _errMsg.PageMessageType);
            }

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
            if (!Utility.IsValidData(txtUserId, "AD", "70", true)) _flag = false;
            return _flag;
        }
    }
}