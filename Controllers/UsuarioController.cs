using backend.Config.db;
using backend.Controllers.DTO.usuario;
using backend.Controllers.DTO.usuario;
using backend.Controllers.DTO.ususario;
using backend.Entities;
using backend.Errors;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            List<UsuarioResponseDTO> lstUsuarioDTO = new List<UsuarioResponseDTO>();

            foreach (var usuario in usuarios)
            {
                lstUsuarioDTO.Add(new UsuarioResponseDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Admin = usuario.Admin
                });
            }

            return Ok(lstUsuarioDTO);
        }

        [HttpGet("{id}")]
        public IActionResult ObterUsuarioPorId([FromRoute] int id) 
        {
            try
            {
                var usuario = _service.ObterUsuarioPorId(id);

                UsuarioResponseDTO usuarioResponseDTO = new UsuarioResponseDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Admin = usuario.Admin
                };

                return Ok(usuarioResponseDTO);
            }catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }


        [HttpPost]
        public IActionResult AdicionarUsuario([FromBody] UsuarioRequestDTO usuarioDTO)
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

                var usuarioResponse = new UsuarioResponseDTO
                {
                    Nome = usuarioDTO.Nome,
                    Email = usuarioDTO.Email,
                    Admin = usuarioDTO.Admin,
                };

                return Created($"/usuarios/{usuario.IdUsuario}", usuarioResponse);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarUsuario([FromBody] UsuarioRequestDTO usuarioDTO, [FromRoute] int id)
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

                _service.Atualizar(usuario, id);

                var usuarioResponse = new UsuarioResponseDTO
                {
                    Nome = usuarioDTO.Nome,
                    Email = usuarioDTO.Email,
                    Admin = usuarioDTO.Admin,
                };


                return Ok(usuarioResponse);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar([FromRoute] int id) 
        {
            try
            {
                _service.Deletar(id);
                return Ok();
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
