function SetControlValues(signalId)
{
    AddSignalToList(signalId);
}

function AddSignalToList(signalId) {
    var tosend = {};
    tosend.signalID = signalId;
    var signalLocation = "";
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        SaveSignalToDatabase(signalId, data);        
    });
}

function SaveSignalToDatabase(signalId, signalLocation) {
    var tosend = {};
    tosend.RouteId = $("#Route_Id").val();
    tosend.SignalId = signalId;
    $.ajax({
        type: "POST",
        url: urlpathCreateRouteSignal,
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        headers: GetRequestVerificationTokenObject(),
        data: JSON.stringify(tosend),
        success: function (data) {
            if (data != "") {
                var signalList = $('#SelectedSignalsList');
                signalList.append(new Option(signalId + " - " + signalLocation, data, true, true));
            }
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function GetRequestVerificationTokenObject() {
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}

function displayRouteInfobox(e) {
    if (e.targetType == 'pushpin') {
        actionArray = new Array();
        var SignalID = e.target.SignalID.toString();

        var tosend = {};
        tosend.signalID = SignalID;
        $.ajax({
            url: urlpathSignalInfoBox,
            type: "POST",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(tosend),
            success: function (data) {
                if (infobox != null) {
                    infobox.setMap(null);
                }
                infobox = new Microsoft.Maps.Infobox(e.target.getLocation(), { offset: new Microsoft.Maps.Point(-100, 0), htmlContent: data });
                infobox.setMap(map);
            }
        });
    }
}