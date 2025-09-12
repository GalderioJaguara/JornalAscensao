using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornalAscensao.Migrations
{
    /// <inheritdoc />
    public partial class SlugUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Artigos",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Artigos_Slug",
                table: "Artigos",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Artigos_Slug",
                table: "Artigos");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Artigos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
