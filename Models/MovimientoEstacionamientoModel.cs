using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

		[Required(ErrorMessage = "El usuario que registra el movimiento es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Usuario que Registró")] 
		public string UsuarioRegistro { get; set; }
	}
}