using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Controllers
{
	public class OperadorController: Controller
	{
		public ActionResult LogIn()
		{
			return View();
		}
		public ActionResult LogIn(string nombre)
		{
			Session["operador"] = nombre;
			return View();
		}
		public ActionResult LogOut(string nombre)
		{
			Session["operador"] = "";
			return View();
		}
	}
}