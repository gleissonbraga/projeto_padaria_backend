using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class ProdutoPedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COD_PROD_PEDIDO")]
        public int CodigoProdutoPedido { get; set; }

        [Column("COD_PRODUTO")]
        public int CodigoProduto { get; set; }
        public Produto Produto { get; set; }

        [Column("COD_PEDIDO")]
        public int CodigoPedido { get; set; }
        public Pedido Pedido { get; set; }

        [Column("QTD_PRODUTO")]
        public int QuantidadeProduto { get; set; }

        
    }
}
