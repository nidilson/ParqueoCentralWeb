$(function () {

    $('#tblMovimientos').DataTable({

        processing: true,
        serverSide: true,
        responsive: true,
        searching: true,
        ordering: true,

        ajax: {
            url: "/Movimientos/ObtenerMovimientos",
            type: "POST"
        },

        columns: [

            {
                data: "Placa"
            },

            {
                data: "CodigoEspacio"
            },

            {
                data: "FechaHoraEntrada",
                render: function (data) {

                    return formatoHora(data)
                }
            },

            {
                data: "FechaHoraSalida",
                render: function (data) {
                    if(data != null)
                        return formatoHora(data)

                    return ''
                }
            },

            {
                data: "MontoCobrado",
                render: function (data) {
                    if (data != null)
                        return "₡ " + data.toLocaleString("es-CR", {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2
                        })

                    return ''
                }
            },

            {
                data: "EstadoMovimiento",
                render: function (data) {

                    if (data === "En uso")
                        return '<span class="badge bg-success">En uso</span>';

                    return '<span class="badge bg-secondary">Finalizado</span>';
                }
            },

            {
                data: "UsuarioRegistro"
            },

            {
                data: null,
                orderable: false,
                searchable: false,
                className: "text-center",

                render: function (data) {

                    var botones =
                        `<a href="/Movimientos/Details/${data.IdMovimiento}"
                            class="btn btn-info btn-sm me-1">
                            <i class="bi bi-eye"></i>
                        </a>`;

                    if (data.EstadoMovimiento === "En uso") {

                        botones +=
                            `<a href="/Movimientos/Salida/${data.IdMovimiento}"
                                class="btn btn-success btn-sm">
                                <i class="bi bi-box-arrow-right"></i>
                            </a>`;
                    }

                    return botones;
                }
            }

        ],

        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json"
        }

    });

    function formatoHora(hora) {
        var fecha = new Date(parseInt(hora.substr(6)));

        var valor =
            fecha.getFullYear() + "-" +
            String(fecha.getMonth() + 1).padStart(2, '0') + "-" +
            String(fecha.getDate()).padStart(2, '0') + "T" +
            String(fecha.getHours()).padStart(2, '0') + ":" +
            String(fecha.getMinutes()).padStart(2, '0');

        return valor;
    }

});