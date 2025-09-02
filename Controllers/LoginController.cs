using backend.Controllers.DTO;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly LoginService _service;

        public LoginController(LoginService service)
        {
            _service = service;
        }

        // POST api/login
        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = _service.Login(loginDTO.Email, loginDTO.Senha);

                return Ok(token);
            } catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
