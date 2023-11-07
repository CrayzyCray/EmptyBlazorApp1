using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class mgr13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HashTags_Communities_CommunityId",
                table: "HashTags");

            migrationBuilder.DropIndex(
                name: "IX_HashTags_CommunityId",
                table: "HashTags");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "HashTags");

            migrationBuilder.CreateTable(
                name: "CommunityCommunityHashTag",
                columns: table => new
                {
                    CommunityId = table.Column<int>(type: "INTEGER", nullable: false),
                    HashTagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityCommunityHashTag", x => new { x.CommunityId, x.HashTagsId });
                    table.ForeignKey(
                        name: "FK_CommunityCommunityHashTag_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityCommunityHashTag_HashTags_HashTagsId",
                        column: x => x.HashTagsId,
                        principalTable: "HashTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityCommunityHashTag_HashTagsId",
                table: "CommunityCommunityHashTag",
                column: "HashTagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityCommunityHashTag");

            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "HashTags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HashTags_CommunityId",
                table: "HashTags",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_HashTags_Communities_CommunityId",
                table: "HashTags",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }
    }
}
