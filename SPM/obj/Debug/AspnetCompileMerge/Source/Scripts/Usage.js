$(function (ready) {
    $(".datepicker").attr('type', 'text');
    $(".datepicker").datepicker();
});

function GetCharts()
{
    var tosend = {};
    tosend.startDate = $("#StartDate").val();
    tosend.endDate = $("#EndDate").val();
    GetMetricsUsage(tosend);
    GetAgencyUsage(tosend);
    GetReportRuns(tosend);
    GetActionsByMetric(tosend);
    GetActionCharts(tosend);
}

function GetActionCharts(tosend)
{
    $.ajax({type:"Get", url:urlGetMetrics, success:function(data)
    {
        $.each(data, function(i, val)
        {
            GetActionsByMetricID(tosend, val.MetricTypeID, val.Abbreviation);
        })
    },
        error:
            function (req, status, errorObj)
            {
                alert(errorObj);
            }
    })
}

function GetActionsByMetricID(tosend, metricTypeID, metricAbbreviation) {
    tosend.metricTypeID = metricTypeID;
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        headers: GetRequestVerificationTokenObject(),
        url: urlActionsByMetricTypeID,
        success: function (data) {
            $("#PlaceHolder_" + metricAbbreviation).html(data + "<hr />");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            $("#PlaceHolder_" + metricAbbreviation).html(status);
        }
    });
}

function GetReportRuns(tosend)
{
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        headers: GetRequestVerificationTokenObject(),
        url: urlReportsRun,
        success: function (data) {
            $("#ReportsRunPlaceHolder").html(data + "<hr />");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            $("#ReportsRunPlaceHolder").html(status);
        }
    });
}

function GetActionsByMetric(tosend) {
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        headers: GetRequestVerificationTokenObject(),
        url: urlActionsByMetric,
        success: function (data) {
            $("#ActionsByMetricPlaceHolder").html(data + "<hr />");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            $("#ActionsByMetricPlaceHolder").html(status);
        }
    });
}

function GetMetricsUsage(tosend) {
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        headers: GetRequestVerificationTokenObject(),
        url: urlMetricsUsage,
        success: function (data) {
            $("#MetricsUsagePlaceHolder").html(data + "<hr />");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            $("#MetricsUsagePlaceHolder").html(status);
        }
    });
}

function GetAgencyUsage(tosend) {
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        headers: GetRequestVerificationTokenObject(),
        url: urlAgencyUsage,
        success: function (data) {
            $("#AgencyUsagePlaceHolder").html(data + "<hr />");
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            500: function (content) { alert('internal server error'); }
        },
        error: function (req, status, errorObj) {
            $("#AgencyUsagePlaceHolder").html(status);
        }
    });
}

function GetRequestVerificationTokenObject() {
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}