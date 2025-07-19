using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weblog.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class add_newsletter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsLetterSubscriptions",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsLetterSubscriptions", x => x.Id);
                    table.UniqueConstraint("AK_NewsLetterSubscriptions_Email", x => x.Email);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsLetterSubscriptions");
        }
    }
}
