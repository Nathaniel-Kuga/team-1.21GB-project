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
            BaseSource =
                "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}";
        }

        public string GetJsonStringsFromEndpoint(string id)
        {
            string source = string.Format(BaseSource,Token, id);
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

        public async Task<SteamUser> GetSteamUser(string id)
        {
            string response = GetJsonStringsFromEndpoint(id);
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
    }
}
