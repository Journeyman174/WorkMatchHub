using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JobOfferMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_User_RecruiterId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_RecruiterId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "JobOffers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecruiterId",
                table: "JobOffers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_RecruiterId",
                table: "JobOffers",
                column: "RecruiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_User_RecruiterId",
                table: "JobOffers",
                column: "RecruiterId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
