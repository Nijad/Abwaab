using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefultColumnToPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultPlan",
                table: "Plans",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPlan",
                table: "Plans");
        }
    }
}
