$(function (ready) {
    $("#SignalID").rules("remove", "required")
    ShowArchiveSettings();
    ShowDeleteMoveOptions();
});
function ShowArchiveSettings() {
    var archiveEnabled = $("#DatabaseArchiveSettings_EnableDatbaseArchive").is(":checked");;
    if (archiveEnabled == true) {
        $('#EnabledArchiveSettings').removeClass("invisible");
    }
    else {
        $('#EnabledArchiveSettings').addClass("invisible");
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
        $('#MoveOption').removeClass("invisible");
        var tableType = $("#DatabaseArchiveSettings_SelectedTableScheme").val();
        if (tableType == 2) {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').removeClass("invisible");
        } else {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').addClass("invisible");
        }
    }
    else {
        $('#MoveOption').addClass("invisible");
        var tableType = $("#DatabaseArchiveSettings_SelectedTableScheme").val();
        if (tableType == 2) {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').removeClass("invisible");
        } else {
            $('#DatabaseArchiveSettings_DeleteForStandardTable').addClass("invisible");
        }
    }
}

function PartitionTablesOptions() {
    var thisIsPartitionTable = document.getElementById("IsPartitionTables").checked;
    if (thisIsPartitionTable == true) {
        $('#DivOff').removeClass("invisible");
        $('#DivMonthsToRemoveIndex').removeClass("invisible");
        $('#DivEndTime').addClass("invisible");
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

