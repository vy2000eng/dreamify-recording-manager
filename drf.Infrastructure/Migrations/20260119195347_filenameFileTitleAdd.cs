using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace drf.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class filenameFileTitleAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Dreams",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Dreams");
        }
    }
}
