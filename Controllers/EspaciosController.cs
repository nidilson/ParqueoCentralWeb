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
		/// <summary>
		/// Obtiene la lista vista de los espacios almacenados en la base de datos
		/// </summary>
		/// <returns>Vista con los espacios almacenados</returns>
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// Método para obtener la vista con el formulario para crear un nuevo espacio
		/// </summary>
		/// <returns>Vista para crear el formulario</returns>
		[HttpGet]
		public ActionResult Create()
		{

			return View();
		}

		/// <summary>
		/// Inserta un nuevo objeto de tipo Espacio en la base de datos con los datos que el cliente envía
		/// Se hace la validación del modelo
		/// </summary>
		/// <param name="espacio">Datos del cliente del espacio a insertar</param>
		/// <returns>Redirecciona a Inicio en caso de que se ingrese exitosamente, de lo contrario 
		/// regresa a la vista de crear con los errores obtenidos</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
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

		/// <summary>
		/// Método que obtiene la vista para editar los datos de un espacio
		/// </summary>
		/// <param name="id">Id del objeto que se quiere editar. Es obligatorio</param>
		/// <returns>Vista con los datos del espacio en un formulario</returns>
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

		/// <summary>
		/// Método POST que modifica los datos del espacio con los nuevos datos ingresados por el usuario
		/// </summary>
		/// <param name="espacio">Objeto con los datos del espacio modificados</param>
		/// <returns>Redirecciona al inicio en caso de que los datos se hayan modificado correctamente.
		/// En caso contrario recarga la página con los mensajes de error pertinentes</returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
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
		/// <summary>
		/// Método que obtiene la vista con los datos de un espacio especificado
		/// </summary>
		/// <param name="id">Id del espacio del que se desea ver los detalles</param>
		/// <returns>Vista con los detalles del espacio</returns>
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

		/// <summary>
		/// Borra un espacio que esté almacenado en la base de datos, pero que no tenga ningún movimiento asociado
		/// </summary>
		/// <param name="id">Id del espacio que se quiere eliminar</param>
		/// <returns>Redirecciona a la lista de los espacios con un mensaje de confirmación o error</returns>
		[HttpGet]
		public ActionResult Delete(int id)
		{
			if(_database.MovimientoEstacionamiento.Count(m => m.IdEspacio == id) > 0)
			{
				TempData["Message"] = "No se puede eliminar un espacio utilizado en un movimiento";
				TempData["MessageType"] = "danger";
				return RedirectToAction("Index");
			}

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
		/// <summary>
		/// Método que valida si un espacio se encuentra almacenado en la base de datos.
		/// Esto previene que dos espacios sean guardados con el mismo código de espacio.
		/// Se verifica que no haya un espacio almacenado en la base de datos con ese mismo código, en caso
		/// de que lo haya se verifica que no sea el mismo espacio que se está editando
		/// </summary>
		/// <param name="codigoEspacio">Código del espacio que se desea verificar</param>
		/// <param name="idEspacio">Opcional. Id del espacio que se está editando, permite verificar si el codigo
		/// almacenado pertenece al espacio que se está editando, en caso contrario lo notifica</param>
		/// <returns>true = Existe un código ya registrado
		///			 false = No existe un código igual registrado</returns>
		[HttpGet]
		public JsonResult ValidarCodigo(string codigoEspacio, int? idEspacio)
		{
			bool existe = _database.EspacioEstacionamiento.Any(e =>
				e.CodigoEspacio == codigoEspacio &&
				e.IdEspacio != idEspacio);

			return Json(!existe, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Obtiene los espacios que están almacenados en la base de datos para llenar la tabla
		/// de la vista Index. En la Request se envían los parámetros para filtrar los datos
		/// </summary>
		/// <returns>Objeto DataTableResponse con los datos filtrados</returns>
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