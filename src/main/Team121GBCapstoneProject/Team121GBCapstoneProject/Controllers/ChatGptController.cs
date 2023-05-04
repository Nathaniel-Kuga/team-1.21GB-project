using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;

namespace Team121GBCapstoneProject.Controllers;
[ExcludeFromCodeCoverage]
[Route("api/[controller]")]
[ApiController]
public class ChatGptController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IChatGptService _chatGptService;
    private readonly IOpenAIService _openAiService;

    public ChatGptController(UserManager<ApplicationUser> userManager,
                                         IChatGptService chatGptService,
                                         IOpenAIService openAiService)
    {
        _userManager = userManager;
        _chatGptService = chatGptService;
        _openAiService = openAiService;
    }

    private async Task<CreateModerationResponse> PromptModerationTask (string prompt) =>  await _openAiService.Moderation.CreateModeration(new CreateModerationRequest() {Input = prompt} );

    [HttpGet("GetChatResponse")]
    public async Task<ActionResult<string>> GetChatResponse(string prompt)
    {
        try
        {
            
            string response = "";
            if (String.IsNullOrEmpty(prompt) is false)
            {
                CreateModerationResponse moderationResponse = PromptModerationTask(prompt).Result;
                if (moderationResponse.Results.FirstOrDefault()!.Flagged) return BadRequest("Inappropriate prompt.");
                //* If we got to this point send the prompt.
                response = await _chatGptService.GetChatResponse(prompt);
            }
            return Ok(response);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}