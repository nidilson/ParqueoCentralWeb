document.getElementById("btnBuscarPlaca").addEventListener("click", function () {

    const placa = document.getElementById("Placa").value;

    $.ajax({
        url: "/Movimientos/RevisarPlaca",
        type: "GET",
        data: {
            placa: placa
        },
        dataType: "json",
        success: function (response) {

            $("#Propietario").val(response.Propietario);
            $("#TipoVehiculo").val(response.TipoVehiculo);
            $("#IdVehiculo").val(response.IdVehiculo);
            $("#VehiculoNoEncontradoDiv").addClass("visually-hidden")
            $("#VehiculoIngresadoDiv").addClass("visually-hidden")

        },
        error: function (response) {
            $("#Vehiculo_Propietario").val("");
            $("#Vehiculo_TipoVehiculo").val("");
            $("#IdVehiculo").val(0);
            if (response.status == 403) {
                $("#VehiculoIngresadoDiv").removeClass("visually-hidden")
                $("#VehiculoNoEncontradoDiv").addClass("visually-hidden")
            } else {
                $("#VehiculoNoEncontradoDiv").removeClass("visually-hidden")
                $("#VehiculoIngresadoDiv").addClass("visually-hidden")
            }
        }
    });

});