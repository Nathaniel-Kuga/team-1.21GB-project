// Purpose: ChatGPT page logic.

$(function () {
    $('#send').on('click', async () => {
        $('#loadingChatGif').removeAttr("hidden");
        let $prompt = $('#prompt').val();
        $prompt = $prompt.trim();
        if ($prompt === "" || $prompt === null || $prompt === undefined) {
            await sendMessageError("Please enter a prompt.");
            return;
        }
        $('#prompt').val('');
        await addUserQueryToDOM($prompt);
        await sendMessage($prompt);
    });
    $("#prompt").on('keypress', async (event) => {
        if (event.keyCode === 13) {
            $('#loadingChatGif').removeAttr("hidden");
            let $prompt = $('#prompt').val();
            $prompt = $prompt.trim();
            if ($prompt === "" || $prompt === null || $prompt === undefined) {
                await sendMessageError("Please enter a prompt.");
                return;
            }
            $('#prompt').val('');
            await addUserQueryToDOM($prompt);
            await sendMessage($prompt);
        }
    });
});

async function sendMessage(prompt) {
    await $.ajax({
        url: '/api/ChatGpt/GetChatResponse',
        method: 'GET',
        data: {
            prompt: prompt
        },
        success: sendMessageSuccess,
        error: sendMessageError
    });
}

async function sendMessageSuccess(data) {
    $('#loadingChatGif').attr('hidden', 'true');
    console.log(data);
    let $responses = $('#responses');
    let $row = $('<div></div>').addClass('card text-white bg-primary');
    const $newDiv = await $responses.append($row);
    $newDiv[0].scrollIntoView(true);
    typeLetter(data);
}

async function sendMessageError(data) {
    $('#loadingChatGif').attr('hidden', 'true');
    console.log(data.responseText);
    if (data.responseText === 'Inappropriate prompt.') {
        alert('ChatGPT does not respond to inappropriate prompts. The bot has been disabled for this session.');
        $('#prompt').prop('disabled', true);
        $('#send').prop('disabled', true);
        $('#recordBtn').prop('disabled', true);
        $('#stopBtn').prop('disabled', true);
    }
    let $responses = $('#responses');
    let $row = $('<div></div>').addClass('card text-white bg-danger');
    await $responses.append($row);
    if (data === 'Please enter a prompt.') {
        await typeLetter(data);
        return;
    }
    await typeLetter(data.responseText);
}

async function typeLetter(data) {
    const text = data.split(' ');
    let $responses = $('#responses').children().last();
    $responses.css('color', 'white');
    // * typing animation
    $.each(text, function (i, letter) {
        setTimeout(function () {
            $responses.append(letter + ' ');
        }, 100 * i);
    });
}

async function addUserQueryToDOM(prompt) {
    let $responses = $('#responses');
    let msg = $('<div>').html(prompt)
        .addClass('card text-white bg-secondary');
    $responses.append(msg);
}