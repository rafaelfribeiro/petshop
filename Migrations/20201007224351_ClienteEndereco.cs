using Microsoft.EntityFrameworkCore.Migrations;

namespace Petshop.Migrations
{
    public partial class ClienteEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Especies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AlterColumn<int>(
                name: "PorteId",
                table: "Especies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Clientes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Especies_PorteId",
                table: "Especies",
                column: "PorteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Especies_Portes_PorteId",
                table: "Especies",
                column: "PorteId",
                principalTable: "Portes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Especies_Portes_PorteId",
                table: "Especies");

            migrationBuilder.DropIndex(
                name: "IX_Especies_PorteId",
                table: "Especies");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Clientes");

            migrationBuilder.AlterColumn<int>(
                name: "PorteId",
                table: "Especies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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
        }
    }
}
