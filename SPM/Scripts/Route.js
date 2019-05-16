

$(function (ready) {

    $("#Routes").change(function () {
        GetSignals();        
    });
    $('input[type=date]').each(function () {
        this.type = "text";
    });
    if (/Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor)) {
        Date.prototype.addDays = function (days) {
            var dat = new Date(this.valueOf());
            dat.setDate(dat.getDate() + days);
            return dat;
        }
        var today = new Date();
        $("#StartDate").val($.datepicker.formatDate('mm/dd/yy', today.addDays(-1)));
        $("#EndDate").val($.datepicker.formatDate('mm/dd/yy', today.addDays(-1)));
    }
    $('.datepicker').datepicker({ dateFormat: 'mm/dd/yy' }); //Initialise any date pickers    
    
});

function GetSignals()
{
    var routeId = $("#Routes").val();
    $.ajax({
        url: urlpathSignals + '/' + routeId,
        success: function (data) { $('#signalsPlaceHolder').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    //$.get(urlpathSignals + '/' + routeId, function (data) {
    //    $('#signalsPlaceHolder').html(data);
    });
}

function StartReportSpinner() {
    $("#RunReportSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("fa fa-circle-o-notch fa-spin");
}

function StartPCDsReportSpinner() {
    $("#PCDRunReportSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopPCDsReportSpinner() {    
    $("#PCDRunReportSpinner").removeClass("fa fa-circle-o-notch fa-spin");
}

function GetPCDOptions(signalId, downSignalId, upstreamDirection, downstreamDirection,
    delta) {
    var postedDays = {};
    postedDays.DayIDs = [];
    $("input[id^='PostedDays_DayIDs']").each(function (i, obj) {
        if ($(obj).is(":checked")) {
            postedDays.DayIDs.push(obj.value);
        }
    });
    //if ($("#PostedDays_DayIDs0").val())
    //{
    //    postedDays.DayIDs.push("0"); 
    //}
    //if ($("#PostedDays_DayIDs1").val()) {
    //    postedDays.DayIDs.push("1");
    //}
    //if ($("#PostedDays_DayIDs2").val()) {
    //    postedDays.DayIDs.push("2");
    //}
    //if ($("#PostedDays_DayIDs3").val()) {
    //    postedDays.DayIDs.push("3");
    //}
    //if ($("#PostedDays_DayIDs4").val()) {
    //    postedDays.DayIDs.push("4");
    //}
    //if ($("#PostedDays_DayIDs5").val()) {
    //    postedDays.DayIDs.push("5");
    //}
    //if ($("#PostedDays_DayIDs6").val()) {
    //    postedDays.DayIDs.push("6");
    //}

    var tosend = {};
    var startDt = $("#StartDate").val() + ' ' + $("#StartTime").val() + ' ' + $("#StartAMPM").val();
    var endDt = $("#EndDate").val() + ' ' + $("#EndTime").val() + ' ' + $("#EndAMPM").val();
    tosend.SelectedApproachRouteId = $("#Routes").val();
    tosend.StartDate = startDt;
    tosend.EndDate = endDt;
    tosend.StartTime = $("#StartTime").val();
    tosend.EndTime = $("#EndTime").val();
    tosend.StartAMPM = $("#StartAMPM").val();
    tosend.EndAMPM = $("#EndAMPM").val();
    tosend.StartingPoint = $("#StartingPoint").val();
    tosend.CycleLength = $("#CycleLength").val();
    tosend.Bias = $("#Bias").val();
    tosend.BiasUpDownStream = $("#BiasUpDownStream").val();
    tosend.PostedDays = postedDays;
    tosend.SelectedSignalId = signalId;
    tosend.SelectedDownSignalId = downSignalId;
    tosend.SelectedUpstreamDirection = upstreamDirection;
    tosend.SelectedDownstreamDirection = downstreamDirection;
    tosend.SelectedDelta = delta;
    $.ajax({
        url: urlpathPCDOptions,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#PCDOptionsPlaceHolder').html(data);
            $('#SelectedStartDate')[0].selectedIndex = 0;
            $('#SelectedStartDate').focus();
            $.validator.unobtrusive.parse($("#PCDOptionsPlaceHolder"));
        },
        error: function (xhr, status, error) {
            alert(xhr.responseText);
        }

    });
}

function GetPCDs(signalId, downSignalId, upstreamDirection, downstreamDirection,
    delta) {
    var tosend = {};
    tosend.SignalId = signalId;
    tosend.DownSignalId = downSignalId;
    tosend.DownDirection = downstreamDirection;
    tosend.UpstreamDirection = upstreamDirection;
    tosend.Delta = delta;
    tosend.YAxis = $("#uxYAxisTextBox").val();
    tosend.SelectedStartDate = $("#uxDatesListbox :selected").val();
    tosend.SelectedEndDate = $("#EndDate").val() + ' ' + $("#EndTime").val() + ' ' + $("#EndAMPM").val();
    StartPCDsReportSpinner();
    $.ajax({
        url: urlpathPCDs,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#PCDsPlaceHolder').html(data);
        }
    });
}

function AdjustOffset() {
    var table = document.getElementById("AdjustmentTable");
    var rowCount = table.rows.length;
    var cycleLength = $("#CycleLength").val();

    var cumulativeChange = 0;
    for (var i = rowCount - 1; i >= 1; i--) {
        //Get the offset
    var offset = cumulativeChange +parseInt($(table.rows[i].cells[4]).find('input').val());
        //add it to column 6
    if (offset >= cycleLength) {
        var tempOffset = offset;
        while (tempOffset >= cycleLength)
        {
            tempOffset = tempOffset - cycleLength;
        }
        table.rows[i].cells[5].innerText = tempOffset;
    }
    else
    {
        table.rows[i].cells[5].innerText = offset;
    }
        //Get the new offset
    var newOffset = offset + parseInt($(table.rows[i].cells[6]).find('input').val());
        //Check to make sure that the new offset isn't greater than the cycle length and then set it to column 8
    if (newOffset > cycleLength) {
        var tempNewOffset = newOffset;
        while (tempNewOffset > cycleLength) {
            tempNewOffset = tempNewOffset - cycleLength;
        }
        table.rows[i].cells[7].innerText = tempNewOffset;
        }
    else {
        table.rows[i].cells[7].innerText = newOffset;
        }

        //update the cumulative change
        cumulativeChange = offset;
    }
}

function ClearReport()
{
    $('#PCDsPlaceHolder').html('');
    $('#PCDOptionsPlaceHolder').html('');
    $('#ReportPlaceHolder').html('');
}

