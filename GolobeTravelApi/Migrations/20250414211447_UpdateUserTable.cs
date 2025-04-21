using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolobeTravelApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TblUser",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "TblUser",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "TblUser",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "TblUser");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "TblUser");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "TblUser",
                newName: "Name");
        }
    }
}
