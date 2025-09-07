using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TabelaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
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
                    table.PrimaryKey("PK_Pedidos", x => x.COD_PEDIDO);
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 2, 21, 38, 42, 189, DateTimeKind.Utc).AddTicks(7110));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 2, 21, 27, 55, 51, DateTimeKind.Utc).AddTicks(5810));
        }
    }
}
