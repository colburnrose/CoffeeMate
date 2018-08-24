using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoffeeMate.API.Data;
using CoffeeMate.API.DTO;
using CoffeeMate.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;

        public AccountController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // Register method
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerUser)
        {
            registerUser.Username = registerUser.Username.ToLower();
            // validate the request            
            if(await _repo.UserExists(registerUser.Username)) return BadRequest("Username already exists.");

            var createdUser = new User 
            {
                UserName = registerUser.Username
            };

            var user = await _repo.Register(createdUser, registerUser.Password);
            return StatusCode(201);
        }

        // Login method
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            // verified user exist in the db
            var user = await _repo.Login(loginUser.Username.ToLower(), loginUser.Password);
            if(user == null)
            return Unauthorized();
            // building up the token (userId, username)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            // creating a Security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            // then using the key as part of the sign-in credentials while encrypting the key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // creating the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeUtil.ToUniversalTime(System.DateTime.Now.AddDays(1)),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}