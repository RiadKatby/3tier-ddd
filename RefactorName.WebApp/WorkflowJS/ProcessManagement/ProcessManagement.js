//Properties   

var process = [];
var index = 0;

$body = $("#dialog_loader");
//Ready
$(document).ready(function () {

    var container = $('#Users');
    for (i = 0; i < Object.keys(users).length; i++) {
        var isAdmin = "";
        for (j = 0; j < model.Admins.length; j++) {
            if (model.Admins[j].UserID == Object.keys(users)[i]) {
                isAdmin = "checked='true'";
            }
        }
        container.append('<li><input type="checkbox" id=chk' + Object.keys(users)[i] + ' value="' + Object.keys(users)[i] + '" ' + isAdmin + '" /> ' + Object.values(users)[i] + '<br/></li>');
    }

})

//Save Clent Side
function Save() {
    if (!ValidateProcess())
        return;
    var processAdmins = [];
    var userNames = "";
    for (i = 0; i < Object.keys(users).length; i++) {
        if (document.getElementById("chk" + Object.keys(users)[i]).checked) {
            userNames += Object.values(users)[i];
            if (i != (Object.keys(users).length - 1))
                userNames += "-";
            processAdmins.push(
                {
                    key: Object.keys(users)[i],
                    value: Object.values(users)[i]
                });
        }
    }
    process.push(
            {
                Name: $('#processName').val(),
                UserNames: processAdmins,
                Admins: model.Admins,
                ProcessId: $('#ProcessId').val()
            });
    submitForm();
}

//Save server side
function submitForm() {

    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: CreateProcessURL,//"CreateProcess",
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(process)
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);


    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";
        process = [];
    });
}

//Validate Data Client Side
function ValidateProcess() {
    var processName = $('#processName').val();
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");


    if (processName == "") {
        messageContainer.style.display = "inline-block";
        message.textContent = "Process Name is required";
        isValid = false;
    }
    if (processName.length > 100) {
        messageContainer.style.display = "inline-block";
        message.textContent = "Process Name must be less than 100 charcter";
        isValid = false;
    }
    return isValid;
}
