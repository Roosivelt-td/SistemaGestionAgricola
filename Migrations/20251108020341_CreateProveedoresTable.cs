using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionAgricola.Migrations
{
    /// <inheritdoc />
    public partial class CreateProveedoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProveedorId",
                table: "InsumosUtilizados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ruc = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Direccion = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoServicio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contacto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosUtilizados_ProveedorId",
                table: "InsumosUtilizados",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Nombre",
                table: "Proveedores",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_Ruc",
                table: "Proveedores",
                column: "Ruc",
                unique: true,
                filter: "[Ruc] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_TipoServicio",
                table: "Proveedores",
                column: "TipoServicio");

            migrationBuilder.AddForeignKey(
                name: "FK_InsumosUtilizados_Proveedores_ProveedorId",
                table: "InsumosUtilizados",
                column: "ProveedorId",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InsumosUtilizados_Proveedores_ProveedorId",
                table: "InsumosUtilizados");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_InsumosUtilizados_ProveedorId",
                table: "InsumosUtilizados");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "InsumosUtilizados");
        }
    }
}
