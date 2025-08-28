using backend.Entities;

namespace backend.Interfaces
{
    public interface InterfaceUsuario
    {
        List<Usuario> ObterTodosUsuarios();
        Usuario ObterUsuarioPorId(int id);
        void Adicionar(Usuario usuario);
        Usuario Atualizar(Usuario usuario, int id);
        void Deletar(int id);
    }
}
