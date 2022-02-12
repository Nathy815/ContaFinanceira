using Microsoft.EntityFrameworkCore.Migrations;

namespace ContaFinanceira.Persistance.Migrations
{
    public partial class DistincaoPFePJ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpfCnpj",
                table: "tbcf_clientes",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoPessoa",
                table: "tbcf_clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "tbcf_clientes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CpfCnpj",
                value: "51865798916");

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$suS.0JwXHYjqT7ZIIAAMeuwnaxiZko4kfMOH8.CDOxQ9V0covPWdq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpfCnpj",
                table: "tbcf_clientes");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "tbcf_clientes");

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$VWUh7TyMRjU/Xc/RyPHCHOpsO8gBQbzgT/aq9UATypmThq8t2i4iS");
        }
    }
}
