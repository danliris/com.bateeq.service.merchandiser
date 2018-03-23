using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CMT_Price",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.DropColumn(
                name: "isFabricCMT",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.AddColumn<double>(
                name: "CM_Price",
                table: "CostCalculationGarment_Materials",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isFabricCM",
                table: "CostCalculationGarment_Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CM_Price",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.DropColumn(
                name: "isFabricCM",
                table: "CostCalculationGarment_Materials");

            migrationBuilder.AddColumn<double>(
                name: "CMT_Price",
                table: "CostCalculationGarment_Materials",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isFabricCMT",
                table: "CostCalculationGarment_Materials",
                nullable: false,
                defaultValue: false);
        }
    }
}
