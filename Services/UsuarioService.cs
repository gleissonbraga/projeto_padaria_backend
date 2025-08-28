using backend.Config.db;
using backend.Entities;
using backend.Enums;
using backend.Errors;
using backend.Interfaces;
using System.Security.Cryptography;

namespace backend.Services
{
    public class UsuarioService : InterfaceUsuario
    {
        // Faz a conexão com o banco de dados e o Entity Framework
        private readonly Conexao _Conexao;

        // Lista para gerar erros
        public List<ErrorDetalhe> errors = new List<ErrorDetalhe>();

        // Construtor para injeção de dependencia
        public UsuarioService(Conexao _db)
        {
            _Conexao = _db;
        }

        // Adiciona o usuário no banco de dados
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

            var hashSenha = HashSenha(usuario.Senha);
            usuario.Senha = hashSenha;
            usuario.DateNow = DateTime.UtcNow;
            usuario.Status = (short)Status.ATIVO;

            _Conexao.Usuarios.Add(usuario);
            _Conexao.SaveChanges();
        }

        // Atualiza o usuário no banco de dados
        public Usuario Atualizar(Usuario usuario, int id)
        {
            if (string.IsNullOrEmpty(usuario.Nome))
                errors.Add(new ErrorDetalhe("O nome não pode ser vazio"));
            if (string.IsNullOrEmpty(usuario.Email))
                errors.Add(new ErrorDetalhe("O email não pode ser vazio"));
            if (string.IsNullOrEmpty(usuario.Senha))
                errors.Add(new ErrorDetalhe("A senha não pode ser vazia"));

            var vericaEmail = _Conexao.Usuarios.Any(u => u.Email == usuario.Email);
            var findUser = _Conexao.Usuarios.Find(id);

            if (findUser == null)
                errors.Add(new ErrorDetalhe("Usuário não encontrado"));
            if (vericaEmail && findUser.Email != usuario.Email)
                errors.Add(new ErrorDetalhe("Este email já existe"));

            if (errors.Count > 0)
            {
                throw new ErroHttp(errors);
            }

            string hashSenha = HashSenha(usuario.Senha);
            var teste = hashSenha.Length;
            findUser.Nome = usuario.Nome;
            findUser.Email = usuario.Email;
            findUser.Senha = hashSenha;
            findUser.Admin = usuario.Admin;
            findUser.Status = usuario.Status;

            _Conexao.Usuarios.Update(findUser);
            _Conexao.SaveChanges();

            return findUser;
        }

        // Exclui o usuário do banco de dados
        public void Deletar(int id)
        {
            var user = ObterUsuarioPorId(id);

            if (user != null)
            {
                _Conexao.Usuarios.Remove(user);
                _Conexao.SaveChanges();
            }
            else
            {
                errors.Add(new ErrorDetalhe("Usuário não encontrado"));
            }

            if (errors.Count > 0)
                throw new ErroHttp(errors);
        }

        // Mostra todos os usuários 
        public List<Usuario> ObterTodosUsuarios()
        {
            var query = _Conexao.Usuarios.ToList();

            return query;
        }

        public List<Usuario> ObterTodosUsuariosInativos()
        {
            var query = _Conexao.Usuarios.Where(u => u.Status == (short)Status.INATIVO).ToList();

            return query;
        }

        public List<Usuario> ObterTodosUsuariosAtivos()
        {
            var query = _Conexao.Usuarios.Where(u => u.Status == (short)Status.ATIVO).ToList();

            return query;
        }

        // busca usuário por id
        public Usuario ObterUsuarioPorId(int id)
        {
            Usuario usuario = _Conexao.Usuarios.Find(id);

            if (usuario == null)
                errors.Add(new ErrorDetalhe("Usuário não encontrado"));

            if (errors.Count > 0)
                throw new ErroHttp(errors);
            else
                return usuario;
        }

        // Realiza o hash da senha para deixar mais seguro
        public static string HashSenha(string senha)
        {
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }


            var pbkdf2 = new Rfc2898DeriveBytes(senha, salt, 100000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(32);

            byte[] hashBytes = new byte[48];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
