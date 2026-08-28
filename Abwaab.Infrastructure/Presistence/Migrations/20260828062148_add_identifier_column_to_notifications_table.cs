using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class add_identifier_column_to_notifications_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationWays_Notifications_NotificationId",
                table: "NotificationWays");

            migrationBuilder.DropIndex(
                name: "IX_NotificationWays_NotificationId",
                table: "NotificationWays");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "NotificationWays");

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "NotificationWays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationWays_NotificationId",
                table: "NotificationWays",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationWays_Notifications_NotificationId",
                table: "NotificationWays",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }
    }
}
