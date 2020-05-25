
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
}
function MoveUp() {
    MoveSignalUp();
    $('#SelectedSignalsList option:selected:first-child').prop("selected", false);
    before = $('#SelectedSignalsList option:selected:first').prev();
    $('#SelectedSignalsList option:selected').detach().insertBefore(before);
}
function MoveDown() {
    MoveSignalDown();
    $('#SelectedSignalsList option:selected:last-child').prop("selected", false);
    after = $('#SelectedSignalsList option:selected:last').next();
    $('#SelectedSignalsList option:selected').detach().insertAfter(after);
}
function Remove() {
    var routeSignalId = $('#SelectedSignalsList option:selected').val();
    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: { "id": routeSignalId },
        url: urlpathDeleteSignal,
        success: function (data) { $('#DeleteConfirmation').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function DeleteRouteSignal() {
    var routeSignalId = $('#SelectedSignalsList option:selected').val();
    $.ajax({
        type: "Post",
        cache: false,
        async: true,
        data: { "id": routeSignalId },
        headers: GetRequestVerificationTokenObject(),
        url: urlpathDeleteSignal,
        success: function(data) {
             $('#DeleteConfirmation').html(data);
             $('#SelectedSignalsList option:selected').remove();
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function MoveSignalUp() {
    var signalId = $('#SelectedSignalsList option:selected').val();
    var routeId = $('#Route_Id').val();
    $.ajax({
        type: "Post",
        cache: false,
        async: true,
        data: { "routeId": routeId, "signalId": signalId },
        headers: GetRequestVerificationTokenObject(),
        url: urlpathMoveSignalUp,
        success: function (data) {
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function MoveSignalDown() {
    var signalId = $('#SelectedSignalsList option:selected').val();
    var routeId = $('#Route_Id').val();
    $.ajax({
        type: "Post",
        cache: false,
        async: true,
        data: { "routeId": routeId, "signalId": signalId },
        headers: GetRequestVerificationTokenObject(),
        url: urlpathMoveSignalDown,
        success: function (data) {
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function CancelDelete() {
    $('#DeleteConfirmation').html('');
}

function LoadRouteEdit()
{
    var routeId = $("#ApproachRouteId").val();
    $("#ConfigurationTableHeader").click(function() {
        GetConfigurationTable(routeId);
    });
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

