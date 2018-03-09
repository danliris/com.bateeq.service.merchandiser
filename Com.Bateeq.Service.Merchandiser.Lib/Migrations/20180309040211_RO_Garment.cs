using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class RO_Garment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RO_GarmentId",
                table: "CostCalculationGarments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Information",
                table: "CostCalculationGarment_Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RO_Garments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CostCalculationGarmentId = table.Column<int>(type: "int", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RO_Garments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RO_Garment_SizeBreakdowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ColorId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ColorName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RO_GarmentId = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RO_Garment_SizeBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RO_Garment_SizeBreakdowns_RO_Garments_RO_GarmentId",
                        column: x => x.RO_GarmentId,
                        principalTable: "RO_Garments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RO_Garment_SizeBreakdown_Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Information = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RO_Garment_SizeBreakdownId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    SizeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_RO_Garment_SizeBreakdown_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RO_Garment_SizeBreakdown_Details_RO_Garment_SizeBreakdowns_RO_Garment_SizeBreakdownId",
                        column: x => x.RO_Garment_SizeBreakdownId,
                        principalTable: "RO_Garment_SizeBreakdowns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCalculationGarments_RO_GarmentId",
                table: "CostCalculationGarments",
                column: "RO_GarmentId",
                unique: true,
                filter: "[RO_GarmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RO_Garment_SizeBreakdown_Details_RO_Garment_SizeBreakdownId",
                table: "RO_Garment_SizeBreakdown_Details",
                column: "RO_Garment_SizeBreakdownId");

            migrationBuilder.CreateIndex(
                name: "IX_RO_Garment_SizeBreakdowns_RO_GarmentId",
                table: "RO_Garment_SizeBreakdowns",
                column: "RO_GarmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCalculationGarments_RO_Garments_RO_GarmentId",
                table: "CostCalculationGarments",
                column: "RO_GarmentId",
                principalTable: "RO_Garments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCalculationGarments_RO_Garments_RO_GarmentId",
                table: "CostCalculationGarments");

            migrationBuilder.DropTable(
                name: "RO_Garment_SizeBreakdown_Details");

            migrationBuilder.DropTable(
                name: "RO_Garment_SizeBreakdowns");

            migrationBuilder.DropTable(
                name: "RO_Garments");

            migrationBuilder.DropIndex(
                name: "IX_CostCalculationGarments_RO_GarmentId",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "RO_GarmentId",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "Information",
                table: "CostCalculationGarment_Materials");
        }
    }
}
