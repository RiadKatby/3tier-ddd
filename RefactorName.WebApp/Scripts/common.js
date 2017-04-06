$(window).load(function () {

    //store the selected tab when changed
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var tabName = $(e.target).closest('ul.nav.nav-tabs').attr('id');
        tabName = tabName || 'lastTab';
        if (tabName)
            localStorage.setItem(tabName, $(e.target).attr('href'));
    });

    //color animation
    //fixPagingStyle();
    //animateLoader();
    confirmlinksinit();
    confirmsubmitbuttonsinit();
    //tooltipfy();
    //InitPopOver(); //script added in truncate helper
    datePickerInit();
    //update selected tabs from the local storage, if it exists:
    updateSelectedTabs();

    $('.only-number').filter_input({ regex: '[0-9]', selector: '.only-number' });
    $('.double').filter_input({ regex: /\d*\.?\d*/, selector: '.double' });
    $('.arabic').filter_input({ regex: '[\u0600-\u06FF \\s ]*', selector: '.arabic' });
    $('.arabicNS').filter_input({ regex: '[0-9_\u0600-\u06FF \\s ()-]*', selector: '.arabicNS' });
    $('.username').filter_input({ regex: '[0-9A-Za-z_]', selector: '.username' });
    $('.english').filter_input({ regex: "[A-Za-z_ ']", selector: '.english' });
    $('.englishN').filter_input({ regex: '[0-9A-Za-z_]', selector: '.englishN' });
    $('.englishNS').filter_input({ regex: '[\x20-\x7E]', selector: '.englishNS' });

    //Table Loader init
    tabelLoader();

    $(".message-area .close").click(function () {
        $(this).closest("div").slideUp(300, '', function () {
            // Animation complete.
        });
    });

    $(".message-area").find("label").each(function (i, val) {
        if ($.trim($(this).text()).length > 2) {
            ShowMessage($(this).closest("div"));
        }
    });

    //for one time submit button
    $('input[type="submit"].click-once , button[type="submit"].click-once').click(function () {
        var myForm = $(this).parents('form:first');
        myForm.submit();
        $(this).attr('disabled', 'disabled');
        if (!myForm.valid())
            $(this).removeAttr('disabled');
        return false;
    });

    $(document).on('hidden.bs.modal', '.modal-confirm', function (e) {
        $(this).remove();
    });


});

//Global Ajax  Handler
$(document).bind("ajaxSend", function (e, xhr, settings) {
    //Sent

}).bind("ajaxComplete", function (e, xhr, settings) {
    //Complete
    //tooltipfy();
    //fixPagingStyle();
    var url = settings.url;
    tabelLoader();
}).bind("ajaxSuccess", function (e, xhr, settings) {
    //Success
    try {
        var resultParesed = jQuery.parseJSON(xhr.responseText);
        JSRedirect(resultParesed);
    }
    catch (re) { }
}).bind("ajaxError", function (e, xhr, settings, thrownError) {
    //Error
});

//to hide validation mess onclick
$(document).on("click", ".field-validation-error", function () {
    $(this).fadeOut(function () {
        $(this).removeAttr("style");
        $(this).addClass("field-validation-valid").removeClass("field-validation-error");

    });
});

$(document).on("click", ".btnRefresh", function () {
    location.reload();
});

$(document).on('change', 'select[data-ajax="true"]', function (event) {
    var selectedValue = $(this).val();
    var method = $(this).data('ajax-method') || 'GET';
    var begin = $(this).data('ajax-begin');
    var success = $(this).data('ajax-success');
    var failure = $(this).data('ajax-failure');
    var complete = $(this).data('ajax-complete');
    var loading = $(this).data('ajax-loading');
    var mode = $(this).data('ajax-mode');
    var update = $(this).data('ajax-update');
    var url = $(this).data('ajax-url');
    var addData = $(this).data('ajax-dataprovider');

    //replace value on the url
    url = url.replace('%25value%25', selectedValue);

    //add additional data if any
    if (addData) {
        var fn = new Function('return ' + addData + '();');
        var data = fn();
        if (data) {
            url += url.indexOf('?') == -1 ? '?' : '';
            for (var key in data) {
                url += '&';
                url += key + '=' + data[key];
            }
        }
    }

    $.ajax({
        type: method.toUpperCase(),
        url: url,
        beforeSend: function () {
            //call onBegin
            var returnValue = executeFunction(begin);
            if (returnValue == false)
                return false;
            //show loader
            if (loading)
                $(loading).show();
        },
        error: function (res, status, error) {
            if (failure) {
                var fn = new Function('res', 'status', 'error', failure + '(res, status, error);');
                fn(res, status, error);
            }
        },
        success: function (result) {
            //update target
            if (update) {
                switch (mode) {
                    case 'after':
                        $(update).after(result);
                        break;
                    case 'before':
                        $(update).before(result);
                        break;
                    default:
                        $(update).html(result);
                        break;
                }
            }

            //call success
            if (success) {
                var fn = new Function('result', success + '(result);');
                fn(result);
            }
        },
        complete: function () {
            if (loading)
                $(loading).hide();

            //execute complete
            executeFunction(complete);
        }
    });

});

