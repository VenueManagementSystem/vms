using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper;
using Config;
using System.Data;

namespace VMS.Module
{
    public partial class GenericView : BlankBasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            ///Debug
            _objSession.QueryString = new Dictionary<string, object>();
            DataSet ds = new DataSet();
            if (!_objSession.QueryString.ContainsKey("PageId"))
            {
                if (Request.QueryString["id"] != null)
                    _objSession.QueryString.Add("PageId", Request.QueryString["id"]);
                else
                {
                    _errMsg.PageMessage = "Invalid page access.";
                    _errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Error];
                    return;
                }
            }
            if (!File.Exists(Server.MapPath(ResolveUrl("~/Module/Content/Metadata/" + _objSession.QueryString["PageId"] + ".xml"))))
            {
                ds = DBProcs.GetGenericViewData(_objSession, _errMsg);
                ds.WriteXml(Server.MapPath(ResolveUrl("~/Module/Content/Metadata/" + _objSession.QueryString["PageId"] + ".xml")));
            }
            else
            {
                ds.ReadXml(Server.MapPath(ResolveUrl("~/Module/Content/Metadata/" + _objSession.QueryString["PageId"] + ".xml")));
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                _errMsg.PageMessage = "Invalid page access.";
                _errMsg.PageMessageType = Constants.Alerts[Constants.PageMessageType.Error];
            }
            else
            {
                _objSession.GenericData = ds;
                this.MasterPageFile = ds.Tables[0].Rows[0]["R001011"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_objSession.GenericData != null)
            {
                DataTable dt = DBProcs.GetGenericGridData(_objSession.GenericData.Tables[0].Rows[0]["R001003"].ToString(), _objSession, _errMsg);
                gdvGeneric.DataSource = dt;
                gdvGeneric.DataBind();
            }
        }
    }
}