using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Models.DTO;

namespace Team121GBCapstoneProject.Services.Abstract;

public interface IIgdbService
{
    void SetCredentials(string clientId, string token);
    Task<IEnumerable<IgdbGame>> SearchGames(string query);

}
