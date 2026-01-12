using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatiCO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixArchiveUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedChats_Users_UserId1",
                table: "ArchivedChats");

            migrationBuilder.DropIndex(
                name: "IX_ArchivedChats_UserId1",
                table: "ArchivedChats");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ArchivedChats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "ArchivedChats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedChats_UserId1",
                table: "ArchivedChats",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedChats_Users_UserId1",
                table: "ArchivedChats",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
