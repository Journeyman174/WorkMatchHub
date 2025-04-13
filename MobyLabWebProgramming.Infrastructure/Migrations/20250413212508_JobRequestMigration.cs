using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JobRequestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobRequests_User_JobSeekerId",
                table: "JobRequests");

            migrationBuilder.DropIndex(
                name: "IX_JobRequests_JobSeekerId",
                table: "JobRequests");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "JobRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobSeekerId",
                table: "JobRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobRequests_JobSeekerId",
                table: "JobRequests",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobRequests_User_JobSeekerId",
                table: "JobRequests",
                column: "JobSeekerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
