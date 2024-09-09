using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("753796ac-9ce4-47d5-a5e3-8a9634f794fc"),
                columns: new[] { "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime" },
                values: new object[] { "AQAAAAIAAYagAAAAELUpSzMjQ5pjTFxJQ/tj5cthe1/3Q7TBTeEntMlT0D2xgFJoDyJhZAoO6r/W+09E1g==", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "ApplicationUser");

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("753796ac-9ce4-47d5-a5e3-8a9634f794fc"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHg2iLs09M8MGgreyCgTVIY0qfDDTqAzoT1MjofKtGif2o9xfl0FABF97K05/zY2Cg==");
        }
    }
}
