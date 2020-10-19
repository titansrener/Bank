using Microsoft.EntityFrameworkCore.Migrations;

namespace BANCO.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "TB_CONTAS",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NUMERO = table.Column<int>(type: "int", nullable: false),
                    SALDO = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CPF_CLIENTE = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true),
                    NOME_CLIENTE = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true),
                    RENDA_MENSAL_CLIENTE = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NOME_BANCO = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTA", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONTAS",
                schema: "dbo");
        }
    }
}
