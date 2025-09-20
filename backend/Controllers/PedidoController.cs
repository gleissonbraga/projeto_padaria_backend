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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [HttpGet("{id}")]
        public IActionResult ObterPedidoPorId([FromRoute] int id)
        {
            try
            {
                var pedido = _Service.ObterPedidoPorId(id);

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet()]
        public IActionResult ObterTodosPedidos()
        {
            var pedidos = _Service.ObterTodasPedidos();

            return Ok(pedidos);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar([FromRoute] int id)
        {
            try
            {
                _Service.Deletar(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message});
            }
        }

        [HttpPatch("confirmar/entrega/{id}")]
        public IActionResult ConfirmarEntrega([FromRoute] int id, [FromBody] PedidoConfirmarDTO pedidoDTO)
        {
            try
            {
                _Service.ConfirmarEntrega(id, pedidoDTO.Chave);
                return Ok(new {message = "Entrega confirmada" });
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPatch("confirmar/pagamento/{id}")]
        public IActionResult ConfirmarPagamento([FromRoute] int id)
        {
            try
            {
                _Service.ConfirmarPagamento(id);
                return Ok(new { message = "Pagamento Confirmado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
