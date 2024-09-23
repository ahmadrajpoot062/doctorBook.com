using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doctorBook.com.Data.Migrations
{
    /// <inheritdoc />
    public partial class myAppUsers : Migration
    {
        public string First_Name { get; internal set; }
        public string Last_Name { get; internal set; }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "First_Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Last_Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Last_Name",
                table: "AspNetUsers");
        }
    }
}
