using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using Microsoft.SqlServer.Types;

namespace envanter
{
	public partial class office_display_all : System.Web.UI.Page
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
						del_od_record();
					}
					break;
				case "add":
					{
						add_od_record();
					}
					break;
				case "edit":
					{
						edit_od_record();
					}
					break;
				default:
					break;
			}
		}

		private void edit_od_record()
		{
			string id = Request.Form["id"];
			DataTable devices = sql.query("SELECT * FROM office_devices where id="+ id, true);

			if (devices.Rows.Count < 1)
				return;

			devices.Rows[0]["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
			devices.Rows[0]["brand"] = Request.Form["brand"];//.Text;
			devices.Rows[0]["model"] = Request.Form["model"];//.Text;
			devices.Rows[0]["have_printer"] = Request.Form["have_printer"] == "1" ? "1" : "0";//.On;
			devices.Rows[0]["have_plotter"] = Request.Form["have_plotter"] == "1" ? "1" : "0";//.On;
			devices.Rows[0]["have_scanner"] = Request.Form["have_scanner"] == "1" ? "1" : "0";//.On;
			devices.Rows[0]["have_fax"] = Request.Form["have_fax"] == "1" ? "1" : "0";//.On;
			devices.Rows[0]["color_print"] = Request.Form["color_print"] == "1" ? "1" : "0";
			devices.Rows[0]["bought_at"] = Request.Form["bought_at"];//

			sql.update(devices);
		}

		private void add_od_record()
		{
			DataTable devices = sql.query_schema("SELECT * FROM office_devices", true);

			int count = 0;

			Int32.TryParse(Request.Form["amount"], out count);
			if (count == 0)
				count++;

			for (int i = 0; i < count; i++)
			{
				DataRow r = devices.NewRow();

				r["owner"] = SqlHierarchyId.Parse(Request.Form["owner"]);
				r["brand"] = Request.Form["brand"];//.Text;
				r["model"] = Request.Form["model"];//.Text;
				r["have_printer"] = Request.Form["have_printer"] == "1" ? "1" : "0";//.Checked;
				r["have_plotter"] = Request.Form["have_plotter"] == "1" ? "1" : "0";//.Checked;
				r["have_scanner"] = Request.Form["have_scanner"] == "1" ? "1" : "0";//.Checked;
				r["have_fax"] = Request.Form["have_fax"] == "1" ? "1" : "0";//.Checked;
				r["color_print"] = Request.Form["color_print"] == "1" ? "1" : "0";
				r["bought_at"] = Request.Form["bought_at"];//

				devices.Rows.Add(r);
			}
			sql.update(devices);
		}

		private void del_od_record()
		{
			string[] idarray = Request.Form["id"].ToString().Split(',');

			foreach (string id in idarray)
				sql.execute("delete from office_devices where id=" + id);
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
