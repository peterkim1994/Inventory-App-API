using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InventoryPOS.DataStore.Daos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InventoryPOS.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticatorController : ControllerBase
    {

        private IConfiguration _config;

        public AuthenticatorController(IConfiguration config)
        {
            _config = config;
        }

        //public IActionResult Login(string userName, string pass)
        //{
        //    UserModel login = new UserModel();
        //    login.UserName = userName;
        //    login.Password = pass;
        //    IActionResult response = Unauthorized();

        //    var user = AuthenticateUser(login);
        //}

        //public UserModel AuthenticateUser(UserModel login)
        //{
        //    UserModel user = null;
        //    if(login.UserName == "peter" && login.Password == "123")
        //}

        [HttpPost("authenticate")]
        public IActionResult Authenticate()
        {
            //normall when u sign in it grabs all this stuff from db 
            var shopClaims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some id"),
                new Claim("claim-key", "claim cookie")
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



    }
}
