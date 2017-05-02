
var index = 0;
var transitions = [];
var transitionActivities = [];
var transitionActions = [];
var currentTransitionId = 0;
var currentActivities = [];
var CurrentStateId;
var CurrentState;
var NextStateId;
var NextState;
var currentTransition = []
var actionNames = "";
var activityNames = "";

$(document).ready(function () {
    $("#btnSave").on("click", Save);

    var container = document.getElementById('StatesDiv');
    var result = "<table border=1  class='table table-striped table-bordered table-hover table-condensed'><thead><tr><th></th>";
    for (i = 0; i < Object.keys(states).length; i++) {
        var content = '<th>' + Object.values(states)[i] + '</th>'
        result += content;
    }
    result += "</tr></thead><tbody>";
    for (j = 0; j < Object.keys(states).length; j++) {
        result += "<tr>";
        var content = '<td>' + Object.values(states)[j] + '</td>'
        result += content
        for (k = 0; k < Object.keys(states).length; k++) {
            if (j == k) {
                content = '<input disabled type="checkbox"></input> ';
            }
            else {
                content = '<input onclick="changeStatus(id);" type="checkbox" id="chk' + Object.keys(states)[j] + ',' + Object.keys(states)[k] + '" value="' + Object.keys(states)[j] + ',' + Object.keys(states)[k] + '" ></input> ';
            }
            result += "<td>" + content + "</td>";
        }
        result += "</tr>";
    }
    result += "</tbody></table>";
    result += '<a class="btn" data-popup-open="popup-1" href="#">Actions and Activities</a>';
    container.innerHTML = (result);


    for (i = 0; i < model.length ; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        var tempTransitionActions = [];
        var tempTransitionActivities = [];
        actionNames = "";
        activityNames = "";
        for (j = 0; j < model[i].TransitionActivities.length; j++) {
            activityNames += model[i].TransitionActivities[j].Name;
            activityNames += "-";
            tempTransitionActivities.push(
                {
                    key: model[i].TransitionActivities[j].ActivityId,
                    value: model[i].TransitionActivities[j].Name
                });
        }
        for (j = 0; j < model[i].TransitionActions.length; j++) {
            actionNames += model[i].TransitionActions[j].Name;
            actionNames += "-";
            tempTransitionActions.push(
                {
                    key: model[i].TransitionActions[j].ActionId,
                    value: model[i].TransitionActions[j].Name
                });
        }
        activityNames = activityNames.substring(0, activityNames.length - 1)
        actionNames = actionNames.substring(0, actionNames.length - 1)

        transitions.push(
                {
                    ProcessId: $("#ProcessId").val(),
                    index: index,
                    CurrentStateId: model[i].CurrentStateId,
                    CurrentStateName: model[i].CurrentState.Name,
                    NextStateName: model[i].NextState.Name,
                    NextStateId: model[i].NextStateId,
                    ActionNames: tempTransitionActions,
                    TransitionActivities: tempTransitionActivities,
                    ActivityNames: tempTransitionActivities,
                    TransitionActions: tempTransitionActions,
                    TransitionId: model[i].TransitionId
                });
        $('#Transition_table').append('<tr><td>' + model[i].CurrentState.Name + '</td><td>' + model[i].NextState.Name + '</td><td>' + actionNames + '</td><td>' + activityNames + '</td><td><a href="#" onclick="DeleteTransitionRow(event,this,' + index + ')" id="delete">Delete</a></td><td><a href="#" onclick="EditTransitionRow(event,this,' + index + ')" id="edit">Edit</a></td></tr>');
        index++;
    }

})


function changeStatus(id) {
    currentTransition = document.getElementById(id);
    if (currentTransition.checked) {
        CurrentStateId = currentTransition.value.split(",")[0];

        NextStateId = currentTransition.value.split(",")[1];
    }
    else {
        CurrentStateId = "";
        CurrentState = "";
        NextStateId = "";
        NextState = "";
    }
    for (j = 0; j < Object.keys(states).length; j++) {
        for (k = 0; k < Object.keys(states).length; k++) {
            if (j != k) {
                if (id != "chk" + Object.keys(states)[j] + "," + Object.keys(states)[k]) {
                    if (currentTransition.checked) {
                        document.getElementById("chk" + Object.keys(states)[j] + "," + Object.keys(states)[k]).disabled = true
                    }
                    else {
                        document.getElementById("chk" + Object.keys(states)[j] + "," + Object.keys(states)[k]).disabled = false
                    }
                }
                else if (id == "chk" + Object.keys(states)[j] + "," + Object.keys(states)[k]) {
                    if (currentTransition.checked) {
                        CurrentState = Object.values(states)[j];
                        NextState = Object.values(states)[k];
                    }
                    else {
                        CurrentState = '';
                        NextState = '';
                    }
                }
            }
        }
    }
}


