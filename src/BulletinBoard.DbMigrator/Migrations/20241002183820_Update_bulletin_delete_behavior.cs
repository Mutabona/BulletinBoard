using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulletinBoard.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class Update_bulletin_delete_behavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bulletins_Categories_CategoryId",
                table: "Bulletins");

            migrationBuilder.AddForeignKey(
                name: "FK_Bulletins_Categories_CategoryId",
                table: "Bulletins",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bulletins_Categories_CategoryId",
                table: "Bulletins");

            migrationBuilder.AddForeignKey(
                name: "FK_Bulletins_Categories_CategoryId",
                table: "Bulletins",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
