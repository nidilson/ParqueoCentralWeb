using ParqueoCentralWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Controllers
{
	public class VehiculosController: Controller
	{
		private readonly ParqueoCentralDBEntities _database = new ParqueoCentralDBEntities();

		[HttpGet]
		public ActionResult Index()
		{
			var vehiculos = _database.Vehiculo.Select(v => new VehiculoModel
			{
				IdVehiculo = v.IdVehiculo,
				Placa = v.Placa,
				TipoVehiculo = v.TipoVehiculo,
				Propietario = v.Propietario,
				Contacto = v.Contacto
			}).ToList();

			return View(vehiculos);
		}

		[HttpGet]
		public ActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public ActionResult Create(VehiculoModel vehiculo)
		{
			if(_database.Vehiculo.Any(v => v.Placa == vehiculo.Placa))
			{
				ModelState.AddModelError("Placa", "La placa ya se encuentra registrada");
			}
			if (!ModelState.IsValid)
			{
				TempData["Message"] = "Hay un error en los datos";
				TempData["MessageType"] = "danger";
				return View(vehiculo);
			}
			Vehiculo vehiculoNuevo = new Vehiculo
			{
				Placa = vehiculo.Placa,
				TipoVehiculo = vehiculo.TipoVehiculo,
				Propietario = vehiculo.Propietario,
				Contacto = vehiculo.Contacto
			};
			_database.Vehiculo.Add(vehiculoNuevo);
			_database.SaveChanges();

			TempData["Message"] = "Se ha agregado correctamente el vehículo";
			TempData["MessageType"] = "success";
			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(int id)
		{

			VehiculoModel vehiculo = _database.Vehiculo.Where(v => v.IdVehiculo == id).Select(v => new VehiculoModel
			{
				IdVehiculo = v.IdVehiculo,
				Placa = v.Placa,
				TipoVehiculo = v.TipoVehiculo,
				Propietario = v.Propietario,
				Contacto = v.Contacto
			}).FirstOrDefault();
			if(vehiculo == null)
			{
				return RedirectToAction("Index");
			}
			return View(vehiculo);
		}

		[HttpPost]
		public ActionResult Edit(VehiculoModel vehiculo)
		{
			if (_database.Vehiculo.Any(v => v.Placa == vehiculo.Placa &&
				v.IdVehiculo != vehiculo.IdVehiculo))
			{
				ModelState.AddModelError("Placa", "La placa ya se encuentra registrada");
			}
			if (!ModelState.IsValid)
			{
				TempData["Message"] = "Hay un error en los datos";
				TempData["MessageType"] = "danger";
				return View(vehiculo);
			}

			Vehiculo vehiculoEditar = _database.Vehiculo.Find(vehiculo.IdVehiculo);
			if (vehiculoEditar == null)
				return HttpNotFound();

			vehiculoEditar.Placa = vehiculo.Placa;
			vehiculoEditar.TipoVehiculo = vehiculo.TipoVehiculo;
			vehiculoEditar.Propietario = vehiculo.Propietario;
			vehiculoEditar.Contacto = vehiculo.Contacto;

			_database.SaveChanges();

			TempData["Message"] = "Se ha modificado correctamente el vehículo";
			TempData["MessageType"] = "success";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Details(int id)
		{
			VehiculoModel vehiculo = _database.Vehiculo.Where(v => v.IdVehiculo == id).Select(v => new VehiculoModel
			{
				IdVehiculo = v.IdVehiculo,
				Placa = v.Placa,
				TipoVehiculo = v.TipoVehiculo,
				Propietario = v.Propietario,
				Contacto = v.Contacto
			}).FirstOrDefault();
			if (vehiculo == null)
			{
				return RedirectToAction("Index");
			}
			return View(vehiculo);
		}

		[HttpGet]
		public ActionResult Delete(int id)
		{
			Vehiculo vehiculoBorrar = _database.Vehiculo.Find(id);
			try
			{
				_database.Vehiculo.Remove(vehiculoBorrar);
				_database.SaveChanges();
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Hubo un error al borrar el vehículo";
				TempData["MessageType"] = "danger";
				return RedirectToAction("Index");
			}

			TempData["Message"] = "Se ha eliminado correctamente el vehículo";
			TempData["MessageType"] = "success";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public JsonResult ValidarPlaca(string placa, int? idVehiculo)
		{
			bool existe = _database.Vehiculo.Any(v =>
				v.Placa == placa &&
				v.IdVehiculo != idVehiculo);

			return Json(!existe, JsonRequestBehavior.AllowGet);
		}
	}
}