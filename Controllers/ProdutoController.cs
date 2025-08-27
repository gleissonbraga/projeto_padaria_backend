using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Errors;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private readonly ProdutoService _service;
        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        // GET api/<ProdutoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // GET - api/produtos
        [HttpGet]
        public IActionResult ObterTodosProdutos()
        {
            var produtos = _service.ObterTodosProdutos();

            return Ok(produtos);
        }

        // POST - api/produtos
        [HttpPost]
        public IActionResult Adcionar([FromBody] ProdutoDTO produtoDTO)
        {
            try
            {
                var produto = new Produto
                {
                    Nome = produtoDTO.Nome,
                    Preco = produtoDTO.Preco,
                    Quantidade = produtoDTO.Quantidade,
                    Imagem = produtoDTO.Imagem
                };

                _service.Adicionar(produto);
                return Created($"/produtos/{produto.IdProduto}", produto);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        // PUT api/<ProdutoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProdutoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
