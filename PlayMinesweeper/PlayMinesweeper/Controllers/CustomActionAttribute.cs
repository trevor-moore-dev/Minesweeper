using PlayMinesweeper.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlayMinesweeper.Controllers
{
	public class CustomActionAttribute : FilterAttribute, IActionFilter
	{

		void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
		{
			string name = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + ":" + filterContext.ActionDescriptor.ActionName;
			MyLogger1.GetInstance().Info("Exiting Controller: " + name);
		}

		void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
		{
			string name = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + ":" + filterContext.ActionDescriptor.ActionName;
			MyLogger1.GetInstance().Info("Entering Controller: " + name);
		}

	}
}