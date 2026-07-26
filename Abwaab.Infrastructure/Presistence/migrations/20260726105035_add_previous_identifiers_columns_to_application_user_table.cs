using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class add_previous_identifiers_columns_to_application_user_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviousEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousPhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PreviousPhoneNumber",
                table: "AspNetUsers");
        }
    }
}
