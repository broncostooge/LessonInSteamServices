using LessonInSteam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonInSteam.Models
{

    public class Player
    {
        [JsonProperty("steamid")]
        public string steamid { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public int communityvisibilitystate { get; set; }

        [JsonProperty("profilestate")]
        public int profilestate { get; set; }

        [JsonProperty("personaname")]
        public string personaname { get; set; }

        [JsonProperty("lastlogoff")]
        public int lastlogoff { get; set; }

        [JsonProperty("profileurl")]
        public string profileurl { get; set; }

        [JsonProperty("avatar")]
        public string avatar { get; set; }

        [JsonProperty("avatarmedium")]
        public string avatarmedium { get; set; }

        [JsonProperty("avatarfull")]
        public string avatarfull { get; set; }

        [JsonProperty("personastate")]
        public int personastate { get; set; }

        [JsonProperty("primaryclanid")]
        public string primaryclanid { get; set; }

        [JsonProperty("timecreated")]
        public int timecreated { get; set; }

        [JsonProperty("personastateflags")]
        public int personastateflags { get; set; }
    }

    public class Players
    {
        [JsonProperty("players")]
        public List<Player> Player { get; set; }
    }

    public class PlayerContainer
    {
        [JsonProperty("response")]
        public Players response { get; set; }
    }
}