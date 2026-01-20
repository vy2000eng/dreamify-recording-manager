using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace drf.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedUserIdToDream : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DreamId",
                table: "Dreams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DreamId",
                table: "Dreams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
