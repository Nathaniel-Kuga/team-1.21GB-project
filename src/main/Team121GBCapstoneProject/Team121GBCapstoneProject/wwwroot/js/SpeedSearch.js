$(document).ready(function () {
    $("#games-submit").on("click", function (event) {
        event.preventDefault(); // prevent the default form submission behavior
        const $query = document.getElementById("GameEntry").value;
        var resultOfSpeedSearch;
        SpeedSearchSubmit($query);
        /*console.log(resultOfSpeedSearch);
        var location = "/Search/Results/?query=" + encodeURIComponent($query);
        window.location.href = location;*/
    });
});

$(document).ready(function () {
    if (window.location.href.includes("/Search/Results/DisplaySpeedSearch")) {
        let urlParams = new URLSearchParams(window.location.search);
        let query = urlParams.get("query");
        let data = urlParams.get("data");
        data = JSON.parse(data);
        console.log("query: ", query);
        console.log("data: ", data);
        console.log("data typeof: ", typeof (data));
        console.log("data[0]: ", data[0]);
        DisplaySpeedSearchResults(data);
    }
});

function SpeedSearchSubmit(query) {
    return $.ajax({
        type: "POST",
        url: `/api/Game/SpeedSearch/?query=${query}`,
        datatype: "json",
        success: successAlert,
        error: ErrorAlert
    });
}

function successAlert(data) {
    resultOfSpeedSearch = data;
    console.log("Success");
    console.log(resultOfSpeedSearch);

    /*for (let i = 0; i < resultOfSpeedSearch.length; i++) {
        console.log(resultOfSpeedSearch[i]);
        console.log(resultOfSpeedSearch[i].value);
    }*/

    const $query = document.getElementById("GameEntry").value;
    var location = "/Search/Results/DisplaySpeedSearch?query=" + encodeURIComponent($query) + "&data=" + encodeURIComponent(JSON.stringify(data));
    window.location.href = location;
}

function ErrorAlert() {
    console.log("Error");
}

function DisplaySpeedSearchResults(data) {
        if (data.length === 0) {
            // Display "No results found" message
            $("#gameTableBody").html("<tr> <td colspan=\"4\" style=\"text-align: center; color: gray;\">No results found</td></tr>");
        } else {
            $("#gameTableBody").empty(); // clear the table body before populating with new data
            $.each(data,
                function (i, game) {
                    try {
                        // resize cover image
                        var resizedCoverArt = game.gameCoverArt.replace("thumb", "logo_med");

                        let platformArray = [];
                        $.each(game.platforms, (i, platform) => {
                            platformArray.push(platform);
                        });
                        let genreArray = [];
                        $.each(game.genres, (i, genre) => {
                            genreArray.push(genre);
                        });
                        var row = `<tr>
                                    <td><img src="${resizedCoverArt}"></td>
                                    <td><b>${game.gameTitle}</b></td>
                                    <td><a class="btn btn-warning" href="${game.gameWebsite}">IGDB Page</a></td>
                                    <td><p>${game.gameDescription}</p></td>
                                    <td><p>${game.firstReleaseDate}</p></td>
                                    <td><p>${platformArray}</p></td>
                                    <td><p>${genreArray}</p></td>
                                    <td><button id="${i}" class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#AddGame">Add Game</button></td>
                                    <td style="display: none">${game.id}</td>
                               </tr>`;
                        $("#gameTableBody").append(row);
                    }
                    // for some reason there are situations where even when the image link has been cached it still 
                    //doesn't show up in the view so I added this to handle that edge case.
                    catch {
                        // resize cover image
                        let noCover = "https://images.igdb.com/igdb/image/upload/t_thumb/nocover.png";
                        var resizedCoverArt = noCover.replace("thumb", "logo_med");
                        let platformArray = [];
                        $.each(game.platforms, (i, platform) => {
                            platformArray.push(platform);
                        });
                        let genreArray = [];
                        $.each(game.genres, (i, genre) => {
                            genreArray.push(genre);
                        });
                        var row = `<tr>
                                    <td><img src="${resizedCoverArt}"></td>
                                    <td><b>${game.gameTitle}</b></td>
                                    <td><a class="btn btn-warning" href="${game.gameWebsite}">IGDB Page</a></td>
                                    <td><p>${game.gameDescription}</p></td>
                                    <td><p>${game.firstReleaseDate}</p></td>
                                    <td><p>${platformArray}</p></td>
                                    <td><p>${genreArray}</p></td>
                                    <td><button id="${i}" class="btn btn-warning" data-bs-toggle="modal" data-bs-target="#AddGame">Add Game</button></td>
                                    <td style="display: none">${game.id}</td>
                               </tr>`;
                        $("#gameTableBody").append(row);
                    }
                });
        
    }
}

/*function SpeedSearchSubmit(query) {
    $.ajax({
        type: "POST",
        url: `/api/Game/SpeedSearch?query=${query}`,
        data: {
            query: query
        },
        datatype: "json",
        success: alert("success"),
        error: alert("error")
    });
}*/