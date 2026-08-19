using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abwaab.Infrastructure.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_AttributeDataType_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Attributes");

            migrationBuilder.AddColumn<Guid>(
                name: "AttributeDataTypeId",
                table: "Attributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AttributeDataTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDataTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_AttributeDataTypeId",
                table: "Attributes",
                column: "AttributeDataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDataTypes_Name",
                table: "AttributeDataTypes",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_AttributeDataTypes_AttributeDataTypeId",
                table: "Attributes",
                column: "AttributeDataTypeId",
                principalTable: "AttributeDataTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_AttributeDataTypes_AttributeDataTypeId",
                table: "Attributes");

            migrationBuilder.DropTable(
                name: "AttributeDataTypes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_AttributeDataTypeId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "AttributeDataTypeId",
                table: "Attributes");

            migrationBuilder.AddColumn<int>(
                name: "DataType",
                table: "Attributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
