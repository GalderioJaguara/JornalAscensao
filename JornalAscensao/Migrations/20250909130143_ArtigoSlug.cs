using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornalAscensao.Migrations
{
    /// <inheritdoc />
    public partial class ArtigoSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Artigos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Artigos");
        }
    }
}
