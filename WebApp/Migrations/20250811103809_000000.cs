using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class _000000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetRoles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    fullName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    birthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    userName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    emailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: true),
                    securityStamp = table.Column<string>(type: "text", nullable: true),
                    concurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    phoneNumber = table.Column<string>(type: "text", nullable: true),
                    phoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    twoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    accessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetUsers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleId = table.Column<string>(type: "text", nullable: false),
                    claimType = table.Column<string>(type: "text", nullable: true),
                    claimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetRoleClaims", x => x.id);
                    table.ForeignKey(
                        name: "fK_AspNetRoleClaims_AspNetRoles_roleId",
                        column: x => x.roleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<string>(type: "text", nullable: false),
                    claimType = table.Column<string>(type: "text", nullable: true),
                    claimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetUserClaims", x => x.id);
                    table.ForeignKey(
                        name: "fK_AspNetUserClaims_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    loginProvider = table.Column<string>(type: "text", nullable: false),
                    providerKey = table.Column<string>(type: "text", nullable: false),
                    providerDisplayName = table.Column<string>(type: "text", nullable: true),
                    userId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetUserLogins", x => new { x.loginProvider, x.providerKey });
                    table.ForeignKey(
                        name: "fK_AspNetUserLogins_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    roleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetUserRoles", x => new { x.userId, x.roleId });
                    table.ForeignKey(
                        name: "fK_AspNetUserRoles_AspNetRoles_roleId",
                        column: x => x.roleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_AspNetUserRoles_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    loginProvider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_AspNetUserTokens", x => new { x.userId, x.loginProvider, x.name });
                    table.ForeignKey(
                        name: "fK_AspNetUserTokens_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    courseId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    courseCode = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    courseName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    credits = table.Column<int>(type: "integer", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    teacherId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_courses", x => x.courseId);
                    table.ForeignKey(
                        name: "fK_courses_users_teacherId",
                        column: x => x.teacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assignedUsers",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    courseId = table.Column<string>(type: "character varying(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_assignedUsers", x => new { x.userId, x.courseId });
                    table.ForeignKey(
                        name: "fK_assignedUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_assignedUsers_courses_courseId",
                        column: x => x.courseId,
                        principalTable: "courses",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_AspNetRoleClaims_roleId",
                table: "AspNetRoleClaims",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_AspNetUserClaims_userId",
                table: "AspNetUserClaims",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_AspNetUserLogins_userId",
                table: "AspNetUserLogins",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_AspNetUserRoles_roleId",
                table: "AspNetUserRoles",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_assignedUsers_courseId",
                table: "assignedUsers",
                column: "courseId");

            migrationBuilder.CreateIndex(
                name: "iX_courses_teacherId",
                table: "courses",
                column: "teacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "assignedUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
