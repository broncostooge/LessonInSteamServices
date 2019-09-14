using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using LessonInSteam.Models;
using Newtonsoft.Json.Linq;

namespace LessonInSteam.Services
{
    public class SteamDataService
    {
        private string steamAPIKey = "3F1EEFCCC0C8F311EFD50A76A5C26E68";
        private string steamUserAPI = "http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/";
        private string steamGameAPI = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/";
        private string SteamPlayerAPI = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/";
        private string steamIDs = "&steamids=";
        private string steamID = "&steamid=";
        private string formatJSON = "&format=json";
        private string keyParameter = "?key=";
        private string steamUserParameter = "&vanityurl=";
        private static readonly HttpClient client = new HttpClient();

        public async Task<bool> VerifySteam64IDAsync(long steamID)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(SteamPlayerAPI + keyParameter + steamAPIKey + steamIDs + steamID))
            using (HttpContent content = response.Content)
            {
                bool foundUser = false;

                string result = await content.ReadAsStringAsync();

                PlayerContainer player = JsonConvert.DeserializeObject<PlayerContainer>(result);

                if (player.response.Player.Count() > 0)
                {
                    foundUser = true;
                }

                return foundUser;
            }
        }

        public async Task<SteamUserContainer> GetSteamUser64IDAsync(string steamUserName)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(steamUserAPI + keyParameter + steamAPIKey + steamUserParameter + steamUserName))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                SteamUserContainer SteamUser = JsonConvert.DeserializeObject<SteamUserContainer>(result);

                return SteamUser;
            }
        }

        public async Task<SteamGameContainer> GetUsersGames(string steamuser64ID)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(steamGameAPI + keyParameter + steamAPIKey + steamID + steamuser64ID + formatJSON + "&include_appinfo=1"))
            using (HttpContent content = response.Content)
            {
                string r = steamGameAPI + keyParameter + steamAPIKey + steamID + steamuser64ID + formatJSON + "&include_appinfo=1";
                string result = await content.ReadAsStringAsync();

                SteamGameContainer SteamUserGameList = JsonConvert.DeserializeObject<SteamGameContainer>(result);

                return SteamUserGameList;
            }
        }
    }
}