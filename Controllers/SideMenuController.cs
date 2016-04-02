using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VMS.Views;

namespace VMS.Models
{
    public class SideMenuController
    {
        private SideMenuModel Model;
        private SideMenuView View;

        public SideMenuController()
        {
        }
        public SideMenuController(SideMenuModel paramModel, SideMenuView paramView)
        {
            Model = paramModel;
            View = paramView;
        }

        public void InitializedComponent(bool isPostBack)
        {
            //if (!isPostBack)
            SetMenuList();
            View.InitializedView(Model);
        }

        private void SetMenuList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<li><a href=../Dashboard.aspx>Dashboard</a></li>");
            sb.Append("<li><a href=#>Level 1<span class='fa arrow'></span></a>");
            sb.Append("<ul class=nav nav-second-level>");
            sb.Append("<li><a href=#>Level 2 a</a></li>");
            sb.Append("<li><a href=#>Level 2 b</a></li>");
            sb.Append("<li><a href=#>Level 2 c<span class='fa arrow'></span></a>");
            sb.Append("<ul class=nav nav-third-level>");
            sb.Append("<li><a href=#>Level 3 a</a></li>");
            sb.Append("</ul>");
            sb.Append("</li>");
            sb.Append("</ul>");
            sb.Append("</li>");

            Model.SideMenuList = sb.ToString();
        }
    }
}