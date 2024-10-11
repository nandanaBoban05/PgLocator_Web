using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class pg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pid",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pid",
                table: "Room");
        }
    }
}
