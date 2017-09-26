function GetConfigurationTable(routeID) {

    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: { "RouteID": routeID },
        url: "/ApproachRouteDetails/Edit",
        success: function (data) { $('#ApproachRouteDetailsCollapse').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function LoadRouteEdit()
{
    var routeId = $("#ApproachRouteId").val();
    $("#ConfigurationTableHeader").click(function () {
        GetConfigurationTable(routeId);
    })
}