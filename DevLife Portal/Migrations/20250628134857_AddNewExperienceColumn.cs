using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddNewExperienceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "experience",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience",
                table: "users");
        }
    }
}
