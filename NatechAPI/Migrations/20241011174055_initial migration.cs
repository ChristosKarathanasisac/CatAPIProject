using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatechAPI.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatTags",
                columns: table => new
                {
                    CatEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatTags", x => new { x.CatEntityId, x.TagEntityId });
                    table.ForeignKey(
                        name: "FK_CatTags_Cats_CatEntityId",
                        column: x => x.CatEntityId,
                        principalTable: "Cats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatTags_Tags_TagEntityId",
                        column: x => x.TagEntityId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatTags_TagEntityId",
                table: "CatTags",
                column: "TagEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatTags");

            migrationBuilder.DropTable(
                name: "Cats");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
