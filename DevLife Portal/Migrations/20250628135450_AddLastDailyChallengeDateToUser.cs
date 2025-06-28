using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddLastDailyChallengeDateToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastDailyChallengeDate",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDailyChallengeDate",
                table: "users");
        }
    }
}
