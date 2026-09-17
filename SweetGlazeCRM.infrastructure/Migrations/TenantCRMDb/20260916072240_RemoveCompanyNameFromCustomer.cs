using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SweetGlazeCRM.infrastructure.Migrations.TenantCRMDb
{
    /// <inheritdoc />
    public partial class RemoveCompanyNameFromCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Customers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Customers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
