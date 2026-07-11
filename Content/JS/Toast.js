document.addEventListener("DOMContentLoaded", function () {

    var toastElement = document.getElementById("toastMensaje");

    var toast = new bootstrap.Toast(toastElement, {
        delay: 4000,
        autohide: true
    });

    toast.show();

});