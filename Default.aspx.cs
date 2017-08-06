using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Profile;

namespace envanter
{
	public partial class Default : System.Web.UI.Page
	{
		public string jscmd = "";
		public string usr = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (User.Identity.IsAuthenticated)
			{
				usr = User.Identity.Name + " - <a href='javascript:logout();'>Çıkış</a> ";
				usr += " | <a href='javascript:open_users();' >Kullanıcılar</a> ";
				jscmd = "$('#pc_display_all').load('pc_display_all.aspx');";
			}
			else
			{
				usr = "<a href='javascript:open_login();'>Giriş</a>";
				jscmd += "open_login();";
			}				
		}
	}
}