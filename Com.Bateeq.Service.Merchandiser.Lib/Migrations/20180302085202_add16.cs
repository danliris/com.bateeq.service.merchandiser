using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RO_RetailId",
                table: "CostCalculationRetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RO_Retails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ColorId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ColorName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CostCalculationRetailId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RO_Retails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RO_RetailSizeBreakdowns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RO_RetailId = table.Column<int>(type: "int", nullable: false),
                    SizeQuantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_RO_RetailSizeBreakdowns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RO_RetailSizeBreakdowns_RO_Retails_RO_RetailId",
                        column: x => x.RO_RetailId,
                        principalTable: "RO_Retails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostCalculationRetails_RO_RetailId",
                table: "CostCalculationRetails",
                column: "RO_RetailId",
                unique: true,
                filter: "[RO_RetailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RO_RetailSizeBreakdowns_RO_RetailId",
                table: "RO_RetailSizeBreakdowns",
                column: "RO_RetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostCalculationRetails_RO_Retails_RO_RetailId",
                table: "CostCalculationRetails",
                column: "RO_RetailId",
                principalTable: "RO_Retails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostCalculationRetails_RO_Retails_RO_RetailId",
                table: "CostCalculationRetails");

            migrationBuilder.DropTable(
                name: "RO_RetailSizeBreakdowns");

            migrationBuilder.DropTable(
                name: "RO_Retails");

            migrationBuilder.DropIndex(
                name: "IX_CostCalculationRetails_RO_RetailId",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "RO_RetailId",
                table: "CostCalculationRetails");
        }
    }
}