function fixPagingStyle(gridID) {
    var sel;
    if (gridID) {
        sel = $("#" + gridID + " .footerGrid td");
    }
    else {
        sel = $(".footerGrid td");
    }
    //check if code is not excuted before
    if (sel.find(".pagination").length == 0) {
        sel.find("a").wrap("<li />");
        sel.contents().filter(function () {
            return (this.nodeType === 3 && this.data.trim().length)
        })
                 .wrap('<li class="active" />').wrap("<a />");


        sel.wrapInner('<ul class="pagination" />');
    }
}

function tabelLoader() {
    $(".grid a[data-swhglnk=true]").click(function () {
        grid = $(this).closest(".grid");
        var modal = $('<div class="table-modal-back" />');
        modal.css("width", grid.css("width"));
        modal.css("height", grid.css("height"));
        modal.append('<div style="position: absolute; left: 50%;top:40%"><div style="position: relative; left: -50%;"><img src="' + $("#root").attr("href") + 'Content/images/loading32.gif"   /></div></div>');
        grid.append(modal);
        //obj.append('<div class="modal-back"><div class="animated-th"></div></div>');
    });
}

function onFailureDefault(req, status, error) {
    if (!req.responseJSON) console.error(req.responseText);
    else mciMessage(req.responseJSON.message, mciMessageType.Danger, 10);
}

function init_validators(formSelector) {
    var selector = formSelector || 'form';
    $(selector).removeData("validator");
    $(selector).removeData("unobtrusiveValidation");
    //$(selector).each(function () {

    //    $.data($(this)[0], 'validator', undefined);
    //});
    $.validator.unobtrusive.parse(selector);
}

function renderModelStateErrors(formSelector, modelStateErrors) {
    var validator = $(formSelector).data('validator');
    if (!validator || !modelStateErrors) return;
    for (var i = 0; i < modelStateErrors.length; i++) {
        var element = $('#' + modelStateErrors[i].key)[0];
        if (!element) continue;
        validator.settings.highlight.call(validator, element, validator.settings.errorClass, validator.settings.validClass);
        validator.showLabel(element, modelStateErrors[i].value);
    }
}


function JSRedirect(result) {

    if (result.url) {
        window.location.href = result.url;
        return false;
    }
}

function clearForm(formSelector) {
    formSelector = formSelector || 'form';
    //clear uploader if exist
    var $uploaderContainer = $(formSelector).find($('.file_uploader'));
    if ($uploaderContainer.length) {
        $(formSelector).find($('.file_uploader_message')).empty();
        $uploaderContainer.show();
        $uploaderContainer.fineUploader('reset');

    }
    //clear other inputs
    $(formSelector).find(':input:not(:hidden)').each(function () {
        if ($(this).css("visibility") === "hidden") return;

        switch (this.type) {
            case 'password':
            case 'select-multiple':
            case 'select-one':
            case 'text':
            case 'textarea':
                $(this).val(' ');
                $(this).val('');
                break;
            case 'checkbox':
            case 'radio':
                this.checked = false;
        }
        $(this).removeClass("input-validation-error");
    });
    $(formSelector + " .field-validation-error").removeClass("field-validation-error").addClass("field-validation-valid");
}

function resetUploader(uploaderId) {
    var uploaderSelector = "#" + uploaderId + "_attachement_div";
    $(uploaderSelector).find($('.file_uploader_message')).empty();
    $(uploaderSelector).find($('.file_uploader')).show();
    $(uploaderSelector).find($('.file_uploader')).fineUploader('reset');
}

