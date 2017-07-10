using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExampleApp.Utils;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using ExampleApp.Models.RequestModels;
using Microsoft.AspNetCore.Identity;
using ExampleApp.Models;

namespace ExampleApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public Dictionary<string, object> GetAll()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            SqlManager manager = new SqlManager("TorzsFelhSelect");
            manager.addParam("@UserName", SqlDbType.VarChar, username);
            manager.addParam("@Tsz", SqlDbType.VarChar, "");
            manager.addParam("@Nev", SqlDbType.VarChar, "");
            manager.addParam("@SzulDatst", SqlDbType.VarChar, "");
            manager.addParam("@Szerv", SqlDbType.VarChar, "");
            manager.addParam("@FelhNev", SqlDbType.VarChar, "");
            var responseData = manager.getData();

            response.Add("users", responseData);
            return response;
        }

        [Authorize]
        [HttpPost]
        public Dictionary<string, object> Generate([FromBody] GenerateJson json)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            SqlManager manager = new SqlManager("FelhNevGeneral");
            manager.addParam("@UserName", SqlDbType.VarChar, username);
            manager.addParam("@Tsz", SqlDbType.Int, json.tsz.ToString());
            var generated = manager.getData();

            response.Add("generated", generated[0]);
            return response;
        }

        [Authorize]
        [HttpPost]
        public async Task<Dictionary<string, object>> Create([FromBody] CreateJson json)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;
            string adminCreated = "";
            string workerUpdated = "";

            if (json.tsz != 0)
            {
                SqlManager manager = new SqlManager("FelhNevUpdate");
                manager.addParam("@UserName", SqlDbType.VarChar, username);
                manager.addParam("@Tsz", SqlDbType.Int, json.tsz.ToString());
                manager.addParam("@FelhNev", SqlDbType.VarChar, json.username);
                manager.addParam("@Jelszo", SqlDbType.VarChar, json.password);
                workerUpdated = manager.updateRow();
            }

            var user = new ApplicationUser { UserName = json.username, Email = json.email };
            var result = await _userManager.CreateAsync(user, json.password);
            bool isCreated = result.Succeeded;

            if (isCreated)
            {
                SqlManager manager = new SqlManager("UserCreateAdmin");
                manager.addParam("@UserName", SqlDbType.VarChar, username);
                manager.addParam("@FelhNev", SqlDbType.VarChar, json.username);
                manager.addParam("@Tsz", SqlDbType.Int, json.tsz.ToString());
                manager.addParam("@Email", SqlDbType.VarChar, json.email);
                manager.addParam("@Jelszo", SqlDbType.VarChar, json.password);
                adminCreated = manager.updateRow();
            }

            response.Add("isSuccessful", isCreated);
            response.Add("adminCreated", adminCreated);
            response.Add("workerUpdated", workerUpdated);
            return response;
        }

        public Dictionary<string, object> UpdateRole([FromBody] UpdateJson json)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            SqlManager manager = new SqlManager("FelhRoleUpdate");
            manager.addParam("@UserName", SqlDbType.VarChar, username);
            manager.addParam("@FelhNev", SqlDbType.VarChar, json.username);
            manager.addParam("@RoleName", SqlDbType.VarChar, json.role);
            manager.addParam("@Jel", SqlDbType.Bit, json.setTo == true ? 1 : 0);
            var error = manager.updateRow();

            response.Add("error", error);
            return response;
        }
    }
}