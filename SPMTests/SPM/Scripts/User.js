function AddRoleToUser(userName) {
    var tosend = {};
    tosend.roleName = $("#RoleName").val();
    tosend.userName = userName;
    $.ajax({
            type: "POST",
            url: urlpathAddRoleToUser,
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            headers: GetRequestVerificationTokenObject(),
            data: JSON.stringify(tosend),
            success: function (data) {
                $('#EditRoleResult').html("");
                $('#EditRoleResult').html(data);
                $('#RolesContainer').append("<li id = '" + tosend.roleName + "'>" + tosend.roleName + "</li>");
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

function RemoveRoleFromUser(userName) {
    var tosend = {};
    tosend.roleName = $("#RoleName").val();
    tosend.userName = userName;
    $.ajax({
        type: "POST",
        url: urlpathRemoveRoleFromUser,
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        headers: GetRequestVerificationTokenObject(),
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#EditRoleResult').html("");
            $('#EditRoleResult').html(data);
            $('#'+tosend.roleName).remove();
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

function GetRequestVerificationTokenObject()
{
    var headers = {};
    var token = $('[name=__RequestVerificationToken]').val();
    headers['__RequestVerificationToken'] = token;
    return headers;
}
