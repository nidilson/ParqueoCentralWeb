using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParqueoCentralWeb.Models
{
	public class EspacioModel
	{
		public int IdEspacio { get; set; }

		[Required(ErrorMessage = "El código del espacio es obligatorio.")]
		[StringLength(20, ErrorMessage = "El código del espacio no puede exceder los 20 caracteres.")]
		[Remote("ValidarCodigo", "Espacios", AdditionalFields = "IdEspacio", ErrorMessage = "El espacio ya se encuentra registrado.")]
		[Display(Name = "Código del Espacio")]
		public string CodigoEspacio { get; set; }

		[Required(ErrorMessage = "El tipo de espacio es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Tipo de Espacio")]
		public string TipoEspacio { get; set; }

		[Required(ErrorMessage = "El estado es obligatorio.")]
		[StringLength(50)]
		[Display(Name = "Estado")]
		public string Estado { get; set; }

		[Display(Name = "Activo")]
		public bool Activo { get; set; }
	}
}