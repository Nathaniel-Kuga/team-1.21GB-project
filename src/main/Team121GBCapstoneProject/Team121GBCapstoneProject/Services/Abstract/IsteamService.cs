using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.Services.Abstract
{
    public interface IsteamService
    {
        Task<SteamUser> GetSteamUser(string id);
    }
}
