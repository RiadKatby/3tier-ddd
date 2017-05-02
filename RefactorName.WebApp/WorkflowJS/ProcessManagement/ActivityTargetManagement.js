var index = 0;
var ActivityTargets = [];
var currentActivityTargetId = 0;


//Ready
$(document).ready(function () {
    $('#delete').click(function (e) {
        e.preventDefault();
    });

    for (i = 0; i < model.length; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        var activityTarget = {
            ProcessId: model[i].ProcessId,
            ActivityIndex: index,
            ActivityId: model[i].ActivityId,
            ActivityName: model[i].ActivityName,
            TargetId: model[i].TargetId,
            TargetName: model[i].TargetName,
            GroupId: model[i].GroupId,
            GroupName: model[i].GroupName,
            ActivityTargetId: model[i].ActivityTargetId
        };
        ActivityTargets.push(activityTarget);
        index++;

        var activityTargetRow = Object();
        activityTargetRow['ActivityName'] = model[i].ActivityName;
        activityTargetRow['TargetName'] = model[i].TargetName;
        activityTargetRow['GroupName'] = model[i].GroupName;

        jPut.activityTargetRows.append(activityTargetRow);
    }
})

//Delete Activity Target Client Side
function DeleteActivityTargetRow(ev, index) {

    jPut.activityTargetRows.remove(index);
    var row = ActivityTargets[index - 1];

    if (index > -1) {
        ActivityTargets.splice(index, 1);
    }
    if (ActivityTargets.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    currentActivityTargetId = 0;
    ev.preventDefault();
}

//Edit Activity Target Client Side
function EditActivityTargetRow(ev, index) {

    jPut.activityTargetRows.remove(index);
    var currentActivityTarget = ActivityTargets[index];

    if (index > -1) {
        ActivityTargets.splice(index, 1);
    }
    $('#ActivityId').val(currentActivityTarget.ActivityId);
    $('#TargetId').val(currentActivityTarget.TargetId);
    $("#GroupId").val(currentActivityTarget.GroupId);
    if (ActivityTargets.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    currentActivityTargetId = currentActivityTarget.ActivityTargetId;
    ev.preventDefault();
}

//Save Activity Target Client Side
function Save(ev) {
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    var activityTarget = {
        ProcessId: $("#ProcessId").val(),
        ActivityTargetIndex: index,
        ActivityId: $("#ActivityId").val(),
        TargetId: $("#TargetId").val(),
        GroupId: $("#GroupId").val(),
        ActivityTargetId: currentActivityTargetId
    };
    ActivityTargets.push(activityTarget);
    index++;

    var activityTargetRow = Object();
    activityTargetRow['ActivityName'] = $("#ActivityId option:selected").text();
    activityTargetRow['TargetName'] = $("#TargetId option:selected").text();
    activityTargetRow['GroupName'] = $("#GroupId option:selected").text();

    jPut.activityTargetRows.append(activityTargetRow);
    ev.preventDefault();
    currentActivityTargetId = 0;
    clearForm();
}

//Save Activity Target Server Side
function submitForm() {

    if (!ValidateActivities())
        return;
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: CreateActivityTargetsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(ActivityTargets)
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

//Back to Activity Management
function Back() {

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

//Reload Activity Target
function Reload() {
    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToActivityTargetsURL,
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

//Valisate Activity Targets
function ValidateActivities() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (ActivityTargets.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " Activity Target is Required";
        isValid = false;
    }
    return isValid;
}