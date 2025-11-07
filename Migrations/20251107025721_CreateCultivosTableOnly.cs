using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaGestionAgricola.Migrations
{
    /// <inheritdoc />
    public partial class CreateCultivosTableOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cultivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TerrenoId = table.Column<int>(type: "int", nullable: false),
                    TipoCultivoId = table.Column<int>(type: "int", nullable: false),
                    FechaSiembra = table.Column<DateTime>(type: "date", nullable: false),
                    FechaCosechaEstimada = table.Column<DateTime>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "planificado")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cultivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cultivos_Terrenos_TerrenoId",
                        column: x => x.TerrenoId,
                        principalTable: "Terrenos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cultivos_TipoCultivos_TipoCultivoId",
                        column: x => x.TipoCultivoId,
                        principalTable: "TipoCultivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivos_Estado",
                table: "Cultivos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivos_FechaCosechaEstimada",
                table: "Cultivos",
                column: "FechaCosechaEstimada");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivos_FechaSiembra",
                table: "Cultivos",
                column: "FechaSiembra");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivos_TerrenoId",
                table: "Cultivos",
                column: "TerrenoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cultivos_TipoCultivoId",
                table: "Cultivos",
                column: "TipoCultivoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cultivos");
        }
    }
}
