using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Petshop.Migrations
{
    public partial class CriarBanco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Sobrenome = table.Column<string>(nullable: true),
                    DataNasc = table.Column<DateTime>(nullable: false),
                    Cpf = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Especies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    PorteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Sobrenome = table.Column<string>(nullable: true),
                    DataNasc = table.Column<DateTime>(nullable: false),
                    Cpf = table.Column<string>(nullable: true),
                    Matricula = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Portes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoServico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Animais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    DataNasc = table.Column<DateTime>(nullable: false),
                    EspecieId = table.Column<int>(nullable: true),
                    ClienteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animais_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Animais_Especies_EspecieId",
                        column: x => x.EspecieId,
                        principalTable: "Especies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Preco = table.Column<double>(nullable: false),
                    Duracao = table.Column<int>(nullable: false),
                    TipoServicoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Servicos_TipoServico_TipoServicoId",
                        column: x => x.TipoServicoId,
                        principalTable: "TipoServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atendimentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHora = table.Column<DateTime>(nullable: false),
                    AnimalId = table.Column<int>(nullable: true),
                    FuncionarioId = table.Column<int>(nullable: true),
                    Duracao = table.Column<int>(nullable: false),
                    Preco = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Animais_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atendimentos_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtendimentoServicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicoId = table.Column<int>(nullable: true),
                    AtendimentoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentoServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtendimentoServicos_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AtendimentoServicos_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Especies",
                columns: new[] { "Id", "Descricao", "Nome", "PorteId" },
                values: new object[,]
                {
                    { 1, "Pássaro com topete", "Calopsita", 1 },
                    { 2, "Gato comum", "Gato", 2 },
                    { 3, "Cachorro salsicha", "Dachshund", 3 },
                    { 4, "Cachorro salsicha", "Dachshund", 3 },
                    { 5, "Cachorro labrador", "Labrador Retriever", 4 }
                });

            migrationBuilder.InsertData(
                table: "Portes",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Animais de 0cm a 15cm de comprimento", "Muito Pequeno" },
                    { 2, "Animais de 15,1cm a 25cm de comprimento", "Pequeno" },
                    { 3, "Animais de 25,5cm a 50cm de comprimento", "Médio" },
                    { 4, "Animais com mais de 50,1cm de comprimento", "Grande" }
                });

            migrationBuilder.InsertData(
                table: "TipoServico",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Qualquer procedimento que não envolva cirurgia ou medicação", "Estético" },
                    { 2, "Qualquer procedimento cirurgico ou medicinal", "Médico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animais_ClienteId",
                table: "Animais",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Animais_EspecieId",
                table: "Animais",
                column: "EspecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_AnimalId",
                table: "Atendimentos",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Atendimentos_FuncionarioId",
                table: "Atendimentos",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentoServicos_AtendimentoId",
                table: "AtendimentoServicos",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentoServicos_ServicoId",
                table: "AtendimentoServicos",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_TipoServicoId",
                table: "Servicos",
                column: "TipoServicoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtendimentoServicos");

            migrationBuilder.DropTable(
                name: "Portes");

            migrationBuilder.DropTable(
                name: "Atendimentos");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "Animais");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "TipoServico");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Especies");
        }
    }
}
