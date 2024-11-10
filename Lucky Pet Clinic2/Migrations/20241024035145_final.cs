using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lucky_Pet_Clinic2.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NextVisitType",
                table: "Vaccinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NextVisitType",
                table: "Surgeries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NextVisitType",
                table: "Examinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Examinations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextVisitType",
                table: "Vaccinations");

            migrationBuilder.DropColumn(
                name: "NextVisitType",
                table: "Surgeries");

            migrationBuilder.DropColumn(
                name: "NextVisitType",
                table: "Examinations");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Examinations");
        }
    }
}
