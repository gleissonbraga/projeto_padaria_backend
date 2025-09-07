using backend.Config.db;
using backend.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace backend.Services
{
    public class LoginService
    {
        public readonly Conexao _Conexao;
        //public readonly UsuarioService _UsuarioService;

        public LoginService(Conexao conexao, UsuarioService usuarioService)
        {
            _Conexao = conexao;
            //_UsuarioService = usuarioService;
        }

        public string Login(string email, string Senha)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("Insira um email");

            if (string.IsNullOrEmpty(Senha))
                throw new Exception("Insira um senha");

            var user = _Conexao.Usuarios.FirstOrDefault(x => x.Email == email);

            if (user == null)
                throw new Exception("Email incorreto");

            bool verificaSenha = UsuarioService.VerificaSenha(Senha, user.Senha);
            if (verificaSenha)
                throw new Exception("Senha incorreta");

            string token = GerarToken(user);

            return token;
        }

        public static string GerarToken(Usuario usuario) 
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (string.IsNullOrEmpty(secretKey)) throw new Exception("JWT não configurado no ambiente");

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, usuario.Nome)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            Console.WriteLine(secretKey);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return JsonSerializer.Serialize(new { token = tokenHandler.WriteToken(token) });
        }
    }
}
