using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicsAPP.Migrations
{
    /// <inheritdoc />
    public partial class addFeedbackDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FeedbackDate",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeedbackDate",
                table: "Feedbacks");
        }
    }
}
