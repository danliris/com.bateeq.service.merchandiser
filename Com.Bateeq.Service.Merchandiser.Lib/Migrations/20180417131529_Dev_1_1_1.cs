using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class Dev_1_1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "CostCalculationRetails",
                newName: "RO_SerialNumber");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "CostCalculationGarments",
                newName: "RO_SerialNumber");

            migrationBuilder.AddColumn<string>(
                name: "PO",
                table: "CostCalculationRetail_Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PO_SerialNumber",
                table: "CostCalculationRetail_Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PO",
                table: "CostCalculationGarment_Materials",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PO_SerialNumber",
                table: "CostCalculationGarment_Materials",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RO_SerialNumber",
                table: "CostCalculationRetails",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "RO_SerialNumber",
                table: "CostCalculationGarments",
                newName: "SerialNumber");

            migrationBuilder.DropColumn(
                name: "PO",
                table: "CostCalculationRetail_Materials");

            migrationBuilder.DropColumn(
                name: "PO_SerialNumber",
                table: "CostCalculationRetail_Materials");
            
            migrationBuilder.DropColumn(
                name: "PO",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.DropColumn(
                name: "PO_SerialNumber",
                table: "CostCalculationGarment_Materials");
        }
    }
}
