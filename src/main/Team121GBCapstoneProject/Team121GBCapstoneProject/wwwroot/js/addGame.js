import { setUpURL, getGameInfoFromPage, addGame, afterAddGame, errorAddingGameToList, getUserLists, getUserListsSuccess, getUserListSuccessDOM, getUserListsFailure } from "./addGameHelperFunctions.js";

console.log("Hello from addGame.js");


$(function () {
    let $lastClickedRow;
    let requestInProgress = false;
  
    $("table").on("click", "button", async function () {
      $lastClickedRow = $(this).closest("tr");
  
      // retrieve info from form tag
      const data = await getUserLists();
      console.log(data[0].listKind);
      getUserListSuccessDOM(data);
    });
  
    $("#formSubmit").on("click", async function (event) {
      try {
        event.preventDefault(); // prevent the default form submission behavior
  
        if (requestInProgress) {
          return;
        }
  
        requestInProgress = true;
  
        const gameDto = getGameInfoFromPage($lastClickedRow);
        const origin = $(location).attr("origin");
        const url = setUpURL(origin);
        const response = await addGame(gameDto, url);
        console.log(response);
  
        requestInProgress = false;
      } catch (error) {
        console.log(error);
        requestInProgress = false;
      }
    });
  });
  