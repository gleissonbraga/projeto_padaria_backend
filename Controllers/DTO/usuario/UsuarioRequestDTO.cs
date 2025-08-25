namespace backend.Controllers.DTO.usuario
{
    public class UsuarioRequestDTO
    {

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public short Admin {  get; set; }
    }
}
