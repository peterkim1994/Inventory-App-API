using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InventoryPOS.DataStore;
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
        private readonly DBContext _context;
        public AuthenticatorController
        (
            IConfiguration config,
            SignInManager<IdentityUser> signinManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context
        )
        {
            _config = config;
            _signInManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(Object jsonResult)
        {
            dynamic reqBody = JObject.Parse(jsonResult.ToString());
            string userName = reqBody.userName;
            string password = reqBody.password;
            if (userName.Length > 4 && password.Length > 4)
            {
                var user = new IdentityUser { UserName = userName };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await Login(new { userName = userName, password = password });
                    return Ok("signed in!");
                }
                string error = "";
                foreach (var err in result.Errors)
                {
                    error += err + "\n";
                }
                return BadRequest(error);
            }
            return BadRequest("password and username must be 5 characters or longer");
        }

        [HttpPost("Login")]       
        public async Task<IActionResult> Login(Object jsonObj)
        {
            dynamic reqBody = JObject.Parse(jsonObj.ToString());
            string userName = reqBody.userName;
            string password = reqBody.password;
            IdentityUser user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest("Username is invalid");
            }
            else if (await _signInManager.CanSignInAsync(user))
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (signInResult.Succeeded)
                {
                    return new ObjectResult(await GenerateToken(userName));
                }
            }
            return BadRequest("Incorrect Password");
            //      await _signInManager.SignInAsync(user, isPersistent: false);
            //    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
        }

        [Authorize]
        [HttpPost("Logout")]
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
                new Claim(ClaimTypes.Name, "high")
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
                   notBefore: DateTime.Now,
                   expires: DateTime.Now.AddHours(1),
                   signingCredentials
                );//c# way of representing a json token, parses json

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token = tokenString });
        }

        [HttpPost("CreateAdminRole")]
        public async Task<IActionResult> CreateAdminRole([FromBody] string userName)
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


        [HttpPost("createStaffRole")]
        public async Task<IActionResult> CreateWokerRole([FromBody]string userName)
        {
            IdentityRole role = new IdentityRole { Name = "staff" };
            var user = await _userManager.FindByNameAsync(userName);
            var roleCreated = await _roleManager.CreateAsync(role);

            var roleAdded = await _userManager.AddToRoleAsync(user, role.Name);
            if (roleCreated.Succeeded && user!=null)
            {               
                return Ok(roleAdded.Succeeded);
            }
            return BadRequest("somthing went wrong");
        }

        [HttpGet]
        public async Task AddUserPositionClaim([FromBody] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var c1 = new Claim(ClaimTypes.Name, userName);
            var c2 = new Claim(ClaimTypes.Role, "shopAdmin");
            await _userManager.RemoveClaimAsync(user, c2);
            await _userManager.AddClaimAsync(user, c1);
            await _userManager.AddClaimAsync(user, c2);
        }


        

        public async Task<dynamic> GenerateToken(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf,  new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var adminRole = claims.FirstOrDefault(c => c.Value.Equals("adminRole"));
            var defaultExpire = new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString());
            var twoHourExpire = new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(2)).ToUnixTimeSeconds().ToString());

            if (adminRole != null)
            {
                claims.Add(twoHourExpire);
            }
            else
            {
                claims.Add(defaultExpire);
            }

            var secretBytes = Encoding.UTF8.GetBytes(_config["SecretKey"]);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            SigningCredentials signingCredentials = new SigningCredentials(key, algorithm);
            var token = new JwtSecurityToken(new JwtHeader(signingCredentials), new JwtPayload(claims));
            var output = new
            {
                AccessToken = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token),
                UserName = userName
            };
            return output;
        }
    }
}
