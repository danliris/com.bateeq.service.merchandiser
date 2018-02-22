using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commission",
                table: "CostCalculationGarments");

            migrationBuilder.AddColumn<double>(
                name: "CommissionPortion",
                table: "CostCalculationGarments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CommissionRate",
                table: "CostCalculationGarments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionPortion",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "CostCalculationGarments");

            migrationBuilder.AddColumn<double>(
                name: "Commission",
                table: "CostCalculationGarments",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
