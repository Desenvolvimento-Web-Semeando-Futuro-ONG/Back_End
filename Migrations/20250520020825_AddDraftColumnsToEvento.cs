using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftColumnsToEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataPublicacao",
                table: "Eventos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EhRascunho",
                table: "Eventos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataPublicacao",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "EhRascunho",
                table: "Eventos");
        }
    }
}
