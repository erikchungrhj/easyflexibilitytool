function showErrorNotification(message) {
    showNotification('error', message);
}

function showSuccessNotification(message) {
    showNotification('success', message);
}

function showNotification(type, message) {
    toastr[type](message);
}

function showWaitingWindow() {
    $('#window_loading').modal({
        escapeClose: false,
        clickClose: false,
        showClose: false
    });
}

function hideWaitingWindow() {
    $.modal.close();
}

$.fn.resizeselect = function(settings) {
    return this.each(function() {
        $(this)
            .change(function() {
                // create test element
                const text = $(this).find('option:selected').text();
                const test = $('<span style="white-space: nowrap;">').html(text);

                // add to body, get width, and get out
                test.appendTo(this.parentNode);
                const width = test.width();
                test.remove();

                // set select width
                $(this).width(width);
            })
            .change();
    });
};