using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerProcessManagement.Migrations
{
    /// <inheritdoc />
    public partial class corrigindo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_LegalPersons_IdLegalPerson",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_PhysicalPersons_IdPhysicalPerson",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "IdPhysicalPerson",
                table: "Contacts",
                newName: "IdPhysicalPersonContact");

            migrationBuilder.RenameColumn(
                name: "IdLegalPerson",
                table: "Contacts",
                newName: "IdLegalPersonContact");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_IdPhysicalPerson",
                table: "Contacts",
                newName: "IX_Contacts_IdPhysicalPersonContact");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_IdLegalPerson",
                table: "Contacts",
                newName: "IX_Contacts_IdLegalPersonContact");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_LegalPersons_IdLegalPersonContact",
                table: "Contacts",
                column: "IdLegalPersonContact",
                principalTable: "LegalPersons",
                principalColumn: "IdLegalPerson",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_PhysicalPersons_IdPhysicalPersonContact",
                table: "Contacts",
                column: "IdPhysicalPersonContact",
                principalTable: "PhysicalPersons",
                principalColumn: "IdPhysicalPerson",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_LegalPersons_IdLegalPersonContact",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_PhysicalPersons_IdPhysicalPersonContact",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "IdPhysicalPersonContact",
                table: "Contacts",
                newName: "IdPhysicalPerson");

            migrationBuilder.RenameColumn(
                name: "IdLegalPersonContact",
                table: "Contacts",
                newName: "IdLegalPerson");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_IdPhysicalPersonContact",
                table: "Contacts",
                newName: "IX_Contacts_IdPhysicalPerson");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_IdLegalPersonContact",
                table: "Contacts",
                newName: "IX_Contacts_IdLegalPerson");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_LegalPersons_IdLegalPerson",
                table: "Contacts",
                column: "IdLegalPerson",
                principalTable: "LegalPersons",
                principalColumn: "IdLegalPerson",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_PhysicalPersons_IdPhysicalPerson",
                table: "Contacts",
                column: "IdPhysicalPerson",
                principalTable: "PhysicalPersons",
                principalColumn: "IdPhysicalPerson",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
