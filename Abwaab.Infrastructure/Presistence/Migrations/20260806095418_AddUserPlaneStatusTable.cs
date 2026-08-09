using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPlaneStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserPlans");

            migrationBuilder.AddColumn<Guid>(
                name: "UserPlanStateId",
                table: "UserPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserPlansStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlansStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlans_UserPlanStateId",
                table: "UserPlans",
                column: "UserPlanStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlans_UserPlansStatus_UserPlanStateId",
                table: "UserPlans",
                column: "UserPlanStateId",
                principalTable: "UserPlansStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPlans_UserPlansStatus_UserPlanStateId",
                table: "UserPlans");

            migrationBuilder.DropTable(
                name: "UserPlansStatus");

            migrationBuilder.DropIndex(
                name: "IX_UserPlans_UserPlanStateId",
                table: "UserPlans");

            migrationBuilder.DropColumn(
                name: "UserPlanStateId",
                table: "UserPlans");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
