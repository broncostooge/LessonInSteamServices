using LessonInSteam.Models;
using LessonInSteam.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LessonInSteam.Controllers
{
    public class LoginController : ApiController
    {
        [Route("Register")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task RegisterUserAsync([FromBody]User newUser)
        {
            DatabaseService DBService = new DatabaseService();

            await DBService.RegisterUserToDBAsync(newUser);
        }

        [Route("Login")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string LoginUser([FromBody]User user)
        {
            DatabaseService DBService = new DatabaseService();

            string accepted;

            accepted = DBService.LoginUser(user);

            return accepted;
        }

        [Route("UpdateUser")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void UpdateUserPassword([FromBody]User user)
        {
            DatabaseService DBService = new DatabaseService();

            DBService.UpdateUserPassword(user);
        }

        [Route("DeleteUser")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public void DeleteUser([FromBody]User user)
        {
            DatabaseService DBService = new DatabaseService();

            DBService.DeleteUserFromLoginTable(user);
        }

        [Route("GetUserSteamGameInfoFromDB")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<SteamGame> GetUserSteamGameInfo([FromBody] User user)
        {
            DatabaseService DBService = new DatabaseService();

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            UserSteamGameList = DBService.GetUserSteamGameInfoFromDB(user);

            return UserSteamGameList;
        }

        [Route("GetUserSteamGameInfoFromSteamAPI")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<List<SteamGame>> GetUserSteamGameInfoFromSteamAPI([FromBody] User user)
        {
            DatabaseService DBService = new DatabaseService();

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            UserSteamGameList = await DBService.GetUserSteamGameInfoFromSteamAPI(user);

            return UserSteamGameList;
        }

        [Route("UpdateAndLoadUserSteamInfo")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<List<SteamGame>> UpdateAndLoadUserSteamInfoAsync([FromBody] User user)
        {
            DatabaseService DBService = new DatabaseService();

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            UserSteamGameList = await DBService.UpdateAndLoadUserSteamInfoAsync(user);

            return UserSteamGameList;
        }

        [Route("UpdateAndLoadUserSteamInfoFrom64ID")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<List<SteamGame>> UpdateAndLoadUserSteamInfoFrom64IDAsync([FromBody] User user)
        {
            DatabaseService DBService = new DatabaseService();

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            long steamID = Int64.Parse(user.username);

            UserSteamGameList = await DBService.UpdateAndLoadUserSteamInfoFrom64IDAsync(steamID);

            return UserSteamGameList;
        }

        [Route("VerifySteamUserName")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<SteamUserContainer> VerifySteamUserNameAsync([FromBody] User user)
        {
            SteamUserContainer SteamUser = new SteamUserContainer();
            SteamDataService SteamData = new SteamDataService();

            SteamUser = await SteamData.GetSteamUser64IDAsync(user.username);

            return SteamUser;
        }

        [Route("VerifySteam64ID")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<bool> VerifySteamIDAsync([FromBody] User user)
        {
            bool foundUser = false;
            SteamUserContainer SteamUser = new SteamUserContainer();
            SteamDataService SteamData = new SteamDataService();

            long steamID = Int64.Parse(user.username);

            foundUser = await SteamData.VerifySteam64IDAsync(steamID);

            return foundUser;
        }
    }
}
