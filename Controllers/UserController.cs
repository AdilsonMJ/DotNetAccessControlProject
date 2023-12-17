using ControllerDeAcesso.Data;
using ControllerDeAcesso.Data.DTO;
using ControllerDeAcesso.Entity.DTO;
using ControllerDeAcesso.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControllerDeAcesso.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController(UserService registerService)
        {
            _userService = registerService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Cadastrar(CreateUserDto userDto)
        {
            var result = await _userService.CreateUser(userDto);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok("Succeeded");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
           var token =  await _userService.Login(loginDto);

            return Ok(token);
        }

    }
}
