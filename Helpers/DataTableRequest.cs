using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParqueoCentralWeb.Helpers
{
	public class DataTableRequest
	{
		public int Draw { get; set; }

		public int Start { get; set; }

		public int Length { get; set; }

		public string Search { get; set; }

		public string SortColumn { get; set; }

		public string SortDirection { get; set; }

		public static DataTableRequest FromRequest(HttpRequestBase request)
		{
			var columnIndex = request.Form["order[0][column]"];

			return new DataTableRequest
			{
				Draw = int.Parse(request.Form["draw"]),
				Start = int.Parse(request.Form["start"]),
				Length = int.Parse(request.Form["length"]),
				Search = request.Form["search[value]"],
				SortDirection = request.Form["order[0][dir]"],
				SortColumn = request.Form[$"columns[{columnIndex}][data]"]
			};
		}
	}
}