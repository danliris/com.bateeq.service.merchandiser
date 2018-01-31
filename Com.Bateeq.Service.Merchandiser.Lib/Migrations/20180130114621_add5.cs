using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OLCalculatedRate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL1CalculatedRate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL2CalculatedRate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL3CalculatedRate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OLCalculatedRate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL1CalculatedRate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL2CalculatedRate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL3CalculatedRate",
                table: "CostCalculationRetails");
        }
    }
}
