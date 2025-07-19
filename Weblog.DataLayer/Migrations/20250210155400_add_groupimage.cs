using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weblog.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_groupimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupImageName",
                table: "ChatGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupImageName",
                table: "ChatGroups");
        }
    }
}
