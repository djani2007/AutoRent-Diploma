using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileChangeRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewAddress",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewCity",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewDriverLicenseIssueDate",
                table: "ContactMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewDriverLicenseNumber",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewIdentityCardNumber",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewPhoneNumber",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContactMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewAddress",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "NewCity",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "NewDriverLicenseIssueDate",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "NewDriverLicenseNumber",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "NewIdentityCardNumber",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "NewPhoneNumber",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "ContactMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContactMessages");
        }
    }
}
