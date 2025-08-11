using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class _000002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "AspNetUsers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "AspNetUsers",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "AspNetUsers",
                type: "character varying(24)",
                maxLength: 24,
                nullable: true);
        }
    }
}
