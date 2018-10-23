using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlayMinesweeper.Controllers
{
    public class CustomAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        
		void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
		{
			// if the username session variable is null, redirect user to the login page
			if(System.Web.HttpContext.Current.Session["Username"] == null)
				filterContext.Result = new RedirectResult("/User/Login");
		}

    }
}