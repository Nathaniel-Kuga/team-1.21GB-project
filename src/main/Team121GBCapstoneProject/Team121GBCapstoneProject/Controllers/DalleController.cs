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

namespace Team121GBCapstoneProject.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class DalleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDalleService _dalleService;
        private readonly IReCaptchaV3Service _reCaptchaService;
        private readonly IRepository<Person> _genericPersonRepository;
        private readonly IOpenAIService _openAiService;
        private readonly string _url = "https://www.google.com/recaptcha/api/siteverify";
        public DalleController(UserManager<ApplicationUser> userManager, 
                               IDalleService dalleService, 
                               IReCaptchaV3Service reCaptchaService, 
                               IRepository<Person> genericPersonRepository,
                               IOpenAIService openAiService)
        {
            _userManager = userManager;
            _dalleService = dalleService;
            _reCaptchaService = reCaptchaService;
            _genericPersonRepository = genericPersonRepository;
            _openAiService = openAiService;
        }
        private async Task<CreateModerationResponse> PromptModerationTask (string prompt) =>  await _openAiService.Moderation.CreateModeration(new CreateModerationRequest() {Input = prompt} );

        [HttpGet("GetImages")]
        public ActionResult<string> GetImages(string prompt, string gRecaptchaResponse)
        {
            try
            {
                if (gRecaptchaResponse == null) return BadRequest("recaptcha is null");
                if (!_reCaptchaService.IsValid(gRecaptchaResponse, _url).Result) return BadRequest("Recaptcha is not valid");
                string image = "";
                if (String.IsNullOrEmpty(prompt) is false)
                {
                    // * verify the prompt does not break content moderation by OpenAI
                    CreateModerationResponse moderationResponse = PromptModerationTask(prompt).Result;
                    if (moderationResponse.Results.FirstOrDefault()!.Flagged) return BadRequest("Inappropriate prompt.");
                    //If we got to this point send the prompt.
                    image = _dalleService.GetImages(prompt).Result;
                }
                // * update user credits
                string authorizationId = _userManager.GetUserId(User);
                Person person = _genericPersonRepository.GetAll()
                                                        .FirstOrDefault(p => p.AuthorizationId == authorizationId)!;
                if (person != null)
                {
                    person.DallECredits -= 1;
                    person = _genericPersonRepository.AddOrUpdate(person);
                    Debug.WriteLine(person);
                }
                return Ok(image);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(e);
            }
        }

        [HttpPost("SetImageToProfilePicure")]
        public ActionResult SetImageToProfilePicure()
        {
            try
            {
                // ! trying to get the image url from the form data as a parameter was not working for me 
                // ! so I am getting it from the request body instead
                HttpRequest httpRequest = HttpContext.Request;
                string imageUrl = httpRequest.Form["imageUrl"].ToString();
                if (imageUrl != null)
                {
                    byte[] imageByteArray = _dalleService.TurnImageUrlIntoByteArray(imageUrl).Result;
                    if (imageByteArray.Length == 0)
                    {
                        return BadRequest("Some thing went wrong turning the image into a byte array");
                    }
                    ApplicationUser loggedInUser= _userManager.GetUserAsync(User).Result;
                    loggedInUser.ProfilePicture = imageByteArray;
                    var check = _userManager.UpdateAsync(loggedInUser).Result;

                    return Ok("Successfully updated profile picture");
                }
                return BadRequest("Could not update profile picture");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest("Something went wrong updating the profile picture");
            }
        }

        [HttpGet("GetCreditsForView")]
        public int GetUserCredits()
        {
            try
            {
                string authorizationId = _userManager.GetUserId(User);
                int? creditCount = _genericPersonRepository.GetAll()
                    .FirstOrDefault(p => p.AuthorizationId == authorizationId)!
                    .DallECredits;
                if (creditCount != null) return (int)creditCount;
                else return 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        //[HttpPost("UpdateProfilePicture")]
        //public async Task UpdateProfilePicture(string imageURL)
        //{
        //    try
        //    {
        //        if (imageURL != null)
        //        {
        //            await _dalleService.TurnImageUrlIntoByteArray(imageURL);
        //            //await _dalleService.SetImageToProfilePicure(imageURL);
        //            return;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //    }
        //}
    }
}

            //DalleVM dalleVM = new DalleVM();
            //if (prompt != null)
            //{
            //    dalleVM.Prompt = prompt;
            //    dalleVM.ImageURL = _dalleService.GetImages(prompt).Result;
            //}
            //else
            //{
            //    throw new Exception("Prompt Invalid");
            //}

            //return Ok(dalleVM);