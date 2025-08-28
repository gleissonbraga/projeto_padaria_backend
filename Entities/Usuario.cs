using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Entities
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(60)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(70)]
        public string Senha { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public short Admin { get; set; }

        [Column(TypeName = "smallint")]
        public short Status { get; set; }

        public DateTime DateNow { get; set; }
    }
}
