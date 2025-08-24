using backend.Config.db;
using backend.Controllers.DTO;
using backend.Entities;
using backend.Errors;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }


        [HttpGet]
        public IActionResult ObterTodosUsuarios()
        {
            var usuarios = _service.ObterTodosUsuarios();

            return Ok(usuarios);
        }


        [HttpPost]
        public IActionResult AdicionarUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = usuarioDTO.Nome,
                    Email = usuarioDTO.Email,
                    Senha = usuarioDTO.Senha,
                    Admin = usuarioDTO.Admin,
                };

                _service.Adicionar(usuario);

                return Created($"/usuarios/{usuario.IdUsuario}", usuario);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Messages);
            }
        }
    }
}
