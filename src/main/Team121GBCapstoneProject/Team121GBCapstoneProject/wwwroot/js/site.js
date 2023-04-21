// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Search Feature
$(document).ready(function () {
    $("#searchButton").click(function (event) {

        console.log("Search button has been clicked");

        event.preventDefault(); // prevent the default form submission behavior
        var query = $("#searchInput").val(); // get the value from the search input field
        let $platform = $("#platformSelectList").val() ?? ""; // * get the platform value
        let $genre = $("#genreSelectList").val() ?? "";
        let $esrbRatingId = $("#esrbRatingSelectList").val() ?? 0;
        // navigate to query url
        var location = "/Search/Results/?query=" + encodeURIComponent(query) + "&platform=" + encodeURIComponent($platform) + "&genre=" + encodeURIComponent($genre) + "&esrbRating=" + encodeURIComponent($esrbRatingId);
        window.location.href = location;
    });
});

$(document).ready(function () {
    if (window.location.href.includes("/Search/Results/")) {

        var urlParams = new URLSearchParams(window.location.search);
        var query = urlParams.get('query');
        let platform = urlParams.get('platform');
        let genre = urlParams.get('genre');
        let esrbRating = urlParams.get('esrbRating');

        console.log("Search: ", query, "Platform: ", platform, "Genre: ", genre, "EsrbRating: ", esrbRating);

        if (query != null) {
            displaySearchResults(query, platform, genre, esrbRating);
        } else {
            console.log("No search query provided");
        }
    }
});

// DALLE
function dalleModalOpen() {
    $('#DalleModal').modal('show');
}
function dalleModalClose() {
    $('#DalleModal').modal('hide');
}



//$('#dark-mode-toggle').click(function () {
//    // toggle the dark mode
//    $('body').toggleClass('dark-mode');

//    // save the user's preference
//    if ($('body').hasClass('dark-mode')) {
//        localStorage.setItem('dark-mode-enabled', 'true');
//    } else {
//        localStorage.setItem('dark-mode-enabled', 'false');
//    }

//    darkmode.toggle();
//    console.log("dark mode activated: ", darkmode.isActivated()); // should return true
//});

// ATTEMPT 2

//// check the user's preference on page load
//$(document).ready(function () {
//    var darkModeEnabled = localStorage.getItem('dark-mode-enabled');
//    if (darkModeEnabled === 'true') {
//        var sliderBefore = $('.slider:before');
//        sliderBefore.css('left', '34px');

//        if (!darkmode.isActivated) {
//            darkmode.toggle();
//        }
//    } else {
//        if (darkmode.isActivated) {
//            darkmode.toggle();
//        }
//    }

//    // localStorage.setItem('dark-mode-enabled', 'true');

//    // update the state of the toggle button
//    //var toggleSwitch = $('#dark-mode-toggle input[type="checkbox"]');
//    //if (darkmode.isActivated()) {
//    //    toggleSwitch.prop('checked', true);
//    //} else {
//    //    toggleSwitch.prop('checked', false);
//    //}
//});

//$('#dark-mode-toggle').click(function () {

//    darkmode.toggle();
//    console.log("dark mode activated: ", darkmode.isActivated()); // should return the mode

//    // update the state of the toggle button
//    //var toggleSwitch = $('#dark-mode-toggle input[type="checkbox"]');

//    if (darkmode.isActivated()) {
//        localStorage.setItem('dark-mode-enabled', 'true');
//    } else {
//        localStorage.setItem('dark-mode-enabled', 'false');
//    }
//});


// *** ATTEMPT 3 ***

//$(document).ready(function () {
//    var darkModeEnabled = localStorage.getItem('dark-mode-enabled');
//    if (darkModeEnabled === 'true') {
//        var sliderBefore = $('.slider:before');
//        sliderBefore.css('left', '26px');

//        if (!darkmode.isActivated()) {
//            darkmode.toggle();
//        }
//    } else {
//        if (darkmode.isActivated()) {
//            darkmode.toggle();
//        }
//    }

//    $('#dark-mode-toggle').click(function () {
//        darkmode.toggle();
//        console.log("dark mode activated: ", darkmode.isActivated()); // should return the mode

//        if (darkmode.isActivated()) {
//            localStorage.setItem('dark-mode-enabled', 'true');
//        } else {
//            localStorage.setItem('dark-mode-enabled', 'false');
//        }
//    });
//});

$('#darkModeButton').click(function () {
    darkmode.toggle();
    console.log("dark mode activated: ", darkmode.isActivated());
});


// Darkmode.js
// See https://darkmodejs.learn.uno/
var options = {
    bottom: '64px', // default: '32px'
    right: 'unset', // default: '32px'
    left: '32px', // default: 'unset'
    time: '0.3s', // default: '0.3s'
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff',  // default: '#fff'
    buttonColorDark: '#100f2c',  // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    label: '🌓', // default: ''
    autoMatchOsTheme: true // default: true
}

var darkmode = new Darkmode(options);
darkmode.showWidget();