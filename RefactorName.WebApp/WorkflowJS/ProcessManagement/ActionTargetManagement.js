var index = 0;
var ActionTargets = [];
var currentActionTargetId = 0;

//Ready
$(document).ready(function () {
    $('#delete').click(function (e) {
        e.preventDefault();
    });
    for (i = 0; i < model.length; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        var actionTarget = {
            ProcessId: model[i].ProcessId,
            ActionIndex: index,
            ActionId: model[i].ActionId,
            ActionName: model[i].ActionName,
            TargetId: model[i].TargetId,
            TargetName: model[i].TargetName,
            GroupId: model[i].GroupId,
            GroupName: model[i].GroupName,
            ActionTargetId: model[i].ActionTargetId
        };
        ActionTargets.push(actionTarget);
        index++;

        var actionTargetRow = Object();
        actionTargetRow['ActionName'] = model[i].ActionName;
        actionTargetRow['TargetName'] = model[i].TargetName;
        actionTargetRow['GroupName'] = model[i].GroupName;

        jPut.actionTargetRows.append(actionTargetRow);
    }
})

//Delete Action Target Client Side
function DeleteActionTargetRow(ev, index) {

    jPut.actionTargetRows.remove(index);
    var row = ActionTargets[index - 1];

    if (index > -1) {
        ActionTargets.splice(index, 1);
    }
    if (ActionTargets.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    currentActionTargetId = 0;
    ev.preventDefault();
}

//Edit Action Target Client Side
function EditActionTargetRow(ev, index) {

    jPut.actionTargetRows.remove(index);
    var currentActionTarget = ActionTargets[index];

    if (index > -1) {
        ActionTargets.splice(index, 1);
    }
    $('#ActionId').val(currentActionTarget.ActionId);
    $('#TargetId').val(currentActionTarget.TargetId);
    $("#GroupId").val(currentActionTarget.GroupId);
    if (ActionTargets.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    currentActionTargetId = currentActionTarget.ActionTargetId;
    ev.preventDefault();
}

//Save Action Target Client Sice
function Save(ev) {
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    var actionTarget = {
        ProcessId: $("#ProcessId").val(),
        ActionTargetIndex: index,
        ActionId: $("#ActionId").val(),
        TargetId: $("#TargetId").val(),
        GroupId: $("#GroupId").val(),
        ActionTargetId: currentActionTargetId
    };
    ActionTargets.push(actionTarget);
    index++;

    var actionTargetRow = Object();
    actionTargetRow['ActionName'] = $("#ActionId option:selected").text();
    actionTargetRow['TargetName'] = $("#TargetId option:selected").text();
    actionTargetRow['GroupName'] = $("#GroupId option:selected").text();

    jPut.actionTargetRows.append(actionTargetRow);
    ev.preventDefault();
    currentActionTargetId = 0;

    clearForm();
}

//Save Action Target Server Side
function SubmitForm() {

    if (!ValidateActions())
        return;
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: CreateActionTargetsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(ActionTargets)
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
//Back to Action Management
function Back() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToActionsURL,
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

//Reload Action Target 
function Reload() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToActionTargetsURL,
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

//Validate Action Targets Client Side
function ValidateActions() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (ActionTargets.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " Action Target is Required";
        isValid = false;
    }
    return isValid;
}
