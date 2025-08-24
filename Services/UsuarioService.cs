using backend.Config.db;
using backend.Entities;
using backend.Errors;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace backend.Services
{
    public class UsuarioService : InterfaceUsuario
    {

        private readonly Conexao _Conexao;
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();

        public UsuarioService(Conexao _db)
        {
            _Conexao = _db;
        }

        public void Adicionar(Usuario usuario)
        {

            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));
            else if (string.IsNullOrEmpty(usuario.Email))
                errors.Add(new ErrorDetalhe("O email não pode ser vazio"));
            else if (string.IsNullOrEmpty(usuario.Senha))
                errors.Add(new ErrorDetalhe("A senha não pode ser vazia"));

            if (errors.Count > 0) 
            {
                throw new ErroHttp(errors);
            }

            _Conexao.Usuarios.Add(usuario);
            _Conexao.SaveChanges();
        }

        public void Atualizar(Usuario usuario, int id)
        {
            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));

            if (string.IsNullOrEmpty(usuario.Email))
                errors.Add(new ErrorDetalhe("O email não pode ser vazio"));

            if (string.IsNullOrEmpty(usuario.Senha))
                errors.Add(new ErrorDetalhe("A senha não pode ser vazia"));

            var vericaEmail = _Conexao.Usuarios.Any(u => u.Email == usuario.Email);
            var findUser = _Conexao.Usuarios.Find(id);

            if(findUser == null)
                errors.Add(new ErrorDetalhe("Usuário não encontrado"));
            if (vericaEmail && findUser.Email != usuario.Email)
                errors.Add(new ErrorDetalhe("Este email já existe"));


            if (errors.Count > 0)
            {
                throw new ErroHttp(errors);
            }


            findUser.Nome = usuario.Nome;
            findUser.Email = usuario.Email;
            findUser.Senha = usuario.Senha;
            findUser.Admin = usuario.Admin;


            _Conexao.Usuarios.Update(findUser);
            _Conexao.SaveChanges();
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
