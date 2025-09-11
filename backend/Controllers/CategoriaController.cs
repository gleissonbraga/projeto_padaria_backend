using backend.Entities;
using backend.Errors;
using backend.Interfaces;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _Service;

        public CategoriaController(CategoriaService categoriaService)
        {
            _Service = categoriaService;
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] Categoria categoria)
        {
            try
            {
                _Service.Adicionar(categoria);
                return Created($"/categoria/{categoria.CodigoCategoria}", categoria);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar([FromBody] Categoria categoria, [FromRoute] int id)
        {
            try
            {
                var categoriaAtualizada = _Service.Atualizar(categoria, id);

                return Ok(categoriaAtualizada);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObterCategoriaPorId([FromRoute] int id)
        {
            try
            {
                var categoriaAtualizada = _Service.ObterCategoriaPorId(id);

                return Ok(categoriaAtualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet()]
        public IActionResult ObterTodasCategorias([FromRoute] int id)
        {
            var categoriaAtualizada = _Service.ObterTodasCategorias();

            return Ok(categoriaAtualizada);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar([FromRoute] int id)
        {
            try
            {
                _Service.Deletar(id);

                return Ok();
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }
    }
}
