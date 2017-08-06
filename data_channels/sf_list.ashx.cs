using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

namespace envanter
{
	/// <summary>
	/// Summary description for sf_list
	/// </summary>
	public class sf_list : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			HttpRequest req = context.Request;


			//string _search = request["_search"];
			//string numberOfRows = request["rows"];
			//string pageIndex = request["page"];
			//string sortColumnName = request["sidx"];
			//string sortOrderBy = request["sord"];

			//{_search=true&nd=1331712660100&rows=10&page=1&sidx=brand&sord=asc&searchField=brand&searchString=pcx&searchOper=eq&filters=}
			string p = req["page"];
			string r = req["rows"];//number of rows requested per page
			string sortColumnName = req["sidx"];
			string sortOrderBy = req["sord"];

			string search = req["search"];
			string searchField = req["searchField"];
			string searchString = req["searchString"];
			string searchOper = req["searchOper"];

			var ser = req.QueryString.Keys;
			int page_num = Convert.ToInt32(req["page"].ToString());
			int norr = Convert.ToInt32(r);


			fr freq = new fr();
			if (req["filters"] != null)
				freq = (new JavaScriptSerializer()).Deserialize<fr>(req["filters"]);


			//{"groupOp":"AND","rules":[{"field":"brand","op":"bw","data":"pc"}]}

			string cmd = "SELECT div.hid FROM " +
			"aspnet_Users au INNER JOIN aspnet_Membership am " +
			"ON au.UserId = am.UserId " +
			"INNER JOIN Divisions div " +
			"ON am.Division = div.id " +
			"WHERE au.UserName='" + context.User.Identity.Name + "' ORDER BY div.hid.ToString()";

			string hid = sql.execute_s(cmd).ToString();


			string where_clause = " WHERE ";
			if (freq.rules != null)
			{
				foreach (_rules crit in freq.rules)
				{
					if (crit.field == "owner")
					{
						hid = crit.data;
						continue;
					}
					where_clause += " " + crit.field + "='" + crit.data + "' AND";
				}
			}
			where_clause += " owner.IsDescendantOf('" + hid + "')=1 ";

			DataTable dt = sql.query("SELECT * FROM softwares " + where_clause + " ORDER BY " + sortColumnName + " " + sortOrderBy);
			JQGridResults jqr = new JQGridResults();

			jqr.page = Convert.ToInt32(req["page"].ToString()) > 0 ? Convert.ToInt32(req["page"].ToString()) : 1;
			jqr.total = ((dt.Rows.Count % norr) > 0 ? (dt.Rows.Count / norr) + 1 : (dt.Rows.Count / norr));
			jqr.records = dt.Rows.Count;

			List<JQGridRow> rows = new List<JQGridRow>();

			for (int i = (page_num - 1) * norr; (i < dt.Rows.Count) && (i < (page_num * norr)); i++)
			{
				JQGridRow row = new JQGridRow();
				row.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
				row.cell = new string[] {
					dt.Rows[i]["owner"].ToString(),
					dt.Rows[i]["brand"].ToString(),
					dt.Rows[i]["product_name"].ToString(),
					dt.Rows[i]["version"].ToString(),
					dt.Rows[i]["have_license"].ToString(),
					dt.Rows[i]["amount"].ToString()
				};
				rows.Add(row);
			}

			jqr.rows = rows.ToArray();
			context.Response.Write((new JavaScriptSerializer()).Serialize(jqr));
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public string oper_conversion(string input)
		{
			string sql_oper = "";
			switch (input)
			{
				case "eq":
					{
						sql_oper = "=";
					}
					break;
				case "ne":
					{
						sql_oper = "<>";
					}
					break;
				case "lt":
					{
						sql_oper = "<";
					}
					break;
				case "le":
					{
						sql_oper = "<=";
					}
					break;
				case "gt":
					{
						sql_oper = ">";
					}
					break;
				case "ge":
					{
						sql_oper = ">=";
					}
					break;
				case "bw":
					{
						sql_oper = "LIKE";
					}
					break;
				case "bn":
					{
						sql_oper = "NOT LIKE";
					}
					break;
				case "in":
					{
						sql_oper = "LIKE";
					}
					break;
				case "ni":
					{
						sql_oper = "NOT LIKE";
					}
					break;
				case "ew":
					{
						sql_oper = "LIKE";
					}
					break;
				case "en":
					{
						sql_oper = "NOT LIKE";
					}
					break;
				case "cn":
					{
						sql_oper = "LIKE";
					}
					break;
				case "nc":
					{
						sql_oper = "NOT LIKE";
					}
					break;
				default:
					{
						sql_oper = "=";
					}
					break;
			}


			return sql_oper;
		}

		public struct JQGridResults
		{
			public int page;
			public int total;
			public int records;
			public JQGridRow[] rows;

		}
		public struct JQGridRow
		{
			public int id;
			public string[] cell;
		}
		class fr
		{
			public string groupOp;
			public _rules[] rules;

		}
		public class _rules
		{
			public string field, op, data;
		}
	}
}