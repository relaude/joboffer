function Logout() {
    $.ajax({
        url: '/api/auth/logout',
        method: 'POST',
        credentials: 'include',

        success: function (response) {
            window.location.href = '/identity/account/login';
        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: options.failureTitle || 'Error',
                text: xhr.responseText || 'An error occurred.'
            });
        }
    });
}

window.initTooltips = () => {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};






