$(function (ready) {
    $("#SignalHeader").click(function() {
        GetSignals();
    });
    //$.validator.unobtrusive.parse($("#SignalSearchContainer"));
    $.validator.unobtrusive.parse($('form'));
});
function AddEventsForSignalSearch() {
    $("#FilterButton").click(function() { GetSignals(1); });
    $("#ClearFilterButton").click(function() {
        ClearSignalSearch();
    });
}


function ClearSignalSearch() {
    $("#Filters").val(null);
    $("#FilterCriteria").val(null);
    GetSignals(1);
}

//$("#SignalID").keypress(function (e) {
//    if (e.which == 13) {
//        e.preventDefault();
//        GetSignalLocation();
//    }
//});

function SignalIdPress(e) {
    if (e.which == 13) {
        e.preventDefault();
        GetSignalLocation();
    }
}

function GetSignals(page) {
    var filterType = $("#Filters").val();
    var filterCriteria = $("#FilterCriteria").val();
    var tosend = {};
    tosend.page = page;
    tosend.filterType = filterType;
    tosend.filterCriteria = filterCriteria;
    $.ajax({
        url: urlpathFillSignals,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#SignalsPlaceHolder').html(data);
            AddEventsForSignalSearch();
        },
        onerror: function () { alert("Error"); }

    });
}

