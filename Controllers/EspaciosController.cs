using ParqueoCentralWeb.Filtro;
using ParqueoCentralWeb.Helpers;
using ParqueoCentralWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Controllers
{
	[OperadorAuthorize]
	public class EspaciosController: Controller
	{
		private readonly ParqueoCentralDBEntities _database = new ParqueoCentralDBEntities();

		[HttpGet]
		public ActionResult Index()
		{
			var espacios = _database.EspacioEstacionamiento.Select(v => new EspacioModel
			{
				IdEspacio = v.IdEspacio,
				CodigoEspacio = v.CodigoEspacio,
				TipoEspacio = v.TipoEspacio,
				Estado = v.Estado,
				Activo = v.Activo
			}).ToList();

			return View(espacios);
		}

		[HttpGet]
		public ActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public ActionResult Create(EspacioModel espacio)
		{
			if (_database.EspacioEstacionamiento.Any(v => v.CodigoEspacio == espacio.CodigoEspacio))
			{
				ModelState.AddModelError("Espacio", "El espacio ya se encuentra registrado");
			}
			if (!ModelState.IsValid)
			{
				TempData["Message"] = "Hay un error en los datos";
				TempData["MessageType"] = "danger";
				return View(espacio);
			}
			EspacioEstacionamiento espacioNuevo = new EspacioEstacionamiento
			{
				CodigoEspacio = espacio.CodigoEspacio,
				TipoEspacio = espacio.TipoEspacio,
				Estado = espacio.Estado,
				Activo = espacio.Activo
			};
			_database.EspacioEstacionamiento.Add(espacioNuevo);
			_database.SaveChanges();

			TempData["Message"] = "Se ha agregado correctamente el espacio";
			TempData["MessageType"] = "success";
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{

			EspacioModel espacio = _database.EspacioEstacionamiento.Where(e => e.IdEspacio == id).Select(e => new EspacioModel
			{
				IdEspacio = e.IdEspacio,
				CodigoEspacio = e.CodigoEspacio,
				TipoEspacio = e.TipoEspacio,
				Estado = e.Estado,
				Activo = e.Activo
			}).FirstOrDefault();
			if (espacio == null)
			{
				return RedirectToAction("Index");
			}
			return View(espacio);
		}

		[HttpPost]
		public ActionResult Edit(EspacioModel espacio)
		{
			if (_database.Vehiculo.Any(e => e.Placa == espacio.CodigoEspacio &&
				e.IdVehiculo != espacio.IdEspacio))
			{
				ModelState.AddModelError("Espacio", "El espacio ya se encuentra registrado");
			}
			if (!ModelState.IsValid)
			{
				TempData["Message"] = "Hay un error en los datos";
				TempData["MessageType"] = "danger";
				return View(espacio);
			}

			EspacioEstacionamiento espacioEditar = _database.EspacioEstacionamiento.Find(espacio.IdEspacio);
			if (espacioEditar == null)
				return HttpNotFound();

			espacioEditar.CodigoEspacio = espacio.CodigoEspacio;
			espacioEditar.TipoEspacio = espacio.TipoEspacio;
			espacioEditar.Estado = espacio.Estado;
			espacioEditar.Activo = espacio.Activo;

			_database.SaveChanges();

			TempData["Message"] = "Se ha modificado correctamente el espacio";
			TempData["MessageType"] = "success";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Details(int id)
		{
			EspacioModel espacio = _database.EspacioEstacionamiento.Where(e => e.IdEspacio == id).Select(e => new EspacioModel
			{
				IdEspacio = e.IdEspacio,
				CodigoEspacio = e.CodigoEspacio,
				TipoEspacio = e.TipoEspacio,
				Estado = e.Estado,
				Activo = e.Activo
			}).FirstOrDefault();
			if (espacio == null)
			{
				return RedirectToAction("Index");
			}
			return View(espacio);
		}

		[HttpGet]
		public ActionResult Delete(int id)
		{
			EspacioEstacionamiento espacioBorrar = _database.EspacioEstacionamiento.Find(id);
			try
			{
				_database.EspacioEstacionamiento.Remove(espacioBorrar);
				_database.SaveChanges();
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Hubo un error al borrar el espacio";
				TempData["MessageType"] = "danger";
				return RedirectToAction("Index");
			}

			TempData["Message"] = "Se ha eliminado correctamente el espacio";
			TempData["MessageType"] = "success";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public JsonResult ValidarCodigo(string codigoEspacio, int? idEspacio)
		{
			bool existe = _database.EspacioEstacionamiento.Any(e =>
				e.CodigoEspacio == codigoEspacio &&
				e.IdEspacio != idEspacio);

			return Json(!existe, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult ObtenerEspacios()
		{
			var request = DataTableRequest.FromRequest(Request);

			IQueryable<EspacioEstacionamiento> consulta =
				_database.EspacioEstacionamiento.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				consulta = consulta.Where(e =>
					e.CodigoEspacio.Contains(request.Search) ||
					e.TipoEspacio.Contains(request.Search) ||
					e.Estado.Contains(request.Search));
			}

			var resultado = DataTableService.Create(

				consulta.Select(e => new
				{
					e.IdEspacio,
					e.CodigoEspacio,
					e.TipoEspacio,
					e.Estado,
					e.Activo
				}),
				request,
				defaultOrderColumn: "IdEspacio");

			return Json(resultado);
		}
	}
}