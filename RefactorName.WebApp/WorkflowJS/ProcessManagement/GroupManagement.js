//Properties
var count = Object.keys(users).length;
var index = 0;
var groups = [];
var groupUsers = [];
var currentGroupId = 0;
var currentMembers = [];

//Ready
$(document).ready(function () {
    var container = $('#Users');
    for (i = 0; i < Object.keys(users).length; i++) {
        container.append('<li><input type="checkbox" id=chk' + Object.keys(users)[i] + ' value="' + Object.keys(users)[i] + '" /> ' + Object.values(users)[i] + '<br /></li>');
    }
    for (i = 0; i < model.length ; i++) {
        document.getElementById("grid_div").style.visibility = "visible";
        document.getElementById("DivNoData").style.visibility = "hidden";
        groupUsers = [];
        var userNames = "";
        for (j = 0; j < model[i].Memebers.length; j++) {
            userNames += model[i].Memebers[j].FullName;
            userNames += "-";
            groupUsers.push(
            {
                key: model[i].Memebers[j].UserID,
                value: model[i].Memebers[j].FullName
            });
        }
        userNames = userNames.substring(0, userNames.length - 1)
        groups.push(
        {
            GroupId: model[i].GroupId,
            ProcessId: model[i].ProcessId,
            index: index,
            Name: model[i].Name,
            UserNames: groupUsers,
            Memebers: model[i].Memebers
        });
        $('#groups_table').append('<tr><td>' + model[i].Name + '</td><td>' + userNames + '</td><td><a href="#" onclick="deleteGroupRow(event,this,' + index + ')" id="delete"><span class="glyphicon glyphicon-remove"></span></a></td><td><a href="#" onclick="EditGroupRow(event,this,' + index + ')" id="Edit">Edit</a></td></tr>');
        index++;
    }
})

//Save Group Clent Side
function Save(ev) {

    if (!ValidateGroup())
        return;
    document.getElementById("grid_div").style.visibility = "visible";
    document.getElementById("DivNoData").style.visibility = "hidden";

    var userNames = "";
    groupUsers = [];
    for (i = 0; i < Object.keys(users).length; i++) {
        if (document.getElementById("chk" + Object.keys(users)[i]).checked) {
            userNames += Object.values(users)[i];
            userNames += "-";
            groupUsers.push(
            {
                key: Object.keys(users)[i],
                value: Object.values(users)[i]
            });
        }
    }
    userNames = userNames.substring(0, userNames.length - 1)
    groups.push(
    {
        GroupId: currentGroupId,
        ProcessId: $("#ProcessId").val(),
        index: index,
        Name: $('#GroupName').val(),
        userNames: groupUsers,
        Members: currentMembers
    });
    $('#groups_table').append('<tr><td>' + $('#GroupName').val() + '</td><td>' + userNames + '</td><td><a href="#" onclick="deleteGroupRow(event,this,' + index + ')" id="delete"><span class="glyphicon glyphicon-remove"></span></a></td><td><a href="#" onclick="EditGroupRow(event,this,' + index + ')" id="Edit">Edit</a></td></tr>');
    index++;
    ev.preventDefault();
    clearForm();
    currentGroupId = 0;
    currentMembers = [];
}

//Delete Group Client Side
function deleteGroupRow(ev, obj, currentIndex) {
    $(obj).closest('tr').remove();
    currentGroupIndex = groups.findIndex(x => x.index == currentIndex)
    groups.splice(currentGroupIndex, 1);
    ev.preventDefault();
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }
    currentGroupId = 0;
    currentMembers = [];
}

//Edit Group Client Side
function EditGroupRow(ev, obj, currentIndex) {
    $(obj).closest('tr').remove();
    currentGroupIndex = groups.findIndex(x => x.index == currentIndex)
    var currentGroup = groups[currentGroupIndex];;
    $('#GroupName').val(currentGroup.Name);
    currentGroupId = currentGroup.GroupId;

    for (i = 0; i < currentGroup.userNames.length; i++) {
        for (j = 0; j < Object.keys(users).length; j++) {
            var tempCheckBox = document.getElementById("chk" + Object.keys(users)[j]);
            if (currentGroup.userNames[i].key == Object.keys(users)[j]) {
                tempCheckBox.checked = true;
            }
        }
    }
    groups.splice(currentGroupIndex, 1);
    ev.preventDefault();
    --index;
    if (index == 0) {
        document.getElementById("grid_div").style.visibility = "hidden";
        document.getElementById("DivNoData").style.visibility = "visible";
    }

}

//Save Groups Server Side
function submitForm() {

    if (!ValidateGroups())
        return;
    document.getElementById("dialog_loader").style.display = "inline-block";

    $.ajax({
        type: "Post",
        url: CreateGroupsURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(groups)
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);
        $("#mydiv").css('display', 'in-block');
        $("#mydiv").attr("class", "alert success");
        $("#AlertMessage").html("Success");
    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#mydiv").css('display', 'in-block');
        $("#mydiv").attr("class", "alert success");
        $("#AlertMessage").html("Success");
    });
}

//Back To Process
function Back() {

    var tempProcessID = '{"ProcessId":"' + ($("#ProcessId").val()) + '"}'
    document.getElementById("dialog_loader").style.display = "inline-block";
    $.ajax({
        type: "Post",
        url: BackToProcessURL,
        dataType: "html",
        contentType: "application/json; charset=utf-8",
        data: tempProcessID
    }).done(function (data) {

        document.getElementById("dialog_loader").style.display = "none";
        $("#divStep").html(data);
        $("#mydiv").css('display', 'none');
    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    });
}

//Reload and Refresh the page
function Reload() {
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
        $("#mydiv").css('display', 'none');
    }).fail(function (A, B, C) {

        document.getElementById("dialog_loader").style.display = "none";

        var result = "@Html.RenderMCIMessagesArea()";
        $("#MCIMessagesArea").html(result);
    });
}

//Valide Group Data Client Side
function ValidateGroup() {
    var groupName = $('#GroupName').val();
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");
    if (groupName == "") {
        messageContainer.style.display = "inline-block";
        message.textContent = "Group Name is required";
        isValid = false;
    }
    if (groupName.length > 100) {
        messageContainer.style.display = "inline-block";
        message.textContent = "Group Name must be less than 100 charcter";
        isValid = false;
    }
    return isValid;
}

//Validate all Group Data
function ValidateGroups() {
    var isValid = true;
    var messageContainer = document.getElementById("messageContainer");
    var message = document.getElementById("message");


    if (groups.length == 0) {
        messageContainer.style.display = "inline-block";
        message.textContent = "Group is Required";
        isValid = false;
    }
    return isValid;
}