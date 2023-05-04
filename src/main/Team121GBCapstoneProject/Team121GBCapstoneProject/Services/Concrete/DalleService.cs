using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using Microsoft.AspNetCore.Identity;
using OpenAI.GPT3.Interfaces;
using Team121GBCapstoneProject.Services.Abstract;
using System.Net;

namespace Team121GBCapstoneProject.Services.Concrete;

public class DalleService : IDalleService
{
    private readonly IOpenAIService _openAiService;
    private readonly HttpClient _httpClient;

    public DalleService(IOpenAIService openAiService, HttpClient httpClient)
    {
        _openAiService = openAiService;
        _httpClient = httpClient;
    }

    public async Task<string> GetImages(string prompt)
    {
        try
        {
            var imageResult = await _openAiService.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = prompt,
                N = 1,
                Size = StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url
            });

            string finalImage = null;
            foreach (var image in imageResult.Results)
            {
                finalImage = image.Url;
            }
            return finalImage;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error in GetImages method in DalleService", e);
        }
    }


    public async Task<byte[]> TurnImageUrlIntoByteArray(string imageURL)
    {
        if (imageURL == null)
        {
            return null;
        }
        if (imageURL == "")
        {
            return null;
        }
        byte[] imageBytes;
        using (_httpClient)
        {
            using(HttpResponseMessage response = await _httpClient.GetAsync(imageURL))
            {
                imageBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            }
        }
        return imageBytes;
    }

    
}