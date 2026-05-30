using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Peng.Modules.Courses.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EnrollmentMemberId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "courses",
                table: "Enrollments",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_CourseId_UserId",
                schema: "courses",
                table: "Enrollments",
                newName: "IX_Enrollments_CourseId_MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberId",
                schema: "courses",
                table: "Enrollments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_CourseId_MemberId",
                schema: "courses",
                table: "Enrollments",
                newName: "IX_Enrollments_CourseId_UserId");
        }
    }
}
