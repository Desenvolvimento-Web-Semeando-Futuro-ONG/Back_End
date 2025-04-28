using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class InitialNewCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adms_Usuarios_Id",
                table: "Adms");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Adms",
                newName: "SenhaHash");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Adms",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Adms",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Adms",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Adms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Adms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Adms",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Adms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Adms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Doadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataEvento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImagemUrl = table.Column<string>(type: "text", nullable: true),
                    CriadoPorAdmId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Adms_CriadoPorAdmId",
                        column: x => x.CriadoPorAdmId,
                        principalTable: "Adms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegracoesWhatsApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mensagem = table.Column<string>(type: "text", nullable: false),
                    Destinatario = table.Column<string>(type: "text", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Enviado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracoesWhatsApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Texto = table.Column<string>(type: "text", nullable: false),
                    DataPublicacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdmId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Adms_AdmId",
                        column: x => x.AdmId,
                        principalTable: "Adms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Habilidades = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Disponibilidade = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    DataDoacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodoPagamento = table.Column<string>(type: "text", nullable: false),
                    DoadorId = table.Column<int>(type: "integer", nullable: false),
                    ComprovanteUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doacoes_Doadores_DoadorId",
                        column: x => x.DoadorId,
                        principalTable: "Doadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventoVoluntarios",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    VoluntarioId = table.Column<int>(type: "integer", nullable: false),
                    DataInscricao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoVoluntarios", x => new { x.EventoId, x.VoluntarioId });
                    table.ForeignKey(
                        name: "FK_EventoVoluntarios_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoVoluntarios_Voluntarios_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doacoes_DoadorId",
                table: "Doacoes",
                column: "DoadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CriadoPorAdmId",
                table: "Eventos",
                column: "CriadoPorAdmId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoVoluntarios_VoluntarioId",
                table: "EventoVoluntarios",
                column: "VoluntarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_AdmId",
                table: "Publicacoes",
                column: "AdmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doacoes");

            migrationBuilder.DropTable(
                name: "EventoVoluntarios");

            migrationBuilder.DropTable(
                name: "IntegracoesWhatsApp");

            migrationBuilder.DropTable(
                name: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Doadores");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Adms");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Adms");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Adms");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Adms");

            migrationBuilder.DropColumn(
                name: "Telefone",
                table: "Adms");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Adms");

            migrationBuilder.RenameColumn(
                name: "SenhaHash",
                table: "Adms",
                newName: "Senha");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Adms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Adms",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CPF = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Adms_Usuarios_Id",
                table: "Adms",
                column: "Id",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
