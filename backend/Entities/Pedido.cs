using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COD_PEDIDO")]
        public int CodigoPedido { get; set; }

        [Column("NM_PESSOA")]
        [StringLength(60)]
        public string NomePessoa { get; set; }

        [Column("CONTATO")]
        [StringLength(9)]
        public string Contato { get; set; }

        [Column("DT_RETIRADA", TypeName = "date")]
        public DateOnly DataRetirada { get; set; }

        [Column("HR_RETIRADA", TypeName = "time")]
        public TimeOnly HoraRetirada { get; set; }

        [Column("CD_CHAVE")]
        [StringLength(8)]
        public string Chave { get; set; }

        [Column("VL_TOTAL")]
        public decimal ValorTotal { get; set; }

        [Column("DT_PEDIDO", TypeName = "timestamptz")]
        public DateTime DataPedido { get; set; }

        [Column("STATUS", TypeName = "smallint")]
        public short Status { get; set; }

        public ICollection<ProdutoPedido> ProdutosPedido { get; set; } = new List<ProdutoPedido>();
    }
}
