using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechTalkIntegrationTests.Infrastructure.Migrations
{
    public partial class Table_Task : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Completed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");
        }
    }
}
