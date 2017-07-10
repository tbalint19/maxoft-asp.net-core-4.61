using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExampleApp.Utils;
using System.Data;

namespace ExampleApp.Controllers
{
    public class InfoController : Controller
    {
        public InfoController() { }

        [Authorize]
        [HttpGet]
        public Dictionary<string, object> Client()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            SqlManager manager = new SqlManager("UgyfelSelectByUserName");
            manager.addParam("@UserName", SqlDbType.VarChar, username);
            var responseData = manager.getData()[0];

            response.Add("client", responseData);
            return response;
        }

        [Authorize]
        [HttpGet]
        public Dictionary<string, object> Firm()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            SqlManager manager = new SqlManager("CegSelectByUserName");
            manager.addParam("@UserName", SqlDbType.VarChar, username);
            var responseData = manager.getData()[0];

            response.Add("firm", responseData);
            return response;
        }
    }
}