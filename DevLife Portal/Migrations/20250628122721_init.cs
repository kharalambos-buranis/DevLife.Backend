using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "daily_challenges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_slug = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_challenges", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "bugchase_scores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bugchase_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_bugchase_scores_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_daily_challenges",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_daily_challenges", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_daily_challenges_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_streaks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    current_streak = table.Column<int>(type: "integer", nullable: false),
                    last_completed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_streaks", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_streaks_users",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "zodiac_signs",
                columns: new[] { "id", "daily_tip", "emoji", "end_day", "EndMonth", "lucky_technology", "name", "start_day", "start_month" },
                values: new object[,]
                {
                    { 1, null, "♑", 0, 0, null, "Capricorn", 0, 0 },
                    { 2, null, "♒", 0, 0, null, "Aquarius", 0, 0 },
                    { 3, null, "♓", 0, 0, null, "Pisces", 0, 0 },
                    { 4, null, "♈", 0, 0, null, "Aries", 0, 0 },
                    { 5, null, "♉", 0, 0, null, "Taurus", 0, 0 },
                    { 6, null, "♊", 0, 0, null, "Gemini", 0, 0 },
                    { 7, null, "♋", 0, 0, null, "Cancer", 0, 0 },
                    { 8, null, "♌", 0, 0, null, "Leo", 0, 0 },
                    { 9, null, "♍", 0, 0, null, "Virgo", 0, 0 },
                    { 10, null, "♎", 0, 0, null, "Libra", 0, 0 },
                    { 11, null, "♏", 0, 0, null, "Scorpio", 0, 0 },
                    { 12, null, "♐", 0, 0, null, "Sagittarius", 0, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_bugchase_scores_user_id",
                table: "bugchase_scores",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_daily_challenges_user_id",
                table: "user_daily_challenges",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_streaks_user_id",
                table: "user_streaks",
                column: "user_id");

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
                name: "bugchase_scores");

            migrationBuilder.DropTable(
                name: "daily_challenges");

            migrationBuilder.DropTable(
                name: "user_daily_challenges");

            migrationBuilder.DropTable(
                name: "user_streaks");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "zodiac_signs");
        }
    }
}
