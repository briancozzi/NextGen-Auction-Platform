using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExternalLoginApp.Data.Migrations
{
    public partial class Added_New_Entity_UserExternalSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserExternalSessions",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ExpireAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalSessions", x => x.UniqueId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserExternalSessions");
        }
    }
}
