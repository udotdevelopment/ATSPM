function SetDatePicker() {
    $(".datepicker").attr('type', 'text');
    $(".datepicker").datepicker();
}

function SaveSuccesful()
{
    $("#ActionMessage").text("Save Successful! " + new Date().toLocaleString());
}
function NewActionMessage(message) {
    $("#ActionMessage").html(message);
}
function ClearActionMessage() {
    $("#ActionMessage").text("");
}
function SaveError() {
    $("#ActionMessage").text("Save Failed!");
}

function GetCreateComment() {
    var signalID = $("#editSignalID").val();
    var metricPath = urlpathCreateMetricComments +'/' + signalID;

    $.ajax({
        url: metricPath,
        success: function (data) { $('#NewComment').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}
function GetConfigurationTable(signalID){
   
    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data:{"SignalID":signalID},
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

function PostCreateComment() {
    var tosend = {};
    tosend.SignalID = $("#editSignalID").val();
    tosend.CommentText = $("#CommentText").val();
    tosend.MetricIDs = [];
    $("[name='MetricIDs']").each(function () {
        if (this.checked) {
            tosend.MetricIDs.push($(this).val());
        }
    });
    if (tosend.MetricIDs.length == 0) {
        alert("You must select a metric type.");
    }
    else {
        $.ajax({
            type: "POST",
            url: urlpathCreateMetricComments,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify(tosend),
            success: function (data) {
                $('#NewComment').html("");
                $('#AddedComment').html(data + $('#AddedComment').html());
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
}

function GetCreateDetectorComment(ID) {
    var metricPath = urlpathCreateDetectorComments+'/' + ID;

    $.ajax({
        url: metricPath,
        success: function (data) { $('#NewDetectorComment_' + ID).html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function PostCreateDetectorComment(ID) {
    var tosend = {};
    tosend.ID = ID;
    tosend.CommentText = $("#CommentText").val();
    $.ajax({
            type: "POST",
            url: urlpathCreateDetectorComments,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify(tosend),
            success: function (data) {
                $('#NewDetectorComment_' + tosend.ID).html("");
                $('#AddedDetectorComment_' + tosend.ID).html(data + $('#AddedDetectorComment_' + tosend.ID).html());
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

function CreateNewSignal() {
    var newSignalID = prompt("Please enter the new SignalID", "123456");
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        url: urlpathCreateSignal + "/" + newSignalID,
        headers: GetRequestVerificationTokenObject(),
        success: function (data) {
            $('#SignalEdit').html(data);
            SetSignalID(newSignalID);
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
        }
    });
}
function SetSignalID(newSignalID)
{
    $("#SignalID").val(newSignalID);
}

function CopySignal() {    
    var signalID = $("#editSignalID").val();
    var newSignalID = prompt("Please enter the new SignalID", "123456");

    var parameters = {};
    parameters.ID = signalID;
    parameters.newID = newSignalID;
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        headers: GetRequestVerificationTokenObject(),
        data:JSON.stringify({"id":signalID,"newID":newSignalID}),
        url: urlpathCopySignal,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#SignalEdit').html(data);
            SetSignalID(newSignalID);
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

function CopyApproach(approachID) {
    var parameters = {};
    parameters.approachID = approachID;
    parameters.id = $("#editSignalID").val();
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(parameters),
        headers: GetRequestVerificationTokenObject(),
        url: urlpathCopyApproach,
        success: function (data) {
            //NewActionMessage("Approach Copy Successful! " + new Date().toLocaleString());
            LoadSignalEdit(SignalID = $("#SignalID").val());
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            NewActionMessage(status + new Date().toLocaleString());
        }
    });
}

function CheckForDuplicatChannels()
{
    if(IsDuplicateChannel())
    {
        alert("This is a duplicate channel");
    }
}

function IsDuplicateChannel() {
    var channelArray=[];
    $(".detectorChannel").each(function (i, obj) {
        channelArray.push(obj.value);        
    });
    var channelUniqueArray = channelArray.slice();
    $.uniqueSort(channelUniqueArray);
    return (channelArray.length != channelUniqueArray.length);    
}

function CopyDetector(ID, approachID) {
    var signalID = $("#editSignalID").val();
    var approachIndex = $("#Index" + approachID).val();
    var metricPath = urlpathCreateDetector;
    $.ajax({
        type: "Post",
        cache: false,
        async: true,
        headers: GetRequestVerificationTokenObject(),
        data: { "ID": ID,"signalID": signalID, "approachID": approachID, "approachIndex": approachIndex },
        url: urlpathCopyDetector,
        success: function (data) {
            $('#DetectorsList_' + approachID).append(data);
            NewActionMessage("Detector Copy Successful! " + new Date().toLocaleString());
            $(".datepicker").attr('type', 'text');
            $(".datepicker").datepicker();      
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert(status);
        }
    });
}

function GetRequestVerificationTokenObject()
{
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}

function CreateNewApproach() {
    var signalID = $("#editSignalID").val();
        var metricPath = urlpathCreateApproach;
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            //data: { "id": signalID },
            headers: GetRequestVerificationTokenObject(),
            url: metricPath + "/" + signalID,
            success: function (data) { $('#ApproachesList').append(data); },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert(content.responseText); }
            },
            error: function (req, status, errorObj) {
            }
        });
    }


function PostCreateApproach() {
    var tosend = {};
    tosend.SignalID = $("#editSignalID").val();
    tosend.DirectionTypeID= $("#DirectionTypeID").val();
    tosend.Description = $("#Description").val();
    tosend.MPH = $("#MPH").val();
    tosend.DecisionPoint = $("#DecisionPoint").val();
    tosend.MovementDelay = $("#MovementDelay").val();
    $.ajax({
            type: "POST",
            url: urlpathCreateApproach,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify(tosend),
            success: function (data) {
                $('#NewApproach').html("");
                $('#AddedApproach').html(data + $('#AddedComment').html());
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
            }
        });
    
}

function DeleteDetector(detectorId, ID) {
    if (confirm(
        "Are you sure you want to delete detector " +
        detectorId + "?")) {
        $.ajax({
            type: "POST",
            url: urlpathDeleteDetector+ "/" + ID,
            cache: false,
            async: true,
            datatype: "json",
            headers: GetRequestVerificationTokenObject(),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                LoadSignalEdit(SignalID = $("#editSignalID").val());
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
            }
        });
    }
}

function DeleteApproach(approachId, approachDescription) {
    if (confirm(
        "Are you sure you want to delete approach " +
        approachDescription + " and all items associated with it?")) {
        $.ajax({
            type: "POST",
            url: urlpathDeleteApproach + "/" + approachId,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            success: function (data) {
                LoadSignalEdit(SignalID = $("#editSignalID").val());
                //$('#ApproachConfiguration_' + approachId).html("");
                
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
            }
        });
    }
}

function GetCreateNewDetector(approachID) {
        var approachIndex = $("#Index"+approachID).val();
        var tosend = {};
        tosend.signalID = $("#editSignalID").val();
        tosend.approachID = approachID;
        tosend.approachIndex = $("#Index" + approachID).val();
        $.ajax({
                type: "POST",
                cache: false,
                async: true,
                datatype: "json",
                url: urlpathCreateDetector,
                headers: GetRequestVerificationTokenObject(),
                data: tosend,
                success: function (data) {
                    $('#DetectorsList_' + approachID).append(data);
                    $(".datepicker").attr('type', 'text');
                    $(".datepicker").datepicker();
                },
                statusCode: {404: function (content) {alert('cannot find resource'); },
                            500: function (content) { alert(content.responseText); }},
                error: function (req, status, errorObj) {
                }
                });
}

function SyncText(e, approachID)
{
    $(".mph_" + approachID).each(function () {
        $(this).val($(e).val());
    })
}

function ShowHideDetectionTypeOptions(e,ID) {
    if ($(e).val() == 2) {
        $(".DetectionTypes_"+ID).each(function (i, obj) {
            if (obj.value == 3) {
                ShowHideMPH(e, obj, ID);
            }
        });
        $(".PCD_" + ID).each(function (i, obj) {
            ShowHideControl(obj, e.checked);
        });
    }
    else if ($(e).val() == 3) {
        $(".DetectionTypes_" + ID).each(function (i, obj) {
            if (obj.value == 2) {
                ShowHideMPH(e, obj, ID);
            }
        });
        $(".Speed_" + ID).each(function (i, obj) {
            ShowHideControl(obj, e.checked);
        });
    }
    //else if ($(e).val() == 4 || $(e).val() == 5 || $(e).val() == 6) {
    //    $(".Lanes_" + ID).each(function (i, obj) {
    //        ShowHideControl(obj, e.checked);
    //    });       
    //}
}

function ShowHideMPH(control1, control2, ID)
{
    var mph = $("#MPHDiv_" + ID);
    if ($(control1).is(':checked') || $(control2).is(':checked'))
    {
        $(mph).removeClass("hidden");
    }
    else if (!$(control1).is(':checked') && !$(control2).is(':checked'))
    {
        $(mph).addClass("hidden");
    }
}

function ShowHideControl(obj, checked) {
    if (checked) {
        if ($(obj).hasClass("hidden")) {
            $(obj).removeClass("hidden");
        }
    }
    else {
        if (!$(obj).hasClass("hidden")) {
            $(obj).addClass("hidden");
        }
    }
}