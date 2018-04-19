using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class _1_AddPoNumberForBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "CostCalculationGarments");

            migrationBuilder.AddColumn<int>(
                name: "RO_SerialNumber",
                table: "CostCalculationRetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<int>(
                name: "RO_SerialNumber",
                table: "CostCalculationGarments",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
            migrationBuilder.DropColumn(
                name: "RO_SerialNumber",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "PO",
                table: "CostCalculationRetail_Materials");

            migrationBuilder.DropColumn(
                name: "PO_SerialNumber",
                table: "CostCalculationRetail_Materials");

            migrationBuilder.DropColumn(
                name: "RO_SerialNumber",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "PO",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.DropColumn(
                name: "PO_SerialNumber",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "CostCalculationRetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "CostCalculationGarments",
                nullable: false,
                defaultValue: 0);
        }
    }
}
