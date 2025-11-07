using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionAgricola.Migrations
{
    /// <inheritdoc />
    public partial class CreateManosObraTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManosObra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProcesoId = table.Column<int>(type: "int", nullable: false),
                    NumeroPeones = table.Column<int>(type: "int", nullable: false),
                    DiasTrabajo = table.Column<int>(type: "int", nullable: false),
                    CostoPorDia = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManosObra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManosObra_ProcesosAgricolas_ProcesoId",
                        column: x => x.ProcesoId,
                        principalTable: "ProcesosAgricolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ManosObra_ProcesoId",
                table: "ManosObra",
                column: "ProcesoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManosObra");
        }
    }
}
