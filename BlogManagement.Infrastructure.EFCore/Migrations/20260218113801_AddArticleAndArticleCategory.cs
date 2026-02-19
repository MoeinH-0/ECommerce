using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleAndArticleCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Picture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PictureAlt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PictureTitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ShowOrder = table.Column<int>(type: "integer", nullable: false),
                    Slug = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    Keywords = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MetaDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CanonicalAddress = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PictureAlt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PictureTitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Slug = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    Keywords = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MetaDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CanonicalAddress = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ArticleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ArticleCategories");
        }
    }
}
