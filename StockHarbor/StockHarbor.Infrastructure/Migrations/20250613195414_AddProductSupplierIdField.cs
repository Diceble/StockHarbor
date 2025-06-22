using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockHarbor.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSupplierIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSupplierId",
                table: "ProductSuppliers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSupplierId",
                table: "ProductSuppliers");
        }
    }
}
