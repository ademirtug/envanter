using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace System
{
	public class sql
	{
		public static Dictionary<Guid, SqlDataAdapter> adapters = new Dictionary<Guid, SqlDataAdapter>();
		public static DataTable query(string cmdText, bool requires_update = false)
		{
			DataTable resultset = new DataTable();
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["envanter"].ConnectionString);

			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			adapter.Fill(resultset);

			if (requires_update)
			{
				SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
				Guid key = Guid.NewGuid();
				adapters[key] = adapter;
				resultset.ExtendedProperties["key"] = key;
			}

			cnn.Close();

			return resultset;
		}
		public static DataTable query_schema(string cmdText, bool requires_update = false)
		{
			DataTable resultset = new DataTable();
			
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["envanter"].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			adapter.FillSchema(resultset, SchemaType.Source);

			if (requires_update)
			{
				SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
				Guid key = Guid.NewGuid();
				adapters[key] = adapter;
				resultset.ExtendedProperties["key"] = key;
			}

			cnn.Close();
			
			return resultset;
		}
		public static int update(DataTable table)
		{
			SqlDataAdapter adapter = adapters[(Guid)table.ExtendedProperties["key"]];
			var x  = adapter.SelectCommand.Connection;


			int aff = -1;
			try
			{
				aff = adapter.Update(table);
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				adapters.Remove((Guid)table.ExtendedProperties["key"]);
			}
			
			return aff;
		}
		public static int execute(string cmdText)
		{
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["envanter"].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			int ret = cmd.ExecuteNonQuery();

			cnn.Close();
			return ret;
		}

		public static object execute_s(string cmdText)
		{
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["envanter"].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			object ret = cmd.ExecuteScalar();

			cnn.Close();
			return ret;
		}


///---------------------------


		public static DataTable queryx(string cmdText, string db, bool requires_update = false)
		{
			DataTable resultset = new DataTable();
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString);

			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			adapter.Fill(resultset);

			if (requires_update)
			{
				SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
				Guid key = Guid.NewGuid();
				adapters[key] = adapter;
				resultset.ExtendedProperties["key"] = key;
			}

			cnn.Close();

			return resultset;
		}
		public static DataTable query_schemax(string cmdText, string db, bool requires_update = false)
		{
			DataTable resultset = new DataTable();

			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			adapter.FillSchema(resultset, SchemaType.Source);

			if (requires_update)
			{
				SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
				Guid key = Guid.NewGuid();
				adapters[key] = adapter;
				resultset.ExtendedProperties["key"] = key;
			}

			cnn.Close();

			return resultset;
		}

		public static int executex(string cmdText, string db)
		{
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			int ret = cmd.ExecuteNonQuery();

			cnn.Close();
			return ret;
		}

		public static float execute_sx(string cmdText, string db)
		{
			SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString);
			cnn.Open();
			SqlCommand cmd = new SqlCommand(cmdText, cnn);
			float ret = (float)cmd.ExecuteScalar();

			cnn.Close();
			return ret;
		}



	}
}