using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R6tracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4eeafee4-0065-4b00-9878-b60d6d519684", "AQAAAAIAAYagAAAAEMaNRKqqTRa8wRLNIPXE8h1Q2Ouxf8F2CeeDUrxDWk7JrNo/og3lVVP3vxLoiFJ0ww==", "4f56966b-a5af-4299-b936-19fed427fbb0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d8d3713-1e58-42e3-996d-eed62c03c42c", "AQAAAAIAAYagAAAAEM2ZBR3h87BKELPvNcIkfOdJNm6MB0apK6NzOnuydP5ZxNnWKuqIWGpdlzJTCMKilA==", "ce8ba279-8d1c-413c-9b9d-5b967e0a5c3d" });
        }
    }
}
