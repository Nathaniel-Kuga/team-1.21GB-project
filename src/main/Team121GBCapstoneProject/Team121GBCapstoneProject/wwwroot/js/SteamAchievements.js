function steamModalOpen(gameId) {
    const steamId = BigInt(document.getElementById("steamId").value);

    $.ajax({
        method: "GET",
        url: `/api/Steam/GetSteamAchievements?userID=${steamId}&gameID=${gameId}`,
        dataType: "json",					// data type expected in response
        success: displaySteam,
        error: errorOnAjax
    });
}
function steamModalClose() {
    $('#SteamModal').modal('hide');
}

function displaySteam(data) {
    console.log(data);
    let flexDiv = document.getElementById("here");
    flexDiv.innerHTML = "";
    if (data.length != 0) {
        for (let i = 0; i < data.length; i++) {
            let flexItem = document.createElement("div");
            flexItem.style.padding = "2px";
            flexItem.style.position = "relative";
            if (data[i].achieved === 0) {
                let img = document.createElement("img");
                img.src = `${data[i].iconGrey}`;
                img.title = `${data[i].displayName}: ${data[i].description}`;
                img.style.height = "100px";
                img.style.width = "100px";
                flexItem.appendChild(img);
            }
            else {
                let img = document.createElement("img");
                img.src = `${data[i].icon}`;
                img.title = `${data[i].displayName}: ${data[i].description}`;
                img.style.height = "100px";
                img.style.width = "100px";
                flexItem.appendChild(img);
            }
            flexDiv.append(flexItem);
        }

        console.log("made it to achievements");
    }
    else {
        let notFound = document.createElement("div")
        notFound.innerHTML = "<p>No Achievements Found</p>";
        flexDiv.append(notFound);
    }

    $('#SteamModal').modal('show');
}

function errorOnAjax(data) {
    console.log("Error in AJAX call " + data);
}