// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
console.log("hello from reCAPTCHA js");

$(function () {
    $(document).ready(function () {
        $('form').on('submit', function (e) {
            if (grecaptcha.getResponse() == "") {
                e.preventDefault();
                alert("Please complete the CAPTCHA before submitting the form.");
            }
            else {
                console.log("reCAPTCHA passed on client side.");	
            }
        });
    });
});
