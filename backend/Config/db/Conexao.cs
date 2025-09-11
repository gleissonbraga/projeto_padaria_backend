using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Config.db
{
    public class Conexao : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProdutoPedido> ProdutoPedido { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

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

            modelBuilder.Entity<ProdutoPedido>()
       .HasKey(pp => new { pp.CodigoPedido, pp.CodigoProduto });

            modelBuilder.Entity<ProdutoPedido>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.ProdutosPedido)
                .HasForeignKey(pp => pp.CodigoPedido);

            modelBuilder.Entity<ProdutoPedido>()
                .HasOne(pp => pp.Produto)
                .WithMany(p => p.ProdutosPedido)
                .HasForeignKey(pp => pp.CodigoProduto);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CodigoCategoria);
        }

        public Conexao(DbContextOptions<Conexao> options) : base(options)
        {
        }

    }
}
