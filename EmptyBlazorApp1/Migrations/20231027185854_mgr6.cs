using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class mgr6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Subscribers",
                table: "Communities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Communities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Subscribers",
                table: "Communities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
