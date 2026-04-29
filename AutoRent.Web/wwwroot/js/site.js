// AutoRent JavaScript

// Initialize tooltips
document.addEventListener('DOMContentLoaded', function () {
    // Bootstrap tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Auto-dismiss alerts after 5 seconds
    var alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            var bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });

    // Date validation for rental forms
    var startDateInput = document.getElementById('StartDate');
    var endDateInput = document.getElementById('EndDate');

    if (startDateInput && endDateInput) {
        startDateInput.addEventListener('change', function () {
            var startDate = new Date(this.value);
            startDate.setDate(startDate.getDate() + 1);
            endDateInput.min = startDate.toISOString().split('T')[0];

            if (new Date(endDateInput.value) <= new Date(this.value)) {
                endDateInput.value = startDate.toISOString().split('T')[0];
            }
        });
    }
});

// Confirm delete actions
function confirmDelete(message) {
    return confirm(message || 'Are you sure you want to delete this item?');
}

// Calculate rental price
function calculateRentalPrice(pricePerDay) {
    var startDate = document.getElementById('StartDate').value;
    var endDate = document.getElementById('EndDate').value;

    if (startDate && endDate) {
        var start = new Date(startDate);
        var end = new Date(endDate);
        var days = Math.ceil((end - start) / (1000 * 60 * 60 * 24));

        if (days > 0) {
            var totalPrice = days * pricePerDay;
            document.getElementById('totalPrice').textContent = totalPrice.toFixed(2) + ' €';
            document.getElementById('totalDays').textContent = days + ' дни';
        }
    }
}

// Form validation
(function () {
    'use strict';

    var forms = document.querySelectorAll('.needs-validation');

    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }

            form.classList.add('was-validated');
        }, false);
    });
})();
