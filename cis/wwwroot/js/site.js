// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function selectedChoicePop() {
    const selectedChoice = document.getElementById("choicePop");
    if (selectedChoice.value === "1") {
        redirectTo("/People/PID");
    } else if (selectedChoice.value === "2") {
        redirectTo("/People/NAME");
    }   else if (selectedChoice.value === "3") {
        redirectTo("/People/OLDNAME");
    }
}

function formname(nameindex) {
    document.getElementById("nameindex").value = nameindex;
    const form = document.getElementById("formname");
    form.submit();
}