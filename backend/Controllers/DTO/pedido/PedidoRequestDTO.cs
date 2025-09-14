using backend.Controllers.DTO.produto;

namespace backend.Controllers.DTO.pedido
{
    public class PedidoRequestDTO
    {
        public string? NomePessoa { get; set; }
        public string? Contato { get; set; }
        public DateOnly DataRetirada { get; set; }
        public TimeOnly HoraRetirada { get; set; }
        public List<ProdutoDTO> Produtos { get; set; } = new();
        public int? CodigoPedido { get; set; }
        public DateTime? DataPedido { get; set; }
        public decimal? ValorTotal { get; set; }
        public string? Chave { get; set; }

    }
}
