function OffOptions() {
    var thisIsOff = document.getElementById("IsOff").checked;
    if (thisIsOff == true) {
        $('#DivOff').addClass("hidden");
    }
    //else {
    //    $('#DivOff').removeClass("hidden");
    //}
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