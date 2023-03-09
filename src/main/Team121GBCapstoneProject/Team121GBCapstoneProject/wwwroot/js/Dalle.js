
function dalleModalOpen() {
	$('#DalleModal').modal('show');
}
function dalleModalClose() {
	$('#DalleModal').modal('hide');
}

function displayImage(data) {
	console.log("Successfully made image with dalle: " + data);

	let temp = document.createElement("img");
	temp.src = `${data.responseText}`;
	temp.style.display = "block";
	temp.style.marginLeft = "auto";
	temp.style.marginRight = "auto";
	document.getElementById("applyProfilePhoto").removeAttribute("hidden");
	document.getElementById("imageHere").removeAttribute("hidden");

	document.getElementById("imageHere").innerHTML = "";
	//document.getElementById("imageHere").setAttribute("href", `${data.responseText}`)
	//document.getElementById("imageHere").setAttribute("download", "DalleGeneratedImage")
	document.getElementById("imageHere").appendChild(temp);
	document.getElementById("submitPromptButton").setAttribute("aria-disabled", "false");
	

	//document.getElementById("imageHere").removeAttribute("hidden");
	//document.getElementById("imageHere").setAttribute("src", `${data.responseText}`)
}

function errorOnAjax(data) {
	console.log("Error in AJAX call" + data.url);
}

$(function () {
	$("#promptForm").submit(function (event) {
		// prevent automatic submission of the form from button press or typing enter
		event.preventDefault();
	})

//	document.getElementById("submitPromptButton").setAttribute("aria-disabled", "true");

//	$("#submitPromptButton").click(function () {
//		const userPrompt = document.getElementById("userPrompt")
//		if (userPrompt.value != "") {
//			console.log(userPrompt);
//			$.ajax({
//				method: "GET",
//				url: `/api/Dalle/GetImages?prompt=${userPrompt.value}`,
//				dataType: "json",					// data type expected in response
//				success: displayImage,
//				error: displayImage
//			});
//		};
//	});
});

function dalleClick() {
	const userPrompt = document.getElementById("userPrompt")
	if (userPrompt.value != "") {
		console.log(userPrompt);
		$.ajax({
			method: "GET",
			url: `/api/Dalle/GetImages?prompt=${userPrompt.value}`,
			dataType: "json",					// data type expected in response
			success: displayImage,
			error: displayImage
		});
	};
};