

using MatLand.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatLand.API.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLogin user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (await CheckUserExist(user))
            {
                return GenerateToken(user);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegister user)
        {
            if (ModelState.IsValid && user.Password == user.RePassword)
            {
                var result = await _userManager.CreateAsync(
                    new IdentityUser()
                    {
                        UserName = user.UserName,
                        Email = user.Email
                    },
                    user.Password
                );
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                user.Password = null;
                user.RePassword = null;
                return Created("", user);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        private async Task<bool> CheckUserExist(UserLogin userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user == null)
                return false;

            var passwordValidation =
             await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!passwordValidation)
                return false;

            return true;
        }

        private string GenerateToken(UserLogin userLogin)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userLogin.Email),
                new Claim(JwtRegisteredClaimNames.Email, userLogin.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
