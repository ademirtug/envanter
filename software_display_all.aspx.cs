using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.SqlServer.Types;

namespace envanter
{
	public partial class software_display_all : System.Web.UI.Page
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
						del_sf_record();
					}
					break;
				case "add":
					{
						add_sf_record();
					}
					break;
				case "edit":
					{
						edit_sf_record();
					}
					break;
				default:
					break;
			}
		}
		private void edit_sf_record()
		{
			string id = Request.Form["id"];
			DataTable softwares = sql.query("SELECT * FROM softwares where id=" + id, true);

			if (softwares.Rows.Count < 1)
				return;


			softwares.Rows[0]["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
			softwares.Rows[0]["brand"] = Request.Form["brand"];
			softwares.Rows[0]["product_name"] = Request.Form["product_name"];
			softwares.Rows[0]["version"] = Request.Form["version"];
			softwares.Rows[0]["have_license"] = Request.Form["have_license"] == "1" ? "1" : "0";
			softwares.Rows[0]["amount"] = Request.Form["amount"];
			sql.update(softwares);
		}

		private void add_sf_record()
		{
			DataTable softwares = sql.query_schema("SELECT * FROM softwares", true);
			DataRow r = softwares.NewRow();

			r["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
			r["brand"] = Request.Form["brand"];
			r["product_name"] = Request.Form["product_name"];
			r["version"] = Request.Form["version"];
			r["have_license"] = Request.Form["have_license"] == "1" ? "1" : "0";
			r["amount"] = Convert.ToInt32(Request.Form["amount"]);

			softwares.Rows.Add(r);
			sql.update(softwares);
		}

		private void del_sf_record()
		{
			string[] idarray = Request.Form["id"].ToString().Split(',');

			foreach (string id in idarray)
				sql.execute("delete from softwares where id=" + id);
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