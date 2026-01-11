using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R6tracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConsoleGuiFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DatePlayed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "R6Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MatchesPlayed = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    KillDeathRatio = table.Column<double>(type: "float", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R6Players", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessions");

            migrationBuilder.DropTable(
                name: "R6Players");
        }
    }
}
