$("#id_rola").change(function (e) {
    if ($("#id_rola option:selected").text() === "recepcja" || $("#id_rola option:selected").text() === "admin" || $("#id_rola option:selected").text() === "super_admin"  )
    {
  
        $('#availOptions option').prop('selected', true);
        var selectedOpts = $('#availOptions option:selected');

        $('#selectedOptions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    }

    if ($("#id_rola option:selected").text() !== "recepcja" || $("#id_rola option:selected").text() !== "admin" || $("#id_rola option:selected").text() !== "super_admin" ) {

        console.log("Zmieniono");
        $('#selectedOptions option').prop('selected', true);
        var selectedOpts = $('#selectedOptions option:selected');

        $('#availOptions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    }
});



$('#btnRmv').click(function (e) {
    var selectedOpts = $('#selectedOptions option:selected');
    if (selectedOpts.length == 0) {
        alert('Brak obiektu do przeniesienia.');
        e.preventDefault();
    }


    $('#availOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});


$('#btnAdd').click(function (e) {
    var selectedOpts = $('#availOptions option:selected');
    if (selectedOpts.length == 0) {
        alert('Brak obiektu do przeniesienia.');
        e.preventDefault();
    }


    $('#selectedOptions').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnSubmit').click(function (e) {
    $('#selectedOptions option').prop('selected', true);
});

