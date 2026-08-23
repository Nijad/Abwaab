using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class remove_day_column_from_timeslot_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_EndTime",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_StartTime",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "TimeSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Day",
                table: "TimeSlots",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_EndTime",
                table: "TimeSlots",
                column: "EndTime");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_StartTime",
                table: "TimeSlots",
                column: "StartTime");
        }
    }
}
