using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParqueoCentralWeb.Filtro;
using ParqueoCentralWeb.Models;

namespace ParqueoCentralWeb.Controllers
{
	[OperadorAuthorize]
	public class HomeController : Controller
	{
		private readonly ParqueoCentralDBEntities _database = new ParqueoCentralDBEntities();
		public ActionResult Index()
		{
			ViewBag.Operador = "Nidilson";
			ViewBag.EspaciosDisponibles = _database.EspacioEstacionamiento.Count(e => e.Estado == "Disponible");

			ViewBag.EspaciosOcupados = _database.EspacioEstacionamiento.Count(e => e.Estado == "Ocupado");

			ViewBag.Vehiculos = _database.MovimientoEstacionamiento
								.AsNoTracking()
								.Count(m => m.EstadoMovimiento == "En uso");

			var hoy = DateTime.Today;
			var manana = hoy.AddDays(1);

			ViewBag.Movimientos = _database.MovimientoEstacionamiento.Count(m =>
				m.FechaHoraEntrada >= hoy &&
				m.FechaHoraEntrada < manana);

			return View();
			
		}

		[HttpPost]
		public JsonResult ObtenerMovimientos()
		{
			var draw = Request.Form["draw"];

			var start = Convert.ToInt32(Request.Form["start"]);

			var length = Convert.ToInt32(Request.Form["length"]);

			var search = Request.Form["search[value]"];

			var consulta =
				from m in _database.MovimientoEstacionamiento.AsNoTracking()
				join v in _database.Vehiculo on m.IdVehiculo equals v.IdVehiculo
				join e in _database.EspacioEstacionamiento on m.IdEspacio equals e.IdEspacio
				select new
				{
					Placa = v.Placa,
					CodigoEspacio = e.CodigoEspacio,
					HoraEntrada = m.FechaHoraEntrada,
					Estado = m.EstadoMovimiento
				};

			if (!string.IsNullOrWhiteSpace(search))
			{
				consulta = consulta.Where(x =>
					x.Placa.Contains(search) ||
					x.CodigoEspacio.Contains(search) ||
					x.Estado.Contains(search));
			}

			var totalRegistros = consulta.Count();

			var datos = consulta
				.OrderByDescending(x => x.HoraEntrada)
				.Skip(start)
				.Take(length)
				.ToList()
				.Select(x => new
				{
					x.Placa,
					x.CodigoEspacio,
					HoraEntrada = x.HoraEntrada.ToString("HH:mm"),
					Estado = x.Estado
				});

			return Json(new
			{
				draw = draw,
				recordsTotal = totalRegistros,
				recordsFiltered = totalRegistros,
				data = datos
			});
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}