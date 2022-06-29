using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STS.DataAccessLayer.Migrations
{
    public partial class mig_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Permission");

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                table: "Permission",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CategoryId",
                table: "Permission",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ApplicationId_Title",
                table: "Category",
                columns: new[] { "ApplicationId", "Title" },
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Category_CategoryId",
                table: "Permission",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Category_CategoryId",
                table: "Permission");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Permission_CategoryId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Permission");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
