document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('signUpForm');
    var firstNameInput = document.getElementById('firstname');
    var lastNameInput = document.getElementById('lastname');
    var emailInput = document.getElementById('email');
    var passwordInput = document.getElementById('password');

    // Add email validation with AJAX call
    emailInput.addEventListener('blur', function () {
        var email = this.value;
        var emailError = document.getElementById('emailError');

        // Validate email format
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(email)) {
            emailError.textContent = 'Invalid email format';
            return;
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Account/CheckEmail', true);
        xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest'); // Set the X-Requested-With header
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    console.log(response);
                    if (response.exists) {
                        emailError.textContent = 'Email already exists';
                    } else {
                        emailError.textContent = ''; // Clear error message if email is valid
                    }
                } else {
                    emailError.textContent = 'Error occurred while checking email';
                }
            }
        };
        xhr.send('email=' + encodeURIComponent(email));
    });

    form.addEventListener('submit', function (event) {
        var isValid = true;

        // Reset previous validation messages and field highlights
        clearValidationMessagesAndHighlights();

        // Validate first name
        if (!isValidName(firstNameInput.value.trim())) {
            showErrorMessageAndHighlightField(firstNameInput, 'First name is required and should contain only letters.');
            isValid = false;
        }

        // Validate last name
        if (!isValidName(lastNameInput.value.trim())) {
            showErrorMessageAndHighlightField(lastNameInput, 'Last name is required and should contain only letters.');
            isValid = false;
        }

        // Validate email
        if (!isValidEmail(emailInput.value.trim())) {
            showErrorMessageAndHighlightField(emailInput, 'Please enter a valid email address.');
            isValid = false;
        }

        // Validate password
        if (!isValidPassword(passwordInput.value.trim())) {
            showErrorMessageAndHighlightField(passwordInput, 'Password is required and should be at least 6 characters long.');
            isValid = false;
        }

        // Prevent form submission if validation fails
        if (!isValid) {
            event.preventDefault();
        }
    });

    // Function to validate name (first name and last name)
    function isValidName(name) {
        return /^[A-Za-z]+$/.test(name);
    }

    // Function to validate email
    function isValidEmail(email) {
        return /\S+@\S+\.\S+/.test(email);
    }

    // Function to validate password
    function isValidPassword(password) {
        return password.length >= 6;
    }

    // Function to show error message for an input field and highlight the field
    function showErrorMessageAndHighlightField(inputElement, message) {
        var errorMessageElement = document.createElement('div');
        errorMessageElement.className = 'text-danger';
        errorMessageElement.textContent = message;
        inputElement.parentNode.appendChild(errorMessageElement);
        inputElement.classList.add('is-invalid');
    }

    // Function to clear previous validation messages and field highlights
    function clearValidationMessagesAndHighlights() {
        var errorMessages = form.querySelectorAll('.text-danger');
        errorMessages.forEach(function (errorMessage) {
            errorMessage.remove();
        });
        var invalidInputs = form.querySelectorAll('.is-invalid');
        invalidInputs.forEach(function (input) {
            input.classList.remove('is-invalid');
        });
    }
});