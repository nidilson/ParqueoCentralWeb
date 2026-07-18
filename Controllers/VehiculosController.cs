using ParqueoCentralWeb.Filtro;
using ParqueoCentralWeb.Helpers;
using ParqueoCentralWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Controllers
{
	[OperadorAuthorize]
	public class VehiculosController: Controller
	{
		private readonly ParqueoCentralDBEntities _database = new ParqueoCentralDBEntities();

		/// <summary>
		/// Métdod que devuelve la vista con todos los vehículos ingresados en la base de datos
		/// </summary>
		/// <returns>Vista con los datos de los vehículos</returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Método que devuelve la vista con un formulario para crear un nuevo objeto de tipo Vehículo
		/// </summary>
		/// <returns>Vista con formulario para los datos del vehículo</returns>
		[HttpGet]
		public ActionResult Create()
		{

			return View();
		}

		/// <summary>
		/// Método que guarda el vehículo en la base de datos según los datos que ingresó el usuario
		/// </summary>
		/// <param name="vehiculo">Datos del vehículo que se va a ingresar</param>
		/// <returns>Redirige a la lista de vehículos con mensaje de confirmación en caso de que se haya ingresado correctamente.
		/// En caso contrario devuelve la vista con el formulario mostrando un mensaje de error</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
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
			try
			{
				Vehiculo vehiculoNuevo = new Vehiculo
				{
					Placa = vehiculo.Placa,
					TipoVehiculo = vehiculo.TipoVehiculo,
					Propietario = vehiculo.Propietario,
					Contacto = vehiculo.Contacto
				};
				_database.Vehiculo.Add(vehiculoNuevo);
				_database.SaveChanges();
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Hubo un error al ingresar el vehículo";
				TempData["MessageType"] = "danger";
				return View(vehiculo);
			}
			

			TempData["Message"] = "Se ha agregado correctamente el vehículo";
			TempData["MessageType"] = "success";
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Método que devuelve una vista para editar los datos de un vehículo ingresado en la base de datos
		/// </summary>
		/// <param name="id">Id del vehículo a editar</param>
		/// <returns>Vista con el formulario para editar los datos</returns>
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
		/// <summary>
		/// Método POST que modifica los datos del vehículo en la base de datos
		/// </summary>
		/// <param name="vehiculo">Datos modificados del vehículo</param>
		/// <returns>Redirecciona</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
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
			try
			{
				Vehiculo vehiculoEditar = _database.Vehiculo.Find(vehiculo.IdVehiculo);
				if (vehiculoEditar == null)
					return HttpNotFound();

				vehiculoEditar.Placa = vehiculo.Placa;
				vehiculoEditar.TipoVehiculo = vehiculo.TipoVehiculo;
				vehiculoEditar.Propietario = vehiculo.Propietario;
				vehiculoEditar.Contacto = vehiculo.Contacto;

				_database.SaveChanges();
			}
			catch (Exception ex)
			{
				TempData["Message"] = "Hubo un error al modificar los datos";
				TempData["MessageType"] = "danger";
				return View(vehiculo);
			}

			TempData["Message"] = "Se ha modificado correctamente el vehículo";
			TempData["MessageType"] = "success";

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Método para obtener la vista con todos los detalles de un vehículo
		/// </summary>
		/// <param name="id">Id del vehículo del que se desea ver los detalles</param>
		/// <returns>Vista con los detalles del vehículo</returns>
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
		/// <summary>
		/// Borra un vehiculo de la base de datos, primero valida que el vehículo no tenga un movimiento asociado
		/// </summary>
		/// <param name="id">Id del vehículo a eliminar</param>
		/// <returns>Redirige a la lista de vehículos con el mensaje de error o éxito</returns>
		[HttpGet]
		public ActionResult Delete(int id)
		{
			Vehiculo vehiculoBorrar = _database.Vehiculo.Find(id);
			if(_database.MovimientoEstacionamiento.Count(m => m.IdVehiculo == vehiculoBorrar.IdVehiculo) > 0)
			{
				TempData["Message"] = "No se puede eliminar un vehículo con un movimiento asociado";
				TempData["MessageType"] = "danger";
				return RedirectToAction("Index");
			}

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
		/// <summary>
		/// Valida que la placa no esté asociada a un vehículo ya almacenado en la base de datos diferente al que se
		/// está creando o editando
		/// </summary>
		/// <param name="placa">Número de la placa a verificar</param>
		/// <param name="idVehiculo">Opcional. Id del vehículo que se está editando</param>
		/// <returns>true = La placa se encuentra registrada en la base de datos
		///			 false = La placa no se encuentra registrada en la base de datos</returns>
		[HttpGet]
		public JsonResult ValidarPlaca(string placa, int? idVehiculo)
		{
			bool existe = _database.Vehiculo.Any(v =>
				v.Placa == placa &&
				v.IdVehiculo != idVehiculo);

			return Json(!existe, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Obtiene la lista de vehículos según los parametros enviados en la request. Sirve para llenar de datos
		/// la tabla de vehículos de la vista Index
		/// </summary>
		/// <returns>Objeto DataTableResponse con los datos filtrados de los vehículos</returns>
		[HttpPost]
		public JsonResult ObtenerVehiculos()
		{
			var request = DataTableRequest.FromRequest(Request);

			IQueryable<Vehiculo> consulta =
				_database.Vehiculo.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				consulta = consulta.Where(v =>
					v.Placa.Contains(request.Search) ||
					v.Propietario.Contains(request.Search) ||
					v.Contacto.Contains(request.Search));
			}

			var resultado = DataTableService.Create(

				consulta.Select(v => new
				{
					v.IdVehiculo,
					v.Placa,
					v.TipoVehiculo,
					v.Propietario,
					v.Contacto
				}),
				request,
				defaultOrderColumn: "IdVehiculo");

			return Json(resultado);
		}
	}
}