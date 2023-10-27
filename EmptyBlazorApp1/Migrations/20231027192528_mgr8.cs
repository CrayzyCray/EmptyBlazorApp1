using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class mgr8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Communities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_CreatorId",
                table: "Communities",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_Users_CreatorId",
                table: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Communities_CreatorId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Communities");
        }
    }
}
