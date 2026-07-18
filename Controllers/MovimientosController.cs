                      using ParqueoCentralWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParqueoCentralWeb.Helpers;
using ParqueoCentralWeb.Filtro;

namespace ParqueoCentralWeb.Controllers
{
	[OperadorAuthorize]
	public class MovimientosController: Controller
	{
		private readonly ParqueoCentralDBEntities _database = new ParqueoCentralDBEntities();

		/// <summary>
		/// Devuelve la vista con el historial de los movimientos registrados en la base de datos
		/// </summary>
		/// <returns>Vista con los movimientos registrados en la base de datos</returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Obtiene los movimientos de la base de datos según los parámetros enviados en la solicitud. sirve para llenar los datos de la tabla de movimientos
		/// </summary>
		/// <returns>Objeto DataTableResponse con los datos filtrados</returns>
		[HttpPost]
		public JsonResult ObtenerMovimientos()
		{
			var request = DataTableRequest.FromRequest(Request);

			var consulta =
				from m in _database.MovimientoEstacionamiento.AsNoTracking()
				join v in _database.Vehiculo on m.IdVehiculo equals v.IdVehiculo
				join e in _database.EspacioEstacionamiento on m.IdEspacio equals e.IdEspacio
				select new
				{
					m.IdMovimiento,

					Placa = v.Placa,

					CodigoEspacio = e.CodigoEspacio,

					FechaHoraEntrada = m.FechaHoraEntrada,

					FechaHoraSalida = m.FechaHoraSalida,

					MontoCobrado = m.MontoCobrado,

					EstadoMovimiento = m.EstadoMovimiento,

					UsuarioRegistro = m.UsuarioRegistro
				};

			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				consulta = consulta.Where(x =>
					x.Placa.Contains(request.Search) ||
					x.CodigoEspacio.Contains(request.Search) ||
					x.EstadoMovimiento.Contains(request.Search) ||
					x.UsuarioRegistro.Contains(request.Search));
			}

			var resultado = DataTableService.Create(
				consulta,
				request,
				defaultOrderColumn: "FechaHoraEntrada",
				defaultOrderDirection: "desc");

			return Json(resultado);
		}

		/// <summary>
		/// Obtiene la vista con los detalles de un movimiento seleccionado por el usuario
		/// </summary>
		/// <param name="id">Id del movimiento que se quiere ver</param>
		/// <returns>Vista con los detalles del movimiento</returns>
		public ActionResult Details(int id)
		{
			MovimientoEstacionamientoModel mov = (from m in _database.MovimientoEstacionamiento
												  join v in _database.Vehiculo on m.IdVehiculo equals v.IdVehiculo
												  join e in _database.EspacioEstacionamiento on m.IdEspacio equals e.IdEspacio
												  where m.IdMovimiento == id
												  select new MovimientoEstacionamientoModel
												  {
													  IdMovimiento = m.IdMovimiento,
													  IdVehiculo = v.IdVehiculo,
													  IdEspacio = e.IdEspacio,
													  Placa = v.Placa,
													  TipoVehiculo = v.TipoVehiculo,
													  Propietario = v.Propietario,
													  CodigoEspacio = e.CodigoEspacio,
													  FechaHoraEntrada = m.FechaHoraEntrada,
													  FechaHoraSalida = m.FechaHoraSalida,
													  EstadoMovimiento = m.EstadoMovimiento,
													  MontoCobrado = m.MontoCobrado,
													  UsuarioRegistro = m.UsuarioRegistro
												  }).FirstOrDefault();

			mov.ObtenerTiempoParqueado();
			return View(mov);
		}

		#region Entradas

		/// <summary>
		/// Obtiene la vista para registrar una nueva entrada en el parqueo
		/// </summary>
		/// <returns>Vista con el formulario para ingresar los datos</returns>
		[HttpGet]
		public ActionResult Entrada()
		{
			ViewBag.Espacios = new SelectList(_database.EspacioEstacionamiento.Where(e => e.Estado.Equals("Disponible") && e.Activo).ToList(), "IdEspacio", "CodigoEspacio");
			MovimientoEstacionamientoModel movimiento = new MovimientoEstacionamientoModel();
			movimiento.FechaHoraEntrada = DateTime.Now;
			movimiento.EstadoMovimiento = "En uso";
			return View(movimiento);
		}

