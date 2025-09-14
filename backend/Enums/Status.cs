using System.ComponentModel;

namespace backend.Enums
{
    public enum Status
    {
        [Description("Ativo")]
        ATIVO = 1,
        [Description("Inativo")]
        INATIVO = 2,
        [Description("Pendente")]
        PENDENTE = 3,
        [Description("Confirmado")]
        CONFIRMADO = 4,
        [Description("Entregue")]
        ENTREGUE = 5,
        [Description("Cancelado")]
        CANCELADO = 6
    }
}
