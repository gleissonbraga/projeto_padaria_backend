using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoPedido_Pedido_COD_PEDIDO",
                table: "ProdutoPedido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido");

            migrationBuilder.RenameTable(
                name: "Pedido",
                newName: "Pedidos");

            migrationBuilder.AddColumn<int>(
                name: "CodigoCategoria",
                table: "Produtos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "COD_PEDIDO");

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CD_CATEGORIA = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NM_CATEGORIA = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CD_CATEGORIA);
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 11, 21, 46, 2, 205, DateTimeKind.Utc).AddTicks(9957));

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CodigoCategoria",
                table: "Produtos",
                column: "CodigoCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoPedido_Pedidos_COD_PEDIDO",
                table: "ProdutoPedido",
                column: "COD_PEDIDO",
                principalTable: "Pedidos",
                principalColumn: "COD_PEDIDO",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categoria_CodigoCategoria",
                table: "Produtos",
                column: "CodigoCategoria",
                principalTable: "Categoria",
                principalColumn: "CD_CATEGORIA",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoPedido_Pedidos_COD_PEDIDO",
                table: "ProdutoPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categoria_CodigoCategoria",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CodigoCategoria",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "CodigoCategoria",
                table: "Produtos");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Pedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedido",
                table: "Pedido",
                column: "COD_PEDIDO");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 7, 16, 4, 4, 789, DateTimeKind.Utc).AddTicks(5343));

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoPedido_Pedido_COD_PEDIDO",
                table: "ProdutoPedido",
                column: "COD_PEDIDO",
                principalTable: "Pedido",
                principalColumn: "COD_PEDIDO",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