function animateLoader() {
    $('.waiting-modal span').animate({ color: '#D89312' });
    $('.waiting-modal span').animate({ color: '#D89312' });
    $('.waiting-modal span').animate({ color: '#075654' });
    $('.waiting-modal span').animate({ color: '#075654' });
    $('.waiting-modal span').animate({ color: '#D89312' });
    $('.waiting-modal span').animate({ color: '#D89312' });
    setTimeout(animateLoader, 1500);
}

function executeFunction(functionName) {
    if (functionName) {
        var fn = window[functionName];
        if (typeof fn === "function") return fn();
    }
}

function tooltipfy() {
    $('[data-toggle="tooltip"]').tooltip();
}

function InitPopOver() {
    $('[data-toggle="popover"]').popover();
}

function datePickerInit(selecter) {
    var selecterAr = selecter ? selecter + '.datepicker' : '.datepicker';
    var selecterMix = selecter ? selecter + '.datepicker-mix' : '.datepicker-mix';

    $(selecterAr + ',' + selecterMix).each(function () {
        var range = $(this).attr('data-year-range') || 'c-20:c+20';

        $(this).calendarsPicker({
            calendar: $.calendars.instance('ummalqura', 'ar'),
            dateFormat: 'dd/mm/yyyy',
            yearRange: range,
            showOtherMonths: true,
            showTrigger: '#calImg',
            prevText: '<span class="glyphicon glyphicon-chevron-left"></span>', prevStatus: 'عرض الشهر السابق',
            prevJumpText: '&#x3e;&#x3e;', prevJumpStatus: '',
            nextText: '<span class="glyphicon glyphicon-chevron-right"></span>', nextStatus: 'عرض الشهر القادم',
            nextJumpText: '&#x3c;&#x3c;', nextJumpStatus: '',
            currentText: 'اليوم', currentStatus: 'عرض الشهر الحالي',
            todayText: 'اليوم', todayStatus: 'عرض الشهر الحالي',
            clearText: '<span class="glyphicon glyphicon-remove"></span>', clearStatus: 'امسح التاريخ الحالي',
            closeText: '<span class="glyphicon glyphicon-chevron-up"></span>', closeStatus: 'إغلاق بدون حفظ',
            yearStatus: 'عرض سنة آخرى', monthStatus: 'عرض شهر آخر',
            weekText: 'أسبوع', weekStatus: 'أسبوع السنة',
            dayStatus: 'اختر D, M d', defaultStatus: 'اختر يوم',

        });
    });

    $(selecterMix).each(function () {
        if (!$(selecterMix + ' div.checkbox_appended').length) {
            var div = $('<div />', { 'class': 'checkbox_appended' });
            div.append($('<input />', { type: 'checkbox', name: 'cb_' + $(this).attr("name"), id: 'cb_' + $(this).attr("id"), value: 'gregorian' }));
            div.append($('<label />', { 'for': 'cb_' + $(this).attr("id"), text: 'ميلادي' }));
            div.appendTo($(this).closest('.form-group'));
        }
    });
}

$(document).on("change", ".checkbox_appended input:checkbox", function () {
    var textBox = $(this).parent().parent().find(".datepicker-mix");
    var from = "gregorian", to = "ummalqura", newCul = "ar-SA";
    if (this.checked) {
        from = "ummalqura";
        to = "gregorian";
        newCul = "en-US";
    }
    try {
        var jdDate = $.calendars.instance(from).parseDate("dd/mm/yyyy", textBox.val()).toJD();
        textBox.val($.calendars.instance(to).formatDate('dd/mm/yyyy', $.calendars.instance(to).fromJD(jdDate)));
    }
    catch (eee) {
        textBox.val("");
    }
    textBox.attr("data-culture", newCul);
    textBox.calendarsPicker('option', { calendar: $.calendars.instance(to, 'ar') });
});

