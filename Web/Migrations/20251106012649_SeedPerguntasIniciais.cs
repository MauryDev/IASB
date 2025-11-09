using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedPerguntasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perguntas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Texto = table.Column<string>(type: "TEXT", nullable: false),
                    Ordem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perguntas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sabados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    Trimestre = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    AlunosPresentes = table.Column<int>(type: "INTEGER", nullable: false),
                    EstudoDiarioBibliaLicao = table.Column<int>(type: "INTEGER", nullable: false),
                    ParticipacaoPequenoGrupo = table.Column<int>(type: "INTEGER", nullable: false),
                    EstudosBiblicosDados = table.Column<int>(type: "INTEGER", nullable: false),
                    OutrasAtividadesMissionarias = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sabados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SabadoAlunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SabadoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PessoaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SabadoAlunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SabadoAlunos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SabadoAlunos_Sabados_SabadoId",
                        column: x => x.SabadoId,
                        principalTable: "Sabados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Perguntas",
                columns: new[] { "Id", "Ordem", "Texto" },
                values: new object[,]
                {
                    { 1, 1, "NÚMERO DE ALUNOS PRESENTES:" },
                    { 2, 2, "QUANTOS ESTUDARAM DIARIAMENTE A BÍBLIA E A LIÇÃO?" },
                    { 3, 3, "QUANTOS PARTICIPARAM DE UM PEQUENO GRUPO NESTA SEMANA?" },
                    { 4, 4, "QUANTOS DERAM ALGUM ESTUDO BÍBLICO NESTA SEMANA?" },
                    { 5, 5, "QUANTOS REALIZARAM ALGUMA OUTRA ATIVIDADE MISSIONÁRIA UTILIZANDO SEU TALENTO?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SabadoAlunos_PessoaId",
                table: "SabadoAlunos",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_SabadoAlunos_SabadoId",
                table: "SabadoAlunos",
                column: "SabadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perguntas");

            migrationBuilder.DropTable(
                name: "SabadoAlunos");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Sabados");
        }
    }
}
