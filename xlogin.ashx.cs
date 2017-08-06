using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Data;
using System.Web.SessionState;

namespace envanterv2
{
	/// <summary>
	/// Summary description for xlogin
	/// </summary>
	public class xlogin : IHttpHandler, IRequiresSessionState
	{
		public void ProcessRequest(HttpContext context)
		{
			HttpRequest r = context.Request;
			xlogin_response rsp = new xlogin_response();

			if (r["act"] == "logout")
			{
				logout(context);
				return;
			}

			string u = r["username"];
			string p = r["password"];

			if (Membership.ValidateUser(r["username"], r["password"]))
			{
				//success
				rsp.Success = "OK";
				FormsAuthentication.SetAuthCookie(u, true);
				rsp.Command = "document.getElementById('topbnnr').innerHTML = \""+r["username"] + " - <a href='javascript:logout();'>Çıkış</a> ";
				rsp.Command += " | <a href='javascript:open_users();' >Kullanıcılar</a>\"";
			}
			else
			{
				//fail
				rsp.Success = "FAIL";
				rsp.Message = "Bilgilerinizi tekrar kontrol edin";
			}

			string sr = (new JavaScriptSerializer()).Serialize(rsp);
			context.Response.Write(sr);
		}

		private void logout(HttpContext context)
		{
			FormsAuthentication.SignOut();
			xlogin_response rsp = new xlogin_response();
			rsp.Success = "OK";
			rsp.Command = "document.getElementById('topbnnr').innerHTML = \"<a href='javascript:open_login();'>Giriş</a>\"; open_login();";
			context.Response.Write( (new JavaScriptSerializer()).Serialize(rsp) );
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}

	public class xlogin_response
	{
		public string Success;
		public string Message;
		public string Command; 
	}

}