function LoadRoute() {
    var RouteId = $("#SelectedRouteID").val();
    $.ajax({
        url: urlpathGetRouteSignals + "/" + RouteId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#RouteSignals').html(data);
            $.validator.unobtrusive.parse($("#RouteSignals"));
        },
        onerror: function () { alert("Error"); }
    });
}