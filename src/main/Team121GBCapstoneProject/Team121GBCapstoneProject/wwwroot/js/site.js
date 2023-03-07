// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Search feature
$(document).ready(function () {
    $("#searchButton").click(function (event) {
        event.preventDefault(); // prevent the default form submission behavior
        var query = $("#searchInput").val(); // get the value from the search input field

        $.ajax({
            type: "GET",
            url: "/api/Game",
            data: { query: query }, // pass the search query as a parameter to the API Controller
            dataType: "json",
            success: function (data) {
                $("#gameTableBody").empty(); // clear the table body before populating with new data
                $.each(data, function (i, game) {
                    // resize cover image
                    var resizedCoverArt = game.gameCoverArt.replace("thumb", "logo_med");

                    var row = `<tr>
                                    <td><img src="${resizedCoverArt}"></td>
                                    <td id="td${i}">${game.gameTitle}</td>
                                    <td><a href="${game.gameWebsite}">${game.gameWebsite}"</a></td>
                                    <td><button id="${i}" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#AddGame">Add Game</button></td>
                               </tr>`;
                    $("#gameTableBody").append(row);
                });
                // redirect to the search results page after the ajax call is successful
                //window.location.href = "/Search/Results";
            }
        });
    });
});

function dalleModalOpen() {
    $('#DalleModal').modal('show');
}
function dalleModalClose() {
    $('#DalleModal').modal('hide');
}
//$("table").on("click", "button", function () {
//    // Get the title from the data attribute of the parent cell
//    let title = $(this).parent().data("title");

//    // Log some information for debugging
//    console.log(this.id);
//    console.log($(this).parent().attr("id"));
//    console.log(title);

//    // Make an AJAX request using jQuery
//    $.ajax({
//        type: "GET",
//        url: "/api/Game/getUserLists",
//        dataType: "json",
//        context: { title }, // Use shorthand property for context object
//        success: getUserListsSuccess,
//        error: getUserListsFailure
//    });
//});
//put the get the lists method
$("table").on("click", "button", function () {
    let id = $(this).attr("id");
    let tableDataId = $(`#td${id}`).attr("id");
    const title = $(`#${tableDataId}`).text();

    console.log(id);
    console.log(tableDataId);
    console.log(title);
    //
    $.ajax({
        type: "GET",
        url: "/api/Game/getUserLists",
        dataType: "json",
        success: function (data) {
            console.log("Getting user lists succeeded");
            let listNameFormId = $("#listName").attr("id");
            $("#listName").empty();
            for (let i = 0; i < data.length; ++i) {
                console.log(data[i]);
                let selectOption = `<option value="${data[i].listName}">${data[i].listName}</option>`;
                $("#listName").append(selectOption);
            }

            $("#formSubmit").on("click", function () {
                const list = $("#listName").text();
                console.log("listName = " + list);
                let origin = $(location).attr("origin");
                console.log(title);
                const dataToSend = { listName: list, gameTitle: title };
                console.log(dataToSend);
                $.ajax({
                    type: "POST",
                    dataType: "json",
                    url: `${origin}/api/Game/addGame`,
                    contentType: "application/json; charset=UTF-8",
                    data: JSON.stringify(dataToSend),
                    success: afterAddGame,
                    error: errorAlert
                });
            });
        },
        error: getUserListsFailure
    });

});


function afterAddGame() {
    alert("Success!");
    //$("#confirmation").empty();
    //let confirmation = `<h1 class="text-success">
    //                        Success!
    //                    </h1>`;
    //$("#confirmation").append(confirmation);
}

function errorAlert() {
    alert("Something went wrong, please try again.")
}

//function getUserListsSuccess(data, title) {
function getUserListsSuccess(data) {

    console.log("Getting user lists succeeded");
    let listNameFormId = $("#listName").attr("id");
    $("#listName").empty();
    for (let i = 0; i < data.length; ++i) {
        console.log(data[i]);
        let selectOption = `<option value="${data[i].listName}">${data[i].listName}</option>`;
        $("#listName").append(selectOption);
    }

    $("#formSubmit").on("click", function () {
        const list = $("#listName").text();
        console.log("listName = " + list);
        //let id = $(this).attr("id");
        //let tableDataId = $(`#td${id}`).attr("id");
        //const title = $(`#${tableDataId}`).text();
        ////const title = $("#").text();

        console.log(this.title);
        const dataToSend = { listName: list, gameTitle: this.title };
        console.log(dataToSend);
        $.ajax({
            type: "POST",
            dataType: "json",
            url: `api/Game/addGame`,
            contentType: "application/json; charset=UTF-8",
            data: JSON.stringify(dataToSend),
            success: afterAddGame,
            error: errorAlert
        });
    });
}

function getUserListsFailure(data) {
    console.log("You don't have any lists to add a game to!");
}
