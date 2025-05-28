using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegracoesWhatsApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Mensagem = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Destinatario = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Enviado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegracoesWhatsApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SenhaHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adms_Usuarios_Id",
                        column: x => x.Id,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doadores_Usuarios_Id",
                        column: x => x.Id,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Habilidades = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Disponibilidade = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Usuarios_Id",
                        column: x => x.Id,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(100000)", maxLength: 100000, nullable: false),
                    DataEvento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImagemUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CriadoPorAdmId = table.Column<int>(type: "integer", nullable: false),
                    EhRascunho = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EhEventoEspecifico = table.Column<bool>(type: "boolean", nullable: false),
                    TipoEventoEspecifico = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CriadoPorAdmId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_Adms_CriadoPorAdmId",
                        column: x => x.CriadoPorAdmId,
                        principalTable: "Adms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Texto = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
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
                name: "Doacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false),
                    DataDoacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodoPagamento = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DoadorId = table.Column<int>(type: "integer", nullable: false),
                    ComprovanteUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ProjetoVoluntarios",
                columns: table => new
                {
                    ProjetoId = table.Column<int>(type: "integer", nullable: false),
                    VoluntarioId = table.Column<int>(type: "integer", nullable: false),
                    Acao = table.Column<int>(type: "integer", nullable: false),
                    DataInscricao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FuncaoDesejada = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoVoluntarios", x => new { x.ProjetoId, x.VoluntarioId });
                    table.ForeignKey(
                        name: "FK_ProjetoVoluntarios_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoVoluntarios_Voluntarios_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adms_Login",
                table: "Adms",
                column: "Login",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_CriadoPorAdmId",
                table: "Projetos",
                column: "CriadoPorAdmId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoVoluntarios_VoluntarioId",
                table: "ProjetoVoluntarios",
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
                name: "HistoricosAprovacao");

            migrationBuilder.DropTable(
                name: "IntegracoesWhatsApp");

            migrationBuilder.DropTable(
                name: "ProjetoVoluntarios");

            migrationBuilder.DropTable(
                name: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Doadores");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Adms");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
