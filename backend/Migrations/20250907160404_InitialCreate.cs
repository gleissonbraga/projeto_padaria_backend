using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    COD_PEDIDO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NM_PESSOA = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CONTATO = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    DT_RETIRADA = table.Column<DateOnly>(type: "date", nullable: false),
                    HR_RETIRADA = table.Column<TimeOnly>(type: "time", nullable: false),
                    CD_CHAVE = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    VL_TOTAL = table.Column<decimal>(type: "numeric", nullable: false),
                    DT_PEDIDO = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    STATUS = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.COD_PEDIDO);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    IdProduto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: true),
                    Imagem = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    DateNow = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.IdProduto);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Senha = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Admin = table.Column<short>(type: "smallint", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    DateNow = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoPedido",
                columns: table => new
                {
                    COD_PRODUTO = table.Column<int>(type: "integer", nullable: false),
                    COD_PEDIDO = table.Column<int>(type: "integer", nullable: false),
                    COD_PROD_PEDIDO = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QTD_PRODUTO = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoPedido", x => new { x.COD_PEDIDO, x.COD_PRODUTO });
                    table.ForeignKey(
                        name: "FK_ProdutoPedido_Pedido_COD_PEDIDO",
                        column: x => x.COD_PEDIDO,
                        principalTable: "Pedido",
                        principalColumn: "COD_PEDIDO",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoPedido_Produtos_COD_PRODUTO",
                        column: x => x.COD_PRODUTO,
                        principalTable: "Produtos",
                        principalColumn: "IdProduto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Admin", "DateNow", "Email", "Nome", "Senha", "Status" },
                values: new object[] { 1, (short)1, new DateTime(2025, 9, 7, 16, 4, 4, 789, DateTimeKind.Utc).AddTicks(5343), "admin@admin.com", "Administrador", "admin", (short)0 });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoPedido_COD_PRODUTO",
                table: "ProdutoPedido",
                column: "COD_PRODUTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoPedido");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
