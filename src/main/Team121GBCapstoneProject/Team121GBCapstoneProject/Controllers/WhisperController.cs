using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Team121GBCapstoneProject.Services.Abstract;
using OpenAI.GPT3.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace Team121GBCapstoneProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WhisperController : ControllerBase
{
    private readonly IWhisperService _whisperService;
    private readonly IOpenAIService _openAiService;

    public WhisperController(IWhisperService whisperService, IOpenAIService openAIService)
    {
        _whisperService = whisperService;
        _openAiService = openAIService;
    }
    private async Task<CreateModerationResponse> PromptModerationTask (string prompt) =>  await _openAiService.Moderation.CreateModeration(new CreateModerationRequest() {Input = prompt} );

    [HttpPost("PostTextFromSpeech")]
    public async Task<ActionResult> Post(IFormFile file)
    {
        try
        {
            // * verify a file is not null
            if (file == null)
                return BadRequest("No file selected");
            // * verify the file is an mp3
            if (file.ContentType != "audio/mp3")
                return BadRequest("File is not an mp3");
            string resultText = await _whisperService.GetTextFromSpeech(file);
            // * verify the prompt does not break content moderation by OpenAI
            var moderationResponse = await PromptModerationTask(resultText);
            if (moderationResponse.Results.FirstOrDefault()!.Flagged) return BadRequest("Inappropriate prompt.");
            // * If we got to this point return the prompt.
            return Ok(resultText);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

