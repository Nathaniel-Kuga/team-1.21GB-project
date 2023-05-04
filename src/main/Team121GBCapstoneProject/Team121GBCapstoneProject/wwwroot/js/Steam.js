function displaySteam(data) {
    console.log(data);

    const img = data.result.avatarURL;
    const profile = data.result.profileURL;
    const username = data.result.username;

    let temp = document.createElement("img");
    temp.src = `${img}`;
    document.getElementById("profileImage").innerHTML = "";
    document.getElementById("profileImage").appendChild(temp);

    document.getElementById("profileName").innerHTML = "";
    document.getElementById("profileName").innerHTML = `<a href=${profile}>${username}</a>`
}

function errorOnAjax(data) {
    console.log("Error in AJAX call " + data);
}

function showButton() {
    document.getElementById("finalBtn").removeAttribute("hidden");
}

function hideButton() {
    document.getElementById("finalBtn").setAttribute("hidden", "");
}


function steamLoad() {
    const steamId = BigInt(document.getElementById("steamId").value);
    hideButton()
    $.ajax({
        method: "GET",
        url: `/api/Steam/GetSteamUser?id=${steamId}`,
        dataType: "json",					// data type expected in response
        success: displaySteam,
        error: errorOnAjax
    });
}