$(function () {
    //----- OPEN
    $('[data-popup-open]').on('click', function (e) {
        var containerActions = document.getElementById('Actions');
        var containerActivities = document.getElementById('Activities');
        var contentActions = '';
        var contentActivities = '';
        for (i = 0; i < Object.keys(actions).length; i++) {
            contentActions += ('<li><input type="checkbox" id=chkActions' + Object.keys(actions)[i] + ' value="' + Object.keys(actions)[i] + '" /> ' + Object.values(actions)[i] + '<br/></li>');
        }


        for (i = 0; i < Object.keys(activities).length; i++) {
            contentActivities += ('<li><input type="checkbox" id=chkActivities' + Object.keys(activities)[i] + ' value="' + Object.keys(activities)[i] + '" /> ' + Object.values(activities)[i] + '<br/></li>');
        }

        containerActions.innerHTML = contentActions;
        containerActivities.innerHTML = contentActivities;

        for (i = 0; i < transitionActions.length; i++) {
            document.getElementById("chkActions" + transitionActions[i].key).checked = true
        }
        for (i = 0; i < transitionActivities.length; i++) {
            document.getElementById("chkActivities" + transitionActivities[i].key).checked = true
        }

        var targeted_popup_class = jQuery(this).attr('data-popup-open');
        $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);

        e.preventDefault();
    });

    //----- CLOSE
    $('[data-popup-close]').on('click', function (e) {
        actionNames = "";
        for (i = 0; i < Object.keys(actions).length; i++) {
            if (document.getElementById("chkActions" + Object.keys(actions)[i]).checked) {
                actionNames += Object.values(actions)[i];
                actionNames += "-";
                transitionActions.push(
                    {
                        key: Object.keys(actions)[i],
                        value: Object.values(actions)[i]
                    });
            }
        }
        actionNames = actionNames.substring(0, actionNames.length - 1);
        activityNames = "";
        for (i = 0; i < Object.keys(activities).length; i++) {
            if (document.getElementById("chkActivities" + Object.keys(activities)[i]).checked) {
                activityNames += Object.values(activities)[i];
                activityNames += "-";
                transitionActivities.push(
                    {
                        key: Object.keys(activities)[i],
                        value: Object.values(activities)[i]
                    });
            }
        }
        activityNames = activityNames.substring(0, activityNames.length - 1);
        var targeted_popup_class = jQuery(this).attr('data-popup-close');
        $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);

        e.preventDefault();
    });
});

function Save() {

    if (!ValidateTransition())
        return;

    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";
    transitions.push(
                {
                    ProcessId: $("#ProcessId").val(),
                    index: index,
                    CurrentStateId: CurrentStateId,
                    CurrentStateName: CurrentState,
                    NextStateName: NextState,
                    NextStateId: NextStateId,
                    ActionNames: transitionActions,
                    ActivityNames: transitionActivities
                });
    $('#Transition_table').append('<tr><td>' + CurrentState + '</td><td>' + NextState + '</td><td>' + actionNames + '</td><td>' + activityNames + '</td><td><a href="#" onclick="DeleteTransitionRow(event,this,' + index + ')" id="delete">Delete</a></td><td><a href="#" onclick="EditTransitionRow(event,this,' + index + ')" id="edit">Edit</a></td></tr>');
    index++;
    currentTransitionStates = CurrentStateId + ',' + NextStateId
    document.getElementById("chk" + currentTransitionStates).checked = false;
    changeStatus("chk" + currentTransitionStates);

    clearForm();

    currentTransition = []
    currentTransitionId = 0
    CurrentStateId = 0
    CurrentState = ''
    NextStateId = 0
    NextState = ''
    transitionActions = []
    transitionActivities = []
    actionNames = "";
    activityNames = "";
}

function DeleteTransitionRow(ev, obj, currentIndex) {

    currntRowIndex = obj.parentNode.parentNode.rowIndex - 1
    $(obj).closest('tr').remove();
    transitions.splice(currntRowIndex, 1);
    ev.preventDefault();
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
}

function EditTransitionRow(ev, obj, currentIndex) {

    currntRowIndex = obj.parentNode.parentNode.rowIndex - 1
    $(obj).closest('tr').remove();
    currentTransition = transitions[currntRowIndex]
    transitions.splice(currntRowIndex, 1);
    currentTransitionId = currentTransition.TransitionId
    CurrentStateId = currentTransition.CurrentStateId
    CurrentState = currentTransition.CurrentStateName
    NextStateId = currentTransition.NextStateId
    NextState = currentTransition.NextStateName
    transitionActions = currentTransition.ActionNames
    transitionActivities = currentTransition.ActivityNames

    currentTransitionStates = currentTransition.CurrentStateId + ',' + currentTransition.NextStateId


    clearForm();
    var checks = document.querySelectorAll('#StatesDiv input[type="checkbox"]');
    for (var i = 0; i < checks.length; i++) {
        var check = checks[i];
        if (check.value == currentTransitionStates) {
            check.disabled = false;
            check.checked = true;
        }
        else {
            check.disabled = true;
            check.checked = false;
        }
    }
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
}

function submitForm() {

    if (!ValidateTransitions())
        return;
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: CreateTransitionsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(transitions)
    }).done(function (data) {

        window.location = '/ProcessManagement/ProcessManagement/Index/';

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
        url: BackToStatesURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: tempProcessID
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);

    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

    });
}
function Reload() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToTransitionsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: tempProcessID
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);

    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

    });
}
function ValidateTransition() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (CurrentStateId == undefined) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Current State is required";
        isValid = false;
    }
    if (NextStateId == undefined) {
        messageContainer.style.display = "inline-block";
        message.textContent += " Next State is required";
        isValid = false;
    }

    return isValid;
}
function ValidateTransitions() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    message.textContent = "";

    if (transitions.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = " Transition is Required";
        isValid = false;
    }
    return isValid;
}