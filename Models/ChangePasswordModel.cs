using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class ChangePasswordModel
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
        public String ConfirmNewPassword { get; set; }
        public ChangePasswordModel(string paramOldPwd, string paramNewPwd,string paramConfirmNewPwd)
        {
            OldPassword = paramOldPwd;
            NewPassword = paramNewPwd;
            ConfirmNewPassword = paramConfirmNewPwd;
        }
    }
}