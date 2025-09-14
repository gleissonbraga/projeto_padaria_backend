using backend.Controllers.DTO.pedido;
using backend.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Interfaces
{
    public interface IPedido
    {
        List<PedidoRequestDTO> ObterTodasPedidos();
        List<PedidoRequestDTO> ObterTodosProdutosPedido();
        PedidoRequestDTO ObterPedidoPorId(int id);
        PedidoRequestDTO Adicionar(Pedido pedido, List<Produto> produtos);
        void Deletar(int id);
        void ConfirmarEntrega(int id, string chave);
    }
}
