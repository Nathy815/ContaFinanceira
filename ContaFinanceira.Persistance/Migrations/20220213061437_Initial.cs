using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContaFinanceira.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbcf_agencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcf_agencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbcf_contas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgenciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcf_contas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbcf_contas_tbcf_agencias_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "tbcf_agencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbcf_clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoPessoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpfCnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    ContaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcf_clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbcf_clientes_tbcf_contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "tbcf_contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbcf_transacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContaId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcf_transacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbcf_transacoes_tbcf_contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "tbcf_contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tbcf_agencias",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 1, "Agência 1" });

            migrationBuilder.InsertData(
                table: "tbcf_agencias",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 2, "Agência 2" });

            migrationBuilder.InsertData(
                table: "tbcf_contas",
                columns: new[] { "Id", "AgenciaId", "DataCriacao", "Senha" },
                values: new object[] { 1, 1, new DateTime(2022, 2, 11, 23, 18, 0, 0, DateTimeKind.Unspecified), "$2a$11$4/WPXcRIcujksmWmnV6bBehoyugcezsR/wQ3Gq1zOKSi0WYuI8svm" });

            migrationBuilder.InsertData(
                table: "tbcf_clientes",
                columns: new[] { "Id", "ContaId", "CpfCnpj", "Nome", "TipoPessoa" },
                values: new object[] { 1, 1, "51865798916", "Nathália Lopes", "PessoaFisica" });

            migrationBuilder.InsertData(
                table: "tbcf_transacoes",
                columns: new[] { "Id", "ContaId", "Data", "Valor" },
                values: new object[] { 1, 1, new DateTime(2022, 2, 11, 23, 18, 0, 0, DateTimeKind.Unspecified), 10m });

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_clientes_ContaId",
                table: "tbcf_clientes",
                column: "ContaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_clientes_Id_ContaId",
                table: "tbcf_clientes",
                columns: new[] { "Id", "ContaId" });

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_contas_AgenciaId",
                table: "tbcf_contas",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_contas_Id",
                table: "tbcf_contas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_transacoes_ContaId",
                table: "tbcf_transacoes",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_transacoes_Id_Data_Valor",
                table: "tbcf_transacoes",
                columns: new[] { "Id", "Data", "Valor" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbcf_clientes");

            migrationBuilder.DropTable(
                name: "tbcf_transacoes");

            migrationBuilder.DropTable(
                name: "tbcf_contas");

            migrationBuilder.DropTable(
                name: "tbcf_agencias");
        }
    }
}
