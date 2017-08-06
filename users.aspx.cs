using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.SqlServer.Types;

namespace envanterv2
{
	public partial class users : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!User.Identity.IsAuthenticated)
				return;

			string cmd = Request.Form["oper"] != null ? Request.Form["oper"].ToString() : "";

			switch (cmd)
			{
				case "del":
					{
						del_user_record();
					}
					break;
				case "add":
					{
						add_user_record();
					}
					break;
				case "edit":
					{
						edit_user_record();
					}
					break;
				default:
					break;
			}
		}
		public void del_user_record()
		{
			string[] idarray = Request.Form["id"].ToString().Split(',');

			foreach (string id in idarray)
				Membership.DeleteUser(Membership.GetUser(new Guid(id)).UserName, true);
		}

		public void add_user_record()
		{
			try
			{
				string hid = sql.execute_s("SELECt id FROM divisions WHERE hid='" + Request.Form["division"]+"'").ToString() ;
				string usrname = Request.Form["UserName"];
				string password = Request.Form["Password"];
				string email = Request.Form["Email"];

				MembershipUser user = Membership.CreateUser(usrname, password, email);

				sql.execute("UPDATE aspnet_Membership SET division="+hid+" WHERE UserId='"+ user.ProviderUserKey +"'");

				if (Request.Form["IsLocketOut"] != "1")
				{
		
				}

			}
			catch (Exception ex)
			{

			}
		}

		public void edit_user_record()
		{
			
			MembershipUser user = Membership.GetUser(new Guid(Request.Form["id"]));
			
			if (Request.Form["IsLocketOut"] != "1")
			{
				user.UnlockUser();
			}


			user.ChangePassword(user.GetPassword(), Request.Form["Password"].ToString());
		}

		public string get_ch
		{
			get
			{
				string cmd = "SELECT div.hid FROM " +
				"aspnet_Users au INNER JOIN aspnet_Membership am " +
				"ON au.UserId = am.UserId " +
				"INNER JOIN Divisions div " +
				"ON am.Division = div.id " +
				"WHERE au.UserName='" + User.Identity.Name + "' ORDER BY div.hid.ToString()";

				string hid = sql.execute_s(cmd).ToString();




				cmd = "SELECT DISTINCT div.hid,  REPLICATE('&nbsp;&nbsp;', div.hid.GetLevel() - 1 ) +  div.name AS name " +
				"FROM divisions AS div " +
				"INNER JOIN divisions AS div2 ON div.hid.IsDescendantOf(div2.hid) = 1 " +
				"WHERE div.hid.IsDescendantOf('" + hid + "')=1 ORDER BY div.hid ";

				DataTable dtchildren = sql.query(cmd);
				if (dtchildren.Rows.Count > 0)
				{
					if (dtchildren.Rows[0]["hid"].ToString() == "/")
						dtchildren.Rows[0]["name"] = "Bakanlık";
				}
				Dictionary<string, string> children = new Dictionary<string, string>();

				for (int i = 0; i < dtchildren.Rows.Count; i++)
					children[dtchildren.Rows[i]["hid"].ToString()] = dtchildren.Rows[i]["name"].ToString();

				string ret = "'{";
				for (int i = 0; i < children.Count; i++)
				{
					ret += "\"" + children.ElementAt(i).Key + "\":\"" + (children.ElementAt(i).Value.Replace("'", "\\'")) + "\",";
				}
				ret = ret.Remove(ret.Length - 1);
				ret += "}'";

				return ret;
			}
		}

	}
}