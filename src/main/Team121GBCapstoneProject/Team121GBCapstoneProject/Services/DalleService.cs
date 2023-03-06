using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using Microsoft.AspNetCore.Identity;
using OpenAI.GPT3.Interfaces;

namespace Team121GBCapstoneProject.Services;

class DalleService : IDalleService
{
    private readonly IOpenAIService _openAiService;

    public DalleService(IOpenAIService openAiService)
    {
        _openAiService = openAiService;
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

            //if (imageResult.Error == null)
            //{
            //    throw new Exception("Error processing result");
            //}

            return finalImage;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}