
// Use this function for updating the SearchResults page using Ajax
let requestSent = false;
function displaySearchResults(query, platform, genre, esrbRating) {
    if (!requestSent) {
        requestSent = true;
        $.ajax({
            type: "GET",
            url: "/api/Game",
            data: {
                query: query,
                platform: platform,
                genre: genre,
                esrbRating: esrbRating
            },
            dataType: "json",
            success: searchingGamesSuccess,
            error: searchingGamesError
        });
    }
}

// ! Modifying the DOM
const searchingGamesError = (data) => {
    const errorMessage = "No results found";
    $("#gameTableBody").html("<tr> <td colspan=\"4\" style=\"text-align: center; color: red;\">No results found</td></tr>");
}


const searchingGamesSuccess = (data) => {
    if (data.length === 0) {
        // Display "No results found" message
        $("#gameTableBody").html("<tr> <td colspan=\"4\" style=\"text-align: center; color: gray;\">No results found</td></tr>");
    } else {
        $("#gameTableBody").empty(); // clear the table body before populating with new data
        $.each(data,
            function (i, game) {
                console.log(game);
                if (game.gameCoverArt != null || game.gameCoverArt != undefined || game.gameCoverArt != "") {
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
                // ! for some reason there are situations where even when the image link has been cached it still 
                // ! doesn't show up in the view so I added this to handle that edge case.
                else {
                    // * resize cover image
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