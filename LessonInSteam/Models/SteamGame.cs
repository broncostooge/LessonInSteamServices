using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonInSteam.Models
{
    public class SteamGame
    {
        [JsonProperty("appid")]
        public int appid { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("playtime_forever")]
        public int playtime_forever { get; set; }

        [JsonProperty("img_icon_url")]
        public string img_icon_url { get; set; }

        [JsonProperty("img_logo_url")]
        public string img_logo_url { get; set; }
    }

    public class SteamGames
    {

        [JsonProperty("game_count")]
        public int game_count { get; set; }

        [JsonProperty("games")]
        public List<SteamGame> games { get; set; }
    }

    public class SteamGameContainer
    {
        [JsonProperty("response")]
        public SteamGames response { get; set; }
    }
}