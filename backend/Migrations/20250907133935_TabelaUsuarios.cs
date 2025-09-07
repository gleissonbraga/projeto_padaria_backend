using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class TabelaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 7, 13, 39, 35, 670, DateTimeKind.Utc).AddTicks(6288));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                column: "DateNow",
                value: new DateTime(2025, 9, 2, 23, 19, 50, 139, DateTimeKind.Utc).AddTicks(6730));
        }
    }
}
