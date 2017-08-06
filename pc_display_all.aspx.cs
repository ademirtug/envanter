using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.SqlServer.Types;

namespace envanter
{
	public partial class pc_display_all : System.Web.UI.Page
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
						del_pc_record();
					}
					break;
				case "add":
					{
						add_pc_record();
					}
					break;
				case "edit":
					{
						edit_pc_record();
					}
					break;
				default:
					break;
			}
		}

		public void del_pc_record()
		{
			string[] idarray = Request.Form["id"].ToString().Split(',');

			foreach (string id in idarray)
				sql.execute("delete from pc where id=" + id);
		}


		public void add_pc_record()
		{
			DataTable pcs = sql.query_schema("SELECT * FROM pc", true);

			int count = 0;

			Int32.TryParse(Request.Form["amount"], out count);
			if (count == 0)
				count++;

			for (int i = 0; i < count; i++)
			{
				DataRow r = pcs.NewRow();
				r["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
				r["brand"] = Request.Form["brand"];
				r["model"] = Request.Form["model"];
				r["cpu"] = Request.Form["cpu"];
				r["memory"] = Request.Form["memory"];
				r["hdd"] = Request.Form["hdd"];
				r["video_card"] = Request.Form["video_card"];
				r["mac"] = Request.Form["mac"];
				r["bought_at"] = Request.Form["bought_at"];

				pcs.Rows.Add(r);
			}
			sql.update(pcs);
		}

		public void edit_pc_record()
		{
			string id = Request.Form["id"];

			DataTable pcs = sql.query("SELECT * FROM pc where id=" + id, true);

			if (pcs.Rows.Count < 1)
				return;

			pcs.Rows[0]["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
			pcs.Rows[0]["brand"] = Request.Form["brand"];
			pcs.Rows[0]["model"] = Request.Form["model"];
			pcs.Rows[0]["cpu"] = Request.Form["cpu"];
			pcs.Rows[0]["memory"] = Request.Form["memory"];
			pcs.Rows[0]["hdd"] = Request.Form["hdd"];
			pcs.Rows[0]["video_card"] = Request.Form["video_card"];
			pcs.Rows[0]["mac"] = Request.Form["mac"];
			pcs.Rows[0]["bought_at"] = Request.Form["bought_at"];


			sql.update(pcs);
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
				"WHERE div.hid.IsDescendantOf('" + hid + "')=1 ORDER BY div.hid";

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