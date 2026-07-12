using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Controllers
{
	public class OperadorController: Controller
	{
		[HttpGet]
		public ActionResult LogIn()
		{
			Session.Clear();
			return View();
		}
		[HttpPost]
		public ActionResult LogIn(string nombre)
		{
			Session["operador"] = nombre;
			return RedirectToAction("Index", "Home");
		}
	}
}