function UseArchiveYesOptions() {
    var YesUseArchive = document.getElementById("UseArchiveYes").checked;
    if (YesUseArchive == true) {
        $('#DivOff').removeClass("hidden");
    }
    //else {
    //    $('#DivOff').removeClass("hidden");
    //}
}

function UseArchiveNoOptions() {
    var NoUseArchive = document.getElementById("UseArchiveNo").checked;
    if (NoUseArchive == true) {
        $('#DivOff').addClass("hidden");
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

function NonPartitionTablesOptions() {
    var thisIsNonPartitionTable = document.getElementById("IsNonPartitionTables").checked;
    if (thisIsNonPartitionTable == true) {
        $('#DivOff').removeClass("hidden");
        $('#DivMonthsToRemoveIndex').addClass("hidden");
        $('#DivEndTime').removeClass("hidden");
    }
}

function DeleteOptions() {
    var thisIsDelete = document.getElementById("IsDelete").checked;
    if (thisIsDelete == true) {
        $('#DivMovePath').addClass("hidden");
    }
}

function MoveOptions() {
    var thisIsMove = document.getElementById("IsMove").checked;
    if (thisIsMove == true) {
        $('#DivMovePath').removeClass("hidden");
    }
}

function LoadSignal() {
    var signalId = $("#SignalID").val();
    $.ajax({
        url: urlpathGetSignal + "/" + signalId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#RouteSignals').append(data);
            $.validator.unobtrusive.parse($("#RouteSignals"));
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
            LoadSignal();
        }
    });
}


