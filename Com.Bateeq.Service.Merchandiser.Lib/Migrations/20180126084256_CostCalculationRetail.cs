using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class CostCalculationRetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCalculationRetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Article = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    EfficiencyId = table.Column<int>(type: "int", nullable: false),
                    EfficiencyName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HPP = table.Column<double>(type: "float", nullable: false),
                    OLCost = table.Column<double>(type: "float", nullable: false),
                    OLId = table.Column<int>(type: "int", nullable: false),
                    OTL1Cost = table.Column<double>(type: "float", nullable: false),
                    OTL1Id = table.Column<int>(type: "int", nullable: false),
                    OTL2Cost = table.Column<double>(type: "float", nullable: false),
                    OTL2Id = table.Column<int>(type: "int", nullable: false),
                    OTL3Cost = table.Column<double>(type: "float", nullable: false),
                    OTL3Id = table.Column<int>(type: "int", nullable: false),
                    Proposed20 = table.Column<double>(type: "float", nullable: false),
                    Proposed21 = table.Column<double>(type: "float", nullable: false),
                    Proposed22 = table.Column<double>(type: "float", nullable: false),
                    Proposed23 = table.Column<double>(type: "float", nullable: false),
                    Proposed24 = table.Column<double>(type: "float", nullable: false),
                    Proposed25 = table.Column<double>(type: "float", nullable: false),
                    Proposed26 = table.Column<double>(type: "float", nullable: false),
                    Proposed27 = table.Column<double>(type: "float", nullable: false),
                    Proposed28 = table.Column<double>(type: "float", nullable: false),
                    Proposed29 = table.Column<double>(type: "float", nullable: false),
                    Proposed30 = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Risk = table.Column<double>(type: "float", nullable: false),
                    Rounding20 = table.Column<double>(type: "float", nullable: false),
                    Rounding21 = table.Column<double>(type: "float", nullable: false),
                    Rounding22 = table.Column<double>(type: "float", nullable: false),
                    Rounding23 = table.Column<double>(type: "float", nullable: false),
                    Rounding24 = table.Column<double>(type: "float", nullable: false),
                    Rounding25 = table.Column<double>(type: "float", nullable: false),
                    Rounding26 = table.Column<double>(type: "float", nullable: false),
                    Rounding27 = table.Column<double>(type: "float", nullable: false),
                    Rounding28 = table.Column<double>(type: "float", nullable: false),
                    Rounding29 = table.Column<double>(type: "float", nullable: false),
                    Rounding30 = table.Column<double>(type: "float", nullable: false),
                    RoundingOthers = table.Column<double>(type: "float", nullable: false),
                    SH_Cutting = table.Column<double>(type: "float", nullable: false),
                    SH_Finishing = table.Column<double>(type: "float", nullable: false),
                    SH_Sewing = table.Column<double>(type: "float", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    SeasonName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SizeRangeId = table.Column<int>(type: "int", nullable: false),
                    SizeRangeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StyleId = table.Column<int>(type: "int", nullable: false),
                    StyleName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    WholesalePrice = table.Column<double>(type: "float", nullable: false),
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
                    table.PrimaryKey("PK_CostCalculationRetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCalculationRetail_Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Conversion = table.Column<double>(type: "float", nullable: false),
                    CostCalculationRetailId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    UOMPriceId = table.Column<int>(type: "int", nullable: false),
                    UOMPriceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UOMQuantityId = table.Column<int>(type: "int", nullable: false),
                    UOMQuantityName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_CostCalculationRetail_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCalculationRetail_Materials_CostCalculationRetails_CostCalculationRetailId",
                        column: x => x.CostCalculationRetailId,
                        principalTable: "CostCalculationRetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCalculationRetail_Materials_CostCalculationRetailId",
                table: "CostCalculationRetail_Materials",
                column: "CostCalculationRetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCalculationRetail_Materials");

            migrationBuilder.DropTable(
                name: "CostCalculationRetails");
        }
    }
}
