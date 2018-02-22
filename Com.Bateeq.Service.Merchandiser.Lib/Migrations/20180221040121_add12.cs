using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCalculationGarments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessoriesAllowance = table.Column<double>(type: "float", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Article = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commission = table.Column<double>(type: "float", nullable: false),
                    Commodity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmPrice = table.Column<double>(type: "float", nullable: false),
                    ConvectionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EfficiencyId = table.Column<int>(type: "int", nullable: false),
                    EfficiencyValue = table.Column<double>(type: "float", nullable: false),
                    FabricAllowance = table.Column<double>(type: "float", nullable: false),
                    Freight = table.Column<double>(type: "float", nullable: false),
                    FreightCost = table.Column<double>(type: "float", nullable: false),
                    Index = table.Column<double>(type: "float", nullable: false),
                    Insurance = table.Column<double>(type: "float", nullable: false),
                    LeadTime = table.Column<int>(type: "int", nullable: false),
                    NETFOB = table.Column<double>(type: "float", nullable: false),
                    NETFOBP = table.Column<double>(type: "float", nullable: false),
                    OTL1CalculatedRate = table.Column<double>(type: "float", nullable: false),
                    OTL1Id = table.Column<int>(type: "int", nullable: false),
                    OTL1Rate = table.Column<double>(type: "float", nullable: false),
                    OTL2CalculatedRate = table.Column<double>(type: "float", nullable: false),
                    OTL2Id = table.Column<int>(type: "int", nullable: false),
                    OTL2Rate = table.Column<double>(type: "float", nullable: false),
                    ProductionCost = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RateDollarId = table.Column<int>(type: "int", nullable: false),
                    RateDollarRate = table.Column<double>(type: "float", nullable: false),
                    Risk = table.Column<double>(type: "float", nullable: false),
                    SMV_Cutting = table.Column<int>(type: "int", nullable: false),
                    SMV_Finishing = table.Column<int>(type: "int", nullable: false),
                    SMV_Sewing = table.Column<int>(type: "int", nullable: false),
                    SMV_Total = table.Column<int>(type: "int", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<int>(type: "int", nullable: false),
                    SizeRangeId = table.Column<int>(type: "int", nullable: false),
                    SizeRangeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    THRId = table.Column<int>(type: "int", nullable: false),
                    THRRate = table.Column<double>(type: "float", nullable: false),
                    WageId = table.Column<int>(type: "int", nullable: false),
                    WageRate = table.Column<double>(type: "float", nullable: false),
                    _CreatedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _DeletedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    _LastModifiedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCalculationGarments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCalculationGarment_Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    BudgetQuantity = table.Column<double>(type: "float", nullable: false),
                    CMT_Price = table.Column<double>(type: "float", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conversion = table.Column<double>(type: "float", nullable: false),
                    CostCalculationGarmentId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    ShippingFeePortion = table.Column<double>(type: "float", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    TotalShippingFee = table.Column<double>(type: "float", nullable: false),
                    UOMPriceId = table.Column<int>(type: "int", nullable: false),
                    UOMPriceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UOMQuantityId = table.Column<int>(type: "int", nullable: false),
                    UOMQuantityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _CreatedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _DeletedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _DeletedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    _LastModifiedAgent = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    _LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isFabricCMT = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCalculationGarment_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCalculationGarment_Materials_CostCalculationGarments_CostCalculationGarmentId",
                        column: x => x.CostCalculationGarmentId,
                        principalTable: "CostCalculationGarments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCalculationGarment_Materials_CostCalculationGarmentId",
                table: "CostCalculationGarment_Materials",
                column: "CostCalculationGarmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCalculationGarment_Materials");

            migrationBuilder.DropTable(
                name: "CostCalculationGarments");
        }
    }
}
