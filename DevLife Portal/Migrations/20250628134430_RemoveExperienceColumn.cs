using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExperienceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "experience",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
