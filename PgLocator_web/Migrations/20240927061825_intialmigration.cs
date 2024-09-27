using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PgLocator_web.Migrations
{
    /// <inheritdoc />
    public partial class intialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Lid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Lid);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Mid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Mid);
                });

            migrationBuilder.CreateTable(
                name: "Owner",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lid = table.Column<int>(type: "int", nullable: false),
                    Ownername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Whatsapp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Chatlink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owner", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Pg",
                columns: table => new
                {
                    Pgid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Oid = table.Column<int>(type: "int", nullable: false),
                    Pgname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amentities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foodavailable = table.Column<bool>(type: "bit", nullable: false),
                    Meal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rules = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pg", x => x.Pgid);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Rid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reviewteaxt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reviewdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Rid);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Rid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pid = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Deposit = table.Column<int>(type: "int", nullable: false),
                    Services = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roomtype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Totalroom = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Rid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lid = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Uid);
                });

            migrationBuilder.InsertData(
                table: "Owner",
                columns: new[] { "Oid", "Adress", "Chatlink", "Dob", "Email", "Lid", "Ownername", "Password", "Phone", "Whatsapp" },
                values: new object[,]
                {
                    { 1, "456 Elm Street, Springfield, IL, USA", "https://chat.whatsapp.com/examplelink1", "1990-04-15", "johndoe@example.com", 1, "John", "John", "+1234567890", "+1234567890" },
                    { 2, "789 Maple Avenue, Springfield, IL, USA", "https://chat.whatsapp.com/examplelink2", "1985-09-20", "janesmith@example.com", 102, "Jane", "Jane", "+1987654321", "+1987654321" },
                    { 3, "123 Oak Drive, Springfield, IL, USA", "https://chat.whatsapp.com/examplelink3", "1992-11-30", "alicejohnson@example.com", 103, "Alice", "Alice", "+1123456789", "+1123456789" },
                    { 4, "321 Pine Street, Springfield, IL, USA", "https://chat.whatsapp.com/examplelink4", "1988-05-12", "bobbrown@example.com", 104, "Bob", "Bob", "+1456789012", "+1456789012" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Uid", "Dob", "Email", "Gender", "Lid", "Name", "Password", "Phone", "Status" },
                values: new object[,]
                {
                    { 1, "1990-05-15", "alice.johnson@example.com", "Female", 1, "Alice Johnson", "Alice", "123-456-7890", "Active" },
                    { 2, "1988-09-22", "bob.smith@example.com", "Male", 2, "Bob Smith", "Bob", "987-654-3210", "Inactive" },
                    { 3, "1995-12-03", "charlie.brown@example.com", "Male", 3, "Charlie Brown", "Charlie", "555-123-4567", "Active" },
                    { 4, "1992-03-08", "diana.prince@example.com", "Female", 4, "Diana Prince", "Diana", "444-987-6543", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Owner");

            migrationBuilder.DropTable(
                name: "Pg");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
