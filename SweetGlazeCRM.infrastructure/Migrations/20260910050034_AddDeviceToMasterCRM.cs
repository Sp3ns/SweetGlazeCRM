using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweetGlazeCRM.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceToMasterCRM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId_DeviceCode",
                table: "Devices",
                columns: new[] { "CompanyId", "DeviceCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_CompanyId_DeviceCode",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Companies_CompanyId",
                table: "Devices",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
