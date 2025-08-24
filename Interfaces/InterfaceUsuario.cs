using backend.Entities;

namespace backend.Interfaces
{
    public interface InterfaceUsuario
    {
        List<Usuario> ObterTodosUsuarios();
        Usuario ObterUsuarioPorId(int id);
        void Adicionar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void Deletar(int id);
    }
}
