using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ExampleApp.Models;
using ExampleApp.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ExampleApp.Models.RequestModels;
using ExampleApp.Utils;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace ExampleApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<Dictionary<string, object>> Login([FromBody] LoginJson json)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            var result = await _signInManager.PasswordSignInAsync(json.username, json.password, true, false);
            response.Add("isSuccessful", result.Succeeded);
            return response;
        }

        [HttpPost]
        public async Task<Dictionary<string, object>> Logout([FromBody] LogoutJson json)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            await _signInManager.SignOutAsync();
            response.Add("isSuccessful", true);
            return response;
        }

        [Authorize]
        public Dictionary<string, object> Init()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            string username = HttpContext.User.Identity.Name;

            response.Add("error", "");
            response.Add("username", username);
            return response;
        }

        public Dictionary<string, object> Error()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            response.Add("error", "authentication");
            return response;
        }
    }
}