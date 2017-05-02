var index = 0;
var Actions = [];
var currentActionId = 0;

//Ready
$(document).ready(function () {
    for (i = 0; i < model.length; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        var action = {
            ProcessId: model[i].ProcessId,
            ActionIndex: index,
            Name: model[i].Name,
            ActionTypeID: model[i].ActionTypeID,
            ActionTyepName: model[i].ActionTypeName,
            Description: model[i].Description,
            ActionId: model[i].ActionId
        };
        Actions.push(action);
        index++;

        var actionRow = Object();
        actionRow['ActionName'] = model[i].Name;
        actionRow['ActionTypeName'] = model[i].ActionTypeName;

        jPut.actionRows.append(actionRow);
    }
})
//Delate Action Client Side
function DeleteActionRow(ev, index) {

    jPut.actionRows.remove(index);
    var row = Actions[index];

    if (index > -1) {
        Actions.splice(index, 1);
    }
    if (Actions.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    ev.preventDefault();
}

//Edit Action Client Side
function EditActionRow(ev, index) {
    jPut.actionRows.remove(index);
    var row = Actions[index];
    var currentAction = Actions[index];
    if (index > -1) {
        Actions.splice(index, 1);
    }
    $('#ActionName').val(currentAction.Name);
    $('#ActionDescription').val(currentAction.Description);
    $("#ActionTypeID").val(currentAction.ActionTypeID);
    if (Actions.length == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    ev.preventDefault();
    currentActionId = currentAction.ActionId;
}

//Save Action Client Side
function Save(ev) {

    if (!ValidateAction())
        return;
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    var action = {
        ProcessId: $("#ProcessId").val(),
        ActionIndex: index,
        Name: $("#ActionName").val(),
        ActionTypeID: $("#ActionTypeID").val(),
        ActionTyepName: $("#ActionTypeID option:selected").text(),
        Description: $("#ActionDescription").val(),
        ActionId: currentActionId
    };
    Actions.push(action);
    index++;

    var actionRow = Object();
    actionRow['ActionName'] = $('#ActionName').val();
    actionRow['ActionTypeName'] = $("#ActionTypeID option:selected").text();

    jPut.actionRows.append(actionRow);
    ev.preventDefault();
    clearForm();
    currentActivityId = 0;
}

//Save Action Server Side
function SubmitForm() {

    if (!ValidateActions())
        return fal;
    document.getElementById("dialog_loader").style.display = "inline-block";

    $.ajax({
        type: "Post",
        url: CreateActionsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(Actions)
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

//Back to Activity Targets
function Back() {
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

//Reload Page
function Reload() {
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

//Validate Action Client Side
function ValidateAction() {
    var actionName = $('#ActionName').val();
    var actionDescription = $('#ActionDescription').val();
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (actionName == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " Action Name is required";
        isValid = false;
    }
    if (actionDescription == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " Action Description is required";
        isValid = false;
    }
    if (actionName.length > 100) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Action Name must be less than 100 charcter";
        isValid = false;
    }
    if (actionDescription.length > 255) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Action Description must be less than 255 charcter";
        isValid = false;
    }
    return isValid;
}

//Validate Actions Client Side
function ValidateActions() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (Actions.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " Action is Required";
        isValid = false;
    }
    return isValid;
}