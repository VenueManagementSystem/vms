using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VMS.Models;

namespace VMS.Views
{
    public partial class SideMenuView : System.Web.UI.UserControl
    {
        private SideMenuController Controller;
        private SideMenuModel Model = new SideMenuModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = new SideMenuController(Model, this);
            Controller.InitializedComponent(IsPostBack);
        }

        public void InitializedView(SideMenuModel Model)
        {
            ltrMenuList.Text = Model.SideMenuList;
        }
    }
}