using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerProcessManagement.Migrations
{
    /// <inheritdoc />
    public partial class corrigindo3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PhysicalPersons",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "LegalPersons",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PhysicalPersons");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "LegalPersons");
        }
    }
}
