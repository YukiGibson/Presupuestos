function loading(name) {
    $('#' + name).val('Cargando...');
}
$('document').ready(function () {
    $("input[type='checkbox']").each(function () { this.checked = false; });
});
