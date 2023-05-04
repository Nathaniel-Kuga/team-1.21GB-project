using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Services.Abstract;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Team121GBCapstoneProject.Models.DTO;
using Microsoft.DotNet.MSIdentity.Shared;
using System;

namespace Team121GBCapstoneProject.Services.Concrete
{
    public class SteamService : IsteamService
    {
        private string BaseSource { get; }
        private string Token;

        public SteamService(string token)
        {
            Token = token;
            BaseSource = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";
        }

        public string GetJsonStringsFromEndpoint(string source)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, source)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" }
                }
            };
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, source);

            var response = client.Send(request);

            if (response.IsSuccessStatusCode)
            
            {
                // Note there is only an async version of this so to avoid forcing you to use all async I'm waiting for the result manually
                string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseText;
            }
            else
            {
                // What to do if failure? 401? Should throw and catch specific exceptions that explain what happened
                throw new HttpRequestException();
            }
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);

            //if (response.IsSuccessStatusCode)
            //{
            //    string responseText = await response.Content.ReadAsStringAsync();
            //    return responseText;
            //}
            //else
            //{
            //    throw new HttpRequestException();
            //}
        }

        public string GetJsonStringFromEndpoint(string uri)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = client.Send(request);

            if (response.IsSuccessStatusCode)
            {
                // Note there is only an async version of this so to avoid forcing you to use all async I'm waiting for the result manually
                string responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return responseText;
            }
            else
            {
                // What to do if failure? 401? Should throw and catch specific exceptions that explain what happened
                return null;
            }

        }

        public async Task<SteamUser> GetSteamUser(string id)
        {
            string source = string.Format(BaseSource, Token, id);
            string response = GetJsonStringsFromEndpoint(source);
            Root deserializedJson = JsonConvert.DeserializeObject<Root>(response);
            foreach (var deserialized in deserializedJson.response.players)
            {
                SteamUser user = new SteamUser()
                {
                    AvatarURL = deserialized.avatarmedium,
                    ProfileURL = deserialized.profileurl,
                    Username = deserialized.personaname
                };
                return user;
            }

           throw new HttpRequestException();
        }

        public List<SteamAchievement> GetSteamAchievements(string userID, string gameID)
        {
            List<SteamAchievement> Achievements = new List<SteamAchievement>();
            string source = string.Format(
                    "http://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001/?appid={0}&key={1}&steamid={2}", gameID, Token, userID);
            string source2 = string.Format(
                "https://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?appid={0}&key={1}", gameID, Token);
            string response = null;
            string response2 = null;
            try
            {
                 response = GetJsonStringsFromEndpoint(source);
                 response2 = GetJsonStringsFromEndpoint(source2);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return Achievements;
            }
            
            SteamAchievementsDTO.Root deserialize = JsonConvert.DeserializeObject<SteamAchievementsDTO.Root>(response);
            SteamAchievementsExtraDTO.Root deserialized = JsonConvert.DeserializeObject<SteamAchievementsExtraDTO.Root>(response2);

            foreach (var achievement in deserialize.playerstats.achievements)
            {
                SteamAchievement temp = new SteamAchievement()
                {
                    Name = achievement.apiname,
                    Achieved = achievement.achieved
                };
                Achievements.Add(temp);
            }

            int count = 0;
            foreach (var achievement in deserialized.game.availableGameStats.achievements)
            {
                Achievements[count].DisplayName = achievement.displayName;
                Achievements[count].Icon = achievement.icon;
                Achievements[count].IconGrey = achievement.icongray;
                Achievements[count].Description = achievement.description;
                count++;
            }

            return Achievements;
        }

        //Used from Justin From SIN team. Was unit tested heavily from them.
        public SteamGamesDTO GetGames(string steamId)
        {
            string source = string.Format("http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json&include_appinfo=1",Token,steamId);
            string jsonReponse = GetJsonStringFromEndpoint(source);

            if(jsonReponse == null)
            {
                return null;
            }
            else
            {
                //wrap in try catch
               return System.Text.Json.JsonSerializer.Deserialize<SteamGamesDTO>(jsonReponse);
            }
            
        }

    }
}
