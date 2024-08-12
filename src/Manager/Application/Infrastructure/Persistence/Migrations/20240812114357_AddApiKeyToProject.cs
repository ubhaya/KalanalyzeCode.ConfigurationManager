using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KalanalyzeCode.ConfigurationManager.Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKeyToProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApiKey",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Projects");
        }
    }
}
