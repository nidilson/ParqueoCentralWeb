using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Models
{
	public class MovimientoEstacionamientoModel
	{
		public int IdMovimiento { get; set; }

		[Required(ErrorMessage = "Debe seleccionar un vehículo.")]
		[Display(Name = "Vehículo")] 
		public int IdVehiculo { get; set; }

		[Required(ErrorMessage = "Debe seleccionar un espacio.")]
		[Display(Name = "Espacio")]
		public int IdEspacio { get; set; }

		[Required(ErrorMessage = "La placa es obligatoria.")]
		[StringLength(8, ErrorMessage = "La placa no puede tener más de 8 caracteres.")]
		[Display(Name = "Placa")]
		public string Placa { get; set; }

		[Required(ErrorMessage = "Se debe elegir una placa")]
		[StringLength(50)]
		[Display(Name = "Tipo de Vehículo")]
		public string TipoVehiculo { get; set; }

		[Required(ErrorMessage = "Se debe elegir una placa")]
		[StringLength(50)]
		public string Propietario { get; set; }

		[StringLength(20, ErrorMessage = "El código del espacio no puede exceder los 20 caracteres.")]
		[Display(Name = "Código del Espacio")]
		public string CodigoEspacio { get; set; }

		[Required(ErrorMessage = "La fecha y hora de entrada son obligatorias.")]
		[Display(Name = "Fecha y Hora de Entrada")]
		[DataType(DataType.DateTime)] 
		public DateTime FechaHoraEntrada { get; set; }

		[Display(Name = "Fecha y Hora de Salida")]
		[DataType(DataType.DateTime)] 
		public DateTime? FechaHoraSalida { get; set; }

		[Required(ErrorMessage = "El estado del movimiento es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Estado")]
		public string EstadoMovimiento { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a cero.")]
		[Display(Name = "Monto Cobrado")]
		[DataType(DataType.Currency)] 
		public decimal? MontoCobrado { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a cero.")]
		[Display(Name = "Tiempo Parqueado")]
		public double? TiempoParqueado { get; set; }

		[Required(ErrorMessage = "El usuario que registra el movimiento es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Usuario que Registró")] 
		public string UsuarioRegistro { get; set; }

	}
}