using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Data.Migrations
{
    public partial class IsRequiredoperatorticketfalse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_SupportOperators_SupportOperatorId",
                table: "SupportTickets");

            migrationBuilder.AlterColumn<int>(
                name: "SupportOperatorId",
                table: "SupportTickets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_SupportOperators_SupportOperatorId",
                table: "SupportTickets",
                column: "SupportOperatorId",
                principalTable: "SupportOperators",
                principalColumn: "SupportOperatorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_SupportOperators_SupportOperatorId",
                table: "SupportTickets");

            migrationBuilder.AlterColumn<int>(
                name: "SupportOperatorId",
                table: "SupportTickets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_SupportOperators_SupportOperatorId",
                table: "SupportTickets",
                column: "SupportOperatorId",
                principalTable: "SupportOperators",
                principalColumn: "SupportOperatorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
