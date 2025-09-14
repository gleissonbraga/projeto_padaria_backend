using backend.Controllers.DTO.pedido;
using backend.Controllers.DTO.produto;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _Service;

        public PedidoController(PedidoService pedidoService)
        {
            _Service = pedidoService;
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] PedidoRequestDTO pedidoResquestDTO)
        {
            try
            {
                List<Produto> lstProdutos = new List<Produto>();
                foreach (var item in pedidoResquestDTO.Produtos) 
                {
                    lstProdutos.Add(new Produto
                    {
                        IdProduto = item.IdProduto,
                        Quantidade = item.Quantidade,
                    });
                }

                Pedido pedido = new Pedido
                {
                    NomePessoa = pedidoResquestDTO.NomePessoa,
                    Contato = pedidoResquestDTO.Contato,
                    DataRetirada = pedidoResquestDTO.DataRetirada,
                    HoraRetirada = pedidoResquestDTO.HoraRetirada,
                };

                var pedidoResponse = _Service.Adicionar(pedido, lstProdutos);
                return Ok(pedidoResponse);
            }
            catch (ErroHttp ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        //[HttpPut("{id}")]
        //public IActionResult Atualizar([FromBody] Categoria categoria, [FromRoute] int id)
        //{
        //    try
        //    {
        //        var categoriaAtualizada = _Service.Atualizar(categoria, id);

        //        return Ok(categoriaAtualizada);
        //    }
        //    catch (ErroHttp ex)
        //    {
        //        return BadRequest(ex.Errors);
        //    }
        //}

        //[HttpGet("{id}")]
        //public IActionResult ObterCategoriaPorId([FromRoute] int id)
        //{
        //    try
        //    {
        //        var categoriaAtualizada = _Service.ObterCategoriaPorId(id);

        //        return Ok(categoriaAtualizada);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpGet()]
        //public IActionResult ObterTodasCategorias([FromRoute] int id)
        //{
        //    var categoriaAtualizada = _Service.ObterTodasCategorias();

        //    return Ok(categoriaAtualizada);
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Deletar([FromRoute] int id)
        //{
        //    try
        //    {
        //        _Service.Deletar(id);

        //        return Ok();
        //    }
        //    catch (ErroHttp ex)
        //    {
        //        return BadRequest(ex.Errors);
        //    }
        //}
    }
}
