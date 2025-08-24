using backend.Config.db;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace backend.Services
{
    public class UsuarioService : InterfaceUsuario
    {

        private readonly Conexao _Conexao;

        public UsuarioService(Conexao _db)
        {
            _Conexao = _db;
        }

        public void Adicionar(Usuario usuario)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add("O nome não pode ser vazio");
            else if (string.IsNullOrEmpty(usuario.Email))
                errors.Add("O email não pode ser vazio");
            else if (string.IsNullOrEmpty(usuario.Senha))
                errors.Add("A senha não pode ser vazia");

            if (errors.Count > 0) 
            {
                throw new ErroHttp(errors);
            }

            _Conexao.Usuarios.Add(usuario);
            _Conexao.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Usuario> ObterTodosUsuarios()
        {
            var query = _Conexao.Usuarios.ToList();

            return query;
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
