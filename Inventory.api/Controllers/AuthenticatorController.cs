using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InventoryPOS.DataStore.Daos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticatorController : ControllerBase
    {

        private IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticatorController
        (
            IConfiguration config,
            SignInManager<IdentityUser> signinManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _config = config;
            _signInManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());            
            string userName = reqBody.userName;
            string password = reqBody.password;
            if(userName.Length > 4 && password.Length > 4)
            {
                var user = new IdentityUser { UserName = userName };
                var result = await _userManager.CreateAsync(user, password);           
                if (result.Succeeded)
                {
                   await SignIn(user);
                   return Ok("signed in!");
                }
                string error = "";
                foreach(var err in result.Errors)
                {
                    error += err + "\n";
                }
                return BadRequest(error);
            }
            return BadRequest("password and username must be 5 characters or longer");
        }
        
        [HttpPost("Login")]
        public async Task SignIn(IdentityUser user)
        {
           await _signInManager.SignInAsync(user, isPersistent: false);
           var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);          
        }

        [Authorize]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate()
        {
            //normall when u sign in it grabs all this stuff from db 
            var shopClaims = new List<Claim>()
            {
                new Claim("cred-level", "high")            
            };

            var familyClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Korean"),
                new Claim("addy", "hamilton")
            };

            var secretBytes = Encoding.UTF8.GetBytes(_config["SecretKey"]);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            SigningCredentials signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken
                (
                   _config["JwtTokenParam:Issuer"],
                   _config["JwtTokenParam:Audience"],
                   shopClaims,
                   notBefore : DateTime.Now,
                   expires : DateTime.Now.AddHours(1),
                   signingCredentials
                );//c# way of representing a json token, parses json

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token = tokenString });
        }

        [HttpPost("CreateAdminRole")]
        public async Task<IActionResult> CreateAdminRole([FromBody]string userName)
        {
            IdentityRole adminRole = await _roleManager.FindByNameAsync("shopAdmin");                  
            if (adminRole != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                    BadRequest("That user does not exist");
                var result = await _userManager.AddToRoleAsync(user, adminRole.Name);
                return Ok();
            }
            return BadRequest();
        }

        public async Task<IActionResult> CreateWokerRole()
        {
            IdentityRole role = new IdentityRole { Name = "worker" };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest("somthing went wrong");
        }


    }
}
