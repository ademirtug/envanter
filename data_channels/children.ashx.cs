using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace envanterv2.data_channels
{
	/// <summary>
	/// Summary description for children
	/// </summary>
	public class children : IHttpHandler, IRequiresSessionState
	{

		public void ProcessRequest(HttpContext context)
		{

			if (!context.User.Identity.IsAuthenticated)
				return;


			string cmd = "SELECT div.hid FROM " +
			"aspnet_Users au INNER JOIN aspnet_Membership am " +
			"ON au.UserId = am.UserId " +
			"INNER JOIN Divisions div " +
			"ON am.Division = div.id " +
			"WHERE au.UserName='" + context.User.Identity.Name + "' ORDER BY div.hid.ToString()";

			string hid = sql.execute_s(cmd).ToString();


			cmd = "SELECT DISTINCT div.hid, REPLICATE('&nbsp;&nbsp;', div.hid.GetLevel() - 1 ) + div.name AS name " +
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
			{
				children[dtchildren.Rows[i]["hid"].ToString()] = dtchildren.Rows[i]["name"].ToString();
			}


			string ret = "<select>";
			for (int i = 0; i < children.Count; i++)
			{
				ret += "<option value='" + children.ElementAt(i).Key + "'>" + (children.ElementAt(i).Value.Replace("'", "\\'")) + "</option>";
			}
			ret += "</select>";

			context.Response.Write(ret);

		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}