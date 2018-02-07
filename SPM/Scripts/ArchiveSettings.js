function UseArchiveYesOptions() {
    var YesUseArchive = document.getElementById("UseArchiveYes").checked;
    if (YesUseArchive == true) {
        $('#DivOff').removeClass("hidden");
    }
    //else {
    //    $('#DivOff').removeClass("hidden");
    //}
}

function UseArchiveNoOptions() {
    var NoUseArchive = document.getElementById("UseArchiveNo").checked;
    if (NoUseArchive == true) {
        $('#DivOff').addClass("hidden");
    }
}


function PartitionTablesOptions() {
    var thisIsPartitionTable = document.getElementById("IsPartitionTables").checked;
    if (thisIsPartitionTable == true) {
        $('#DivOff').removeClass("hidden");
        $('#DivMonthsToRemoveIndex').removeClass("hidden");
        $('#DivEndTime').addClass("hidden");
    }
}

function NonPartitionTablesOptions() {
    var thisIsNonPartitionTable = document.getElementById("IsNonPartitionTables").checked;
    if (thisIsNonPartitionTable == true) {
        $('#DivOff').removeClass("hidden");
        $('#DivMonthsToRemoveIndex').addClass("hidden");
        $('#DivEndTime').removeClass("hidden");
    }
}

function DeleteOptions() {
    var thisIsDelete = document.getElementById("IsDelete").checked;
    if (thisIsDelete == true) {
        $('#DivMovePath').addClass("hidden");
    }
}

function MoveOptions() {
    var thisIsMove = document.getElementById("IsMove").checked;
    if (thisIsMove == true) {
        $('#DivMovePath').removeClass("hidden");
    }
}