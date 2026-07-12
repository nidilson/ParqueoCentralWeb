$(document).ready(function () {

    $('#tblEspacios').DataTable({

        processing: true,
        serverSide: true,
        responsive: true,
        searching: true,
        ordering: true,

        ajax: {
            url: "/Espacios/ObtenerEspacios",
            type: "POST"
        },

        columns: [

            { data: "CodigoEspacio" },

            { data: "TipoEspacio" },

            {
                data: "Estado",
                render: function (data) {

                    if (data === "Disponible")
                        return '<span class="badge bg-success">Disponible</span>';

                    return '<span class="badge bg-danger">Ocupado</span>';
                }
            },

            {
                data: "Activo",
                className: "text-center",
                render: function (data) {

                    if (data)
                        return '<span class="badge bg-primary">Sí</span>';

                    return '<span class="badge bg-secondary">No</span>';
                }
            },

            {
                data: "IdEspacio",
                orderable: false,
                searchable: false,
                className: "text-center",

                render: function (data) {

                    return `
                        <div class="btn-group">

                            <a href="/Espacios/Details/${data}"
                               class="btn btn-info btn-sm">
                                <i class="bi bi-eye"></i>
                            </a>

                            <a href="/Espacios/Edit/${data}"
                               class="btn btn-warning btn-sm">
                                <i class="bi bi-pencil"></i>
                            </a>

                            <button type="button"
                                    class="btn btn-danger btn-sm btnEliminarEspacio"
                                    data-id="${data}">
                                <i class="bi bi-trash"></i>
                            </button>

                        </div>`;
                }
            }

        ],

        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json"
        }

    });

});


$(document).on("click", ".btnEliminarEspacio", function () {

    var id = $(this).data("id");

    $("#btnConfirmarEliminarEspacio")
        .attr("href", "/Espacios/Delete/" + id);

    var modal = new bootstrap.Modal(
        document.getElementById("modalEliminarEspacio"));

    modal.show();

});