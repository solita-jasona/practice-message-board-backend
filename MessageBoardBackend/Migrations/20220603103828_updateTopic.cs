using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardBackend.Migrations
{
    public partial class updateTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic");

            migrationBuilder.AlterColumn<int>(
                name: "LastMessageId",
                table: "Topic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Topic",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_UserId",
                table: "Topic",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic",
                column: "LastMessageId",
                principalTable: "Message",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_User_UserId",
                table: "Topic",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic");

            migrationBuilder.DropForeignKey(
                name: "FK_Topic_User_UserId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_UserId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Topic");

            migrationBuilder.AlterColumn<int>(
                name: "LastMessageId",
                table: "Topic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic",
                column: "LastMessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
