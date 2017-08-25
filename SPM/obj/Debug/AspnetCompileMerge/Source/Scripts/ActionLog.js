function PostCreateActionLog() {
    var tosend = {};
    tosend.Name = $("#Name").val();
    tosend.Date = $("#Date").val();
    tosend.SignalID = $("#SignalID").val();
    tosend.AgencyID = $("#AgencyID").val();
    tosend.Comment = $("#Comment").val();
    tosend.CheckBoxListReturnActions = [];
    $("[name='CheckBoxListReturnActions']").each(function () {
        if (this.checked) {
            tosend.CheckBoxListReturnActions.push($(this).val());
        }
    });
    tosend.CheckBoxListReturnMetricTypes = [];
    $("[name='CheckBoxListReturnMetricTypes']").each(function () {
        if (this.checked) {
            tosend.CheckBoxListReturnMetricTypes.push($(this).val());
        }
    });
    if (tosend.CheckBoxListReturnActions.length == 0) {
        alert("You must select an action type.");
    }
    else if (tosend.CheckBoxListReturnMetricTypes.length == 0) {
        alert("You must select a metric type.");
    }
    else {
        $.ajax({
            type: "POST",
            url: urlActionLogCreate,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify(tosend),
            success: function (data) {
                $('#ResultPlaceHolder').html(data);
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                500: function (content) { alert('internal server error'); }
            },
            error: function (req, status, errorObj) {
                $('#ResultPlaceHolder').html(data);
            }
        });
    }
}

function GetRequestVerificationTokenObject() {
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}