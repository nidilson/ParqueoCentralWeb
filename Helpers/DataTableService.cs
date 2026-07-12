using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace ParqueoCentralWeb.Helpers
{
	public class DataTableService
	{
		public static DataTableResponse Create<T>(
			IQueryable<T> query,
			DataTableRequest request,
			string defaultOrderColumn = "Id",
			string defaultOrderDirection = "asc")
		{
			int total = query.Count();

			string sortColumn = string.IsNullOrWhiteSpace(request.SortColumn)
				? defaultOrderColumn
				: request.SortColumn;

			string sortDirection = string.IsNullOrWhiteSpace(request.SortDirection)
				? defaultOrderDirection
				: request.SortDirection;

			query = query.OrderBy($"{sortColumn} {sortDirection}");

			var data = query
				.Skip(request.Start)
				.Take(request.Length)
				.ToList();

			return new DataTableResponse
			{
				draw = request.Draw,
				recordsTotal = total,
				recordsFiltered = total,
				data = data
			};
		}
	}
}