		/// <summary>
		/// Registra la entrada en la base de datos y cambia el estado de del espacio a "Ocupado". Se verifica los datos antes de ingresarla
		/// </summary>
		/// <param name="movimiento"></param>
		/// <returns>Redirige al inicio con un mensaje de error o de éxito</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Entrada(MovimientoEstacionamientoModel movimiento)
		{
			VehiculoModel vehiculo = ObtenerVehiculoPorId(movimiento.IdVehiculo);

			if (VerificarEntradasVehiculo(vehiculo) != 0)
			{
				ModelState.AddModelError("IdVehiculo", "Debe ingresar una placa válida");
			}
			if (!ModelState.IsValid) {
				ViewBag.Espacios = new SelectList(
					_database.EspacioEstacionamiento
						.Where(e => e.Estado.Equals("Disponible") && e.Activo)
						.ToList(),
					"IdEspacio",
					"CodigoEspacio");
				return View(movimiento);
			}
			MovimientoEstacionamiento movimientoNuevo = new MovimientoEstacionamiento
			{
				IdEspacio = movimiento.IdEspacio,
				IdVehiculo = movimiento.IdVehiculo,
				FechaHoraEntrada = movimiento.FechaHoraEntrada,
				EstadoMovimiento = "En uso",
				UsuarioRegistro = Session["Operador"].ToString()
			};

			using(var transaction = _database.Database.BeginTransaction()) {
				try
				{
					_database.MovimientoEstacionamiento.Add(movimientoNuevo);
					EspacioEstacionamiento espacio = _database.EspacioEstacionamiento.Find(movimientoNuevo.IdEspacio);
					if (espacio.Estado == "Ocupado")
						throw new Exception();
					espacio.Estado = "Ocupado";
					_database.SaveChanges();
					transaction.Commit();
				
				}
				catch (Exception)
				{
					transaction.Rollback();
					Response.StatusCode = 500;
					TempData["Message"] = "Hubo un error al registrar la entrada";
					TempData["MessageType"] = "danger";
					return View(movimiento);
				}
			}
			TempData["Message"] = "Entrada registrada correctamente";
			TempData["MessageType"] = "success";
			return RedirectToAction("Index", "Home");
		}
		/// <summary>
		/// Revisa que la placa que se ingresó no tenga ninguna entrada ya registrada, así no se le registran varios movimientos de entrada
		/// a un solo vehículo
		/// </summary>
		/// <param name="placa">Placa que se desea verificar</param>
		/// <returns>Datos del vehículo en caso de que no tenga entradas, caso contrario retorna un código de error</returns>
		[HttpGet]
		public ActionResult RevisarPlaca(string placa)
		{
			
			VehiculoModel vehiculo = ObtenerVehiculoPorPlaca(placa);

			switch (VerificarEntradasVehiculo(vehiculo))
			{
				case 0:
					return Json(vehiculo, JsonRequestBehavior.AllowGet);
				case 1:
					return HttpNotFound();
				case 2:
					Response.StatusCode = 403;
					return Json(new { message = "Este vehículo ya tiene una entrada registrada" }, JsonRequestBehavior.AllowGet);
				default:
					Response.StatusCode = 500;
					return Json(new { message = "Error en el servidor" }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Obtiene el vehículo con el numero de placa
		/// </summary>
		/// <param name="placa">numero de placa a buscar</param>
		/// <returns>Vehículo obtenido, puede ser null</returns>
		private VehiculoModel ObtenerVehiculoPorPlaca(string placa)
		{
			try
			{
				VehiculoModel vehiculo = _database.Vehiculo.Where(v => v.Placa.Equals(placa)).Select(v =>
				new VehiculoModel{
					IdVehiculo = v.IdVehiculo,
					Placa = v.Placa,
					Propietario = v.Propietario,
					TipoVehiculo = v.TipoVehiculo
				}).FirstOrDefault();
				return vehiculo;
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		/// <summary>
		/// Obtiene el vehículo con el Id
		/// </summary>
		/// <param name="id">Id a buscar</param>
		/// <returns>Vehículo obtenido, puede ser null</returns>
		private VehiculoModel ObtenerVehiculoPorId(int id)
		{
			try
			{
				VehiculoModel vehiculo = _database.Vehiculo.Where(v => v.IdVehiculo == id).Select(v =>
				new VehiculoModel
				{
					IdVehiculo = v.IdVehiculo,
					Placa = v.Placa,
					Propietario = v.Propietario,
					TipoVehiculo = v.TipoVehiculo
				}).FirstOrDefault();
				return vehiculo;
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		/// <summary>
		/// Determina si un vehículo es null o si ya tiene una entrada activa
		/// </summary>
		/// <param name="vehiculo">Vehículo a revisar</param>
		/// <returns>Codigo con el resultado 0: Sin problemas 1: Null 2: Tiene entrada Activa 3: Error</returns>
		private int VerificarEntradasVehiculo(VehiculoModel vehiculo)
		{
			if (vehiculo == null)
				return 1;
			try
			{
				if (_database.MovimientoEstacionamiento.Any(e => e.IdVehiculo == vehiculo.IdVehiculo && e.EstadoMovimiento.Equals("En uso")))
				return 2;
			}
			catch (Exception ex)
			{
				return 3;
			}
			
			return 0;
		}
		#endregion

		#region Salidas

		/// <summary>
		/// Devuelve una vista para poder registrar la salida de un vehículo del parqueo
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult Salida(int? id)
		{
			
			if (id != null)
			{
				string placa = (from m in _database.MovimientoEstacionamiento
								join v in _database.Vehiculo on m.IdVehiculo equals v.IdVehiculo
								where m.IdMovimiento == id
								select v.Placa).FirstOrDefault();
				MovimientoEstacionamientoModel mov = new MovimientoEstacionamientoModel { Placa = placa };
				return View(mov);
			}
			return View();
		}

		/// <summary>
		/// Registra la salida del vehículo del parqueo y modifica el estado del espacio a "Disponible"
		/// </summary>
		/// <param name="movimiento">Movimiento al cual se le quiere registrar la salida</param>
		/// <returns>Redirige al Inicio de la aplicación con un mensaje de éxito, en caso contrario recarga la página con un mensaje de error</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Salida(MovimientoEstacionamientoModel movimiento)
		{
			MovimientoEstacionamiento movimientoModificar;
			EspacioEstacionamiento espacio;
			using (var transaction = _database.Database.BeginTransaction())
			{
				try
				{
					movimientoModificar = _database.MovimientoEstacionamiento.Find(movimiento.IdMovimiento);
					espacio = _database.EspacioEstacionamiento.Find(movimiento.IdEspacio);
					
					movimientoModificar.EstadoMovimiento = "Finalizado";
					movimientoModificar.FechaHoraSalida = movimiento.FechaHoraSalida;
					movimientoModificar.MontoCobrado = movimiento.MontoCobrado;

					espacio.Estado = "Disponible";

					_database.SaveChanges();

					transaction.Commit();

				}
				catch (Exception ex)
				{
					transaction.Rollback();
					TempData["Message"] = "Hubo un error al registrar la salida";
					TempData["MessageType"] = "danger";
					return View();
				}
			}

			TempData["Message"] = "Salida registrada correctamente";
			TempData["MessageType"] = "success";
			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// Obtiene los datos de un movmiento almacenado en la base de datos para registrar la salida.
		/// </summary>
		/// <param name="placa">Placa del vehículo que se desea obtener el movimiento activo</param>
		/// <returns>Movimiento activo de la placa, en caso de no encontrarse ninguno entonces se devuelve un código 404 (Http Not Found)</returns>
		[HttpGet]
		public ActionResult ObtenerMovimiento(string placa)
		{
			MovimientoEstacionamientoModel movimiento = new MovimientoEstacionamientoModel();
			try
			{
				var consulta = from m in _database.MovimientoEstacionamiento
							   join v in _database.Vehiculo on m.IdVehiculo equals v.IdVehiculo
							   join e in _database.EspacioEstacionamiento on m.IdEspacio equals e.IdEspacio
							   where v.Placa == placa && m.EstadoMovimiento == "En uso"
							   select new MovimientoEstacionamientoModel
							   {
								   IdMovimiento = m.IdMovimiento,
								   IdVehiculo = m.IdVehiculo,
								   IdEspacio = m.IdEspacio,
								   Propietario = v.Propietario,
								   TipoVehiculo = v.TipoVehiculo,
								   Placa = v.Placa,
								   CodigoEspacio = e.CodigoEspacio,
								   FechaHoraEntrada = m.FechaHoraEntrada,
								   EstadoMovimiento = m.EstadoMovimiento,
								   UsuarioRegistro = m.UsuarioRegistro
							   };

				movimiento = consulta.ToList().FirstOrDefault();

			}catch(Exception ex)
			{
				return new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError);
			}

			if (movimiento == null)
				return HttpNotFound();

			movimiento.CalcularDatosParaSalida();

			return Json(movimiento, JsonRequestBehavior.AllowGet);
		}

		#endregion
	}
}