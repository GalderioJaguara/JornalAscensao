using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornalAscensao.Migrations
{
    /// <inheritdoc />
    public partial class Artigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artigos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Gancho = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Referencias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Aprovado = table.Column<bool>(type: "bit", nullable: false),
                    Publicado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PautaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RevisorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Criado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Atualizado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Excluido = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artigos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artigos_AspNetUsers_AutorId",
                        column: x => x.AutorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigos_AspNetUsers_RevisorId",
                        column: x => x.RevisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Artigos_Pautas_PautaId",
                        column: x => x.PautaId,
                        principalTable: "Pautas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_AutorId",
                table: "Artigos",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_PautaId",
                table: "Artigos",
                column: "PautaId");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_RevisorId",
                table: "Artigos",
                column: "RevisorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artigos");
        }
    }
}
