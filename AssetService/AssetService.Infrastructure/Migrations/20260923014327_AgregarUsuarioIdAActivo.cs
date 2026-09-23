using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetService.Infrastructure.Migrations;

public partial class AgregarUsuarioIdAActivo : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "UsuarioId",
            table: "Activos",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Activos_UsuarioId",
            table: "Activos",
            column: "UsuarioId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Activos_UsuarioId",
            table: "Activos");

        migrationBuilder.DropColumn(
            name: "UsuarioId",
            table: "Activos");
    }
}