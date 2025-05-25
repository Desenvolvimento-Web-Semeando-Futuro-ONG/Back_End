using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class AjusteProjetoVoluntario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Acao",
                table: "ProjetoVoluntarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HistoricosAprovacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjetoId = table.Column<int>(type: "integer", nullable: false),
                    VoluntarioId = table.Column<int>(type: "integer", nullable: false),
                    AdministradorId = table.Column<int>(type: "integer", nullable: false),
                    Acao = table.Column<int>(type: "integer", nullable: false),
                    DataAcao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricosAprovacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricosAprovacao_Adms_AdministradorId",
                        column: x => x.AdministradorId,
                        principalTable: "Adms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricosAprovacao_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricosAprovacao_Voluntarios_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosAprovacao_AdministradorId",
                table: "HistoricosAprovacao",
                column: "AdministradorId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosAprovacao_ProjetoId",
                table: "HistoricosAprovacao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricosAprovacao_VoluntarioId",
                table: "HistoricosAprovacao",
                column: "VoluntarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricosAprovacao");

            migrationBuilder.DropColumn(
                name: "Acao",
                table: "ProjetoVoluntarios");
        }
    }
}
