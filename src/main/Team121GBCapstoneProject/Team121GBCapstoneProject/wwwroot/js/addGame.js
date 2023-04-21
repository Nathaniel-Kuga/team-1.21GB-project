import { setUpURL, getGameInfoFromPage, addGame, afterAddGame, errorAddingGameToList, getUserLists, getUserListsSuccess, getUserListSuccessDOM, getUserListsFailure } from "./addGameHelperFunctions.js";

console.log("Hello from addGame.js");

$(document).ready(function () {
    $("table").on("click", "button", async function () {
        const $row = $(this).closest("tr");
        // * retrieve info from from tag
        const data = await getUserLists();
        console.log(data[0].listKind);
        getUserListSuccessDOM(data);
        $("#formSubmit").on("click", function (event) {
            try {
                event.preventDefault(); // prevent the default form submission behavior
                const gameDto = getGameInfoFromPage($row);
                const origin = $(location).attr("origin");
                const url = setUpURL(origin);
                const response = addGame(gameDto, url);
                const data = response;
                console.log(data);
            }
            catch (error) {
                console.log(error);
            }
        });
    });
});