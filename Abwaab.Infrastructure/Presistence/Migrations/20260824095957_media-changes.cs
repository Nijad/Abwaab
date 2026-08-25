using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class mediachanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Advertisments_AdvertismentId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Advertisments_AdvertismentId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Advertisments");

            migrationBuilder.DropIndex(
                name: "IX_Media_AdvertismentId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "AdvertismentId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "StoragePath",
                table: "Media");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfView",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Media",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Media",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCover",
                table: "Media",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Media",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StoredFileName",
                table: "Media",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfView",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IsCover",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "StoredFileName",
                table: "Media");

            migrationBuilder.AddColumn<Guid>(
                name: "AdvertismentId",
                table: "Media",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoragePath",
                table: "Media",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Advertisments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    EndDisplayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDisplayDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_AdvertismentId",
                table: "Media",
                column: "AdvertismentId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_EndDisplayDate",
                table: "Advertisments",
                column: "EndDisplayDate");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisments_StartDisplayDate",
                table: "Advertisments",
                column: "StartDisplayDate");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Advertisments_AdvertismentId",
                table: "Media",
                column: "AdvertismentId",
                principalTable: "Advertisments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Advertisments_AdvertismentId",
                table: "Payments",
                column: "AdvertismentId",
                principalTable: "Advertisments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
