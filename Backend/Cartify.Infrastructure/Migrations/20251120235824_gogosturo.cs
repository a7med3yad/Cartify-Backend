using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cartify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class gogosturo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "LkpProductDetailsAttributes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "LkpProductDetailsAttributes");
        }
    }
}
