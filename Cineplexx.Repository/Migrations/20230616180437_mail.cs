using Microsoft.EntityFrameworkCore.Migrations;

namespace Cineplexx.Repository.Migrations
{
    public partial class mail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "EmailMessages");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "EmailMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "EmailMessages");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "EmailMessages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
