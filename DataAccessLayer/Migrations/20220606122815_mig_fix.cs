using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class mig_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Students",
                newName: "studentSurname");

            migrationBuilder.RenameColumn(
                name: "Student_number",
                table: "Students",
                newName: "studentNumber");

            migrationBuilder.RenameColumn(
                name: "Section_name",
                table: "Students",
                newName: "studentName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Students",
                newName: "studentFacultyName");

            migrationBuilder.RenameColumn(
                name: "Faculty_Name",
                table: "Students",
                newName: "studentDepartment");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "studentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "studentSurname",
                table: "Students",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "studentNumber",
                table: "Students",
                newName: "Student_number");

            migrationBuilder.RenameColumn(
                name: "studentName",
                table: "Students",
                newName: "Section_name");

            migrationBuilder.RenameColumn(
                name: "studentFacultyName",
                table: "Students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "studentDepartment",
                table: "Students",
                newName: "Faculty_Name");

            migrationBuilder.RenameColumn(
                name: "studentId",
                table: "Students",
                newName: "Id");
        }
    }
}
