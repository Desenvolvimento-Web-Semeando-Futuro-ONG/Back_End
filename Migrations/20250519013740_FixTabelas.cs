using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class FixTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Adms_CriadoPorAdmId",
                table: "Projetos");

            migrationBuilder.AddColumn<string>(
                name: "FuncaoDesejada",
                table: "ProjetoVoluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Adms_CriadoPorAdmId",
                table: "Projetos",
                column: "CriadoPorAdmId",
                principalTable: "Adms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Adms_CriadoPorAdmId",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "FuncaoDesejada",
                table: "ProjetoVoluntarios");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Adms_CriadoPorAdmId",
                table: "Projetos",
                column: "CriadoPorAdmId",
                principalTable: "Adms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
