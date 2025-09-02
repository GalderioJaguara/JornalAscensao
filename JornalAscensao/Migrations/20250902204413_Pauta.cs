using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornalAscensao.Migrations
{
    /// <inheritdoc />
    public partial class Pauta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pautas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LinkConteudo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Fechada = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Criado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Atualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Excluido = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pautas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pautas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pautas_UsuarioId",
                table: "Pautas",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pautas");
        }
    }
}
