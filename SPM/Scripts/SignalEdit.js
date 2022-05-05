function SetDatePicker() {
    $(".datepicker").attr('type', 'text');
    $(".datepicker").datepicker({ constrainInput: false });
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
function CreateJsonArray(formArray) {
    var obj = {};
    $.each(formArray, function (i, pair) {
        var cObj = obj, pObj, cpName;
        $.each(pair.name.split("."), function (i, pName) {
            pObj = cObj;
            cpName = pName;
            cObj = cObj[pName] ? cObj[pName] : (cObj[pName] = {});
        });
        pObj[cpName] = pair.value;
    });
    return obj;
}

function AddNewVersion() {
    var signalId = $("#editSignalID").val();
    //var formData = $("#form0").serializeArray();
    //var jsonForm = JSON.stringify(CreateJsonArray(formData));
    //$.ajax({
    //    url: urlpathGetSignalEdit,
    //    headers: GetRequestVerificationTokenObject(),
    //    type: "POST",
    //    cache: false,
    //    dataType: 'json',
    //    data: formData,
    //    async: true,
    //    success: function (data) {
    //    },
    //    onerror: function () { alert("Error"); }
    //});
    var newVersionId;
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        //headers: GetRequestVerificationTokenObject(),
        //data: jsonForm,
        //    dataType: 'json',
        url: urlpathCopyVersion + "/" + signalId,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            newVersionId = data;
        },
        complete:function() {
            LoadVersionByVersionID(newVersionId);
            SetDatePicker();
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            alert(req.responseText);
        }
    });
}

function DeleteVersion() {

    if ($('#VersionID').children('option').length < 2) {
        alert("Unable to delete version. You must have more than one version to delete.");
    }
    else {
        var signalID = $("#SignalID").val();
        var versionId = $("#VersionID option:selected").val();
        var versionDescription = $("#VersionID option:selected").text();
        var parameters = {};
        parameters.ID = versionId;
        if (confirm("Are you sure you want to delete the version " + versionDescription + " ?")) {
            $.ajax({
                type: "POST",
                cache: false,
                async: true,
                headers: GetRequestVerificationTokenObject(),
                data: JSON.stringify({ "versionId": versionId }),
                url: urlpathDeleteVersion,
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                success: function(data) {
                    $('#SignalEdit').html(data);

                },
                statusCode: {
                    404: function(content) { alert('cannot find resource'); },
                    500: function(content) { alert('internal server error'); }
                },
                error: function(req, status, errorObj) {
                    alert("Error");
                }
            });
        }
    }
}

