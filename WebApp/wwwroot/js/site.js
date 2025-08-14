$(document).ready(function () {
    $(document).on('mouseenter', '.course-card', function () {
        $(this).addClass('bg-light');
    }).on('mouseleave', '.course-card', function () {
        $(this).removeClass('bg-light');
    });

    $(document).on('click', '.course-card', function (event) {
        if ($(event.target).closest('a, button').length) {
            return;
        }

        let url = $(this).data('details-url');
        if (url) {
            window.location.href = url;
        }
    });
});