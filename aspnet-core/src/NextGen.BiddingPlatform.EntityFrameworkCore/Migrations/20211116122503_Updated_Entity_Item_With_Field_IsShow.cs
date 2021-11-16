using Microsoft.EntityFrameworkCore.Migrations;

namespace NextGen.BiddingPlatform.Migrations
{
    public partial class Updated_Entity_Item_With_Field_IsShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShow",
                table: "Items",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShow",
                table: "Items");
        }
    }
}
