using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace drf.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedDreamsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucketKey",
                table: "Dreams");

            migrationBuilder.DropColumn(
                name: "BucketUrl",
                table: "Dreams");

            migrationBuilder.DropColumn(
                name: "RecordedAt",
                table: "Dreams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BucketKey",
                table: "Dreams",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "BucketUrl",
                table: "Dreams",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordedAt",
                table: "Dreams",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
