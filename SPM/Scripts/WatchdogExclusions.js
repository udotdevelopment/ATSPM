$(document).ready(function() {
    GetExclusions();
});

function GetExclusions() {
    var tosend = {};
    $.ajax({
        url: urlFillExclusions,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function(data) {
            $('#WatchdogExclusionsListContainer').html(data);
        },
        onerror: function() { alert("Error"); }

    });
}

function addExclusionOnClick() {
    var tosend = {};
    var sigID = $('#AddSignalIDBox').val();
    var phaID = $('#AddSignalPhaseBox').val();
    var typ = $('#AddSignalReportSelectBox').val();
    tosend.signalID = sigID;
    tosend.phaseID = phaID;
    tosend.type = typ;
    $.ajax({
        url: urlAddExclusion,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function(result) {
            $('#WatchdogExclusionsListContainer').html(result);
        }
    });
};