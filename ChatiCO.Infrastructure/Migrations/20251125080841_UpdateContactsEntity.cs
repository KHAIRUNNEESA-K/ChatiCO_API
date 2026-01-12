using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatiCO.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsArchived",
                table: "Contacts",
                newName: "IsBlocked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBlocked",
                table: "Contacts",
                newName: "IsArchived");
        }
    }
}
