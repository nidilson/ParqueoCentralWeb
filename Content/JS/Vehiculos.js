$(document).ready(function () {

    $('#tblVehiculos').DataTable({

        processing: true,
        serverSide: true,
        responsive: true,
        searching: true,
        ordering: true,
        paging: true,

        ajax: {
            url: "/Vehiculos/ObtenerVehiculos",
            type: "POST"
        },

        columns: [
            { data: "Placa" },
            { data: "TipoVehiculo" },
            { data: "Propietario" },
            { data: "Contacto" },
            {
                data: "IdVehiculo",
                orderable: false,
                searchable: false,
                className: "text-center",
                render: function (data) {

                    return `
                        <div class="btn-group" role="group">

                            <a href="/Vehiculos/Details/${data}"
                               class="btn btn-info btn-sm">
                                <i class="bi bi-eye"></i>
                            </a>

                            <a href="/Vehiculos/Edit/${data}"
                               class="btn btn-warning btn-sm">
                                <i class="bi bi-pencil"></i>
                            </a>

                            <button type="button"
                                class="btn btn-danger btn-sm btnEliminar"
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

$(document).on("click", ".btnEliminar", function () {

    var id = $(this).data("id");

    $("#btnConfirmarEliminar")
        .attr("href", "/Vehiculos/Delete/" + id);

    var modal = new bootstrap.Modal(
        document.getElementById("modalEliminar"));

    modal.show();

});