using System.Diagnostics;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using Team121GBCapstoneProject.Services.Abstract;
using static OpenAI.GPT3.ObjectModels.Models;

namespace Team121GBCapstoneProject.Services.Concrete;
#nullable enable
public class ChatGptService : IChatGptService
{
    // private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOpenAIService _openAiService;   
    public ChatGptService(IOpenAIService openAiService)
    {
        // _httpClientFactory = httpClientFactory;
        _openAiService = openAiService;
    }


    public async Task<string> GetChatResponse(string prompt)
    {
        try
        {
            if (String.IsNullOrEmpty(prompt))
                return "";
            var chatResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromUser(prompt)
                },
                Model = ChatGpt3_5Turbo
            });
            Debug.Assert(chatResult.Choices.Any());
            if (chatResult.Choices is not null or { Count: 0 })
                return chatResult.Choices.First().Message.Content;
            return "No response from GPT-3";
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return e.Message;
        }
    }
}