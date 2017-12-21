
function GetRecordCount() {
    var toSend = GetValuesToSend();
    $.ajax({
        type: "Get",
        cache: false,
        async: true,
        data: toSend,
        url: urlpathGetRecordCount,
        success: function (data) { $('#GenerateData').html(data); },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert(content.responseText); }
        },
        error: function (req, status, errorObj) {
            alert("Error");
        }
    });
}

function GetValuesToSend() {
    var toSend = {};
    toSend.SignalId = $('#SignalId').val();
    toSend.StartDate = $('#StartDate').val();
    toSend.EndDate = $('#EndDate').val();
    toSend.EventParams = $('#EventParams').val();
    toSend.EventCodes = $('#EventCodes').val();
    return toSend;
}