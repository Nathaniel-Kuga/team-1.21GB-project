using Newtonsoft.Json;
using System.Net;
using Microsoft.Net.Http.Headers;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using System.Net.Http.Headers;
using Team121GBCapstoneProject.Models.DTO;
using Microsoft.DotNet.MSIdentity.Shared;

namespace Team121GBCapstoneProject.Services.Concrete;

public class IgdbService : IIgdbService
{
    // * Inject the IHttpClientFactory through constructor injection
    private readonly IHttpClientFactory _httpClientFactory;

    private string _bearerToken;
    private string _clientId;

    public IgdbService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }

    public async Task<string> GetJsonStringFromEndpoint(string token, string uri, string clientId, string rawBody)
    {

        HttpClient httpClient = _httpClientFactory.CreateClient();

        // Add Headers
        httpClient.BaseAddress = new Uri(uri);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("Client-ID",clientId);

        HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(rawBody));

        // This is only a minimum version; make sure to cover all your bases here
        if (response.IsSuccessStatusCode)
        {
            // This is blocking; use ReadAsStreamAsync instead
            string responseText = await response.Content.ReadAsStringAsync();
            return responseText;
        }
        else
        {
            // TODO: Throw specific exceptions that explain what happened
            throw new HttpRequestException();
        }
    }

    public void SetCredentials(string clientId, string token)
    {
        _clientId = clientId;
        _bearerToken = token;
    }



    public async Task<IEnumerable<IgdbGame>> SearchGames(string query = "")
    {
        string searchBody = $"search \"{query}\"; fields name, cover.url, url; where parent_game = null;";
        string searchUri = "https://api.igdb.com/v4/games/";

        string response = await GetJsonStringFromEndpoint(_bearerToken, searchUri, _clientId, searchBody);

        IEnumerable<GameJsonDTO> gamesJsonDTO;
        try
        {
            gamesJsonDTO = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<GameJsonDTO>>(response);
        }
        catch (System.Text.Json.JsonException)
        {
            gamesJsonDTO = null;
        }

        if (gamesJsonDTO != null)
        {
            return gamesJsonDTO.Select(g => new IgdbGame(g.id, g.name, g.cover?.url?.ToString(), g.url));
        }


        return Enumerable.Empty<IgdbGame>();
    }
}