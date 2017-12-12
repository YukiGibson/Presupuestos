$('document').ready(function () {
    $("input[type='checkbox']").each(function () { this.checked = false; });
    for (var i = 0; i < 7; i++) {
        if ($('#month_' + i + '__value').val() !== null) {
            thousandSeparator("month_" + i + "__value");
        }
    }
    $(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
    $('#SortButton').on('click', function () {
        var $btn = $(this).button('loading');
    });
    $('#SubmitButton').on('click', function () {
        var $btn = $(this).button('loading');
    });
});
function thousandSeparator(name) {
    if (document.getElementById(name).value !== null) {
        var inputValue = $('#' + name).val();
        var input = inputValue.replace(/[^\d\.]+/g, "");
        if (input.charAt(input.length - 1) !== '.') {
            input = input ? parseFloat(input, 10) : 0;
            $('#' + name).val(function () {
                return input === 0 ? 0 : input.toLocaleString("en-US", { maximumFractionDigits: 2 });
            });
        }
    }
}