using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserExtraFields_Extended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Users",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MainResumeId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Education",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MainResumeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Users");
        }
    }
}
