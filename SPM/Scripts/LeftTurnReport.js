$(function (ready) {
    $(".datepicker").attr('type', 'text');
    $("#StartDate").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#EndDate").val($.datepicker.formatDate('mm/dd/yy', new Date()));
    $("#StartEndDaySelector").datepicker({
        onSelect: function (dateText) {
            $("#StartDate").val(dateText);
            $("#EndDate").val(dateText);
        }
    });
    $(".datepicker").datepicker();
});

$( "#StartDate" ).change(function () {
    GetSignalLocation()
})

$('#RunChecks').click(function () { RunChecks(); });

function RunChecks() {
    StartChecksSpinner();
    var signalID = $("#SignalID").val(); 
    var cyclesWithPedCalls = $("#CyclesWithPedCalls").val();
    var cyclesWithGapOuts = $("#CyclesWithGapOuts").val();
    var leftTurnVolume = $("#LeftTurnVolume").val();
    var startDate = $("#StartDate").val();
    var endDate = $("#EndDate").val();
    var DaysOfWeek = getDaysOfWeek();
    GetCheckBoxes();
    if (!signalID) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>Signal Not Found</h3");
        StopChecksSpinner();
    }
    else if (!cyclesWithPedCalls) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>Cycles With Ped Calls Not Found</h3");
        StopChecksSpinner();
    }
    else if (!cyclesWithGapOuts) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>Cycles With Gap Outs Not Found</h3");
        StopChecksSpinner();
    }
    else if (!leftTurnVolume) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>Left Turn Volume Not Found</h3");
        StopChecksSpinner();
    }
    else if (!startDate) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>Start Date Not Found</h3");
        StopChecksSpinner();
    }
    else if (!endDate) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>End Date Not Found</h3");
        StopChecksSpinner();
    }
    else {
        var approachIds = getApproachIds();
        var tosend = {};
        tosend.signalId = signalID;
        tosend.cyclesWithPedCalls = cyclesWithPedCalls;
        tosend.cyclesWithGapOuts = cyclesWithGapOuts;
        tosend.leftTurnVolume = leftTurnVolume;
        tosend.startDate = startDate;
        tosend.endDate = endDate;
        tosend.approachIds = approachIds;
        tosend.DaysOfWeek = DaysOfWeek;
        $.ajax({
            url: urlpathGetSignalDataCheckReport,
            method: 'POST',
            data: tosend,
            traditional: true,
            success: function (data) {
                StopChecksSpinner();
                $('#FinalGapAnalysisPlaceHolder').html(data);
            },
            error: function (data) {
                StopChecksSpinner();
                $('#FinalGapAnalysisPlaceHolder').html(data.responseText);
            }
        });
    }
}

$('#RunReports').click(function () { RunReports(); });

function getApproachIds() {
    var approachIds = [];
    $('input[name="turn.Checked"]:checked').each(function() {
        var approachId = this.attributes.getNamedItem("approachId").value;
        if (approachId != 0) {
            approachIds.push(parseInt(approachId));
        }
    });
    return approachIds;
}

function getDaysOfWeek() {
    var dayIds = [];
    $('input[name="PostedDays.DayIDs"]:checked').each(function () {      
            dayIds.push(parseInt(this.value));
        
    });
    return dayIds;
}

function GetCheckBoxes(){
    var checkboxes = $('input[name="turn.Checked"]:checked');  
    if (!checkboxes) {
        $('#SignalDataCheckPlaceHolder').html("<h3 class='text-danger'>No Approaches Selected</h3");
    }
}

