using Config;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace VMS.Module.Misc
{
    public partial class DownloadClassProc : BlankBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _quaryStringValue = Request.QueryString["type"] as string;
            if (string.IsNullOrEmpty(_quaryStringValue)) close();

            if (_quaryStringValue == "Pc")
            {
                string database = _objSession.SiteMap.Split('~')[0];
                string table = _objSession.SiteMap.Split('~')[1];
                string Proc = string.Empty;
                string Class = string.Empty;
                createProcedure(_objSession.Dataset, database, table, ref Proc, ref Class);
                downLoad(table + ".txt", Proc + Environment.NewLine + Environment.NewLine + Class);

            }
            else if (_quaryStringValue == "Design")
            {
                //_objSession.SiteMap = txtPageName.Text + "~" + txtPageTitle.Text + "~" + txtMasterPage.Text + "~" + txtTheme.Text + "~" + txtBasePage.Text;
                string _pagename = _objSession.SiteMap.Split('~')[0];
                string _pagetitle = _objSession.SiteMap.Split('~')[1];
                string _masterpage = _objSession.SiteMap.Split('~')[2];
                string _theme = _objSession.SiteMap.Split('~')[3];
                string basePage = _objSession.SiteMap.Split('~')[4];
                string design = createDesignPage(_pagename, _pagetitle, _masterpage, _theme);
                downLoad(_pagename + ".aspx", design);

            }
            else if (_quaryStringValue == "Code")
            {
                string _pagename = _objSession.SiteMap.Split('~')[0];
                string _pagetitle = _objSession.SiteMap.Split('~')[1];
                string _masterpage = _objSession.SiteMap.Split('~')[2];
                string _theme = _objSession.SiteMap.Split('~')[3];
                string basePage = _objSession.SiteMap.Split('~')[4];
                string code = createCodePage(_pagename, _pagetitle, basePage);
                downLoad(_pagename + ".aspx.cs", code);
            }
        }

        private void createProcedure(DataSet _ds, string database, string table, ref string Proc, ref string Class)
        {

            DataTable Columns = _ds.Tables[0];

            string idenityCol = string.Empty;
            string timeStamp = string.Empty;
            string primaryKey = string.Empty;


            if (_ds.Tables[1].Rows.Count > 0) idenityCol = _ds.Tables[1].Rows[0][0].ToString();
            if (_ds.Tables[2].Rows.Count > 0) primaryKey = _ds.Tables[2].Rows[0][0].ToString();
            timeStamp = Columns.AsEnumerable().FirstOrDefault(p => p["type"].ToString().ToLower() == "timestamp")["column_name"].ToString();



            StringBuilder ProcParameter = new StringBuilder();
            StringBuilder ProcInsertColumns = new StringBuilder();
            StringBuilder ProcInsertParameters = new StringBuilder();
            StringBuilder ProcUpdate = new StringBuilder();
            StringBuilder ProcSelect = new StringBuilder();
            StringBuilder ClassFields = new StringBuilder();
            StringBuilder ClassProperties = new StringBuilder();


            ProcParameter.Append("CREATE proc [usp" + table + "]");
            ProcParameter.Append(Environment.NewLine);
            ProcParameter.Append("@uType varchar(2)");

            int counter = 1;

            foreach (DataRow _dr in Columns.Rows)
            {
                #region Proc Parameters
                //@Log5002 Varchar(100)=null,
                ProcParameter.Append(", " + Environment.NewLine);
                ProcParameter.Append("@" + _dr["column_name"] + " ");

                switch (_dr["type"].ToString().ToLower())
                {
                    case "varchar":
                    case "char":
                        ProcParameter.Append(_dr["type"].ToString() + " (" + (_dr["length"].ToString() == "-1" ? "max" : _dr["length"].ToString()) + ")");
                        break;
                    case "decimal":
                    case "numeric":
                        ProcParameter.Append(_dr["type"].ToString() + " (" + (_dr["prec"].ToString() + ", " + _dr["scale"].ToString()) + ")");
                        break;
                    default:
                        ProcParameter.Append(_dr["type"].ToString());
                        break;
                }
                ProcParameter.Append(" = Null");
                #endregion

                #region Proc Insert

                if (_dr["column_name"].ToString().EndsWith("DF7"))
                {
                    ProcInsertColumns.Append(", " + (counter % 6 == 0 ? Environment.NewLine : string.Empty) + _dr["column_name"].ToString());
                    ProcInsertParameters.Append(", 1 ");
                }
                else if (!(_dr["column_name"].ToString() == timeStamp || _dr["column_name"].ToString() == idenityCol))
                {
                    ProcInsertColumns.Append(", " + (counter % 6 == 0 ? Environment.NewLine : string.Empty) + _dr["column_name"].ToString());
                    ProcInsertParameters.Append(", " + (counter % 6 == 0 ? Environment.NewLine : string.Empty) + "@" + _dr["column_name"].ToString());
                }

                #endregion

                #region Proc Update

                if (!(_dr["column_name"].ToString() == timeStamp || _dr["column_name"].ToString() == idenityCol || _dr["column_name"].ToString() == primaryKey || _dr["column_name"].ToString().EndsWith("DF3") || _dr["column_name"].ToString().ToLower().EndsWith("DF6")))
                {
                    ProcUpdate.Append(", " + (counter % 6 == 0 ? Environment.NewLine : string.Empty) + _dr["column_name"].ToString() + " = @" + _dr["column_name"].ToString());
                }
                #endregion

                #region Select
                ProcSelect.Append(", " + (counter % 6 == 0 ? Environment.NewLine : string.Empty) + _dr["column_name"].ToString());
                #endregion

                #region Class

                //private string Log1001 = null;


                //if (_dr["column_name"].ToString().ToLower() != table + "DF6")
                //{
                ClassFields.Append(Environment.NewLine + "private ");
                ClassProperties.Append(Environment.NewLine + "public ");
                switch (_dr["type"].ToString().ToLower())
                {
                    case "varchar":
                    case "char":
                    case "uniqueidentifier":
                        ClassFields.Append(" string ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + "; }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value;   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "decimal":
                    case "numeric":
                        ClassFields.Append(" decimal? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToDecimal();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "int":
                        ClassFields.Append(" Int32? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToInt32();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "smallint":
                        ClassFields.Append(" Int16? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToInt16();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "bigint":
                        ClassFields.Append(" Int64? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToInt64();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "smalldatetime":
                        ClassFields.Append(" DateTime? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToDateTime();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "datetime":
                        ClassFields.Append(" DateTime? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToStringLongDateTime(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToLongDateTime();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "timestamp":
                        ClassFields.Append(" Int64? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToInt64();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "tinyint":
                        ClassFields.Append(" byte? ");
                        ClassProperties.Append("string _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + ".ToVObject().ToString(); }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value.ToVObject().ToByte();   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                    case "bit":
                        ClassFields.Append(" bool? ");
                        ClassProperties.Append("bool? _" + _dr["column_name"].ToString() + Environment.NewLine);
                        ClassProperties.Append("{" + Environment.NewLine);
                        ClassProperties.Append("get { return " + _dr["column_name"].ToString() + "; }" + Environment.NewLine);
                        ClassProperties.Append("set { " + _dr["column_name"].ToString() + "= value;   }" + Environment.NewLine);
                        ClassProperties.Append("}" + Environment.NewLine);
                        break;
                }
                ClassFields.Append(_dr["column_name"].ToString() + " = null; ");

                //}
                #endregion

                counter++;
            }

            Proc = ProcParameter.ToString() + Environment.NewLine +
                " AS " + Environment.NewLine +
                " BEGIN " + Environment.NewLine +
                " select @" + table + "DF3=GetDate()" + Environment.NewLine +
                " select @" + table + "DF8=GetDate()" + Environment.NewLine +
                " select @" + table + "DF6=newid()" + Environment.NewLine +
                " IF @uType = 'I' " + Environment.NewLine +
                " BEGIN" + Environment.NewLine +
                " Select @" + primaryKey + " = NEXT VALUE FOR " + table + "_Seq" + Environment.NewLine +
                " Insert Into " + table + Environment.NewLine +
                " (" + ProcInsertColumns.ToString().Substring(2) + ")" + Environment.NewLine +
                " select " +
                ProcInsertParameters.ToString().Substring(2) + Environment.NewLine +
                " select @" + primaryKey + " as " + primaryKey + Environment.NewLine +
                " END" + Environment.NewLine +
                Environment.NewLine +
                " IF @uType='S'" + Environment.NewLine +
                " select " +
                ProcSelect.ToString().Substring(2) + Environment.NewLine +
                " From " + table +
                " Where " + primaryKey + " = @" + primaryKey + " and " + table + "DF7 =1" + Environment.NewLine +
                " IF @uType='U'" + Environment.NewLine +
                " update " + table + " Set" + Environment.NewLine +
                ProcUpdate.ToString().Substring(2) + Environment.NewLine +
                " Where " + primaryKey + " = @" + primaryKey + " and " + timeStamp + " = @" + timeStamp + " and " + table + "DF7 = 1" + Environment.NewLine +
                " If @uType='R'" + Environment.NewLine +
                " Delete " + table + " Where " + primaryKey + " = @" + primaryKey + " and " + timeStamp + " = @" + timeStamp + Environment.NewLine +
                " If @uType='D'" + Environment.NewLine +
                " update " + table + " set " + table + "DF7=0," + table + "DF4= @" + table + "DF4," + table + "DF8=@" + table + "DF8," + table + "DF9=@" + table + "DF9 Where " + primaryKey + " = @" + primaryKey + " and " + timeStamp + " = @" + timeStamp + Environment.NewLine +
                " End";


            Class = Environment.NewLine +
               "using System;" + Environment.NewLine +
               "using Helper;" + Environment.NewLine +
               "using Config;" + Environment.NewLine +
               "using System.Data.SqlClient;" + Environment.NewLine +
               "using System.Data;" + Environment.NewLine + Environment.NewLine +
                "namespace " + database + Environment.NewLine +
               "{" + Environment.NewLine +
               "public class " + table + Environment.NewLine + Environment.NewLine +
               "{" + Environment.NewLine + Environment.NewLine +
               "#region Private Members" + Environment.NewLine + Environment.NewLine +
               ClassFields.ToString().Substring(2) + Environment.NewLine +
               "#endregion" + Environment.NewLine + Environment.NewLine +
               "#region Public Properties" + Environment.NewLine + Environment.NewLine +
               ClassProperties.ToString().Substring(2) +
               "#endregion" + Environment.NewLine + Environment.NewLine +

               "#region Default Constructor" + Environment.NewLine +
               "public " + table + "(UserIdentity _objSession)" + Environment.NewLine +
               "{" + Environment.NewLine +
               table + "DF1 = _objSession.UserID;" + Environment.NewLine +
               table + "DF2 = _objSession.AccountID;" + Environment.NewLine +
               table + "DF4 = _objSession.loginTrackID;" + Environment.NewLine +
               table + "DF9 = _objSession.UserID;" + Environment.NewLine +
               "}" + Environment.NewLine +
               "#endregion" + Environment.NewLine +

               "#region Get Records By Primary Key" + Environment.NewLine +
               "public void fn_getRecordPr(UserIdentity _objSession, Error _errMsg)" + Environment.NewLine +
               "{" + Environment.NewLine +
               "DataTable _result = new DataTable();" + Environment.NewLine +
               "_result = SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, \"usp" + table + "\"," + Environment.NewLine +
               "new SqlParameter(\"@uType\", \"S\")," + Environment.NewLine +
               "new SqlParameter(\"@" + primaryKey + "\", " + primaryKey + ")).Tables[0];" + Environment.NewLine +
               "if (_result != null && _result.Rows.Count > 0)" + Environment.NewLine +
               "{" + Environment.NewLine +
               "fn_assignVariables(_errMsg, _result);" + Environment.NewLine +
               "}" + Environment.NewLine +
               "}" + Environment.NewLine +
               "#endregion" + Environment.NewLine +

               "#region Add Records" + Environment.NewLine +
               "public void fn_addRecord(UserIdentity _objSession, Error _errMsg)" + Environment.NewLine +
               "{" + Environment.NewLine +
               "DataTable _result = new DataTable();" + Environment.NewLine +
               "_result = SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, \"usp" + table + "\"," + Environment.NewLine +
               "new SqlParameter(\"@uType\", \"I\")," + Environment.NewLine;
            int i = 0;
            foreach (DataRow _dr in Columns.Rows)
            {
                i++;
                if (!(_dr["column_name"].ToString() == timeStamp || _dr["column_name"].ToString() == idenityCol || _dr["column_name"].ToString().EndsWith("DF6")))
                    Class += "new SqlParameter(\"@" + _dr["column_name"].ToString() + "\", " + _dr["column_name"].ToString() + ")";
                if (i == Columns.Rows.Count)
                    Class += ").Tables[0];" + Environment.NewLine;
                else if (!(_dr["column_name"].ToString() == timeStamp || _dr["column_name"].ToString() == idenityCol || _dr["column_name"].ToString().EndsWith("DF6")))
                    Class += "," + Environment.NewLine;
            }

            Class += "" + primaryKey + "=Convert.ToInt64(_result.Rows[0][\"" + primaryKey + "\"]);" + Environment.NewLine +
           "}" + Environment.NewLine +
           "#endregion" + Environment.NewLine +

           "#region Update Records" + Environment.NewLine +
               "public void fn_updateRecord(UserIdentity _objSession, Error _errMsg)" + Environment.NewLine +
               "{" + Environment.NewLine +
               "SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, \"usp" + table + "\"," + Environment.NewLine +
               "new SqlParameter(\"@uType\", \"U\")," + Environment.NewLine;
            i = 0;
            foreach (DataRow _dr in Columns.Rows)
            {
                i++;
                if (!(_dr["column_name"].ToString() == idenityCol) || _dr["column_name"].ToString().EndsWith("DF6"))
                    Class += "new SqlParameter(\"@" + _dr["column_name"].ToString() + "\", " + _dr["column_name"].ToString() + ")";
                if (i == Columns.Rows.Count)
                    Class += ");" + Environment.NewLine;
                else
                    Class += "," + Environment.NewLine;
            }


            Class += "}" + Environment.NewLine +
           "#endregion" + Environment.NewLine +

           "#region Deactivate Records" + Environment.NewLine +
               "public void fn_deactivateRecord(UserIdentity _objSession, Error _errMsg)" + Environment.NewLine +
               "{" + Environment.NewLine +

               "SqlHelper.ExecuteDataset(Constants.conLog, CommandType.StoredProcedure, \"usp" + table + "\"," + Environment.NewLine +
               "new SqlParameter(\"@uType\", \"D\")," + Environment.NewLine +

            "new SqlParameter(\"@" + primaryKey + "\", " + primaryKey + ")," + Environment.NewLine +
            "new SqlParameter(\"@" + timeStamp + "\", " + timeStamp + ")," + Environment.NewLine +
            "new SqlParameter(\"@" + table + "DF9\", " + table + "DF9));" + Environment.NewLine +
           "}" + Environment.NewLine +
           "#endregion" + Environment.NewLine +

           "#region Assign Variables" + Environment.NewLine +
               "public void fn_assignVariables(Error _errMsg, DataTable _dt)" + Environment.NewLine +
               "{" + Environment.NewLine;

            i = 0;
            foreach (DataRow _dr in Columns.Rows)
            {
                i++;
                #region Assign Variable

                if (_dr["column_name"].ToString().ToLower() != timeStamp)
                {
                    switch (_dr["type"].ToString().ToLower())
                    {
                        case "varchar":
                        case "char":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToString(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "decimal":
                        case "numeric":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToDecimal(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "int":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToInt32(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "smallint":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToInt16(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "bigint":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToInt64(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "smalldatetime":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToDateTime(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "datetime":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToDateTime(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "timestamp":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToInt64(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "tinyint":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToByte(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                        case "bit":
                            Class += _dr["column_name"].ToString() + " = ClsConvert.DBToBoolean(_dt.Rows[0][\"" + _dr["column_name"].ToString() + "\"]);" + Environment.NewLine;
                            break;
                    }


                }
                #endregion

            }

            Class += "}" + Environment.NewLine +
           "#endregion" + Environment.NewLine +

           "protected string IdentityCol" + Environment.NewLine + Environment.NewLine +
           "{" + Environment.NewLine +
           "get { return \"" + idenityCol + "\"; }" + Environment.NewLine +
           "}" + Environment.NewLine +
           "protected string TimeStampCol" + Environment.NewLine +
           "{" + Environment.NewLine +
           "get { return \"" + timeStamp + "\"; }" + Environment.NewLine +
           "}" + Environment.NewLine + Environment.NewLine +
           "protected string PrimaryCol" + Environment.NewLine +
           "{" + Environment.NewLine +
           "get { return \"" + primaryKey + "\"; }" + Environment.NewLine +
           "}" + Environment.NewLine + Environment.NewLine +

           "}" + Environment.NewLine +
           "}"
           ;
        }


        private string createDesignPage(string pagename, string pagetitle, string masterpage, string theme)
        {
            return "<%@ Page Title=\"" + pagetitle + "\" Language= \"C#\"  MasterPageFile=\"~/Admin/MasterPages/" + masterpage + ".master\" AutoEventWireup= \"true\" CodeFile=\"" + pagename + ".aspx.cs\" Inherits=\"" + pagename + "\" Theme=\"" + theme + "\" %>" + Environment.NewLine
               + " <asp:Content ID=\"Content1\" ContentPlaceHolderID=\"head\" Runat=\"Server\">" +
               Environment.NewLine +
                   "</asp:Content>" +
                   Environment.NewLine +
                    "<asp:Content ID=\"Content2\" ContentPlaceHolderID=\"ContentPlaceHolder1\" Runat=\"Server\">" +
                Environment.NewLine +
                "</asp:Content>";

        }
        private string createCodePage(string pageName, string pageTitle, string basepage)
        {
            return "using System; " + Environment.NewLine +
                         "using ESPLHelper;" + Environment.NewLine +
                         "using ProjectConfig;" + Environment.NewLine +
                         "public partial class " + pageName + " : " + basepage + " " + Environment.NewLine +
                         "{" + Environment.NewLine +
                            "protected void Page_Load(object sender, EventArgs e) " + Environment.NewLine +
                                  "{" + Environment.NewLine +
                                   Environment.NewLine +
                                  "}" + Environment.NewLine +

                          " protected override string PageTitle" + Environment.NewLine +
                           "{" + Environment.NewLine +
                            "   get { return \"" + pageTitle + "\"; }" + Environment.NewLine +
                           "}" + Environment.NewLine +

                           "protected override void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)" + Environment.NewLine +
                           "{" + Environment.NewLine +

                             "  }" + Environment.NewLine +
                           "}" + Environment.NewLine;

        }

        private void close()
        {
            string close = "<script>window.close();</script>";
            Response.Write(close);
        }

        private void downLoad(string filename, string content)
        {
            try
            {
                this.Response.Clear();
                this.Response.AppendHeader("content-type", "text/plain");
                Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
                Response.Write(content);
                Response.Flush();
                Response.End();
            }
            catch (ThreadAbortException)
            {
                ;
            }
        }
    }
}