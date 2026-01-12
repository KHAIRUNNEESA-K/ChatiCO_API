using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatiCO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTempIdToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempId",
                table: "Messages");
        }
    }
}
