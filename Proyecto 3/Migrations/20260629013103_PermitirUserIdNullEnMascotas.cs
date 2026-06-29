using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_3.Migrations
{
    /// <inheritdoc />
    public partial class PermitirUserIdNullEnMascotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mascotas_users_UserId",
                table: "mascotas");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "mascotas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_mascotas_users_UserId",
                table: "mascotas",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mascotas_users_UserId",
                table: "mascotas");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "mascotas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_mascotas_users_UserId",
                table: "mascotas",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
