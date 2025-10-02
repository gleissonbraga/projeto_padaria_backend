namespace backend.Controllers.DTO.produto
{
    public class ProdutoDTO
    {
        public int IdProduto {  get; set; }
        public string? Nome { get; set; }
        public decimal? Preco { get; set; }
        public long? Quantidade { get; set; }
        public string? Imagem { get; set; }
        public string? Status { get; set; }
        public int? Categoria { get; set; }
        public string? NomeCategoria { get; set; }
    }
}
