using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesTestTask.Migrations
{
    /// <inheritdoc />
    public partial class ChangeArticleForeignSectionIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articles_sections_SectionId",
                table: "articles");

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                table: "articles",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_articles_sections_SectionId",
                table: "articles",
                column: "SectionId",
                principalTable: "sections",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articles_sections_SectionId",
                table: "articles");

            migrationBuilder.AlterColumn<long>(
                name: "SectionId",
                table: "articles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_articles_sections_SectionId",
                table: "articles",
                column: "SectionId",
                principalTable: "sections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
