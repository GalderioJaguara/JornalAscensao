using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornalAscensao.Migrations
{
    /// <inheritdoc />
    public partial class UserCriadoAtualizadoExcluido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Atualizado",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Criado",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Excluido",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atualizado",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Criado",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "AspNetUsers");
        }
    }
}
