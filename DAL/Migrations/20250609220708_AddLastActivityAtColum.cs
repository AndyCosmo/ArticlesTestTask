using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ArticlesTestTask.Migrations
{
    /// <inheritdoc />
    public partial class AddLastActivityAtColum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_name",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_articles_tags",
                table: "articles_tags");

            migrationBuilder.DropIndex(
                name: "IX_articles_tags_article_id",
                table: "articles_tags");

            migrationBuilder.DropIndex(
                name: "IX_articles_SectionId",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "id",
                table: "articles_tags");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "articles_tags");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "articles_tags");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "tags",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "tags",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "name_lower",
                table: "tags",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "sections",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "sections",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "articles_count",
                table: "sections",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "articles",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "articles",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_activity_at",
                table: "articles",
                type: "timestamp without time zone",
                nullable: false,
                computedColumnSql: "COALESCE(updated_at, created_at)",
                stored: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_articles_tags",
                table: "articles_tags",
                columns: new[] { "article_id", "tag_id" });

            migrationBuilder.CreateIndex(
                name: "IX_tags_name_lower",
                table: "tags",
                column: "name_lower",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_articles_SectionId_last_activity_at",
                table: "articles",
                columns: new[] { "SectionId", "last_activity_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tags_name_lower",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_articles_tags",
                table: "articles_tags");

            migrationBuilder.DropIndex(
                name: "IX_articles_SectionId_last_activity_at",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "last_activity_at",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "name_lower",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "articles_count",
                table: "sections");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "tags",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "tags",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "sections",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "sections",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<long>(
                name: "id",
                table: "articles_tags",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "articles_tags",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "articles_tags",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "articles",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "articles",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_articles_tags",
                table: "articles_tags",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_name",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_articles_tags_article_id",
                table: "articles_tags",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_articles_SectionId",
                table: "articles",
                column: "SectionId");
        }
    }
}
