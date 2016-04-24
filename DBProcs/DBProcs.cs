using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Config;
using Helper;
using VMS.Models;
using System.Data.SqlClient;
using System.Transactions;

namespace VMS
{
    class DBProcs
    {
        //private static string conLog = System.Configuration.ConfigurationManager.AppSettings["conLog"];
        internal static void ChangePassword(ChangePasswordModel Model, ChangePasswordView View, T001 t001)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                T004 t004 = new T004(View._objSession);
                t004._T004002 = t001._T001001;
                t004._T004003 = Model.OldPassword;
                t004._T004004 = Model.OldPassword;
                t004._T004005 = DateTime.Now.ToString();
                t004.fn_addRecord(View._objSession, View._errMsg);

                t001._T001004 = Model.NewPassword;
                t001._T001005 = Model.NewPassword;
                t001._T001DF8 = DateTime.Now.ToString();
                t001._T001DF9 = View._objSession.UserID.ToString();
                t001.fn_updateRecord(View._objSession, View._errMsg);

                ts.Complete();
            }
        }

        internal static DataSet GetGenericViewData(UserIdentity _objSession, Error _errMsg)
        {
            return SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, "GetGenericViewData",
                new SqlParameter("@PageId", _objSession.QueryString["PageId"]));
        }

        internal static DataTable GetGenericGridData(string table, UserIdentity _objSession, Error _errMsg)
        {
            return SqlHelper.ExecuteDataset(Constants.conLog, CommandType.Text, "select * from " + table).Tables[0];
        }

        internal static DataSet CreateProcClass(UserIdentity _objSession, Error _errMsg, string query)
        {
            return SqlHelper.ExecuteDataset(Constants.conLog, CommandType.Text, query);
        }

        internal static void CreateAccount(string loginId, string email, string password, UserIdentity _objSession, Error _errMsg)
        {
            T001 t001 = new T001(_objSession);
            DataSet dt = GetUserByLoginId(loginId, _objSession, _errMsg);

            if (dt.Tables[0].Rows.Count > 0)
            {
                _errMsg.PageMessage = "This login id already exists. Please select different login id.";
                _errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Warning];
                return;
            }
            using (TransactionScope ts = new TransactionScope())
            {
                t001._T001002 = loginId;
                t001._T001003 = Convert.ToString(_objSession.UserID);
                t001._T001004 = password;// Security.getSHA256Hash(loginId + password);
                t001._T001005 = password;// Security.getSHA256Hash(loginId + password);
                t001._T001006 = "0";
                t001.fn_addRecord(_objSession, _errMsg);

                T002 t002 = new T002(_objSession);
                t002._T002004 = email;
                t002._T002006 = t001._T001001;
                t002.fn_addRecord(_objSession, _errMsg);
                //TODO: Send email
                ts.Complete();
            }

            _errMsg.PageMessage = "User created successfully.";
            _errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Success];
        }

        public static DataSet GetUserByLoginId(string loginId, UserIdentity _objSession, Error _errMsg)
        {
            return SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, "uspGetUserByLoginId",
                new SqlParameter("@loginId", loginId));

        }
    }
}
