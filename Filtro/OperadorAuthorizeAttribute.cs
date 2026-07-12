using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Filtro
{
	public class OperadorAuthorizeAttribute: ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var operador = filterContext.HttpContext.Session["Operador"];

			if (operador == null)
			{
				filterContext.Result = new RedirectToRouteResult(
					new System.Web.Routing.RouteValueDictionary
					{
						{ "controller", "Operador" },
						{ "action", "LogIn" }
					});
			}

			base.OnActionExecuting(filterContext);
		}
	}
}