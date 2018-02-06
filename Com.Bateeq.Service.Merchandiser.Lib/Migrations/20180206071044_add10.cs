using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Merchandiser.Lib.Migrations
{
    public partial class add10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CounterId",
                table: "CostCalculationRetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CounterName",
                table: "CostCalculationRetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "CostCalculationRetails");

            migrationBuilder.DropColumn(
                name: "CounterName",
                table: "CostCalculationRetails");
        }
    }
}
