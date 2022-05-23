using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STS.DataAccessLayer.Migrations
{
    public partial class mig_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Role_RoleId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_Title",
                table: "Permission",
                newName: "IX_Permission_Title");

            migrationBuilder.RenameIndex(
                name: "IX_Permissions_RoleId",
                table: "Permission",
                newName: "IX_Permission_RoleId");

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId",
                table: "Role",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ApllicationId",
                table: "Permission",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ApplicationId",
                table: "Permission",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SecretKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_ApplicationId",
                table: "Role",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ApplicationId",
                table: "Permission",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Application_SecretKey_Id",
                table: "Application",
                columns: new[] { "SecretKey", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Application_Title",
                table: "Application",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Application_ApplicationId",
                table: "Permission",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Role_RoleId",
                table: "Permission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Application_ApplicationId",
                table: "Role",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Application_ApplicationId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Role_RoleId",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Application_ApplicationId",
                table: "Role");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropIndex(
                name: "IX_Role_ApplicationId",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.DropIndex(
                name: "IX_Permission_ApplicationId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "ApllicationId",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "Permissions");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_Title",
                table: "Permissions",
                newName: "IX_Permissions_Title");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_RoleId",
                table: "Permissions",
                newName: "IX_Permissions_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permissions",
                table: "Permissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Role_RoleId",
                table: "Permissions",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
