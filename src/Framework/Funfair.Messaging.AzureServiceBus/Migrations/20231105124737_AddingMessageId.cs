using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Funfair.Messaging.AzureServiceBus.Migrations
{
    /// <inheritdoc />
    public partial class AddingMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Outboxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MessageId",
                table: "Inboxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Outboxes");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Inboxes");
        }
    }
}
