using Microsoft.EntityFrameworkCore.Migrations;

namespace ContaFinanceira.Persistance.Migrations
{
    public partial class Add_Cliente_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tbcf_clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "tbcf_clientes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "nathalialcoimbra@gmail.com");

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$6Z.fPfL4wrADq24QUy9lDOixtP8iZpDod1WormbJf9lutCUTrEdXC");

            migrationBuilder.CreateIndex(
                name: "IX_tbcf_clientes_Email",
                table: "tbcf_clientes",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbcf_clientes_Email",
                table: "tbcf_clientes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "tbcf_clientes");

            migrationBuilder.UpdateData(
                table: "tbcf_contas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Senha",
                value: "$2a$11$4/WPXcRIcujksmWmnV6bBehoyugcezsR/wQ3Gq1zOKSi0WYuI8svm");
        }
    }
}
