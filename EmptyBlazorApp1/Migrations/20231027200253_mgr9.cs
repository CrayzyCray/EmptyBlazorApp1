using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class mgr9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities");

            migrationBuilder.CreateTable(
                name: "HashTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    CommunityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HashTags_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HashTags_CommunityId",
                table: "HashTags",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities");

            migrationBuilder.DropTable(
                name: "HashTags");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