function confirmlinksinit() {
    $(document).on('click', 'a.mci-confirm', '', function (link) {
        var random_id = Math.floor((Math.random() * 1000000) + 1);
        link.preventDefault();
        var theLink = $(this);
        var theHREF = theLink.attr("href");
        var theREL = theLink.attr("rel") ? theLink.attr("rel") : 'هل أنت متأكد؟';
        var theTITLE = theLink.attr("title") ? theLink.attr("title") : 'تأكيد';
        var theTARGET;
        var isModal = theLink.hasClass('mci-confirm-modal');

        //build ok link
        var okButton = $('<a id="okButton_' + random_id + '" class="btn btn-primary okButton">نعم</a>');
        $(okButton).attr('href', theHREF);
        //for ajax
        var ajax = theLink.attr('data-ajax');
        if (ajax == 'true') {
            var ajaxUpdate = theLink.attr('data-ajax-update');
            var AjaxLoading = theLink.attr('data-ajax-loading');
            var onfail = theLink.attr('data-ajax-failure');
            var method = theLink.attr('data-ajax-method');
            var onsuccess = theLink.attr('data-ajax-success');


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
            theTARGET = buildModal(random_id, theTITLE, theREL, okButton[0].outerHTML);
            $(theTARGET).modal({ backdrop: "static", show: true });
        }
        else {
            okButton.addClass('btn-sm');
            var noButton = $('<button id="noButton_' + random_id + '" class="btn btn-default btn-sm noButton">لا</button>');
            $(document).on('click', '#noButton_' + random_id, function () {
                theLink.popover('hide');
            });

            var content = theREL + '<div class="popover-footer">' + noButton[0].outerHTML + okButton[0].outerHTML + "</div>";

            if (!theLink.data('bs.popover')) {
                theLink.popover({
                    placement: 'top auto',
                    trigger: 'focus',
                    container: "body",
                    html: true,
                    title: theTITLE,
                    content: content
                });
                theLink.popover('show');
            }
        }
    });

    //the following is to prevent ajax link from action (not for  links loaded with ajax) so use the helper MCIAjaxLinkConfirm
    //$('a.mci-confirm[data-ajax="true"]').each(function () {
    //    var theLink = $(this);
    //    var onbegin = theLink.attr('data-ajax-begin');
    //    onbegin =onbegin || 'ReturnFalse';
    //    theLink.attr('data-ajax-begin', onbegin);
    //});
}

function confirmsubmitbuttonsinit() {
    $(document).on('click', 'input[type="submit"].mci-confirm , button[type="submit"].mci-confirm', '', function (event) {
        var random_id = Math.floor((Math.random() * 1000000) + 1);
        event.preventDefault();
        var button = $(this);
        var theForm = $(this).parents('form:first');
        var theREL = button.attr("rel") ? button.attr("rel") : 'هل أنت متأكد؟';
        var theTITLE = button.attr("title") ? button.attr("title") : 'تأكيد';
        var okButton = $('<a id="okButton_' + random_id + '" class="btn btn-primary okButton" data-dismiss="modal">نعم</a>');
        var theTARGET;
        var isModal = button.hasClass('mci-confirm-modal');

        //add name and value of the button to the form
        if (button.attr('name') && button.attr('value')) {
            if ($("input[type='hidden'][name='" + button.attr('name') + "']").length) {
                $("input[type='hidden'][name='" + button.attr('name') + "']").val(button.val());
            }
            else {
                $('<input />').attr('type', 'hidden')
            .attr('name', button.attr('name'))
            .attr('value', button.val())
            .appendTo(theForm);
            }
        }

        $(document).on('click', '#okButton_' + random_id, function () {
            theForm.submit();
        });

        if (isModal) {
            theTARGET = buildModal(random_id, theTITLE, theREL, okButton[0].outerHTML)
            if (!button.hasClass('cancel'))
                if (!$(theForm).valid())
                    return;

            $(theTARGET).modal({ backdrop: "static", show: true });
        }
        else {
            okButton.addClass('btn-sm');
            var noButton = $('<button id="noButton_' + random_id + '" class="btn btn-default btn-sm noButton">لا</button>');
            $(document).on('click', '#noButton_' + random_id, function () {
                button.popover('hide');
            });

            var content = theREL + '<div class="popover-footer">' + noButton[0].outerHTML + okButton[0].outerHTML + "</div>";

            if (!button.data('bs.popover')) {
                button.popover({
                    placement: 'top auto',
                    trigger: 'focus',
                    container: "body",
                    html: true,
                    title: theTITLE,
                    content: content
                });
                button.popover('show');
            }
        }
    });
}

function ReturnFalse() {
    return false;
}

