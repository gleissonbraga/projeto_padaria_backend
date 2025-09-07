using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TabelaProdutoPedidosEManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Pedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "COD_PEDIDO");

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

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 2, 23, 19, 50, 139, DateTimeKind.Utc).AddTicks(6730));

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "Pedidos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "COD_PEDIDO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 2, 21, 38, 42, 189, DateTimeKind.Utc).AddTicks(7110));
        }
    }
}
