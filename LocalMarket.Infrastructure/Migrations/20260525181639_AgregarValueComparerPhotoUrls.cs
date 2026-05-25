using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalMarket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarValueComparerPhotoUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorites_businesses_BusinessId",
                table: "favorites");

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_businesses_BusinessId",
                table: "favorites",
                column: "BusinessId",
                principalTable: "businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorites_businesses_BusinessId",
                table: "favorites");

            migrationBuilder.AddForeignKey(
                name: "FK_favorites_businesses_BusinessId",
                table: "favorites",
                column: "BusinessId",
                principalTable: "businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
