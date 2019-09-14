using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonInSteam.Models
{
    public class SteamID
    {
        [JsonProperty("steamID")]
        public int steamID { get; set; }
    }
}