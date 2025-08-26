using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock1.Data.Migrations
{
    /// <inheritdoc />
    public partial class Segunda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stocks_compradores_compradorId",
                table: "stocks");

            migrationBuilder.AlterColumn<int>(
                name: "compradorId",
                table: "stocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_stocks_compradores_compradorId",
                table: "stocks",
                column: "compradorId",
                principalTable: "compradores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stocks_compradores_compradorId",
                table: "stocks");

            migrationBuilder.AlterColumn<int>(
                name: "compradorId",
                table: "stocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_stocks_compradores_compradorId",
                table: "stocks",
                column: "compradorId",
                principalTable: "compradores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
