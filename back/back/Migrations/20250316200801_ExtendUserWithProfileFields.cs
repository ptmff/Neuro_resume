using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using back.Models;

#nullable disable

namespace back.Migrations
{
    /// <inheritdoc />
    public partial class ExtendUserWithProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Resumes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<Experience>>(
                name: "Experience",
                table: "Resumes",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Template",
                table: "Resumes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "Template",
                table: "Resumes");
        }
    }
}
