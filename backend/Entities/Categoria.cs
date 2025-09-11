using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CD_CATEGORIA")]
        public int CodigoCategoria { get; set; }

        [Column("NM_CATEGORIA")]
        public string NomeCategoria { get;set; }

        public ICollection<Produto>? Produtos { get; set; }
    }
}