function DeleteSignal() {
    var signalID = $("#SignalID").val();

    if (confirm("Are you sure you want to delete signal " + signalID + " ?")) {
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify({ "Id": signalID }),
            url: urlpathDeleteSignal,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function(data){window.location.reload(false)},
                //(data) { $('#ConfigurationTableCollapse').html(data); },
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


function GetCreateComment() {
    var signalID = $("#SignalID").val();
   
    var versionId =  $("#VersionID").val();
    var metricPath = urlpathCreateMetricComments + '/' + versionId;

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
//function GetConfigurationTable(signalID) {
    function GetConfigurationTable() {
    var signalID = $("#SignalID").val();
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

function GetConfigurationTableForVersion(versionId) {

    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: { "VersionID": versionId },
        url: urlpathGetConfigurationTableForVersion,
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

function UpdateVersionDropdown()
{
    var selIndex = $("#VersionID option:selected").index();
    var dd = document.getElementById('VersionID');
    var oldVersionDescription = $("#VersionID option:selected").text();
    var note = $("#Note").val();
    var date = $("#End").val();
    var newVersionDescription = date + " - " + note;
    dd.options[selIndex].text = newVersionDescription;
}

function PostCreateComment() {
    var tosend = {};
    
    tosend.VersionID =  $("#VersionID").val();
    tosend.SignalID = $("#SignalID").val();
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
        success: function(data) {
            $('#SignalEdit').html(data);
            SetSignalID(newSignalID);
        },
        complete: function () {
            var startDate = $("#Start")[0].defaultValue;
            SetDatePicker();
            $("#Start").val(startDate);
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
    var signalID = $("#SignalID").val();
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
   // parameters.id = $("#SignalID").val();
    parameters.versionId =  $("#VersionID").val();
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
    var channelUniqueArray = unique(channelArray);
    return (channelArray.length != channelUniqueArray.length);    
}

function unique(array) {
    return $.grep(array, function (el, index) {
        return index == $.inArray(el, array);
    });
}

function CopyDetector(ID, approachID) {
    var signalID = $("#SignalID").val();
    var versionId =  $("#VersionID").val();
    var approachIndex = $("#Index" + approachID).val();
    var metricPath = urlpathCreateDetector;
    $.ajax({
        type: "Post",
        cache: false,
        async: true,
        headers: GetRequestVerificationTokenObject(),
        data: { "ID": ID, "versionId": versionId, "approachID": approachID, "approachIndex": approachIndex },
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
    var signalID = $("#SignalID").val();
    var versionId =  $("#VersionID").val();
        var metricPath = urlpathCreateApproach;
        $.ajax({
            type: "POST",
            cache: false,
            async: true,
            data: { "versionId": versionId },
            headers: GetRequestVerificationTokenObject(),
            url: metricPath,
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
    tosend.SignalID = $("#SignalID").val();
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
        var versionID =  $("#VersionID").val();
        var approachIndex = $("#Index"+approachID).val();
        var tosend = {};
        tosend.signalID = $("#SignalID").val();
        tosend.versionId = versionID;
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

function SyncText(e, approachID) {
    $(".mph_" + approachID).each(function() {
        $(this).val($(e).val());
    });
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
        $(mph).removeClass("invisible");
    }
    else if (!$(control1).is(':checked') && !$(control2).is(':checked'))
    {
        $(mph).addClass("invisible");
    }
}

function ShowHideControl(obj, checked) {
    if (checked) {
        if ($(obj).hasClass("invisible")) {
            $(obj).removeClass("invisible");
        }
    }
    else {
        if (!$(obj).hasClass("invisible")) {
            $(obj).addClass("invisible");
        }
    }
}

function UpdatePedsare1to1() {
    var pedchecked = document.getElementById('Pedsare1to1-value');
    var pedphases = document.getElementsByClassName('ped-phase-value');
    var protphases = document.getElementsByClassName('protected-phase-value');
    var peddetectors = document.getElementsByClassName('ped-detectors-string');
    var pedoverlap = document.getElementsByClassName('ped-overlap-checkbox');
    //if All Peds are 1:1 is checked, then disable ped fields and set ped phase equal to protected phase
    if (pedchecked.checked) {
        for (var i = 0; i < pedphases.length; i++) {
            pedphases[i].setAttribute("readonly", "readonly");
            pedphases[i].value = protphases[i].value;
            peddetectors[i].setAttribute("readonly", "readonly");
            peddetectors[i].value = protphases[i].value;
            //pedoverlap[i].setAttribute("readonly", "readonly");
            pedoverlap[i].checked = false;
            //pedoverlap[i].addClass("greycheckbox");
        }
    }
    //if All Peds are 1:1 is not checked, then enable editing for ped fields and make them blank
    else {
        for (var i = 0; i < pedphases.length; i++) {
            pedphases[i].removeAttribute("readonly");
            pedphases[i].value = "";
            peddetectors[i].removeAttribute("readonly");
            peddetectors[i].value = "";
            //pedoverlap[i].removeAttribute("readonly");
            pedoverlap[i].checked = false;
        }
    }
}

function CheckboxReadOnly() {
    var pedchecked = document.getElementById('Pedsare1to1-value');
    var pedoverlap = document.getElementsByClassName('ped-overlap-checkbox');
    if (pedchecked.checked) {
       // return false;
        for (var i = 0; i < pedoverlap.length; i++) {
            pedoverlap[i].checked = false;
        }
    }
}