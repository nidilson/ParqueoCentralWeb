$(document).ready(function () {

    $('#tblParqueos').DataTable({

        processing: true,
        serverSide: true,
        filter: true,
        ordering: false,

        ajax: {
            url: '/Home/ObtenerMovimientos',
            type: 'POST'
        },

        columns: [
            {
                data: 'Placa'
            },
            {
                data: 'CodigoEspacio'
            },
            {
                data: 'HoraEntrada'
            },
            {
                data: 'Estado',
                render: function (data) {

                    if (data === "En uso") {
                        return '<span class="badge bg-success">En uso</span>';
                    }

                    return '<span class="badge bg-secondary">Finalizado</span>';
                }
            }
        ],

        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json'
        }

    });

});