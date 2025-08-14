using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class _000007 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_assignedUsers_AspNetUsers_userId",
                table: "assignedUsers");

            migrationBuilder.AddForeignKey(
                name: "fK_assignedUsers_users_userId",
                table: "assignedUsers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_assignedUsers_users_userId",
                table: "assignedUsers");

            migrationBuilder.AddForeignKey(
                name: "fK_assignedUsers_AspNetUsers_userId",
                table: "assignedUsers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
