using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R6tracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddR6Map : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Map",
                table: "GameSessions");

            migrationBuilder.AddColumn<int>(
                name: "MapId",
                table: "GameSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "R6Map",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R6Map", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76a831e1-0164-4f33-a1c3-9ba0e122449c", "AQAAAAIAAYagAAAAELSfs+C14ZeINkUnW+xb1MaOVrwuOV+SEW5mcSNkPQZLjLtF183KpvzzwpK+PEZRGw==", "3dee2736-0c98-43eb-8a5f-1b6518a006b4" });

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_MapId",
                table: "GameSessions",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_R6Map_MapId",
                table: "GameSessions",
                column: "MapId",
                principalTable: "R6Map",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_R6Map_MapId",
                table: "GameSessions");

            migrationBuilder.DropTable(
                name: "R6Map");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_MapId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "GameSessions");

            migrationBuilder.AddColumn<string>(
                name: "Map",
                table: "GameSessions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15637f7a-e82e-478a-98ce-f6dd448538ea", "AQAAAAIAAYagAAAAEB6Jn9Wff+wtXboXAvvRvqHKbQwQJ2Cqn4U0XVrMESgKRCvzrbczczTuBLoPAdyOkg==", "512d5319-4d91-4871-be95-6fdeffb65c45" });
        }
    }
}
