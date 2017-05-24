$(document).ready(function () {
    $('[data-toggle="popover"]').click(function (e) {
        e.stopPropagation();
    });

    $(document).click(function (e) {
        if (($('.popover').has(e.target).length == 0) || $(e.target).is('.close')) {
            $('[data-toggle="popover"]').popover('hide');
        }
    });


    $('[data-toggle="popover"]').popover({
        placement: 'top',
        html: true,
        content: function () {
            var isSuccess = false;
            var returnData;
            $.ajax({
                type: "Get",
                url: this.attributes["href"].nodeValue,
                dataType: "html",
                async: false
            }).done(function (data) {
                isSuccess = true;
                returnData = data;
            }).fail(function (A, B, C) {
                isSuccess = false;
            });
            if (isSuccess)
                return returnData;
            else
                return "reload";
        }
    });
});

function Hide() {
    $('[data-toggle="popover"]').popover('hide');
}

function GetModal(e)
{
    debugger;
    $.ajax({
        type: "Get",
        url: e.attributes["href"].nodeValue,
        dataType: "html",
        async: false
    }).done(function (data) {
        isSuccess = true;
        $('#myModal').remove();
        $("body").append(data);
    }).fail(function (A, B, C) {
        isSuccess = false;
    });

    $("#myModal").modal();
}
