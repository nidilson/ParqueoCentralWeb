document.getElementById("btnBuscarPlaca").addEventListener("click", function () {

    const placa = document.getElementById("Placa").value;

    $.ajax({
        url: "/Movimientos/ObtenerMovimiento",
        type: "GET",
        data: {
            placa: placa
        },
        dataType: "json",
        success: function (response) {

            $("#IdMovimiento").val(response.IdMovimiento);
            $("#IdVehiculo").val(response.IdVehiculo);
            $("#Propietario").val(response.Propietario);
            $("#TipoVehiculo").val(response.TipoVehiculo);
            $("#IdEspacio").val(response.IdEspacio);
            $("#CodigoEspacio").val(response.CodigoEspacio);
            $("#FechaHoraEntrada").val(formatoHora(response.FechaHoraEntrada));
            $("#FechaHoraSalida").val(formatoHora(response.FechaHoraSalida));
            $("#TiempoTranscurrido").val(formatoTiempo(response.TiempoParqueado));
            $("#MontoCobradoSpan").text("₡ " + response.MontoCobrado.toLocaleString("es-CR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }));
            $("#MontoCobrado").val(response.MontoCobrado);

            $("#VehiculoNoEncontradoDiv").addClass("visually-hidden")
            $("#botonRegistrar").prop("disabled", false)

        },
        error: function (response) {
            $("#IdMovimiento").val(0);
            $("#IdVehiculo").val(0);
            $("#Propietario").val("");
            $("#TipoVehiculo").val("");
            $("#IdEspacio").val(0);
            $("#CodigoEspacio").val("");
            $("#HoraEntrada").val("");
            $("#FechaHoraSalida").val("");
            $("#TiempoTranscurrido").val("");
            $("#MontoCobrado").val("");

            $("#VehiculoNoEncontradoDiv").removeClass("visually-hidden")

            $("#botonRegistrar").prop("disabled", true)
            
        }
    });

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
function formatoTiempo(horasCant) {
    var horas = Math.floor(horasCant)
    var minutos = Math.floor((horasCant - horas) * 60)

    return `${horas}h ${minutos}m`;
}
