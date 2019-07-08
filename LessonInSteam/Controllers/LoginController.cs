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

namespace LessonInSteam.Controllers
{
    public class LoginController : ApiController
    {
        [Route("Register")]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async System.Threading.Tasks.Task RegisterUserAsync([FromBody]User newUser)
        {
            DatabaseService DBService = new DatabaseService();

            await DBService.RegisterUserToDBAsync(newUser);
        }
        
        [Route("Login")]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public string GetPasswordForUser([FromBody]User user)
        {
            DatabaseService DBService = new DatabaseService();

            string hashedPassword = DBService.ReturnUserHashPassword(user);

            return hashedPassword;
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
    }
}
