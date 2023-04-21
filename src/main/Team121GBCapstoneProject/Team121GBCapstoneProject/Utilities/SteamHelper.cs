using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Team121GBCapstoneProject.Areas.Identity.Data;


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
    }
}
