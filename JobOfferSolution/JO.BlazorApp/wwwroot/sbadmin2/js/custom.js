window.initTooltips = () => {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};


window.auth = {
    login: async function (email) {
        const res = await fetch('/api/account/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({
                email: email
            })
        });

        if (!res.ok) return false;

        try {
            const data = await res.json();
            return data === true;
        } catch {
            return false;
        }
    },
    logout: async function () {
        await fetch('/api/account/logout', {
            method: 'POST',
            credentials: 'include'
        });
    }
};