function RunReports() {
    StartReportSpinner();
    
    var form = $("#MainForm")[0];
    if (!$(form).valid()) {
        StopReportSpinner();
        return alert("Please select a signal");
    }
    var timeOptions = $("input:radio[name='TimeOptions']:checked").val();
    var GetGapReport = $('#finalGapAnalysisCheck').is(":checked");
    var GetSplitFail = $('#splitFailAnalysisCheck').is(":checked");
    var GetPedestrianCall = $('#pedestrianCallAnalysisCheck').is(":checked");
    var GetConflictingVolume = $('#conflictingVolumesAnalysisCheck').is(":checked");
    var StartHour;
    var EndHour;
    var StartMinute;
    var EndMinute;
    var GetAMPMPeakHour = false;
    var GetAMPMPeakPeriod = false;
    var Get24HourPeriod = false;
    if (timeOptions == "customTimeRadiobutton") {
        StartHour = $("#StartTimeHour :selected").val();
        var StartAMPM = $("#StartAMPM :selected").val();
        StartHour = parseInt(StartHour);
        if (StartAMPM == "PM") {
            StartHour += 12;
        }
        EndHour = $("#EndTimeHour :selected").val();
        var EndAMPM = $("#EndAMPM :selected").val();
        EndHour = parseInt(EndHour)
        if (EndAMPM == "PM") {
            EndHour += 12;
        }
        var StartMinute = $("#StartTimeMinute :selected").val();
        var EndMinute = $("#EndTimeMinute :selected").val();
    }
    else if (timeOptions == "PeakHourRadiobutton") {
        GetAMPMPeakHour = true;
    }
    else if (timeOptions == "PeakPeriodRadiobutton") {
        GetAMPMPeakPeriod = true;
    }
    else if (timeOptions == "FullDayRadiobutton") {
        Get24HourPeriod = true;
        StartHour = 0;
        EndHour = 24;
        StartMinute = 0;
        EndMinute = 0;
    }

    var signalId = $("#SignalID").val();
    var StartDate = $("#StartDate").val();
    var EndDate = $("#EndDate").val();
    var ApproachIds = getApproachIds();
    var DaysOfWeek = getDaysOfWeek();
    var AcceptableGapPercentage = $("#AcceptableGaps").val() / 100; 
    var AcceptableSplitFailPercentage = $("#CyclesWithSplitFail").val() / 100;
    var tosend = {};
    tosend.SignalId = signalId;
    tosend.ApproachIds = ApproachIds;
    tosend.StartDate = StartDate;
    tosend.EndDate = EndDate;
    tosend.ApproachIds = ApproachIds;
    tosend.DaysOfWeek = DaysOfWeek;
    tosend.StartHour = StartHour;
    tosend.StartMinute = StartMinute;
    tosend.EndHour = EndHour;
    tosend.EndMinute = EndMinute;
    tosend.GetAMPMPeakPeriod = GetAMPMPeakPeriod;
    tosend.GetAMPMPeakHour = GetAMPMPeakHour;
    tosend.Get24HourPeriod = Get24HourPeriod;
    tosend.AcceptableGapPercentage = AcceptableGapPercentage;
    tosend.GetGapReport = GetGapReport;
    tosend.GetSplitFail = GetSplitFail;
    tosend.CyclesWithSplitFail = CyclesWithSplitFail;
    tosend.GetPedestrianCall = GetPedestrianCall;
    tosend.AcceptableSplitFailPercentage = AcceptableSplitFailPercentage;
    tosend.GetConflictingVolume = GetConflictingVolume;
    $.ajax({
        url: urlpathGetFinalGapAnalysisReport,
        method: 'POST',
        data: tosend,
        traditional: true,
       
        success: function (data) {
            StopReportSpinner();
            var result = data.pdfResult;
            $('#FinalGapAnalysisPlaceHolder').html(result.HTML);
            //$('#FinalGapAnalysisPlaceHolder').html("<iframe src=\"http://localhost/spmImages/" + data +
            //    "\"style=\"width:100%; height:600px;\" frameborder=\"0\"></iframe>");
            deleteTempFile(result.FileName);
        },
        error: function (data) {
            StopReportSpinner();
            $('#FinalGapAnalysisPlaceHolder').html(data.responseText);
        }
    });
}

function deleteTempFile(fileName) {
    var tosend = {};
    tosend.fileName = fileName;
    $.ajax({
        url: urlpathDeleteTempPdf,
        method: 'DELETE',
        data: tosend,
        success: function (data) {
        }
    });
}

function customTimeClick() {
    var radiobuttons = document.getElementsByName('TimeOptions'); //$('#TimeOptions');
    for (var i = 0, length = radiobuttons.length; i < length; i++) {
        if (radiobuttons[i].value == 'customTimeRadiobutton') {
            if (radiobuttons[i].checked) {
                $('#customTimeDiv').removeClass('d-none');
            } else {
                $('#customTimeDiv').addClass('d-none');
            }
        }
    }

}


function finalGapAnalysisCheckClick() {
    if ($("#finalGapAnalysisCheck").is(":checked")) {
        $('#AcceptableGapsDiv').removeClass('d-none');
    } else {
        $('#AcceptableGapsDiv').addClass('d-none');
    }

}

function splitFailAnalysisCheckClick() {
    if ($("#splitFailAnalysisCheck").is(":checked")) {
        $('#CyclesWithSplitFailDiv').removeClass('d-none');
    } else {
        $('#CyclesWithSplitFailDiv').addClass('d-none');
    }

}

function pedestrianCallAnalysisCheckClick() {
    if ($("#pedestrianCallAnalysisCheck").is(":checked")) {
        $('#LtCyclesWithPedCallsDiv').removeClass('d-none');
    } else {
        $('#LtCyclesWithPedCallsDiv').addClass('d-none');
    }

}

function SetControlValues(signalID, selectedMetricID) {
    $("#SignalID").val(signalID);
    GetSignalLocation(selectedMetricID);
}

function GetSignalLocation() {
    var tosend = {};
    var signalID = $("#SignalID").val();
    var startDate = $("#StartDate").val();
    tosend.signalID = signalID;
    tosend.date = startDate;
    $.get(urlpathGetLeftTurnCheckBoxes, tosend, function (data) {
        $('#LeftTurnCheckBoxesDiv').html(data);
    });
    $.get(urlpathGetSignalLocation, tosend, function (data) {
        $('#SignalLocation').text(data);
        if (data !== "Signal Not Found") {
            GetMetricsList(signalID, selectedMetricID);
        }
    });
}
$("#ResetDate").click(function () { ResetDates(); });

function ResetDates() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = month + '/' +
        + day + '/' +
        + d.getFullYear();
    $("#StartDate").val(output);
    $("#EndDate").val(output);
}

function StartReportSpinner() {
    $("#RunReportSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopReportSpinner() {
    $("#RunReportSpinner").removeClass("fa fa-circle-o-notch fa-spin");
}

function StartChecksSpinner() {
    $("#RunChecksSpinner").addClass("fa fa-circle-o-notch fa-spin");
}

function StopChecksSpinner() {
    $("#RunChecksSpinner").removeClass("fa fa-circle-o-notch fa-spin");
}


