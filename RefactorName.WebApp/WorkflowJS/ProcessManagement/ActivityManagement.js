var index = 0;
var Activities = [];
var currentActivityId = 0;
$('#delete').click(function (e) {
    e.preventDefault();
});

//Ready
$(document).ready(function () {

    for (i = 0; i < model.length; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        var activity = {
            ProcessId: model[i].ProcessId,
            ActivityIndex: index,
            Name: model[i].Name,
            ActivityTypeID: model[i].ActivityTypeID,
            ActivityTyepName: model[i].ActivityTypeName,
            Description: model[i].Description,
            ActivityId: model[i].ActivityId
        };
        Activities.push(activity);
        index++;

        var activityRow = Object();
        activityRow['ActivityName'] = model[i].Name;
        activityRow['ActivityTypeName'] = model[i].ActivityTypeName;

        jPut.activityRows.append(activityRow);
    }
})

//Delete Activity Client Side
function deleteActivityRow(ev, index) {

    jPut.activityRows.remove(index);
    var row = Activities.findIndex(x => x.index == index);

    if (index > -1) {
        Activities.splice(index, 1);
    }
    if (Activities.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    ev.preventDefault();
    currentActivityId = 0;
}

//Edit Activity Client Side
function editActivityRow(ev, index) {

    jPut.activityRows.remove(index);
    var row = Activities[index - 1];
    var currentActivity = Activities[index];
    if (index > -1) {
        Activities.splice(index, 1);
    }
    $('#ActivityName').val(currentActivity.Name);
    $('#ActivityDescription').val(currentActivity.Description);
    $("#ActivityTypeID").val(currentActivity.ActivityTypeID);
    if (Activities.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    ev.preventDefault();
    currentActivityId = currentActivity.ActivityId;
}

//Save Activity Client Side
function Save(ev) {

    if (!ValidateActivity())
        return;
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    var activity = {
        ProcessId: $("#ProcessId").val(),
        ActivityIndex: index,
        Name: $("#ActivityName").val(),
        ActivityTypeID: $("#ActivityTypeID").val(),
        ActivityTyepName: $("#ActivityTypeID option:selected").text(),
        Description: $("#ActivityDescription").val(),
        ActivityId: currentActivityId
    };
    Activities.push(activity);
    index++;

    var activityRow = Object();
    activityRow['ActivityName'] = $('#ActivityName').val();
    activityRow['ActivityTypeName'] = $("#ActivityTypeID option:selected").text();

    jPut.activityRows.append(activityRow);
    ev.preventDefault();
    clearForm();
    currentActivityId = 0;
}

//Save Activity Server Side
function submitForm() {

    if (!ValidateActivities())
        return;

    document.getElementById("dialog_loader").style.display = "inline-block";

    $.ajax({
        type: "Post",
        url: CreateActivitiesURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(Activities)
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);

    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

    });

}

//Back To Group Management
function Back() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToGroupsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: tempProcessID
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);
        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";
        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    });
}

//Reload Activity Page
function Reload() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToActivitiesURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: tempProcessID
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);
        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    });
}

//Validate Activity
function ValidateActivity() {
    var activityName = $('#ActivityName').val();
    var activityDescription = $('#ActivityDescription').val();
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (activityName == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " Activity Name is required";
        isValid = false;
    }
    if (activityDescription == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " Activity Description is required";
        isValid = false;
    }
    if (activityName.length > 100) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Activity Name must be less than 100 charcter";
        isValid = false;
    }
    if (activityDescription.length > 255) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Activity Description must be less than 255 charcter";
        isValid = false;
    }
    return isValid;
}

//Validate Activities
function ValidateActivities() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (Activities.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " Activity is Required";
        isValid = false;
    }
    return isValid;
}