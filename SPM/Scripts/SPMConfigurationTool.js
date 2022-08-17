$(function (ready) {
    $("#collapseTwo").removeClass("show");
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
            //HideBasicCheckBoxes();
            $("#ConfigurationTableHeader").click(function () {
                GetConfigurationTable(signalID);
            });
            $.validator.unobtrusive.parse($("#SignalEdit"));
        },
        complete: function() {
            RemoveHiddenInputFromCheckboxes();
            var startDate = $("#Start")[0].defaultValue;
            SetDatePicker();
            $("#Start").val(startDate);

        },
        onerror: function () { alert("Error"); }
    });

}

function RemoveHiddenInputFromCheckboxes() {
    $("input[type='hidden'][name$=Overlap]").remove();
}

function LoadVersionByVersionID(vId) {
   
   
    $.ajax({
        url: urlpathGetVersionEdit + "/" + vId,
        type: "GET",
        cache: false,
        async: true,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#SignalEdit').html(data);
            $("#SignalConfigurationCollapseOne").addClass("in");
            //SetDatePicker();
            //HideBasicCheckBoxes();
            $("#ConfigurationTableHeader").click(function () {
                GetConfigurationTableForVersion(vId);
            });
            $.validator.unobtrusive.parse($("#SignalEdit"));
        },
        complete: function() {
            RemoveHiddenInputFromCheckboxes();
            var startDate = $("#Start")[0].defaultValue;
            SetDatePicker();
            $("#Start").val(startDate);
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

function CheckboxReadOnly() {
    var pedchecked = document.getElementById('Pedsare1to1-value');
    var pedoverlap = document.getElementsByClassName('ped-overlap-checkbox');
    if (pedchecked.checked) {
        for (var i = 0; i < pedoverlap.length; i++) {
            pedoverlap[i].checked = false;
        }
    }
}