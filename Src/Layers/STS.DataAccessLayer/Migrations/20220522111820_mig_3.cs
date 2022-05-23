using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STS.DataAccessLayer.Migrations
{
    public partial class mig_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permission_Title",
                table: "Permission");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "Role",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Caption_ApplicationId",
                table: "Role",
                columns: new[] { "Caption", "ApplicationId" },
                unique: true,
                filter: "[Caption] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Title_ApllicationId",
                table: "Permission",
                columns: new[] { "Title", "ApllicationId" },
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Role_Caption_ApplicationId",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Permission_Title_ApllicationId",
                table: "Permission");

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Title",
                table: "Permission",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }
    }
}
