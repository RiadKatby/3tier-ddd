function initUploader(container, textboxID, controller, types, maxsize, mode, onSuccessCallback, onDeleteCallback) {
    var root1 = $("#root").attr("href");

    $(document).ready(function () {
        var $txtInput = $('#' + textboxID);
        var $uploader = $('#' + container).find('.file_uploader');
        $uploader.fineUploader({
            request: {
                endpoint: root1 + controller + "/Upload",
                params: { 'name': textboxID }
            },
            multiple: false,
            validation: {
                allowedExtensions: types,
                sizeLimit: maxsize * 1024, // 50 kB = 50 * 1024 bytes
                itemLimit: 1
            },
            text: {
                uploadButton: 'إختر ملفاً..',
                uploadButtonClass: 'btn btn-primary',
                cancelButton: 'إلغاء',
                retryButton: 'إعادة',
                deleteButton: 'حذف',
                failUpload: 'فشل التحميل',
                dragZone: 'اسحب الملفات هنا لرفعها',
                dropProcessing: 'جاري رفع الملفات...',
                formatProgress: "{percent}% من {total_size}",
                waitingForResponse: "جاري التحميل...",
                HintText: 'صيغ الملفات المسموحة: ' + types.join()
            },
            deleteFile: {
                enabled: true,
                endpoint: root1 + controller + "/deleteFile",
                customHeaders: {},
                params: {}
            },
            showMessage: function (message) {
                // Using Bootstrap's classes
                MCIAlert("خطأ", message);
            },
            

        }).on('complete', function (event, id, fileName, responseJSON) {
            var $message = $('#' + container).find('.file_uploader_message');
            var deletebtn = $(" <span class='qq-deletebtn glyphicon glyphicon-trash'   title='حذف الملف' alt='حذف الملف' />");

            if (responseJSON.success) {
                $uploader.fineUploader('setDeleteFileParams', { fileName: responseJSON.fileName }, id);
                $txtInput.val(responseJSON.fileName);
                $message.html("<a target='_blank' href='" + responseJSON.fileName + "' rel='lightbox' alt='عرض الملف' title='عرض الملف' >" + fileName + "</a>").append(deletebtn);
                $uploader.hide();
                //call onSuccessCallback function
                if (onSuccessCallback) {
                    var fn = new Function("fileName", onSuccessCallback + '(fileName);');
                    fn(responseJSON.fileName);
                }
            }
            else {
                if (responseJSON.innerHTML && responseJSON.innerHTML.toLowerCase().indexOf('maximum request length exceeded') >= 1)
                    $uploader.fineUploader('itemError', 'sizeError', fileName);
                else if (responseJSON.error)
                    $uploader.fineUploader('showMessage', responseJSON.error);
            }
            deletebtn.click(function () {
                $message.empty();
                $uploader.show();
                $uploader.fineUploader('deleteFile', id);
                $txtInput.val("");


                if (onDeleteCallback) {
                    var fn = new Function("fileName", onDeleteCallback + '(fileName);');
                    fn(responseJSON.fileName);
                }
            });
        });
        try {
            var myLength = $txtInput.val().length

            if (myLength > 0) setFileLink(container, textboxID, controller, $txtInput.val(), mode, onDeleteCallback);
        }
        catch (ee) { }
    });
}

function setFileLink(container, textboxID, controller, fileName, mode, onDeleteCallback) {
    $(document).ready(function () {
        var $uploader = $('#' + container).find('.file_uploader');
        var $txtInput = $('#' + textboxID);
        var $message = $('#' + container).find('.file_uploader_message');

        var fileName1 = "عرض الملف";
        var root1 = $("#root").attr("href");
        if (mode != "ViewMode") {
            var deletebtn = $(" <span class='qq-deletebtn glyphicon glyphicon-trash' title='حذف الملف' alt='حذف الملف' />");
        }

        var finalPath = fileName;
        if (fileName.indexOf('~/') === 0) {
            finalPath = fileName.replace('~/', '');
            finalPath = root1 + finalPath;
        }
        $uploader.fineUploader('loadDumpFile', finalPath);

        $uploader.fineUploader('setDeleteFileParams', { fileName: finalPath }, 0);
        $txtInput.val(finalPath);
        $message.html("<a target='_blank' href='" + finalPath + "' rel='lightbox' alt='عرض  ' title='عرض  ' >" + fileName1 + "</a>").append(deletebtn);
        $uploader.hide();

        if (mode != "ViewMode") {
            deletebtn.click(function () {
                $message.empty();
                $uploader.show();
                $uploader.fineUploader('deleteFile', 0);
                $txtInput.val("");

                if (onDeleteCallback) {
                    var fn = new Function("fileName", onDeleteCallback + '(fileName);');
                    fn(fileName);
                }

            });
        }
    });
}