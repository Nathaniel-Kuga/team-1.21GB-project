using OpenAI.GPT3.Interfaces;
using Team121GBCapstoneProject.Services.Abstract;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.Extensions;
using static OpenAI.GPT3.ObjectModels.Models;
using System.Diagnostics;
using System.IO;

namespace Team121GBCapstoneProject.Services.Concrete;

public class WhisperService : IWhisperService
{
    private readonly IOpenAIService _openAIService;
    public WhisperService(IOpenAIService openAIService) => _openAIService = openAIService;

    public async Task<byte[]> ReadBytesFromFile(IFormFile file)
    {
        MemoryStream ms = new MemoryStream();
        await file.CopyToAsync(ms);
        byte[] fileBytes = ms.ToArray();         
        return fileBytes;
    }
    public async Task<string> GetTextFromSpeech(IFormFile file)
    {
        if (file is null or { Length: 0 }) throw new ArgumentNullException(nameof(file), "The file is null or empty.");

        // * read out the file into a byte array
        byte[] fileBytes = await ReadBytesFromFile(file);
            
        // * send the byte array to the OpenAI API
        var audioResult = await _openAIService.Audio.CreateTranscription(new AudioCreateTranscriptionRequest()
        {
            FileName = file.FileName,
            File = fileBytes,
            Model = WhisperV1,
            ResponseFormat = StaticValues.AudioStatics.ResponseFormat.VerboseJson
        });
        if (audioResult.Successful)
        {
            Debug.WriteLine(string.Join("\n", audioResult.Text));
            return audioResult.Text;
        }
        else
        {
            if (audioResult.Error == null)
            {
                throw new Exception("Unknown Error returned from OpenAI API.");
            }
            Debug.WriteLine($"{audioResult.Error.Code}: {audioResult.Error.Message}");
            throw new Exception(audioResult.Error.Message);
        }
    }
}
