using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhoneNumberColumnFromContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "contacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "contacts",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
