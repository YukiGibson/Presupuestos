function loading(name) {
    $('#' + name).val('Cargando...');
}
function thousandSeparator(name) {
    if (document.getElementById(name).value !== null) {
        var inputValue = $('#' + name).val();
        var input = inputValue.replace(/[^\d\.]+/g, "");
        if (input.charAt(input.length - 1) !== '.') {
            input = input ? parseFloat(input, 10) : 0;
            $('#' + name).val(function () {
                return input;
            });
        }
    }
}


