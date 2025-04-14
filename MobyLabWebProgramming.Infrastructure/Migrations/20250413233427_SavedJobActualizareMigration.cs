using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SavedJobActualizareMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SavedJob_UserId_JobOfferId",
                table: "SavedJob",
                columns: new[] { "UserId", "JobOfferId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob");

            migrationBuilder.DropIndex(
                name: "IX_SavedJob_UserId_JobOfferId",
                table: "SavedJob");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedJob",
                table: "SavedJob",
                columns: new[] { "UserId", "JobOfferId" });
        }
    }
}
