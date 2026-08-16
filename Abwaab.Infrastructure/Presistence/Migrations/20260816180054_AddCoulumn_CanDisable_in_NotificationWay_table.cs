using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoulumn_CanDisable_in_NotificationWay_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDisable",
                table: "NotificationWays",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDisable",
                table: "NotificationWays");
        }
    }
}
