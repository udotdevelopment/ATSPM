$(function (ready) {
    $("#collapseTwo").removeClass("show");
    $("#collapseTwoLink").addClass("collapsed");
});
function LoadSignal(signalID) {
    $.ajax({
        url: urlpathGetSignalDetail + "/" + signalID,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#SignalResult').html(data);           
            GetConfigurationTable();
        },
        onerror: function () { alert("Error"); }
    });
}

function GetConfigurationTable() {
    var signalID = $("#SignalID").val();
    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: { "SignalID": signalID },
        url: urlpathGetConfigurationTable,
        success: function (data) { $('#ConfigurationTableCollapse').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);
    GetSignalLocation();
}

function GetSignalLocation() {
    var signalID = $("#SignalID").val();
    var tosend = {};
    tosend.signalID = signalID;
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        $('#SignalLocation').text(data);
        if (data == "Signal Not Found") {
            $('#SignalEdit').html("");
        } else {
            LoadSignal(signalID);
        }
    });
}