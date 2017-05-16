$(window).load(function () {
    confirmlinksinit();
});

function confirmlinksinit() {
    $(document).on('click', 'a.mci-confirm', '', function (link) {
        debugger;
        link.preventDefault();

        var random_id = Math.floor((Math.random() * 1000000) + 1);
        var anchor = $(this);
        var hrefUrl = anchor.attr("href");
        var confirmMessage = anchor.attr("dialog-content") ? anchor.attr("dialog-content") : 'هل أنت متأكد؟';
        var title = anchor.attr("dialog-title") ? anchor.attr("dialog-title") : 'تأكيد';
        var positiveButton = anchor.attr("dialog-positiveButton");
        var negativeButton = anchor.attr("dialog-negativeButton");
        var positiveButtonStyle = anchor.attr("dialog-positiveButtonStyle");
        var isModal = anchor.hasClass('mci-confirm-modal');

        var theTARGET;


        //build ok link
        var okButton = $('<a id="okButton_' + random_id + '" class="btn btn-' + positiveButtonStyle + ' okButton">' + positiveButton + '</a>');
        $(okButton).attr('href', hrefUrl);
        //for ajax
        var ajax = anchor.attr('data-ajax');
        if (ajax == 'true') {
            var ajaxUpdate = anchor.attr('data-ajax-update');
            var AjaxLoading = anchor.attr('data-ajax-loading');
            var onfail = anchor.attr('data-ajax-failure');
            var method = anchor.attr('data-ajax-method');
            var onsuccess = anchor.attr('data-ajax-success');


            $(okButton).attr('data-ajax', 'true');
            if (ajaxUpdate)
                $(okButton).attr('data-ajax-update', ajaxUpdate);
            if (AjaxLoading)
                $(okButton).attr('data-ajax-loading', AjaxLoading);
            if (onfail)
                $(okButton).attr('data-ajax-failure', onfail);
            if (onsuccess)
                $(okButton).attr('data-ajax-success', onsuccess);
            if (method)
                $(okButton).attr('data-ajax-method', method.toUpperCase());

            if (isModal) {
                $(document).on('click', '#okButton_' + random_id, function () {
                    $(this).closest('.modal').modal('hide');
                });
            }
        }

        if (isModal) {
            theTARGET.load('/Home/ConfirmModal');
            theTARGET = buildModal(random_id, title, confirmMessage, okButton[0].outerHTML);
            $(theTARGET).modal({ backdrop: "static", show: true });
        }
        else {
            okButton.addClass('btn-sm');
            var noButton = $('<button id="noButton_' + random_id + '" class="btn btn-default btn-sm noButton">' + negativeButton + '</button>');
            $(document).on('click', '#noButton_' + random_id, function () {
                anchor.popover('hide');
            });

            var content = confirmMessage + '<div class="popover-footer">' + noButton[0].outerHTML + okButton[0].outerHTML + "</div>";

            if (!anchor.data('bs.popover')) {
                anchor.popover({
                    placement: 'top auto',
                    trigger: 'focus',
                    container: "body",
                    html: true,
                    title: title,
                    content: content
                });
                anchor.popover('show');
            }
        }
    });

    //the following is to prevent ajax link from action (not for  links loaded with ajax) so use the helper MCIAjaxLinkConfirm
    //$('a.mci-confirm[data-ajax="true"]').each(function () {
    //    var anchor = $(this);
    //    var onbegin = anchor.attr('data-ajax-begin');
    //    onbegin =onbegin || 'ReturnFalse';
    //    anchor.attr('data-ajax-begin', onbegin);
    //});
}

function buildModal(modalId, title, confirmMessage, button) {
    var noButtonText = button == '' ? 'موافق' : 'لا';
    var result = '<div id="modal_confirm_' + modalId + '" class="modal fade modal-confirm" tabindex="-1" role="dialog" aria-labelledby="modalConfirmTitle" aria-hidden="true">';
    result += '<div class="modal-dialog modal-sm">';
    result += '<div class="modal-content">';
    result += '<div class="modal-header">';
    result += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
    result += '<h4 class="modal-title" id="modalConfirmTitle">' + title + '</h4></div>';
    result += '<div class="modal-body">' + confirmMessage + '</div>';
    result += '<div class="modal-footer">';
    result += '<button type="button" class="btn btn-default" data-dismiss="modal">' + noButtonText + '</button>';
    result += button;
    result += '</div></div></div></div>';
    return result;
}