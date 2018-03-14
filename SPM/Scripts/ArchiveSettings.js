$(function(ready) {
    ShowArchiveSettings();
    ShowDeleteMoveOptions();
});
function ShowArchiveSettings() {
    var archiveEnabled = $("#DatabaseArchiveSettings_EnableDatbaseArchive").is(":checked");;
    if (archiveEnabled == true) {
        $('#EnabledArchiveSettings').removeClass("hidden");
    }
    else {
        $('#EnabledArchiveSettings').addClass("hidden");
    }
}

function RemoveSignal(signalId) {
    $.ajax({
        url: urlpathRemoveSignal + "/" + signalId,
        type: "POST",
        headers: GetRequestVerificationTokenObject(),
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#'+signalId).remove();
        },
        onerror: function () { alert("Error"); }
    });
}

function ShowDeleteMoveOptions() {
    var deleteOrMove = $("#DatabaseArchiveSettings_SelectedDeleteOrMove").val();
    if (deleteOrMove == 2) {
        $('#MoveOption').removeClass("hidden");
        var tableType = $("#DatabaseArchiveSettings_SelectedTableScheme").val();
        if (tableType == 2) {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').removeClass("hidden");
        } else {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').addClass("hidden");
        }
    }
    else {
        $('#MoveOption').addClass("hidden");
        var tableType = $("#DatabaseArchiveSettings_SelectedTableScheme").val();
        if (tableType == 2) {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').removeClass("hidden");
        } else {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').addClass("hidden");
        }
    }
}

function PartitionTablesOptions() {
    var thisIsPartitionTable = document.getElementById("IsPartitionTables").checked;
    if (thisIsPartitionTable == true) {
        $('#DivOff').removeClass("hidden");
        $('#DivMonthsToRemoveIndex').removeClass("hidden");
        $('#DivEndTime').addClass("hidden");
    }
}


function LoadExclusionSignal() {
    var signalId = $("#SignalID").val();
    $.ajax({
        url: urlpathAddSignal + "/" + signalId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#SignalExclusionList').append(data);
            $.validator.unobtrusive.parse($("#ExcludedSignals"));
        },
        onerror: function () { alert("Error"); }
    });
}


function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);
    GetSignalLocation(selectedMetricID);
}

function GetSignalLocation(selectedMetricID) {
    if (selectedMetricID == null) {
        var metricsList = $("#MetricsList");
        if (metricsList != null) {
            selectedMetricID = metricsList.val();
        }
    }
    var signalID = $("#SignalID").val();
    var tosend = {};
    tosend.signalID = signalID;
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        $('#SignalLocation').text(data);
        if (data != "Signal Not Found") {
            LoadExclusionSignal();
        }
    });
}

function GetRequestVerificationTokenObject() {
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}

