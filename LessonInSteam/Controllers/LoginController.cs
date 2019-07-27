﻿using LessonInSteam.Models;
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

            return "TRUE";
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

        [Route("GetUserSteamGameInfo")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<SteamGame> GetUserSteamGameInfo([FromBody] User user)
        {
            DatabaseService DBService = new DatabaseService();

            List<SteamGame> UserSteamGameList = new List<SteamGame>();

            UserSteamGameList = DBService.GetUserSteamGameInfo(user);

            return UserSteamGameList;
        }
    }
}
