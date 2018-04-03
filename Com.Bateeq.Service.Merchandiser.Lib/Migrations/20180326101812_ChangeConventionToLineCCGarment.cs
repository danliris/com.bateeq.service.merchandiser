using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class ChangeConventionToLineCCGarment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvectionId",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "ConvectionName",
                table: "CostCalculationGarments");

            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "CostCalculationGarments",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LineName",
                table: "CostCalculationGarments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineId",
                table: "CostCalculationGarments");

            migrationBuilder.DropColumn(
                name: "LineName",
                table: "CostCalculationGarments");

            migrationBuilder.AddColumn<string>(
                name: "ConvectionId",
                table: "CostCalculationGarments",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConvectionName",
                table: "CostCalculationGarments",
                maxLength: 500,
                nullable: true);
        }
    }
}
