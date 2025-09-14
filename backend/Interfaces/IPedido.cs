using backend.Controllers.DTO.pedido;
using backend.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Interfaces
{
    public interface IPedido
    {
        List<Pedido> ObterTodasPedidos();
        List<Pedido> ObterTodosProdutosPedido();
        Categoria ObterPedidoPorId(int id);
        PedidoRequestDTO Adicionar(Pedido pedido, List<Produto> produtos);
        void Deletar(int id);
    }
}
