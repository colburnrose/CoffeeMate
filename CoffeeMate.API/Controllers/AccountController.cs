using System.Threading.Tasks;
using CoffeeMate.API.Data;
using CoffeeMate.API.DTO;
using CoffeeMate.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AccountController(IAuthRepository repo)
        {
            _repo = repo;

        }

        // Register method
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel registerUser)
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
    }
}