using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessageBoardBackend.Migrations
{
    public partial class UpdateMessageTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_LastMessageId",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Topic");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageTimeStamp",
                table: "Topic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "Message",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Message_TopicId",
                table: "Message",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Topic_TopicId",
                table: "Message",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Topic_TopicId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_TopicId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "LastMessageTimeStamp",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "Message");

            migrationBuilder.AddColumn<int>(
                name: "LastMessageId",
                table: "Topic",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_LastMessageId",
                table: "Topic",
                column: "LastMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Message_LastMessageId",
                table: "Topic",
                column: "LastMessageId",
                principalTable: "Message",
                principalColumn: "Id");
        }
    }
}
