using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standups_BDD_Tests.Shared
{
    // Sitewide definitions and useful methods
    public class Common
    {
        public const string BaseUrl = "https://localhost:55942";     // copied from launchSettings.json
        //https://localhost:55942
        //https://gamingplatform.azurewebsites.net

        // File to store browser cookies in
        public const string CookieFile = "..\\..\\..\\..\\..\\..\\..\\..\\StandupsCookies.txt";

        // Page names that everyone should use
        // A handy way to look these up
        public static readonly Dictionary<string, string> Paths = new()
        {
            { "Home" , "/" },
            { "Login", "/Identity/Account/Login" },
            { "Profile", "/Identity/Account/Manage/account%20preferences"},
            { "Steam Games", "/SteamGames" },
            { "Dalle Page", "/Home/GenerateImage"},
            { "Friend", "/Home/FindFriends" },
            { "ChatGPT page", "/Home/ChatGpt"},
            { "SpeedSearch", "/Views/SpeedSearch"}
        };

        public static string PathFor(string pathName) => Paths[pathName];
        public static string UrlFor(string pathName) => BaseUrl + Paths[pathName];
    }
}