function buildModal(theID, theTITLE, theREL, button) {
    var noButtonText = button == '' ? 'موافق' : 'لا';
    var result = '<div id="modal_confirm_' + theID + '" class="modal fade modal-confirm" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
    result += '<div class="modal-dialog modal-sm">';
    result += '<div class="modal-content">';
    result += '<div class="modal-header">';
    result += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
    result += '<h4 class="modal-title" id="myModalLabel">' + theTITLE + '</h4></div>';
    result += '<div class="modal-body">' + theREL + '</div>';
    result += '<div class="modal-footer">';
    result += '<button type="button" class="btn btn-default" data-dismiss="modal">' + noButtonText + '</button>';
    result += button;
    result += '</div></div></div></div>';
    return result;
}

function MCIAlert(theTITLE, theMessage) {
    var random_id = Math.floor((Math.random() * 1000000) + 1);
    var theTARGET = buildModal(random_id, theTITLE, theMessage, '');
    $(theTARGET).modal({ backdrop: "static", show: true });
}

function updateSelectedTabs() {
    $('ul.nav.nav-tabs').each(function () {
        var tabName = $(this).attr('id');
        tabName = tabName || 'lastTab';
        var lastTab = localStorage.getItem(tabName);
        if (lastTab)
            $('a[href=' + lastTab + ']').tab('show');
    });
}

function MCIConfirm(theTITLE, theMessage, callBackFunction) {
    var random_id = Math.floor((Math.random() * 1000000) + 1);

    var okButton = $('<button id="okButton_' + random_id + '" class="btn btn-primary okButton">نعم</button>');

    var theTARGET = buildModal(random_id, theTITLE, theMessage, okButton[0].outerHTML);

    $(theTARGET).modal({ backdrop: "static", show: true });
    $(document).on('click', '#okButton_' + random_id, function () {
        var diag = $(this).closest('.modal').modal('hide');
        if (typeof callBackFunction == "function") callBackFunction();
    });

}

function replaceQueryString(url, param, value) {
    var re = new RegExp("([?|&])" + param + "=.*?(&|$)", "i");
    if (url.match(re))
        return url.replace(re, '$1' + param + "=" + value + '$2');
    else
        return url + '&' + param + "=" + value;
}

function showValidationErrorMessageFor(element, error) {
    $('span[data-valmsg-for="' + element.name + '"]').text(error).addClass("field-validation-error").removeClass("field-validation-valid");
}

function changeValidationIgnoreSelector(formSelector, ignoreSelector) {
    var validator = $.data($(formSelector)[0], 'validator');
    if (!validator)
        init_validators(formSelector);
    validator.settings.ignore = ignoreSelector;
}

function MCIWizardCurrentStep(wizardId) {
    wizardId = "#" + (wizardId || "mciWizard");
    var $wizardDiv = $(wizardId);
    if ($wizardDiv.hasClass('bs-wizard'))
        return $(wizardId + " .bs-wizard-step.active").attr('id');
    else if ($wizardDiv.hasClass('mci-wizard'))
        return $(wizardId + " .current").attr('id');
}

function MCIWizardChangeStep(stepId, wizardId) {
    wizardId = "#" + (wizardId || "mciWizard");
    var $wizardDiv = $(wizardId);
    if ($wizardDiv.hasClass('bs-wizard')) {
        $(wizardId + " .bs-wizard-step").removeClass('complete active').addClass('disabled');
        $wizardDiv.children(".bs-wizard-step").each(function () {
            $(this).removeClass('disabled');
            if ($(this).attr('id') == stepId) {
                $(this).addClass('active');
                return false;
            }
            else
                $(this).addClass('complete');
        });
    }
    else if ($wizardDiv.hasClass('mci-wizard')) {//a.current ~ a
        $('#mciWizard2 a.current').removeClass('current');
        $('#mciWizard2 a#' + stepId).removeClass('not-completed').addClass('current');
        $('#mciWizard2 a.current ~ a').removeClass('not-completed');
    }
}

function NoAjaxCache() {
    $.ajaxSetup({ cache: false });
}

//this line is to fix bootstrap modal auto hide when close inner modal
//the alternative is to set backdrop="static" for modal
//$.fn.modal.Constructor.prototype.enforceFocus = function () { };

(function (jQuery) {

    jQuery.fn.hasAttr = function (name) {
        for (var i = 0, l = this.length; i < l; i++) {
            if (!!(this.attr(name) !== undefined)) {
                return true;
            }
        }
        return false;
    };

})(jQuery);

