using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatiCO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetUserIdToGroupMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetUserId",
                table: "GroupMessages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetUserId",
                table: "GroupMessages");
        }
    }
}
