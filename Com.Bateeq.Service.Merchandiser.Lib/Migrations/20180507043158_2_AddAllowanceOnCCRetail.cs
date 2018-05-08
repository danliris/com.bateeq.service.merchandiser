using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class _2_AddAllowanceOnCCRetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccessoriesAllowance",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FabricAllowance",
                table: "CostCalculationRetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessoriesAllowance",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "FabricAllowance",
                table: "CostCalculationRetails");
        }
    }
}
