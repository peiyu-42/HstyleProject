using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace H2StyleStore.filter
{
	public class AuthrizeHelperAttribute : AuthorizeAttribute
	{
		private string[] _identity;
		public AuthrizeHelperAttribute(params string[] identity)
		{
			this._identity=identity;
		}
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var token = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (token == null) { return false; }
			var role = FormsAuthentication.Decrypt(httpContext.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData;  //抓出cookie  跟  roles比較


			
			return _identity.Contains(role);//用來判斷權限


		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectResult("/Employees/permissions_failed");
		}
	}
}