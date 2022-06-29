using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STS.DataAccessLayer.Migrations
{
    public partial class mig_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Role");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
