using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EfficiencyName",
                table: "CostCalculationRetails");

            migrationBuilder.AddColumn<double>(
                name: "EfficiencyValue",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EfficiencyValue",
                table: "CostCalculationRetails");

            migrationBuilder.AddColumn<string>(
                name: "EfficiencyName",
                table: "CostCalculationRetails",
                maxLength: 500,
                nullable: true);
        }
    }
}
