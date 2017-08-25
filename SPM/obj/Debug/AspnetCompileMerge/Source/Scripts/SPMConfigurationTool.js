$(function (ready) {
    $("#collapseTwo").removeClass("in");
});

function LoadSignalEdit(signalID) {
    $.ajax({
        url: urlpathGetSignalEdit + "/" + signalID,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#SignalEdit').html(data);
            $("#SignalConfigurationCollapseOne").addClass("in");
            SetDatePicker();
            //HideBasicCheckBoxes();
            $("#ConfigurationTableHeader").click(function () {
                GetConfigurationTable(signalID);
            });
            $.validator.unobtrusive.parse($("#SignalEdit"));
        },
        onerror: function () { alert("Error"); }
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
        if(data == "Signal Not Found"){        
            $('#SignalEdit').html("");
        } else {
            LoadSignalEdit(signalID);            
        }
    });    
}



function DeleteSignal(signalID) {
    if (confirm(
           "Are you sure you want to delete signal " +
           signalID + "?")) {
        var tosend = {};
        var token = $('[name=__RequestVerificationToken]').val();
        var headers = {};
        headers['__RequestVerificationToken'] = token;
        $.ajax({
            url: urlpathDeleteSignal + "/" + signalID,
            type: "POST",
            cache: false,
            async: true,
            datatype: "json",
            headers: headers,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#SignalEdit').html(data);
            },
            onerror: function () { alert("Error"); }
        });
    }
}