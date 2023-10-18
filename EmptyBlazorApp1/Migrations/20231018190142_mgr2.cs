using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmptyBlazorApp1.Migrations
{
    /// <inheritdoc />
    public partial class mgr2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialNetworkLink",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "UserProfile",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "SocialNetworkLink",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "University",
                table: "UserProfile");
        }
    }
}
