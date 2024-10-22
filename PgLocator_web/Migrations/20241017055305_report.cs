using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReportedToAdmin",
                table: "Review",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "ReportedToAdmin",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "Review");
        }
    }
}
