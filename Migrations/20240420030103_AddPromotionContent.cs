using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LTWeb_CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotionContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Promotions");
        }
    }
}
