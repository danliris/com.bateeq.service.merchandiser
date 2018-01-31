using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class CostCalculationRetail_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OLCost",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL1Cost",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL2Cost",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL3Cost",
                table: "CostCalculationRetails");

            migrationBuilder.AddColumn<double>(
                name: "OLRate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL1Rate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL2Rate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL3Rate",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OLRate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL1Rate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL2Rate",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "OTL3Rate",
                table: "CostCalculationRetails");

            migrationBuilder.AddColumn<double>(
                name: "OLCost",
                table: "CostCalculationRetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL1Cost",
                table: "CostCalculationRetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL2Cost",
                table: "CostCalculationRetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTL3Cost",
                table: "CostCalculationRetails",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
