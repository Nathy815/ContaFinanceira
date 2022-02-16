using Microsoft.EntityFrameworkCore.Migrations;

namespace ContaFinanceira.Persistance.Migrations
{
    public partial class Adiciona_NotificacaoEnviada_Transacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotificacaoEnviada",
                table: "tbcf_transacoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$jk.wJ7hPVnyrCf5BYRJ5PekeQscvKKfNkvi15nh4/zgp4ekmj19C.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificacaoEnviada",
                table: "tbcf_transacoes");

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$6Z.fPfL4wrADq24QUy9lDOixtP8iZpDod1WormbJf9lutCUTrEdXC");
        }
    }
}
