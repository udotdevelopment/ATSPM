
window.onload = function () { DisplayRouteApproaches(); };

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
//}

function LoadRouteEdit()
{
    var routeId = $("#ApproachRouteId").val();
    $("#ConfigurationTableHeader").click(function () {
        GetConfigurationTable(routeId);
    })
}

function DisplayApproaches(signalId) {
    var tosend = {};
    tosend.id = signalId;
    $.ajax({
        type: "POST",
        url: urlpathApproaches,
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        headers: GetRequestVerificationTokenObject(),
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#ApproachList').html(data);
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function DisplayRouteApproaches() {
    var tosend = {};
    tosend.id = $("#Route_Id").val();
    $.ajax({
        type: "POST",
        url: urlpathRouteApproaches,
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        headers: GetRequestVerificationTokenObject(),
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#RouteApproachList').html(data);
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });

}

function SetApproach(protectedPhaseNumber, directionTypeID, isProtectedPhaseOverlap, isPrimary, routeSignalId) {
    document.body.style.cursor = 'wait';
    var tosend = {};
    tosend.Phase = protectedPhaseNumber;
    tosend.DirectionTypeID = directionTypeID;
    tosend.IsOverlap = isProtectedPhaseOverlap;
    tosend.IsPrimaryApproach = isPrimary;
    tosend.RouteSignalId = routeSignalId;
    $.ajax({
        type: "POST",
        url: urlpathUpdateApproach,
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        headers: GetRequestVerificationTokenObject(),
        data: JSON.stringify(tosend),
        success: function (data) {
            DisplayRouteApproaches();
            document.body.style.cursor = 'default';
        },
        error: function (req, status, errorObj) {
            document.body.style.cursor = 'default';
            alert("Error");
        }
    });
}