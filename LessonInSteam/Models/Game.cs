using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonInSteam.Models
{
    public class Game
    {   [JsonProperty("appId")]
        public int appId { get; set; }
    }
}