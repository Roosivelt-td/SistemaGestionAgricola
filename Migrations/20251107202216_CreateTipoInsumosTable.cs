using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionAgricola.Migrations
{
    /// <inheritdoc />
    public partial class CreateTipoInsumosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoInsumos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Categoria = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoInsumos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InsumosUtilizados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcesoId = table.Column<int>(type: "int", nullable: false),
                    TipoInsumoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CostoFlete = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsumosUtilizados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsumosUtilizados_ProcesosAgricolas_ProcesoId",
                        column: x => x.ProcesoId,
                        principalTable: "ProcesosAgricolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsumosUtilizados_TipoInsumos_TipoInsumoId",
                        column: x => x.TipoInsumoId,
                        principalTable: "TipoInsumos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosUtilizados_ProcesoId",
                table: "InsumosUtilizados",
                column: "ProcesoId");

            migrationBuilder.CreateIndex(
                name: "IX_InsumosUtilizados_TipoInsumoId",
                table: "InsumosUtilizados",
                column: "TipoInsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_TipoInsumos_Categoria",
                table: "TipoInsumos",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_TipoInsumos_Nombre",
                table: "TipoInsumos",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsumosUtilizados");

            migrationBuilder.DropTable(
                name: "TipoInsumos");
        }
    }
}
