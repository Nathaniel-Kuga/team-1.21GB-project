let mediaRecorder;
let chunks = [];
let mediaStream;

const recordBtn = $('#recordBtn');
const stopBtn = $('#stopBtn');
const sendBtn = $('#send');
const prompt = $('#prompt');

console.log('Hello from whisper.js');

$(function () {

    recordBtn.on('click', async () => {
        sendBtn.attr('disabled', 'true');
        recordBtn.attr('disabled', 'true');
        await startRecording();

    });
    stopBtn.on('click', async () => {
        console.log('Stop button clicked');
        await stopRecording();
        console.log('Stop should be done');
    });
});

// * turns on microphone and starts recording
// * pushes data to chunks array
const startRecording = async () => {
    navigator.mediaDevices.getUserMedia({ audio: true })
        .then(stream => {
            mediaStream = stream;
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start();
            console.log(mediaRecorder.state);
            mediaRecorder.ondataavailable = e => {
                chunks.push(e.data);
            };
        });
};

// * waits for all data to be pushed to chunks array
// * and then stops recording
// * creates blob from chunks array
// * creates file from blob
// * creates formData object
// * appends file to formData object
// * sends formData object to server
const stopRecording = async () => {
    try {
        await new Promise(resolve => {
            mediaRecorder.onstop = resolve;
            mediaRecorder.stop();
        });
        console.log(mediaRecorder.state);
        const blob = new Blob(chunks, { type: 'audio/mp3' });
        const file = new File([blob], 'recording.mp3', { type: 'audio/mp3' });
        chunks = [];
        const formData = new FormData();
        formData.append('file', file);
        console.log('formData', formData);
        console.log('formData.get(file);', formData.get('file'));
        console.log('Calling ajax');
        await $.ajax({
            url: '/api/Whisper/PostTextFromSpeech',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: successOnAjax,
            error: errorOnAjax
        });
    }
    catch (e) {
        console.log(e);
        await mediaStream.getTracks().forEach(track => track.stop());

    }
};
const successOnAjax = async (response) => {
    await mediaStream.getTracks().forEach(track => track.stop());
    console.log('success:', response);
    $('#prompt').val(response);
    sendBtn.removeAttr('disabled');
    recordBtn.removeAttr('disabled');
}

const errorOnAjax = async (xhr, status, error) => {
    await mediaStream.getTracks().forEach(track => track.stop());
    console.log(xhr.responseText);
    console.log(status);
    console.log(error);
    $('#loadingChatGif').attr('hidden', 'true');
    if (xhr.responseText === 'Inappropriate prompt.') {
        alert('ChatGPT does not respond to inappropriate prompts. The bot has been disabled for this session.');
        sendBtn.prop('disabled', true);
        recordBtn.prop('disabled', true);
        stopBtn.prop('disabled', true);	
        prompt.prop('disabled', true);
    }
    let $responses = $('#responses');
    let $row = $('<div></div>').addClass('card text-white bg-danger');
    await $responses.append($row);
    if (xhr.responseText === 'Please enter a prompt.') {
        await typeLetter(xhr.responseText);
        return;
    }
    await typeLetter(xhr.responseText);
}