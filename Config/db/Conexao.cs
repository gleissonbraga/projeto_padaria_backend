using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Config.db
{
    public class Conexao : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                IdUsuario = 1,
                Nome = "Administrador",
                Email = "admin@admin.com",
                Senha = "admin",
                Admin = 1,
                DateNow = DateTime.UtcNow
           });
        }

        public Conexao(DbContextOptions<Conexao> options) : base(options)
    {
    }

}
}
