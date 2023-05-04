//import { onClick } from "./reCAPTCHAV3.js";

$(document).ready(() => {
    getCreditCount();
    $('#submitPromptButton').on('click', (event) => {
        event.preventDefault();
        $('#loadingAnimation').removeAttr("hidden"); // show the loading animation
        const userPrompt = document.getElementById("userPrompt")
        const recaptcha = $("#dalleRecaptcha").val();
        if (userPrompt.value != "") {
            console.log(userPrompt.value);
            $.ajax({
                method: "GET",
                url: `/api/Dalle/GetImages?prompt=${userPrompt.value}&gRecaptchaResponse=${recaptcha}`,
                dataType: "text",					// data type expected in response
                success: displayImage,
                error: displayErrorStatusToClient
            });
        };
    });
    $('#applyProfilePhoto').on('click', () => {
        applyProfilePhoto();
    });
});

function getCreditCount() {
    const baseUrl = $(location).attr("origin");
    $.ajax({
        method: "GET",
        url: `${baseUrl}/api/Dalle/GetCreditsForView`,
        contentType: "json",
        success: successGettingCreditCount,
        errorOnAjax: errorGettingCreditCount
    });
}
function successGettingCreditCount(data) {
    console.log('succeeded getting credit count');
    if (data === 0) {
        $('#creditsCounter').empty()
            .text(`Credits remaining: ${data} You have used all of your free credits.`);
        $('#userPrompt').prop('disabled', true);
        $('#submitPromptButton').prop('disabled', true);
        return;
    }
    $('#creditsCounter').empty()
        .text(`Credits remaining: ${data}`);

}
function errorGettingCreditCount(data) {
    console.log('failed getting credit count');
}

function displayImage(data) {
    getCreditCount(); //update the number of credits a user has on the view.
    displayStatusToClient();
    $('#loadingAnimation').attr("hidden", "true"); // hide the loading animation
    //console.log(data);
    //console.log(typeof (data));
    let temp = document.getElementById("dalleImage");
    //temp.setAttribute("id", "dalleImage");
    temp.src = `${data}`;
    temp.style.display = "block";
    temp.style.marginLeft = "auto";
    temp.style.marginRight = "auto";
    document.getElementById("applyProfilePhoto").removeAttribute("hidden");
    document.getElementById("imageHere").removeAttribute("hidden");
    //document.getElementById("imageHere").appendChild(temp);
    document.getElementById("submitPromptButton").setAttribute("aria-disabled", "false");
}

function displayStatusToClient() {
    $("#statusNotificationDiv").empty();
    const notification = `<div class="alert alert-success alert-dismissible" role="alert">
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            <h1>Successfully made image with DALL-E!</h1>
                          </div>`;
    $("#statusNotificationDiv").append(notification);
}

function displayErrorStatusToClient(data) {
    console.log(data);
    if (data.responseText === "Inappropriate prompt.") {
        $('#loadingAnimation').attr("hidden", "true"); // hide the loading animation
        $("#statusNotificationDiv").empty();
        const notification = `<div class="alert alert-danger alert-dismissible" role="alert">
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            <h1>${data.responseText}</h1>
                          </div>`;
        $("#statusNotificationDiv").append(notification);
        return;
    }
    console.log(typeof (data.responseText));
    $('#loadingAnimation').attr("hidden", "true"); // hide the loading animation
    $("#statusNotificationDiv").empty();
    const notification = `<div class="alert alert-danger alert-dismissible" role="alert">
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            <h1>Something went wrong...</h1>
                          </div>`;
    $("#statusNotificationDiv").append(notification);
}

function successUpdatingProfilePicture(data) {
    console.log(data);
    $("#statusNotificationDiv").empty();
    const notification = `<div class="alert alert-success alert-dismissible" role="alert">
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            <h1>Successfully updated profile picutre!</h1>
                          </div>`;
    $("#statusNotificationDiv").append(notification);
    const imageUrl = $('#dalleImage').attr('src');
    $("#profilePicInNavbar").attr("src", imageUrl);
}

function errorUpdatingProfilePicture(data) {
    console.log("error updating profile picture");
    console.log(data);
    $("#statusNotificationDiv").empty();
    const notification = `<div class="alert alert-danger alert-dismissible" role="alert">
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            <h1>Failed to update profile picutre...</h1>
                          </div>`;
    $("#statusNotificationDiv").append(notification);
}

function applyProfilePhoto() {
    const imageUrl = $('#dalleImage').attr('src');
    console.log("image url: " + imageUrl);
    $.ajax({
        method: "POST",
        url: "/api/Dalle/SetImageToProfilePicure",
        data: { imageUrl: imageUrl },
        success: successUpdatingProfilePicture,
        error: errorUpdatingProfilePicture
    });
    $("#profilePicInNavbar").attr("src", imageUrl);
}