using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Models.DTO;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.Services.Abstract;
using static OpenAI.GPT3.ObjectModels.SharedModels.IOpenAiModels;
using Team121GBCapstoneProject.Services.Concrete;

namespace Team121GBCapstoneProject.Utilities
{
    
    public class SteamHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SteamHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetSteamId(ClaimsPrincipal user)
        {
            string steamId = null;
            ApplicationUser User = _userManager.GetUserAsync(user).Result;
            var LoginInfo = _userManager.GetLoginsAsync(User).Result;
            foreach (var external in LoginInfo)
            {
                if(external.LoginProvider != "Steam") continue;
                steamId = external.ProviderKey.Split("/")[5];
            }
            return steamId;
        }

        //Create method to do items in the controller instead


/*        public SteamGamesDTO GetSteamGames(ClaimsPrincipal user)
        {
            //string? userId = _userManager.GetUserId(user);
            var steamId = GetSteamId(user);

            return _steamService.GetGames(steamId);
        }*/
    }
}
