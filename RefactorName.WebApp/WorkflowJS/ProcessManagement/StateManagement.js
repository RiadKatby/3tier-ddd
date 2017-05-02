var count = Object.keys(users).length;
var index = 0;
var states = [];
var stateActivities = [];
var currentStateId = 0;
var currentActivities = [];

//Ready
$(document).ready(function () {

    var container = $('#Activities');
    for (j = 0; j < Object.keys(activities).length; j++) {
        container.append('<li><input type="checkbox" id=chk' + activities[j].Value + ' value="' + activities[j].Value + '" /> ' + activities[j].Text + '<br/></li>');
    }
    for (i = 0; i < model.length ; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        stateActivities = [];
        var activityNames = "";
        for (j = 0; j < model[i].Activities.length; j++) {
            activityNames += model[i].Activities[j].Name;
            activityNames += "-";
            stateActivities.push(
                {
                    key: model[i].Activities[j].ActivityId,
                    value: model[i].Activities[j].Name
                });
        }
        activityNames = activityNames.substring(0, activityNames.length - 1)

        states.push(
            {
                ProcessId: $("#ProcessId").val(),
                index: index,
                Name: model[i].Name,
                Description: model[i].Description,
                StateTypeId: model[i].StateTypeId,
                StateTyepName: model[i].StateTyepName,
                ActivityNames: stateActivities,
                Activities: model[i].Activities,
                StateId: model[i].StateId
            });
        $('#states_table').append('<tr><td>' + model[i].Name + '</td><td>' + activityNames + '</td><td><a href="#" onclick="DeleteStateRow(event,this,' + index + ')" id="delete"><span class="glyphicon glyphicon-remove"></span></a></td><td><a href="#" onclick="EditStateRow(event,this,' + index + ')" id="Edit">Edit</a></td></tr>');
        index++;
    }

})

//Edit State Client Side
function EditStateRow(ev, obj, currentIndex) {
    currntRowIndex = obj.parentNode.parentNode.rowIndex - 1
    var currentState;
    currentState = states[currntRowIndex];
    $(obj).closest('tr').remove();
    $('#StateName').val(currentState.Name);
    $('#StateDescription').val(currentState.Description);
    $("#StateTypeID").val(currentState.StateTypeId);
    currentStateId = currentState.StateId;

    for (i = 0; i < currentState.Activities.length; i++) {
        for (j = 0; j < Object.keys(activities).length; j++) {
            var tempCheckBox = document.getElementById("chk" + activities[j].Value);
            if (currentState.Activities[i].ActivityId == activities[j].Value) {
                tempCheckBox.checked = true;
            }
        }
    }
    states.splice(currntRowIndex, 1);
    ev.preventDefault();
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }

}


function Save(ev) {
    if (!ValidateState())
        return;
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    var ActivityNames = [];
    currentActivities = [];
    var StateActivities = "";
    for (i = 0; i < Object.keys(activities).length; i++) {
        if (document.getElementById("chk" + activities[i].Value).checked) {
            StateActivities += activities[i].Text;
            StateActivities += "-";
            currentActivities.push({
                ActivityId: activities[i].Value,
                Name: activities[i].Text
            });
            ActivityNames.push(
                {
                    key: activities[i].Value,
                    value: activities[i].Text
                });
        }
    }
    StateActivities = StateActivities.substring(0, StateActivities.length - 1);
    states.push(
            {
                ProcessId: $("#ProcessId").val(),
                index: index,
                Name: $('#StateName').val(),
                Description: $('#StateDescription').val(),
                StateTypeId: $("#StateTypeID").val(),
                StateTyepName: $("#StateTypeID option:selected").text(),
                ActivityNames: ActivityNames,
                StateId: currentStateId,
                Activities: currentActivities
            });
    $('#states_table').append('<tr><td>' + $('#StateName').val() + '</td><td>' + StateActivities + '</td><td><a href="#" onclick="DeleteStateRow(event,this,' + index + ')" id="delete"><span class="glyphicon glyphicon-remove"></span></a></td><td><a href="#" onclick="EditStateRow(event,this,' + index + ')" id="edit">Edit</span></a></td></tr>');
    index++;
    ev.preventDefault();
    clearForm();
}

function DeleteStateRow(ev, obj, currentIndex) {

    currntRowIndex = obj.parentNode.parentNode.rowIndex - 1
    states.splice(currntRowIndex, 1);
    $(obj).closest('tr').remove();
    currentStateId = 0;
    ev.preventDefault();
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
}

function submitForm() {

    if (!ValidateStates())
        return;
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: CreateStatesURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(states)
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


function Back() {
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


function Reload() {
    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToStatesURL,
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

function ValidateState() {
    var stateName = $('#StateName').val();
    var stateDescription = $('#StateDescription').val();
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (stateName == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " State Name is required";
        isValid = false;
    }
    if (stateDescription == "") {
        messageContainer.style.display = "inline-block";
        message.textContent += " State Description is required";
        isValid = false;
    }
    if (stateName.length > 100) {
        messageContainer.style.display = "inline-block";
        message.textContent += " State Name must be less than 100 charcter";
        isValid = false;
    }
    if (stateDescription.length > 255) {
        messageContainer.style.display = "inline-block";
        message.textContent += " State Description must be less than 255 charcter";
        isValid = false;
    }
    return isValid;
}


function ValidateStates() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (states.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " State is Required";
        isValid = false;
    }
    return isValid;
}