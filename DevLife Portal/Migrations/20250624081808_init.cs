using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "zodiac_signs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    emoji = table.Column<string>(type: "text", nullable: false),
                    start_month = table.Column<int>(type: "integer", nullable: false),
                    start_day = table.Column<int>(type: "integer", nullable: false),
                    EndMonth = table.Column<int>(type: "integer", nullable: false),
                    end_day = table.Column<int>(type: "integer", nullable: false),
                    daily_tip = table.Column<string>(type: "text", nullable: true),
                    lucky_technology = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_zodiac_signs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    techno_stack = table.Column<string>(type: "text", nullable: false),
                    experience = table.Column<string>(type: "text", nullable: false),
                    zodiac_sign_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_points = table.Column<int>(type: "integer", nullable: false),
                    streak = table.Column<int>(type: "integer", nullable: false),
                    avatar_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_zodiac_signs",
                        column: x => x.zodiac_sign_id,
                        principalTable: "zodiac_signs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_zodiac_sign_id",
                table: "users",
                column: "zodiac_sign_id");

            migrationBuilder.CreateIndex(
                name: "ux_users_usernames",
                table: "users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "zodiac_signs");
        }
    }
}
