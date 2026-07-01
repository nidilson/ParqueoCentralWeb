using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Models
{
	public class VehiculoModel
	{
		public int IdVehiculo { get; set; }

		[Required(ErrorMessage = "La placa es obligatoria.")]
		[StringLength(8, ErrorMessage = "La placa no puede tener más de 8 caracteres.")]
		[Remote("ValidarPlaca", "Vehiculos", AdditionalFields = "IdVehiculo", ErrorMessage = "La placa ya se encuentra registrada.")]
		[Display(Name = "Placa")]
		public string Placa { get; set; }

		[Required(ErrorMessage = "El tipo de vehículo es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Tipo de Vehículo")]
		public string TipoVehiculo { get; set; }

		[Required(ErrorMessage = "El propietario es obligatorio.")]
		[StringLength(50)]
		public string Propietario { get; set; }

		[Required(ErrorMessage = "El contacto es obligatorio.")]
		[StringLength(50)]
		public string Contacto { get; set; }
	}
}