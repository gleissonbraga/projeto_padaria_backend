using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProduto { get; set; }

        [Required]
        [StringLength(50)]
        public string? Nome { get; set; }

        [Required]
        public decimal? Preco {  get; set; }

        public long? Quantidade { get; set; }

        [StringLength(70)]
        public string? Imagem { get; set; }

        [Column(TypeName = "smallint")]
        public short Status { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTimeOffset DateNow { get; set; }
    }
}
