
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonInSteam.Models
{
    public class SteamUser
    {
        [JsonProperty("steamid")]
        public string steamID { get; set; }

        [JsonProperty("success")]
        public int success { get; set; }
    }

    public class SteamUserContainer
    {
        [JsonProperty("response")]
        public SteamUser response { get; set; }
    }
}