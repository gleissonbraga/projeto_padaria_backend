using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Enums;
using backend.Errors;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
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

        // GET api/produtos/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult ObterPodutoPorId([FromRoute] int id)
        {
            try
            {
                var produto = _service.ObterProdutoPorId(id);
                return Ok(produto);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        // GET - api/produtos
        [HttpGet]
        public IActionResult ObterTodosProdutos()
        {
            var produtos = _service.ObterTodosProdutos();

            return Ok(produtos);
        }

        // GET - api/produtos/inativos
        [HttpGet("inativos")]
        public IActionResult ObterTodosProdutosInativos()
        {
            var produtos = _service.ObterTodosProdutosInativos();

            return Ok(produtos);
        }

        // GET - api/produtos/ativos
        [HttpGet("ativos")]
        public IActionResult ObterTodosProdutosAtivos()
        {
            var produtos = _service.ObterTodosProdutosAtivos();

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
                    Imagem = produtoDTO.Imagem,
                };

                _service.Adicionar(produto);
                return Created($"/produtos/{produto.IdProduto}", produto);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        // PUT api/produtos/5
        [HttpPut("{id}")]
        public IActionResult AtualizarProduto([FromRoute] int id, [FromBody] ProdutoDTO produtoDTO)
        {
            try
            {
                var produto = new Produto
                {
                    Nome = produtoDTO.Nome,
                    Preco = produtoDTO.Preco,
                    Quantidade = produtoDTO.Quantidade,
                    Status = produtoDTO.Status == Status.ATIVO.ToString() ? (short)Status.ATIVO : (short)Status.INATIVO,
                    Imagem = produtoDTO.Imagem
                };

                _service.Atualizar(produto, id);

                return Ok(produto);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        // DELETE api/produtos/5
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
