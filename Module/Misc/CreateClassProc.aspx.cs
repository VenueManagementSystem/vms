using Config;
using System;
using System.Data;

namespace VMS.Module.Misc
{
    public partial class CreateClassProc : BlankBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTable.Text.Trim())) throw new ArgumentNullException("Table Name can not be blank.");

            txtTable.Text = txtTable.Text.Trim().ToUpper();

            //Query to get information about Table
            string query =

                "DECLARE @tableName varchar(10)" + System.Environment.NewLine +
                "SET @tableName='" + txtTable.Text + "'" + System.Environment.NewLine +
                "select sc.name Column_Name,st.name Type, sc.max_length Length, sc.precision Prec, sc.scale Scale  " + System.Environment.NewLine +
                "from sys.columns sc inner join sys.types st on sc.user_type_id=st.user_type_id where object_id=OBJECT_ID(@tablename)" + System.Environment.NewLine +
                System.Environment.NewLine +
                "select ISC.COLUMN_NAME as IdentityColumn" + System.Environment.NewLine +
                "from INFORMATION_SCHEMA.COLUMNS ISC" + System.Environment.NewLine +
                "where TABLE_NAME=@tablename" + System.Environment.NewLine +
                "and (COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1)" + System.Environment.NewLine +
                System.Environment.NewLine +
                "SELECT K.COLUMN_NAME, c.CONSTRAINT_TYPE" + System.Environment.NewLine +
                "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C" + System.Environment.NewLine +
                "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K" + System.Environment.NewLine +
                "ON C.TABLE_NAME = K.TABLE_NAME" + System.Environment.NewLine +
                "AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG" + System.Environment.NewLine +
                "AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA" + System.Environment.NewLine +
                "AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME" + System.Environment.NewLine +
                "WHERE c.TABLE_NAME=@tablename AND c.CONSTRAINT_TYPE='PRIMARY KEY'" + System.Environment.NewLine;

            DataSet TableDetail = DBProcs.CreateProcClass(_objSession, _errMsg, query);

            _objSession.Dataset = TableDetail;
            _objSession.SiteMap = "VMS" + "~" + txtTable.Text;

            string script = "<script> var win=window.open('', 'Download', 'target=_blank,resizable=yes,scrollbars=yes, Height=400, Width=600'); win.focus();</script>";
            Config.Utility.OpenPopUp(this, "DownLoadClassProc.aspx?type=Pc", "Download", "_blank", true, true, "400", "300");
        }
    }
}