using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DevLife_Portal.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
